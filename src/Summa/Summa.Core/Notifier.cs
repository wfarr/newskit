// Notifier.cs
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

namespace Summa.Core {
    public class Notifier {
        public event Summa.Core.NotificationEventHandler Notification;
        public event EventHandler ZoomChanged;
        public event EventHandler ViewChanged;
        
        public Notifier() {}
        
        public void Notify(string message) {
            Summa.Core.NotificationEventArgs args = new Summa.Core.NotificationEventArgs();
            args.Message = message;
            Notification(this, args);
        }
        
        public void PopupNotification(string message, string body) {
            Console.WriteLine("Notify");
            
            Summa.Interfaces.INotification n = NDesk.DBus.Bus.Session.GetObject<Summa.Interfaces.INotification>("org.freedesktop.Notifications", new ObjectPath("/org/freedesktop/Notifications"));
            
            n.Notify("Summa", 0, "summa", message, body, null, null, -1);
        }
        
        public void ChangeZoom() {
            ZoomChanged(this, new EventArgs());
        }
        
        public void ChangeView() {
            ViewChanged(this, new EventArgs());
        }
    }
}
