///* /home/eosten/Summa/Summa/Feed.cs
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

namespace NewsKit {
    public class Feed {
        public string Name {
            get {
                return feed.GetProperty("name");
            }
            set {
                feed.SetProperty("name", value);
            }
        }
        public string Url {
            get {
                return feed.GetProperty("url");
            }
            set {
                feed.SetProperty("url", value);
            }
        }
        public string Author {
            get {
                return feed.GetProperty("author");
            }
            set {
                feed.SetProperty("author", value);
            }
        }
        public string Subtitle {
            get {
                return feed.GetProperty("subtitle");
            }
            set {
                feed.SetProperty("subtitle", value);
            }
        }
        public string License {
            get {
                return feed.GetProperty("license");
            }
            set {
                feed.SetProperty("license", value);
            }
        }
        public string Image {
            get {
                return feed.GetProperty("image");
            }
            set {
                feed.SetProperty("image", value);
            }
        }
        public string Status {
            get {
                return feed.GetProperty("status");
            }
            set {
                feed.SetProperty("status", value);
            }
        }
        public ArrayList Tags {
            get {
                string tags = feed.GetProperty("tags");
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
                feed.SetProperty("tags", jtags);
            }
        }
        public string Favicon {
            get {
                return feed.GetProperty("favicon");
            }
            set {
                feed.SetProperty("favicon", value);
            }
        }
        
        public string Uid;
        private NewsKitFeed feed;
        
        public Feed(string feeduid) {
            feed = Bus.Session.GetObject<NewsKitFeed>("org.gnome.NewsKit.Feeds", new ObjectPath("/org/gnome/NewsKit/"+feeduid));
            Uid = feeduid;
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
        
        public ArrayList GetItems() {
            string[] uris = feed.GetItems();
            ArrayList items = new ArrayList();
            
            foreach ( string uri in uris ) {
                items.Add(new NewsKit.Item(uri, Uid));
            }
            
            return items;
        }
        
        public bool Update() {
            return feed.Update();
        }
        
        public int GetUnreadCount() {
            return feed.GetUnreadCount();
        }
        
        public void MarkItemsRead() {
            feed.MarkItemsRead();
        }
    }
}
