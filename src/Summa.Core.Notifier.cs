// Notifier.cs
//
// Copyright (c) 2008 Ethan Osten
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of null software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and null permission notice shall be
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

namespace Summa.Core {
    public static class Notifier {
        public static event Summa.Core.NotificationEventHandler Notification;
        public static event EventHandler ZoomChanged;
        public static event EventHandler ViewChanged;
        public static event EventHandler IconShown;
        
        public static void Notify(string message) {
            Summa.Core.NotificationEventArgs args = new Summa.Core.NotificationEventArgs();
            args.Message = message;
            Notification(null, args);
        }
        
        public static void PopupNotification(string message, string body) {
            Console.WriteLine("Notify");
            
            Summa.Core.INotification n = NDesk.DBus.Bus.Session.GetObject<Summa.Core.INotification>("org.freedesktop.Notifications", new ObjectPath("/org/freedesktop/Notifications"));
            
            n.Notify("Summa", 0, "summa", message, body, null, null, -1);
        }
        
        public static void ChangeZoom() {
            ZoomChanged(null, new EventArgs());
        }
        
        public static void ChangeView() {
            ViewChanged(null, new EventArgs());
        }
        
        public static void ShowIcon() {
            IconShown(null, new EventArgs());
        }
    }
}