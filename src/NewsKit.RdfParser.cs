// RdfParser.cs
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
using System.Xml;
using System.Text;

using NewsKit;

namespace NewsKit {
    public class RdfParser : IFeedParser {
        private XmlDocument document;
        private XmlNamespaceManager mgr;
        
        private string name;
        public string Name {
            get {
                try {
                    return name;
                } catch ( Exception e ) {
                    Globals.Exception(e);
                    return "";
                }
            }
            set { name = value; }
        }
        private string subtitle;
        public string Subtitle {
            get {
                try {
                    return subtitle;
                } catch ( Exception e ) {
                    Globals.Exception(e);
                    return "";
                }
            }
            set { subtitle = value; }
        }
        private string uri;
        public string Uri {
            get {
                try {
                    return uri;
                } catch ( Exception e ) {
                    Globals.Exception(e);
                    return "";
                }
            }
            set { uri = value; }
        }
        private string author;
        public string Author {
            get {
                try {
                    return author;
                } catch ( Exception e ) {
                    Globals.Exception(e);
                    return "";
                }
            }
            set { author = value; }
        }
        private string image;
        public string Image {
            get {
                try {
                    return image;
                } catch ( Exception e ) {
                    Globals.Exception(e);
                    return "";
                }
            }
            set { image = value; }
        }
        private string license;
        public string License {
            get {
                try {
                    return license;
                } catch ( Exception e ) {
                    Globals.Exception(e);
                    return "";
                }
            }
            set { license = value; }
        }
        private string etag;
        public string Etag {
            get {
                try {
                    return etag;
                } catch ( Exception e ) {
                    Globals.Exception(e);
                    return "";
                }
            }
            set { etag = value; }
        }
        private string modified;
        public string Modified {
            get {
                try {
                    return modified;
                } catch ( Exception e ) {
                    Globals.Exception(e);
                    return "";
                }
            }
            set { modified = value; }
        }
        private string favicon;
        public string Favicon {
            get {
                try {
                    return favicon;
                } catch ( Exception e ) {
                    Globals.Exception(e);
                    return "";
                }
            }
            set { favicon = value; }
        }
        
        private Request request;
        public Request Request {
            get {
                return request;
            } set {
                request = value;
            }
        }
        
        private ArrayList items;
        public ArrayList Items {
            get {
                try {
                    return items;
                } catch ( Exception e ) {
                    Globals.Exception(e);
                    return new ArrayList();
                }
            }
            set { items = value; }
        }
        
        public RdfParser(string uri, string xml) {
            this.uri = uri;
            this.document = new XmlDocument();
            
            try {
                document.LoadXml(xml);
            } catch (XmlException e) {
                Globals.Exception(e);
                bool have_stripped_control = false;
                StringBuilder sb = new StringBuilder ();

                foreach (char c in xml) {
                    if (Char.IsControl(c) && c != '\n') {
                        have_stripped_control = true;
                    } else {
                        sb.Append(c);
                    }
                }

                bool loaded = false;
                if (have_stripped_control) {
                    try {
                        document.LoadXml(sb.ToString ());
                        loaded = true;
                    } catch (Exception) {
                    }
                }

                if (!loaded) {                              
                }
            }
            mgr = new XmlNamespaceManager(document.NameTable);
            mgr.AddNamespace("rss10", "http://purl.org/rss/1.0/");
            mgr.AddNamespace("content", "http://purl.org/rss/1.0/modules/content/");
            mgr.AddNamespace("dc", "http://purl.org/dc/elements/1.1/");
            
            XmlNodeList channodes = document.SelectNodes("//rss10:channel", mgr);
            foreach ( XmlNode node in channodes ) {
                Name = GetXmlNodeText(node, "rss10:title");
                Subtitle = GetXmlNodeText(node, "rss10:description");
            }
            
            XmlNodeList nodes = document.SelectNodes("//rss10:item", mgr);
            
            Items = new ArrayList();
            foreach (XmlNode node in nodes) {
                items.Add(ParseItem(node));
            }
        }
        
        private Item ParseItem(XmlNode node) {
            Item item = new Item();
            
            item.Title = GetXmlNodeText(node, "rss10:title");
            item.Author = GetXmlNodeText(node, "rss10:author");
            item.Uri = GetXmlNodeText(node, "rss10:link");
            item.Contents = GetXmlNodeText(node, "rss10:description");
            Console.WriteLine(item.Contents);
            try {
                if ( item.Contents.Length < GetXmlNodeText(node, "content:encoded").Length ) {
                    item.Contents = GetXmlNodeText(node, "content:encoded");
                }
            } catch ( Exception ) {
                if ( String.IsNullOrEmpty(item.Contents) ) {
                    item.Contents = GetXmlNodeText(node, "content:encoded");
                }
            }
            item.Date = Convert.ToDateTime(GetXmlNodeText(node, "dc:date")).ToString();
            
            item.LastUpdated = GetRfc822DateTime(node, "dcterms:modified").ToString();
            
            
            return item;
        }
        
        public string GetXmlNodeText(XmlNode node, string tag) {
            XmlNode n = node.SelectSingleNode(tag, mgr);
            return (n == null) ? null : n.InnerText.Trim();
        }
        
        public DateTime GetRfc822DateTime(XmlNode node, string tag) {
            DateTime ret = DateTime.MinValue;
            string result = GetXmlNodeText(node, tag);

            if (!String.IsNullOrEmpty(result)) {
                ret = RssCommon.Parse(result);
            }
                    
            return ret;              
        }
    }
}

