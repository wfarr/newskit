using System;
using System.Collections;

using NewsKit;

namespace NewsKit {
    public interface IFeedParser {
        string Name {get; set;}
        string Subtitle {get; set;}
        string Uri {get; set;}
        string Author {get; set;}
        string Image {get; set;}
        string License {get; set;}
        string Etag {get; set;}
        string Modified {get; set;}
        string Favicon {get; set;}
        
        Request Request {get; set;}
        
        ArrayList Items {get; set;}
    }
}
