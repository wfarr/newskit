using System;
using System.Xml;

namespace Summa {
    namespace Net {
        public static class Feed {
            public static Summa.Data.Parser.FeedParser Sniff(Summa.Net.Request request) {
                Summa.Data.Parser.FeedParser parser = null;
                
                //try {
                parser = new Summa.Data.Parser.AtomParser(request.Uri, request.Xml);
                return parser;
                //} catch ( Exception e ) {}
                
                if ( parser != null ) {
                    if ( parser.Name == null ) {
                        try {
                            parser = new Summa.Data.Parser.RssParser(request.Uri, request.Xml);
                        } catch ( Exception e ) {}
                    }
                } else {
                    try {
                        parser = new Summa.Data.Parser.RssParser(request.Uri, request.Xml);
                    } catch ( Exception e ) {}
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
        }
    }
}
