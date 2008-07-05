// GeckoView.cs
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
using Gecko;
using Gtk;

namespace Summa.Gui {
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
        
        public void Render(Summa.Data.Item item) {
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
        
        public void Render(Summa.Data.Feed feed) {
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
