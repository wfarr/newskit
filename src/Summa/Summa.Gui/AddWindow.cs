// AddWindow.cs
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
        public class AddWindow : Gtk.Window {
            private Gtk.VBox vbox;
            private Gtk.HBox hbox;
            private Gtk.HButtonBox bbox;
            private Gtk.Image image;
            private Gtk.Table table;
            private Gtk.Label label;
            private Gtk.Entry entry;
            private Gtk.Button cancel_button;
            private Gtk.Button add_button;
            
            public AddWindow() : base(Gtk.WindowType.Toplevel) {
                Title = "Add subscription";
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
                
                label = new Gtk.Label("Enter the URL of the feed:");
                label.Markup = "<b>Enter the URL of the feed:</b>";
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
            
            new public void Show() {
                ShowAll();
            }
            
            private void OnCancel(object obj, EventArgs args) {
                Destroy();
            }
            
            private void OnAdd(object obj, EventArgs args) {
                Summa.Data.Core.RegisterFeed(entry.Text);
                
                foreach ( Summa.Gui.Browser browser in Summa.Core.Application.Browsers ) {
                    browser.TagView.Update();
                    browser.FeedView.Update();
                }
                
                Destroy();
            }
        }
    }
}
