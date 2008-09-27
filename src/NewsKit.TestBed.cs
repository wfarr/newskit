using System;

namespace NewsKit {
    public static class TestBed {
        public static void Main() {
            IFeedParser p;
            bool found = NewsKit.Core.ParseUri("http://timesonline.typepad.com/dons_life/index.rdf", "", out p);
            
            Console.WriteLine("Feed Name: "+p.Name);
            Console.WriteLine("Feed  URL: "+p.Uri);
            Console.WriteLine("");
            foreach ( NewsKit.Item i in p.Items ) {
                Console.WriteLine("Item Name: "+i.Title);
                Console.WriteLine("Item URI: "+i.Uri);
                Console.WriteLine("Item Date: "+i.Date);
                if ( i.Contents == null ) {
                    Console.WriteLine("Item Contents: No");
                } else {
                    if ( i.Contents.Length > 5 ) {
                        Console.WriteLine("Item Contents: Yes");
                    } else {
                        Console.WriteLine("Item Contents: No");
                    }
                }
                Console.WriteLine("");
            }
        }
    }
}
