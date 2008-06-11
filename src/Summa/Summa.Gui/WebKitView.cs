///* /home/eosten/Summa/Summa/WebKitView.cs
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
using System.Text;
using Gtk;
using WebKit;
using Gnome;

namespace Summa {
    namespace Gui {
        public class WebSettings : WebKit.WebSettings {
            public WebSettings() {}
            
            public int DefaultFontSize {
                get { return (int)GetProperty("default-font-size").Val; }
                set { SetProperty("default-font-size", new GLib.Value(value)); }
            }
        }

        public class WebKitView : WebKit.WebView {
            private Statusbar statusbar;
            private StringBuilder content = new StringBuilder();
            
            public WebKitView(Gtk.Statusbar sb) {
                statusbar = sb;

                NavigationRequested += new NavigationRequestedHandler(OnNavigationRequested);
                HoveringOverLink += new HoveringOverLinkHandler(OnHoveringOverLink);
                
                string starting_content = "Welcome to <b>Summa</b>, a GNOME feed reader.<br /><br />This is a preview release, not intended to be used by anyone. Exercise caution.";
                Render(starting_content);
                
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
                Summa.Core.Config.DefaultZoomLevel = size;
                
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

            public bool CanCutClipboard() {
                return false;
            }
            
            private void OnNavigationRequested(object obj, NavigationRequestedArgs args) {
                Gnome.Url.Show(args.Request.Uri);
                
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
                        statusbar.Push(statusbar.GetContextId(text), text);
                    } else {
                        statusbar.Push(statusbar.GetContextId(""), "");
                    }
                }
            }
            
            public void Render(string data) {
                LoadString(data, "text/html", "utf-8", "http:///");
            }

            public void Render(Summa.Data.Item item) {
                if (content.ToString() != String.Empty) {
                    // Empties the StringBuilder if it has more than ""
                    content.Remove(0, content.Length);
                }

                content.AppendFormat("<b>{0}</b>", item.Title);

                if ( item.Author != String.Empty ) {
                    content.AppendFormat(" by {0}<br />", item.Author);
                } else {
                    content.Append("<br />");
                }
                content.AppendFormat("<b>URL</b>: <a href=\"{0}\">{0}</a><br />", item.Uri);
                if ( item.EncUri != String.Empty ) {
                    content.AppendFormat("<b>Enclosure</b>: <a href=\"{0}\">{0}</a><br />", item.EncUri);
                }
                content.Append("<hr/>");
                content.Append(item.Contents);
                
                Render(content.ToString());
            }
            
            public void Render(Summa.Data.Feed feed) {
                if (content.ToString() != String.Empty) {
                    // Empties the StringBuilder if it has more than ""
                    content.Remove(0, content.Length);
                }

                content.AppendFormat("<b>{0}</b>", feed.Name);

                if ( feed.Author != String.Empty ) {
                    content.AppendFormat(" by {0}<br />", feed.Author);
                } else {
                    content.Append("<br />");
                }

                if ( feed.Subtitle != String.Empty ) {
                    content.AppendFormat("<b>Subtitle</b>: {0}<br />", feed.Subtitle);
                }

                content.AppendFormat("<b>URL</b>: <a href=\"{0}\">{0}</a><br />", feed.Url);
                content.Append("<hr/>");
                content.AppendFormat("<img src=\"{0}\">", feed.Image);
                
                Render(content.ToString());
            }
        }
    }
}
