using System;
using System.Collections;
using System.Xml;
using System.Text;

namespace Summa {
    namespace Data {
        namespace Parser {
            public class RssParser : Summa.Data.Parser.FeedParser {
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
                
                public RssParser(string uri, string xml) {
                    this.uri = uri;
                    this.document = new XmlDocument();
                    
                    try {
                        document.LoadXml(xml);
                    } catch (XmlException e) {
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
                    Parse();
                }
                
                public RssParser(string uri, XmlDocument doc) {
                    this.uri = uri;
                    this.document = doc;
                    Parse();
                }
                
                private void Parse() {
                    Name = GetXmlNodeText(document, "/rss/channel/title");
                    Subtitle = GetXmlNodeText(document, "/rss/channel/description");
                    License = GetXmlNodeText(document, "/rss/channel/copyright");
                    Image = GetXmlNodeText(document, "/rss/channel/image/url");
                    Author = GetXmlNodeText(document, "/rss/channel/author");
                    
                    XmlNodeList nodes = document.SelectNodes("//item");
                    
                    Items = new ArrayList();
                    foreach (XmlNode node in nodes) {
                        items.Add(ParseItem(node));
                    }
                }
                
                private Summa.Data.Parser.Item ParseItem(XmlNode node) {
                    Summa.Data.Parser.Item item = new Summa.Data.Parser.Item();
                    
                    item.Title = GetXmlNodeText(node, "title");
                    item.Author = GetXmlNodeText(node, "author");
                    item.Uri = GetXmlNodeText(node, "link");
                    item.Contents = GetXmlNodeText(node, "description");
                    item.Date = GetRfc822DateTime(node, "pubDate").ToString();
                    item.LastUpdated = GetRfc822DateTime(node, "dcterms:modified").ToString();
                    item.EncUri = GetXmlNodeText(node, "enclosure/@url");
                    
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
                        Migo.Syndication.Rfc822DateTime.TryParse(result, out ret);
                    }
                            
                    return ret;              
                }
            }
        }
    }
}

