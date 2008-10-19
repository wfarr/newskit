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
using System.Collections;
using Gtk;

using Summa.Core;
using Summa.Data;
using Summa.Gui;

namespace Summa.Gui {
    public class TagWindow : Window {private VBox vbox;
        private Table table;
        private ButtonBox bbox;
        
        private CellRendererToggle crt;
        private ListStore store;
        private TreeView treeview;
        
        public ComboBox ComboBox;
        
        private TreeIter iter;
        
        private ArrayList Tags;
        
        public Feed Selected {
            get {
                if ( treeview.Selection.CountSelectedRows() != 0 ) {
                    TreeModel selectmodel;
                    treeview.Selection.GetSelected(out selectmodel, out iter);
                } else { store.GetIterFirst(out iter); }
                
                string val = (string)store.GetValue(iter, 2);
                
                return Feeds.RegisterFeed(val);
            }
        }
        
        public TagWindow() : base(WindowType.Toplevel) {
            Tags = new ArrayList();
            
            Title = "Manage your tags";
            BorderWidth = 5;
            DeleteEvent += OnClose;
            Resizable = false;
            
            vbox = new VBox();
            vbox.Spacing = 6;
            Add(vbox);
            
            table = new Table(2, 2, false);
            table.RowSpacing = 6;
            vbox.PackStart(table, false, false, 0);
            
            bbox = new HButtonBox();
            bbox.Layout = ButtonBoxStyle.End;
            
            AddTagsCombobox();
            AddFeedTreeView();
            AddButtons();
            vbox.PackStart(bbox, false, false, 0);
            
            Database.FeedChanged += OnFeedChanged;
        }
        
        private void OnFeedChanged(object obj, ChangedEventArgs args ) {
            if ( args.ItemProperty == "tags" ) {
                foreach ( string tag in args.Value.Split(',') ) {
                    if ( !Tags.Contains(tag) ) {
                        if ( tag != "All" ) {
                            ComboBox.AppendText(tag);
                            Tags.Add(tag);
                        }
                    }
                }
            }
        }
        
        private void AddTagsCombobox() {
            ComboBox = ComboBox.NewText();
            foreach ( string tag in Feeds.GetTags() ) {
                if ( tag != "All" ) {
                    ComboBox.AppendText(tag);
                    Tags.Add(tag);
                }
            }
            
            ComboBox.Changed += new EventHandler(OnCbUpdateIntervalChanged);
            
            table.Attach(ComboBox, 0, 1, 0, 1, AttachOptions.Fill, AttachOptions.Fill, 0, 0);
        }
        
        private void OnCbUpdateIntervalChanged(object obj, EventArgs args) {
            Populate(ComboBox.ActiveText);
        }
        
        private void AddFeedTreeView() {
            store = new ListStore(typeof(bool), typeof(string), typeof(string), typeof(Gdk.Pixbuf));
            
            crt = new CellRendererToggle();
            crt.Activatable = true;
            crt.Toggled += new ToggledHandler(OnCrtToggled);
            
            CellRendererText trender = new CellRendererText();
            trender.Ellipsize = Pango.EllipsizeMode.End;
            
            treeview = new TreeView();
            treeview.Model = store;
            
            ScrolledWindow treeview_swin = new ScrolledWindow(new Adjustment(0, 0, 0, 0, 0, 0), new Adjustment(0, 0, 0, 0, 0, 0));
            treeview_swin.Add(treeview);
            treeview_swin.SetSizeRequest(200, 300);
            treeview_swin.ShadowType = ShadowType.In;
            treeview_swin.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
            
            TreeViewColumn column_Use = new TreeViewColumn("Use", crt, "active", 0);
            treeview.AppendColumn(column_Use);
            
            TreeViewColumn column_Icon = new TreeViewColumn("Icon", new CellRendererPixbuf(), "pixbuf", 3);
            treeview.AppendColumn(column_Icon);
            
            TreeViewColumn column_Name = new TreeViewColumn("Title", trender, "text", 1);
            treeview.AppendColumn(column_Name);
            
            table.Attach(treeview_swin, 0, 1, 1, 2);
        }
        
        private void OnCrtToggled(object obj, ToggledArgs args) {
            TreeIter iter;
            store.GetIter(out iter, new TreePath(args.Path));
            
            if ( (bool)store.GetValue(iter, 0) ) {
                store.SetValue(iter, 0, false);
                Feed feed = Feeds.RegisterFeed((string)store.GetValue(iter, 2));
                
                feed.RemoveTag(ComboBox.ActiveText);
            } else {
                store.SetValue(iter, 0, true);
                Feed feed = Feeds.RegisterFeed((string)store.GetValue(iter, 2));
                
                feed.AppendTag(ComboBox.ActiveText);
            }
            Browser b = (Browser)Summa.Core.Application.Browsers[0];
            b.TagView.Update();
        }
        
        private void Populate(string tag) {
            store.Clear();
            
            foreach ( Feed feed in Feeds.GetFeeds() ) {
                TreeIter iter = store.Append();
                
                store.SetValue(iter, 0, feed.Tags.Contains(ComboBox.ActiveText));
                store.SetValue(iter, 1, feed.Name);
                store.SetValue(iter, 2, feed.Url);
                store.SetValue(iter, 3, feed.Favicon);
            }
        }
        
        private void AddButtons() {
            Button add_button = new Button(Stock.Add);
            add_button.Clicked += new EventHandler(OnAdd);
            bbox.PackStart(add_button);
            
            Button close_button = new Button(Stock.Close);
            close_button.Clicked  += new EventHandler(OnClose);
            bbox.PackStart(close_button);
        }
        
        private void OnAdd(object obj, EventArgs args) {
            Window t = new AddTagDialog(this);
            t.ShowAll();
        }
        
        private void OnClose(object obj, EventArgs args) {
            Destroy();
        }
    }
}
