// Item.cs
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

using Summa.Core;
using Summa.Data;

namespace Summa.Data {
    public class Item {
        public string Title {
            get {
                try {
                    string[] item = Summa.Core.Database.GetItem(FeedUri, Uri);
                    string hack = item[0].Replace("???", "'");
                    return hack;
                } catch ( Exception e ) {
                    Summa.Core.Log.Exception(e);
                    return "";
                }
            }
            set {
                Summa.Core.Database.ChangeItemInfo(FeedUri, Uri, "title", value);
            }
        }
        public string Uri;
        public DateTime Date {
            get {
                try {
                    string[] item = Summa.Core.Database.GetItem(FeedUri, Uri);
                    return Convert.ToDateTime(item[2]);
                } catch ( Exception e ) {
                    Summa.Core.Log.Exception(e);
                    return DateTime.MinValue;
                }
            }
            set {
                Summa.Core.Database.ChangeItemInfo(FeedUri, Uri, "date", value.ToString());
            }
        }
        public string LastUpdated {
            get {
                try {
                    string[] item = Summa.Core.Database.GetItem(FeedUri, Uri);
                    return item[3];
                } catch ( Exception e ) {
                    Summa.Core.Log.Exception(e);
                    return "";
                }
            }
            set {
                Summa.Core.Database.ChangeItemInfo(FeedUri, Uri, "last_updated", value);
            }
        }
        public string Author {
            get {
                try {
                    string[] item = Summa.Core.Database.GetItem(FeedUri, Uri);
                    return item[4];
                } catch ( Exception e ) {
                    Summa.Core.Log.Exception(e);
                    return "";
                }
            }
            set {
                Summa.Core.Database.ChangeItemInfo(FeedUri, Uri, "author", value);
            }
        }
        public string Tags {
            get {
                try {
                    string[] item = Summa.Core.Database.GetItem(FeedUri, Uri);
                    return item[5];
                } catch ( Exception e ) {
                    Summa.Core.Log.Exception(e);
                    return "";
                }
            }
            set {
                Summa.Core.Database.ChangeItemInfo(FeedUri, Uri, "title", value);
            }
        }
        public string Contents {
            get {
                try {
                    string[] item = Summa.Core.Database.GetItem(FeedUri, Uri);
                    string hack = item[6].Replace("???", "'");
                    hack = hack.Replace("??", "");
                    return hack;
                } catch ( Exception e ) {
                    Summa.Core.Log.Exception(e);
                    return "";
                }
            }
            set {
                Summa.Core.Database.ChangeItemInfo(FeedUri, Uri, "content", value);
            }
        }
        public string EncUri {
            get {
                try {
                    string[] item = Summa.Core.Database.GetItem(FeedUri, Uri);
                    return item[7];
                } catch ( Exception e ) {
                    Summa.Core.Log.Exception(e);
                    return "";
                }
            }
            set {
                Summa.Core.Database.ChangeItemInfo(FeedUri, Uri, "encuri", value);
            }
        }
        public bool Read {
            get {
                try {
                    string[] item = Summa.Core.Database.GetItem(FeedUri, Uri);
                    string val = item[8];
                    if ( val == "True" ) {
                        return true;
                    } else {
                        return false;
                    }
                } catch ( Exception e ) {
                    Summa.Core.Log.Exception(e);
                    return true;
                }
            }
            set {
                if ( value == true ) {
                    Summa.Core.Database.ChangeItemInfo(FeedUri, Uri, "read", "True");
                } else {
                    Summa.Core.Database.ChangeItemInfo(FeedUri, Uri, "read", "False");
                }
            }
        }
        public bool Flagged {
            get {
                try {
                    string[] item = Summa.Core.Database.GetItem(FeedUri, Uri);
                    string val = item[9];
                    if ( val == "True" ) {
                        return true;
                    } else {
                        return false;
                    }
                } catch ( Exception e ) {
                    Summa.Core.Log.Exception(e);
                    return false;
                }
            }
            set {
                if ( value == true ) {
                    Summa.Core.Database.ChangeItemInfo(FeedUri, Uri, "flagged", "True");
                } else {
                    Summa.Core.Database.ChangeItemInfo(FeedUri, Uri, "flagged", "False");
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
