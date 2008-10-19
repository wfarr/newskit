using System;

using NewsKit;

namespace NewsKit.Exceptions {
// for failures to download
    [Serializable()]
    public class NotFound : System.Exception {
        public NotFound() { }
        public NotFound(string message) { }
        public NotFound(string message, System.Exception inner) { }
        protected NotFound(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) { }
    }
    
    // for feeds that have not been updated in Request
    [Serializable()]
    public class NotUpdated : System.Exception {
        public NotUpdated() { }
        public NotUpdated(string message) { }
        public NotUpdated(string message, System.Exception inner) { }
        protected NotUpdated(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) { }
    }
}
