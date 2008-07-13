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
using Gtk;

namespace Summa.Gui {
    public class ConfigDialog : Gtk.Window {
        private Gtk.VBox vbox;
        private Gtk.Notebook notebook;
        private Gtk.ButtonBox bbox;
        private Gtk.VBox general_vbox;
        
        private Gtk.CheckButton cb_notifications;
        private Gtk.CheckButton cb_sortfeedview;
        private Gtk.CheckButton cb_tabs;
        private Gtk.CheckButton cb_widescreen;
        private Gtk.ComboBox cb_updateinterval;
        private string[] updateinterval_options;
        
        public ConfigDialog() : base(Gtk.WindowType.Toplevel) {
            Title = "Summa Preferences";
            BorderWidth = 5;
            DeleteEvent += OnClose;
            
            vbox = new Gtk.VBox();
            vbox.Spacing = 6;
            Add(vbox);
            
            notebook = new Gtk.Notebook();
            vbox.PackStart(notebook, false, false, 0);
            
            bbox = new Gtk.HButtonBox();
            bbox.Layout = Gtk.ButtonBoxStyle.End;
            vbox.PackStart(bbox, false, false, 0);
            
            AddGeneralTab();
            AddCloseButton();
        }
        
        private void AddGeneralTab() {
            general_vbox = new Gtk.VBox();
            general_vbox.BorderWidth = 5;
            general_vbox.Spacing = 10;
            
            Frame interface_frame = new Gtk.Frame();
            Label interface_label = new Gtk.Label();
            interface_label.Markup = ("<b>Interface</b>");
            interface_label.UseUnderline = true;
            interface_frame.LabelWidget = interface_label;
            interface_frame.LabelXalign = 0.0f;
            interface_frame.LabelYalign = 0.5f;
            interface_frame.Shadow = ShadowType.None;
            general_vbox.PackStart(interface_frame, false, false, 0);
            
            Alignment interface_alignment = new Gtk.Alignment(0.0f, 0.0f, 1.0f, 1.0f);
            interface_alignment.TopPadding = (uint)(interface_frame == null ? 0 : 5);
            interface_alignment.LeftPadding = 12;
            interface_frame.Add(interface_alignment);
            
            VBox interface_vbox = new Gtk.VBox();
            interface_vbox.BorderWidth = 5;
            interface_vbox.Spacing = 6;
            interface_alignment.Add(interface_vbox);
            
            cb_notifications = new Gtk.CheckButton("Show notifications on feed updates");
            cb_notifications.Active = Summa.Core.Config.ShowNotifications;
            cb_notifications.Toggled += new EventHandler(OnCbNotificationsToggled);
            interface_vbox.PackStart(cb_notifications, false, false, 0);
            
            cb_sortfeedview = new Gtk.CheckButton("Sort feeds according to unread items");
            cb_sortfeedview.Active = Summa.Core.Config.SortFeedview;
            cb_sortfeedview.Toggled += new EventHandler(OnCbSortFeedViewToggled);
            interface_vbox.PackStart(cb_sortfeedview, false, false, 0);
            
            cb_tabs = new Gtk.CheckButton("Open links in tabs inside Summa");
            cb_tabs.Active = Summa.Core.Config.OpenTabs;
            cb_tabs.Toggled += new EventHandler(OnCbTabsToggled);
            interface_vbox.PackStart(cb_tabs, false, false, 0);
            
            cb_widescreen = new Gtk.CheckButton("Arrange window in widescreen mode");
            cb_widescreen.Active = Summa.Core.Config.WidescreenView;
            cb_widescreen.Toggled += new EventHandler(OnCbWidescreenToggled);
            interface_vbox.PackStart(cb_widescreen, false, false, 0);
            
            Frame updating_frame = new Gtk.Frame();
            Label updating_label = new Gtk.Label();
            updating_label.Markup = ("<b>Updating</b>");
            updating_label.UseUnderline = true;
            updating_frame.LabelWidget = updating_label;
            updating_frame.LabelXalign = 0.0f;
            updating_frame.LabelYalign = 0.5f;
            updating_frame.Shadow = ShadowType.None;
            general_vbox.PackStart(updating_frame, false, false, 0);
            
            Alignment updating_alignment = new Gtk.Alignment(0.0f, 0.0f, 1.0f, 1.0f);
            updating_alignment.TopPadding = (uint)(updating_frame == null ? 0 : 5);
            updating_alignment.LeftPadding = 12;
            updating_frame.Add(updating_alignment);
            
            VBox updating_vbox = new Gtk.VBox();
            updating_vbox.BorderWidth = 5;
            updating_vbox.Spacing = 6;
            updating_alignment.Add(updating_vbox);
            
            Label updateinterval_label = new Gtk.Label("Update interval: ");
            HBox updateinterval_hbox = new Gtk.HBox();
            updateinterval_hbox.PackStart(updateinterval_label, false, false, 0);
            updating_vbox.PackStart(updateinterval_hbox);
            
            updateinterval_options = new string[4];
            updateinterval_options[0] = "Every thirty minutes";
            updateinterval_options[1] = "Every hour";
            updateinterval_options[2] = "Every two hours";
            updateinterval_options[3] = "Daily";
            
            cb_updateinterval = new Gtk.ComboBox(updateinterval_options);
            SetCbUpdateIntervalText();
            cb_updateinterval.Changed += new EventHandler(OnCbUpdateIntervalChanged);
            updateinterval_hbox.PackStart(cb_updateinterval);
            
            notebook.AppendPage(general_vbox, new Gtk.Label("General"));
        }
        
        private void OnCbNotificationsToggled(object obj, EventArgs args) {
            if ( cb_notifications.Active ) {
                Summa.Core.Config.ShowNotifications = true;
            } else {
                Summa.Core.Config.ShowNotifications = false;
            }
        }
        
        private void OnCbSortFeedViewToggled(object obj, EventArgs args) {
            if ( cb_sortfeedview.Active ) {
                Summa.Core.Config.SortFeedview = true;
            } else {
                Summa.Core.Config.SortFeedview = false;
            }
        }
        
        private void OnCbTabsToggled(object obj, EventArgs args) {
            if ( cb_tabs.Active ) {
                Summa.Core.Config.OpenTabs = true;
            } else {
                Summa.Core.Config.OpenTabs = false;
            }
        }
        
        private void OnCbWidescreenToggled(object obj, EventArgs args) {
            if ( cb_widescreen.Active ) {
                Summa.Core.Config.WidescreenView = true;
            } else {
                Summa.Core.Config.WidescreenView = false;
            }
        }
        
        private void SetCbUpdateIntervalText() {
            TreeIter iter;
            
            switch(Summa.Core.Config.GlobalUpdateInterval) {
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
                    Summa.Core.Config.GlobalUpdateInterval = 1800000;
                    break;
                case "Every hour":
                    Summa.Core.Config.GlobalUpdateInterval = 3600000;
                    break;
                case "Every two hours":
                    Summa.Core.Config.GlobalUpdateInterval = 7200000;
                    break;
                case "Daily":
                    Summa.Core.Config.GlobalUpdateInterval = 86400000;
                    break;
            }
        }
        
        private void AddCloseButton() {
            Button close_button = new Gtk.Button(Gtk.Stock.Close);
            close_button.Clicked  += new EventHandler(OnClose);
            bbox.PackStart(close_button);
        }
        
        private void OnClose(object obj, EventArgs args) {
            Hide();
        }
    }
}
