// AtomParser.cs
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

namespace Summa.Parser {
    public class AtomParser : Summa.Interfaces.IFeedParser {
        private XmlDocument document;
        private XmlNamespaceManager mgr;
        
        private string name;
        public string Name {
            get {
                try {
                    return name;
                } catch ( Exception e ) {
                    Summa.Core.Log.LogException(e);
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
                    Summa.Core.Log.LogException(e);
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
                    Summa.Core.Log.LogException(e);
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
                    Summa.Core.Log.LogException(e);
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
                    Summa.Core.Log.LogException(e);
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
                    Summa.Core.Log.LogException(e);
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
                    Summa.Core.Log.LogException(e);
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
                    Summa.Core.Log.LogException(e);
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
                    Summa.Core.Log.LogException(e);
                    return "";
                }
            }
            set { favicon = value; }
        }
        
        private ArrayList items;
        public ArrayList Items {
            get {
                try {
                    return items;
                } catch ( Exception e ) {
                    Summa.Core.Log.LogException(e);
                    return new ArrayList();
                }
            }
            set { items = value; }
        }
        
        public AtomParser(string uri, string xml) {
            this.uri = uri;
            this.document = new XmlDocument();
            
            document.LoadXml(xml);
            
            mgr = new XmlNamespaceManager(document.NameTable);
            this.mgr.AddNamespace("atom", "http://www.w3.org/2005/Atom");
            Parse();
        }
        
        public AtomParser(string uri, XmlDocument doc) {
            this.uri = uri;
            this.document = doc;
            this.mgr = new XmlNamespaceManager(document.NameTable);
			this.mgr.AddNamespace("atom", "http://www.w3.org/2005/Atom");
            Parse();
        }
        
        private void Parse() {
            Name = GetXmlNodeText(document, "/atom:feed/atom:title");
            Subtitle = GetXmlNodeText(document, "/atom:feed/atom:subtitle");
            License = GetXmlNodeText(document, "/atom:feed/atom:license");
            Image = GetXmlNodeText(document, "/atom:feed/atom:banner");
            Author = GetXmlNodeText(document, "/atom:feed/atom:author/atom:name");
            
            XmlNodeList nodes = document.SelectNodes("//atom:entry", mgr);
            
            Items = new ArrayList();
            foreach (XmlNode node in nodes) {
                items.Add(ParseItem(node));
            }
        }
        
        private Summa.Parser.Item ParseItem(XmlNode node) {
            Summa.Parser.Item item = new Summa.Parser.Item();
            
            item.Title = GetXmlNodeText(node, "atom:title");
            item.Author = GetXmlNodeText(node, "atom:author/atom:name");
            item.Uri = GetXmlNodeUrl(node, "atom:link");
            item.Contents = GetXmlNodeContent(node);
            item.Date = GetRfc822DateTime(node, "atom:updated").ToString();
            item.LastUpdated = "";
            item.EncUri = "";
            
            return item;
        }
        
        public string GetXmlNodeText(XmlNode node, string tag) {
            XmlNode n = node.SelectSingleNode(tag, mgr);
            return (n == null) ? null : n.InnerText.Trim();
        }
        
        public string GetXmlNodeContent(XmlNode node) {
            XmlNode n = node.SelectSingleNode("atom:content", mgr);
            return (n == null) ? null : n.InnerXml.Trim().Replace("\n", "<br/>\n");
        }
        
        public string GetXmlNodeUrl(XmlNode node, string tag) {
            XmlNode n = node.SelectSingleNode(tag, mgr);
            return (n == null) ? null : (string)n.Attributes.Item(0).Value;
        }
        
        public DateTime GetRfc822DateTime(XmlNode node, string tag) {
            DateTime ret = DateTime.MinValue;
            string result = GetXmlNodeText(node, tag);

            if (!String.IsNullOrEmpty(result)) {
                Migo.Syndication.Rfc822DateTime.TryAtomParse(result, out ret);
            }
                    
            return ret;              
        }
    }
}
