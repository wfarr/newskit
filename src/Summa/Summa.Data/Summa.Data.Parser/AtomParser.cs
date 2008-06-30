using System;
using System.Collections;
using System.Xml;
using System.Text;

namespace Summa {
    namespace Data {
        namespace Parser {
            public class AtomParser : Summa.Data.Parser.FeedParser {
                private XmlDocument document;
                private XmlNamespaceManager mgr;
                
                private string name;
                public string Name {
                    get { return name; }
                    set { name = value; }
                }
                private string subtitle;
                public string Subtitle {
                    get { return subtitle; }
                    set { subtitle = value; }
                }
                private string uri;
                public string Uri {
                    get { return uri; }
                    set { uri = value; }
                }
                private string author;
                public string Author {
                    get { return author; }
                    set { author = value; }
                }
                private string image;
                public string Image {
                    get { return image; }
                    set { image = value; }
                }
                private string license;
                public string License {
                    get { return license; }
                    set { license = value; }
                }
                private string etag;
                public string Etag {
                    get { return etag; }
                    set { etag = value; }
                }
                private string modified;
                public string Modified {
                    get { return modified; }
                    set { modified = value; }
                }
                private string favicon;
                public string Favicon {
                    get { return favicon; }
                    set { favicon = value; }
                }
                
                private ArrayList items;
                public ArrayList Items {
                    get { return items; }
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
                
                private Summa.Data.Parser.Item ParseItem(XmlNode node) {
                    Summa.Data.Parser.Item item = new Summa.Data.Parser.Item();
                    
                    item.Title = GetXmlNodeText(node, "atom:title");
                    item.Author = GetXmlNodeText(node, "atom:author/atom:name");
                    item.Uri = GetXmlNodeUrl(node, "atom:link");
                    //item.Contents = node.SelectSingleNode("atom:content", mgr).InnerText;
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
    }
}
