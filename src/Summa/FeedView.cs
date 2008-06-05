///* /home/eosten/Summa/Summa/FeedView.cs
// *
// * Copyright (C) 2008  Ethan Osten
// *
// * This library is free software: you can redistribute it and/or modify
// * it under the terms of the GNU Lesser General Public License as published by
// * the Free Software Foundation, either version 2.1 of the License, or
// * (at your option) any later version.
// *
// * This library is distributed in the hope that it will be useful,
// * but WITHOUT ANY WARRANTY; without even the implied warranty of
// * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// * GNU Lesser General Public License for more details.
// *
// * You should have received a copy of the GNU Lesser General Public License
// * along with this library.  If not, see <http://www.gnu.org/licenses/>.
// *
// * Author:
// *     Ethan Osten <senoki@gmail.com>
// */
//

using System;
using Gtk;
using System.Collections;
using Gdk;
using System.Linq;
using Pango;

namespace Summa {
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
        public NewsKit.Feed Selected {
            get {
                if ( Selection.CountSelectedRows() != 0 ) {
                    Selection.GetSelected(out selectmodel, out iter);
                } else { store.GetIterFirst(out iter); }
                
                string val = (string)store.GetValue(iter, 2);
                
                return NewsKit.Daemon.RegisterFeed(val);
            }
        }
        
        private Gtk.CellRendererText trender;
        
        public FeedView() {
            // set up the liststore for the view
            store = new Gtk.ListStore(typeof(Gdk.Pixbuf),    // the icon
                                      typeof(string),        // the name
                                      typeof(string),        // the url
                                      typeof(bool));            // unread?
            Model = store;
            trender = new Gtk.CellRendererText();
            trender.Ellipsize = Pango.EllipsizeMode.End;
            
            // set up the columns for the view
            InsertColumn(-1, "Read", new Gtk.CellRendererPixbuf(), "pixbuf", 0);
            InsertColumn(-1, "Name", trender, "text", 1);
            
            // set up the icon theme so that we can make stuff pretty
            icon_theme = Gtk.IconTheme.Default;
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
            ArrayList ufeeds = NewsKit.Daemon.GetFeedsByTag(SetTag);
            
            foreach (NewsKit.Feed feed in ufeeds) {
                if ( !feeds.Contains(feed.Url) ) {
                    Gtk.TreeIter iter;
                    iter = store.Append();
                    
                    AppendFeed(feed, iter);
                }
            }
            feeds = NewsKit.Daemon.GetFeedsByTag(SetTag);
        }
        
        public void UpdateSelected() {
            AppendFeed(Selected, iter);
        }
        
        public void UpdateFeed(NewsKit.Feed feed) {
            store.GetIterFirst(out iter);
            
            bool found_the_feed = false;
            
            while ( !found_the_feed ) {
                GLib.Value urlvalue = new GLib.Value("");
                if ( !store.IterIsValid(iter) ) {
                    break;
                }
                store.GetValue(iter, 2, ref urlvalue);
                
                if ( urlvalue.ToString() == feed.Url ) {
                    found_the_feed = true;
                    break;
                } else {
                    store.IterNext(ref iter);
                }
            }
            
            if ( found_the_feed ) {
                AppendFeed(feed, iter);
            }
        }
        
        public void DeleteFeed(NewsKit.Feed feed) {
            store.GetIterFirst(out iter);
            
            bool found_the_feed = false;
            
            while ( !found_the_feed ) {
                GLib.Value urlvalue = new GLib.Value("");
                if ( !store.IterIsValid(iter) ) {
                    break;
                }
                store.GetValue(iter, 2, ref urlvalue);
                if ( urlvalue.ToString() == feed.Url ) {
                    found_the_feed = true;
                    break;
                } else {
                    store.IterNext( ref iter );
                }
            }
            
            if ( found_the_feed ) {
                store.Remove(ref iter);
            }
        }
        
        public void AppendFeed(NewsKit.Feed feed, Gtk.TreeIter titer) {
            int count = feed.GetUnreadCount();
            bool unread = false;
            
            Gdk.Pixbuf icon;
            if ( count > 0 ) {
                icon = new Gdk.Pixbuf("/usr/share/pixmaps/summa-unread.png");
                unread = true;
            } else {
                icon = new Gdk.Pixbuf("/usr/share/pixmaps/summa-inactive.png");
                unread = false;
            }
            
            string feedname = feed.Name;
            string feedurl = feed.Url;
            
            store.SetValue(titer, 0, icon);
            store.SetValue(titer, 1, feedname);
            store.SetValue(titer, 2, feedurl);
            store.SetValue(titer, 3, unread);
            
            while ( Gtk.Application.EventsPending() ) {
                Gtk.Main.Iteration();
            }
        }
        
        /*private void PopulateWithSearches() {
            store.clear();
            
            GLib.List<NewsKit.Search> searches = NewsKit.get_searches();
            
            foreach (NewsKit.Search search in searches) {
                Gtk.TreeIter iter;
                store.append(out iter);
                
                int count = search.get_unread_count();
                
                string feedname = search.name;
                string feedurl = null;
                
                store.set(iter, 0, icon_theme.lookup_icon("system-search", Gtk.IconSize.MENU, Gtk.IconLookupFlags.NO_SVG).load_icon(), 1, feedname, 2, feedurl, 3, true, -1);
            }
        }*/
        
        private void PopulateWithFeeds(string tag) {            
            store.Clear();
            
            feeds = NewsKit.Daemon.GetFeedsByTag(tag);
            
            SetTag = tag;
            
            foreach (NewsKit.Feed feed in feeds) {
                Gtk.TreeIter iter;
                iter = store.Append();
                
                AppendFeed(feed, iter);
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
