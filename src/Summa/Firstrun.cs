///* /home/eosten/Summa/Summa/Firstrun.cs
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
using Gtk;
using NewsKit;
using System.Collections;

namespace Summa {
	public class Firstrun : Gtk.Window {
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
		private Summa.Browser browser;
		
		public Firstrun(Summa.Browser browse) : base(Gtk.WindowType.Toplevel) {
			IconName = Gtk.Stock.Convert;
            browser = browse;
			TransientFor = browser;
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
			
			fcdialog = new Summa.OpmlDialog();
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
			NewsKit.register_feed_source("http://planet.gnome.org/rss20.xml");
			NewsKit.register_feed_source("http://newsrss.bbc.co.uk/rss/newsonline_world_edition/front_page/rss.xml");
			NewsKit.register_feed_source("http://rss.slashdot.org/Slashdot/slashdot");*/
		}
		
		private void OnImport(object obj, EventArgs args) {
			Title = "Importing...";
			bbox.Sensitive = false;
			image.Sensitive = false;
			label.Sensitive = false;
			browser.Sensitive = false;
			
			ProgressBar pb = new Gtk.ProgressBar();
			table.Attach(pb, 1, 2, 1, 2);
			pb.Show();
			
			if ( fcdialog.Uris.Length > 0 ) {
				string uri = fcdialog.Uri;
				string[] feeds = NewsKit.Daemon.ImportOpml(uri);
				double step = 1.0/feeds.Length;
				double progress = 0.0;
				
				foreach ( string feed in feeds ) {
					browser.statusbar.Push(browser.contextid, "Importing feed \""+feed+"\"");
					while ( Gtk.Application.EventsPending() ) {
						Gtk.Main.Iteration();
					}
					
					bool it_worked = true;
					
					/*try {*/
					NewsKit.Feed uid = NewsKit.Daemon.RegisterFeed(feed);
					it_worked = true;
					/*} catch ( Error ex ) {
						browser.statusbar.push(browser.contextid, "Import of feed \""+feed+"\" failed.");
						browser.contextid++;
						it_worked = false;
						string uid = "0";
					}*/
					
					pb.Fraction = progress;
					progress += step;
					
					if ( it_worked ) {
						//browser.contextid++;
						
						while ( Gtk.Application.EventsPending() ) {
							Gtk.Main.Iteration();
						}
					}
				}
			}
			/*browser.feedview.update();
			browser.set_sensitive(true);*/
			Destroy();
		}
	}
}
