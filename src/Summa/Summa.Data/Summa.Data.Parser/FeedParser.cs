using System;
using System.Collections;

namespace Summa {
    namespace Data {
        namespace Parser {
            public interface FeedParser {
                string Name {get; set;}
                string Subtitle {get; set;}
                string Uri {get; set;}
                string Author {get; set;}
                string Image {get; set;}
                string License {get; set;}
                string Etag {get; set;}
                string Modified {get; set;}
                string Favicon {get; set;}
                
                ArrayList Items {get; set;}
            }
        }
    }
}
