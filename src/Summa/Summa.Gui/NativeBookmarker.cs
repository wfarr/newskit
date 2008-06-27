using System;
using Gtk;
using System.Net;
using System.Collections;

namespace Summa {
    namespace Gui {
        public class NativeBookmarker : Summa.Core.Bookmarker {
            public NativeBookmarker() {
            }
            
            public void ShowBookmarkWindow(string title, string url, string content, string tags) {
            }
            
            public bool CanBookmark() {
                return false;
            }
            
            public ArrayList GetBookmarks() {
                return new ArrayList();
            }
        }
    }
}
