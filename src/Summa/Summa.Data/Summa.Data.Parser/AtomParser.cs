using System;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
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
                    
                    Atom.AtomFeed af = Atom.AtomFeed.LoadFromXml(xml);
                    items = new ArrayList();
                    
                    try {
                        Name = af.Title.Text;
                    } catch ( Exception e ) {
                        Summa.Core.Util.Log("Name problem", e);
                        Name = "";
                    }
                    try {
                        Subtitle = af.Subtitle;
                    } catch ( Exception e ) {
                        Summa.Core.Util.Log("Subtitle problem", e);
                        Subtitle = "";
                    }
                    try {
                        Author = af.Author.Name;
                    } catch ( Exception e ) {
                        Summa.Core.Util.Log("Author problem", e);
                        Author = "";
                    }
                    try {
                        Image = af.Icon;
                    } catch ( Exception e ) {
                        Summa.Core.Util.Log("Icon problem", e);
                        Image = "";
                    }
                    try {
                        License = af.License;
                    } catch ( Exception e ) {
                        Summa.Core.Util.Log("License problem", e);
                        License = "";
                    }
                    
                    foreach (Atom.AtomEntry entry in af.Entry) {
                        Summa.Data.Parser.Item item = new Summa.Data.Parser.Item();
                        
                        try {
                            item.Title = entry.Title.Text;
                        } catch ( Exception e ) {
                            Summa.Core.Util.Log("Item Title problem", e);
                            item.Title = "";
                        }
                        try {
                            item.Uri = entry.Link[0].Url;
                        } catch ( Exception e ) {
                            Summa.Core.Util.Log("Item Uri problem", e);
                            item.Uri = "";
                        }
                        try {
                            item.Date = entry.Published.ToString();
                        } catch ( Exception e ) {
                            Summa.Core.Util.Log("Item Date problem", e);
                            item.Date = "";
                        }
                        try {
                            item.LastUpdated = entry.ModifyTime.ToString();
                        } catch ( Exception e ) {
                            Summa.Core.Util.Log("Item Last Updated problem", e);
                            item.LastUpdated = "";
                        }
                        try {
                            item.Author = entry.Author.Name;
                        } catch ( Exception e ) {
                            Summa.Core.Util.Log("Item Author problem", e);
                            item.Author = "";
                        }
                        try {
                            item.Contents = "";
                            foreach ( Atom.AtomText text in entry.Content ) {
                                string str = String.Concat(item.Contents, text.Text);
                                item.Contents = str;
                            }
                        } catch ( Exception e ) {
                            Summa.Core.Util.Log("Item Contents problem", e);
                            item.Contents = "";
                        }
                        try {
                            item.EncUri = entry.LinkByType("enclosure").Url;
                        } catch ( Exception e ) {
                            Summa.Core.Util.Log("Item EncUri problem", e);
                        }
                        
                        try {
                            if ( entry.Summary.Text.Length > item.Contents.Length ) {
                                item.Contents = entry.Summary.Text;
                            }
                        } catch ( Exception e ) {}
                        
                        items.Add(item);
                    }
                }
            }
        }
    }
}

// thanks Blam!
namespace Atom {
    [XmlType("feed")]
    public class AtomFeed {
        [XmlElement("link")] public AtomLink[] Link = null;
        
        [XmlElement("updated")] public DateTime UpdateTime = DateTime.MinValue;
        [XmlElement("modified")] public DateTime ModifyTime = DateTime.MinValue;
        [XmlElement("title")] public AtomText Title = null;
        [XmlElement("subtitle")] public string Subtitle = null;
        [XmlElement("author")] public AtomAuthor Author = null;
        [XmlElement("icon")] public string Icon = null;
        [XmlElement("license")] public string License = null;
        
        [XmlElement("entry")] public AtomEntry[] Entry;
        
        private static XmlSerializer ser = new XmlSerializer(typeof(AtomFeed), "http://www.w3.org/2005/Atom");

        public static AtomFeed LoadFromXml(string xml) {
            System.IO.TextReader xr = new System.IO.StringReader(xml);
            return (AtomFeed)ser.Deserialize(xr);
        }
        
        public DateTime Modified {
            get {
                if(UpdateTime != DateTime.MinValue){
                    return UpdateTime;
                } else {
                    return ModifyTime;
                }
            }
        }
        
        public DateTime Updated {
            get {
                return Modified;
            }
        }

        public AtomLink LinkByType(string type) {
                foreach(AtomLink link in Link){
                    if(link.Type == type){
                        return link;
                    }
                }

            return null;
        }
    }

    [XmlType("author")]
    public class AtomAuthor {
        [XmlElement("name")] public string Name;
        [XmlElement("email")] public string Email;
    }

    [XmlType("link")]
    public class AtomLink {
        [XmlAttribute("href")] public string Url = null;
        [XmlAttribute("rel")] public string Rel = null;
        [XmlAttribute("type")] public string Type = null;
    }

    [XmlType("entry")]
    public class AtomEntry {
        [XmlElement("link")] public AtomLink[] Link = null;
        [XmlElement("published")] public DateTime Published;
        [XmlElement("updated")] public DateTime UpdateTime = DateTime.MinValue;
        [XmlElement("modified")] public DateTime ModifyTime = DateTime.MinValue;
        [XmlElement("title")] public AtomText Title;
        [XmlElement("author")] public AtomAuthor Author = null;
        [XmlElement("id")] public string Id;

        [XmlElement("content")] public AtomText[] Content;
        [XmlElement("summary")] public AtomText Summary;

        public DateTime Modified {
            get {
                if(UpdateTime != DateTime.MinValue){
                    return UpdateTime;
                } else {
                    return ModifyTime;
                }
            }
        }
        
        public DateTime Updated {
            get {
                return Modified;
            }
        }

        public AtomLink LinkByType(string type) {
            foreach(AtomLink link in Link){
                if(link.Rel == type){
                        return link;
                }
            }
            return null;
        }

        public AtomText ContentByType(string type) {
                foreach(AtomText text in Content){
                    if(text.Type == type){
                        return text;
                    }
                }

            return null;
        }
    }

    public class AtomText {
        [XmlText] public string Text = null;
        [XmlAttribute("type")] public string Type = null;
    }
}
