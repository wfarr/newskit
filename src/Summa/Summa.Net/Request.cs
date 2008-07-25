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

namespace Summa.Net {
    public class Request {
        public string Uri;
        public HttpStatusCode Status;
        public string Etag;
        public string LastModified;
        public string Xml;
        
        private HttpWebRequest webrequest;
        private HttpWebResponse webresponse;
        private Stream stream;
        
        public Request(string uri) {
            if ( Summa.Core.Config.Connected ) {
                this.Uri = uri;
                
                try {
                    webrequest = (HttpWebRequest)WebRequest.Create(uri);
                    webrequest.AllowAutoRedirect = true;
                    
                    try {
                        string modified = Summa.Core.Application.Database.GetFeed(uri)[9];
                        DateTime m = Convert.ToDateTime(modified);
                        webrequest.IfModifiedSince = m;
                    } catch ( Exception e ) {
                        Summa.Core.Log.Exception(e);
                    }
                    
                    webresponse = (HttpWebResponse)webrequest.GetResponse();
                    Status = webresponse.StatusCode;
                    
                    byte[] buffer = new byte[8192];
                    StringBuilder sb  = new StringBuilder();
                    
                    if ( Status == HttpStatusCode.NotModified ) {
                        throw new Summa.Core.Exceptions.NotUpdated();
                    } else {
                        stream = webresponse.GetResponseStream();
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
                        
                        LastModified = webresponse.LastModified.ToString();
                        try {
                            Etag = webresponse.Headers.GetValues("ETag")[0];
                        } catch ( Exception e ) {
                            Summa.Core.Log.Exception(e, "Etag not found");
                        }
                    }
                } catch ( System.Net.WebException e ) {
                    Summa.Core.Log.Exception(e, "Failed to download");
                    throw new Summa.Core.Exceptions.NotFound();
                }
            } else {
                throw new Summa.Core.Exceptions.NotFound();
            }
        }
    }
}
