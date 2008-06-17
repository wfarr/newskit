///* /home/eosten/Summa/Summa/TagView.cs
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
using System.Collections;
using System.Linq;
using Gtk;
using Gdk;

namespace Summa {
    namespace Gui {
        public class TagView : Gtk.TreeView {
            private Gtk.ListStore store;
            private Gtk.IconTheme icon_theme;
            public ArrayList Tags;
            
            private Gtk.TreeModel selectmodel;
            private Gtk.TreeIter tagiter;
            private GLib.Value retval;
            
            public string Selected {
                get {
                    Selection.GetSelected(out selectmodel, out tagiter);
                    string val = (string)store.GetValue(tagiter, 1);
                    return val;
                }
            }
            
            public TagView() {
                // set up the liststore for the view
                store = new Gtk.ListStore(typeof(Gdk.Pixbuf), typeof(string));
                Model = store;
                
                // set up the columns for the view, and hide them. 
                InsertColumn(-1, "Pix", new Gtk.CellRendererPixbuf(), "pixbuf", 0);
                InsertColumn(-1, "Tag", new Gtk.CellRendererText(), "text", 1);
                HeadersVisible = false;
                
                // set up the icon theme so that we can make stuff pretty 
                icon_theme = Gtk.IconTheme.Default;
                
                // set up for NewsKit
                Tags = Summa.Data.Core.GetTags();
                
                /* Gtk.TreeIter tagiter;
                store.append(out tagiter);
                store.set(tagiter, 0, icon_theme.lookup_icon("system-search", Gtk.IconSize.MENU, Gtk.IconLookupFlags.NO_SVG).load_icon(), 1, "Searches", -1);*/
                
                Gtk.TreeIter tagiter;
                tagiter = store.Append();
                Pixbuf icon = new Gdk.Pixbuf("/usr/share/epiphany-browser/icons/hicolor/16x16/status/feed-presence.png");
                store.SetValue(tagiter, 0, icon);
                store.SetValue(tagiter, 1, "All feeds");
                
                foreach (string tag in Tags) {
                    if ( tag != "All" ) {
                        AppendTag(tag);
                    }
                }
            }
            
            public void Update() {
                ArrayList utags = Summa.Data.Core.GetTags();
                
                foreach ( string tag in utags ) {
                    if ( tag != "All" ) {
                        if ( !Tags.Contains(tag) ) {
                            AppendTag(tag);
                        }
                    }
                }
                Tags = Summa.Data.Core.GetTags();
            }
            
            public void AppendTag(string tag) {
                Gtk.TreeIter iter;
                iter = store.Append();
                store.SetValue(iter, 0, new Gdk.Pixbuf("/home/eosten/Desktop/tag.png"));
                store.SetValue(iter, 1, tag);
            }
        }
    }
}
