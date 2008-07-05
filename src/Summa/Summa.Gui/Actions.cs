// Actions.cs
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

namespace Summa.Actions {
    public class AddAction : Gtk.Action {
        private Summa.Gui.Browser browser;
        
        public AddAction(Summa.Gui.Browser browser) : base("Add", "_New subscription") {
            this.browser = browser;
            
            Tooltip = "Create a new feed";
            StockId = Gtk.Stock.Add;
            Activated += NewAddFeedDialog;
        }
        
        public void NewAddFeedDialog(object obj, EventArgs args) {
            Summa.Gui.AddFeedDialog add_dialog = new Summa.Gui.AddFeedDialog();
            add_dialog.TransientFor = browser;
            add_dialog.Show();
        }
    }
    
    public class ImportAction : Gtk.Action {
        private Summa.Gui.Browser browser;
        
        public ImportAction(Summa.Gui.Browser browser) : base("Import", "_Import") {
            this.browser = browser;
            
            Tooltip = "Import data";
            Activated += Import;
        }
        
        public void Import(object obj, EventArgs args) {
            Summa.Gui.FirstRun fr = new Summa.Gui.FirstRun();
            fr.TransientFor = browser;
            fr.ShowAll();
        }
    }
    
    public class UpdateAllAction : Gtk.Action {
        private Summa.Gui.Browser browser;
        
        public UpdateAllAction(Summa.Gui.Browser browser) : base("Update_all", "_Update all feeds") {
            this.browser = browser;
            
            Tooltip = "Update all feeds";
            StockId = Gtk.Stock.Refresh;
            Activated += UpdateAll;
        }
        
        public void UpdateAll(object obj, EventArgs args) {
            Summa.Core.Application.Updater.Update();
        }
    }
    
    public class PrintAction : Gtk.Action {
        private Summa.Gui.Browser browser;
        
        public PrintAction(Summa.Gui.Browser browser) : base("Print", "_Print") {
            this.browser = browser;
            
            Tooltip = "_Print the currently selected item";
            StockId = Gtk.Stock.Print;
            Activated += Print;
            Sensitive = false;
        }
        
        public void Print(object obj, EventArgs args) {
            browser.HtmlView.Print();
        }
        
        public void CheckShouldSensitive() {
            if ( browser.HtmlView.CanPrint() ) {
                browser.print_action.Sensitive = true;
            }
        }
    }
    
    public class PrintPreviewAction : Gtk.Action {
        private Summa.Gui.Browser browser;
        
        public PrintPreviewAction(Summa.Gui.Browser browser) : base("Print_preview", "Print previe_w") {
            this.browser = browser;
            
            Tooltip = "Show a preview of the printed document";
            StockId = Gtk.Stock.PrintPreview;
            Activated += PrintPreview;
            Sensitive = false;
        }
        
        public void PrintPreview(object obj, EventArgs args) {
        }
    }
    
    public class EmailLinkAction : Gtk.Action {
        private Summa.Gui.Browser browser;
        
        public EmailLinkAction(Summa.Gui.Browser browser) : base("Email_link", "_Email this item") {
            this.browser = browser;
            
            Tooltip = "Email a copy of the selected item";
            Activated += EmailLink;
            Sensitive = false;
        }
        
        public void EmailLink(object obj, EventArgs args) {
        }
    }
    
    public class BookmarkAction : Gtk.Action {
        private Summa.Gui.Browser browser;
        private Summa.Interfaces.IBookmarker bookmarker;
        
        public BookmarkAction(Summa.Gui.Browser browser) : base("Bookmark", "_Bookmark this item") {
            this.browser = browser;
            
            IconSet bookmark_iconset = new Gtk.IconSet();
            IconSource bookmark_iconsource = new Gtk.IconSource();
            bookmark_iconsource.IconName = "bookmark-new";
            bookmark_iconset.AddSource(bookmark_iconsource);
            browser.factory.Add("summa-bookmark-new", bookmark_iconset);
            
            Tooltip = "Bookmark this item";
            StockId = "summa-bookmark-new";
            Activated += Bookmark;
            
            switch(Summa.Core.Config.Bookmarker) {
                case "Native":
                    bookmarker = new Summa.Gui.NativeBookmarker();
                    break;
                case "Dieu":
                    bookmarker = new Summa.Gui.DieuBookmarker();
                    break;
            }
        }
        
        public void Bookmark(object obj, EventArgs args) {
            if ( browser.ItemView.HasSelected ) {
                bookmarker.ShowBookmarkWindow(browser.curitem.Title, browser.curitem.Uri, browser.curitem.Contents, "");
            }
        }
        
        public void CheckShouldSensitive() {
            if ( bookmarker.CanBookmark() ) {
                Sensitive = true;
            }
        }
    }
    
    public class NewWindowAction : Gtk.Action {
        private Summa.Gui.Browser browser;
        
        public NewWindowAction(Summa.Gui.Browser browser) : base("New_window", "New _window") {
            this.browser = browser;
            
            Tooltip = "Open a new window";
            Activated += NewWindow;
        }
        
        public void NewWindow(object obj, EventArgs args) {
            Window b = new Summa.Gui.Browser();
            Summa.Core.Application.Browsers.Add(b);
            b.ShowAll();
        }
    }
    
    public class CloseWindowAction : Gtk.Action {
        private Summa.Gui.Browser browser;
        
        public CloseWindowAction(Summa.Gui.Browser browser) : base("Close_window", "_Close window") {
            this.browser = browser;
            
            Tooltip = "Close this window";
            StockId = Gtk.Stock.Close;
            Activated += CloseWindow;
        }
        
        public void CloseWindow(object obj, EventArgs args) {
            Summa.Core.Application.CloseWindow(browser);
        }
    }
    
    public class CopyAction : Gtk.Action {
        private Summa.Gui.Browser browser;
        
        public CopyAction(Summa.Gui.Browser browser) : base("Copy", "_Copy") {
            this.browser = browser;
            
            Tooltip = "Copy";
            StockId = Gtk.Stock.Copy;
            Activated += Copy;
            Sensitive = false;
        }
        
        public void Copy(object obj, EventArgs args) {
        }
    }
    
    public class SelectAllAction : Gtk.Action {
        private Summa.Gui.Browser browser;
        
        public SelectAllAction(Summa.Gui.Browser browser) : base("Select_all", "_Select all text") {
            this.browser = browser;
            
            Tooltip = "Select all text";
            Activated += SelectAll;
            Sensitive = false;
        }
        
        public void SelectAll(object obj, EventArgs args) {
        }
    }
    
    public class FindAction : Gtk.Action {
        private Summa.Gui.Browser browser;
        
        public FindAction(Summa.Gui.Browser browser) : base("Find", "_Find...") {
            this.browser = browser;
            
            Tooltip = "Find an item";
            StockId = Gtk.Stock.Find;
            Activated += Find;
            Sensitive = false;
        }
        
        public void Find(object obj, EventArgs args) {
        }
    }
    
    public class PreferencesAction : Gtk.Action {
        private Summa.Gui.Browser browser;
        
        public PreferencesAction(Summa.Gui.Browser browser) : base("Preferences", "_Preferences") {
            this.browser = browser;
            
            Tooltip = "Preferences for Summa";
            StockId = Gtk.Stock.Preferences;
            Activated += ShowConfigDialog;
        }
        
        public void ShowConfigDialog(object obj, EventArgs args) {
            Summa.Core.Application.ConfigDialog.TransientFor = browser;
            Summa.Core.Application.ConfigDialog.TransientFor.ShowAll();
        }
    }
    
    public class UpdateAction : Gtk.Action {
        private Summa.Gui.Browser browser;
        
        public UpdateAction(Summa.Gui.Browser browser) : base("Update", "_Update feed") {
            this.browser = browser;
            
            Tooltip = "Update selected feed";
            StockId = Gtk.Stock.Refresh;
            Activated += Update;
        }
        
        public void Update(object obj, EventArgs args) {
            if ( browser.FeedView.HasSelected ) {
                bool updated = browser.FeedView.Selected.Update();
                
                if ( updated ) {
                    browser.FeedView.UpdateSelected();
                    browser.ItemView.Update();
                    browser.UpdateName();
                    browser.ShowNotification(browser.FeedView.Selected);
                }
            }
        }
    }
    
    public class MarkReadAction : Gtk.Action {
        private Summa.Gui.Browser browser;
        
        public MarkReadAction(Summa.Gui.Browser browser) : base("Mark_read", "_Mark feed as read") {
            this.browser = browser;
            
            Tooltip = "Mark all items in the selected feed as read";
            StockId = Gtk.Stock.Apply;
            Activated += MarkAllItemsRead;
        }
        
        public void MarkAllItemsRead(object obj, EventArgs args) {
            if ( browser.FeedView.HasSelected ) {
                browser.FeedView.Selected.MarkItemsRead();
            }
        }
    }
    
    public class DeleteAction : Gtk.Action {
        private Summa.Gui.Browser browser;
        
        public DeleteAction(Summa.Gui.Browser browser) : base("Delete", "_Delete subscription") {
            this.browser = browser;
            
            Tooltip = "Delete the selected feed";
            StockId = Gtk.Stock.Delete;
            Activated += DeleteFeed;
        }
        
        public void DeleteFeed(object obj, EventArgs args) {
            if ( browser.FeedView.HasSelected ) {
                Window del = new Summa.Gui.DeleteConfirmationDialog(browser.curfeed);
                del.TransientFor = browser;
                del.ShowAll();
            }
        }
    }
    
    public class PropertiesAction : Gtk.Action {
        private Summa.Gui.Browser browser;
        
        public PropertiesAction(Summa.Gui.Browser browser) : base("Properties", "_Properties") {
            this.browser = browser;
            
            Tooltip = "Properties of the selected feed";
            StockId = Gtk.Stock.Properties;
            Activated += ShowPropertiesDialog;
        }
        
        public void ShowPropertiesDialog(object obj, EventArgs args) {
            if ( browser.FeedView.HasSelected ) {
                Window dialog = new Summa.Gui.FeedPropertiesDialog(browser.FeedView.Selected);
                dialog.TransientFor = browser;
                dialog.ShowAll();
            }
        }
    }
    
    public class TagsAction : Gtk.Action {
        private Summa.Gui.Browser browser;
        
        public TagsAction(Summa.Gui.Browser browser) : base("Tags", "Edit subscription _tags") {
            this.browser = browser;
            
            Tooltip = "Edit the tags of your feeds by tag";
            Activated += ShowTagsWindow;
        }
        
        public void ShowTagsWindow(object obj, EventArgs args) {
            Window dialog = new Summa.Gui.TagWindow();
            dialog.TransientFor = browser;
            dialog.ShowAll();
        }
    }
    
    public class ZoomInAction : Gtk.Action {
        private Summa.Gui.Browser browser;
        
        public ZoomInAction(Summa.Gui.Browser browser) : base("ZoomIn", "_Increase text size") {
            this.browser = browser;
            
            Tooltip = "Increase text size";
            StockId = Gtk.Stock.ZoomIn;
            Activated += ZoomIn;
        }
        
        public void ZoomIn(object obj, EventArgs args) {
            browser.HtmlView.ZoomIn();
        }
        
        public void CheckShouldSensitive() {
            if ( browser.ItemView.HasSelected ) {
                Sensitive = true;
            }
        }
    }
    
    public class ZoomOutAction : Gtk.Action {
        private Summa.Gui.Browser browser;
        
        public ZoomOutAction(Summa.Gui.Browser browser) : base("ZoomOut", "_Decrease text size") {
            this.browser = browser;
            
            Tooltip = "Decrease text size";
            StockId = Gtk.Stock.ZoomOut;
            Activated += ZoomOut;
        }
        
        public void ZoomOut(object obj, EventArgs args) {
            browser.HtmlView.ZoomOut();
        }
        
        public void CheckShouldSensitive() {
            if ( browser.ItemView.HasSelected ) {
                if ( Summa.Core.Config.DefaultZoomLevel != 3 ) {
                    Sensitive = true;
                } else {
                    Sensitive = false;
                }
            }
        }
    }
    
    public class NextItemAction : Gtk.Action {
        private Summa.Gui.Browser browser;
        
        public NextItemAction(Summa.Gui.Browser browser) : base("Next_item", "_Go to next unread item") {
            this.browser = browser;
            
            Tooltip = "Go to next unread item";
            StockId = Gtk.Stock.GoForward;
            Activated += GoToNextItem;
        }
        
        public void GoToNextItem(object obj, EventArgs args) {
            bool win = browser.ItemView.GoToNextItem();
            if (!win) {
                bool nextfeed = browser.FeedView.GoToNextUnreadFeed();
                
                if ( nextfeed ) {
                    browser.ItemView.Populate(browser.FeedView.Selected);
                    
                    browser.ItemView.GoToPreviousItem();
                    browser.UpdateHtmlview();
                } else {
                    browser.statusbar.Push(browser.contextid, "There are no more unread items.");
                    browser.contextid++;
                }
            } else {
                browser.UpdateHtmlview();
            }
        }
    }
    
    public class PreviousItemAction : Gtk.Action {
        private Summa.Gui.Browser browser;
        
        public PreviousItemAction(Summa.Gui.Browser browser) : base("Previous_item", "_Go to the previous item") {
            this.browser = browser;
            
            Tooltip = "Go to the previous item";
            StockId = Gtk.Stock.GoBack;
            Activated += GoToPreviousItem;
        }
        
        public void GoToPreviousItem(object obj, EventArgs args) {
            browser.ItemView.GoToPreviousItem();
            if ( browser.ItemView.HasSelected ) {
                browser.UpdateHtmlview();
            }
        }
    }
    
    public class FlagAction : Gtk.Action {
        private Summa.Gui.Browser browser;
        private Summa.Data.Item item;
        
        public FlagAction(Summa.Gui.Browser browser) : base("Flag_item", "Flag this item") {
            this.browser = browser;
            
            Tooltip = "Flag/unflag this item";
            Activated += Flag;
        }
        
        public void Flag(object obj, EventArgs args) {
            browser.ItemView.MarkSelectedFlagged();
            Populate(item);
        }
        
        public void Populate(Summa.Data.Item item) {
            this.item = item;
            
            if ( item.Flagged ) {
                Label = "Unflag this item";
            } else {
                Label = "Flag this item";
            }
        }
    }
    
    public class EnclosureAction : Gtk.Action {
        private Summa.Gui.Browser browser;
        private Summa.Interfaces.IMediaPlayer mediaplayer;
        
        public EnclosureAction(Summa.Gui.Browser browser) : base("Play_pause", "_Play enclosed media") {
            this.browser = browser;
            
            Tooltip = "Play or pause the media enclosed";
            StockId = Gtk.Stock.MediaPlay;
            Activated += Play;
            
            mediaplayer = new Summa.Gui.TotemMediaPlayer();
        }
        
        public void Play(object obj, EventArgs args) {
            if ( browser.play_action.StockId == Gtk.Stock.MediaPause ) {
                mediaplayer.Pause();
                SetToPlay();
            } else {
                mediaplayer.Play(browser.ItemView.Selected.EncUri);
                SetToPause();
            }
        }
        
        public void Populate(Summa.Data.Item item) {
            if ( item.EncUri != "" ) {
                Sensitive = true;
            } else {
                Sensitive = false;
            }
        }
        
        public void SetToPlay() {
            Label = "_Play enclosed media";
            StockId = Gtk.Stock.MediaPlay;
        }
        
        public void SetToPause() {
            Label = "_Pause enclosed media";
            StockId = Gtk.Stock.MediaPause;
        }
    }
    
    public class HelpAction : Gtk.Action {
        private Summa.Gui.Browser browser;
        
        public HelpAction(Summa.Gui.Browser browser) : base("Contents", "_Contents") {
            this.browser = browser;
            
            Tooltip = "Get help";
            StockId = Gtk.Stock.Help;
            Activated += Help;
            Sensitive = false;
        }
        
        public void Help(object obj, EventArgs args) {
        }
    }
    
    public class AboutAction : Gtk.Action {
        private Summa.Gui.Browser browser;
        
        public AboutAction(Summa.Gui.Browser browser) : base("About", "_About") {
            this.browser = browser;
            
            Tooltip = "About";
            StockId = Gtk.Stock.About;
            Activated += ShowAbout;
        }
        
        public void ShowAbout(object obj, EventArgs args) {
            browser.AboutDialog.ShowAll();
        }
    }
}
