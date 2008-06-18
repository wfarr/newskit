using System;

namespace Summa {
    namespace Core {
        namespace Exceptions {
            [Serializable()]
            public class BadFeed : System.Exception {
                public BadFeed() { }
                public BadFeed(string message) { }
                public BadFeed(string message, System.Exception inner) { }
                protected BadFeed(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) { }
            }
        }
    }
}
