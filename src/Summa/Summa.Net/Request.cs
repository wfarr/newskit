// Request.cs
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
using System.IO;
using System.Text;
using System.Net;

namespace Summa {
    namespace Net {
        public class Request {
            public string Uri;
            
            public HttpStatusCode Status;
            
            private string etag;
            public string Etag {
                get {
                    try {
                        return etag;
                    } catch ( Exception e ) {
                        return "";
                    }
                }
                set { etag = value; }
            }
            
            private string lastmodified;
            public string LastModified {
                get {
                    try {
                        return lastmodified;
                    } catch ( Exception e ) {
                        return "";
                    }
                }
                set { lastmodified = value; }
            }
            
            private string xml;
            public string Xml {
                get {
                    try {
                        return xml;
                    } catch ( Exception e ) {
                        return "";
                    }
                }
                set { xml = value; }
            }
            
            private HttpWebRequest httprequest;
            private HttpWebResponse httpresponse;
            private Stream stream;
            
            public Request(string uri) {
                Uri = uri;
                
                try {
                    httprequest = (HttpWebRequest)WebRequest.Create(uri);
                    httprequest.AllowAutoRedirect = true;
                    
                    try {
                        string modified = Summa.Core.Application.Database.GetFeed(uri)[9];
                        DateTime m = Convert.ToDateTime(modified);
                        httprequest.IfModifiedSince = m;
                    } catch ( Exception e ) {}
                    
                    httpresponse = (HttpWebResponse)httprequest.GetResponse();
                    Status = httpresponse.StatusCode;
                    
                    byte[] buffer = new byte[8192];
                    StringBuilder sb  = new StringBuilder();
                    
                    if ( httpresponse.StatusCode == HttpStatusCode.NotModified ) {
                        Xml = "";
                    } else {
                        stream = httpresponse.GetResponseStream();
                        string tempString = null;
                        int count = 0;
                        
                        do {
                            count = stream.Read(buffer, 0, buffer.Length);
                            
                            if (count != 0) {
                                tempString = Encoding.ASCII.GetString(buffer, 0, count);
                                
                                sb.Append(tempString);
                            }
                        }
                        while (count > 0);
                        
                        Xml = sb.ToString();
                        Xml = System.Text.RegularExpressions.Regex.Replace(Xml, "( [a-z]+)=([a-zA-Z0-9:/._%;?=&-]+)", "$1=\"$2\"");
                        
                        LastModified = httpresponse.LastModified.ToString();
                        try {
                            Etag = httpresponse.Headers.GetValues("ETag")[0];
                        } catch ( Exception e ) {
                            Summa.Core.Util.Log("Etag not found", e);
                        }
                    }
                } catch ( System.Net.WebException e ) {
                    Summa.Core.Util.Log("Failed to download", e);
                    Status = HttpStatusCode.NotFound;
                    return;
                }
            }
        }
    }
}
