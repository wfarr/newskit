using System;
using Gtk;

namespace Summa {
    public class AddTagDialog : Gtk.Window {
        private Summa.FeedPropertiesDialog propdialog;
        
        private Gtk.VBox vbox;
        private Gtk.HBox hbox;
        private Gtk.HButtonBox bbox;
        private Gtk.Image image;
        private Gtk.Table table;
        private Gtk.Label label;
        private Gtk.Entry entry;
        private Gtk.Button cancel_button;
        private Gtk.Button add_button;
        
        public AddTagDialog(Summa.FeedPropertiesDialog dialog) : base(Gtk.WindowType.Toplevel) {
            TransientFor = dialog;
            propdialog = dialog;
            
            Title = "Add tag";
            IconName = "add";
            
            DeleteEvent += OnCancel;
            
            Resizable = false;
            BorderWidth = 6;
            
            vbox = new Gtk.VBox(false, 6);
            Add(vbox);
            hbox = new Gtk.HBox(false, 6);
            
            vbox.PackStart(hbox);
            
            image = new Gtk.Image(Gtk.Stock.Add, Gtk.IconSize.Dialog);
            hbox.PackStart(image);
            
            table = new Gtk.Table(2, 3, false);
            table.RowSpacing = 6;
            hbox.PackEnd(table);
            
            label = new Gtk.Label();
            label.Markup = "Enter the name of the tag:";
            table.Attach(label, 1, 2, 0, 1);
            
            entry = new Gtk.Entry();
            table.Attach(entry, 1, 2, 1, 2);
            
            bbox = new Gtk.HButtonBox();
            bbox.Layout = Gtk.ButtonBoxStyle.End;
            bbox.Spacing = 6;
            vbox.PackEnd(bbox);
                
            cancel_button = new Gtk.Button(Gtk.Stock.Cancel);
            cancel_button.Clicked += new EventHandler(OnCancel);
            bbox.PackStart(cancel_button);
            
            add_button = new Gtk.Button(Gtk.Stock.Add);
            add_button.Clicked += new EventHandler(OnAdd);
            bbox.PackEnd(add_button);
        }
        
        private void OnCancel(object obj, EventArgs args) {
            Destroy();
        }
        
        private void OnAdd(object obj, EventArgs args) {
            propdialog.feed.AppendTag(entry.Text);
            
            TreeIter iter = propdialog.store_tags.Append();
            propdialog.store_tags.SetValue(iter, 0, true);
            propdialog.store_tags.SetValue(iter, 1, entry.Text);
            
            propdialog.browser.tagview.Update();
            
            if ( propdialog.browser.tagview.Selected == entry.Text ) {
                propdialog.browser.feedview.Update();
            }
            
            Destroy();
        }
    }
}
