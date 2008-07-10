// FirstRun.cs
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
using System.Collections;

namespace Summa.Gui {
    public class FirstRun : Gtk.Window {
        private Gtk.VBox vbox;
        private Gtk.HBox hbox;
        private Gtk.HButtonBox bbox;
        private Gtk.Image image;
        private Gtk.Table table;
        private Gtk.Label label;
        private Gtk.FileChooserDialog fcdialog;
        private Gtk.FileChooserButton fcbutton;
        private Gtk.Button cancel_button;
        private Gtk.Button add_button;
        
        public FirstRun() : base(Gtk.WindowType.Toplevel) {
            IconName = Gtk.Stock.Convert;
            Title = "Import OPML file";
            
            Resizable = false;
            BorderWidth = 6;
            
            vbox = new Gtk.VBox(false, 6);
            Add(vbox);
            
            hbox = new Gtk.HBox(false, 6);
            vbox.PackStart(hbox);
            
            image = new Gtk.Image("dialog-question", Gtk.IconSize.Dialog);
            hbox.PackStart(image);
            
            table = new Gtk.Table(2, 3, false);
            table.RowSpacing = 6;
            hbox.PackStart(table);
            
            label = new Gtk.Label("To import feeds, select an OPML file.");
            label.LineWrap = true;
            table.Attach(label, 1, 2, 0, 1);
            
            fcdialog = new Summa.Gui.OpmlDialog();
            fcbutton = new Gtk.FileChooserButton(fcdialog);
            table.Attach(fcbutton, 1, 2, 1, 2);
            
            bbox = new Gtk.HButtonBox();
            vbox.PackEnd(bbox);
            
            cancel_button = new Gtk.Button(Gtk.Stock.Cancel);
            cancel_button.Clicked += OnCancel;
            bbox.PackEnd(cancel_button);
            
            add_button = new Gtk.Button(Gtk.Stock.Convert);
            add_button.Clicked += new EventHandler(OnImport);
            bbox.PackEnd(add_button);
        }
        
        private void OnCancel(object obj, EventArgs args) {
            Destroy();/*
            Summa.Data.register_feed_source("http://planet.gnome.org/rss20.xml");
            Summa.Data.register_feed_source("http://newsrss.bbc.co.uk/rss/newsonline_world_edition/front_page/rss.xml");
            Summa.Data.register_feed_source("http://rss.slashdot.org/Slashdot/slashdot");*/
        }
        
        private void OnImport(object obj, EventArgs args) {
            Title = "Importing...";
            bbox.Sensitive = false;
            image.Sensitive = false;
            label.Sensitive = false;
            foreach ( Summa.Gui.Browser browser in Summa.Core.Application.Browsers ) {
                browser.Sensitive = false;
            }
            
            ProgressBar pb = new Gtk.ProgressBar();
            table.Attach(pb, 1, 2, 1, 2);
            pb.Show();
            
            if ( fcdialog.Uris.Length > 0 ) {
                string uri = fcdialog.Uri;
                ArrayList feeds = Summa.Data.Core.ImportOpml(uri);
                double step = 1.0/feeds.Count;
                double progress = 0.0;
                
                foreach ( string feed in feeds ) {
                    Summa.Core.Application.Notifier.Notify("Importing feed \""+feed+"\"");
                    while ( Gtk.Application.EventsPending() ) {
                        Gtk.Main.Iteration();
                    }
                    
                    bool it_worked = true;
                    
                    try {
                        Summa.Data.Core.RegisterFeed(feed);
                        it_worked = true;
                    } catch ( Summa.Core.Exceptions.BadFeed e ) {
                        Summa.Core.Log.LogException(e);
                        Summa.Core.Application.Notifier.Notify("Import of feed \""+feed+"\" failed.");
                        it_worked = false;
                    }
                    
                    pb.Fraction = progress;
                    progress += step;
                    
                    if ( it_worked ) {
                        Summa.Core.Application.Notifier.Notify("Import of feed\""+feed+"\" was successful.");
                        
                        while ( Gtk.Application.EventsPending() ) {
                            Gtk.Main.Iteration();
                        }
                    }
                }
            }
            foreach ( Summa.Gui.Browser browser in Summa.Core.Application.Browsers ) {
                browser.FeedView.Update();
                browser.Sensitive = true;
            }
            Destroy();
        }
    }
}
