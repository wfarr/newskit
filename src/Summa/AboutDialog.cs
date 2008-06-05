///* /home/eosten/Summa/Summa/AboutDialog.cs
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
using Gdk;

namespace Summa {
    public class AboutDialog : Gtk.AboutDialog {
        public AboutDialog() {
            Version = "0.0.0";
	    	Website = "http://code.google.com/p/newskit/";
	    	WebsiteLabel = "http://code.google.com/p/newskit/";
	    	License = "LGPL";
	    	
	    	string[] authors = new string[2];
	    	authors[0] = "Ethan Osten";
            authors[1] = "Will Farrington";
	    	
	    	Authors = authors;
	    	Comments = "Aggregate and read RSS feeds";
	    	
	    	IconTheme i = Gtk.IconTheme.Default;
	    	
	    	Gdk.Pixbuf image_window = i.LoadIcon("add", 0, Gtk.IconLookupFlags.NoSvg);
	    	
	    	LogoIconName = "internet-news-reader";
	    	Icon = image_window;
	    	
	    	//Response += new EventHandler(OnHide);
        }
		
		private void OnHide(object obj, EventArgs args) {
			Hide();
		}
    }
}
