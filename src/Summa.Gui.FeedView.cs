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
using System.Collections;
using System.Linq;

using Gtk;
using Gdk;
using Pango;

using Summa.Core;
using Summa.Data;
using Summa.Gui;

namespace Summa.Gui {
    public class FeedView : TreeView{
        public ListStore store;
        public string SetTag;
        
        private ArrayList feeds;
        
        public TreeIter iter;
        public TreeModel selectmodel;
        
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
        
        public ISource Selected {
            get {
                if ( Selection.CountSelectedRows() != 0 ) {
                    Selection.GetSelected(out selectmodel, out iter);
                } else { store.GetIterFirst(out iter); }
                
                string val = (string)store.GetValue(iter, 2);
                
                if ( val != null ) {
                    return Feeds.RegisterFeed(val);
                } else {
                    if ( (string)store.GetValue(iter, 1) == "Unread items" ) {
                        Search search = new Search("summasearch://read:false");
                        search.Name = "Unread items";
                        search.AddSearchTerm("read", "False");
                        return search;
                    } else if ( (string)store.GetValue(iter, 1) == "Flagged items" ) {
                        Search search = new Search("summasearch://flagged:true");
                        search.Name = "Flagged items";
                        search.AddSearchTerm("flagged", "True");
                        return search;
                    }
                }
                return null;
            }
        }
        
        public bool FeedSort {
            get { return Config.SortFeedview; }
            set { 
                if ( value ) {
                    store.SetSortColumnId(3, SortType.Descending);
                }
                Config.SortFeedview = value;
            }
        }
        
        private CellRendererText trender;
        private Hashtable feedhash;
        
        public FeedView() {
            // set up the liststore for the view
            store = new ListStore(typeof(Gdk.Pixbuf),    // the icon
                                  typeof(string),        // the name
                                  typeof(string),        // the url
                                  typeof(bool),          // unread?
                                  typeof(int));          // font weight
            Model = store;
            trender = new CellRendererText();
            trender.Ellipsize = Pango.EllipsizeMode.End;
            
            // set up the columns for the view
            TreeViewColumn column_Read = new TreeViewColumn("Read", new CellRendererPixbuf(), "pixbuf", 0);
            column_Read.SortColumnId = 3;
            column_Read.SortIndicator = false;
            AppendColumn(column_Read);
            
            TreeViewColumn column_Name = new TreeViewColumn("Title", trender, "text", 1);
            column_Name.AddAttribute(trender, "weight", 4);
            column_Name.SortColumnId = 1;
            column_Name.SortIndicator = true;
            AppendColumn(column_Name);
            
            HeadersVisible = false;
            
            feedhash = new Hashtable();
            feeds = new ArrayList();
            
            Database.FeedAdded += OnFeedAdded;
            Database.FeedDeleted += OnFeedDeleted;
            Database.FeedChanged += OnFeedChanged;
            Database.ItemChanged += OnItemChanged;
            Database.ItemAdded += OnItemAdded;
            Database.ItemDeleted += OnItemDeleted;
        }
        
        private void OnFeedAdded (object obj, AddedEventArgs args) {
            Feed feed = new Feed(args.Uri);
            
            if ( feed.Tags.Contains(SetTag) ) {
                AddNewFeed(new Feed(args.Uri));
            }
            
            ShowAll();
        }
        
        private void OnFeedDeleted(object obj, AddedEventArgs args) {
            DeleteFeed(new Feed(args.Uri));
        }
        
        private void OnFeedChanged(object obj, ChangedEventArgs args) {
            try {
                UpdateFeed(new Feed(args.Uri));
            } catch ( Exception e ) {
                Log.Exception(e);
            }
            
            if ( args.ItemProperty == "tags" ) {
                if ( args.Value.Split(',').Contains(SetTag) ) {
                    try {
                    } catch ( Exception ) {
                        AddNewFeed(new Feed(args.Uri));
                    }
                }
            }
            
            ShowAll();
        }
        
        private void OnItemAdded(object obj, AddedEventArgs args) {
            try {
                UpdateFeed(new Feed(args.FeedUri));
            } catch ( Exception e ) {
                Log.Exception(e);
            }
        }
        
        private void OnItemDeleted(object obj, AddedEventArgs args) {
            try {
                UpdateFeed(new Feed(args.FeedUri));
            } catch ( Exception e ) {
                Log.Exception(e);
            }
        }
        
        private void OnItemChanged(object obj, ChangedEventArgs args) {
            try {
                UpdateFeed(new Feed(args.FeedUri));
            } catch ( Exception e ) {
                Log.Exception(e);
            }
        }
        
        public void Populate(string tag) {
            switch(tag) {
                case "All feeds":
                    PopulateWithFeeds("All");
                    break;
                case "Searches":
                    PopulateWithSearches();
                    break;
                default:
                    PopulateWithFeeds(tag);
                    break;
            }
        }
        
        public void Update() {
            ArrayList ufeeds = Feeds.GetFeeds(SetTag);
            ArrayList haveurls = new ArrayList();
            
            foreach ( ISource feed in feeds ) {
                haveurls.Add(feed.Url);
            }
            
            foreach (ISource feed in ufeeds) {
                if ( !haveurls.Contains(feed.Url) ) {
                    TreeIter iter;
                    iter = store.Append();
                    
                    AppendFeed(feed, iter);
                }
            }
            feeds = Feeds.GetFeeds(SetTag);
        }
        
        public void UpdateSelected() {
            AppendFeed(Selected, iter);
        }
        
        public void UpdateFeed(ISource feed) {
            TreePath path = (TreePath)feedhash[feed.Url];
            TreeIter iter;
            store.GetIter(out iter, path);
            AppendFeed(feed, iter);
            QueueDraw();
        }
        
        public void DeleteFeed(ISource feed) {
            try {
                TreePath path = (TreePath)feedhash[feed.Url];
                TreeIter iter;
                store.GetIter(out iter, path);
                store.Remove(ref iter);
            } catch ( Exception e ) {
                Log.Exception(e);
            }
            QueueDraw();
            QueueResize();
        }
        
        public void AddNewFeed(ISource feed) {
            TreeIter iter = store.Append();
            
            AppendFeed(feed, iter);
            QueueDraw();
            QueueResize();
        }
        
        public void AppendFeed(ISource feed, TreeIter titer) {
            bool unread = feed.HasUnread;
            
            Gdk.Pixbuf icon = feed.Favicon;
            
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
            } catch ( System.ArgumentException e ) {
                Log.Exception(e);
            }
        }
        
        private void PopulateWithSearches() {
            store.Clear();
            
            SetTag = "Searches";
            
            Search search = new Search("summasearch://read:false");
            search.Name = "Unread items";
            search.AddSearchTerm("read", "False");
            AddNewFeed(search);
            
            search = new Search("summasearch://flagged:true");
            search.Name = "Flagged items";
            search.AddSearchTerm("flagged", "True");
            AddNewFeed(search);
        }
        
        private void PopulateWithFeeds(string tag) {            
            store.Clear();
            
            feeds = Feeds.GetFeeds(tag);
            
            SetTag = tag;
            
            foreach (Feed feed in feeds) {
                if ( tag == SetTag ) {
                    TreeIter iter;
                    iter = store.Append();
                    
                    AppendFeed(feed, iter);
                } else {
                    return;
                }
            }
        }
        
        public bool GoToNextUnreadFeed() {
            TreeModel selectmodel;
            if ( Selection.CountSelectedRows() != 0 ) {
                Selection.GetSelected(out selectmodel, out iter);
            } else { store.GetIterFirst(out iter); }
            
            bool has_unread = false;
            
            while ( !has_unread ) {
                GLib.Value readvalue = GLib.Value.Empty;
                
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
                    GLib.Value readvalue = GLib.Value.Empty;
                    
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
