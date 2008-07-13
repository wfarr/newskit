// ItemNotebook.cs
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
    public class ItemNotebook : Gtk.Notebook {
        public Summa.Gui.WebKitView CurrentView {
            get {
                Bin c = (Gtk.Bin)GetNthPage(Page);
                return (Summa.Gui.WebKitView)c.Child;
            }
        }
        
        public bool CloseFirstTab;
        
        public ItemNotebook() {
            Scrollable = true;
            ShowTabs = false;
            
            CloseFirstTab = false;
            Load("Welcome to <b>Summa</b>, a GNOME feed reader.<br /><br />This is a preview release, not intended to be used by anyone. Exercise caution.");
            CloseFirstTab = true;
        }
        
        public void Load(Summa.Data.Item item) {
            if ( CloseFirstTab ) {
                RemovePage(0);
            }
            
            Gtk.HBox container = new Gtk.HBox();
            Gtk.Label label = new Gtk.Label(item.Title);
            container.PackStart(label);
            Gtk.Button button = new Gtk.Button(new Gtk.Image(Gtk.IconTheme.Default.LookupIcon("gtk-close", 16, Gtk.IconLookupFlags.NoSvg).LoadIcon()));
            button.Clicked += OnClicked;
            button.Relief = Gtk.ReliefStyle.None;
            button.SetSizeRequest(20, 20);
            container.PackEnd(button);
            
            Summa.Gui.WebKitView view = new Summa.Gui.WebKitView();
            view.notebook = this;
            ScrolledWindow view_swin = new Gtk.ScrolledWindow(new Gtk.Adjustment(0, 0, 0, 0, 0, 0), new Gtk.Adjustment(0, 0, 0, 0, 0, 0));
            view_swin.Add(view);
            view_swin.SetPolicy(Gtk.PolicyType.Automatic, Gtk.PolicyType.Automatic);
            view.Render(item);
            
            AppendPage(view_swin, container);
            container.ShowAll();
        }
        
        public void Load(string content) {
            if ( CloseFirstTab ) {
                RemovePage(0);
            }
            
            Gtk.HBox container = new Gtk.HBox();
            Gtk.Label label = new Gtk.Label("Summa");
            container.PackStart(label);
            Gtk.Button button = new Gtk.Button(new Gtk.Image(Gtk.IconTheme.Default.LookupIcon("gtk-close", 16, Gtk.IconLookupFlags.NoSvg).LoadIcon()));
            button.Clicked += OnClicked;
            button.Relief = Gtk.ReliefStyle.None;
            button.SetSizeRequest(20, 20);
            container.PackEnd(button);
            
            Summa.Gui.WebKitView view = new Summa.Gui.WebKitView();
            view.notebook = this;
            ScrolledWindow view_swin = new Gtk.ScrolledWindow(new Gtk.Adjustment(0, 0, 0, 0, 0, 0), new Gtk.Adjustment(0, 0, 0, 0, 0, 0));
            view_swin.Add(view);
            view_swin.SetPolicy(Gtk.PolicyType.Automatic, Gtk.PolicyType.Automatic);
            view.Render(content);
            
            AppendPage(view_swin, container);
            container.ShowAll();
        }
        
        public void LoadUri(string uri) {
            if ( CloseFirstTab ) {
                RemovePage(0);
            }
            
            Gtk.HBox container = new Gtk.HBox();
            Gtk.Label label = new Gtk.Label(uri);
            container.PackStart(label);
            Gtk.Button button = new Gtk.Button(new Gtk.Image(Gtk.IconTheme.Default.LookupIcon("gtk-close", 16, Gtk.IconLookupFlags.NoSvg).LoadIcon()));
            button.Clicked += OnClicked;
            button.Relief = Gtk.ReliefStyle.None;
            button.SetSizeRequest(20, 20);
            container.PackEnd(button);
            
            Summa.Gui.WebKitView view = new Summa.Gui.WebKitView();
            view.notebook = this;
            ScrolledWindow view_swin = new Gtk.ScrolledWindow(new Gtk.Adjustment(0, 0, 0, 0, 0, 0), new Gtk.Adjustment(0, 0, 0, 0, 0, 0));
            view_swin.Add(view);
            view_swin.SetPolicy(Gtk.PolicyType.Automatic, Gtk.PolicyType.Automatic);
            view.RenderUri(uri);
            
            AppendPage(view_swin, container);
            container.ShowAll();
        }
        
        private void OnClicked(object obj, EventArgs args) {
            RemovePage(Page);
        }
    }
}
