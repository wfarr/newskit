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

using Summa.Core;
using Summa.Data;
using Summa.Gui;

namespace Summa.Gui {
    public class StatusIcon : Gtk.StatusIcon {
        private int Unread;
        
        public StatusIcon() {
            FromIconName = "summa";
            
            Unread = Feeds.GetUnreadCount();
            
            Activate += new EventHandler(ToggleBrowserStatus);
            Database.ItemChanged += OnItemChanged;
            Database.ItemAdded += OnItemAdded;
            Database.ItemDeleted += OnItemDeleted;
            
            UpdateTooltip();
            CheckVisibility();
        }
        
        private void UpdateTooltip() {
            string us = Unread.ToString();
            
            Tooltip = us + " Unread items.";
        }
        
        public void CheckVisibility() {
            if ( Config.ShowStatusIcon ) {
                bool found = false;
                foreach ( string url in Config.IconFeedUris ) {
                    Feed feed = Feeds.RegisterFeed(url);
                    
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
        
        private void OnItemChanged(object obj, ChangedEventArgs args) {
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
        
        private void OnItemAdded(object obj, AddedEventArgs args) {
            Unread++;
            UpdateTooltip();
            CheckVisibility();
        }
        
        private void OnItemDeleted(object obj, AddedEventArgs args) {
            Unread = Feeds.GetUnreadCount();
            UpdateTooltip();
            CheckVisibility();
        }
    }
}
