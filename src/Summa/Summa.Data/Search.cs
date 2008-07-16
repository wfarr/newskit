// Search.cs
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
    // simple dummy implementation of Search, only powerful enough for things
    // like "get all unread items." Real search support, with multiple params
    // and live updates, is on the roadmap for 0.2. (FIXME)
    public class Search : Summa.Interfaces.ISource {
        private string name;
        public string Name {
            get {
                return name;
            }
            set {
                name = value;
            }
        }
        
        private string url;
        public string Url {
            get {
                return url;
            }
            set {
                url = value;
            }
        }
        
        public string Author {
            get { return ""; }
            set {}
        }
        
        public string Subtitle {
            get { return ""; }
            set {}
        }
        
        public string License {
            get { return ""; }
            set {}
        }
        
        public string Image {
            get { return ""; }
            set {}
        }
        
        public string Status {
            get { return ""; }
            set {}
        }
        
        public ArrayList Tags {
            get {
                ArrayList al = new ArrayList();
                al.Add("summa:Searches");
                return al;
            }
            set {}
        }
        
        public Gdk.Pixbuf Favicon {
            get { return Gtk.IconTheme.Default.LookupIcon("system-search", (int)Gtk.IconSize.Menu, Gtk.IconLookupFlags.NoSvg).LoadIcon(); }
            set {}
        }
        
        public int UnreadCount {
            get {
                int count = 0;
                foreach ( Summa.Data.Item item in Items ) {
                    if ( !item.Read ) {
                        count++;
                    }
                }
                return count;
            }
        }
        
        public bool HasUnread {
            get {
                bool unread = false;
                foreach ( Summa.Data.Item item in Items ) {
                    if ( !item.Read ) {
                        unread = true;
                        break;
                    }
                }
                return unread;
            }
        }
        
        public ArrayList Items {
            get {
                ArrayList items = new ArrayList();
                
                foreach ( ArrayList term in terms ) {
                    foreach ( Summa.Data.Feed feed in Summa.Data.Core.GetFeeds() ) {
                        if ( (string)term[0] == "read" && (string)term[1] == "False" && !feed.HasUnread ) {
                            continue;
                        } else {
                            foreach ( Summa.Data.Item item in feed.Items ) {
                                if ( (string)term[0] == "read" ) {
                                    if ( (string)term[1] == "True" && item.Read ) {
                                        items.Add(item);
                                    } else if ( (string)term[1] == "False" && !item.Read ) {
                                        items.Add(item);
                                    }
                                } else if ( (string)term[0] == "flagged" ) {
                                    if ( (string)term[1] == "True" && item.Flagged ) {
                                        items.Add(item);
                                    } else if ( (string)term[1] == "False" && !item.Flagged ) {
                                        items.Add(item);
                                    }
                                }
                            }
                        }
                    }
                }
                return items;
            }
        }
        
        private ArrayList terms;
        
        public Search(string searchuri) { // will take a specialized URI - see Database.cs for details (FIXME)
            terms = new ArrayList();
        }
        
        public void AppendTag(string tag) {}
        public void RemoveTag(string tag) {}
        
        public bool Update() { return false; }
        
        public void MarkItemsRead() {
            foreach ( Summa.Data.Item item in Items ) {
                item.Read = true;
            }
        }
        
        public void AddSearchTerm(string property, string val) {
            // acceptable terms: read, flagged
            // acceptable vals: True, False
            // FIXME
            ArrayList term = new ArrayList();
            term.Add(property);
            term.Add(val);
            terms.Add(term);
        }
    }
}
