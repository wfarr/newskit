///* /home/eosten/Summa/Summa/DieuBookmarker.cs
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
using System.Collections;
using NDesk.DBus;

[Interface ("org.gnome.Dieu")]
public interface Dieu {
    void AddBookmark (string title, string url, string content, string tags);
}

namespace Summa {
    namespace Gui {
        public class DieuBookmarker : Summa.Core.Bookmarker {
            private Dieu dieu;
            private bool possible;
            
            public DieuBookmarker() {
                Dieu dieu = Bus.Session.GetObject<Dieu>("org.gnome.Dieu", new ObjectPath("/org/gnome/Dieu"));
                possible = true;
            }
            
            public void ShowBookmarkWindow(string title, string url, string content, string tags) {
                dieu.AddBookmark(title, url, content, tags);
            }
            
            public bool CanBookmark() {
                return possible;
            }
            
            public ArrayList GetBookmarks() {
                return new ArrayList();
            }
        }
    }
}
