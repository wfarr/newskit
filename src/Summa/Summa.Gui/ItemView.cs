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

namespace Summa.Gui {
    public class ItemView : Gtk.TreeView {
        public Gtk.ListStore store;
        private Gtk.IconTheme icon_theme;
        private Gdk.Pixbuf icon;
        
        private Summa.Interfaces.ISource feedobj;
        private ArrayList items;
        private Hashtable itemhash;
        
        private Gtk.TreeIter iter;
        
        private Gtk.TreeViewColumn column_Date;
        private Gtk.TreeViewColumn column_Title;
        
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
                Gtk.TreeModel selectmodel;
                if ( Selection.CountSelectedRows() != 0 ) {
                    Selection.GetSelected(out selectmodel, out iter);
                } else { store.GetIterFirst(out iter); }
                return ItemFromIter(iter);
            }
        }
        
        public ItemView() {
            // set up the liststore for the view
            store = new Gtk.ListStore(typeof(Gdk.Pixbuf), typeof(bool), typeof(bool), typeof(string), typeof(string), typeof(string), typeof(int));
            Model = store;
            
            // set up the columns for the view
            InsertColumn(-1, "Read", new Gtk.CellRendererPixbuf(), "pixbuf", 0);
            CellRendererText trender = new Gtk.CellRendererText();
            
            column_Date = new Gtk.TreeViewColumn("Date", trender, "text", 3);
            column_Date.AddAttribute(trender, "weight", 6);
            column_Date.SortColumnId = 3;
            column_Date.SortIndicator = true;
            AppendColumn(column_Date);
            column_Title = new Gtk.TreeViewColumn("Title", trender, "text", 4);
            column_Title.AddAttribute(trender, "weight", 6);
            AppendColumn(column_Title);
            
            RulesHint = true;
            HeadersClickable = true;
            store.SetSortColumnId(3, Gtk.SortType.Descending);
            
            // set up the icon theme so that we can make stuff pretty 
            icon_theme = Gtk.IconTheme.Default;
        }
        
        private void PopulateItem(Summa.Data.Item item) {
            TreeIter iter;
            try {
                iter = (TreeIter)itemhash[item.Uri];
            } catch ( NullReferenceException e ) {
                Summa.Core.Log.Exception(e);
                iter = store.Append();
                itemhash.Add(item.Uri, iter);
            }
            
            if (!item.Read) {
                icon = icon_theme.LookupIcon("feed-item", 16, Gtk.IconLookupFlags.NoSvg).LoadIcon();
                store.SetValue(iter, 6, (int)Pango.Weight.Bold);
            } else if (item.Flagged) {
                icon = icon_theme.LookupIcon("emblem-important", 16, Gtk.IconLookupFlags.NoSvg).LoadIcon();
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
                Summa.Core.Log.Exception(e);
            }
        }
        
        public void Populate(Summa.Interfaces.ISource feed) {
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
            
            Summa.Core.Application.Database.FeedDeleted += OnFeedDeleted;
            //Summa.Core.Application.Database.ItemAdded += OnItemAdded;
            Summa.Core.Application.Database.ItemDeleted += OnItemDeleted;
            Summa.Core.Application.Database.ItemChanged += OnItemChanged;
        }
        
        private void OnFeedDeleted(object obj, Summa.Core.AddedEventArgs args) {
            if ( feedobj.Url == args.Uri ) {
                store.Clear();
            }
        }
        
        private void OnItemAdded(object obj, Summa.Core.AddedEventArgs args) {
            if ( feedobj.Url == args.FeedUri ) {
                PopulateItem(new Summa.Data.Item(args.Uri, args.FeedUri));
            }
        }
        
        private void OnItemDeleted(object obj, Summa.Core.AddedEventArgs args) {
            if ( feedobj.Url == args.FeedUri ) {
                DeleteItem(new Summa.Data.Item(args.Uri, args.FeedUri));
            }
        }
        
        private void OnItemChanged(object obj, Summa.Core.ChangedEventArgs args ) {
            if ( args.FeedUri == feedobj.Url ) {
                Summa.Data.Item item = new Summa.Data.Item(args.Uri, args.FeedUri);
                PopulateItem(item);
            }
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
                Summa.Core.Log.Exception(e, "No feed selected");
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
                Gtk.TreeModel selectmodel;
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
                Gtk.TreeModel selectmodel;
                Selection.GetSelected(out selectmodel, out iter);
            } else { store.GetIterFirst(out iter); }
            
            bool has_unread = false;
            
            while ( !has_unread ) {
                GLib.Value readvalue = new GLib.Value(false);
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
                GLib.Value readvalue = new GLib.Value(false);
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
        
        private Summa.Data.Item ItemFromIter(Gtk.TreeIter treeiter) {
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
                item = Summa.Data.Core.GetItem(val);
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
                Summa.Core.Log.Exception(e);
                return "";
            }
        }
    }
}
