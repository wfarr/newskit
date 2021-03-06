// AddTagDialog.cs
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

using Summa.Core;
using Summa.Gui;

namespace Summa.Gui {
    public class AddTagDialog : Window {
        private FeedPropertiesDialog propdialog;
        private TagWindow twindow;
        
        private VBox vbox;
        private HBox hbox;
        private HButtonBox bbox;
        private Image image;
        private Table table;
        private Label label;
        private Entry entry;
        private Button cancel_button;
        private Button add_button;
        
        public AddTagDialog(FeedPropertiesDialog dialog) : base(WindowType.Toplevel) {
            TransientFor = dialog;
            propdialog = dialog;
            
            DialogConstructPriv();
            
            add_button.Clicked += new EventHandler(OnAdd);
        }
        
        public AddTagDialog(TagWindow dialog) : base(WindowType.Toplevel) {
            TransientFor = dialog;
            twindow = dialog;
            
            DialogConstructPriv();
            
            add_button.Clicked += new EventHandler(OnAddForWindow);
        }
        
        private void DialogConstructPriv() {Title = "Add tag";
            IconName = "add";
            
            DeleteEvent += OnCancel;
            
            Resizable = false;
            BorderWidth = 6;
            
            vbox = new VBox(false, 6);
            Add(vbox);
            hbox = new HBox(false, 6);
            
            vbox.PackStart(hbox);
            
            image = new Image(Stock.Add, IconSize.Dialog);
            hbox.PackStart(image);
            
            table = new Table(2, 3, false);
            table.RowSpacing = 6;
            hbox.PackEnd(table);
            
            label = new Label();
            label.Markup = "Enter the name of the tag:";
            table.Attach(label, 1, 2, 0, 1);
            
            entry = new Entry();
            table.Attach(entry, 1, 2, 1, 2);
            
            bbox = new HButtonBox();
            bbox.Layout = ButtonBoxStyle.End;
            bbox.Spacing = 6;
            vbox.PackEnd(bbox);
                
            cancel_button = new Button(Stock.Cancel);
            cancel_button.Clicked += new EventHandler(OnCancel);
            bbox.PackStart(cancel_button);
            
            add_button = new Button(Stock.Add);
            bbox.PackEnd(add_button);
        }
        
        private void OnCancel(object obj, EventArgs args) {
            Destroy();
        }
        
        private void OnAdd(object obj, EventArgs args) {
            propdialog.feed.AppendTag(entry.Text);
            
            Destroy();
        }
        
        private void OnAddForWindow(object obj, EventArgs args) {
            twindow.ComboBox.AppendText(entry.Text);
            
            Destroy();
        }
    }
}
