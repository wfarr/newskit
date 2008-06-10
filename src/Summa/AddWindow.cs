///* /home/eosten/Summa/Summa/AddWindow.cs
// *
// * Copyright (C) 2008  Ethan Osten
// *
// * This library is free software: you can redistribute it and/or modify
// * it under the terms of the GNU Lesser General Public License as published by
// * the Free Software Foundation, either version 2.1 of the License, or
// * (at your option) any later version.
// *
// * This library is distributed in the hope that it will be useful,
// * but WITHOUT ANY WARRANTY; without even the implied warranty of
// * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// * GNU Lesser General Public License for more details.
// *
// * You should have received a copy of the GNU Lesser General Public License
// * along with this library.  If not, see <http://www.gnu.org/licenses/>.
// *
// * Author:
// *     Ethan Osten <senoki@gmail.com>
// */
//

using System;
using NewsKit;
using Gtk;

namespace Summa {
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
        private Summa.Browser browser;
        
        public AddWindow(Summa.Browser browse) : base(Gtk.WindowType.Toplevel) {
            Title = "Add subscription";
            IconName = "add";
            browser = browse;
            TransientFor = browser;
            
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
            NewsKit.Daemon.RegisterFeed(entry.Text);
            
            browser.tagview.Update();
            browser.feedview.Update();
            
            Destroy();
        }
    }
}
