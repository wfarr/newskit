using System;
using GConf;

namespace Summa {
    public static class Config {
        private static GConf.Client client = new GConf.Client();
        
        private static string SUMMA_PATH = "/apps/summa";
        private static string KEY_LIBNOTIFY = "/apps/summa/show_notifications";
        private static string KEY_WIN_WIDTH = "/apps/summa/win_width";
        private static string KEY_WIN_HEIGHT = "/apps/summa/win_height";
        private static string KEY_MAIN_PANE_POSITION = "/apps/summa/main_pane_pos";
        private static string KEY_LEFT_PANE_POSITION = "/apps/summa/left_pane_pos";
        private static string KEY_RIGHT_PANE_POSITION = "/apps/summa/right_pane_pos";
        private static string KEY_SHOULD_SORT_FEEDVIEW = "/apps/summa/sort_feedview";
        private static string KEY_DEFAULT_ZOOM_LEVEL = "/apps/summa/default_zoom_level";
        
        public static bool ShowNotifications {
            get {
                return (bool)client.Get(KEY_LIBNOTIFY);
            }
            set {
                client.Set(KEY_LIBNOTIFY, value);
            }
        }
        
        public static int WindowHeight {
            get {
                return (int)client.Get(KEY_WIN_HEIGHT);
            }
            set {
                client.Set(KEY_WIN_HEIGHT, value);
            }
        }
        
        public static int WindowWidth {
            get {
                return (int)client.Get(KEY_WIN_WIDTH);
            }
            set {
                client.Set(KEY_WIN_WIDTH, value);
            }
        }
        
        public static int MainPanePosition {
            get {
                return (int)client.Get(KEY_MAIN_PANE_POSITION);
            }
            set {
                client.Set(KEY_MAIN_PANE_POSITION, value);
            }
        }
        
        public static int LeftPanePosition {
            get {
                return (int)client.Get(KEY_LEFT_PANE_POSITION);
            }
            set {
                client.Set(KEY_LEFT_PANE_POSITION, value);
            }
        }
        
        public static int RightPanePosition {
            get {
                return (int)client.Get(KEY_RIGHT_PANE_POSITION);
            }
            set {
                client.Set(KEY_RIGHT_PANE_POSITION, value);
            }
        }
        
        public static bool SortFeedview {
            get {
                return (bool)client.Get(KEY_SHOULD_SORT_FEEDVIEW);
            }
            set {
                client.Set(KEY_SHOULD_SORT_FEEDVIEW, value);
            }
        }
        
        public static int DefaultZoomLevel {
            get {
                return (int)client.Get(KEY_DEFAULT_ZOOM_LEVEL);
            }
            set {
                client.Set(KEY_DEFAULT_ZOOM_LEVEL, value);
            }
        }
    }
}
