using System;

using Summa.Core;

namespace Summa.Core {
    public static class StringCatalog {
        // UI strings are static members on this class. When we add l18n, this
        // can be a wrapper for all of that. It also means that there's an
        // encouragement to have fewer UI strings...
        
        public static string UpdatingFeed {
            get { return "Updating feed: {0}"; }
        }
        
        public static string AboutComments {
            get { return "Aggregate and read RSS feeds"; }
        }
        
        public static string AddFeedTitle {
            get { return "Add subscription"; }
        }
        
        public static string AddFeedMessage {
            get { return "<b>Enter the URL of the feed:</b>"; }
        }
    }
}
