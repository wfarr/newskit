// AboutDialog.cs
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
using Gdk;

using Summa.Core;
using Summa.Gui;

namespace Summa.Gui {
    public class AboutDialog : Gtk.AboutDialog {
        public AboutDialog() {
            Version = "0.1.0";
            Website = "http://code.google.com/p/newskit/";
            WebsiteLabel = "http://code.google.com/p/newskit/";
            License = "LGPL";
            ProgramName = "Summa";
            
            string[] authors = new string[2];
            authors[0] = "Ethan Osten";
            authors[1] = "Will Farrington";
            
            Authors = authors;
            Comments = StringCatalog.AboutComments;
            
            IconTheme i = IconTheme.Default;
            
            Pixbuf image_window = i.LoadIcon("add", 0, IconLookupFlags.NoSvg);
            
            LogoIconName = "summa";
            Icon = image_window;
            
            Response += new ResponseHandler(OnHide);
        }
        
        private void OnHide(object obj, ResponseArgs args) {
            Hide();
        }
    }
}
