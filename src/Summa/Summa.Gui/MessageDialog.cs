// MessageDialog.cs
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

namespace Summa.Gui {
    public class MessageDialog : Gtk.Window {
        private Gtk.VBox vbox;
        private Gtk.HBox hbox;
        private Gtk.HButtonBox bbox;
        private Gtk.Image image;
        private Gtk.Table table;
        private Gtk.Label label;
        private Gtk.TextBuffer buffer;
        private Gtk.TextView textview;
        private Gtk.ScrolledWindow textviewsw;
        private Gtk.Button close_button;
        
        public MessageDialog(ArrayList list) : base(Gtk.WindowType.Toplevel) {
            Title = "Error";
            IconName = "dialog-error";
            
            DeleteEvent += OnClose;
            
            Resizable = false;
            BorderWidth = 6;
            
            vbox = new Gtk.VBox(false, 6);
            Add(vbox);
            hbox = new Gtk.HBox(false, 6);
            
            vbox.PackStart(hbox);
            
            image = new Gtk.Image(Gtk.Stock.DialogError, Gtk.IconSize.Dialog);
            hbox.PackStart(image);
            
            table = new Gtk.Table(2, 3, false);
            table.RowSpacing = 6;
            hbox.PackEnd(table);
            
            label = new Gtk.Label();
            label.Markup = "<big><b>Some feeds failed to import</b></big>";
            table.Attach(label, 1, 2, 0, 1);
            
            buffer = new Gtk.TextBuffer(new Gtk.TextTagTable());
            foreach ( string feed in list ) {
                buffer.Text = buffer.Text + feed+"\n";;
            }
            
            textview = new Gtk.TextView(buffer);
            textview.Editable = false;
            textview.WrapMode = Gtk.WrapMode.Word;
            textview.SetSizeRequest(400, 150);
            
            textviewsw = new Gtk.ScrolledWindow(new Gtk.Adjustment(0, 0, 0, 0, 0, 0), new Gtk.Adjustment(0, 0, 0, 0, 0, 0));
            textviewsw.ShadowType = Gtk.ShadowType.In;
            textviewsw.SetPolicy(Gtk.PolicyType.Automatic, Gtk.PolicyType.Automatic);
            textviewsw.Add(textview);
            table.Attach(textviewsw, 1, 2, 1, 2);
            
            bbox = new Gtk.HButtonBox();
            bbox.Layout = Gtk.ButtonBoxStyle.End;
            bbox.Spacing = 6;
            vbox.PackEnd(bbox);
                
            close_button = new Gtk.Button(Gtk.Stock.Close);
            close_button.Clicked += new EventHandler(OnClose);
            bbox.PackStart(close_button);
            
            TransientFor = (Summa.Gui.Browser)Summa.Core.Application.Browsers[0];
        }
        
        private void OnClose(object obj, EventArgs args) {
            Destroy();
        }
    }
}
