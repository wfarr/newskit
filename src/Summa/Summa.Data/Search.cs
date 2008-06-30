using System;
using System.Collections;

namespace Summa {
    namespace Data {
        public class Search {
            // this is an incredibly weak implementation; it's only here so that
            // we can have "all unread items" and "all flagged items"; support
            // for real searching comes later.
            private string name;
            public string Name {
                get { return name; }
                set { name = value; }
            }
            
            private string SearchType;
            private string Field;
            private string DValue;
            
            public ArrayList GetItems() {
                ArrayList items = new ArrayList();
                
                foreach ( Summa.Data.Feed feed in Summa.Data.Core.GetFeeds() ) {
                    foreach ( Summa.Data.Item item in feed.GetItems() ) {
                        /*switch(field) {
                            case "read":
                                if ( !item.Read ) {
                                    items.Add(item);
                                }
                                break;
                            case "flagged":
                                if ( item.Flagged ) {
                                    items.Add(item);
                                }
                                break;
                            }
                        }*/
                    }
                }
                return items;
            }
            
            public Search(string name) {
                this.name = name;
            }
            
            // searchtypes: LIKE, IS (only IS implemented)
            // field: any field in Summa.Data.Item, as it appears in the Database
            // dvalue: any value of a field in Summa.Data.Item, as it appears in the Database
            public void AddSearchTerm(string searchtype, string field, string dvalue) {
                SearchType = searchtype;
                Field = field;
                DValue = dvalue;
            }
            
            public int GetUnreadCount() {
                int count = 0;
                foreach ( Item item in GetItems() ) {
                    if ( !item.Read ) {
                        count++;
                    }
                }
                return count;
            }
        }
    }
}
