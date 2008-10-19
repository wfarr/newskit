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

using Summa.Core;
using Summa.Gui;

namespace Summa.Gui {
    public class MessageDialog : Window {
        private VBox vbox;
        private HBox hbox;
        private HButtonBox bbox;
        private Image image;
        private Table table;
        private Label label;
        private TextBuffer buffer;
        private TextView textview;
        private ScrolledWindow textviewsw;
        private Button close_button;
        
        public MessageDialog(ArrayList list) : base(WindowType.Toplevel) {
            Title = "Error";
            IconName = "dialog-error";
            
            DeleteEvent += OnClose;
            
            Resizable = false;
            BorderWidth = 6;
            
            vbox = new VBox(false, 6);
            Add(vbox);
            hbox = new HBox(false, 6);
            
            vbox.PackStart(hbox);
            
            image = new Image(Stock.DialogError, IconSize.Dialog);
            hbox.PackStart(image);
            
            table = new Table(2, 3, false);
            table.RowSpacing = 6;
            hbox.PackEnd(table);
            
            label = new Label();
            label.Markup = "<big><b>Some feeds failed to import</b></big>";
            table.Attach(label, 1, 2, 0, 1);
            
            buffer = new TextBuffer(new TextTagTable());
            foreach ( string feed in list ) {
                buffer.Text = buffer.Text + feed+"\n";;
            }
            
            textview = new TextView(buffer);
            textview.Editable = false;
            textview.WrapMode = WrapMode.Word;
            textview.SetSizeRequest(400, 150);
            
            textviewsw = new ScrolledWindow(new Adjustment(0, 0, 0, 0, 0, 0), new Adjustment(0, 0, 0, 0, 0, 0));
            textviewsw.ShadowType = ShadowType.In;
            textviewsw.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
            textviewsw.Add(textview);
            table.Attach(textviewsw, 1, 2, 1, 2);
            
            bbox = new HButtonBox();
            bbox.Layout = ButtonBoxStyle.End;
            bbox.Spacing = 6;
            vbox.PackEnd(bbox);
                
            close_button = new Button(Stock.Close);
            close_button.Clicked += new EventHandler(OnClose);
            bbox.PackStart(close_button);
            
            TransientFor = (Browser)Summa.Core.Application.Browsers[0];
        }
        
        private void OnClose(object obj, EventArgs args) {
            Destroy();
        }
    }
}
