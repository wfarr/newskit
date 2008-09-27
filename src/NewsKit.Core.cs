using System;
using System.Xml;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace NewsKit {
    public static class Core {
        public static bool ParseUri(string uri, string last_modified, out NewsKit.IFeedParser parser) {
            // if no last_modified, put ""
            try {
                Request request = new NewsKit.Request(uri, last_modified);
                
                if ( request.Status != System.Net.HttpStatusCode.NotFound ) {
                    parser = Sniff(request);
                    try {
                        parser.Request = request;
                    } catch ( NullReferenceException e ) {
                        return false;
                    }
                } else {
                    parser = null;
                    return false;
                }
                return true;
            } catch ( NewsKit.Exceptions.NotFound e ) {
                parser = null;
                return false;
            } catch ( NewsKit.Exceptions.NotUpdated e ) {
                parser = null;
                return false;
            }
        }
        
        internal static NewsKit.IFeedParser Sniff(NewsKit.Request request) {
            NewsKit.IFeedParser parser = null;
            
            if ( request.Xml.Contains("<rdf:RDF") ) {
                try {
                    parser = new NewsKit.RdfParser(request.Uri, request.Xml);
                } catch ( Exception e ) {
                    NewsKit.Globals.Exception(e);
                }
            } else if ( request.Xml.Contains("<rss") ) {
                try {
                    parser = new NewsKit.RssParser(request.Uri, request.Xml);
                } catch ( Exception e ) {
                    NewsKit.Globals.Exception(e);
                }
            } else if ( request.Xml.Contains("<feed") ) {
                try {
                    parser = new NewsKit.AtomParser(request.Uri, request.Xml);
                } catch ( Exception e ) {
                    NewsKit.Globals.Exception(e);
                }
            }
            
            if ( parser != null ) {
                if ( parser.Name == null ) {
                    return null;
                } else {
                    return parser;
                }
            } else {
                return null;
            }
        }
        
        internal static bool FindFeed(string website_url, out string feed_url) {
            feed_url = "";
            return false;
        }
        
        public static bool FindFavicon(string website_url, string unique_name, out string feed_uri) {
            try {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ResolveBaseUri(website_url)+"favicon.ico");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                
                if ( response.StatusCode != HttpStatusCode.NotFound ) {
                    Stream stream = response.GetResponseStream();
                    
                    try {
                        System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                        Directory.CreateDirectory(NewsKit.Globals.ImageDirectory);
                        
                        feed_uri = NewsKit.Globals.ImageDirectory + unique_name;
                        //feed_url = "";
                        img.Save(feed_uri, System.Drawing.Imaging.ImageFormat.Png);
                        return true;
                    } catch ( ArgumentException e ) {
                        NewsKit.Globals.Exception(e);
                    }
                }
            } catch ( WebException e ) {
                NewsKit.Globals.Exception(e);
            }  
            feed_uri = "";
            return false;
        }
        
        internal static string ResolveBaseUri(string uri) {
            if ( uri.StartsWith("http://") ) {
                string[] split_uri = uri.Split('/');
                string returi = split_uri[2];
                returi = "http://"+returi+"/";
                return returi;
            } else if ( uri.StartsWith("https://") ) {
                string[] split_uri = uri.Split('/');
                string returi = split_uri[2];
                returi = "https://"+returi+"/";
                return returi;
            } else {
                return uri;
            }
        }
    }
}
