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

namespace Summa  {
    namespace Gui {
        public class Browser : Gtk.Window {
            public Gtk.ActionGroup action_group;
            public Gtk.IconFactory factory;
            public Gtk.UIManager uimanager;
            
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
            public Summa.Core.Bookmarker bookmarker;
            public Gtk.Action new_win_action;
            public Gtk.Action close_action;
            
            // edit menu
            public Gtk.Action copy_action;
            public Gtk.Action select_all_action;
            public Gtk.Action find_action;
            public Gtk.Action prefs_action;
            public Summa.Gui.ConfigDialog config_dialog;
            
            // subscription menu
            public Gtk.Action update_action;
            public Gtk.Action read_action;
            public Gtk.Action delete_action;
            public Gtk.Action props_action;
            public Gtk.Action tags_action;
            
            // item menu
            public Gtk.Action zoom_in_action;
            public Gtk.Action zoom_out_action;
            public Gtk.Action next_item_action;
            public Gtk.Action prev_item_action;
            public Gtk.Action flag_action;
            public Gtk.Action play_action;
            public Summa.Core.MediaPlayer mediaplayer;
            
            // help menu
            public Gtk.Action help_action;
            public Summa.Gui.AboutDialog about_dialog;
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
            
            public Summa.Gui.TagView TagView;
            public Gtk.ScrolledWindow TagView_swin;
            public Summa.Gui.FeedView FeedView;
            public Gtk.ScrolledWindow FeedView_swin;
            public Summa.Gui.ItemView ItemView;
            public Gtk.ScrolledWindow ItemView_swin;
            
            public Summa.Gui.WebKitView HtmlView;
            public Gtk.ScrolledWindow HtmlView_swin;
            public Summa.Data.Item item;
            
            public Gtk.Button connection_button;
            public Gtk.Statusbar statusbar;
            public uint contextid;
            
            // the currently displayed feed
            public Summa.Data.Feed curfeed;
            public Summa.Data.Item curitem;
            
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
                
                TagView = new Summa.Gui.TagView();
                TagView.CursorChanged += new EventHandler(TagviewChanged);
                TagView_swin = new Gtk.ScrolledWindow(new Gtk.Adjustment(0, 0, 0, 0, 0, 0), new Gtk.Adjustment(0, 0, 0, 0, 0, 0));
                TagView_swin.Add(TagView);
                TagView_swin.ShadowType = Gtk.ShadowType.In;
                TagView_swin.SetPolicy(Gtk.PolicyType.Automatic, Gtk.PolicyType.Automatic);
                left_paned.Pack1(TagView_swin, true, true);
                
                FeedView = new Summa.Gui.FeedView();
                FeedView.CursorChanged += new EventHandler(FeedviewChanged);
                FeedView_swin = new Gtk.ScrolledWindow(new Gtk.Adjustment(0, 0, 0, 0, 0, 0), new Gtk.Adjustment(0, 0, 0, 0, 0, 0));
                FeedView_swin.Add(FeedView);
                FeedView_swin.ShadowType = Gtk.ShadowType.In;
                FeedView_swin.SetPolicy(Gtk.PolicyType.Automatic, Gtk.PolicyType.Automatic);
                left_paned.Pack2(FeedView_swin, true, true);
                
                right_paned = new Gtk.VPaned();
                main_paned.Pack2(right_paned, true, true);
                
                ItemView = new Summa.Gui.ItemView();
                ItemView.CursorChanged += new EventHandler(ItemviewChanged);
                ItemView_swin = new Gtk.ScrolledWindow(new Gtk.Adjustment(0, 0, 0, 0, 0, 0), new Gtk.Adjustment(0, 0, 0, 0, 0, 0));
                ItemView_swin.Add(ItemView);
                ItemView_swin.ShadowType = Gtk.ShadowType.In;
                ItemView_swin.SetPolicy(Gtk.PolicyType.Automatic, Gtk.PolicyType.Automatic);
                right_paned.Pack1(ItemView_swin, true, true);
                
                statusbar = new Gtk.Statusbar();
                contextid = 0;
                table.Attach(statusbar, 0, 5, 3, 4, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 0, 0);
                
                HtmlView = new Summa.Gui.WebKitView();
                HtmlView_swin = new Gtk.ScrolledWindow(new Gtk.Adjustment(0, 0, 0, 0, 0, 0), new Gtk.Adjustment(0, 0, 0, 0, 0, 0));
                HtmlView_swin.Add(HtmlView);
                HtmlView_swin.ShadowType = Gtk.ShadowType.In;
                HtmlView_swin.SetPolicy(Gtk.PolicyType.Automatic, Gtk.PolicyType.Automatic);
                right_paned.Pack2(HtmlView_swin, true, true);
                
                about_dialog = new Summa.Gui.AboutDialog();
                config_dialog = new Summa.Gui.ConfigDialog();
                bookmarker = new Summa.Gui.DieuBookmarker();
                mediaplayer = new Summa.Gui.TotemMediaPlayer();
                //Notify.init("Summa");
                
                UpdateFromConfig();
                
                //client.Notify(KEY_LIBNOTIFY); //FIXME
                //client.ValueChanged += b => { update_from_gconf(); };
            }
            
            public void TagviewChanged(object obj, EventArgs args) {
                FeedView.Sensitive = false;
                FeedView.Populate(TagView.Selected);
                FeedView.Sensitive = true;
            }
            
            public void FeedviewChanged(object obj, EventArgs args) {
                UpdateName();
                curfeed = FeedView.Selected;
                play_action.Sensitive = false;
                HtmlView.Render(curfeed);
                ItemView.Populate(curfeed);
            }
            
            public void ItemviewChanged(object obj, EventArgs args) {
                UpdateHtmlview();
                play_action.StockId = Gtk.Stock.MediaPlay;
                
                if ( ItemView.Selected.EncUri != "" ) {
                    play_action.Sensitive = true;
                } else {
                    play_action.Sensitive = false;
                }
            }
            
            public void SetStatusbarText(string text, string text1) {
                statusbar.Push(contextid, text);
                contextid++;
            }
            
            public void ShowAddWindow(object obj, EventArgs args) {
                AddWindow add_dialog = new Summa.Gui.AddWindow();
                add_dialog.Show();
            }
            
            public void UpdateFromConfig() {
                Resize(Summa.Core.Config.WindowWidth, Summa.Core.Config.WindowHeight);
            
                main_paned.Position  = Summa.Core.Config.MainPanePosition;
                left_paned.Position  = Summa.Core.Config.LeftPanePosition;
                right_paned.Position  = Summa.Core.Config.RightPanePosition;
            }
            
            public void CloseWindow(object obj, EventArgs args) {
                /* get dimensions */
                int width;
                int height;
                
                GetSize(out width, out height);
                
                Summa.Core.Config.WindowWidth = width;
                Summa.Core.Config.WindowHeight = height;
                
                /* get pane positions */
                int main_size;
                int left_size;
                int right_size;
                
                main_size = main_paned.Position;
                left_size = left_paned.Position;
                right_size = right_paned.Position;
                
                Summa.Core.Config.MainPanePosition = main_size;
                Summa.Core.Config.LeftPanePosition = left_size;
                Summa.Core.Config.RightPanePosition = right_size;
                
                Gtk.Main.Quit();
            }
            
            public void ShowAboutDialog(object obj, EventArgs args) {
                about_dialog.ShowAll();
            }
            
            public void ShowConfigDialog(object obj, EventArgs args) {
                config_dialog.ShowAll();
            }
            
            public void BookmarkItem(object obj, EventArgs args) {
                if ( ItemView.HasSelected ) {
                    bookmarker.ShowBookmarkWindow(curitem.Title, curitem.Uri, curitem.Contents, "");
                }
            }
            
            public void GoToNextItem(object obj, EventArgs args) {
                bool win = ItemView.GoToNextItem();
                if (!win) {
                    bool nextfeed = FeedView.GoToNextUnreadFeed();
                    
                    if ( nextfeed ) {
                        UpdateName();
                        curfeed = FeedView.Selected;
                        ItemView.Populate(curfeed);
                        HtmlView.Render(curfeed);
                        
                        ItemView.GoToPreviousItem();
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
                ItemView.GoToPreviousItem();
                if ( ItemView.HasSelected ) {
                    UpdateHtmlview();
                }
            }
            
            public void ZoomIn(object obj, EventArgs args) {
                HtmlView.ZoomIn();
            }
            
            public void ZoomOut(object obj, EventArgs args) {
                HtmlView.ZoomOut();
            }
            
            public void UpdateSelectedFeed(object obj, EventArgs args) {
                if ( FeedView.HasSelected ) {
                    bool updated = FeedView.Selected.Update();
                    
                    if ( updated ) {
                        FeedView.UpdateSelected();
                        ItemView.Update();
                        UpdateName();
                        ShowNotification(FeedView.Selected);
                    }
                }
            }
            
            public void UpdateAll(object obj, EventArgs args) {
                /*if (should_update) {
                    should_update = false;
                    UpdateAllPriv();
                    GLib.Timeout.Add(Summa.Core.Config.GlobalUpdateInterval, new GLib.TimeoutHandler(ScheduledUpdateAll));
                }*/
                Summa.Core.Application.Updater.Update();
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
                foreach ( Summa.Data.Feed feed in Summa.Data.Core.GetFeeds() ) {
                    statusbar.Push(contextid, "Updating feed \""+feed.Name+"\"");
                    contextid++;
                    while ( Gtk.Application.EventsPending() ) {
                        Gtk.Main.Iteration();
                    }
                    
                    string url = feed.Url;
                    bool newitems = feed.Update();
                    
                    if ( newitems ) {
                        if ( FeedView.HasSelected ) {
                            if ( feed.Url == FeedView.Selected.Url ) {
                                ItemView.Update();
                                UpdateName();
                            }
                        }
                        FeedView.UpdateFeed(feed);
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
            
            public void ShowNotification(Summa.Data.Feed feed) {
                /*if (Summa.Core.Config.ShowNotifications) {
                    var not = new Notify.Notification("New feed items", "Feed \""+feed.name+"\" has new unread items.", "internet-news-reader", null);
                    not.set_urgency(Notify.Urgency.NORMAL);
                    not.show();
                }*/
            }
            
            public void ShowPropertiesDialog(object obj, EventArgs args) {
                if ( FeedView.HasSelected ) {
                    Window dialog = new Summa.Gui.FeedPropertiesDialog(FeedView.Selected);
                    dialog.ShowAll();
                }
            }
            
            public void ShowTagsWindow(object obj, EventArgs args) {
                Window dialog = new Summa.Gui.TagWindow();
                dialog.ShowAll();
            }
            
            public void UpdateHtmlview() {
                curitem = ItemView.Selected;
                HtmlView.Render(curitem);
                
                if ( HtmlView.CanPrint() ) {
                    item_print_action.Sensitive = true;
                }
                
                if ( bookmarker.CanBookmark() ) {
                    item_bookmark_action.Sensitive = true;
                }
                
                if ( HtmlView.CanZoom() ) {
                    zoom_in_action.Sensitive = true;
                    item_zoom_in_action.Sensitive = true;
                    zoom_out_action.Sensitive = true;
                    item_zoom_out_action.Sensitive = true;
                }
                
                ItemView.MarkSelectedRead();
                UpdateName();
                
                if ( curfeed.GetUnreadCount() == 0 ) {
                    FeedView.UpdateSelected();
                }
            }
            
            public void OnImport(object obj, EventArgs args) {
                Firstrun fr = new Summa.Gui.Firstrun();
                fr.ShowAll();
            }
            
            public void DeleteFeed(object obj, EventArgs args) {
                if ( FeedView.HasSelected ) {
                    Window del = new Summa.Gui.DeleteConfirmationDialog(curfeed);
                    del.ShowAll();
                }
            }
            
            public void MarkItemFlagged(object obj, EventArgs args) {
                if ( ItemView.HasSelected ) {
                    ItemView.MarkSelectedFlagged();
                }
            }
            
            public void EnclosurePlay(object obj, EventArgs args) {
                if ( play_action.StockId == Gtk.Stock.MediaPause ) {
                    mediaplayer.Pause();
                    play_action.StockId = Gtk.Stock.MediaPlay;
                } else {
                    mediaplayer.Play(ItemView.Selected.EncUri);
                    play_action.StockId = Gtk.Stock.MediaPause;
                }
            }
            
            public void MarkAllItemsRead(object obj, EventArgs args) {
                if ( FeedView.HasSelected ) {
                    ItemView.MarkItemsRead();
                    FeedView.UpdateSelected();
                    UpdateName();
                }
            }
            
            public void UpdateName() {
                Summa.Data.Feed feed = FeedView.Selected;
                Title = feed.Name+" ("+feed.GetUnreadCount().ToString()+" unread) - Summa";
            }
            
            public void Print(object obj, EventArgs args) {
                HtmlView.Print();
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
                prefs_action.Activated += new EventHandler(ShowConfigDialog);
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
                props_action.Activated += new EventHandler(ShowPropertiesDialog);
                action_group.Add(props_action);
                
                tags_action = new Gtk.Action("Tags", "Edit subscription _tags", "Edit the tags of your feeds by tag", null);
                tags_action.Activated += new EventHandler(ShowTagsWindow);
                action_group.Add(tags_action);
                
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
                
                play_action = new Gtk.Action("Play_pause", "_Play/pause enclosed media", "Play or pause the media enclosed", Gtk.Stock.MediaPlay);
                play_action.Activated += new EventHandler(EnclosurePlay);
                play_action.Sensitive = false;
                action_group.Add(play_action);
                
                // help menu
                help_action = new Gtk.Action("Contents", "_Contents", "Get help", Gtk.Stock.Help);
                help_action.Activated += new EventHandler(stub); //FIXME
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
            <menuitem action='Tags'/>
            </menu>
            <menu action='ItemMenu'>
            <menuitem action='ZoomIn'/>
            <menuitem action='ZoomOut'/>
            <separator/>
            <menuitem action='Previous_item'/>
            <menuitem action='Next_item'/>
            <separator/>
            <menuitem action='Flag_item'/>
            <menuitem action='Play_pause'/>
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
            <separator/>
            <toolitem action='Play_pause'/>
        </toolbar>
        </ui>";
                uimanager.AddUiFromString(ui);
            }
        }
    }
}
