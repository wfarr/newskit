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
                    System.Console.WriteLine(Updating);
                    
                    Summa.Data.Feed feed = (Summa.Data.Feed)updating_queue.Dequeue();
                    Updating = true;
                    bool update = feed.Update();
                    
                    // it should enter the main loop here long enough to refresh the FeedView and ItemView if necessary.
                    System.Console.WriteLine(feed.Name+" has "+update.ToString());
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
