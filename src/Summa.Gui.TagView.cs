// TagView.cs
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
using System.Linq;
using Gtk;
using Gdk;

using Summa.Core;
using Summa.Data;
using Summa.Gui;

namespace Summa.Gui {
    public class TagView : TreeView {
        public ArrayList Tags;
        
        private TreeIter tagiter;
        
        public string Selected {
            get {
                TreeModel selectmodel;
                Selection.GetSelected(out selectmodel, out tagiter);
                string val = (string)Summa.Core.Application.TagStore.GetValue(tagiter, 1);
                return val;
            }
        }
        
        public TagView() {
            // set up the liststore for the view
            Model = Summa.Core.Application.TagStore;
            
            // set up the columns for the view, and hide them. 
            InsertColumn(-1, "Pix", new CellRendererPixbuf(), "pixbuf", 0);
            InsertColumn(-1, "Tag", new CellRendererText(), "text", 1);
            HeadersVisible = false;
            
            // set up for NewsKit
            Tags = Feeds.GetTags();
            
            if ( Summa.Core.Application.Browsers.Count == 0 ) {
                TreeIter tagiter;
                tagiter = Summa.Core.Application.TagStore.Append();
                Summa.Core.Application.TagStore.SetValue(tagiter, 0, IconTheme.Default.LookupIcon("feed-presence", 16, IconLookupFlags.NoSvg).LoadIcon());
                Summa.Core.Application.TagStore.SetValue(tagiter, 1, "All feeds");
                
                tagiter = Summa.Core.Application.TagStore.Append();
                Summa.Core.Application.TagStore.SetValue(tagiter, 0, IconTheme.Default.LookupIcon("system-search", (int)IconSize.Menu, IconLookupFlags.NoSvg).LoadIcon());
                Summa.Core.Application.TagStore.SetValue(tagiter, 1, "Searches");
            
                foreach ( string tag in Tags ) {
                    if ( tag != "All" ) {
                        AppendTag(tag);
                    }
                }
            }
            
            Database.FeedChanged += OnFeedChanged;
            
            TreeIter iter;
            Summa.Core.Application.TagStore.GetIterFirst(out iter);
            Selection.SelectIter(iter);
        }
        
        private void OnFeedChanged(object obj, EventArgs args) {
            Update();
        }
        
        public void Update() {
            ArrayList utags = Feeds.GetTags();
            
            foreach ( string tag in utags ) {
                if ( tag != "All" ) {
                    if ( !Tags.Contains(tag) ) {
                        AppendTag(tag);
                    }
                }
            }
            Tags = Feeds.GetTags();
        }
        
        public void AppendTag(string tag) {
            TreeIter iter;
            iter = Summa.Core.Application.TagStore.Append();
            Summa.Core.Application.TagStore.SetValue(iter, 0, IconTheme.Default.LookupIcon("tag", 16, IconLookupFlags.NoSvg).LoadIcon());
            Summa.Core.Application.TagStore.SetValue(iter, 1, tag);
        }
    }
}
