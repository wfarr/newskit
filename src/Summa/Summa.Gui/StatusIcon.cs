// StatusIcon.cs
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
        public class StatusIcon : Gtk.StatusIcon {
            private bool shown;
            
            public StatusIcon() {
                FromIconName = "internet-news-reader";
                if ( IconName == null ) {
                    FromIconName = "applications-internet";
                }
                 
                shown = true;
                int unread = Summa.Data.Core.GetUnreadCount();
                string us = unread.ToString();
                
                Tooltip = us + " unread items.";
                
                Activate += new EventHandler(ToggleBrowserStatus);
                PopupMenu += RightClickMenu;
            }
            
            public void ToggleBrowserStatus(object obj, EventArgs args) {
                Summa.Core.Application.ToggleShown();
            }

            private void RightClickMenu(object obj, EventArgs args) {
                Gtk.Menu menu = new Gtk.Menu();

                // Add Button
                ImageMenuItem add_button = new ImageMenuItem("Add New Feed");
                Gtk.Image add_button_img = new Gtk.Image(Gtk.Stock.Add, Gtk.IconSize.Menu);
                add_button.Image = add_button_img;
                menu.Append(add_button);
                
                // Refresh Button
                ImageMenuItem refresh_button = new ImageMenuItem("Refresh Feeds");
                Gtk.Image refresh_button_img = new Gtk.Image(Gtk.Stock.Refresh, Gtk.IconSize.Menu);
                refresh_button.Image = refresh_button_img;
                menu.Append(refresh_button);

                // Preferences Button
                ImageMenuItem prefs_button = new ImageMenuItem("Preferences");
                Gtk.Image prefs_button_img = new Gtk.Image(Gtk.Stock.Preferences, Gtk.IconSize.Menu);
                prefs_button.Image = prefs_button_img;
                menu.Append(prefs_button);
                
                // Show/hide Button
                ImageMenuItem show_button;
                if ( shown ) {
                    show_button = new ImageMenuItem("Hide the window");
                } else {
                    show_button = new ImageMenuItem("Show the window");
                }
                menu.Append(show_button);

                // Quit Button
                ImageMenuItem quit_button = new ImageMenuItem("Quit");
                Gtk.Image quit_button_img = new Gtk.Image(Gtk.Stock.Quit, Gtk.IconSize.Menu);
                quit_button.Image = quit_button_img;
                menu.Append(quit_button);

                menu.ShowAll();
                menu.Popup();
                
                Summa.Gui.Browser b = (Summa.Gui.Browser)Summa.Core.Application.Browsers[0];

                add_button.Activated += new EventHandler(b.addaction.NewAddWindow);
                refresh_button.Activated += new EventHandler(b.Up_all_action.UpdateAll);
                prefs_button.Activated += new EventHandler(b.prefs_action.ShowConfigDialog);
                show_button.Activated += new EventHandler(ToggleBrowserStatus);
                quit_button.Activated += new EventHandler(b.CloseWindow);
            }
        }
    }
}
