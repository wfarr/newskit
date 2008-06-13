using System;
using System.Collections;
using System.Threading;

using GLib;

namespace Summa {
    namespace Core {
        public class Updater {
            private bool updating;
            private bool should_automatic_update;
            
            private Queue updating_queue;
            
            public ArrayList Updating {
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
                updating = false;
                should_automatic_update = true;
                
                updating_queue = new Queue(20);
                
                System.Threading.Thread updatethread = new System.Threading.Thread(UpdateThread);
                updatethread.Start();
                
                GLib.Timeout.Add(Summa.Core.Config.GlobalUpdateInterval, new GLib.TimeoutHandler(ScheduledUpdate));
            }
            
            private void UpdateThread() {
                while ( true ) {
                    if ( updating_queue.Count != 0 ) {
                        updating = true;
                        
                        Summa.Data.Feed feed = (Summa.Data.Feed)updating_queue.Dequeue();
                        bool update = feed.Update();
                        
                        // it should enter the main loop here long enough to refresh the FeedView and ItemView if necessary.
                        System.Console.WriteLine(feed.Name+" has "+update.ToString());
                    } else {
                        updating = false;
                    }
                }
            }
            
            public void Update() {
                if (!updating) {
                    should_automatic_update = false;
                    
                    foreach ( Summa.Data.Feed feed in Summa.Data.Core.GetFeeds() ) {
                        updating_queue.Enqueue(feed);
                    }
                }
            }
            
            public bool ScheduledUpdate() {
                if ( !updating && should_automatic_update ) {
                    foreach ( Summa.Data.Feed feed in Summa.Data.Core.GetFeeds() ) {
                        updating_queue.Enqueue(feed);
                    }
                    
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
