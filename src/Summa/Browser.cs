///* /home/eosten/Summa/Summa/Browser.cs
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
using Gtk;
using NewsKit;
using GConf;

namespace Summa  {
    public class Browser : Gtk.Window {
        public Gtk.ActionGroup action_group;
        public Gtk.IconFactory factory;
        public Gtk.UIManager uimanager;
        
        public GConf.Client client;
        const string SUMMA_PATH = "/apps/summa";
        const string KEY_LIBNOTIFY = "/apps/summa/show_notifications";
        const string KEY_WIN_WIDTH = "/apps/summa/win_width";
        const string KEY_WIN_HEIGHT = "/apps/summa/win_height";
        const string KEY_MAIN_PANE_POSITION = "/apps/summa/main_pane_pos";
        const string KEY_LEFT_PANE_POSITION = "/apps/summa/left_pane_pos";
        const string KEY_RIGHT_PANE_POSITION = "/apps/summa/right_pane_pos";
        const string KEY_SHOULD_SORT_FEEDVIEW = "/apps/summa/sort_feedview";
        public bool show_notifications;
        
        public Gtk.Table table;
        
        //menus
        public Gtk.Action file_menu;
        public Gtk.Action edit_menu;
        public Gtk.Action subs_menu;
        public Gtk.Action item_menu;
        public Gtk.Action help_menu;
        
        // the file menu
        public Gtk.Action file_AddAction;
        public Gtk.Action import_action;
        public Gtk.Action Up_all_action;
        public Gtk.Action print_action;
        public Gtk.Action print_prev_action;
        public Gtk.Action email_action;
        public Gtk.Action bookmark_action;
        public Summa.Bookmarker bookmarker;
        public Gtk.Action new_win_action;
        public Gtk.Action close_action;
        
        // edit menu
        public Gtk.Action copy_action;
        public Gtk.Action select_all_action;
        public Gtk.Action find_action;
        public Gtk.Action prefs_action;
        
        // subscription menu
        public Gtk.Action update_action;
        public Gtk.Action read_action;
        public Gtk.Action delete_action;
        public Gtk.Action props_action;
        
        // item menu
        public Gtk.Action zoom_in_action;
        public Gtk.Action zoom_out_action;
        public Gtk.Action next_item_action;
        public Gtk.Action prev_item_action;
        public Gtk.Action flag_action;
        
        // help menu
        public Gtk.Action help_action;
        public Summa.AboutDialog about_dialog;
        public Gtk.Action about_action;
        
        public Gtk.Widget menubar;
        public Gtk.Widget toolbar;
        
        // toolbuttons
        public Gtk.Action item_print_action;
        public Gtk.Action item_bookmark_action;
        public Gtk.Action item_zoom_in_action;
        public Gtk.Action item_zoom_out_action;
        
        public Gtk.Paned main_paned;
        public Gtk.Paned left_paned;
        public Gtk.Paned right_paned;
        
        public Summa.TagView tagview;
        public Gtk.ScrolledWindow tagview_swin;
        public Summa.FeedView feedview;
        public Gtk.ScrolledWindow feedview_swin;
        public Summa.ItemView itemview;
        public Gtk.ScrolledWindow itemview_swin;
        
        public Summa.WebKitView htmlview;
        public Gtk.ScrolledWindow htmlview_swin;
        public NewsKit.Item item;
        
        public Gtk.Button connection_button;
        public Gtk.Statusbar statusbar;
        public uint contextid;
        
        // the currently displayed feed
        public NewsKit.Feed curfeed;
        public NewsKit.Item curitem;
        
        // some magic for updates
        private bool should_update;
        
        public Browser() : base(Gtk.WindowType.Toplevel) {
            Title = "Summa";
            
            action_group = new Gtk.ActionGroup("general");
            factory = new Gtk.IconFactory();
            factory.AddDefault();
            
            SetUpActionGroup();
            
            uimanager = new Gtk.UIManager();
            uimanager.InsertActionGroup(action_group, 0);
            AddAccelGroup(uimanager.AccelGroup);
            SetUpUimanager();
            
            table = new Gtk.Table(5, 5, false);
            Add(table);
            
            menubar = uimanager.GetWidget("/MenuBar");
            table.Attach(menubar, 0, 5, 0, 1, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 0, 0);
            
            toolbar = uimanager.GetWidget("/ToolBar");
            table.Attach(toolbar, 0, 5, 1, 2, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 0, 0);
            
            main_paned = new Gtk.HPaned();
            table.Attach(main_paned, 0, 5, 2, 3);
            
            left_paned = new Gtk.VPaned();
            main_paned.Pack1(left_paned, true, true);
            
            tagview = new Summa.TagView();
            tagview.CursorChanged += new EventHandler(TagviewChanged);
            tagview_swin = new Gtk.ScrolledWindow(new Gtk.Adjustment(0, 0, 0, 0, 0, 0), new Gtk.Adjustment(0, 0, 0, 0, 0, 0));
            tagview_swin.Add(tagview);
            tagview_swin.ShadowType = Gtk.ShadowType.In;
            tagview_swin.SetPolicy(Gtk.PolicyType.Automatic, Gtk.PolicyType.Automatic);
            left_paned.Pack1(tagview_swin, true, true);
            
            feedview = new Summa.FeedView();
            feedview.CursorChanged += new EventHandler(FeedviewChanged);
            feedview_swin = new Gtk.ScrolledWindow(new Gtk.Adjustment(0, 0, 0, 0, 0, 0), new Gtk.Adjustment(0, 0, 0, 0, 0, 0));
            feedview_swin.Add(feedview);
            feedview_swin.ShadowType = Gtk.ShadowType.In;
            feedview_swin.SetPolicy(Gtk.PolicyType.Automatic, Gtk.PolicyType.Automatic);
            left_paned.Pack2(feedview_swin, true, true);
            
            right_paned = new Gtk.VPaned();
            main_paned.Pack2(right_paned, true, true);
            
            itemview = new Summa.ItemView();
            itemview.CursorChanged += new EventHandler(ItemviewChanged);
            itemview_swin = new Gtk.ScrolledWindow(new Gtk.Adjustment(0, 0, 0, 0, 0, 0), new Gtk.Adjustment(0, 0, 0, 0, 0, 0));
            itemview_swin.Add(itemview);
            itemview_swin.ShadowType = Gtk.ShadowType.In;
            itemview_swin.SetPolicy(Gtk.PolicyType.Automatic, Gtk.PolicyType.Automatic);
            right_paned.Pack1(itemview_swin, true, true);
            
            htmlview = new Summa.WebKitView();
            htmlview_swin = new Gtk.ScrolledWindow(new Gtk.Adjustment(0, 0, 0, 0, 0, 0), new Gtk.Adjustment(0, 0, 0, 0, 0, 0));
            htmlview_swin.Add(htmlview);
            htmlview_swin.ShadowType = Gtk.ShadowType.In;
            htmlview_swin.SetPolicy(Gtk.PolicyType.Automatic, Gtk.PolicyType.Automatic);
            right_paned.Pack2(htmlview_swin, true, true);
            
            statusbar = new Gtk.Statusbar();
            contextid = 0;
            table.Attach(statusbar, 0, 5, 3, 4, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 0, 0);
            
            about_dialog = new Summa.AboutDialog();
            bookmarker = new Summa.DieuBookmarker();
            //Notify.init("Summa");
            
            GLib.Timeout.Add(3600000, new GLib.TimeoutHandler(ScheduledUpdateAll));
            should_update = true;
            
            client = new GConf.Client();
            UpdateFromGconf();
            //client.Notify(KEY_LIBNOTIFY); //FIXME
            //client.ValueChanged += b => { update_from_gconf(); };
        }
        
        public void TagviewChanged(object obj, EventArgs args) {
            feedview.Sensitive = false;
            feedview.Populate(tagview.Selected);
            feedview.Sensitive = true;
        }
        
        public void FeedviewChanged(object obj, EventArgs args) {
            UpdateName();
            curfeed = feedview.Selected;
            htmlview.Render(curfeed);
            itemview.Populate(curfeed);
        }
        
        public void ItemviewChanged(object obj, EventArgs args) {
            UpdateHtmlview();
        }
        
        public void SetStatusbarText(string text, string text1) {
            statusbar.Push(contextid, text);
            contextid++;
        }
        
        public void ShowAddWindow(object obj, EventArgs args) {
            AddWindow add_dialog = new Summa.AddWindow(this);
            add_dialog.Show();
        }
        
        public void UpdateFromGconf() {
            bool not = (bool)client.Get(KEY_LIBNOTIFY);
            
            //client.AddDir(SUMMA_PATH, GConf.ClientPreloadType.None);
            show_notifications = not;
            
            Resize((int)client.Get(KEY_WIN_WIDTH), (int)client.Get(KEY_WIN_HEIGHT));
        
            main_paned.Position  = (int)client.Get(KEY_MAIN_PANE_POSITION);
            left_paned.Position  = (int)client.Get(KEY_LEFT_PANE_POSITION);
            right_paned.Position  = (int)client.Get(KEY_RIGHT_PANE_POSITION);
            
            feedview.FeedSort = (bool)client.Get(KEY_SHOULD_SORT_FEEDVIEW);
        }
        
        public void CloseWindow(object obj, EventArgs args) {
            /* get dimensions */
            int width;
            int height;
            
            GetSize(out width, out height);
            
            client.Set(KEY_WIN_WIDTH, width);
            client.Set(KEY_WIN_HEIGHT, height);
            
            /* get pane positions */
            int main_size;
            int left_size;
            int right_size;
            
            main_size = main_paned.Position;
            left_size = left_paned.Position;
            right_size = right_paned.Position;
            
            client.Set(KEY_MAIN_PANE_POSITION, main_size);
            client.Set(KEY_LEFT_PANE_POSITION, left_size);
            client.Set(KEY_RIGHT_PANE_POSITION, right_size);
            
            if ( feedview.FeedSort ) {
                client.Set(KEY_SHOULD_SORT_FEEDVIEW, true);
            }
            
            Gtk.Main.Quit();
        }
        
        public void ShowAboutDialog(object obj, EventArgs args) {
            about_dialog.ShowAll();
        }
        
        public void BookmarkItem(object obj, EventArgs args) {
            bookmarker.ShowBookmarkWindow(curitem.Title, curitem.Uri, curitem.Contents, "");
        }
        
        public void GoToNextItem(object obj, EventArgs args) {
            bool win = itemview.GoToNextItem();
            if (!win) {
                bool nextfeed = feedview.GoToNextUnreadFeed();
                
                if ( nextfeed ) {
                    UpdateName();
                    curfeed = feedview.Selected;
                    itemview.Populate(curfeed);
                    htmlview.Render(curfeed);
                    
                    itemview.GoToPreviousItem();
                    UpdateHtmlview();
                } else {
                    statusbar.Push(contextid, "There are no more unread items.");
                    contextid++;
                }
            } else {
                UpdateHtmlview();
            }
        }
        
        public void GoToPreviousItem(object obj, EventArgs args) {
            itemview.GoToPreviousItem();
            UpdateHtmlview();
        }
        
        public void ZoomIn(object obj, EventArgs args) {
            htmlview.ZoomIn();
        }
        
        public void ZoomOut(object obj, EventArgs args) {
            htmlview.ZoomOut();
        }
        
        public void UpdateSelectedFeed(object obj, EventArgs args) {
            bool updated = feedview.Selected.Update();
            
            if ( updated ) {
                feedview.UpdateSelected();
                itemview.Update();
                UpdateName();
                ShowNotification(feedview.Selected);
            }
        }
        
        public void UpdateAll(object obj, EventArgs args) {
            should_update = false;
            UpdateAllPriv();
            GLib.Timeout.Add(3600000, new GLib.TimeoutHandler(ScheduledUpdateAll));
        }
        
        private bool ScheduledUpdateAll() {
            if ( should_update ) {
                UpdateAllPriv();
                return true;
            } else {
                should_update = true;
                return false;
            }
        }
        
        private void UpdateAllPriv() {
            foreach ( NewsKit.Feed feed in NewsKit.Daemon.GetFeeds() ) {
                statusbar.Push(contextid, "Updating feed \""+feed.Name+"\"");
                contextid++;
                while ( Gtk.Application.EventsPending() ) {
                    Gtk.Main.Iteration();
                }
                
                string url = feed.Url;
                bool newitems = feed.Update();
                
                if ( newitems ) {
                    if ( feed.Url == feedview.Selected.Url ) {
                        itemview.Update();
                        UpdateName();
                    }
                    feedview.UpdateFeed(feed);
                    statusbar.Push(contextid, "Feed \""+feed.Name+"\" has new items.");
                    ShowNotification(feed);
                    contextid++;
                    while ( Gtk.Application.EventsPending() ) {
                        Gtk.Main.Iteration();
                    }
                }
            }
            statusbar.Push(contextid, "Update complete.");
            contextid++;
        }
        
        public void ShowNotification(NewsKit.Feed feed) {
            /*if (show_notifications) {
                var not = new Notify.Notification("New feed items", "Feed \""+feed.name+"\" has new unread items.", "internet-news-reader", null);
                not.set_urgency(Notify.Urgency.NORMAL);
                not.show();
            }*/
        }
        
        public void UpdateHtmlview() {
            curitem = itemview.Selected;
            htmlview.Render(curitem);
            
            if ( htmlview.CanPrint() ) {
                item_print_action.Sensitive = true;
            }
            
            if ( bookmarker.CanBookmark() ) {
                item_bookmark_action.Sensitive = true;
            }
            
            if ( htmlview.CanZoom() ) {
                zoom_in_action.Sensitive = true;
                item_zoom_in_action.Sensitive = true;
                zoom_out_action.Sensitive = true;
                item_zoom_out_action.Sensitive = true;
            }
            
            itemview.MarkSelectedRead();
            UpdateName();
            
            if ( curfeed.GetUnreadCount() == 0 ) {
                feedview.UpdateSelected();
            }
        }
        
        public void OnImport(object obj, EventArgs args) {
            Firstrun fr = new Summa.Firstrun(this);
            fr.ShowAll();
        }
        
        public void DeleteFeed(object obj, EventArgs args) {
            NewsKit.Daemon.DeleteFeed(curfeed.Url);
            
            feedview.DeleteFeed(curfeed);
            
            htmlview.Render("");
            itemview.store.Clear();
        }
        
        public void MarkItemFlagged(object obj, EventArgs args) {
            itemview.MarkSelectedFlagged();
        }
        
        public void MarkAllItemsRead(object obj, EventArgs args) {
            curfeed.MarkItemsRead();
            feedview.UpdateSelected();
            itemview.MarkItemsRead();
            UpdateName();
        }
        
        public void UpdateName() {
            Feed feed = feedview.Selected;
            Title = feed.Name+" ("+feed.GetUnreadCount().ToString()+" unread) - Summa";
        }
        
        public void Print(object obj, EventArgs args) {
            htmlview.Print();
        }
        
        public void stub(object obj, EventArgs args) { System.Console.WriteLine("FIXME\n"); }
        
        public void SetUpActionGroup() {
            // menus
            file_menu = new Gtk.Action("FileMenu", "_File", null, null);
            action_group.Add(file_menu);
            edit_menu = new Gtk.Action("EditMenu", "_Edit", null, null);
            action_group.Add(edit_menu);
            subs_menu = new Gtk.Action("SubsMenu", "_Subscription", null, null);
            action_group.Add(subs_menu);
            item_menu = new Gtk.Action("ItemMenu", "_Item", null, null);
            action_group.Add(item_menu);
            help_menu = new Gtk.Action("HelpMenu", "_Help", null, null);
            action_group.Add(help_menu);
            
            // the file menu
            file_AddAction = new Gtk.Action("Add", "_New Subscription", "Create a new feed", Gtk.Stock.Add);
            file_AddAction.Activated += new EventHandler(ShowAddWindow);
            action_group.Add(file_AddAction, "<ctrl>n");
            
            import_action = new Gtk.Action("Import", "_Import", "Import data", null);
            import_action.Activated += new EventHandler(OnImport);
            action_group.Add(import_action);
            
            Up_all_action = new Gtk.Action("Update_all", "_Update all feeds", "Update all feeds", Gtk.Stock.Refresh);
            Up_all_action.Activated += new EventHandler(UpdateAll);
            action_group.Add(Up_all_action, "<ctrl>r");
            
            print_action = new Gtk.Action("Print", "_Print...", "Print the currently selected item", Gtk.Stock.Print);
            print_action.Activated += new EventHandler(Print);
            action_group.Add(print_action, "<ctrl>p");
            
            print_prev_action = new Gtk.Action("Print_preview", "Print previe_w", "Show a preview of the printed document", Gtk.Stock.PrintPreview);
            print_prev_action.Activated += new EventHandler(stub); //FIXME
            print_prev_action.Sensitive = false;
            action_group.Add(print_prev_action, "<shift><ctrl>p");
            
            email_action = new Gtk.Action("Email_link", "_Email this item", "Email a copy of the selected item", null);
            email_action.Activated += new EventHandler(stub); //FIXME
            email_action.Sensitive = false;
            action_group.Add(email_action);
            
            bookmark_action = new Gtk.Action("Bookmark", "_Bookmark this item", "Bookmark this item", null);
            bookmark_action.Activated += new EventHandler(BookmarkItem);
            action_group.Add(bookmark_action, "<ctrl>d");
            
            new_win_action = new Gtk.Action("New_window", "New _window", "Open a new window", null);
            new_win_action.Activated += new EventHandler(stub); //FIXME
            new_win_action.Sensitive = false;
            action_group.Add(new_win_action, "<shift><ctrl>N");
            
            close_action = new Gtk.Action("Close_window", "_Close window", "Close this window", Gtk.Stock.Close);
            close_action.Activated += new EventHandler(CloseWindow);
            action_group.Add(close_action, "<ctrl>W");
            
            // edit menu
            copy_action = new Gtk.Action("Copy", "_Copy", "Copy", Gtk.Stock.Copy);
            copy_action.Activated += new EventHandler(stub); //FIXME
            copy_action.Sensitive = false;
            action_group.Add(copy_action, "<ctrl>C");
            
            select_all_action = new Gtk.Action("Select_all", "_Select all text", "Select all text", null);
            select_all_action.Activated += new EventHandler(stub); //FIXME
            select_all_action.Sensitive = false;
            action_group.Add(select_all_action, "<ctrl>A");
            
            find_action = new Gtk.Action("Find", "_Find...", "Find an item", Gtk.Stock.Find);
            find_action.Activated += new EventHandler(stub); //FIXME
            find_action.Sensitive = false;
            action_group.Add(find_action, "<ctrl>F");
            
            prefs_action = new Gtk.Action("Preferences", "_Preferences", "Preferences for Summa", Gtk.Stock.Preferences);
            prefs_action.Activated += new EventHandler(stub); //FIXME
            prefs_action.Sensitive = false;
            action_group.Add(prefs_action);
            
            // subscription menu
            update_action = new Gtk.Action("Update", "_Update feed", "Update selected feed", Gtk.Stock.Refresh);
            update_action.Activated += new EventHandler(UpdateSelectedFeed);
            action_group.Add(update_action, "<ctrl>u");
            
            read_action = new Gtk.Action("Mark_read", "_Mark feed as read", "Mark all items in the selected feed as read", Gtk.Stock.Apply);
            read_action.Activated += new EventHandler(MarkAllItemsRead);
            action_group.Add(read_action, "<ctrl>m");
            
            delete_action = new Gtk.Action("Delete", "_Delete subscription", "Delete the selected feed", Gtk.Stock.Delete);
            delete_action.Activated += new EventHandler(DeleteFeed);
            action_group.Add(delete_action);
            
            props_action = new Gtk.Action("Properties", "_Properties", "Properties of the selected feed", Gtk.Stock.Properties);
            props_action.Activated += new EventHandler(stub); //FIXME
            props_action.Sensitive = false;
            action_group.Add(props_action);
            
            // item menu
            zoom_in_action = new Gtk.Action("ZoomIn", "_Increase text size", "Increase text size", Gtk.Stock.ZoomIn);
            zoom_in_action.Activated += new EventHandler(ZoomIn);
            zoom_in_action.Sensitive = false;
            action_group.Add(zoom_in_action, null);
            
            zoom_out_action = new Gtk.Action("ZoomOut", "_Decrease text size", "Decrease text size", Gtk.Stock.ZoomOut);
            zoom_out_action.Activated += new EventHandler(ZoomOut);
            zoom_out_action.Sensitive = false;
            action_group.Add(zoom_out_action, null);
            
            next_item_action = new Gtk.Action("Next_item", "_Go to next unread item", "Go to next unread item", Gtk.Stock.GoForward);
            next_item_action.Activated += new EventHandler(GoToNextItem);
            action_group.Add(next_item_action, "n");
            
            prev_item_action = new Gtk.Action("Previous_item", "_Go to the previous item", "Go to the previous item", Gtk.Stock.GoBack);
            prev_item_action.Activated += new EventHandler(GoToPreviousItem);
            action_group.Add(prev_item_action, "j");
            
            flag_action = new Gtk.Action("Flag_item", "_Flag/unflag this item", "Flag/unflag this item", null);
            flag_action.Activated += new EventHandler(MarkItemFlagged);
            action_group.Add(flag_action);
            
            // help menu
            help_action = new Gtk.Action("Contents", "_Contents", "Get help", Gtk.Stock.Help);
            help_action.Activated += new EventHandler(stub); //FIXME
            help_action.Sensitive = false;
            help_action.Sensitive = false;
            action_group.Add(help_action, "F11");
            
            about_action = new Gtk.Action("About", "_About", "About", Gtk.Stock.About);
            about_action.Activated += new EventHandler(ShowAboutDialog);
            action_group.Add(about_action);
            
            // the menubar items
            item_print_action = new Gtk.Action("ItemPrint", "_Print...", "Print the currently selected item", Gtk.Stock.Print);
            item_print_action.Activated += new EventHandler(stub); //FIXME
            item_print_action.Sensitive = false;
            action_group.Add(item_print_action);
            
            IconSet bookmark_iconset = new Gtk.IconSet();
            IconSource bookmark_iconsource = new Gtk.IconSource();
            bookmark_iconsource.IconName = "bookmark-new";
            bookmark_iconset.AddSource(bookmark_iconsource);
            factory.Add("summa-bookmark-new", bookmark_iconset);
            
            item_bookmark_action = new Gtk.Action("ItemBookmark", "_Bookmark this item", "Bookmark this item", "summa-bookmark-new");
            item_bookmark_action.Activated += new EventHandler(BookmarkItem);
            item_bookmark_action.Sensitive = false;
            action_group.Add(item_bookmark_action);
            
            item_zoom_in_action = new Gtk.Action("ItemZoomIn", "_Increase text size", "Increase text size", Gtk.Stock.ZoomIn);
            item_zoom_in_action.Activated += new EventHandler(ZoomIn);
            item_zoom_in_action.Sensitive = false;
            action_group.Add(item_zoom_in_action);
            
            item_zoom_out_action = new Gtk.Action("ItemZoomOut", "_Decrease text size", "Decrease text size", Gtk.Stock.ZoomOut);
            item_zoom_out_action.Activated += new EventHandler(ZoomOut);
            item_zoom_out_action.Sensitive = false;
            action_group.Add(item_zoom_out_action);
        }
        
        public void SetUpUimanager() {
            string ui = @"<ui>
    <menubar name='MenuBar'>
        <menu action='FileMenu'>
        <menuitem action='Add'/>
        <menuitem action='Import'/>
        <separator/>
        <menuitem action='Update_all'/>
        <separator/>
        <menuitem action='Print_preview'/>
        <menuitem action='Print'/>
        <menuitem action='Email_link'/>
        <menuitem action='Bookmark'/>
        <separator/>
        <menuitem action='New_window'/>
        <menuitem action='Close_window'/>
        </menu>
        <menu action='EditMenu'>
        <menuitem action='Copy'/>
        <separator/>
        <menuitem action='Select_all'/>
        <menuitem action='Find'/>
        <separator/>
        <menuitem action='Preferences'/>
        </menu>
        <menu action='SubsMenu'>
        <menuitem action='Update'/>
        <menuitem action='Mark_read'/>
        <menuitem action='Delete'/>
        <menuitem action='Properties'/>
        </menu>
        <menu action='ItemMenu'>
        <menuitem action='ZoomIn'/>
        <menuitem action='ZoomOut'/>
        <separator/>
        <menuitem action='Previous_item'/>
        <menuitem action='Next_item'/>
        <separator/>
        <menuitem action='Flag_item'/>
        </menu>
        <menu action='HelpMenu'>
        <menuitem action='Contents'/>
        <menuitem action='About'/>
        </menu>
    </menubar>
    <toolbar name='ToolBar'>
        <toolitem action='Add'/>
        <separator/>
        <toolitem action='Update_all'/>
        <toolitem action='Mark_read'/>
        <separator/>
        <toolitem action='Previous_item'/>
        <toolitem action='Next_item'/>
        <separator/>
        <toolitem action='ItemPrint'/>
        <toolitem action='ItemBookmark'/>
        <toolitem action='ItemZoomIn'/>
        <toolitem action='ItemZoomOut'/>
    </toolbar>
    </ui>";
            uimanager.AddUiFromString(ui);
        }
    }
}
