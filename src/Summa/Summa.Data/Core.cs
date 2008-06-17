///* /home/eosten/Summa/Summa/Summa.Data.cs
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

namespace Summa {
    namespace Data {
        public static class Core {
            public static void ApplicationInit(string name) {
                Summa.Core.Util.Log(String.Format("{0} registered.", name));
            }
            
            public static Summa.Data.Feed RegisterFeed(string uri) {
                ArrayList urls = new ArrayList();
                
                foreach ( Summa.Data.Feed feed in GetFeeds() ) {
                    urls.Add(feed.Url);
                }
                
                Summa.Data.Feed retfeed = null;
                
                if ( !urls.Contains(uri) ) {
                    Summa.Net.Request request = new Summa.Net.Request(uri);
                    Summa.Data.Parser.FeedParser parser = Summa.Net.Feed.Sniff(request);
                    
                    Summa.Core.Application.Database.CreateFeed(parser.Uri, parser.Name, parser.Author, parser.Subtitle, parser.Image, parser.License, request.Etag, request.LastModified, "", "All", parser.Favicon);
                    
                    foreach ( Summa.Data.Parser.Item item in parser.Items ) {
                        Summa.Core.Application.Database.AddItem(uri, item.Title, item.Uri, item.Date, item.LastUpdated, item.Author, item.Tags, item.Contents, item.EncUri, "False", "False");
                    }
                }
                
                foreach ( Summa.Data.Feed feed in GetFeeds() ) {
                    if ( feed.Url == uri ) {
                        retfeed = feed;
                        break;
                    }
                }
                return retfeed;
            }
            
            public static Summa.Data.Feed RegisterFeed(string uri, string username, string password) {
                return null; //not implemented
            }
            
            public static void DeleteFeed(string uri) {
                Summa.Core.Application.Database.DeleteFeed(uri);
            }
            
            public static ArrayList GetFeeds() {
                ArrayList feeds = Summa.Core.Application.Database.GetFeeds();
                ArrayList retfeeds = new ArrayList();
                
                try {
                    foreach (string[] feed in feeds) {
                        retfeeds.Add(new Summa.Data.Feed(feed[1]));
                    }
                } catch ( Exception e ) {
                    Summa.Core.Util.Log("There are no feeds.", e);
                }
                
                return retfeeds;
            }
            
            public static ArrayList GetFeeds(string tag) {
                ArrayList retfeeds = new ArrayList();
                
                foreach (Summa.Data.Feed feed  in GetFeeds()) {
                    if (feed.Tags.Contains(tag)) {
                        retfeeds.Add(feed);
                    }
                }
                return retfeeds;
            }
            
            public static ArrayList GetTags() {
                ArrayList list = new ArrayList();
                
                try {
                    foreach ( Summa.Data.Feed feed in GetFeeds() ) {
                        foreach ( string tag in feed.Tags ) {
                            if ( !list.Contains(tag) ) {
                                list.Add(tag);
                            }
                        }
                    }
                } catch ( Exception e ) {
                    Summa.Core.Util.Log("There are no feeds.", e);
                }
                
                return list;
            }
            
            public static int GetUnreadCount() {
                int count = 0;
                
                try {
                    foreach ( Summa.Data.Feed feed in GetFeeds() ) {
                        foreach ( Summa.Data.Item item in feed.GetItems() ) {
                            if ( item.Read == false ) {
                                count++;
                            }
                        }
                    }
                } catch ( Exception e ) {
                    Summa.Core.Util.Log("There are no feeds.", e);
                }
                return count;
            }
            
            public static string[] ImportOpml(string filename) {
                return null; //not implemented
            }
            
            // FIXME - should have the Search stuff
        }
    }
}
