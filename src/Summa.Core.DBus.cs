// AtomParser.cs
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
using NDesk.DBus;
using org.freedesktop.DBus;

namespace Summa.Core {
    [Interface("org.gnome.feed.Reader")]
    public interface SummaDBus { // make sure to keep up with Liferea
        bool Subscribe(string url);
        bool Ping();
        bool SetOnline(bool online);
        int GetUnreadItems();
        int GetNewItems();
    }
    
    public class DBusInterface : SummaDBus {
        public string BusName = "org.gnome.feed.Reader";
        public string ObjPath = "/org/gnome/feed/Reader";
        
        public DBusInterface() {
            NDesk.DBus.BusG.Init();
            
            Bus.Session.RequestName(BusName);
            Bus.Session.Register(new ObjectPath(ObjPath), this);
        }
        
        public bool Subscribe(string url) {
            Summa.Data.Core.RegisterFeed(url);
            
            return true;
        }
        
        public bool Ping() {
            return true;
        }
        
        public bool SetOnline(bool online) {
            Summa.Core.Config.Connected = online;
            return Summa.Core.Config.Connected;
        }
        
        public int GetUnreadItems() {
            return Summa.Data.Core.GetUnreadCount();
        }
        
        public int GetNewItems() {
            return Summa.Data.Core.GetUnreadCount();
        }
    }
}
