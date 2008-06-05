///* /home/eosten/Summa/Summa/GeckoView.cs
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
// * 	Ethan Osten <senoki@gmail.com>
// */
//

using System;
using Gecko;
using Gtk;
using NewsKit;

namespace Summa {
    public class GeckoView : Gtk.Bin {
        private Gecko.WebControl wbc;
        
        public GeckoView() {
                        
            
            wbc = new Gecko.WebControl();
            wbc.OpenUri += new OpenUriHandler(OnOpenUri);
            wbc.Show();
            Add(wbc);
        }
        
        public bool CanZoom() {
            return false;
        }
        
        public void ZoomIn() {
        }
        
        public void ZoomOut() {
        }
        
        public bool CanPrint() {
            return false;
        }
        
        public void Print() {
        }
        
        public void Render(string data) {
            wbc.RenderData(data, "http://google.com", "text/html");
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
    		
    		wbc.RenderData(content, "http://google.com", "text/html");
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
    		
    		wbc.RenderData(content, "http://google.com", "text/html");
        }
        
        private void OnOpenUri(object Object, Gecko.OpenUriArgs args) {
            Gnome.Url.Show(args.AURI);
        }
    }
}
