// WebKitView.cs
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
using System.Text;
using Gtk;
using WebKit;
using Gnome;

namespace Summa.Gui {
    public class WebSettings : WebKit.WebSettings {
        public WebSettings() {}
        
        public int DefaultFontSize {
            get { return (int)GetProperty("default-font-size").Val; }
            set { SetProperty("default-font-size", new GLib.Value(value)); }
        }
    }

    public class WebKitView : WebKit.WebView {
        private StringBuilder content = new StringBuilder();
        public Summa.Data.Item SelectedItem;
        public Summa.Data.ISource SelectedFeed;
        public Summa.Gui.ItemNotebook notebook;
        
        public WebKitView() {
            NavigationRequested += new NavigationRequestedHandler(OnNavigationRequested);
            HoveringOverLink += new HoveringOverLinkHandler(OnHoveringOverLink);
            
            string starting_content = "Welcome to <b>Summa</b>, a GNOME feed reader.<br /><br />This is a preview release, not intended to be used by anyone. Exercise caution.";
            Render(starting_content);
            
            ZoomTo(Summa.Core.Config.DefaultZoomLevel);
            Summa.Core.Notifier.ZoomChanged += OnZoomChanged;
        }
        
        private void OnZoomChanged(object obj, EventArgs args) {
            ZoomTo(Summa.Core.Config.DefaultZoomLevel);
        }
        
        public bool CanZoom() {
            return true;
        }
        
        public void ZoomIn() {
            Summa.Gui.WebSettings settings = new Summa.Gui.WebSettings();
            
            if ( Summa.Core.Config.DefaultZoomLevel > 4 ) {
                Summa.Core.Config.DefaultZoomLevel++;
                settings.DefaultFontSize = Summa.Core.Config.DefaultZoomLevel;
                
                Settings = settings;
            }
        }
        
        public void ZoomOut() {
            Summa.Gui.WebSettings settings = new Summa.Gui.WebSettings();
            
            if ( Summa.Core.Config.DefaultZoomLevel-1 > 4 ) {
                Summa.Core.Config.DefaultZoomLevel--;
                settings.DefaultFontSize = Summa.Core.Config.DefaultZoomLevel;
                
                Settings = settings;
            }
        }
        
        public void ZoomTo(int size) {
            Summa.Gui.WebSettings settings = new Summa.Gui.WebSettings();
            settings.DefaultFontSize = Summa.Core.Config.DefaultZoomLevel;
            
            Settings = settings;
        }
        
        public bool CanPrint() {
            return false;
        }
        
        public void Print() {
        }
        
        public bool CanGoBackOrForward() {
            return false;
        }

        public new bool CanCutClipboard() {
            return false;
        }
        
        private void OnNavigationRequested(object obj, NavigationRequestedArgs args) {
            if ( Summa.Core.Config.OpenTabs ) {
                try {
                    notebook.LoadUri(args.Request.Uri);
                } catch ( Exception ) {
                    Gnome.Url.Show(args.Request.Uri);
                }
            } else {
                Gnome.Url.Show(args.Request.Uri);
            }
            
            args.RetVal = WebKit.NavigationResponse.Ignore;
        }

        private void OnHoveringOverLink(object obj, HoveringOverLinkArgs args) {
            string text;

            if ((args.Title == String.Empty) && (args.Link == String.Empty))
                text = String.Empty;
            if (args.Title == String.Empty)
                text = args.Link;
            else
                text = args.Title;

            if (text != String.Empty) {
                if ( text != null ) {
                    Summa.Core.Notifier.Notify("Click to visit " + text);
                } else {
                    Summa.Core.Notifier.Notify("");
                }
            }
        }
        
        public void Render(string data) {
            LoadString(data, "text/html", "utf-8", "http:///");
        }
        
        public void RenderUri(string uri) {
            Open(uri);
        }
        
        public void Render(Summa.Data.Item item) {
            Render(Summa.Core.Config.Theme.MakeHtml(item));
            SelectedItem = item;
            Summa.Core.Application.Database.ItemChanged += OnItemChanged;
        }
        
        public void Render(Summa.Data.ISource feed) {
            Render(Summa.Core.Config.Theme.MakeHtml(feed));
            SelectedFeed = feed;
            Summa.Core.Application.Database.FeedChanged += OnFeedChanged;
            
            /*string all_content = "";
            
            foreach ( Summa.Data.Item item in feed.GetItems() ) {
                all_content += MakeItemHtml(item);
                all_content += "<br/><br/><hr/>";
            }
            Render(all_content);*/
        }
        
        private void OnFeedChanged(object obj, Summa.Core.ChangedEventArgs args) {
            if ( args.Uri == SelectedFeed.Url ) {
                Render(content.ToString());
            }
        }
        
        private void OnItemChanged(object obj, Summa.Core.ChangedEventArgs args) {
            if ( args.Uri == SelectedItem.Uri ) {
                Render(SelectedItem);
            }
        }
    }
}
