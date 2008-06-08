using System;
using Gtk;

namespace Summa {
    public class ConfigDialog : Gtk.Window {
        private Gtk.VBox vbox;
        private Gtk.Notebook notebook;
        private Gtk.ButtonBox bbox;
        private Gtk.VBox general_vbox;
        
        private Gtk.CheckButton cb_notifications;
        private Gtk.CheckButton cb_sortfeedview;
        
        public ConfigDialog(Summa.Browser browser) : base(Gtk.WindowType.Toplevel) {
            TransientFor = browser;
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
            cb_notifications.Active = Summa.Config.ShowNotifications;
            cb_notifications.Toggled += new EventHandler(OnCbNotificationsToggled);
            interface_vbox.PackStart(cb_notifications, false, false, 0);
            
            cb_sortfeedview = new Gtk.CheckButton("Sort feeds according to unread items");
            cb_sortfeedview.Active = Summa.Config.SortFeedview;
            cb_sortfeedview.Toggled += new EventHandler(OnCbSortFeedViewToggled);
            interface_vbox.PackStart(cb_sortfeedview, false, false, 0);
            
            notebook.AppendPage(general_vbox, new Gtk.Label("General"));
        }
        
        private void OnCbNotificationsToggled(object obj, EventArgs args) {
            if ( cb_notifications.Active ) {
                Summa.Config.ShowNotifications = true;
            } else {
                Summa.Config.ShowNotifications = false;
            }
        }
        
        private void OnCbSortFeedViewToggled(object obj, EventArgs args) {
            if ( cb_sortfeedview.Active ) {
                Summa.Config.SortFeedview = true;
            } else {
                Summa.Config.SortFeedview = false;
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
