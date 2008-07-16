// Updater.cs
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
using System.Collections;
using System.Threading;

using GLib;

namespace Summa.Core {
    public class Updater {
        public bool Updating;
        private bool should_automatic_update;
        
        private ArrayList updating_queue;
        
        private Summa.Interfaces.ISource updating_feed;
        private string adding_url;
        
        public ArrayList FeedsUpdating {
            get {
                Summa.Data.Feed[] array = (Summa.Data.Feed[])updating_queue.ToArray();
                ArrayList retval = new ArrayList();
                
                foreach ( Summa.Data.Feed feed in array ) {
                    retval.Add(feed);
                }
                
                return retval;
            }
        }
        
        public Updater() {
            Updating = false;
            should_automatic_update = true;
            
            updating_queue = new ArrayList();
            
            GLib.Timeout.Add(Summa.Core.Config.GlobalUpdateInterval, new GLib.TimeoutHandler(ScheduledUpdate));
        }
        
        private void UpdateThread() {
            foreach ( Summa.Data.Feed feed in updating_queue ) {
                Updating = true;
                bool update = false;
                
                try {
                    Summa.Core.NotificationEventArgs fargs = new Summa.Core.NotificationEventArgs();
                    fargs.Message = "Updating feed: "+feed.Name;
                    Gtk.Application.Invoke(this, fargs, OnNotify);
                    
                    update = feed.Update();
                    
                    if ( update ) {
                        Summa.Core.NotificationEventArgs args = new Summa.Core.NotificationEventArgs();
                        args.Message = feed.Name+" has new items.";
                        Gtk.Application.Invoke(this, args, OnNotify);
                    } else {
                        Summa.Core.NotificationEventArgs args = new Summa.Core.NotificationEventArgs();
                        args.Message = feed.Name+" has no new items.";
                        Gtk.Application.Invoke(this, args, new EventHandler(OnNotify));
                    }
                } catch ( NullReferenceException ) {}
            }
        }
        
        private void OnNotify(object obj, EventArgs args) {
            Summa.Core.NotificationEventArgs iargs = (Summa.Core.NotificationEventArgs)args;
            Summa.Core.Application.Notifier.Notify(iargs.Message);
        }
        
        public void Update() {
            if (!Updating) {
                should_automatic_update = false;
                
                foreach ( Summa.Data.Feed feed in Summa.Data.Core.GetFeeds() ) {
                    updating_queue.Add(feed);
                }
            
                System.Threading.Thread updatethread = new System.Threading.Thread(UpdateThread);
                updatethread.Start();
            }
        }
        
        private void UpdateFeedThread() {
            try {
                Summa.Core.NotificationEventArgs fargs = new Summa.Core.NotificationEventArgs();
                fargs.Message = "Updating feed: "+updating_feed.Name;
                Gtk.Application.Invoke(this, fargs, OnNotify);
                
                bool update = updating_feed.Update();
                
                if ( update ) {
                    Summa.Core.NotificationEventArgs args = new Summa.Core.NotificationEventArgs();
                    args.Message = updating_feed.Name+" has new items.";
                    Gtk.Application.Invoke(this, args, OnNotify);
                } else {
                    Summa.Core.NotificationEventArgs args = new Summa.Core.NotificationEventArgs();
                    args.Message = updating_feed.Name+" has no new items.";
                    Gtk.Application.Invoke(this, args, new EventHandler(OnNotify));
                }
            } catch ( NullReferenceException ) {}
        }
        
        public void UpdateFeed(Summa.Interfaces.ISource feed) {
            updating_feed = feed;
            System.Threading.Thread updatethread = new System.Threading.Thread(UpdateFeedThread);
            updatethread.Start();
        }
        
        public bool ScheduledUpdate() {
            if ( !Updating && should_automatic_update ) {
                foreach ( Summa.Data.Feed feed in Summa.Data.Core.GetFeeds() ) {
                    updating_queue.Add(feed);
                }
            
                System.Threading.Thread updatethread = new System.Threading.Thread(UpdateThread);
                updatethread.Start();
                
                return true;
            } else {
                should_automatic_update = true;
                
                GLib.Timeout.Add(Summa.Core.Config.GlobalUpdateInterval, new GLib.TimeoutHandler(ScheduledUpdate));
                
                return false;
            }
        }
        
        private void AddFeedThread() {
            Summa.Core.NotificationEventArgs fargs = new Summa.Core.NotificationEventArgs();
            fargs.Message = "Adding feed "+adding_url;
            Gtk.Application.Invoke(this, fargs, OnNotify);
            
            try {
                Summa.Data.Core.RegisterFeed(adding_url);
                
                fargs = new Summa.Core.NotificationEventArgs();
                fargs.Message = "Successfully added feed "+adding_url;
                Gtk.Application.Invoke(this, fargs, OnNotify);
            } catch ( Exception e ) {
                Summa.Core.Log.Exception(e);
                
                fargs = new Summa.Core.NotificationEventArgs();
                fargs.Message = "Adding feed "+adding_url+" failed";
                Gtk.Application.Invoke(this, fargs, OnNotify);
            }
        }
        
        public void AddFeed(string feedurl) {
            adding_url = feedurl;
            
            System.Threading.Thread addthread = new System.Threading.Thread(AddFeedThread);
            addthread.Start();
        }
    }
}
