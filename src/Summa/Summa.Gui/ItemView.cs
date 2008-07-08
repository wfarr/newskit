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
        
        private Summa.Data.Feed feedobj;
        private ArrayList items;
        private Hashtable itemhash;
        
        private Gtk.TreeModel selectmodel;
        private Gtk.TreeIter iter;
        
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
            
            TreeViewColumn column_Date = new Gtk.TreeViewColumn("Date", trender, "text", 3);
            column_Date.AddAttribute(trender, "weight", 6);
            column_Date.SortColumnId = 3;
            column_Date.SortIndicator = true;
            AppendColumn(column_Date);
            TreeViewColumn column_Title = new Gtk.TreeViewColumn("Title", trender, "text", 4);
            column_Title.AddAttribute(trender, "weight", 6);
            AppendColumn(column_Title);
            
            RulesHint = true;
            HeadersClickable = true;
            store.SetSortColumnId(3, Gtk.SortType.Descending);
            
            // set up the icon theme so that we can make stuff pretty 
            icon_theme = Gtk.IconTheme.Default;
        }
        
        private void AppendItem(Gtk.TreeIter titer, Summa.Data.Item titem) {
            bool read = titem.Read;
            bool flagged = titem.Flagged;
            string date = titem.Date;
            string title = titem.Title;
            string uri = titem.Uri;
            
            if ( !read ) {
                icon = icon_theme.LookupIcon("feed-item", 16, Gtk.IconLookupFlags.NoSvg).LoadIcon();
                store.SetValue(titer, 6, (int)Pango.Weight.Bold);
            } else if ( flagged ) {
                icon = new Gdk.Pixbuf("/Users/wfarr/Desktop/summa-flagged.png");
            } else {
                icon = null;
                store.SetValue(titer, 6, (int)Pango.Weight.Normal);
            }
            
            try {
                itemhash.Add(uri, store.GetPath(titer));
            } catch ( System.ArgumentException e ) {}
            
            if ( icon != null ) {
                store.SetValue(titer, 0, icon);
            } else {
                store.SetValue(titer, 0, "");
            }
            store.SetValue(titer, 1, read);
            store.SetValue(titer, 2, flagged);
            store.SetValue(titer, 3, MakePrettyDate(date));
            store.SetValue(titer, 4, title);
            store.SetValue(titer, 5, uri);
        }
        
        private void DeleteItem(Summa.Data.Item item) {
            TreePath path = (TreePath)itemhash[item.Uri];
            TreeIter iter;
            store.GetIter(out iter, path);
            store.Remove(ref iter);
        }
        
        public void Populate(Summa.Data.Feed feed) {
            feedobj = feed;
            items = feed.Items;
            items.Reverse();
            
            store.Clear();
            
            itemhash = new Hashtable();
            
            foreach ( Summa.Data.Item item in items ) {
                if ( feed.Url == feedobj.Url ) {
                    TreeIter iter = store.Append();
                    
                    AppendItem(iter, item);
                    
                    while ( Gtk.Application.EventsPending() ) {
                        Gtk.Main.Iteration();
                    }
                } else {
                    return;
                }
            }
            
            Summa.Core.Application.Database.FeedDeleted += OnFeedDeleted;
            Summa.Core.Application.Database.ItemAdded += OnItemAdded;
            Summa.Core.Application.Database.ItemDeleted += OnItemDeleted;
        }
        
        private void OnFeedDeleted(object obj, Summa.Core.AddedEventArgs args) {
            if ( feedobj.Url == args.Uri ) {
                store.Clear();
            }
        }
        
        private void OnItemAdded(object obj, Summa.Core.AddedEventArgs args) {
            if ( feedobj.Url == args.FeedUri ) {
                AppendItem(store.Append(), new Summa.Data.Item(args.Uri, args.FeedUri));
            }
        }
        
        private void OnItemDeleted(object obj, Summa.Core.AddedEventArgs args) {
            if ( feedobj.Url == args.FeedUri ) {
                DeleteItem(new Summa.Data.Item(args.Uri, args.FeedUri));
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
                        TreeIter iter = store.Append();
                        
                        AppendItem(iter, item);
                    }
                }
                
                foreach ( Summa.Data.Item item in items ) {
                    if ( !uitemurls.Contains(item.Uri) ) {
                        DeleteItem(item);
                    }
                }
                
                items = feedobj.Items;
            } catch ( Exception e ) {
                Summa.Core.Log.LogException(e, "No feed selected");
            }
        }
        
        public void MarkSelectedRead() {
            Console.WriteLine("MarkSelectedRead : "+DateTime.Now);
            Selected.Read = true;
            
            AppendItem(iter, Selected);
        }
        
        public void MarkSelectedFlagged() {
            if ( Selected.Flagged ) {
                Selected.Flagged = false;
            } else {
                Selected.Flagged = true;
            }
            
            AppendItem(iter, Selected);
        }
        
        public void GoToPreviousItem() {
            if ( Selection.CountSelectedRows() != 0 ) {
                Selection.GetSelected(out selectmodel, out iter);
            } else {
                store.GetIterFirst(out iter);
                Selection.SelectIter(iter);
            }
            if ( store.IterIsValid(iter) ) {
                TreePath path = store.GetPath(iter);
                
                path.Prev();
                
                Selection.SelectPath(path);
            }
        }
        
        public bool GoToNextItem() {
            if ( Selection.CountSelectedRows() != 0 ) {
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
                    AppendItem(iter, item);
                }
            }
        }
        
        private Summa.Data.Item ItemFromIter(Gtk.TreeIter treeiter) {
            string val = (string)store.GetValue(iter, 5);
            Summa.Data.Item item = new Summa.Data.Item(val, feedobj.Url);
            return item;
        }
        
        public string MakePrettyDate(string date) {
            DateTime dtdate = Convert.ToDateTime(date);
            
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
        }
    }
}
