// ConfigDialog.cs
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
    public class ConfigDialog : Window {
        private VBox vbox;
        private Notebook notebook;
        private ButtonBox bbox;
        private VBox general_vbox;
        private VBox statusicon_vbox;
        private VBox feeds_vbox;
        
        private CheckButton cb_notifications;
        private CheckButton cb_sortfeedview;
        private CheckButton cb_tabs;
        private CheckButton cb_icon;
        private CheckButton cb_widescreen;
        private ComboBox cb_updateinterval;
        private string[] updateinterval_options;
        private ListStore store;
        
        public ConfigDialog() : base(WindowType.Toplevel) {
            Title = "Summa Preferences";
            BorderWidth = 5;
            DeleteEvent += OnClose;
            
            vbox = new VBox();
            vbox.Spacing = 6;
            Add(vbox);
            
            notebook = new Notebook();
            vbox.PackStart(notebook, false, false, 0);
            
            bbox = new HButtonBox();
            bbox.Layout = ButtonBoxStyle.End;
            vbox.PackStart(bbox, false, false, 0);
            
            AddGeneralTab();
            AddStatusIconTab();
            AddCloseButton();
        }
        
        private void AddGeneralTab() {
            general_vbox = new VBox();
            general_vbox.BorderWidth = 5;
            general_vbox.Spacing = 10;
            
            Frame interface_frame = new Frame();
            Label interface_label = new Label();
            interface_label.Markup = ("<b>Interface</b>");
            interface_label.UseUnderline = true;
            interface_frame.LabelWidget = interface_label;
            interface_frame.LabelXalign = 0.0f;
            interface_frame.LabelYalign = 0.5f;
            interface_frame.Shadow = ShadowType.None;
            general_vbox.PackStart(interface_frame, false, false, 0);
            
            Alignment interface_alignment = new Alignment(0.0f, 0.0f, 1.0f, 1.0f);
            interface_alignment.TopPadding = (uint)(interface_frame == null ? 0 : 5);
            interface_alignment.LeftPadding = 12;
            interface_frame.Add(interface_alignment);
            
            VBox interface_vbox = new VBox();
            interface_vbox.BorderWidth = 5;
            interface_vbox.Spacing = 6;
            interface_alignment.Add(interface_vbox);
            
            cb_notifications = new CheckButton("Show notifications on feed updates");
            cb_notifications.Active = Config.ShowNotifications;
            cb_notifications.Toggled += new EventHandler(OnCbNotificationsToggled);
            interface_vbox.PackStart(cb_notifications, false, false, 0);
            
            cb_sortfeedview = new CheckButton("Sort feeds according to unread items");
            cb_sortfeedview.Active = Config.SortFeedview;
            cb_sortfeedview.Toggled += new EventHandler(OnCbSortFeedViewToggled);
            interface_vbox.PackStart(cb_sortfeedview, false, false, 0);
            
            cb_tabs = new CheckButton("Open links in tabs inside Summa");
            cb_tabs.Active = Config.OpenTabs;
            cb_tabs.Toggled += new EventHandler(OnCbTabsToggled);
            interface_vbox.PackStart(cb_tabs, false, false, 0);
            
            cb_widescreen = new CheckButton("Arrange window in widescreen mode");
            cb_widescreen.Active = Config.WidescreenView;
            cb_widescreen.Toggled += new EventHandler(OnCbWidescreenToggled);
            interface_vbox.PackStart(cb_widescreen, false, false, 0);
            
            Frame updating_frame = new Frame();
            Label updating_label = new Label();
            updating_label.Markup = ("<b>Updating</b>");
            updating_label.UseUnderline = true;
            updating_frame.LabelWidget = updating_label;
            updating_frame.LabelXalign = 0.0f;
            updating_frame.LabelYalign = 0.5f;
            updating_frame.Shadow = ShadowType.None;
            general_vbox.PackStart(updating_frame, false, false, 0);
            
            Alignment updating_alignment = new Alignment(0.0f, 0.0f, 1.0f, 1.0f);
            updating_alignment.TopPadding = (uint)(updating_frame == null ? 0 : 5);
            updating_alignment.LeftPadding = 12;
            updating_frame.Add(updating_alignment);
            
            VBox updating_vbox = new VBox();
            updating_vbox.BorderWidth = 5;
            updating_vbox.Spacing = 6;
            updating_alignment.Add(updating_vbox);
            
            Label updateinterval_label = new Label("Update interval: ");
            HBox updateinterval_hbox = new HBox();
            updateinterval_hbox.PackStart(updateinterval_label, false, false, 0);
            updating_vbox.PackStart(updateinterval_hbox);
            
            updateinterval_options = new string[4];
            updateinterval_options[0] = "Every thirty minutes";
            updateinterval_options[1] = "Every hour";
            updateinterval_options[2] = "Every two hours";
            updateinterval_options[3] = "Daily";
            
            cb_updateinterval = new ComboBox(updateinterval_options);
            SetCbUpdateIntervalText();
            cb_updateinterval.Changed += new EventHandler(OnCbUpdateIntervalChanged);
            updateinterval_hbox.PackStart(cb_updateinterval);
            
            notebook.AppendPage(general_vbox, new Label("General"));
        }
        
        private void AddStatusIconTab() {
            statusicon_vbox = new VBox();
            statusicon_vbox.BorderWidth = 5;
            statusicon_vbox.Spacing = 10;
            
            cb_icon = new CheckButton("Show the status icon");
            cb_icon.Active = Config.ShowStatusIcon;
            cb_icon.Toggled += new EventHandler(OnCbIconToggled);
            statusicon_vbox.PackStart(cb_icon, false, false, 0);
            
            Frame feeds_frame = new Frame();
            Label feeds_label = new Label();
            feeds_label.Markup = ("<b>Feeds to show icon for</b>");
            feeds_label.UseUnderline = true;
            feeds_frame.LabelWidget = feeds_label;
            feeds_frame.LabelXalign = 0.0f;
            feeds_frame.LabelYalign = 0.5f;
            feeds_frame.Shadow = ShadowType.None;
            statusicon_vbox.PackStart(feeds_frame, false, false, 0);
            
            Alignment feeds_alignment = new Alignment(0.0f, 0.0f, 1.0f, 1.0f);
            feeds_alignment.TopPadding = (uint)(feeds_frame == null ? 0 : 5);
            feeds_alignment.LeftPadding = 12;
            feeds_frame.Add(feeds_alignment);
            
            feeds_vbox = new VBox();
            feeds_vbox.BorderWidth = 5;
            feeds_vbox.Spacing = 6;
            feeds_alignment.Add(feeds_vbox);
            
            AddFeedTreeView();
            
            notebook.AppendPage(statusicon_vbox, new Label("Status Icon"));
        }
        
        private void OnCbNotificationsToggled(object obj, EventArgs args) {
            if ( cb_notifications.Active ) {
                Config.ShowNotifications = true;
            } else {
                Config.ShowNotifications = false;
            }
        }
        
        private void OnCbSortFeedViewToggled(object obj, EventArgs args) {
            if ( cb_sortfeedview.Active ) {
                Config.SortFeedview = true;
            } else {
                Config.SortFeedview = false;
            }
        }
        
        private void OnCbTabsToggled(object obj, EventArgs args) {
            if ( cb_tabs.Active ) {
                Config.OpenTabs = true;
            } else {
                Config.OpenTabs = false;
            }
        }
        
        private void OnCbIconToggled(object obj, EventArgs args) {
            if ( cb_icon.Active ) {
                Config.ShowStatusIcon = true;
            } else {
                Config.ShowStatusIcon = false;
            }
        }
        
        private void OnCbWidescreenToggled(object obj, EventArgs args) {
            if ( cb_widescreen.Active ) {
                Config.WidescreenView = true;
            } else {
                Config.WidescreenView = false;
            }
        }
        
        private void SetCbUpdateIntervalText() {
            TreeIter iter;
            
            switch(Config.GlobalUpdateInterval) {
                case 1800000:
                    cb_updateinterval.Model.GetIterFirst(out iter);
                    cb_updateinterval.SetActiveIter(iter);
                    break;
                case 3600000:
                    cb_updateinterval.Model.GetIterFirst(out iter);
                    cb_updateinterval.Model.IterNext(ref iter);
                    cb_updateinterval.SetActiveIter(iter);
                    break;
                case 7200000:
                    cb_updateinterval.Model.GetIterFirst(out iter);
                    cb_updateinterval.Model.IterNext(ref iter);
                    cb_updateinterval.Model.IterNext(ref iter);
                    cb_updateinterval.SetActiveIter(iter);
                    break;
                case 86400000:
                    cb_updateinterval.Model.GetIterFirst(out iter);
                    cb_updateinterval.Model.IterNext(ref iter);
                    cb_updateinterval.Model.IterNext(ref iter);
                    cb_updateinterval.Model.IterNext(ref iter);
                    cb_updateinterval.SetActiveIter(iter);
                    break;
            }
        }
        
        private void OnCbUpdateIntervalChanged(object obj, EventArgs args) {
            switch(cb_updateinterval.ActiveText) {
                case "Every thirty minutes":
                    Config.GlobalUpdateInterval = 1800000;
                    break;
                case "Every hour":
                    Config.GlobalUpdateInterval = 3600000;
                    break;
                case "Every two hours":
                    Config.GlobalUpdateInterval = 7200000;
                    break;
                case "Daily":
                    Config.GlobalUpdateInterval = 86400000;
                    break;
            }
        }
        
        private void AddFeedTreeView() {
            store = new ListStore(typeof(bool), typeof(string), typeof(string), typeof(Gdk.Pixbuf));
            
            CellRendererToggle crt = new CellRendererToggle();
            crt.Activatable = true;
            crt.Toggled += new ToggledHandler(OnCrtToggled);
            
            CellRendererText trender = new CellRendererText();
            trender.Ellipsize = Pango.EllipsizeMode.End;
            
            TreeView treeview = new TreeView();
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
            
            feeds_vbox.PackStart(treeview_swin);
            Populate("All");
        }
        
        private void OnCrtToggled(object obj, ToggledArgs args) {
            TreeIter iter;
            store.GetIter(out iter, new TreePath(args.Path));
            
            if ( (bool)store.GetValue(iter, 0) ) {
                store.SetValue(iter, 0, false);
                Feed feed = Feeds.RegisterFeed((string)store.GetValue(iter, 2));
                
                ArrayList uris = Config.IconFeedUris;
                uris.Remove(feed.Url);
                Config.IconFeedUris = uris;
            } else {
                store.SetValue(iter, 0, true);
                Feed feed = Feeds.RegisterFeed((string)store.GetValue(iter, 2));
                
                ArrayList uris = Config.IconFeedUris;
                uris.Add(feed.Url);
                Config.IconFeedUris = uris;
            }
            Summa.Core.Application.StatusIcon.CheckVisibility();
        }
        
        private void Populate(string tag) {
            store.Clear();
            
            foreach ( Feed feed in Feeds.GetFeeds() ) {
                TreeIter iter = store.Append();
                
                store.SetValue(iter, 0, Config.IconFeedUris.Contains(feed.Url));
                store.SetValue(iter, 1, feed.Name);
                store.SetValue(iter, 2, feed.Url);
                store.SetValue(iter, 3, feed.Favicon);
            }
        }
        
        private void AddCloseButton() {
            Button close_button = new Button(Stock.Close);
            close_button.Clicked  += new EventHandler(OnClose);
            bbox.PackStart(close_button);
        }
        
        private void OnClose(object obj, EventArgs args) {
            Hide();
        }
    }
}
