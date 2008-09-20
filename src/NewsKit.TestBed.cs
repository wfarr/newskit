using System;

namespace NewsKit {
    public static class TestBed {
        public static void Main() {
            IFeedParser p;
            bool found = NewsKit.Core.ParseUri("http://planet.gnome.org/rss20.xml", out p);
            
            Console.WriteLine("Feed Name: "+p.Name);
            foreach ( NewsKit.Item i in p.Items ) {
                Console.WriteLine("Item Name: "+i.Title);
            }
        }
    }
}
