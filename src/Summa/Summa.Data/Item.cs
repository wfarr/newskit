/* Item.cs
 *
 * Copyright (C) 2008  Ethan Osten
 *
 * This library is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation, either version 2.1 of the License, or
 * (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY{get {}set {}} without even the implied warranty of
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
using NDesk.DBus;

namespace Summa {
    namespace Data {
        public class Item {
            public string Title {
                get {
                    return feed.GetItemProperty(Uri, "title");
                }
                set {
                    feed.SetItemProperty(Uri, "title", value);
                }
            }
            public string Uri;
            public string Date {
                get {
                    return feed.GetItemProperty(Uri, "date");
                }
                set {
                    feed.SetItemProperty(Uri, "date", value);
                }
            }
            public string LastUpdated {
                get {
                    return feed.GetItemProperty(Uri, "last_updated");
                }
                set {
                    feed.SetItemProperty(Uri, "last_updated", value);
                }
            }
            public string Author {
                get {
                    return feed.GetItemProperty(Uri, "author");
                }
                set {
                    feed.SetItemProperty(Uri, "author", value);
                }
            }
            public string Tags {
                get {
                    return feed.GetItemProperty(Uri, "title");
                }
                set {
                    feed.SetItemProperty(Uri, "title", value);
                }
            }
            public string Contents {
                get {
                    return feed.GetItemProperty(Uri, "content");
                }
                set {
                    feed.SetItemProperty(Uri, "content", value);
                }
            }
            public string EncUri {
                get {
                    return feed.GetItemProperty(Uri, "encuri");
                }
                set {
                    feed.SetItemProperty(Uri, "encuri", value);
                }
            }
            public bool Read {
                get {
                    string val = feed.GetItemProperty(Uri, "read");
                    if ( val == "True" ) {
                        return true;
                    } else {
                        return false;
                    }
                }
                set {
                    if ( value == true ) {
                        feed.SetItemProperty(Uri, "read", "True");
                    } else {
                        feed.SetItemProperty(Uri, "read", "False");
                    }
                }
            }
            public bool Flagged {
                get {
                    string val = feed.GetItemProperty(Uri, "flagged");
                    if ( val == "True" ) {
                        return true;
                    } else {
                        return false;
                    }
                }
                set {
                    if ( value == true ) {
                        feed.SetItemProperty(Uri, "flagged", "True");
                    } else {
                        feed.SetItemProperty(Uri, "flagged", "False");
                    }
                }
            }
            private NewsKitFeed feed;
            
            public Item(string the_uri, string feeduid) {
                feed = Bus.Session.GetObject<NewsKitFeed>("org.gnome.NewsKit.Feeds", new ObjectPath("/org/gnome/NewsKit/"+feeduid));
                Uri = the_uri;
            }
        }
    }
}
