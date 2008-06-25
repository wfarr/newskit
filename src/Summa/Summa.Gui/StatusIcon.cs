///* /home/eosten/Summa/Summa/StatusIcon.cs
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

                add_button.Activated += new EventHandler(b.ShowAddWindow);
                refresh_button.Activated += new EventHandler(b.UpdateAll);
                prefs_button.Activated += new EventHandler(b.ShowConfigDialog);
                show_button.Activated += new EventHandler(ToggleBrowserStatus);
                quit_button.Activated += new EventHandler(b.CloseWindow);
            }
        }
    }
}
