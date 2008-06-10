///* /home/eosten/Summa/Summa/Summa.Data.cs
// *
// * Copyright (C) 2008  Ethan Osten
// *
// * This library is free software: you can redistribute it and/or modify
// * it under the terms of the GNU Lesser General Public License as published by
// * the Free Software Foundation, either version 2.1 of the License, or
// * (at your option) any later version.
// *
// * This library is distributed in the hope that it will be useful,
// * but WITHOUT ANY WARRANTY; without even the implied warranty of
// * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// * GNU Lesser General Public License for more details.
// *
// * You should have received a copy of the GNU Lesser General Public License
// * along with this library.  If not, see <http://www.gnu.org/licenses/>.
// *
// * Author:
// *     Ethan Osten <senoki@gmail.com>
// */
//

using System;
using System.Collections;
using NDesk.DBus;

[Interface ("org.gnome.NewsKit")]
public interface NewsKitObj {
    string[] GetTags ();
    int GetUnreadCount ();
    void DeleteSearch (string name);
    string[] GetFeedsByTag (string tag);
    void AppInit (string name);
    void SetConnected (bool value);
    string[] GetFeeds ();
    string RegisterFeedSourceWithAuth(string uri, string username, string password);
    void DeleteFeedSource (string uri);
    string[] GetSearches ();
    string[] ImportOPML (string uri);
    string RegisterSearchSource (string name);
    bool GetConnected ();
    string RegisterFeedSource (string uri);
    event NewFeedHandler NewFeed;
    event FeedDeletedHandler FeedDeleted;
}
public delegate void NewFeedHandler();
public delegate void FeedDeletedHandler();

[Interface ("org.gnome.NewsKit.Feeds")]
public interface NewsKitFeed {
    string[] GetFullItem (string url);
    string GetItemProperty (string url, string property);
    int GetUnreadCount ();
    bool Update ();
    string[] GetItems ();
    void SetProperty (string property, string val);
    string GetProperty (string property);
    void MarkItemsRead ();
    void SetItemProperty (string url, string property, string val);
    event NoNewItemsHandler NoNewItems;
    event ItemChangedHandler ItemChanged;
    event NewItemsHandler NewItems;
    event NameChangedHandler NameChanged;
    event TagsChangedHandler TagsChanged;
}
public delegate void NoNewItemsHandler();
public delegate void ItemChangedHandler();
public delegate void NewItemsHandler();
public delegate void NameChangedHandler();
public delegate void TagsChangedHandler();

namespace Summa {
    namespace Data {
        public static class Core {
            static NewsKitObj daemon = Bus.Session.GetObject<NewsKitObj>("org.gnome.NewsKit", new ObjectPath("/org/gnome/NewsKit"));
            
            public static void ApplicationInit(string name) {
                daemon.AppInit(name);
            }
            
            public static Summa.Data.Feed RegisterFeed(string uri) {
                string uid = daemon.RegisterFeedSource(uri);
                
                return new Summa.Data.Feed(uid);
            }
            
            public static Summa.Data.Feed RegisterFeed(string uri, string username, string password) {
                string uid = daemon.RegisterFeedSourceWithAuth(uri, username, password);
                
                return new Summa.Data.Feed(uid);
            }
            
            public static void DeleteFeed(string uri) {
                daemon.DeleteFeedSource(uri);
            }
            
            public static ArrayList GetFeeds() {
                string[] uids = daemon.GetFeeds();
                ArrayList feeds = new ArrayList();
                
                foreach (string uid in uids) {
                    feeds.Add(new Summa.Data.Feed(uid));
                }
                return feeds;
            }
            
            public static ArrayList GetFeeds(string tag) {
                string[] uids = daemon.GetFeedsByTag(tag);
                ArrayList feeds = new ArrayList();
                
                foreach (string uid in uids) {
                    feeds.Add(new Summa.Data.Feed(uid));
                }
                return feeds;
            }
            
            public static string[] GetTags() {
                return daemon.GetTags();
            }
            
            public static int GetUnreadCount() {
                return daemon.GetUnreadCount();
            }
            
            public static string[] ImportOpml(string filename) {
                return daemon.ImportOPML(filename);
            }
            
            // FIXME - should have the Search stuff
        }
    }
}
