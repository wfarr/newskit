// Feed.cs
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
using System.Linq;

namespace Summa.Data {
    public class ItemEventArgs : EventArgs {
        public Summa.Data.Item Item;
        
        public ItemEventArgs() {}
    }
    
    public class Feed {
        public string Name {
            get {
                string[] feed = Summa.Core.Application.Database.GetFeed(Url);
                return feed[3];
            }
            set {
                Summa.Core.Application.Database.ChangeFeedInfo(Url, "name", value);
            }
        }
        public string Url;
        public string Author {
            get {
                string[] feed = Summa.Core.Application.Database.GetFeed(Url);
                return feed[3];
            }
            set {
                Summa.Core.Application.Database.ChangeFeedInfo(Url, "author", value);
            }
        }
        public string Subtitle {
            get {
                string[] feed = Summa.Core.Application.Database.GetFeed(Url);
                return feed[4];
            }
            set {
                Summa.Core.Application.Database.ChangeFeedInfo(Url, "subtitle", value);
            }
        }
        public string License {
            get {
                string[] feed = Summa.Core.Application.Database.GetFeed(Url);
                return feed[7];
            }
            set {
                Summa.Core.Application.Database.ChangeFeedInfo(Url, "license", value);
            }
        }
        public string Image {
            get {
                string[] feed = Summa.Core.Application.Database.GetFeed(Url);
                return feed[6];
            }
            set {
                Summa.Core.Application.Database.ChangeFeedInfo(Url, "image", value);
            }
        }
        public string Status {
            get {
                string[] feed = Summa.Core.Application.Database.GetFeed(Url);
                return feed[10];
            }
            set {
                Summa.Core.Application.Database.ChangeFeedInfo(Url, "status", value);
            }
        }
        public ArrayList Tags {
            get {
                string[] feed = Summa.Core.Application.Database.GetFeed(Url);
                string tags = feed[11];
                ArrayList al = new System.Collections.ArrayList();
                foreach ( string tag in tags.Split(',') ) {
                    al.Add(tag);
                }
                
                return al;
            }
            set {
                string[] tags = new string[value.Count];
                int index = 0;
                foreach ( string tag in value ) {
                    tags[index] = tag;
                    index++;
                }
                
                string jtags = String.Join(",", tags);
                Summa.Core.Application.Database.ChangeFeedInfo(Url, "tags", jtags);
            }
        }
        public string Favicon {
            get {
                string[] feed = Summa.Core.Application.Database.GetFeed(Url);
                return feed[12];
            }
            set {
                Summa.Core.Application.Database.ChangeFeedInfo(Url, "favicon", value);
            }
        }
        
        public ArrayList Items {
            get {
                ArrayList uris = Summa.Core.Application.Database.GetPosts(Url);
                ArrayList items = new ArrayList();
                
                foreach ( string[] item in uris ) {
                    items.Add(new Summa.Data.Item(item[1], Url));
                }
                
                return items;
            }
        }
        
        public int UnreadCount {
            get {
                int count = 0;
                
                foreach (string[] item in Summa.Core.Application.Database.GetPosts(Url)) {
                    if ( item[8] == "False" ) {
                        count++;
                    }
                }
                return count;
            }
        }
        
        public bool HasUnread {
            get {
                foreach ( string[] item in Summa.Core.Application.Database.GetPosts(Url)) {
                    if ( item[8] == "False" ) {
                        return true;
                    }
                }
                return false;
            }
        }
        
        public Feed(string url) {
            Url = url;
        }
        
        public void AppendTag(string tag) {
            bool found = false;
            foreach ( string stag in Tags ) {
                if ( stag == tag ) {
                    found = true;
                    break;
                }
            }
            
            if (!found) {
                ArrayList tags = Tags;
                tags.Add(tag);
                Tags = tags;
            }
        }
        
        public void RemoveTag(string tag) {
            bool found = false;
            foreach ( string stag in Tags ) {
                if ( stag == tag ) {
                    found = true;
                    break;
                }
            }
            
            if (found) {
                ArrayList tags = Tags;
                tags.Remove(tag);
                Tags = tags;
            }
        }
        
        public bool Update() {
            Summa.Net.Request request;
            try {
                request = new Summa.Net.Request(Url);
            } catch ( Summa.Core.Exceptions.NotFound e ) {
                Summa.Core.Log.LogException(e);
                return false;
            }
            
            Summa.Interfaces.IFeedParser parser = Summa.Net.Util.Sniff(request);
            
            parser.Items.Reverse();
            bool update = false;
            
            ArrayList urls = new ArrayList();
            
            foreach ( string[] item in Summa.Core.Application.Database.GetPosts(Url)) {
                urls.Add(item[1]);
            }
            
            foreach ( Summa.Parser.Item item in parser.Items ) {
                if ( !urls.Contains(item.Uri) ) {
                    update = true;
                    Summa.Core.Application.Database.AddItem(Url, item.Title, item.Uri, item.Date, item.LastUpdated, item.Author, item.Tags, item.Contents, item.EncUri, "False", "False");
                }
            }
            
            if ( update ) {
                // set the limit for the number of items we have at 100, but increase it by the number of flagged items so that they aren't dropped.
                int limit = 100;
                foreach ( string[] item in Summa.Core.Application.Database.GetPosts(Url) ) {
                    if ( item[9] == "True" ) {
                        limit++;
                    }
                }
                
                ArrayList del_items = Summa.Core.Application.Database.GetPosts(Url);
                
                foreach ( string[] item in del_items ) {
                    if ( Summa.Core.Application.Database.GetPosts(Url).Count > limit ) {
                        if ( item[9] == "False" ) {
                            Summa.Core.Application.Database.DeleteItem(Url, item[1]);
                        }
                    } else {
                        break;
                    }
                }
            }
            
            return update;
        }
        
        public void MarkItemsRead() {
            foreach (Summa.Data.Item item in Items) {
                item.Read = true;
            }
        }
    }
}
