// Application.cs
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

namespace Summa.Core {
    namespace Exceptions {
        // thrown when Update() or RegisterFeed() run into problems
        [Serializable()]
        public class BadFeed : System.Exception {
            public BadFeed() { }
            public BadFeed(string message) { }
            public BadFeed(string message, System.Exception inner) { }
            protected BadFeed(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) { }
        }
        
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
}
