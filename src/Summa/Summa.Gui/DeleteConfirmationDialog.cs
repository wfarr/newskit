using System;
using Gtk;

namespace Summa {
    namespace Gui {
        public class DeleteConfirmationDialog : Gtk. Window {
            private Summa.Data.Feed feed;
            
            private Gtk.VBox vbox;
            private Gtk.HBox hbox;
            private Gtk.Image image;
            private Gtk.Table table;
            private Gtk.Label label;
            private Gtk.Label warn_label;
            private Gtk.ButtonBox bbox;
            private Gtk.Button cancel_button;
            private Gtk.Button delete_button;
            
            public DeleteConfirmationDialog(Summa.Data.Feed delfeed) : base(Gtk.WindowType.Toplevel) {
                feed = delfeed;
                
                Title = "Delete feed?";
                IconName = "edit-delete";
                
                DeleteEvent += OnCancel;
                
                Resizable = false;
                BorderWidth = 6;
                
                vbox = new Gtk.VBox(false, 6);
                Add(vbox);
                hbox = new Gtk.HBox(false, 6);
                
                vbox.PackStart(hbox);
                
                image = new Gtk.Image(Gtk.Stock.Delete, Gtk.IconSize.Dialog);
                hbox.PackStart(image);
                
                table = new Gtk.Table(2, 3, false);
                table.RowSpacing = 6;
                hbox.PackEnd(table);
                
                label = new Gtk.Label();
                label.Wrap = true;
                label.Markup = "<b><big>Delete feed "+feed.Name+"?</big></b>";
                table.Attach(label, 1, 2, 0, 1);
                
                warn_label = new Gtk.Label();
                warn_label.Wrap = true;
                warn_label.Markup = "This cannot be undone.";
                warn_label.SetAlignment(0.0F, 0.5F);
                table.Attach(warn_label, 1, 2, 1, 2);
                
                bbox = new Gtk.HButtonBox();
                bbox.Layout = Gtk.ButtonBoxStyle.End;
                bbox.Spacing = 6;
                vbox.PackEnd(bbox);
                    
                cancel_button = new Gtk.Button(Gtk.Stock.Cancel);
                cancel_button.Clicked += new EventHandler(OnCancel);
                bbox.PackStart(cancel_button);
                
                delete_button = new Gtk.Button(Gtk.Stock.Delete);
                delete_button.Clicked += new EventHandler(OnDelete);
                bbox.PackEnd(delete_button);
            }
            
            private void OnCancel(object obj, EventArgs args) {
                Destroy();
            }
            
            private void OnDelete(object obj, EventArgs args) {
                Summa.Data.Core.DeleteFeed(feed.Url);
                
                foreach ( Summa.Gui.Browser browser in Summa.Core.Application.Browsers ) {
                    browser.FeedView.DeleteFeed(feed);
                    
                    browser.HtmlView.Render("");
                    browser.ItemView.store.Clear();
                }
                Destroy();
            }
        }
    }
}
