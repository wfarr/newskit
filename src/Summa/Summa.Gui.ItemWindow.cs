// ItemWindow.cs
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
using Gtk;

namespace Summa.Gui {
    public class ItemWindow : Gtk.Window {
        private Summa.Data.Item item;
        
        private Gtk.ActionGroup action_group;
        private Gtk.UIManager uimanager;
        
        private Gtk.Table table;
        private Gtk.Widget menubar;
        private Gtk.Widget toolbar;
        
        public Summa.Gui.WebKitView HtmlView;
        
        public ItemWindow(Summa.Data.Item item) : base(Gtk.WindowType.Toplevel) {
            this.item = item;
            table = new Gtk.Table(4, 4, false);
            Add(table);
            
            action_group = new Gtk.ActionGroup("general");
            
            SetUpActionGroup();
            
            uimanager = new Gtk.UIManager();
            uimanager.InsertActionGroup(action_group, 0);
            AddAccelGroup(uimanager.AccelGroup);
            SetUpUimanager();
            
            menubar = uimanager.GetWidget("/MenuBar");
            table.Attach(menubar, 0, 5, 0, 1, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 0, 0);
            
            toolbar = uimanager.GetWidget("/ToolBar");
            table.Attach(toolbar, 0, 5, 1, 2, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 0, 0);
            
            HtmlView = new Summa.Gui.WebKitView();
            table.Attach(toolbar, 0, 5, 2, 3);
            
            Populate(item);
            
            Summa.Core.Application.Database.ItemChanged += OnItemChanged;
        }
        
        private void OnItemChanged(object obj, Summa.Core.ChangedEventArgs args) {
            if ( args.Uri == item.Uri ) {
                Populate(item);
            }
        }
        
        private void Populate(Summa.Data.Item item) {
            HtmlView.Render(item);
            UpdateName(item);
        }
        
        private void UpdateName(Summa.Data.Item item) {
            Title = item.Name;
        }
        
        private void SetUpActionGroup() {
            // menus
            edit_menu = new Gtk.Action("EditMenu", "_Edit", null, null);
            action_group.Add(edit_menu);
            item_menu = new Gtk.Action("ItemMenu", "_Item", null, null);
            action_group.Add(item_menu);
            help_menu = new Gtk.Action("HelpMenu", "_Help", null, null);
            action_group.Add(help_menu);
            
            print_action = new Summa.Actions.PrintAction(this);
            action_group.Add(print_action, "<ctrl>p");
            
            print_prev_action = new Summa.Actions.PrintPreviewAction(this);
            action_group.Add(print_prev_action, "<shift><ctrl>p");
            
            email_action = new Summa.Actions.EmailLinkAction(this);
            action_group.Add(email_action);
            
            bookmark_action = new Summa.Actions.BookmarkAction(this);
            bookmark_action.Sensitive = false;
            action_group.Add(bookmark_action, "<ctrl>d");
            
            new_win_action = new Summa.Actions.NewWindowAction(this);
            action_group.Add(new_win_action, "<shift><ctrl>N");
            
            close_action = new Summa.Actions.CloseWindowAction(this);
            action_group.Add(close_action, "<ctrl>W");
            
            copy_action = new Summa.Actions.CopyAction(this);
            action_group.Add(copy_action, "<ctrl>C");
            
            select_all_action = new Summa.Actions.SelectAllAction(this);
            action_group.Add(select_all_action, "<ctrl>A");
            
            find_action = new Summa.Actions.FindAction(this);
            action_group.Add(find_action, "<ctrl>F");
            
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
            
            help_action = new Summa.Actions.HelpAction(this);
            action_group.Add(help_action, "F11");
            
            about_action = new Summa.Actions.AboutAction(this);
            action_group.Add(about_action);
        }
        
        public void SetUpUimanager() {
            string ui = @"<ui>
    <menubar name='MenuBar'>
        <menu action='ItemMenu'>
            <menuitem action='ZoomIn'/>
            <menuitem action='ZoomOut'/>
            <separator/>
            <menuitem action='Previous_item'/>
            <menuitem action='Next_item'/>
            <separator/>
            <menuitem action='Flag_item'/>
            <menuitem action='Play_pause'/>
            <separator/>
            <menuitem action='Print_preview'/>
            <menuitem action='Print'/>
            <menuitem action='Email_link'/>
            <menuitem action='Bookmark'/>
            <separator/>
            <menuitem action='Close_window'/>
        </menu>
        <menu action='EditMenu'>
            <menuitem action='Copy'/>
            <separator/>
            <menuitem action='Select_all'/>
            <menuitem action='Find'/>
        </menu>
        <menu action='HelpMenu'>
            <menuitem action='Contents'/>
            <menuitem action='About'/>
        </menu>
    </menubar>
    <toolbar name='ToolBar'>
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
