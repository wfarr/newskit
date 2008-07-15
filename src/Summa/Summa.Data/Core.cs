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

namespace Summa.Data {
    public static class Core {
        public static void ApplicationInit(string name) {
            Summa.Core.Log.Message(String.Format("{0} registered.", name));
        }
        
        public static Summa.Data.Feed RegisterFeed(string uri) {
            ArrayList urls = new ArrayList();
            
            foreach ( Summa.Data.Feed feed in GetFeeds() ) {
                urls.Add(feed.Url);
            }
            
            Summa.Data.Feed retfeed = null;
            
            if ( !Summa.Core.Application.Database.FeedExists(uri) ) {
                Summa.Net.Request request;
                try {
                    request = new Summa.Net.Request(uri);
                } catch ( Summa.Core.Exceptions.NotFound e ) {
                    Summa.Core.Log.Exception(e);
                    return null;
                }
                
                if ( request.Status != System.Net.HttpStatusCode.NotFound ) {
                    Summa.Interfaces.IFeedParser parser = Summa.Net.Util.Sniff(request);
                    
                    if ( parser != null ) {
                        Summa.Core.Application.Database.CreateFeed(parser.Uri, parser.Name, parser.Author, parser.Subtitle, parser.Image, parser.License, request.Etag, request.LastModified, "", "All", parser.Favicon);
                        
                        parser.Items.Reverse();
                        
                        foreach ( Summa.Parser.Item item in parser.Items ) {
                            Summa.Core.Application.Database.AddItem(uri, item.Title, item.Uri, item.Date, item.LastUpdated, item.Author, item.Tags, item.Contents, item.EncUri, "False", "False");
                        }
                    }
                } else {
                    throw new Summa.Core.Exceptions.BadFeed();
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
                Summa.Core.Log.Exception(e, "There are no feeds.");
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
                foreach ( string tag in Summa.Core.Application.Database.GetTags() ) {
                    if ( !list.Contains(tag) ) {
                        list.Add(tag);
                    }
                }
            } catch ( Exception e ) {
                Summa.Core.Log.Exception(e);
            }
            
            try {
                foreach ( Summa.Data.Feed feed in GetFeeds() ) {
                    foreach ( string tag in feed.Tags ) {
                        if ( !list.Contains(tag) ) {
                            list.Add(tag);
                        }
                    }
                }
            } catch ( Exception e ) {
                Summa.Core.Log.Exception(e, "There are no feeds.");
            }
            
            return list;
        }
        
        public static int GetUnreadCount() {
            int count = 0;
            
            try {
                foreach ( Summa.Data.Feed feed in GetFeeds() ) {
                    count += feed.UnreadCount;
                }
            } catch ( Exception e ) {
                Summa.Core.Log.Exception(e, "There are no feeds.");
            }
            return count;
        }
        
        public static ArrayList ImportOpml(string filename) {
            Summa.Parser.OpmlParser opml = new Summa.Parser.OpmlParser(filename);
            return opml.Uris;
        }
        
        public static Summa.Data.Item GetItem(string itemuri) {
            foreach ( Summa.Data.Feed feed in GetFeeds() ) {
                foreach ( Summa.Data.Item item in feed.Items ) {
                    if ( item.Uri == itemuri ) {
                        return item;
                    }
                }
            }
            return null;
        }
    }
}
