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
using System.Linq;
using NDesk.DBus;

namespace Summa {
    namespace Data {
        public class Feed {
            public string Name {
                get {
                    string[] feed = Summa.Core.Application.Database.GetFeed(Uri);
                    return feed[3];
                }
                set {
                    Summa.Core.Application.Database.ChangeFeedInfo(Url, "name", value);
                }
            }
            public string Url;
            public string Author {
                get {
                    string[] feed = Summa.Core.Application.Database.GetFeed(Uri);
                    return feed[3];
                }
                set {
                    Summa.Core.Application.Database.ChangeFeedInfo(Url, "author", value);
                }
            }
            public string Subtitle {
                get {
                    string[] feed = Summa.Core.Application.Database.GetFeed(Uri);
                    return feed[4];
                }
                set {
                    Summa.Core.Application.Database.ChangeFeedInfo(Url, "subtitle", value);
                }
            }
            public string License {
                get {
                    string[] feed = Summa.Core.Application.Database.GetFeed(Uri);
                    return feed[7];
                }
                set {
                    Summa.Core.Application.Database.ChangeFeedInfo(Url, "license", value);
                }
            }
            public string Image {
                get {
                    string[] feed = Summa.Core.Application.Database.GetFeed(Uri);
                    return feed[6];
                }
                set {
                    Summa.Core.Application.Database.ChangeFeedInfo(Url, "image", value);
                }
            }
            public string Status {
                get {
                    string[] feed = Summa.Core.Application.Database.GetFeed(Uri);
                    return feed[10];
                }
                set {
                    Summa.Core.Application.Database.ChangeFeedInfo(Url, "status", value);
                }
            }
            public ArrayList Tags {
                get {
                    string[] feed = Summa.Core.Application.Database.GetFeed(Uri);
                    string tags = feed[6];
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
                    string[] feed = Summa.Core.Application.Database.GetFeed(Uri);
                    return feed[12];
                }
                set {
                    Summa.Core.Application.Database.ChangeFeedInfo(Url, "favicon", value);
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
            
            public ArrayList GetItems() {
                ArrayList uris = Summa.Core.Application.Database.GetPosts(Url);
                ArrayList items = new ArrayList();
                
                foreach ( string[] item in uris ) {
                    items.Add(new Summa.Data.Item(item[1], Url));
                }
                
                return items;
            }
            
            public bool Update() {
                Summa.Net.Request request = new Summa.Net.Request(Url);
                Summa.Data.Parser.FeedParser parser = Summa.Net.Feed.Sniff(request);
                
                parser.Items.Reverse();
                bool update = false;
                
                ArrayList urls = new ArrayList();
                
                foreach ( string[] item in Summa.Core.Application.Database.GetPosts(Url)) {
                    urls.Add(item[1]);
                }
                
                foreach ( Summa.Data.Parser.Item item in parser.Items ) {
                    if ( !urls.Contains(item.Uri) ) {
                        update = true;
                        Summa.Core.Application.Database.AddItem(Uri, item.Title, item.Uri, item.Date, item.LastUpdated, item.Author, item.Tags, item.Contents, item.EncUri, "False", "False");
                    }
                }
            }
            
            public int GetUnreadCount() {
                int count = 0;
                
                foreach (string[] item in Summa.Core.Application.Database.GetPosts(Url)) {
                    if ( item[8] == "True" ) {
                        count++;
                    }
                }
                return count;
            }
            
            public void MarkItemsRead() {
                foreach (Summa.Data.Item item in GetItems()) {
                    item.Read = true;
                }
            }
        }
    }
}
