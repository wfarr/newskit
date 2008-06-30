// TagWindow.cs
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

namespace Summa {
    namespace Gui {
        public class TagWindow : Gtk.Window {private Gtk.VBox vbox;
            private Gtk.Table table;
            private Gtk.ButtonBox bbox;
            
            private Gtk.CellRendererToggle crt;
            private Gtk.ListStore store;
            private Gtk.TreeView treeview;
            
            public Gtk.ComboBox ComboBox;
            
            private Gtk.TreeModel selectmodel;
            private Gtk.TreeIter iter;
            
            public Summa.Data.Feed Selected {
                get {
                    if ( treeview.Selection.CountSelectedRows() != 0 ) {
                        treeview.Selection.GetSelected(out selectmodel, out iter);
                    } else { store.GetIterFirst(out iter); }
                    
                    string val = (string)store.GetValue(iter, 2);
                    
                    return Summa.Data.Core.RegisterFeed(val);
                }
            }
            
            public TagWindow() : base(Gtk.WindowType.Toplevel) {
                Title = "Manage your tags";
                BorderWidth = 5;
                DeleteEvent += OnClose;
                Resizable = false;
                
                vbox = new Gtk.VBox();
                vbox.Spacing = 6;
                Add(vbox);
                
                table = new Gtk.Table(2, 2, false);
                table.RowSpacing = 6;
                vbox.PackStart(table, false, false, 0);
                
                bbox = new Gtk.HButtonBox();
                bbox.Layout = Gtk.ButtonBoxStyle.End;
                
                AddTagsCombobox();
                AddFeedTreeView();
                AddButtons();
                vbox.PackStart(bbox, false, false, 0);
            }
            
            private void AddTagsCombobox() {
                ComboBox = Gtk.ComboBox.NewText();
                foreach ( string tag in Summa.Data.Core.GetTags() ) {
                    if ( tag != "All" ) {
                        ComboBox.AppendText(tag);
                    }
                }
                
                ComboBox.Changed += new EventHandler(OnCbUpdateIntervalChanged);
                
                table.Attach(ComboBox, 0, 1, 0, 1, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 0, 0);
            }
            
            private void OnCbUpdateIntervalChanged(object obj, EventArgs args) {
                Populate(ComboBox.ActiveText);
            }
            
            private void AddFeedTreeView() {
                store = new Gtk.ListStore(typeof(bool), typeof(string), typeof(string));
                
                crt = new Gtk.CellRendererToggle();
                crt.Activatable = true;
                crt.Toggled += new Gtk.ToggledHandler(OnCrtToggled);
                
                CellRendererText trender = new Gtk.CellRendererText();
                trender.Ellipsize = Pango.EllipsizeMode.End;
                
                treeview = new Gtk.TreeView();
                treeview.Model = store;
                
                ScrolledWindow treeview_swin = new Gtk.ScrolledWindow(new Gtk.Adjustment(0, 0, 0, 0, 0, 0), new Gtk.Adjustment(0, 0, 0, 0, 0, 0));
                treeview_swin.Add(treeview);
                treeview_swin.SetSizeRequest(200, 300);
                treeview_swin.ShadowType = Gtk.ShadowType.In;
                treeview_swin.SetPolicy(Gtk.PolicyType.Automatic, Gtk.PolicyType.Automatic);
                
                TreeViewColumn column_Use = new Gtk.TreeViewColumn("Use", crt, "active", 0);
                treeview.AppendColumn(column_Use);
                
                TreeViewColumn column_Name = new Gtk.TreeViewColumn("Title", trender, "text", 1);
                treeview.AppendColumn(column_Name);
                
                table.Attach(treeview_swin, 0, 1, 1, 2);
            }
            
            private void OnCrtToggled(object obj, ToggledArgs args) {
                TreeIter iter;
                store.GetIter(out iter, new Gtk.TreePath(args.Path));
                
                if ( (bool)store.GetValue(iter, 0) ) {
                    store.SetValue(iter, 0, false);
                    Summa.Data.Feed feed = Summa.Data.Core.RegisterFeed((string)store.GetValue(iter, 2));
                    
                    foreach ( Summa.Gui.Browser browser in Summa.Core.Application.Browsers ) {
                        if ( browser.FeedView.SetTag == ComboBox.ActiveText ) {
                            browser.FeedView.DeleteFeed(feed);
                        }
                    }
                    
                    feed.RemoveTag(ComboBox.ActiveText);
                } else {
                    store.SetValue(iter, 0, true);
                    Summa.Data.Feed feed = Summa.Data.Core.RegisterFeed((string)store.GetValue(iter, 2));
                    
                    foreach ( Summa.Gui.Browser browser in Summa.Core.Application.Browsers ) {
                        if ( browser.FeedView.SetTag == ComboBox.ActiveText ) {
                            browser.FeedView.AddNewFeed(feed);
                        }
                        browser.TagView.Update();
                    }
                    
                    feed.AppendTag(ComboBox.ActiveText);
                }
                Summa.Gui.Browser b = (Summa.Gui.Browser)Summa.Core.Application.Browsers[0];
                b.TagView.Update();
            }
            
            private void Populate(string tag) {
                store.Clear();
                
                foreach ( Summa.Data.Feed feed in Summa.Data.Core.GetFeeds() ) {
                    Gtk.TreeIter iter = store.Append();
                    
                    store.SetValue(iter, 0, feed.Tags.Contains(ComboBox.ActiveText));
                    store.SetValue(iter, 1, feed.Name);
                    store.SetValue(iter, 2, feed.Url);
                }
            }
            
            private void AddButtons() {
                Button add_button = new Gtk.Button(Gtk.Stock.Add);
                add_button.Clicked += new EventHandler(OnAdd);
                bbox.PackStart(add_button);
                
                Button close_button = new Gtk.Button(Gtk.Stock.Close);
                close_button.Clicked  += new EventHandler(OnClose);
                bbox.PackStart(close_button);
            }
            
            private void OnAdd(object obj, EventArgs args) {
                Window t = new Summa.Gui.AddTagDialog(this);
                t.ShowAll();
            }
            
            private void OnClose(object obj, EventArgs args) {
                Destroy();
            }
        }
    }
}
