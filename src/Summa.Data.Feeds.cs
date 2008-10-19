// Core.cs
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

using NewsKit;

using Summa.Core;
using Summa.Core.Exceptions;
using Summa.Data;

namespace Summa.Data {
    public static class Feeds {
        public static void ApplicationInit(string name) {
            Log.Message(String.Format("{0} registered.", name));
        }
        
        public static Feed RegisterFeed(string uri) {
            ArrayList urls = new ArrayList();
            
            foreach ( Feed feed in GetFeeds() ) {
                urls.Add(feed.Url);
            }
            
            Feed retfeed = null;
            
            if ( !Database.FeedExists(uri) ) {
                IFeedParser parser;
                bool success = Parsing.ParseUri(uri, "", out parser);
                
                if ( success ) {
                    if ( parser != null ) {
                        Database.CreateFeed(parser.Uri, parser.Name, parser.Author, parser.Subtitle, parser.Image, parser.License, parser.Request.Etag, parser.Request.LastModified, "", "All", parser.Favicon);
                        
                        parser.Items.Reverse();
                        
                        foreach ( Item item in parser.Items ) {
                            Database.AddItem(parser.Request.Uri, item.Title, item.Uri, item.Date.ToString(), item.LastUpdated.ToString(), item.Author, item.Tags, item.Contents, item.EncUri, "False", "False");
                        }
                        
                        string file_name = null;
                        bool icon_found = Parsing.FindFavicon(parser.Uri, Database.GetGeneratedName(parser.Uri), out file_name);
                        
                        if ( icon_found ) {
                            Database.ChangeFeedInfo(parser.Uri, "favicon", file_name);
                        }
                    } else {
                        throw new BadFeed();
                    }
                } else {
                    throw new BadFeed();
                }
            }
            
            foreach ( Feed feed in GetFeeds() ) {
                if ( feed.Url == uri ) {
                    retfeed = feed;
                    break;
                }
            }
            return retfeed;
        }
        
        public static Feed RegisterFeed(string uri, string username, string password) {
            return null; // FIXME: not implemented
        }
        
        public static void DeleteFeed(string uri) {
            Database.DeleteFeed(uri);
        }
        
        public static ArrayList GetFeeds() {
            ArrayList feeds = Database.GetFeeds();
            ArrayList retfeeds = new ArrayList();
            
            try {
                foreach (string[] feed in feeds) {
                    retfeeds.Add(new Feed(feed[1]));
                }
            } catch ( Exception e ) {
                Log.Exception(e, "There are no feeds.");
            }
            
            return retfeeds;
        }
        
        public static ArrayList GetFeeds(string tag) {
            ArrayList retfeeds = new ArrayList();
            
            foreach (Feed feed  in GetFeeds()) {
                if (feed.Tags.Contains(tag)) {
                    retfeeds.Add(feed);
                }
            }
            return retfeeds;
        }
        
        public static ArrayList GetTags() {
            ArrayList list = new ArrayList();
            
            try {
                foreach ( string tag in Database.GetTags() ) {
                    if ( !list.Contains(tag) ) {
                        list.Add(tag);
                    }
                }
            } catch ( Exception e ) {
                Log.Exception(e);
            }
            
            try {
                foreach ( Feed feed in GetFeeds() ) {
                    foreach ( string tag in feed.Tags ) {
                        if ( !list.Contains(tag) ) {
                            list.Add(tag);
                        }
                    }
                }
            } catch ( Exception e ) {
                Log.Exception(e, "There are no feeds.");
            }
            
            return list;
        }
        
        public static int GetUnreadCount() {
            int count = 0;
            
            try {
                foreach ( Feed feed in GetFeeds() ) {
                    count += feed.UnreadCount;
                }
            } catch ( Exception e ) {
                Log.Exception(e, "There are no feeds.");
            }
            return count;
        }
        
        public static ArrayList ImportOpml(string filename) {
            OpmlParser opml = new OpmlParser(filename);
            return opml.Uris;
        }
        
        public static Item GetItem(string itemuri) {
            foreach ( Feed feed in GetFeeds() ) {
                foreach ( Item item in feed.Items ) {
                    if ( item.Uri == itemuri ) {
                        return item;
                    }
                }
            }
            return null;
        }
    }
}
