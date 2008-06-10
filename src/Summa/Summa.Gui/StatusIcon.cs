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
            private Browser b;
            
            public StatusIcon(Summa.Gui.Browser browser) {
                FromIconName = "internet-news-reader";
                b = browser;
                 if ( IconName == null ) {
                     FromIconName = "applications-internet";
                 }
                 
                 shown = true;
                int unread = Summa.Data.Core.GetUnreadCount();
                string us = unread.ToString();
                
                Tooltip = us+" unread items.";
                 
                 Activate += new EventHandler(ToggleBrowserStatus);
             }
             
             private void ToggleBrowserStatus(object obj, EventArgs args) {
                 if ( shown ) {
                     b.Hide();
                     shown = false;
                } else {
                    b.Show();
                    shown = true;
                }
             }
        }
    }
}
