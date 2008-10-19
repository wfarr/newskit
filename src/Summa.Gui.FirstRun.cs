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
using System.Collections;
using System.Threading;
using Gtk;

using Summa.Core;
using Summa.Core.Exceptions;
using Summa.Data;
using Summa.Gui;

namespace Summa.Gui {
    public class ProgressEventArgs : EventArgs {
        public double Progress;
        
        public ProgressEventArgs() {}
    }
    
    public class FirstRun : Window {
        private VBox vbox;
        private HBox hbox;
        private HButtonBox bbox;
        private Image image;
        private Table table;
        private Label label;
        private FileChooserDialog fcdialog;
        private FileChooserButton fcbutton;
        private Button cancel_button;
        private Button add_button;
        private ProgressBar pb;
        
        private ArrayList failed_feeds;
        
        public FirstRun() : base(WindowType.Toplevel) {
            IconName = Stock.Convert;
            Title = "Import OPML file";
            
            Resizable = false;
            BorderWidth = 6;
            
            vbox = new VBox(false, 6);
            Add(vbox);
            
            hbox = new HBox(false, 6);
            vbox.PackStart(hbox);
            
            image = new Image("dialog-question", IconSize.Dialog);
            hbox.PackStart(image);
            
            table = new Table(2, 3, false);
            table.RowSpacing = 6;
            hbox.PackStart(table);
            
            label = new Label("To import feeds, select an OPML file.");
            label.LineWrap = true;
            table.Attach(label, 1, 2, 0, 1);
            
            fcdialog = new OpmlDialog();
            fcbutton = new FileChooserButton(fcdialog);
            table.Attach(fcbutton, 1, 2, 1, 2);
            
            bbox = new HButtonBox();
            vbox.PackEnd(bbox);
            
            cancel_button = new Button(Stock.Cancel);
            cancel_button.Clicked += OnCancel;
            bbox.PackEnd(cancel_button);
            
            add_button = new Button(Stock.Convert);
            add_button.Clicked += new EventHandler(OnImport);
            bbox.PackEnd(add_button);
        }
        
        private void OnCancel(object obj, EventArgs args) {
            Destroy();
        }
        
        private void OnImport(object obj, EventArgs args) {
            Title = "Importing...";
            add_button.Sensitive = false;
            
            pb = new ProgressBar();
            table.Attach(pb, 1, 2, 1, 2);
            pb.Show();
            
            Thread thread = new Thread(ImportThread);
            thread.Start();
        }
        
        private void ImportThread() {
            failed_feeds = new ArrayList();
            
            if ( fcdialog.Uris.Length > 0 ) {
                string uri = fcdialog.Uri;
                ArrayList feeds = Feeds.ImportOpml(uri);
                double step = 1.0/feeds.Count;
                double progress = 0.0;
                
                foreach ( string feed in feeds ) {
                    NotificationEventArgs args = new NotificationEventArgs();
                    args.Message = "Importing feed \""+feed+"\"";
                    Gtk.Application.Invoke(this, args, new EventHandler(OnNotify));
                    
                    bool it_worked = true;
                    
                    try {
                        Feeds.RegisterFeed(feed);
                        it_worked = true;
                    } catch ( BadFeed e ) {
                        Log.Exception(e);
                        
                        args = new NotificationEventArgs();
                        args.Message = "Import of feed \""+feed+"\" failed";
                        Gtk.Application.Invoke(this, args, new EventHandler(OnNotify));
                        
                        failed_feeds.Add(feed);
                        
                        it_worked = false;
                    }
                    
                    ProgressEventArgs pargs = new ProgressEventArgs();
                    pargs.Progress = progress;
                    Gtk.Application.Invoke(this, pargs, new EventHandler(OnProgress));
                    
                    progress += step;
                    
                    if ( it_worked ) {
                        args = new NotificationEventArgs();
                        args.Message = "Import of feed \""+feed+"\" was successful";
                        Gtk.Application.Invoke(this, args, new EventHandler(OnNotify));
                    }
                }
            }
            Gtk.Application.Invoke(this, new EventArgs(), new EventHandler(OnCancel));
            Gtk.Application.Invoke(this, new EventArgs(), new EventHandler(OnDialog));
        }
        
        private void OnDialog(object obj, EventArgs args) {
            Window md = new MessageDialog(failed_feeds);
            md.ShowAll();
        }
        
        private void OnNotify(object obj, EventArgs args) {
            NotificationEventArgs iargs = (NotificationEventArgs)args;
            Notifier.Notify(iargs.Message);
        }
        
        private void OnProgress(object obj, EventArgs args) {
            ProgressEventArgs pargs = (ProgressEventArgs)args;
            pb.Fraction = pargs.Progress;
        }
    }
}
