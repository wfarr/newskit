// DeleteConfirmationDialog.cs
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
    public class DeleteConfirmationDialog : Gtk. Window {
        private Summa.Interfaces.ISource feed;
        
        private Gtk.VBox vbox;
        private Gtk.HBox hbox;
        private Gtk.Image image;
        private Gtk.Table table;
        private Gtk.Label label;
        private Gtk.Label warn_label;
        private Gtk.ButtonBox bbox;
        private Gtk.Button cancel_button;
        private Gtk.Button delete_button;
        
        public DeleteConfirmationDialog(Summa.Interfaces.ISource delfeed) : base(Gtk.WindowType.Toplevel) {
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
            
            Destroy();
        }
    }
}
