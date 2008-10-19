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

using NewsKit;
using Summa.Core;
using Summa.Data;

namespace Summa.Data {
    public class ItemEventArgs : EventArgs {
        public Item Item;
        
        public ItemEventArgs() {}
    }
    
    public class Feed : ISource {
        public string Name {
            get {
                string[] feed = Database.GetFeed(Url);
                return feed[3];
            }
            set {
                Database.ChangeFeedInfo(Url, "name", value);
            }
        }
        private string url;
        public string Url {
            get { return url; }
            set { url = value; }
        }
        public string Author {
            get {
                string[] feed = Database.GetFeed(Url);
                return feed[4];
            }
            set {
                Database.ChangeFeedInfo(Url, "author", value);
            }
        }
        public string Subtitle {
            get {
                string[] feed = Database.GetFeed(Url);
                return feed[5];
            }
            set {
                Database.ChangeFeedInfo(Url, "subtitle", value);
            }
        }
        public string License {
            get {
                string[] feed = Database.GetFeed(Url);
                return feed[7];
            }
            set {
                Database.ChangeFeedInfo(Url, "license", value);
            }
        }
        public string Image {
            get {
                string[] feed = Database.GetFeed(Url);
                return feed[6];
            }
            set {
                Database.ChangeFeedInfo(Url, "image", value);
            }
        }
        public string Status {
            get {
                string[] feed = Database.GetFeed(Url);
                return feed[10];
            }
            set {
                Database.ChangeFeedInfo(Url, "status", value);
            }
        }
        public ArrayList Tags {
            get {
                string[] feed = Database.GetFeed(Url);
                string tags = feed[11];
                ArrayList al = new ArrayList();
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
                Database.ChangeFeedInfo(Url, "tags", jtags);
            }
        }
        public Gdk.Pixbuf Favicon {
            get {
                string[] feed = Database.GetFeed(Url);
                if ( feed[12] == "" ) {
                    return Gtk.IconTheme.Default.LookupIcon("feed-presence", 16, Gtk.IconLookupFlags.NoSvg).LoadIcon();
                } else {
                    return new Gdk.Pixbuf(feed[12], 16, 16);
                }
            }
            set {
                //Database.ChangeFeedInfo(Url, "favicon", value);
            }
        }
        
        public ArrayList Items {
            get {
                ArrayList uris = Database.GetPosts(Url);
                ArrayList items = new ArrayList();
                
                foreach ( string[] item in uris ) {
                    items.Add(new Item(item[1], Url));
                }
                
                return items;
            }
        }
        
        public int UnreadCount {
            get {
                int count = 0;
                
                foreach (string[] item in Database.GetPosts(Url)) {
                    if ( item[8] == "False" ) {
                        count++;
                    }
                }
                return count;
            }
        }
        
        public bool HasUnread {
            get {
                foreach ( string[] item in Database.GetPosts(Url)) {
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
            IFeedParser p;
            bool success = Parsing.ParseUri(Url, Database.GetFeed(Url)[9], out p);
            if ( !success ) {
                return false;
            }
            
            bool updated = false;
            
            ArrayList urls_we_have = new ArrayList();
            
            foreach ( string[] item in Database.GetPosts(Url)) {
                urls_we_have.Add(item[1]);
            }
            
            ArrayList items_to_add = new ArrayList();
            foreach ( Item item in p.Items ) {
                if ( !urls_we_have.Contains(item.Uri) ) {
                    updated = true;
                    
                    items_to_add.Add(item);
                } else {
                    // we do this because otherwise we have to deal with feeds
                    // like Cat and Girl, which have hundreds of items in 
                    // them. Once we delete them (later in this function), we
                    // end up adding them again on the next update, only to
                    // delete them again. However, this means that items added
                    // in-between old items aren't caught. This is a problem,
                    // but it's rare enough that it doesn't really matter.
                    break;
                }
            }
            
            items_to_add.Reverse();
            foreach ( Item item in items_to_add ) {
                Database.AddItem(Url, item.Title, item.Uri, item.Date.ToString(), item.LastUpdated.ToString(), item.Author, item.Tags, item.Contents, item.EncUri, "False", "False");
            }
            
            if ( updated ) {
                // set the limit for the number of items we have at 100, 
                // but increase it by the number of flagged items so that
                // they aren't dropped.
                int limit = Config.NumberOfItems;
                foreach ( string[] item in Database.GetPosts(Url) ) {
                    if ( item[9] == "True" ) {
                        limit++;
                    }
                }
                
                ArrayList del_items = Database.GetPosts(Url);
                
                foreach ( string[] item in del_items ) {
                    if ( Database.GetPosts(Url).Count > limit ) {
                        if ( item[9] == "False" ) {
                            Database.DeleteItem(Url, item[1]);
                        }
                    } else {
                        break;
                    }
                }
            }
            
            return updated;
        }
        
        public void MarkItemsRead() {
            foreach (Item item in Items) {
                item.Read = true;
            }
        }
    }
}
