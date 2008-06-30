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

namespace Summa {
    namespace Core {
        public class Updater {
            public bool Updating;
            private bool should_automatic_update;
            private bool enqueued;
            
            private Queue updating_queue;
            
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
                enqueued = false;
                
                updating_queue = new Queue(20);
                
                GLib.Timeout.Add(Summa.Core.Config.GlobalUpdateInterval, new GLib.TimeoutHandler(ScheduledUpdate));
            }
            
            private void UpdateThread() {
                while ( updating_queue.Count != 0 ) {
                    Summa.Data.Feed feed = (Summa.Data.Feed)updating_queue.Dequeue();
                    Updating = true;
                    bool update = feed.Update();
                    
                    foreach ( Summa.Gui.Browser browser in Summa.Core.Application.Browsers ) {
                        Gdk.Threads.Enter();
                        if ( update ) {
                            browser.statusbar.Push(browser.contextid, @"Feed """+feed.Name+@""" has new items.");
                        } else {
                            browser.statusbar.Push(browser.contextid, @"Feed """+feed.Name+@""" has no new items.");
                        }
                        
                        if ( feed.Url == browser.FeedView.Selected.Url ) {
                            browser.ItemView.Update();
                        }
                        
                        browser.contextid++;
                        Gdk.Threads.Leave();
                    }
                }
            }
            
            public void Update() {
                if (!Updating) {
                    should_automatic_update = false;
                    
                    foreach ( Summa.Data.Feed feed in Summa.Data.Core.GetFeeds() ) {
                        updating_queue.Enqueue(feed);
                    }
                
                    System.Threading.Thread updatethread = new System.Threading.Thread(UpdateThread);
                    updatethread.Start();
                }
            }
            
            public bool ScheduledUpdate() {
                if ( !Updating && should_automatic_update ) {
                    foreach ( Summa.Data.Feed feed in Summa.Data.Core.GetFeeds() ) {
                        updating_queue.Enqueue(feed);
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
        }
    }
}
