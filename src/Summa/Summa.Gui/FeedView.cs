// FeedView.cs
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
using System.Collections;
using Gdk;
using System.Linq;
using Pango;

namespace Summa.Gui {
    public class FeedView : Gtk.TreeView{
        public Gtk.ListStore store;
        private Gtk.IconTheme icon_theme;
        public string SetTag;
        
        private ArrayList feeds;
        
        public Gtk.TreeIter iter;
        public Gtk.TreeModel selectmodel;
        GLib.Value retval;
        
        // a NewsKitFeed representing the feed selected
        // note that if no feed is selected, trying to get this will cause you
        // some problems.
        public bool HasSelected {
            get {
                if ( Selection.CountSelectedRows() != 0 ) {
                    return true;
                } else {
                    return false;
                }
            }
        }
        
        public Summa.Data.Feed Selected {
            get {
                if ( Selection.CountSelectedRows() != 0 ) {
                    Selection.GetSelected(out selectmodel, out iter);
                } else { store.GetIterFirst(out iter); }
                
                string val = (string)store.GetValue(iter, 2);
                
                return Summa.Data.Core.RegisterFeed(val);
            }
        }
        
        public bool FeedSort {
            get { return Summa.Core.Config.SortFeedview; }
            set { 
                if ( value ) {
                    store.SetSortColumnId(3, Gtk.SortType.Descending);
                }
                Summa.Core.Config.SortFeedview = value;
            }
        }
        
        private Gtk.CellRendererText trender;
        private Hashtable feedhash;
        
        public FeedView() {
            // set up the liststore for the view
            store = new Gtk.ListStore(typeof(Gdk.Pixbuf),    // the icon
                                        typeof(string),        // the name
                                        typeof(string),        // the url
                                        typeof(bool),          // unread?
                                        typeof(int));          // font weight
            Model = store;
            trender = new Gtk.CellRendererText();
            trender.Ellipsize = Pango.EllipsizeMode.End;
            
            // set up the columns for the view
            TreeViewColumn column_Read = new Gtk.TreeViewColumn("Read", new Gtk.CellRendererPixbuf(), "pixbuf", 0);
            column_Read.SortColumnId = 3;
            column_Read.SortIndicator = false;
            AppendColumn(column_Read);
            
            TreeViewColumn column_Name = new Gtk.TreeViewColumn("Title", trender, "text", 1);
            column_Name.AddAttribute(trender, "weight", 4);
            column_Name.SortColumnId = 1;
            column_Name.SortIndicator = true;
            AppendColumn(column_Name);
            
            HeadersVisible = false;
            
            feedhash = new Hashtable();
            feeds = new ArrayList();

            // set up the icon theme so that we can make stuff pretty
            icon_theme = Gtk.IconTheme.Default;
            
            Summa.Core.Application.Database.FeedAdded += OnFeedAdded;
            Summa.Core.Application.Database.FeedDeleted += OnFeedDeleted;
            Summa.Core.Application.Database.FeedChanged += OnFeedChanged;
            Summa.Core.Application.Database.ItemChanged += OnItemChanged;
            Summa.Core.Application.Database.ItemAdded += OnItemAdded;
            Summa.Core.Application.Database.ItemDeleted += OnItemDeleted;
        }
        
        private void OnFeedAdded (object obj, Summa.Core.AddedEventArgs args) {
            Summa.Data.Feed feed = new Summa.Data.Feed(args.Uri);
            
            if ( feed.Tags.Contains(SetTag) ) {
                AddNewFeed(new Summa.Data.Feed(args.Uri));
            }
        }
        
        private void OnFeedDeleted(object obj, Summa.Core.AddedEventArgs args) {
            DeleteFeed(new Summa.Data.Feed(args.Uri));
        }
        
        private void OnFeedChanged(object obj, Summa.Core.ChangedEventArgs args) {
            try {
                UpdateFeed(new Summa.Data.Feed(args.Uri));
            } catch ( Exception e ) {
                Summa.Core.Log.LogException(e);
            }
            
            if ( args.ItemProperty == "tags" ) {
                Console.WriteLine(1);
                if ( args.Value.Split(',').Contains(SetTag) ) {
                    AddNewFeed(new Summa.Data.Feed(args.Uri));
                }
            }
        }
        
        private void OnItemAdded(object obj, Summa.Core.AddedEventArgs args) {
            try {
                UpdateFeed(new Summa.Data.Feed(args.FeedUri));
            } catch ( Exception e ) {
                Summa.Core.Log.LogException(e);
            }
        }
        
        private void OnItemDeleted(object obj, Summa.Core.AddedEventArgs args) {
            try {
                UpdateFeed(new Summa.Data.Feed(args.FeedUri));
            } catch ( Exception e ) {
                Summa.Core.Log.LogException(e);
            }
        }
        
        private void OnItemChanged(object obj, Summa.Core.ChangedEventArgs args) {
            try {
                UpdateFeed(new Summa.Data.Feed(args.FeedUri));
            } catch ( Exception e ) {
                Summa.Core.Log.LogException(e);
            }
        }
        
        public void Populate(string tag) {
            switch(tag) {
                case "All feeds":
                    PopulateWithFeeds("All");
                    break;
                /*case "Searches":
                    PopulateWithSearches();
                    break;*/
                default:
                    PopulateWithFeeds(tag);
                    break;
            }
        }
        
        public void Update() {
            ArrayList ufeeds = Summa.Data.Core.GetFeeds(SetTag);
            ArrayList haveurls = new ArrayList();
            
            foreach ( Summa.Data.Feed feed in feeds ) {
                haveurls.Add(feed.Url);
            }
            
            foreach (Summa.Data.Feed feed in ufeeds) {
                if ( !haveurls.Contains(feed.Url) ) {
                    Gtk.TreeIter iter;
                    iter = store.Append();
                    
                    AppendFeed(feed, iter);
                }
            }
            feeds = Summa.Data.Core.GetFeeds(SetTag);
        }
        
        public void UpdateSelected() {
            AppendFeed(Selected, iter);
        }
        
        public void UpdateFeed(Summa.Data.Feed feed) {
            TreePath path = (TreePath)feedhash[feed.Url];
            TreeIter iter;
            store.GetIter(out iter, path);
            AppendFeed(feed, iter);
        }
        
        /*public void UpdateSearch(Summa.Data.Search search) {
            TreePath path = (TreePath)feedhash[search.Name];
            TreeIter iter;
            store.GetIter(out iter, path);
            AppendSearch(search, iter);
        }*/
        
        public void DeleteFeed(Summa.Data.Feed feed) {
            try {
                TreePath path = (TreePath)feedhash[feed.Url];
                TreeIter iter;
                store.GetIter(out iter, path);
                store.Remove(ref iter);
            } catch ( Exception e ) {
                Summa.Core.Log.LogException(e);
            }
        }
        
        public void AddNewFeed(Summa.Data.Feed feed) {
            Gtk.TreeIter iter;
            iter = store.Append();
            
            AppendFeed(feed, iter);
        }
        
        public void AppendFeed(Summa.Data.Feed feed, Gtk.TreeIter titer) {
            //int count = feed.GetUnreadCount(); // optimize this!
            bool unread = feed.HasUnread;
            
            Gdk.Pixbuf icon;
            if ( unread ) {
                icon = new Gdk.Pixbuf("/usr/share/pixmaps/summa-unread.png");
            } else {
                icon = new Gdk.Pixbuf("/usr/share/pixmaps/summa-inactive.png");
            }
            
            string feedname = feed.Name;
            string feedurl = feed.Url;
            
            store.SetValue(titer, 0, icon);
            store.SetValue(titer, 1, feedname);
            store.SetValue(titer, 2, feedurl);
            store.SetValue(titer, 3, unread);
            if ( unread ) {
                store.SetValue(titer, 4, (int)Pango.Weight.Bold);
            } else {
                store.SetValue(titer, 4, (int)Pango.Weight.Normal);
            }
            
            try {
                feedhash.Add(feedurl, store.GetPath(titer));
            } catch ( System.ArgumentException e ) {}
            
            while ( Gtk.Application.EventsPending() ) {
                Gtk.Main.Iteration();
            }
        }
        
        public void SetAsUpdating(Summa.Data.Feed feed) {
            TreePath path = (TreePath)feedhash[feed.Url];
            TreeIter iter;
            store.GetIter(out iter, path);
            store.SetValue(iter, 0, icon_theme.LookupIcon("reload", (int)Gtk.IconSize.Menu, Gtk.IconLookupFlags.NoSvg).LoadIcon());
        }
        
        public void SetAsNotUpdating(Summa.Data.Feed feed) {
            TreePath path = (TreePath)feedhash[feed.Url];
            TreeIter iter;
            store.GetIter(out iter, path);
            AppendFeed(feed, iter);
        }
        
        /*public void AppendSearch(Summa.Data.Search search, Gtk.TreeIter titer) {
            int count = search.GetUnreadCount(); // optimize this!
            bool unread = false;
            
            Gdk.Pixbuf icon;
            if ( count > 0 ) {
                /*icon = new Gdk.Pixbuf("/usr/share/pixmaps/summa-unread.png");
                unread = true;
            } else {
                /*icon = new Gdk.Pixbuf("/usr/share/pixmaps/summa-inactive.png");
                unread = false;
            }
            
            string feedname = search.Name;
            
            /*store.SetValue(titer, 0, icon);
            store.SetValue(titer, 1, feedname);
            store.SetValue(titer, 3, unread);
            if ( unread ) {
                store.SetValue(titer, 4, (int)Pango.Weight.Bold);
            } else {
                store.SetValue(titer, 4, (int)Pango.Weight.Normal);
            }
            
            try {
                feedhash.Add(feedname, store.GetPath(titer));
            } catch ( System.ArgumentException e ) {}
            
            while ( Gtk.Application.EventsPending() ) {
                Gtk.Main.Iteration();
        }*/
        
        /*private void PopulateWithSearches() {
            store.clear();
            
            GLib.List<Summa.Data.Search> searches = Summa.Data.get_searches();
            
            foreach (Summa.Data.Search search in searches) {
                Gtk.TreeIter iter;
                store.append(out iter);
                
                int count = search.get_unread_count();
                
                string feedname = search.name;
                string feedurl = null;
                
                store.set(iter, 0, icon_theme.lookup_icon("system-search", Gtk.IconSize.MENU, Gtk.IconLookupFlags.NO_SVG).load_icon(), 1, feedname, 2, feedurl, 3, true, -1);
            }
        }*/
        
        /*private void PopulateWithSearches() {
            store.Clear();
            
            Gtk.TreeIter iter;
            iter = store.Append();
            Summa.Data.Search search = new Summa.Data.Search("Unread items");
            search.AddSearchTerm("IS", "read", "False");
            AppendSearch(search, iter);
            
            iter = store.Append();
            Summa.Data.Search search = new Summa.Data.Search("Flagged items");
            search.AddSearchTerm("IS", "flagged", "True");
            AppendSearch(search, iter);
        }*/
        
        private void PopulateWithFeeds(string tag) {            
            store.Clear();
            
            feeds = Summa.Data.Core.GetFeeds(tag);
            
            SetTag = tag;
            
            foreach (Summa.Data.Feed feed in feeds) {
                if ( tag == SetTag ) {
                    Gtk.TreeIter iter;
                    iter = store.Append();
                    
                    AppendFeed(feed, iter);
                } else {
                    return;
                }
            }
        }
        
        public bool GoToNextUnreadFeed() {
            Gtk.TreeModel selectmodel;
            
            if ( Selection.CountSelectedRows() != 0 ) {
                Selection.GetSelected(out selectmodel, out iter);
            } else { store.GetIterFirst(out iter); }
            
            bool has_unread = false;
            
            while ( !has_unread ) {
                GLib.Value readvalue = new GLib.Value(true);
                if ( !store.IterIsValid(iter) ) {
                    break;
                }
                store.GetValue(iter, 3, ref readvalue);
                if ( !(bool)readvalue.Val ) {
                    store.IterNext( ref iter );
                } else {
                    has_unread = true;
                    Selection.SelectIter(iter);
                    ScrollToCell(store.GetPath(iter), null, false, 0, 0);
                    break;
                }
            }
            
            if (!has_unread) {
                store.GetIterFirst(out iter);
                
                while ( !has_unread ) {
                    GLib.Value readvalue = new GLib.Value(false);
                    
                    if ( !store.IterIsValid(iter) ) {
                        break;
                    }
                    store.GetValue(iter, 3, ref readvalue);
                    if ( !(bool)readvalue.Val ) {
                        store.IterNext( ref iter );
                    } else {
                        has_unread = true;
                        Selection.SelectIter(iter);
                        ScrollToCell(store.GetPath(iter), null, false, 0, 0);
                        break;
                    }
                }
            }
            
            return has_unread;
        }
    }
}
