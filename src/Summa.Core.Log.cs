// Log.cs
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

using Summa.Core;

namespace Summa.Core {
    public static class Log {
        public static event EventHandler LogAdded;
        public static event EventHandler LogRemoved;
        
        private static LogList log_list;
        
        static Log() {
            log_list = new LogList();
        }
        
        public static void Exception(Exception e) {
            log_list.AddMessage(e.ToString());
            //Log.LogAdded(null, new EventArgs());
        }
        
        public static void Exception(Exception e, string message) {
            log_list.AddMessage(e.ToString()+" with message "+message);
            //Log.LogAdded(null, new EventArgs());
        }
        
        public static void Message(string message) {
            log_list.AddMessage(message);
            //Log.LogAdded(null, new EventArgs());
        }
        
        public static void LogFunc(string log_domain, GLib.LogLevelFlags log_level, string message) {
            Message(message);
        }
        
        public static void RemoveLog() {
            //Log.LogRemoved(null, new EventArgs());
        }
    }
}
