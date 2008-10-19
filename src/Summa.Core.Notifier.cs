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
using Notifications;

using Summa.Core;

namespace Summa.Core {
    public static class Notifier {
        public static event NotificationEventHandler NewMessage;
        public static event EventHandler ZoomChanged;
        public static event EventHandler ViewChanged;
        public static event EventHandler IconShown;
        
        public static void Notify(string message) {
            NotificationEventArgs args = new NotificationEventArgs();
            args.Message = message;
            NewMessage("", args);
        }
        
        public static void PopupNotification(string message, string body) {
            Notifications.Notification n = new Notifications.Notification(message, body, "summa");
            if ( Config.ShowStatusIcon ) {
                //n.AttachToStatusIcon(Application.StatusIcon);
            }
            n.Show();
        }
        
        public static void ChangeZoom() {
            ZoomChanged("", new EventArgs());
        }
        
        public static void ChangeView() {
            ViewChanged("", new EventArgs());
        }
        
        public static void ShowIcon() {
            try {
                IconShown("", new EventArgs());
            } catch ( Exception ) {}
        }
    }
}
