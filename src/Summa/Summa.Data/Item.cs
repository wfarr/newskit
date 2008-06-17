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
                    string[] item = Summa.Core.Application.Database.GetItem(FeedUri, Uri);
                    return item[0];
                }
                set {
                    Summa.Core.Application.Database.ChangeItemInfo(FeedUri, Uri, "title", value);
                }
            }
            public string Uri;
            public string Date {
                get {
                    string[] item = Summa.Core.Application.Database.GetItem(FeedUri, Uri);
                    return item[2];
                }
                set {
                    Summa.Core.Application.Database.ChangeItemInfo(FeedUri, Uri, "date", value);
                }
            }
            public string LastUpdated {
                get {
                    string[] item = Summa.Core.Application.Database.GetItem(FeedUri, Uri);
                    return item[3];
                }
                set {
                    Summa.Core.Application.Database.ChangeItemInfo(FeedUri, Uri, "last_updated", value);
                }
            }
            public string Author {
                get {
                    string[] item = Summa.Core.Application.Database.GetItem(FeedUri, Uri);
                    return item[4];
                }
                set {
                    Summa.Core.Application.Database.ChangeItemInfo(FeedUri, Uri, "author", value);
                }
            }
            public string Tags {
                get {
                    string[] item = Summa.Core.Application.Database.GetItem(FeedUri, Uri);
                    return item[5];
                }
                set {
                    Summa.Core.Application.Database.ChangeItemInfo(FeedUri, Uri, "title", value);
                }
            }
            public string Contents {
                get {
                    string[] item = Summa.Core.Application.Database.GetItem(FeedUri, Uri);
                    return item[6];
                }
                set {
                    Summa.Core.Application.Database.ChangeItemInfo(FeedUri, Uri, "content", value);
                }
            }
            public string EncUri {
                get {
                    string[] item = Summa.Core.Application.Database.GetItem(FeedUri, Uri);
                    return item[7];
                }
                set {
                    Summa.Core.Application.Database.ChangeItemInfo(FeedUri, Uri, "encuri", value);
                }
            }
            public bool Read {
                get {
                    string[] item = Summa.Core.Application.Database.GetItem(FeedUri, Uri);
                    string val = item[8];
                    if ( val == "True" ) {
                        return true;
                    } else {
                        return false;
                    }
                }
                set {
                    if ( value == true ) {
                        Summa.Core.Application.Database.ChangeItemInfo(FeedUri, Uri, "read", "True");
                    } else {
                        Summa.Core.Application.Database.ChangeItemInfo(FeedUri, Uri, "read", "False");
                    }
                }
            }
            public bool Flagged {
                get {
                    string[] item = Summa.Core.Application.Database.GetItem(FeedUri, Uri);
                    string val = item[9];
                    if ( val == "True" ) {
                        return true;
                    } else {
                        return false;
                    }
                }
                set {
                    if ( value == true ) {
                        Summa.Core.Application.Database.ChangeItemInfo(FeedUri, Uri, "flagged", "True");
                    } else {
                        Summa.Core.Application.Database.ChangeItemInfo(FeedUri, Uri, "flagged", "False");
                    }
                }
            }
            private string FeedUri;
            
            public Item(string the_uri, string feeduri) {
                Uri = the_uri;
                FeedUri = feeduri;
            }
        }
    }
}
