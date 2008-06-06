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
using Gtk;
using NewsKit;
using WebKit;
using Gnome;

namespace Summa {
    public class WebKitView : WebKit.WebView {
        private int start_size;            
        
        public WebKitView() {
            start_size = 10;

            NavigationRequested += new NavigationRequestedHandler (OnLinkClicked);
            
            string starting_content = "Welcome to <b>Summa</b>, a GNOME feed reader.</b><br /><br />This is a preview release, not intended to be used by anyone. Exercise caution.";            
            Render(starting_content);
        }
        
        public bool CanZoom() {
            return false;
        }
        
        public void ZoomIn() {
            WebSettings settings = new WebKit.WebSettings();
            
            if ( start_size > 4 ) {
                start_size++;
                /*settings.DefaultFontSize = start_size;*/
                
                Settings = settings;
            }
        }
        
        public void ZoomOut() {
            WebSettings settings = new WebKit.WebSettings();
            
            if ( start_size-1 > 4 ) {
                start_size--;
                /*settings.DefaultFontSize = start_size;*/
                
                Settings = settings;
            }
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
        
        private void OnLinkClicked(object o, NavigationRequestedArgs args) {
            Gnome.Url.Show(args.Request.Uri);
            
            return;
        }

        public void Render(string data) {
            LoadString(data, "text/html", "utf-8", "http:///");
        }

        public void Render(NewsKit.Item item) {
            string content = "<b>"+item.Title+"</b>";
            if ( item.Author != "" ) {
                content += " by "+item.Author+"<br />";
            } else {
                content += "<br />";
            }
            content += "<b>URL</b>: <a href=\""+item.Uri+"\">"+item.Uri+"</a><br />";
            if ( item.EncUri != "" ) {
                content += "<b>Enclosure</b>: <a href=\""+item.EncUri+"\">"+item.EncUri+"</a><br />";
            }
            content += "<hr/>";
            content += item.Contents;
            
            Render(content);
        }
        
        public void Render(NewsKit.Feed feed) {
            string content = "<b>"+feed.Name+"</b>";
            if ( feed.Author != "" ) {
                content += " by "+feed.Author+"<br />";
            } else {
                content += "<br />";
            }
            if ( feed.Subtitle != "" ) {
                content += "<b>Subtitle</b>: "+feed.Subtitle+"<br />";
            }
            content += "<b>URL</b>: <a href=\""+feed.Url+"\">"+feed.Url+"</a><br />";
            content += "<hr/>";
            content += "<img src=\""+feed.Image+"\">";
            
            Render(content);
        }
    }
}
