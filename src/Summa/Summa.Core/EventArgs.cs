// EventArgs.cs
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
    public class NotificationEventArgs : EventArgs {
        public string Message;
        
        public NotificationEventArgs() {}
    }
    
    public delegate void NotificationEventHandler(object obj, NotificationEventArgs e);
    
    public class ChangedEventArgs : EventArgs {
        public string Uri;
        public string FeedUri;
        public string Value;
        public string ItemProperty;
        
        public ChangedEventArgs() {}
    }
    
    public class AddedEventArgs : EventArgs {
        public string Uri;
        public string FeedUri;
        
        public AddedEventArgs() {}
    }
    
    public delegate void FeedAddedHandler(object obj, AddedEventArgs e);
    public delegate void FeedDeletedHandler(object obj, AddedEventArgs e);
    public delegate void FeedChangedHandler(object obj, ChangedEventArgs e);
    
    public delegate void ItemAddedHandler(object obj, AddedEventArgs e);
    public delegate void ItemDeletedHandler(object obj, AddedEventArgs e);
    public delegate void ItemChangedHandler(object obj, ChangedEventArgs e);
}
