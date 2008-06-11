using System;
using Gtk;

namespace Summa {
    namespace Gui {
        public class TagWindow : Gtk.Window {
            private Summa.Gui.Browser browser;
            
            private Gtk.VBox vbox;
            private Gtk.Table table;
            private Gtk.ButtonBox bbox;
            
            private Gtk.CellRendererToggle crt;
            private Gtk.ListStore store;
            private Gtk.TreeView treeview;
            
            private Gtk.ComboBox cbx;
            
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
            
            public TagWindow(Summa.Gui.Browser browse) : base(Gtk.WindowType.Toplevel) {
                browser = browse;
                
                TransientFor = browser;
                Title = "Manage your tags";
                BorderWidth = 5;
                DeleteEvent += OnClose;
                Resizable = false;
                
                vbox = new Gtk.VBox();
                vbox.Spacing = 6;
                Add(vbox);
                
                table = new Gtk.Table(2, 2, false);
                vbox.PackStart(table, false, false, 0);
                
                bbox = new Gtk.HButtonBox();
                bbox.Layout = Gtk.ButtonBoxStyle.End;
                
                AddTagsCombobox();
                AddFeedTreeView();
                AddCloseButton();
                vbox.PackStart(bbox, false, false, 0);
            }
            
            private void AddTagsCombobox() {
                cbx = Gtk.ComboBox.NewText();
                foreach ( string tag in Summa.Data.Core.GetTags() ) {
                    cbx.AppendText(tag);
                }
                
                cbx.Changed += new EventHandler(OnCbUpdateIntervalChanged);
                
                table.Attach(cbx, 0, 1, 0, 1, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 0, 0);
            }
            
            private void OnCbUpdateIntervalChanged(object obj, EventArgs args) {
                Populate(cbx.ActiveText);
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
                    
                    if ( browser.feedview.SetTag == cbx.ActiveText ) {
                        browser.feedview.DeleteFeed(feed);
                    }
                    
                    feed.RemoveTag(cbx.ActiveText);
                } else {
                    store.SetValue(iter, 0, true);
                    Summa.Data.Feed feed = Summa.Data.Core.RegisterFeed((string)store.GetValue(iter, 2));
                    
                    if ( browser.feedview.SetTag == cbx.ActiveText ) {
                        browser.feedview.AddNewFeed(feed);
                    }
                    
                    feed.AppendTag(cbx.ActiveText);
                }
            }
            
            private void Populate(string tag) {
                store.Clear();
                
                foreach ( Summa.Data.Feed feed in Summa.Data.Core.GetFeeds() ) {
                    Gtk.TreeIter iter = store.Append();
                    
                    store.SetValue(iter, 0, feed.Tags.Contains(cbx.ActiveText));
                    store.SetValue(iter, 1, feed.Name);
                    store.SetValue(iter, 2, feed.Url);
                }
            }
            
            private void AddCloseButton() {
                Button close_button = new Gtk.Button(Gtk.Stock.Close);
                close_button.Clicked  += new EventHandler(OnClose);
                bbox.PackStart(close_button);
            }
            
            private void OnClose(object obj, EventArgs args) {
                Destroy();
            }
        }
    }
}
