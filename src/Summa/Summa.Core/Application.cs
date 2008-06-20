/* Application.cs
 *
 * Copyright (C) 2008  Ethan Osten
 *
 * This library is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation, either version 2.1 of the License, or
 * (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with this library.  If not, see <http://www.gnu.org/licenses/>.
 *
 * Author:
 *     Ethan Osten <senoki@gmail.com>
 */

using System;
using System.Collections;
using Gtk;

namespace Summa {
    namespace Core {
        public static class Application {
            public static ArrayList Browsers;
            
            public static Summa.Gui.StatusIcon StatusIcon;
            public static Summa.Core.Updater Updater;
            public static Summa.Data.Storage.Database Database;
            
            public static Gtk.ListStore TagStore;
            
            public static void Main() {
                Gtk.Application.Init();
                
                TagStore = new Gtk.ListStore(typeof(Gdk.Pixbuf), typeof(string));
                
                Database = new Summa.Data.Storage.Database();
                Browsers = new ArrayList();
                Browsers.Add(new Summa.Gui.Browser());
                StatusIcon = new Summa.Gui.StatusIcon();
                Updater = new Summa.Core.Updater();
                
                foreach ( Summa.Gui.Browser browser in Browsers ) {
                    browser.ShowAll();
                }
                
                Gtk.Application.Run();
            }
        }
    }
}
