using System;

using NewsKit;

namespace NewsKit {
    internal static class Globals {
        // you want to customize this class to your program, so that you can
        // set the method for checking whether you're online, the User-Agent,
        // and whatnot.
                        
        // in theory, NewsKit should be independent of Summa. In
        // practice, it isn't there yet. there still exist a number of calls
        // in NewsKit to various things in the Summa namespaces, which may or
        // may not need to be replaced in your program to make things work
        // fully. Comment out the ones you don't want.
        
        public static bool Connected {
            get {
                if ( Summa.Net.NetworkManager.Status() == Summa.Net.ConnectionState.Connected ) {
                    return true;
                } else {
                    return false;
                }
            }
        }
        
        public static string UserAgent {
            get {
                return "Summa/0.0";
            }
        }
        
        public static string DataDirectory {
            get {
                return Mono.Unix.Native.Stdlib.getenv("HOME")+"/.config/summa/";
            }
        }
        
        public static string ImageDirectory {
            get {
                return Mono.Unix.Native.Stdlib.getenv("HOME")+"/.config/summa/icons/";
            }
        }
        
        public static void Exception(Exception e) {
            //Summa.Core.Log.Exception(e);
        }
    }
}
