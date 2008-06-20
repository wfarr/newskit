///* /home/eosten/Summa/Summa/ItemView.cs
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
using Gdk;
using System.Collections;
using System.Linq;

namespace Summa {
    namespace Gui {
        public class ItemView : Gtk.TreeView {
            public Gtk.ListStore store;
            private Gtk.IconTheme icon_theme;
            private Gdk.Pixbuf icon;
            
            private Summa.Data.Feed feedobj;
            private ArrayList items;
            
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
                store = new Gtk.ListStore(typeof(Gdk.Pixbuf), typeof(bool), typeof(bool), typeof(string), typeof(string), typeof(string));
                Model = store;
                
                // set up the columns for the view
                InsertColumn(-1, "Read", new Gtk.CellRendererPixbuf(), "pixbuf", 0);
                
                TreeViewColumn column_Date = new Gtk.TreeViewColumn("Date", new Gtk.CellRendererText(), "text", 3);
                column_Date.SortColumnId = 3;
                column_Date.SortIndicator = true;
                AppendColumn(column_Date);
                InsertColumn(-1, "Title", new Gtk.CellRendererText(), "text", 4);
                
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
                    icon = new Gdk.Pixbuf("/usr/share/pixmaps/summa-unread.png");
                } else if ( flagged ) {
                    icon = new Gdk.Pixbuf("/usr/share/pixmaps/summa-flagged.png");
                } else {
                    icon = new Gdk.Pixbuf("/usr/share/pixmaps/summa-inactive.png");
                }
                
                store.SetValue(titer, 0, icon);
                store.SetValue(titer, 1, read);
                store.SetValue(titer, 2, flagged);
                store.SetValue(titer, 3, MakePrettyDate(date));
                store.SetValue(titer, 4, title);
                store.SetValue(titer, 5, uri);
            }
            
            public void Populate(Summa.Data.Feed feed) {
                feedobj = feed;
                items = feed.GetItems();
                items.Reverse();
                
                store.Clear();
                
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
            }
            
            public void Update() {
                ArrayList uitems = feedobj.GetItems();
                ArrayList itemurls = new ArrayList();
                
                foreach (Summa.Data.Item item in items) {
                    itemurls.Add(item.Uri);
                }
                
                foreach ( Summa.Data.Item item in uitems ) {
                    if ( !itemurls.Contains(item.Uri) ) {
                        TreeIter iter = store.Append();
                        
                        AppendItem(iter, item);
                    }
                }
                items = feedobj.GetItems();
            }
            
            
            public int GetUnreadCount() {
                return feedobj.GetUnreadCount();
            }
            
            public void MarkSelectedRead() {
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
                /*string[] sdate = date.Split('_');
                string month = "";
                string dies = "";
                string hour = "";
                string minute = "";
                
                if ( sdate[1].Length == 1 ) {
                    month = "0"+sdate[1];
                } else {
                    month = sdate[1];
                }
                if ( sdate[2].Length == 1 ) {
                    dies = "0"+sdate[2];
                } else {
                    dies = sdate[2];
                }
                string day = sdate[0]+"/"+month+"/"+dies;
                
                if ( sdate[3].Length == 1 ) {
                    hour = "0"+sdate[3];
                } else {
                    hour = sdate[3];
                }
                if ( sdate[4].Length == 1 ) {
                    minute = "0"+sdate[4];
                } else {
                    minute = sdate[4];
                }
                
                return day+" at "+hour+":"+minute;*/
                return date;
            }
        }
    }
}
