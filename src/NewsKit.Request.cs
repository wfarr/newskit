using System;
using System.Net;
using System.Text;
using System.IO;

namespace NewsKit {
    public class Request {
        public string Uri;
        
        public HttpStatusCode Status;
        public string LastModified;
        public string Etag;
        
        public string Xml;
        
        private HttpWebRequest webrequest;
        private HttpWebResponse webresponse;
        private Stream stream;
        
        public Request(string uri, string last_modified) {
            Uri = uri;
            
            if ( NewsKit.Globals.Connected ) {
                try {
                    webrequest = (HttpWebRequest)WebRequest.Create(uri);
                    webrequest.AllowAutoRedirect = true;
                    webrequest.UserAgent = NewsKit.Globals.UserAgent;
                    webrequest.MaximumAutomaticRedirections = 4;
                    webrequest.MaximumResponseHeadersLength = 4;
                    
                    try {
                        if ( last_modified != "" ) {
                            DateTime m = Convert.ToDateTime(last_modified);
                            webrequest.IfModifiedSince = m;
                        }
                    } catch ( Exception e ) {
                        NewsKit.Globals.Exception(e);
                    }
                    
                    webresponse = (HttpWebResponse)webrequest.GetResponse();
                    Status = webresponse.StatusCode;
                    
                    byte[] buffer = new byte[8192];
                    StringBuilder sb = new StringBuilder();
                    
                    if ( Status == HttpStatusCode.NotModified ) {
                        throw new NewsKit.Exceptions.NotUpdated();
                    } else {
                        stream = webresponse.GetResponseStream();
                        string tempString = null;
                        int count = 0;
                        
                        do {
                            count = stream.Read(buffer, 0, buffer.Length);
                            
                            if ( count != 0 ) {
                                tempString = Encoding.ASCII.GetString(buffer, 0, count);
                                
                                sb.Append(tempString);
                            }
                        }
                        while (count > 0);
                        
                        Xml = sb.ToString();
                        Xml = System.Text.RegularExpressions.Regex.Replace(Xml, "<( [a-z]+)=([a-zA-Z0-9:/._%;?=&-]+)", "$1=\"$2\"");
                        
                        LastModified = webresponse.LastModified.ToString();
                        try {
                            Etag = webresponse.Headers.GetValues("ETag")[0];
                        } catch ( Exception e ) {
                            NewsKit.Globals.Exception(e);
                        }
                    }
                    webresponse.Close();
                } catch ( System.Net.WebException e ) {
                    NewsKit.Globals.Exception(e);
                    
                    try {
                        webresponse.Close();
                        stream.Close();
                    } catch ( Exception ) {}
                    
                    throw new NewsKit.Exceptions.NotFound();
                }
            } else {
                throw new NewsKit.Exceptions.NotFound();
            }
        }
    }
}
