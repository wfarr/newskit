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
            
            public static Summa.Gui.ConfigDialog ConfigDialog;
            
            public static bool WindowsShown;
            
            public static void Main() {
                Gtk.Application.Init();
                
                TagStore = new Gtk.ListStore(typeof(Gdk.Pixbuf), typeof(string));
                
                Database = new Summa.Data.Storage.Database();
                Browsers = new ArrayList();
                Browsers.Add(new Summa.Gui.Browser());
                StatusIcon = new Summa.Gui.StatusIcon();
                Updater = new Summa.Core.Updater();
                ConfigDialog = new Summa.Gui.ConfigDialog();
                
                foreach ( Summa.Gui.Browser browser in Browsers ) {
                    browser.ShowAll();
                }
                
                WindowsShown = true;
                
                Gtk.Application.Run();
            }
            
            public static void CloseWindow(Summa.Gui.Browser browser) {
                if ( Browsers.Count == 1 ) {
                    /* get dimensions */
                    int width;
                    int height;
                    
                    browser.GetSize(out width, out height);
                    
                    Summa.Core.Config.WindowWidth = width;
                    Summa.Core.Config.WindowHeight = height;
                    
                    /* get pane positions */
                    int main_size;
                    int left_size;
                    int right_size;
                    
                    main_size = browser.main_paned.Position;
                    left_size = browser.left_paned.Position;
                    right_size = browser.right_paned.Position;
                    
                    Summa.Core.Config.MainPanePosition = main_size;
                    Summa.Core.Config.LeftPanePosition = left_size;
                    Summa.Core.Config.RightPanePosition = right_size;
                    
                    Gtk.Main.Quit();
                } else {
                    browser.Destroy();
                    Browsers.Remove(browser);
                }
            }
            
            public static void ToggleShown() {
                foreach ( Summa.Gui.Browser browser in Summa.Core.Application.Browsers ) {
                    if ( WindowsShown ) {
                        browser.Hide();
                    } else {
                        browser.Show();
                    }
                }
                
                if ( WindowsShown ) {
                    WindowsShown = false;
                } else {
                    WindowsShown = true;
                }
            }
        }
    }
}
