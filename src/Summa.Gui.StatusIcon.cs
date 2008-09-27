// StatusIcon.cs
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
using Gtk;

namespace Summa.Gui {
    public class StatusIcon : Gtk.StatusIcon {
        private int Unread;
        
        public StatusIcon() {
            FromIconName = "summa";
            
            Unread = Summa.Data.Core.GetUnreadCount();
            
            Activate += new EventHandler(ToggleBrowserStatus);
            Summa.Core.Application.Database.ItemChanged += OnItemChanged;
            Summa.Core.Application.Database.ItemAdded += OnItemAdded;
            Summa.Core.Application.Database.ItemDeleted += OnItemDeleted;
            
            UpdateTooltip();
            CheckVisibility();
        }
        
        private void UpdateTooltip() {
            string us = Unread.ToString();
            
            Tooltip = us + " Unread items.";
        }
        
        public void CheckVisibility() {
            if ( Summa.Core.Config.ShowStatusIcon ) {
                bool found = false;
                foreach ( string url in Summa.Core.Config.IconFeedUris ) {
                    Summa.Data.Feed feed = Summa.Data.Core.RegisterFeed(url);
                    
                    if ( feed.HasUnread ) {
                        Visible = true;
                        found = true;
                        break;
                    }
                }
                if ( !found ) {
                    Visible = false;
                }
            } else {
                Visible = false;
            }
        }
        
        private void ToggleBrowserStatus(object obj, EventArgs args) {
            Summa.Core.Application.ToggleShown();
        }
        
        private void OnItemChanged(object obj, Summa.Core.ChangedEventArgs args) {
            if ( args.ItemProperty == "read" ) {
                if ( args.Value == "True" ) {
                    Unread--;
                    UpdateTooltip();
                    CheckVisibility();
                } else if ( args.Value == "False" ) {
                    Unread++;
                    UpdateTooltip();
                    CheckVisibility();
                }
            }
        }
        
        private void OnItemAdded(object obj, Summa.Core.AddedEventArgs args) {
            Unread++;
            UpdateTooltip();
            CheckVisibility();
        }
        
        private void OnItemDeleted(object obj, Summa.Core.AddedEventArgs args) {
            Unread = Summa.Data.Core.GetUnreadCount();
            UpdateTooltip();
            CheckVisibility();
        }
    }
}
