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
using System.Collections;
using Gtk;

using Summa.Core;
using Summa.Data;
using Summa.Gui;

namespace Summa.Gui {
    public class ItemNotebook : Notebook {
        public WebKitView CurrentView {
            get {
                Bin c = (Bin)GetNthPage(Page);
                return (WebKitView)c.Child;
            }
        }
        
        public bool CloseFirstTab;
        private Hashtable hash;
        
        public ItemNotebook() {
            Scrollable = true;
            ShowTabs = false;
            
            hash = new Hashtable();
            
            Load("Welcome to <b>Summa</b>, a GNOME feed reader.<br /><br />This is a preview release, not intended to be used by anyone. Exercise caution.");
        }
        
        public void Load(Summa.Data.Item item) {
            HBox container = new HBox();
            Label label = new Label(item.Title);
            container.PackStart(label);
            Button button = new Button(new Image(IconTheme.Default.LookupIcon("gtk-close", 16, IconLookupFlags.NoSvg).LoadIcon()));
            button.Clicked += OnClicked;
            button.Relief = ReliefStyle.None;
            button.SetSizeRequest(20, 20);
            container.PackEnd(button);
            
            WebKitView view = new WebKitView();
            view.notebook = this;
            ScrolledWindow view_swin = new ScrolledWindow(new Adjustment(0, 0, 0, 0, 0, 0), new Adjustment(0, 0, 0, 0, 0, 0));
            view_swin.Add(view);
            view_swin.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
            view.Render(item);
            
            hash.Add(container, view);
            
            AppendPage(view_swin, container);
            container.ShowAll();
            
            ShowAll();
            if ( NPages > 1 ) {
                ShowTabs = true;
            } else {
                ShowTabs = false;
            }
        }
        
        public void Load(string content) {
            HBox container = new HBox();
            Label label = new Label("Summa");
            container.PackStart(label);
            if ( NPages != 0 ) {
                Button button = new Button(new Image(IconTheme.Default.LookupIcon("gtk-close", 16, IconLookupFlags.NoSvg).LoadIcon()));
                button.Clicked += OnClicked;
                button.Relief = ReliefStyle.None;
                button.SetSizeRequest(20, 20);
                container.PackEnd(button);
            }
            
            WebKitView view = new WebKitView();
            view.notebook = this;
            ScrolledWindow view_swin = new ScrolledWindow(new Adjustment(0, 0, 0, 0, 0, 0), new Adjustment(0, 0, 0, 0, 0, 0));
            view_swin.Add(view);
            view_swin.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
            view.Render(content);
            
            hash.Add(container, view);
            
            AppendPage(view_swin, container);
            container.ShowAll();
            
            ShowAll();
            if ( NPages > 1 ) {
                ShowTabs = true;
            } else {
                ShowTabs = false;
            }
        }
        
        public void LoadUri(string uri) {
            HBox container = new HBox();
            Label label = new Label(uri);
            container.PackStart(label);
            Button button = new Button(new Image(IconTheme.Default.LookupIcon("gtk-close", 16, IconLookupFlags.NoSvg).LoadIcon()));
            button.Clicked += OnClicked;
            button.Relief = ReliefStyle.None;
            button.SetSizeRequest(20, 20);
            container.PackEnd(button);
            
            WebKitView view = new WebKitView();
            view.notebook = this;
            ScrolledWindow view_swin = new ScrolledWindow(new Adjustment(0, 0, 0, 0, 0, 0), new Adjustment(0, 0, 0, 0, 0, 0));
            view_swin.Add(view);
            view_swin.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
            view.RenderUri(uri);
            
            hash.Add(container, view);
            
            AppendPage(view_swin, container);
            container.ShowAll();
            
            ShowAll();
            if ( NPages > 1 ) {
                ShowTabs = true;
            } else {
                ShowTabs = false;
            }
        }
        
        private void OnClicked(object obj, EventArgs args) {
            Button b = (Button)obj;
            Widget o = (Widget)hash[b.Parent];
            int pos = PageNum(o.Parent);
            RemovePage(pos);
            hash.Remove(b.Parent);
            
            ShowAll();
            if ( NPages > 1 ) {
                ShowTabs = true;
            } else {
                ShowTabs = false;
            }
        }
    }
}
