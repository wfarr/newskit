// Browser.cs
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
using Gtk;

namespace Summa.Gui {
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
        public Summa.Actions.AddAction addaction;
        public Gtk.Action import_action;
        public Summa.Actions.UpdateAllAction Up_all_action;
        public Summa.Actions.PrintAction print_action;
        public Gtk.Action print_prev_action;
        public Gtk.Action email_action;
        public Summa.Actions.BookmarkAction bookmark_action;
        public Gtk.Action new_tab_action;
        public Gtk.Action new_win_action;
        public Gtk.Action close_action;
        
        // edit menu
        public Gtk.Action copy_action;
        public Gtk.Action select_all_action;
        public Gtk.Action find_action;
        public Summa.Actions.PreferencesAction prefs_action;
        
        // subscription menu
        public Gtk.Action update_action;
        public Gtk.Action read_action;
        public Gtk.Action delete_action;
        public Gtk.Action props_action;
        public Gtk.Action tags_action;
        
        // item menu
        public Summa.Actions.ZoomInAction zoom_in_action;
        public Summa.Actions.ZoomOutAction zoom_out_action;
        public Gtk.Action next_item_action;
        public Gtk.Action prev_item_action;
        public Summa.Actions.FlagAction flag_action;
        public Summa.Actions.EnclosureAction play_action;
        
        // help menu
        public Gtk.Action help_action;
        public Summa.Gui.AboutDialog AboutDialog;
        public Gtk.Action about_action;
        
        public Gtk.Widget menubar;
        public Gtk.Widget toolbar;
        
        public Gtk.Paned main_paned;
        public Gtk.Paned left_paned;
        public Gtk.Paned right_paned;
        
        public Summa.Gui.TagView TagView;
        public Gtk.ScrolledWindow TagView_swin;
        public Summa.Gui.FeedView FeedView;
        public Gtk.ScrolledWindow FeedView_swin;
        public Summa.Gui.ItemView ItemView;
        public Gtk.ScrolledWindow ItemView_swin;
        
        public Summa.Data.Item item;
        
        public Summa.Gui.ItemNotebook ItemNotebook;
        
        public Gtk.Button connection_button;
        public Summa.Gui.NotificationBar StatusBar;
        
        // the currently displayed feed
        public Summa.Interfaces.ISource curfeed;
        public Summa.Data.Item curitem;
        
        public Browser() : base(Gtk.WindowType.Toplevel) {
            Title = "Summa";
            IconName = "summa";
            
            action_group = new Gtk.ActionGroup("general");
            factory = new Gtk.IconFactory();
            factory.AddDefault();
            
            DestroyEvent += CloseWindow;
            
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
            
            if ( Summa.Core.Config.WidescreenView ) {
                right_paned = new Gtk.HPaned();
            } else {
                right_paned = new Gtk.VPaned();
            }
            main_paned.Pack2(right_paned, true, true);
            
            ItemView = new Summa.Gui.ItemView();
            ItemView.CursorChanged += new EventHandler(ItemviewChanged);
            ItemView_swin = new Gtk.ScrolledWindow(new Gtk.Adjustment(0, 0, 0, 0, 0, 0), new Gtk.Adjustment(0, 0, 0, 0, 0, 0));
            ItemView_swin.Add(ItemView);
            ItemView_swin.ShadowType = Gtk.ShadowType.In;
            ItemView_swin.SetPolicy(Gtk.PolicyType.Automatic, Gtk.PolicyType.Automatic);
            right_paned.Pack1(ItemView_swin, true, true);
            
            ItemNotebook = new Summa.Gui.ItemNotebook();
            right_paned.Pack2(ItemNotebook, true, true);
            
            StatusBar = new Summa.Gui.NotificationBar();
            table.Attach(StatusBar, 0, 5, 3, 4, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 0, 0);
            
            AboutDialog = new Summa.Gui.AboutDialog();
            
            UpdateFromConfig();
            
            Summa.Core.Application.Database.ItemChanged += OnItemChanged;
            Summa.Core.Application.Notifier.ViewChanged += OnViewChanged;
        }
        
        private void OnViewChanged(object obj, EventArgs args) {
            right_paned.Remove(ItemView_swin);
            right_paned.Remove(ItemNotebook);
            main_paned.Remove(right_paned);
            if ( Summa.Core.Config.WidescreenView ) {
                right_paned = new Gtk.HPaned();
            } else {
                right_paned = new Gtk.VPaned();
            }
            main_paned.Pack2(right_paned, true, true);
            right_paned.Add1(ItemView_swin);
            right_paned.Add2(ItemNotebook);
            ShowAll();
        }
        
        private void OnItemChanged(object obj, Summa.Core.ChangedEventArgs args) {
            if ( FeedView.HasSelected ) {
                if ( FeedView.Selected.Url == args.FeedUri ) {
                    if ( args.ItemProperty == "read" ) {
                        UpdateName();
                    }
                }
            }
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
            ItemNotebook.CurrentView.Render(curfeed);
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
        
        public void UpdateHtmlview() {
            curitem = ItemView.Selected;
            Gtk.Bin c = (Gtk.Bin)ItemNotebook.GetNthPage(0);
            Summa.Gui.WebKitView view = (Summa.Gui.WebKitView)c.Child;
            view.Render(curitem);
            ItemNotebook.ShowAll();
            
            print_action.CheckShouldSensitive();
            bookmark_action.CheckShouldSensitive();
            new_tab_action.Sensitive = true;
            
            if ( ItemNotebook.CurrentView.CanZoom() ) {
                zoom_in_action.CheckShouldSensitive();
                zoom_out_action.CheckShouldSensitive();
            }
            
            flag_action.Populate(ItemView.Selected);
            play_action.Populate(ItemView.Selected);
            play_action.SetToPlay();
            
            if ( !ItemView.Selected.Read ) {
                ItemView.Selected.Read = true;
            }
        }
        
        public void UpdateFromConfig() {
            Resize(Summa.Core.Config.WindowWidth, Summa.Core.Config.WindowHeight);
        
            main_paned.Position  = Summa.Core.Config.MainPanePosition;
            left_paned.Position  = Summa.Core.Config.LeftPanePosition;
            right_paned.Position  = Summa.Core.Config.RightPanePosition;
        }
        
        public void ShowNotification(Summa.Data.Feed feed) {
            /*if (Summa.Core.Config.ShowNotifications) {
                var not = new Notify.Notification("New feed items", "Feed \""+feed.name+"\" has new unread items.", "internet-news-reader", null);
                not.set_urgency(Notify.Urgency.NORMAL);
                not.show();
            }*/
        }
        
        public void UpdateName() {
            Summa.Interfaces.ISource feed = FeedView.Selected;
            Title = feed.Name+" ("+feed.UnreadCount.ToString()+" unread) - Summa";
        }
        
        public void CloseWindow(object obj, EventArgs args) {
            Summa.Core.Application.CloseWindow(this);
        }
        
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
            addaction = new Summa.Actions.AddAction(this);
            action_group.Add(addaction, "<ctrl>n");
            
            import_action = new Summa.Actions.ImportAction(this);
            action_group.Add(import_action);
            
            Up_all_action = new Summa.Actions.UpdateAllAction(this);
            action_group.Add(Up_all_action, "<ctrl>r");
            
            print_action = new Summa.Actions.PrintAction(this);
            action_group.Add(print_action, "<ctrl>p");
            
            print_prev_action = new Summa.Actions.PrintPreviewAction(this);
            action_group.Add(print_prev_action, "<shift><ctrl>p");
            
            email_action = new Summa.Actions.EmailLinkAction(this);
            action_group.Add(email_action);
            
            bookmark_action = new Summa.Actions.BookmarkAction(this);
            bookmark_action.Sensitive = false;
            action_group.Add(bookmark_action, "<ctrl>d");
            
            new_tab_action = new Summa.Actions.NewTabAction(this);
            new_tab_action.Sensitive = false;
            action_group.Add(new_tab_action, "<ctrl>t");
            
            new_win_action = new Summa.Actions.NewWindowAction(this);
            action_group.Add(new_win_action, "<shift><ctrl>N");
            
            close_action = new Summa.Actions.CloseWindowAction(this);
            action_group.Add(close_action, "<ctrl>W");
            
            // edit menu
            copy_action = new Summa.Actions.CopyAction(this);
            action_group.Add(copy_action, "<ctrl>C");
            
            select_all_action = new Summa.Actions.SelectAllAction(this);
            action_group.Add(select_all_action, "<ctrl>A");
            
            find_action = new Summa.Actions.FindAction(this);
            action_group.Add(find_action, "<ctrl>F");
            
            prefs_action = new Summa.Actions.PreferencesAction(this);
            action_group.Add(prefs_action);
            
            // subscription menu
            update_action = new Summa.Actions.UpdateAction(this);
            action_group.Add(update_action, "<ctrl>u");
            
            read_action = new Summa.Actions.MarkReadAction(this);
            action_group.Add(read_action, "<ctrl>m");
            
            delete_action = new Summa.Actions.DeleteAction(this);
            action_group.Add(delete_action);
            
            props_action = new Summa.Actions.PropertiesAction(this);
            action_group.Add(props_action);
            
            tags_action = new Summa.Actions.TagsAction(this);
            action_group.Add(tags_action);
            
            // item menu
            zoom_in_action = new Summa.Actions.ZoomInAction(this);
            zoom_in_action.Sensitive = false;
            action_group.Add(zoom_in_action, null);
            
            zoom_out_action = new Summa.Actions.ZoomOutAction(this);
            zoom_out_action.Sensitive = false;
            action_group.Add(zoom_out_action, null);
            
            next_item_action = new Summa.Actions.NextItemAction(this);
            action_group.Add(next_item_action, "n");
            
            prev_item_action = new Summa.Actions.PreviousItemAction(this);
            action_group.Add(prev_item_action, "j");
            
            flag_action = new Summa.Actions.FlagAction(this);
            flag_action.Sensitive = false;
            action_group.Add(flag_action);
            
            play_action = new Summa.Actions.EnclosureAction(this);
            play_action.Sensitive = false;
            action_group.Add(play_action);
            
            // help menu
            help_action = new Summa.Actions.HelpAction(this);
            action_group.Add(help_action, "F11");
            
            about_action = new Summa.Actions.AboutAction(this);
            action_group.Add(about_action);
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
        <menuitem action='New_tab'/>
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
        <toolitem action='Print'/>
        <toolitem action='Bookmark'/>
        <toolitem action='ZoomIn'/>
        <toolitem action='ZoomOut'/>
        <separator/>
        <toolitem action='Play_pause'/>
    </toolbar>
    </ui>";
            uimanager.AddUiFromString(ui);
        }
    }
}
