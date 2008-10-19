// Config.cs
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
using GConf;

using Summa.Core;

namespace Summa.Core {
    public static class Config {
        private static Client client = new Client();
        
        //TODO: put these in a schema file.
        private static string KEY_LIBNOTIFY = "/apps/summa/show_notifications";
        private static string KEY_WIN_WIDTH = "/apps/summa/win_width";
        private static string KEY_WIN_HEIGHT = "/apps/summa/win_height";
        private static string KEY_MAIN_PANE_POSITION = "/apps/summa/main_pane_pos";
        private static string KEY_LEFT_PANE_POSITION = "/apps/summa/left_pane_pos";
        private static string KEY_RIGHT_PANE_POSITION = "/apps/summa/right_pane_pos";
        private static string KEY_SHOULD_SORT_FEEDVIEW = "/apps/summa/sort_feedview";
        private static string KEY_DEFAULT_ZOOM_LEVEL = "/apps/summa/default_zoom_level";
        private static string KEY_GLOBAL_UPDATE_INTERVAL = "/apps/summa/global_update_interval";
        private static string KEY_BOOKMARKER = "/apps/summa/bookmarker";
        private static string KEY_TABS = "/apps/summa/open_in_tabs";
        private static string KEY_WIDESCREEN = "/apps/summa/widescreen_view";
        private static string KEY_THEME = "/apps/summa/theme";
        private static string KEY_STATUS_ICON = "/apps/summa/show_status_icon";
        private static string KEY_ICON_FEEDS = "/apps/summa/show_status_icon_for";
        private static string KEY_NUMBER_OF_ITEMS = "/apps/summa/save_number_of_items";
        
        public static bool ShowNotifications {
            get {
                try {
                    return (bool)client.Get(KEY_LIBNOTIFY);
                } catch ( NoSuchKeyException e ) {
                    Log.Exception(e);
                    client.Set(KEY_LIBNOTIFY, true);
                    return true;
                }
            }
            set {
                client.Set(KEY_LIBNOTIFY, value);
            }
        }
        
        public static int WindowHeight {
            get {
                try {
                    return (int)client.Get(KEY_WIN_HEIGHT);
                } catch ( NoSuchKeyException e ) {
                    Log.Exception(e);
                    client.Set(KEY_WIN_HEIGHT, 400);
                    return 400;
                }
            }
            set {
                client.Set(KEY_WIN_HEIGHT, value);
            }
        }
        
        public static int WindowWidth {
            get {
                try {
                    return (int)client.Get(KEY_WIN_WIDTH);
                } catch ( NoSuchKeyException e ) {
                    Log.Exception(e);
                    client.Set(KEY_WIN_WIDTH, 700);
                    return 700;
                }
            }
            set {
                client.Set(KEY_WIN_WIDTH, value);
            }
        }
        
        public static int MainPanePosition {
            get {
                try {
                    return (int)client.Get(KEY_MAIN_PANE_POSITION);
                } catch ( NoSuchKeyException e ) {
                    Log.Exception(e);
                    client.Set(KEY_MAIN_PANE_POSITION, 170);
                    return 170;
                }
            }
            set {
                client.Set(KEY_MAIN_PANE_POSITION, value);
            }
        }
        
        public static int LeftPanePosition {
            get {
                try {
                    return (int)client.Get(KEY_LEFT_PANE_POSITION);
                } catch ( NoSuchKeyException e ) {
                    Log.Exception(e);
                    client.Set(KEY_LEFT_PANE_POSITION, 170);
                    return 170;
                }
            }
            set {
                client.Set(KEY_LEFT_PANE_POSITION, value);
            }
        }
        
        public static int RightPanePosition {
            get {
                try {
                    return (int)client.Get(KEY_RIGHT_PANE_POSITION);
                } catch ( NoSuchKeyException e ) {
                    Log.Exception(e);
                    client.Set(KEY_RIGHT_PANE_POSITION, 170);
                    return 170;
                }
            }
            set {
                client.Set(KEY_RIGHT_PANE_POSITION, value);
            }
        }
        
        public static bool SortFeedview {
            get {
                try {
                    return (bool)client.Get(KEY_SHOULD_SORT_FEEDVIEW);
                } catch ( NoSuchKeyException e ) {
                    Log.Exception(e);
                    client.Set(KEY_SHOULD_SORT_FEEDVIEW, false);
                    return false;
                }
            }
            set {
                client.Set(KEY_SHOULD_SORT_FEEDVIEW, value);
            }
        }
        
        public static int DefaultZoomLevel {
            get {
                try {
                    return (int)client.Get(KEY_DEFAULT_ZOOM_LEVEL);
                } catch ( NoSuchKeyException e ) {
                    Log.Exception(e);
                    client.Set(KEY_DEFAULT_ZOOM_LEVEL, 10);
                    return 10;
                }
            }
            set {
                client.Set(KEY_DEFAULT_ZOOM_LEVEL, value);
                Notifier.ChangeZoom();
            }
        }
        
        public static uint GlobalUpdateInterval {
            get {
                try {
                    int a = (int)client.Get(KEY_GLOBAL_UPDATE_INTERVAL);
                    return (uint)a;
                } catch ( NoSuchKeyException e ) {
                    Log.Exception(e);
                    client.Set(KEY_GLOBAL_UPDATE_INTERVAL, 3600000);
                    return 3600000;
                }
            }
            set {
                client.Set(KEY_GLOBAL_UPDATE_INTERVAL, (int)value);
            }
        }
        
        public static string Bookmarker {
            get {
                try {
                    return (string)client.Get(KEY_BOOKMARKER);
                } catch ( NoSuchKeyException e ) {
                    Log.Exception(e);
                    client.Set(KEY_BOOKMARKER, "Native");
                    return "Native";
                }
            }
            set {
                client.Set(KEY_BOOKMARKER, value);
            }
        }
        
        public static bool OpenTabs {
            get {
                try {
                    return (bool)client.Get(KEY_TABS);
                } catch ( NoSuchKeyException e ) {
                    Log.Exception(e);
                    client.Set(KEY_TABS, false);
                    return false;
                }
            }
            set {
                client.Set(KEY_TABS, value);
            }
        }
        
        public static bool WidescreenView {
            get {
                try {
                    return (bool)client.Get(KEY_WIDESCREEN);
                } catch ( NoSuchKeyException e ) {
                    Log.Exception(e);
                    client.Set(KEY_WIDESCREEN, false);
                    return false;
                }
            }
            set {
                client.Set(KEY_WIDESCREEN, value);
                Notifier.ChangeView();
            }
        }
        
        public static bool Connected {
            get {
                try {
                    if ( Summa.Net.NetworkManager.Status() == Summa.Net.ConnectionState.Connected ) {
                        return true;
                    } else {
                        return false;
                    }
                } catch ( Exception ) {
                    return true;
                }
            }
            set {
            }
        }
        
        public static ITheme Theme {
            get {
                try {
                    string name = (string)client.Get(KEY_THEME);
                    foreach ( ITheme theme in ThemeManager.Themes ) {
                        if ( theme.Name == name ) {
                            return theme;
                        }
                    }
                    return new Summa.Gui.NativeTheme();
                } catch ( Exception ) {
                    client.Set(KEY_THEME, "Native");
                    return new Summa.Gui.NativeTheme();
                }
            }
            set {
                ITheme theme = (ITheme)value;
                client.Set(KEY_THEME, theme.Name);
            }
        }
        
        public static bool ShowStatusIcon {
            get {
                try {
                    return (bool)client.Get(KEY_STATUS_ICON);
                } catch ( NoSuchKeyException e ) {
                    Log.Exception(e);
                    client.Set(KEY_STATUS_ICON, false);
                    return false;
                }
            }
            set {
                client.Set(KEY_STATUS_ICON, value);
                Notifier.ShowIcon();
            }
        }
        
        public static ArrayList IconFeedUris {
            get {
                try {
                    string feeds = (string)client.Get(KEY_ICON_FEEDS);
                    string[] urls = feeds.Split(',');
                    ArrayList returls = new ArrayList();
                    foreach ( string url in urls ) {
                        if ( url != "" && url != null ) {
                            returls.Add(url);
                        }
                    }
                    return returls;
                } catch ( NoSuchKeyException e ) {
                    Log.Exception(e);
                    client.Set(KEY_ICON_FEEDS, "");
                    return new ArrayList();
                }
            }
            set {
                string[] array = (string[])value.ToArray(typeof(string));
                string retstring = String.Join(",", array);
                client.Set(KEY_ICON_FEEDS, retstring);
            }
        }
        
        public static int NumberOfItems {
            get {
                try {
                    return (int)client.Get(KEY_NUMBER_OF_ITEMS);
                } catch ( NoSuchKeyException e ) {
                    Log.Exception(e);
                    client.Set(KEY_NUMBER_OF_ITEMS, 100);
                    return 100;
                }
            }
            set {
                client.Set(KEY_NUMBER_OF_ITEMS, value);
            }
        }
    }
}
