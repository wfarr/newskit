// ItemView.cs
//
// Copyright (c) 2008 Ethan Osten
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.

using System;
using Gtk;
using Gdk;
using System.Collections;
using System.Linq;

using Summa.Core;
using Summa.Data;
using Summa.Gui;

namespace Summa.Gui {
    public class ItemView : TreeView {
        public ListStore store;
        private IconTheme icon_theme;
        private Gdk.Pixbuf icon;
        
        private ISource feedobj;
        private ArrayList items;
        private Hashtable itemhash;
        
        private TreeIter iter;
        
        private TreeViewColumn column_Date;
        private TreeViewColumn column_Title;
        
        public bool HasSelected {
            get {
                if ( Selection.CountSelectedRows() != 0 ) {
                    return true;
                } else {
                    return false;
                }
            }
        }
        
        public Summa.Data.Item Selected {
            get {
                TreeModel selectmodel;
                if ( Selection.CountSelectedRows() != 0 ) {
                    Selection.GetSelected(out selectmodel, out iter);
                } else { store.GetIterFirst(out iter); }
                return ItemFromIter(iter);
            }
        }
        
        public ItemView() {
            // set up the liststore for the view
            store = new ListStore(typeof(Gdk.Pixbuf), typeof(bool), typeof(bool), typeof(string), typeof(string), typeof(string), typeof(int));
            Model = store;
            
            // set up the columns for the view
            InsertColumn(-1, "Read", new CellRendererPixbuf(), "pixbuf", 0);
            CellRendererText trender = new CellRendererText();
            
            column_Date = new TreeViewColumn("Date", trender, "text", 3);
            column_Date.AddAttribute(trender, "weight", 6);
            column_Date.SortColumnId = 3;
            column_Date.SortIndicator = true;
            AppendColumn(column_Date);
            column_Title = new TreeViewColumn("Title", trender, "text", 4);
            column_Title.AddAttribute(trender, "weight", 6);
            AppendColumn(column_Title);
            
            RulesHint = true;
            HeadersClickable = true;
            store.SetSortColumnId(3, SortType.Descending);
            
            // set up the icon theme so that we can make stuff pretty 
            icon_theme = IconTheme.Default;
        }
        
        private void PopulateItem(Summa.Data.Item item) {
            TreeIter iter;
            try {
                iter = (TreeIter)itemhash[item.Uri];
            } catch ( NullReferenceException e ) {
                Log.Exception(e);
                iter = store.Append();
                itemhash.Add(item.Uri, iter);
            }
            
            if (!item.Read) {
                icon = icon_theme.LookupIcon("feed-item", 16, IconLookupFlags.NoSvg).LoadIcon();
                store.SetValue(iter, 6, (int)Pango.Weight.Bold);
            } else if (item.Flagged) {
                icon = icon_theme.LookupIcon("emblem-important", 16, IconLookupFlags.NoSvg).LoadIcon();
                store.SetValue(iter, 6, (int)Pango.Weight.Normal);
            } else {
                icon = new Gdk.Pixbuf(Gdk.Colorspace.Rgb, false, 0, 0, 0);
                store.SetValue(iter, 6, (int)Pango.Weight.Normal);
            }
            
            store.SetValue(iter, 0, icon);
            store.SetValue(iter, 1, item.Read);
            store.SetValue(iter, 2, item.Flagged);
            store.SetValue(iter, 3, MakePrettyDate(item.Date));
            store.SetValue(iter, 4, item.Title);
            store.SetValue(iter, 5, item.Uri);
        }
        
        private void DeleteItem(Summa.Data.Item item) {
            try {
                TreeIter iter = (TreeIter)itemhash[item.Uri];
                store.Remove(ref iter);
            } catch ( Exception e ) {
                Log.Exception(e);
            }
        }
        
        public void Populate(ISource feed) {
            feedobj = feed;
            items = feed.Items;
            items.Reverse();
            
            store.Clear();
            
            itemhash = new Hashtable();
            
            foreach ( Summa.Data.Item item in items ) {
                if ( feed.Url == feedobj.Url ) {
                    PopulateItem(item);
                } else {
                    return;
                }
            }
            
            Database.FeedDeleted += OnFeedDeleted;
            Database.ItemAdded += OnItemAdded;
            Database.ItemDeleted += OnItemDeleted;
            Database.ItemChanged += OnItemChanged;
        }
        
        private void OnFeedDeleted(object obj, AddedEventArgs args) {
            if ( feedobj.Url == args.Uri ) {
                Gtk.Application.Invoke(this, new EventArgs(), new EventHandler(InvokeStoreClear));
            }
        }
        
        private void InvokeStoreClear(object obj, EventArgs args) {
            store.Clear();
        }
        
        private void OnItemAdded(object obj, AddedEventArgs args) {
            if ( feedobj.Url == args.FeedUri ) {
                Gtk.Application.Invoke(this, args, new EventHandler(InvokeAddItem));
            }
        }
        
        private void InvokeAddItem(object obj, EventArgs args) {
            AddedEventArgs aargs = (AddedEventArgs)args;
            PopulateItem(new Summa.Data.Item(aargs.Uri, aargs.FeedUri));
        }
        
        private void OnItemDeleted(object obj, AddedEventArgs args) {
            if ( feedobj.Url == args.FeedUri ) {
                Gtk.Application.Invoke(this, args, new EventHandler(InvokeDeleteItem));
            }
        }
        
        private void InvokeDeleteItem(object obj, EventArgs args) {
            AddedEventArgs aargs = (AddedEventArgs)args;
            DeleteItem(new Summa.Data.Item(aargs.Uri, aargs.FeedUri));
        }
            
        
        private void OnItemChanged(object obj, ChangedEventArgs args ) {
            if ( args.FeedUri == feedobj.Url ) {
                Gtk.Application.Invoke(this, args, new EventHandler(InvokeChangeItem));
            }
        }
        
        private void InvokeChangeItem(object obj, EventArgs args) {
            ChangedEventArgs aargs = (ChangedEventArgs)args;
            PopulateItem(new Summa.Data.Item(aargs.Uri, aargs.FeedUri));
        }
        
        public void Update() {
            try {
                ArrayList uitems = feedobj.Items;
                ArrayList itemurls = new ArrayList();
                ArrayList uitemurls = new ArrayList();
                
                foreach (Summa.Data.Item item in items) {
                    itemurls.Add(item.Uri);
                }
                
                foreach (Summa.Data.Item item in uitems) {
                    uitemurls.Add(item.Uri);
                }
                
                foreach ( Summa.Data.Item item in uitems ) {
                    if ( !itemurls.Contains(item.Uri) ) {
                        PopulateItem(item);
                    }
                }
                
                foreach ( Summa.Data.Item item in items ) {
                    if ( !uitemurls.Contains(item.Uri) ) {
                        DeleteItem(item);
                    }
                }
                
                items = feedobj.Items;
            } catch ( Exception e ) {
                Log.Exception(e, "No feed selected");
            }
        }
        
        public void MarkSelectedFlagged() {
            if ( Selected.Flagged ) {
                Selected.Flagged = false;
            } else {
                Selected.Flagged = true;
            }
            
            PopulateItem(Selected);
        }
        
        public void GoToPreviousItem() {
            if ( Selection.CountSelectedRows() != 0 ) {
                TreeModel selectmodel;
                Selection.GetSelected(out selectmodel, out iter);
            } else {
                store.GetIterFirst(out iter);
                Selection.SelectIter(iter);
            }
            if ( store.IterIsValid(iter) ) {
                TreePath path = store.GetPath(iter);
                
                path.Prev();
                
                Selection.SelectPath(path);
                SetCursor(path, column_Title, false);
            }
        }
        
        public bool GoToNextItem() {
            if ( Selection.CountSelectedRows() != 0 ) {
                TreeModel selectmodel;
                Selection.GetSelected(out selectmodel, out iter);
            } else { store.GetIterFirst(out iter); }
            
            bool has_unread = false;
            
            while ( !has_unread ) {
                GLib.Value readvalue = GLib.Value.Empty;
                if ( !store.IterIsValid(iter) ) {
                    break;
                }
                store.GetValue(iter, 1, ref readvalue);
                if ( (bool)readvalue.Val ) {
                    store.IterNext( ref iter );
                } else {
                    has_unread = true;
                    Selection.SelectIter(iter);
                    ScrollToCell(store.GetPath(iter), null, false, 0, 0);
                    break;
                }
            }
            SetCursor(store.GetPath(iter), column_Title, false);
            return has_unread;
        }
        
        public void MarkItemsRead() {
            store.GetIterFirst(out iter);
            
            while ( true ) {
                GLib.Value readvalue = GLib.Value.Empty;
                if (!store.IterIsValid(iter)) {
                    break;
                }
                store.GetValue(iter, 1, ref readvalue);
                
                if ((bool)readvalue.Val) {
                    store.IterNext(ref iter);
                } else {
                    Summa.Data.Item item = ItemFromIter(iter);
                    item.Read = true;
                    PopulateItem(item);
                }
            }
        }
        
        private Summa.Data.Item ItemFromIter(TreeIter treeiter) {
            string val = (string)store.GetValue(iter, 5);
            bool fail = false;
            Summa.Data.Item item = null;
            
            try {
                item = new Summa.Data.Item(val, feedobj.Url);
                if ( item.Title == null ) {
                    fail = true;
                }
            } catch ( Exception ) {
                fail = true;
            }
            
            if ( fail ) {
                item = Feeds.GetItem(val);
            }
            
            return item;
        }
        
        public string MakePrettyDate(DateTime dtdate) {
            try {
                string year = "";
                string month = "";
                string day = "";
                string hour = "";
                string minute = "";
                string second = "";
                
                year = dtdate.Year.ToString();
                
                if ( dtdate.Month.ToString().Length == 1 ) {
                    month = "0"+dtdate.Month.ToString();
                } else {
                    month = dtdate.Month.ToString();
                }
                
                if ( dtdate.Day.ToString().Length == 1 ) {
                    day = "0"+dtdate.Day.ToString();
                } else {
                    day = dtdate.Day.ToString();
                }
                
                if ( dtdate.Hour.ToString().Length == 1 ) {
                    hour = "0"+dtdate.Hour.ToString();
                } else {
                    hour = dtdate.Hour.ToString();
                }
                
                if ( dtdate.Minute.ToString().Length == 1 ) {
                    minute = "0"+dtdate.Minute.ToString();
                } else {
                    minute = dtdate.Minute.ToString();
                }
                
                if ( dtdate.Second.ToString().Length == 1 ) {
                    second = "0"+dtdate.Second.ToString();
                } else {
                    second = dtdate.Second.ToString();
                }
                
                return year+"/"+month+"/"+day+" at "+hour+":"+minute+":"+second;
            } catch ( Exception e ) {
                Log.Exception(e);
                return "";
            }
        }
    }
}
