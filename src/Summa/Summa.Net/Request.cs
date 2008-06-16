using System;
using System.IO;
using System.Text;
using System.Net;

namespace Summa {
    namespace Net {
        public class Request {
            public string Uri;
            
            public HttpStatusCode Status;
            
            public string Etag;
            public string LastModified;
            
            public string Xml;
            
            private HttpWebRequest httprequest;
            private HttpWebResponse httpresponse;
            private Stream stream;
            
            public Request(string uri) {
                Uri = uri;
                httprequest = (HttpWebRequest)WebRequest.Create(uri);
                
                httpresponse = (HttpWebResponse)httprequest.GetResponse();
                
        		byte[] buffer = new byte[8192];
        		StringBuilder sb  = new StringBuilder();
                
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
        		Status = httpresponse.StatusCode;
        		LastModified = httpresponse.LastModified.ToString();
        		try {
            		Etag = httpresponse.Headers.GetValues("ETag")[0];
        		} catch ( Exception e ) {
        		    Summa.Core.Util.Log("Etag not found", e);
    		    }
            }
        }
    }
}
