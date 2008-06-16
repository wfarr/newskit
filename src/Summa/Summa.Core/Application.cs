/* Application.cs
 *
 * Copyright (C) 2008  Ethan Osten
 *
 * This library is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation, either version 2.1 of the License, or
 * (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with this library.  If not, see <http://www.gnu.org/licenses/>.
 *
 * Author:
 *     Ethan Osten <senoki@gmail.com>
 */

using System;
using Gtk;

namespace Summa {
    namespace Core {
        public static class Application {
            public static Summa.Gui.Browser Browser;
            public static Summa.Gui.StatusIcon StatusIcon;
            public static Summa.Core.Updater Updater;
            
            public static void Main() {
                Gtk.Application.Init();
                
                Browser = new Summa.Gui.Browser();
                StatusIcon = new Summa.Gui.StatusIcon();
                Updater = new Summa.Core.Updater();
                
                Browser.ShowAll();
                
                string xml = @"<?xml version=""1.0"" encoding=""utf-8"" standalone=""yes"" ?>
<feed xmlns=""http://www.w3.org/2005/Atom"">

	<title>Planet GNOME</title>
	<link rel=""self"" href=""http://planet.gnome.org/atom.xml""/>
	<link href=""http://planet.gnome.org/""/>
	<id>http://planet.gnome.org/atom.xml</id>
	<updated>2007-12-09T20:40:08+00:00</updated>
	<generator uri=""http://www.planetplanet.org/"">Planet/2.0 +http://www.planetplanet.org</generator>
    
    <entry xml:lang=""en"">
		<title type=""html"">Still no Ogg support?</title>
		<link href=""http://www.figuiere.net/hub/blog/?2007/12/09/585-still-no-ogg""/>
		<id>http://www.figuiere.net/hub/blog/?2007/12/09/585-still-no-ogg</id>
		<updated>2007-12-09T19:49:26+00:00</updated>
		<content type=""html"">
&lt;img src=&quot;http://planet.gnome.org/heads/hub.png&quot; alt=&quot;&quot; align=&quot;right&quot; style=&quot;float: right;&quot;&gt;&lt;p&gt;I ran into this &lt;a href=&quot;https://bugs.maemo.org/show_bug.cgi?id=176#c48&quot;&gt;bugzilla comment&lt;/a&gt; for Maemo, concerning the lack of Ogg support in Maemo on the Nokia internet tablets (it is really more about that specific comment and the linked PDF file than the bug itself).&lt;/p&gt;		</content>
		<author>
			<name>Hubert Figuiere</name>
			<uri>http://www.figuiere.net/hub/blog/</uri>
		</author>
		<source>
			<title type=""html"">Diary of a CrazyFrench</title>
			<link rel=""self"" href=""http://www.figuiere.net/hub/blog/rss.php""/>
			<id>http://www.figuiere.net/hub/blog/rss.php</id>
			<updated>2007-12-09T20:39:19+00:00</updated>
		</source>
	</entry>
    
	<entry xml:lang=""en"">
		<title type=""html"">Still no Ogg support?</title>
		<link href=""http://www.figuiere.net/hub/blog/?2007/12/09/585-still-no-ogg-support""/>
		<id>http://www.figuiere.net/hub/blog/?2007/12/09/585-still-no-ogg-support</id>
		<updated>2007-12-09T19:49:26+00:00</updated>
		<content type=""html"">
&lt;img src=&quot;http://planet.gnome.org/heads/hub.png&quot; alt=&quot;&quot; align=&quot;right&quot; style=&quot;float: right;&quot;&gt;&lt;p&gt;I ran into this &lt;a href=&quot;https://bugs.maemo.org/show_bug.cgi?id=176#c48&quot;&gt;bugzilla comment&lt;/a&gt; for Maemo, concerning the lack of Ogg support in Maemo on the Nokia internet tablets (it is really more about that specific comment and the linked PDF file than the bug itself).&lt;/p&gt;		</content>
		<author>
			<name>Hubert Figuiere</name>
			<uri>http://www.figuiere.net/hub/blog/</uri>
		</author>
		<source>
			<title type=""html"">Diary of a CrazyFrench</title>
			<link rel=""self"" href=""http://www.figuiere.net/hub/blog/rss.php""/>
			<id>http://www.figuiere.net/hub/blog/rss.php</id>
			<updated>2007-12-09T20:39:19+00:00</updated>
		</source>
	</entry>

	<entry xml:lang=""en"">
		<title type=""html"">Bits &amp;amp; Pieces</title>
		<link href=""http://blogs.gnome.org/pbor/2007/12/09/bits-pieces/""/>
		<id>http://blogs.gnome.org/pbor/2007/12/09/bits-pieces/</id>
		<updated>2007-12-09T16:36:07+00:00</updated>
		<content type=""html"">
&lt;img src=&quot;http://planet.gnome.org/heads/pbor.png&quot; alt=&quot;&quot; align=&quot;right&quot; style=&quot;float: right;&quot;&gt;&lt;p&gt;Last period was fairly busy and I didn&amp;#8217;t get much time to hack on gnome related stuff&amp;#8230; I hope to be able to catch up a bit in the next weeks, especially to take care of some great patches that are waiting in bugzilla.&lt;/p&gt;
&lt;h3&gt;GNOME Talk @ IBM Technical Conference&lt;/h3&gt;
&lt;p&gt;Thanks to Fabio Marzocca I was approached by IBM to give a talk about GNOME and ubuntu at a linux technical conference held at their IBM Forum in Segrate. Despite the fact I am not an experienced presenter the talk went farly well, slides are &lt;a href=&quot;http://www.gnome.org/~pborelli/slides/IBM-gnome.odp&quot;&gt;available here&lt;/a&gt;. At the meeting I also had the chance to meet &lt;a href=&quot;http://ar.linux.it&quot;&gt;Alessandro Rubini&lt;/a&gt; of &lt;a href=&quot;http://lwn.net/Kernel/LDD3/&quot;&gt;Linux Device Drivers&lt;/a&gt; fame.&lt;/p&gt;
&lt;h3&gt;Google GHOP GtkSourceView themes&lt;/h3&gt;
&lt;p&gt;One of the tasks accepted for the Google GHOP program was writing five color schemes for gtksourceview. &lt;a href=&quot;http://dev.compiz-fusion.org/~wfarr/&quot;&gt;Will Farrington&lt;/a&gt; claimed the task and today delivered five good looking themes. Well done Will!&lt;br /&gt;
Check them out at on the&lt;a href=&quot;http://live.gnome.org/GtkSourceView/StyleSchemes&quot; title=&quot;wiki page&quot;&gt; wiki page&lt;/a&gt;.&lt;/p&gt;
&lt;p&gt;I especially like &lt;em&gt;Cobalt&lt;/em&gt;, so here is a screenie.&lt;a href=&quot;http://blogs.gnome.org/pbor/files/2007/12/cobalt.png&quot; title=&quot;gedit cobalt theme&quot;&gt;&lt;img src=&quot;http://blogs.gnome.org/pbor/files/2007/12/cobalt.thumbnail.png&quot; alt=&quot;gedit cobalt theme&quot; /&gt;&lt;/a&gt;&lt;/p&gt;
&lt;p&gt;Speaking of Google, this week I also received a Google SoC t-shirt. Thanks Google!&lt;/p&gt;
&lt;h3&gt;gedit without libgnome&lt;/h3&gt;
&lt;p&gt;&lt;a href=&quot;http://www.tielie.com&quot;&gt;MIkael Hermansson&lt;/a&gt; filed a &lt;a href=&quot;http://bugzilla.gnome.org/show_bug.cgi?id=500922&quot;&gt;patch&lt;/a&gt; in bugzilla to conditionally disable the libgnome dependency of gedit. As &lt;a href=&quot;http://blogs.gnome.org/pbor/2007/09/24/delivering-the-killing-blow-to-libgnome&quot;&gt;mentioned before&lt;/a&gt; I really look forward to disabling libgnome uncoditioally and this is definitely a step in the right direction as we work through the remaining issues.&lt;/p&gt;		</content>
		<author>
			<name>Paolo Borelli</name>
			<uri>http://blogs.gnome.org/pbor</uri>
		</author>
		<source>
			<title type=""html"">Club Silencio</title>
			<subtitle type=""html"">pbor's little blue box</subtitle>
			<link rel=""self"" href=""http://blogs.gnome.org/pbor/feed/""/>
			<id>http://blogs.gnome.org/pbor/feed/</id>
			<updated>2007-12-09T20:38:31+00:00</updated>
		</source>
	</entry>

	<entry xml:lang=""en"">
		<title type=""html"">Dennis Kucinich</title>
		<link href=""http://www.ogmaciel.com/?p=423""/>
		<id>http://www.ogmaciel.com/?p=423</id>
		<updated>2007-12-09T16:28:31+00:00</updated>
		<content type=""html"">
&lt;img src=&quot;http://planet.gnome.org/heads/ogmaciel.png&quot; alt=&quot;&quot; align=&quot;right&quot; style=&quot;float: right;&quot;&gt;&lt;p&gt; &amp;#8221;&lt;em&gt;one Nation under God, indivisible, with liberty and justice for all.&amp;#8221;&lt;/em&gt;&lt;/p&gt;
&lt;p&gt;&lt;span lang=&quot;EN-GB&quot;&gt;&lt;font size=&quot;2&quot;&gt;&lt;a href=&quot;http://www.dennis4president.com/home/&quot;&gt;Dennis Kucinich&lt;/a&gt;! &amp;#8216;nough said!&lt;br /&gt;
&lt;/font&gt;&lt;/span&gt;&lt;/p&gt;		</content>
		<author>
			<name>Og Maciel</name>
			<uri>http://www.ogmaciel.com</uri>
		</author>
		<source>
			<title type=""html"">Journal Of An Open Sourcee</title>
			<subtitle type=""html"">Senseless Bahblings Of An Open Sourcee</subtitle>
			<link rel=""self"" href=""http://www.ogmaciel.com/?feed=rss2""/>
			<id>http://www.ogmaciel.com/?feed=rss2</id>
			<updated>2007-12-09T18:08:07+00:00</updated>
		</source>
	</entry>

	<entry xml:lang=""en"">
		<title type=""html"">Glib and Serial Port Help</title>
		<link href=""http://www.burtonini.com/blog/computers/serial-2007-12-09-14-15""/>
		<id>http://www.burtonini.com/blog/computers/serial-2007-12-09-14-15</id>
		<updated>2007-12-09T14:15:00+00:00</updated>
		<content type=""html"">
&lt;img src=&quot;http://planet.gnome.org/heads/ross.png&quot; alt=&quot;&quot; align=&quot;right&quot; style=&quot;float: right;&quot;&gt;&lt;p&gt;
      So I want to write a small program to read and write data to a serial port
      (115200 baud, 8N1) using &lt;tt&gt;GIOChannel&lt;/tt&gt;.  No matter what I try, I
      can't seem to get input callbacks from the channel. :( Does anyone have
      any small working examples of using a &lt;tt&gt;GIOChannel&lt;/tt&gt; to drive a
      serial port I can look at?
    &lt;/p&gt;

    &lt;p&gt;
      &lt;small&gt;NP: &lt;cite&gt;Mungo's Hi-Fi at Exodus&lt;/cite&gt;&lt;/small&gt;
    &lt;/p&gt;		</content>
		<author>
			<name>Ross Burton</name>
			<uri>http://www.burtonini.com/blog</uri>
		</author>
		<source>
			<title type=""html"">Ross Burton</title>
			<subtitle type=""html"">A potted account of Ross' life</subtitle>
			<link rel=""self"" href=""http://www.burtonini.com/blog/index.rss2""/>
			<id>http://www.burtonini.com/blog/index.rss2</id>
			<updated>2007-12-09T20:36:17+00:00</updated>
		</source>
	</entry>

	<entry xml:lang=""en"">
		<title type=""html"">Open Letter to Google: API Documentation Indexing</title>
		<link href=""http://blogs.gnome.org/cneumair/2007/12/09/open-letter-to-google-api-documentation-indexing/""/>
		<id>http://blogs.gnome.org/cneumair/2007/12/09/open-letter-to-google-api-documentation-indexing/</id>
		<updated>2007-12-09T12:03:48+00:00</updated>
		<content type=""html"">
&lt;p&gt;To all my blog readers: In case you wonder why I&amp;#8217;ve been so silent for months, as a student of electrical engineering I&amp;#8217;m holding a tutorial course at university (electromagnetism), and I&amp;#8217;m absolving a C++/Qt/OpenCV hands-on training at university that deals with OpenCV-powered pattern recognition and image processing.&lt;/p&gt;
&lt;p&gt;The aim is to write an autonomous robot control for a LIDAR+mono-optical+thermo-cam robot that has to absolve various tasks, that require you to think of smart solutions. A very good oppurtunity to improve your practical  engineering skills and accomodate to the industry requirements. Not all solutions that are smart are applicable, and vice versa.&lt;/p&gt;
&lt;p&gt;Now, my open letter&lt;/p&gt;
&lt;p&gt;Dear Google,&lt;/p&gt;
&lt;p&gt;it&amp;#8217;s great that one can use Google to search through online API documentation. I can enter &amp;#8220;gtk_widget_new&amp;#8221; and hits on library.gnome.org will show up in the results. This makes Google an effective and omnipresent devhelp-alternative.&lt;/p&gt;
&lt;p&gt;However, I have some griefs:&lt;/p&gt;
&lt;p&gt;* Often, PageRank thinks the most relevant hits are mailing list posts. This is true only for badly-documented API. One can of course specify site:library.gnome.org, but this is tedious&lt;/p&gt;
&lt;p&gt;* Even as you narrow down your results to the official API docs, PageRank fails to get the most relevant/recent documentation, and instead often brings up very outdated documentation (before we had library.gnome.org, often 1.0 platform API docs were preferred!)&lt;/p&gt;
&lt;p&gt;What about an&amp;#8221;api:&amp;#8230;&amp;#8221; keyword (like &amp;#8220;define:&amp;#8230;&amp;#8221;) that is associated with an indexer, that is aware of library.gnome.org, docs.trolltech.com, go-mono, meaemo, X11 and MSDN documentation (and also all the POSIX and linux low-level stuff), and knows the various version numbers and flavors of libraries?&lt;/p&gt;
&lt;p&gt;You&amp;#8217;d do us software developers a major favor, and get even more fanboys! :).&lt;/p&gt;
&lt;p&gt;Best regards,&lt;/p&gt;
&lt;p&gt;Christian Neumair&lt;/p&gt;		</content>
		<author>
			<name>Christian Neumair</name>
			<uri>http://blogs.gnome.org/cneumair</uri>
		</author>
		<source>
			<title type=""html"">Christian Neumair</title>
			<subtitle type=""html"">Just another GNOME Blogs weblog</subtitle>
			<link rel=""self"" href=""http://blogs.gnome.org/cneumair/feed/""/>
			<id>http://blogs.gnome.org/cneumair/feed/</id>
			<updated>2007-12-09T20:22:44+00:00</updated>
		</source>
	</entry>

	<entry xml:lang=""en"">
		<title type=""html"">A few new Reinteract features</title>
		<link href=""http://blog.fishsoup.net/2007/12/09/a-few-new-reinteract-features/""/>
		<id>http://blog.fishsoup.net/2007/12/09/a-few-new-reinteract-features/</id>
		<updated>2007-12-09T04:29:13+00:00</updated>
		<content type=""html"">
&lt;img src=&quot;http://planet.gnome.org/heads/owen.png&quot; alt=&quot;&quot; align=&quot;right&quot; style=&quot;float: right;&quot;&gt;&lt;div class=&quot;snap_preview&quot;&gt;&lt;p&gt; I had some time today to finish some &lt;a href=&quot;http://www.reinteract.org&quot;&gt;Reinteract&lt;/a&gt; features I&amp;#8217;ve been working on over the last few weeks, namely completion and mouse-over tooltips on the editor contents. Some screenshots:&lt;/p&gt;
&lt;p&gt;Completion&lt;br /&gt;
&lt;a href=&quot;http://owtaylor.files.wordpress.com/2007/12/reinteract-completion.png&quot; title=&quot;Reinteract Completion&quot;&gt;&lt;img src=&quot;http://owtaylor.files.wordpress.com/2007/12/reinteract-completion.thumbnail.png&quot; alt=&quot;Reinteract Completion&quot; /&gt;&lt;/a&gt;&lt;a href=&quot;http://owtaylor.files.wordpress.com/2007/12/reinteract-docs.png&quot; title=&quot;Reinteract Docs Mouseover&quot;&gt;&lt;/a&gt;&lt;/p&gt;
&lt;p&gt;Tooltip showing documentation&lt;br /&gt;
&lt;a href=&quot;http://owtaylor.files.wordpress.com/2007/12/reinteract-docs.png&quot; title=&quot;Reinteract Docs Mouseover&quot;&gt;&lt;img src=&quot;http://owtaylor.files.wordpress.com/2007/12/reinteract-docs.thumbnail.png&quot; alt=&quot;Reinteract Docs Mouseover&quot; /&gt;&lt;/a&gt;&lt;a href=&quot;http://owtaylor.files.wordpress.com/2007/12/reinteract-mouse-over.png&quot; title=&quot;Reinteract Variable Mouseover&quot;&gt;&lt;/a&gt;&lt;/p&gt;
&lt;p&gt;Tooltip showing variable contents&lt;br /&gt;
&lt;a href=&quot;http://owtaylor.files.wordpress.com/2007/12/reinteract-mouse-over.png&quot; title=&quot;Reinteract Variable Mouseover&quot;&gt;&lt;img src=&quot;http://owtaylor.files.wordpress.com/2007/12/reinteract-mouse-over.thumbnail.png&quot; alt=&quot;Reinteract Variable Mouseover&quot; /&gt;&lt;/a&gt;&lt;/p&gt;
&lt;p&gt;The majority of features in my &lt;a href=&quot;http://www.reinteract.org/trac/wiki/CompletionDesign&quot;&gt;completion design notes&lt;/a&gt; are now implemented. (The design took the bold and innovative approach of &amp;#8220;copy how completion works in Eclipse&amp;#8221;). The main thing that&amp;#8217;s still missing is assist for function parameters. But I&amp;#8217;ll probably leave that to the side for the moment and turn to &lt;a href=&quot;http://www.reinteract.org/trac/wiki/NotebookDesign&quot;&gt;notebooks&lt;/a&gt;. And catching up with some bugs fixes. For one thing, the code used to format the variable tooltips should be easily reusable to fix a reported problem where accidentally displaying a large array in Reinteract makes it dead-slow.&lt;/p&gt;
&lt;/div&gt;		</content>
		<author>
			<name>Owen Taylor</name>
			<uri>http://blog.fishsoup.net</uri>
		</author>
		<source>
			<title type=""html"">fishsoup</title>
			<subtitle type=""html"">Owen Taylor on Coding, Food, etc.</subtitle>
			<link rel=""self"" href=""http://blog.fishsoup.net/feed/?mrss=off""/>
			<id>http://blog.fishsoup.net/feed/?mrss=off</id>
			<updated>2007-12-09T04:39:43+00:00</updated>
		</source>
	</entry>

	<entry xml:lang=""en"">
		<title type=""html"">firefox nitpicks, revisited</title>
		<link href=""http://tieguy.org/blog/2007/12/08/firefox-nitpicks-revisited/""/>
		<id>http://tieguy.org/blog/2007/12/08/firefox-nitpicks-revisited/</id>
		<updated>2007-12-09T04:03:45+00:00</updated>
		<content type=""html"">
&lt;img src=&quot;http://planet.gnome.org/heads/luis.png&quot; alt=&quot;&quot; align=&quot;right&quot; style=&quot;float: right;&quot;&gt;&lt;p&gt;[I&amp;#8217;ve studied an immense amount the past 38 hours; this is a brain break while watching the terrible but enjoyable &lt;em&gt;Gladiator.&lt;/em&gt;]&lt;/p&gt;
&lt;p&gt;Someone recently linked to my old post on &lt;a href=&quot;http://tieguy.org/blog/2007/03/19/firefox-nitpicks/&quot;&gt;firefox nitpicks&lt;/a&gt;; I&amp;#8217;ve also been using firefox three this week. Time to revisit&amp;#8230;&lt;/p&gt;
&lt;p&gt;Fixed:&lt;/p&gt;
&lt;ul&gt;
&lt;li&gt;&lt;strong&gt;direct bookmarks from url bar:&lt;/strong&gt; ffox&amp;#8217;s new &amp;#8216;awesomebar&amp;#8217; is really quite nice. Finally caught up to ephy :)&lt;/li&gt;
&lt;li&gt;&lt;strong&gt;bookmarks, generally: &lt;/strong&gt; tags! woohoo! again, ffox catches up.&lt;/li&gt;
&lt;li&gt;&lt;strong&gt;theming:&lt;/strong&gt; new ffox is impressively integrated with gtk/gnome themes. There are some details that aren&amp;#8217;t right- HIGginess generally- but still, overall, very impressive- can&amp;#8217;t have been easy.&lt;/li&gt;
&lt;/ul&gt;
&lt;p&gt;Not fixed/improved:&lt;/p&gt;
&lt;ul&gt;
&lt;li&gt;&lt;strong&gt;printing dialog:&lt;/strong&gt; still not native; gets more painful now that the gnome printing dialog exposes lots of cups functionality. (Who knew my printer could do double-sided printing? Sadly, ffox still doesn&amp;#8217;t, though apparently epiphany doesn&amp;#8217;t expose the full functionality either.)&lt;/li&gt;
&lt;li&gt;&lt;strong&gt;history in new tab:&lt;/strong&gt; still doesn&amp;#8217;t work. Yargh. At least there is a plugin.&lt;/li&gt;
&lt;li&gt;&lt;strong&gt;window icon:&lt;/strong&gt; booo.&lt;/li&gt;
&lt;li&gt;&lt;strong&gt;clutter:&lt;/strong&gt; seems to be about the same, overall. Still really should follow epiphany&amp;#8217;s lead of separating preferences and personal information.&lt;/li&gt;
&lt;/ul&gt;
&lt;p&gt;Ahead(?) of epiphany:&lt;/p&gt;
&lt;ul&gt;
&lt;li&gt;&lt;strong&gt;prism:&lt;/strong&gt; &lt;a href=&quot;http://labs.mozilla.com/2007/10/prism/&quot;&gt;prism&lt;/a&gt; is awesome; ephy should have done something like it &lt;a href=&quot;http://mail.gnome.org/archives/epiphany-list/2006-May/msg00017.html&quot;&gt;ages ago&lt;/a&gt;. (Not actually integrated into ffox3 yet, but&amp;#8230; details.)&lt;/li&gt;
&lt;li&gt;&lt;strong&gt;speed:&lt;/strong&gt; I actually have no idea how fast ephy is these days, but I can&amp;#8217;t ever remember having a browser launch as fast as ffox3 does. Very impressive. Probably helps that I&amp;#8217;m back to nearly-no-plugins state as a result of the upgrade.&lt;/li&gt;
&lt;/ul&gt;
&lt;p&gt;The ffox guys should be proud- this looks like a very nice release, if still not perfect.&lt;/p&gt;		</content>
		<author>
			<name>Luis Villa</name>
			<uri>http://tieguy.org/blog</uri>
		</author>
		<source>
			<title type=""html"">Luis Villa's Blog</title>
			<subtitle type=""html"">Ramblings on law school in New York, free software, and the spaces in between.</subtitle>
			<link rel=""self"" href=""http://tieguy.org/blog/feed/""/>
			<id>http://tieguy.org/blog/feed/</id>
			<updated>2007-12-09T04:07:00+00:00</updated>
		</source>
	</entry>

	<entry xml:lang=""en"">
		<title type=""html"">Clutter software branch</title>
		<link href=""http://chrislord.net/blog/Software/clutter-software-branch.enlighten""/>
		<id>http://chrislord.net/blog/Software/clutter-software-branch</id>
		<updated>2007-12-09T00:43:00+00:00</updated>
		<content type=""html"">
&lt;img src=&quot;http://planet.gnome.org/heads/cwiiis.png&quot; alt=&quot;&quot; align=&quot;right&quot; style=&quot;float: right;&quot;&gt;&lt;p&gt;I haven't done any significant work on this since I last blogged about it (hopefully the viewport function works correctly, sub-textures work correctly and I added affine texture-mapping), but I've committed the code to a branch. So it's even easier to try now! You can check it out from http://svn.o-hand.com/repos/clutter/branches/clutter-software/ and configure with --with-flavour=sogl-sdl - Requires SDL (it'd be cool if someone made this X11 instead of SDL :)) and a pretty meaty PC. The code is pretty slow but there's *plenty* of room for optimisation... Shouldn't be much work to get text working (just need to add support for setting texture alignment), that'll probably be the next thing I do, then get blending working better...
&lt;/p&gt;		</content>
		<author>
			<name>Chris Lord</name>
			<uri>http://chrislord.net/blog</uri>
		</author>
		<source>
			<title type=""html"">ChrisLord.net</title>
			<subtitle type=""html"">This blog details my current software developments, and other related issues.</subtitle>
			<link rel=""self"" href=""http://chrislord.net/blog/?flav=rss20""/>
			<id>http://chrislord.net/blog/?flav=rss20</id>
			<updated>2007-12-09T20:37:05+00:00</updated>
			<rights type=""html"">Copyright 2006,2007 Chris Lord</rights>
		</source>
	</entry>

	<entry>
		<title type=""html"">Wanted: Game Console</title>
		<link href=""http://tirania.org/blog/archive/2007/Dec-08.html""/>
		<id>http://tirania.org/blog/archive/2007/Dec-08.html</id>
		<updated>2007-12-08T22:34:00+00:00</updated>
		<content type=""html"">
&lt;img src=&quot;http://planet.gnome.org/heads/miguel.png&quot; alt=&quot;&quot; align=&quot;right&quot; style=&quot;float: right;&quot;&gt;&lt;p&gt;Having missed on games for the past 15 years last year I
	finally got myself a Wii.   Other than Wii Sports and now
	Metroid 3, I have yet to find anything worth playing.   

	&lt;p&gt;Nat recommended a Nintendo DS, and you guys had
	some &lt;a href=&quot;http://tirania.org/blog/archive/2007/Sep-12.html&quot;&gt;great
	suggestions&lt;/a&gt; back in September.  So far the only one I
	liked was Metroid Hunters (the control is so similar to the
	Wii, that its a pleasure to play) and am still making my way
	through the Sudoku's on the DS.

	&lt;p&gt;I tried Halo3 but with its up/down/left/right-cursor-like
	technology to aim at enemies, it feels almost like am playing
	with a keyboard in 1988.  After using the Wii for
	point-and-shoot, anything short of that for point-and-shoot
	feels unnatural.  Like when the dentist stuffs your mouth with
	junk and still tries to have a conversation with you or trying
	to use a bendy straw for snorkeling.
	
	&lt;p&gt;So am looking at expanding my Console Empire at home and
	purchasing either a PS3 or an
	XBox360.  &lt;a href=&quot;http://abock.org&quot;&gt;Aaron&lt;/a&gt; insists that I
	should not get the PS3 because Blue Ray this-and-that which I
	do not particularly care about.

	&lt;p&gt;Aaron also claims that eventually you get used to
	up/down/left/right.  I guess I will have to live with that, as
	the Wii is barely getting any games worth playing.  And as a
	rule, I do not play anything that glorifies war, but am OK
	shooting at strange looking aliens. 

	&lt;p&gt;So am stuck, and willing to learn to use those unnatural
	controls on the PS3 and the XBox if there is something worth
	playing.

	&lt;p&gt;Dear readers, what should I get, PS3 or XBox?  And which
	games are worth playing?  I do not care about movies on
	demand, or whatever other TV features they are trying to sell
	me, I already have Tivo HD and Tivo with DVD playback and
	recording.

	&lt;p&gt;You can either email me
	or &lt;a href=&quot;http://groups.google.com/group/tiraniaorg-blog-comments/post&quot;&gt;post
	here&lt;/a&gt; your suggestions.&lt;/p&gt;&lt;/p&gt;&lt;/p&gt;&lt;/p&gt;&lt;/p&gt;&lt;/p&gt;&lt;/p&gt;&lt;/p&gt;		</content>
		<author>
			<name>Miguel de Icaza</name>
			<email>miguel@gnome.org</email>
			<uri>http://tirania.org/blog/index.html</uri>
		</author>
		<source>
			<title type=""html"">Miguel de Icaza</title>
			<subtitle type=""html"">Miguel de Icaza's web log</subtitle>
			<link rel=""self"" href=""http://www.tirania.org/blog/miguel.rss2""/>
			<id>http://www.tirania.org/blog/miguel.rss2</id>
			<updated>2007-12-08T22:36:18+00:00</updated>
			<rights type=""html"">Miguel de Icaza</rights>
		</source>
	</entry>

	<entry xml:lang=""en"">
		<title type=""html"">Jürg Billeter’s Vala Tinymail demo E-mail client</title>
		<link href=""http://pvanhoof.be/blog/index.php/2007/12/08/jurg-billeters-vala-tinymail-demo-e-mail-client""/>
		<id>http://pvanhoof.be/blog/index.php/2007/12/08/jurg-billeters-vala-tinymail-demo-e-mail-client</id>
		<updated>2007-12-08T21:27:28+00:00</updated>
		<content type=""html"">
&lt;img src=&quot;http://planet.gnome.org/heads/pvanhoof.png&quot; alt=&quot;&quot; align=&quot;right&quot; style=&quot;float: right;&quot;&gt;&lt;p&gt;The demo E-mail client can be found &lt;a href=&quot;http://tinymail.org/trac/tinymail/browser/trunk/tests/vala-demo/tinymail-vala-test.vala&quot;&gt;here&lt;/a&gt;, the Vala bindings for &lt;a href=&quot;http://tinymail.org&quot;&gt;Tinymail&lt;/a&gt; can be found &lt;a href=&quot;http://tinymail.org/trac/tinymail/browser/trunk/bindings/vala&quot;&gt;here&lt;/a&gt;.&lt;/p&gt;
&lt;p&gt;Many thanks to Jürg!&lt;/p&gt;
&lt;p&gt;&lt;img src=&quot;http://pvanhoof.be/files/tny-vala.png&quot; /&gt;&lt;/p&gt;
&lt;p&gt;&lt;a href=&quot;http://tinymail.org/trac/tinymail/browser/trunk/bindings/vala/README&quot;&gt;Here are&lt;/a&gt; the instructions on how to get the binding regenerated.&lt;/p&gt;
&lt;p&gt;Just do this to get the Vala demo E-mail client running:&lt;/p&gt;
&lt;ul&gt;
&lt;li&gt;Get Vala&amp;#8217;s latest release &lt;a href=&quot;http://live.gnome.org/Vala&quot;&gt;here&lt;/a&gt;. You don&amp;#8217;t need anything special. Just configure, make and sudo make install it with &amp;#8211;prefix=/opt/vala for example.&lt;/li&gt;
&lt;li&gt;export VALAC=/opt/vala/bin/valac&lt;/li&gt;
&lt;li&gt;export PATH=$PATH:/opt/vala/bin&lt;/li&gt;
&lt;li&gt;Build Tinymail: ./configure &amp;#8211;prefix=/opt/tinymail &amp;#8211;enable-vala-bindings &amp;#038;&amp;#038; make &amp;#038;&amp;#038; sudo make install&lt;/li&gt;
&lt;li&gt;Setup your Tinymail (demoui) accounts (described &lt;a href=&quot;http://tinymail.org/trac/tinymail/browser/trunk/README&quot;&gt;here&lt;/a&gt;)&lt;/li&gt;
&lt;li&gt;cd bindings/vala ; make &amp;#038;&amp;#038; sudo make install&lt;/li&gt;
&lt;li&gt;cd tests/vala-demo&lt;/li&gt;
&lt;li&gt;make ; ./tinymail-vala-test&lt;/li&gt;
&lt;/ul&gt;		</content>
		<author>
			<name>Philip Van Hoof</name>
			<uri>http://pvanhoof.be/blog</uri>
		</author>
		<source>
			<title type=""html"">Phenomena in the days of Philip</title>
			<subtitle type=""html"">Just another WordPress weblog</subtitle>
			<link rel=""self"" href=""http://pvanhoof.be/blog/wp-rss2.php""/>
			<id>http://pvanhoof.be/blog/wp-rss2.php</id>
			<updated>2007-12-09T11:55:36+00:00</updated>
		</source>
	</entry>

	<entry xml:lang=""en"">
		<title type=""html"">The Power of Penguins</title>
		<link href=""http://www.alobbs.com/1296/The_Power_of_Penguins.html""/>
		<id>http://www.alobbs.com/1296/The_Power_of_Penguins.html</id>
		<updated>2007-12-08T21:27:24+00:00</updated>
		<content type=""html"">
&lt;p&gt;I know that it is silly, but this image has made me laugh out loud. Check it out: &amp;quot;The power of Penguins&amp;quot; :-)&lt;/p&gt;  &lt;div align=&quot;center&quot;&gt;   &lt;img src=&quot;http://www.alobbs.com/images/power_of_penguins.jpg&quot; alt=&quot;&quot; /&gt; &lt;/div&gt;  &lt;p&gt;Seen at &lt;a href=&quot;http://ffffound.com/image/bb963970accd17a1d5f470f574fa105e53d444aa&quot;&gt;ffffound&lt;/a&gt;.&lt;/p&gt;		</content>
		<author>
			<name>Alvaro Lopez Ortega</name>
			<uri>http://www.alobbs.com</uri>
		</author>
		<source>
			<title type=""html"">Alvaro's blog</title>
			<subtitle type=""html"">Alvaro Lopez Ortega's blog</subtitle>
			<link rel=""self"" href=""http://www.alobbs.com/rss20.php""/>
			<id>http://www.alobbs.com/rss20.php</id>
			<updated>2007-12-09T20:38:12+00:00</updated>
		</source>
	</entry>

	<entry xml:lang=""en"">
		<title type=""html"">HTML5 media support with GStreamer</title>
		<link href=""http://www.atoker.com/blog/2007/12/08/html5-media-support-with-gstreamer/""/>
		<id>http://www.atoker.com/blog/2007/12/08/html5-media-support-with-gstreamer/</id>
		<updated>2007-12-08T20:55:00+00:00</updated>
		<content type=""html"">
&lt;p&gt;What do you get when you take &lt;a href=&quot;http://live.gnome.org/WebKitGtk&quot;&gt;WebKit/GTK+&lt;/a&gt;, add &lt;a href=&quot;http://gstreamer.freedesktop.org/&quot;&gt;GStreamer&lt;/a&gt; and finish off with a sprinkling of code from &lt;a href=&quot;http://clutter-project.org/&quot;&gt;Clutter&lt;/a&gt;?&lt;/p&gt;
&lt;p&gt;&lt;a href=&quot;http://www.atoker.com/blog/wp-content/uploads/2007/12/wkvid.png&quot; title=&quot;WebKit Video&quot;&gt;&lt;img src=&quot;http://www.atoker.com/blog/wp-content/uploads/2007/12/wkvid.png&quot; alt=&quot;WebKit Video&quot; border=&quot;0&quot; /&gt;&lt;/a&gt;&lt;/p&gt;
&lt;p&gt;&lt;img src=&quot;http://www.atoker.com/blog/wp-content/uploads/2007/12/gstreamer-logo-40.png&quot; alt=&quot;GStreamer logo&quot; align=&quot;left&quot; /&gt;Pierre-Luc Beaudoin of Collabora has been working on a GStreamer-based media backend for WebKit.&lt;/p&gt;
&lt;p&gt;Last week, we landed his work (&lt;strike&gt;&lt;a href=&quot;http://bugs.webkit.org/show_bug.cgi?id=16145&quot;&gt;#16145&lt;/a&gt;&lt;/strike&gt;), which adds support for the &lt;a href=&quot;http://www.whatwg.org/specs/web-apps/current-work/multipage/section-embedded.html&quot;&gt;WHATWG HTML5 video/audio specification&lt;/a&gt; allowing streaming media to be embedded in web pages without the need for plug-ins.&lt;/p&gt;
&lt;p&gt;The last couple of days, I&amp;#8217;ve been integrating this media backend with our Cairo-based graphics pipeline (&lt;a href=&quot;http://bugs.webkit.org/show_bug.cgi?id=16356&quot;&gt;#16356&lt;/a&gt;). The result is that streaming web video can now be &lt;strong&gt;CSS&lt;/strong&gt; transformed, embedded in &lt;strong&gt;SVG&lt;/strong&gt;, bounced around the page and generally manipulated with &lt;strong&gt;JavaScript&lt;/strong&gt;.&lt;/p&gt;
&lt;p&gt;And the screenshot? It&amp;#8217;s &lt;a href=&quot;http://ftp.gnome.org/pub/GNOME/teams/guadec/2007/videos/Federico%20Mena-Quintero%20-%20Sabayon.ogg&quot;&gt;a video&lt;/a&gt; (one of many &lt;a href=&quot;http://blogs.gnome.org/thos/2007/12/06/what-happened-to-the-guadec-videos-this-year/&quot;&gt;GUADEC flicks&lt;/a&gt; rescued by Thomas Wood) of &lt;a href=&quot;http://www.gnome.org/~federico/news.html&quot;&gt;Federico&lt;/a&gt; talking about Sabayon, played back natively in Epiphany at an angle of 45° and with an alpha value of 0.6.&lt;/p&gt;
&lt;p&gt;The semi-transparent user controls superimposed on the video are defined entirely in HTML/CSS complete with &lt;a href=&quot;http://webkit.org/blog/138/css-animation/&quot;&gt;animated transition effects&lt;/a&gt; (WebKit-only right now, other browsers will fall back gracefully).&lt;/p&gt;
&lt;p&gt;Exciting stuff!&lt;/p&gt;		</content>
		<author>
			<name>Alp Toker</name>
			<uri>http://www.atoker.com/blog</uri>
		</author>
		<source>
			<title type=""html"">Alp Toker</title>
			<subtitle type=""html"">There is a third way</subtitle>
			<link rel=""self"" href=""http://www.atoker.com/blog/feed/""/>
			<id>http://www.atoker.com/blog/feed/</id>
			<updated>2007-12-08T23:36:23+00:00</updated>
		</source>
	</entry>

	<entry>
		<title type=""html"">Device Icons</title>
		<link href=""http://wayofthemonkey.com/?date=2007-12-08""/>
		<id>http://wayofthemonkey.com/?date=2007-12-08</id>
		<updated>2007-12-08T19:41:22+00:00</updated>
		<content type=""html"">
&lt;p&gt;With the &lt;a href=&quot;http://tango.freedesktop.org/Tango_Fridays&quot;&gt;Tango
Friday&lt;/a&gt; this week being about device icons, I decided to go ahead and clean
up a few of the names in gnome-icon-theme-extras and add some new stuff to the
&lt;a&gt;devices.txt&lt;/a&gt; addendum
to the naming spec, as well as clean some names up. The file now lists every
iPod in every color, with the exception of the versions before the 4th gen
click wheel iPods. I've also added several of the phone icons which are already
in gnome-icon-theme-extras, and a few of the multimedia-player icons as well.
&lt;/p&gt;
&lt;p&gt;The Tango Friday event itself went pretty ok. Unfortunately only a few
people could make it onto IRC and do anything though. Some nice icons, and
the beginnings of icons, came out of it though. Several HTC phone icons, an
iPAQ hx4700, Samsung F700, and Sony DSC-P200 camera icon. And Michael Monreal
went the classic route and drew an old-school Nokia 3310. Here's a sample of
the HTC Kaiser icon that was drawn:&lt;/p&gt;
&lt;p align=&quot;center&quot;&gt;
&lt;img src=&quot;http://cubestuff.files.wordpress.com/2007/12/htckaiser-gallery2.png&quot; /&gt;
&lt;/p&gt;&lt;br /&gt;&lt;a href=&quot;http://del.icio.us/post?url=http://wayofthemonkey.com/?date=2007-12-08&amp;amp;title=Device Icons&quot;&gt;&lt;img src=&quot;http://wayofthemonkey.com/images/delicous.png&quot; border=&quot;0&quot; title=&quot;Post this article to del.icio.us&quot; /&gt;&lt;/a&gt;		</content>
		<author>
			<name>Rodney Dawes</name>
			<uri>http://wayofthemonkey.com/</uri>
		</author>
		<source>
			<title type=""html"">dobey's blog</title>
			<subtitle type=""html"">dobey's blog</subtitle>
			<link rel=""self"" href=""http://wayofthemonkey.com/blog/rss.php""/>
			<id>http://wayofthemonkey.com/blog/rss.php</id>
			<updated>2007-12-09T20:38:40+00:00</updated>
		</source>
	</entry>

	<entry xml:lang=""en"">
		<title type=""html"">Limbo: Why users are more error-prone with git than other VCSes</title>
		<link href=""http://blogs.gnome.org/newren/2007/12/08/limbo-why-users-are-more-error-prone-with-git-than-other-vcses/""/>
		<id>http://blogs.gnome.org/newren/2007/12/08/limbo-why-users-are-more-error-prone-with-git-than-other-vcses/</id>
		<updated>2007-12-08T18:09:30+00:00</updated>
		<content type=""html"">
&lt;img src=&quot;http://planet.gnome.org/heads/elijah.png&quot; alt=&quot;&quot; align=&quot;right&quot; style=&quot;float: right;&quot;&gt;&lt;p&gt;Limbo is a term I use but VCS authors don&amp;#8217;t.  However, that&amp;#8217;s because they tend to ignore a certain state that exists in all major VCSes (and give it no name because they tend to ignore it) despite the fact that this state seems to be the largest source of errors.  I call this state limbo.&lt;/p&gt;
&lt;h3&gt;How to make git behave like other VCSes&lt;/h3&gt;
&lt;p&gt;Most potential git users probably don&amp;#8217;t want to read this whole page, and would like their knowledge from usage of other VCSes to apply without learning how the index and limbo are different in git than their previous VCS (despite the really cool extra functionality it brings).  This can be done by&lt;/p&gt;
&lt;ul&gt;
&lt;li&gt;Always using &lt;strong&gt;git diff HEAD&lt;/strong&gt; instead of &lt;em&gt;git diff&lt;/em&gt;&lt;/li&gt;
&lt;p&gt;and&lt;/p&gt;
&lt;li&gt;Always using &lt;strong&gt;git commit -a&lt;/strong&gt; instead of &lt;em&gt;git commit&lt;/em&gt;&lt;/li&gt;
&lt;/ul&gt;
&lt;p&gt;Either make sure you &lt;em&gt;always&lt;/em&gt; remember those extra arguments, or come back and read this page when you get a nasty surprise.&lt;/p&gt;
&lt;h3&gt;The concept of Limbo&lt;/h3&gt;
&lt;p&gt;VCS users are accustomed to thinking of using their VCS in terms of two states &amp;#8212; a working copy where local changes are made, and the repository where the changes are saved.  However, the working copy is split into three sets (see also &lt;a href=&quot;http://blogs.gnome.org/newren/2007/12/01/the-concepts-a-user-must-learn-to-understand-existing-vcses&quot;&gt;VCS concepts&lt;/a&gt;):&lt;/p&gt;
&lt;ul&gt;
&lt;li&gt; (explicitly) &lt;strong&gt;ignored&lt;/strong&gt; &amp;#8212; files inside your working copy that you explicitly told the VCS system to not track&lt;/li&gt;
&lt;li&gt; &lt;strong&gt;index&lt;/strong&gt; &amp;#8212; the content in your working copy that you asked the VCS to track; this is the portion of your working copy that will be saved when you commit (in CVS, this is done using the CVS/Entries files)&lt;/li&gt;
&lt;li&gt; &lt;strong&gt;limbo&lt;/strong&gt; &amp;#8212; not explicitly ignored, and not explicitly added.  This is stuff in your working copy that won&amp;#8217;t be checked in when you commit but you haven&amp;#8217;t told the VCS to ignore, which typically includes newly created files.&lt;/li&gt;
&lt;/ul&gt;
&lt;p&gt;The first state is identical across all major VCSes.  The second two states are identical across cvs, svn, bzr, hg, and likely others.  But git splits the index and limbo differently.&lt;/p&gt;
&lt;p&gt;One could imagine a VCS which just automatically saves &lt;em&gt;all&lt;/em&gt; changes that aren&amp;#8217;t in an explicitly ignored file (including newly created files) whenever a developer commits, i.e. a VCS where there is no limbo state.  None of the major VCSes do this, however.  There are various rationales for the existence of limbo: maybe developers are too lazy to add new files to the ignored list, perhaps they are unaware of some autogenerated files, or perhaps the VCS only has one ignore list and developers want to share it but not include their own temporary files in such a shared list.  Whatever the reason, limbo is there in all major VCSes.&lt;/p&gt;
&lt;h3&gt;Changes in limbo are a large source of user error&lt;/h3&gt;
&lt;p&gt;The problem with limbo is that changes in this state are, in my experience, the cause of the most errors with users.  If you create a new file and forget to explicitly add it, then it won&amp;#8217;t be included in your commit (happens with all the major VCSes).  Naturally, even those familiar with their VCS forget to do that from time to time.  This always seems to happen when other changes were committed that depend on the new files, and it always happens just before the relevant developers go on vacation&amp;#8230;leaving things in a broken state for me to deal with.  (And sure, I return the favor on occasion when I simply forget to add new files.)&lt;/p&gt;
&lt;h3&gt;A powerful feature of git&lt;/h3&gt;
&lt;p&gt;Unlike other VCSes, git only commits what you explicitly tell it to.  This means that without taking additional steps, the command &amp;#8220;git commit&amp;#8221; will commit nothing (in this particular case it typically complains that there&amp;#8217;s nothing to commit and aborts).  git also gives you a lot of fine-grained control over what to commit, more than most other VCSes.  In particular, you can mark all the changes of a given file for subsequent committing, but unlike other VCSes this only means that you are marking the &lt;em&gt;current&lt;/em&gt; contents of that file for commit; any further changes to the same file will not be included in subsequent commits unless separately added.  Additionally, recent versions of git allow the developer to mark subsets of changes in an existing file for commit (pulling a handy feature from darcs).  The power of this fine-grained choose-what-to-commit functionality is made possible due to the fact that git enables you to generate three different kinds of diffs: (1) just the changes marked for commit (&lt;em&gt;git diff &amp;#8211;cached&lt;/em&gt;), (2) just the changes you&amp;#8217;ve made to files beyond what has been marked for commit (&lt;em&gt;git diff&lt;/em&gt;), or (3) all the changes since the last commit (&lt;em&gt;git diff HEAD&lt;/em&gt;).&lt;/p&gt;
&lt;p&gt;This fine-grained control can come in handy in a variety of special cases:&lt;/p&gt;
&lt;ul&gt;
&lt;li&gt;When doing conflict resolution from large merges (or even just reviewing a largish patch from a new contributor), hunks of changes can be categorized into known-to-be-good and still-needs-review subsets.&lt;/li&gt;
&lt;li&gt;It makes it easier to keep &amp;#8220;dirty&amp;#8221; changes in your working copy for a long time without committing them.&lt;/li&gt;
&lt;li&gt;When adding a feature or refactoring (or otherwise making changes to several different sections of the code), you can mark some changes as known-to-be-good and then continue making further changes or even adding temporary debugging snippets.&lt;/li&gt;
&lt;/ul&gt;
&lt;p&gt;These are features that would have helped me considerably in some GNOME development tasks I&amp;#8217;ve done in the past.&lt;/p&gt;
&lt;h3&gt;How git is more problematic&lt;/h3&gt;
&lt;p&gt;This decision to only commit changes that are explicitly added, and doing so at content boundaries rather than file boundaries, amounts to a shift in the boundary between the index and limbo.  With limbo being much larger in git, there is also more room for user error.  In particular, while this allows for a powerful feature in git noted above, it also comes with some nasty gotchas in common use cases as can be seen in the following scenarios:&lt;/p&gt;
&lt;ul&gt;
&lt;li&gt;Only new files included in the commit
&lt;ol&gt;
&lt;li&gt;Edit bar&lt;/li&gt;
&lt;li&gt;Create foo&lt;/li&gt;
&lt;li&gt;Run &lt;em&gt;git add foo&lt;/em&gt;&lt;/li&gt;
&lt;li&gt;Run &lt;em&gt;git commit&lt;/em&gt;&lt;/li&gt;
&lt;/ol&gt;
&lt;p&gt;In this set of steps, users of other VCSes will be surprised that after step 4 the changes to bar were not included in the commit.  git only commits changes when explicitly asked.  (This can be avoided by either running &lt;em&gt;git add bar&lt;/em&gt; before committing, or running &lt;em&gt;git commit -a&lt;/em&gt;.  The -a flag to commit means &amp;#8220;Act like other VCSes &amp;#8212; commit all changes in any files included in the previous commit&amp;#8221;.)&lt;/p&gt;&lt;/li&gt;
&lt;li&gt;Missing changes in the commit
&lt;ol&gt;
&lt;li&gt;Create/edit the file foo&lt;/li&gt;
&lt;li&gt;Run &lt;em&gt;git add foo&lt;/em&gt;&lt;/li&gt;
&lt;li&gt;Edit foo some more&lt;/li&gt;
&lt;li&gt;Run &lt;em&gt;git commit&lt;/em&gt;&lt;/li&gt;
&lt;/ol&gt;
&lt;p&gt;In this set of steps, users of other VCSes will be surprised that after step 4 the version of foo that was commited was the version that existed at the time step 2 was run; not the version that existed when step 4 was run.  That&amp;#8217;s because step 2 is translated to mean mark the changes &lt;em&gt;currently&lt;/em&gt; in the file foo for commit.  (This can be avoided by running &lt;em&gt;git add foo&lt;/em&gt; again before committing, or running &lt;em&gt;git commit -a&lt;/em&gt; for step 4.)&lt;/p&gt;&lt;/li&gt;
&lt;li&gt;Missing edits in the generated patch
&lt;ol&gt;
&lt;li&gt;Edit the tracked file foo&lt;/li&gt;
&lt;li&gt;Run &lt;em&gt;git add foo&lt;/em&gt;&lt;/li&gt;
&lt;li&gt;Edit foo some more&lt;/li&gt;
&lt;li&gt;Run &lt;em&gt;git diff&lt;/em&gt;&lt;/li&gt;
&lt;/ol&gt;
&lt;p&gt;In this set of steps, users of other VCSes will be surprised that at step 4 they only get a list of changes to foo made in step 3.  To get a list of changes to foo made since the last commit, run &lt;em&gt;git diff HEAD&lt;/em&gt; instead.&lt;/p&gt;&lt;/li&gt;
&lt;li&gt;Missing file in the generated patch
&lt;ol&gt;
&lt;li&gt;Create a new file called foo&lt;/li&gt;
&lt;li&gt;Run &lt;em&gt;git add foo&lt;/em&gt;&lt;/li&gt;
&lt;li&gt;Run &lt;em&gt;git diff&lt;/em&gt;&lt;/li&gt;
&lt;/ol&gt;
&lt;p&gt;In this set of steps, users of other VCSes will be surprised that at step 3 the file foo is not included in the diff (unless changes have been made to foo since step 2, but then only those additional changes will be shown).  To get foo included in the diff, run &lt;em&gt;git diff HEAD&lt;/em&gt; instead.&lt;/p&gt;&lt;/li&gt;
&lt;/ul&gt;
&lt;p&gt;These gotchas are there in addition to the standard gotcha exhibited in all the major VCSes:&lt;/p&gt;
&lt;h3&gt;How all the major VCSes are problematic&lt;/h3&gt;
&lt;ul&gt;
&lt;li&gt;Missing file in the commit
&lt;ol&gt;
&lt;li&gt;Edit bar&lt;/li&gt;
&lt;li&gt;Create a new file called foo&lt;/li&gt;
&lt;li&gt;Run &lt;em&gt;vcs commit&lt;/em&gt; (where &lt;em&gt;vcs&lt;/em&gt; is cvs, svn, hg, bzr&amp;#8230;see below about git)&lt;/li&gt;
&lt;/ol&gt;
&lt;p&gt;In this set of steps, the edits in step 1 will be included in the commit, but the file foo will not be.  The user must first run &lt;em&gt;vcs add foo&lt;/em&gt; (again, replacing &lt;em&gt;vcs&lt;/em&gt; with the relevant VCS being used) before committing in order to get foo included in the commit.&lt;/p&gt;
&lt;p&gt;It turns out that git actually can help the user in this case due to its default to only commit what it is explicitly told to commit; meaning that in this case it won&amp;#8217;t commit anything and tell the user that it wasn&amp;#8217;t told to commit anything.  However, since nearly every tutorial on git[*] says to use &lt;em&gt;git commit -a&lt;/em&gt;, users include that flag most the time (60% of the time?  98%?).  Due to that training, they&amp;#8217;ll still get this nasty bug.  However, they&amp;#8217;re going to forget or neglect this flag sometimes, so they also get the new gotchas above.&lt;/p&gt;&lt;/li&gt;
&lt;/ul&gt;
&lt;p&gt;[*] Recent versions of the &lt;a href=&quot;http://www.kernel.org/pub/software/scm/git/docs/tutorial.html&quot;&gt;official git tutorial&lt;/a&gt; being the only exception I&amp;#8217;ve run across.  It&amp;#8217;s fairly thorough (make sure to also read part two), though it isn&amp;#8217;t quite as explicit about the potential gotchas in certain situations.&lt;/p&gt;
&lt;h3&gt;How bzr, hg, and git mitigate these gotchas (and cvs and svn don&amp;#8217;t)&lt;/h3&gt;
&lt;p&gt;These gotchas can be avoided by always running &lt;em&gt;vcs status&lt;/em&gt; (again, replace &lt;em&gt;vcs&lt;/em&gt; with the relevant VCS being used) and looking closely at the states the VCS lists files in.  It turns out bzr, hg, and git are smart here and try to help the user avoid problems by showing the output of the status command when running a plain &lt;em&gt;vcs commit&lt;/em&gt; (at the end of the commit message they are given to edit).  This helps, but isn&amp;#8217;t foolproof; I&amp;#8217;ve somehow glossed over this extra bit of info in the past and still been bit.  Also, I&amp;#8217;ll often either use the &lt;em&gt;-m&lt;/em&gt; flag to specify the commit message on the command line (for tiny personal projects) or a flag to specify taking the commit message from a file (i.e. using &lt;em&gt;-F&lt;/em&gt; in most vcses, &lt;em&gt;-l&lt;/em&gt; in hg).&lt;/p&gt;		</content>
		<author>
			<name>Elijah Newren</name>
			<uri>http://blogs.gnome.org/newren</uri>
		</author>
		<source>
			<title type=""html"">Elijah\'s Blog</title>
			<subtitle type=""html"">Just another GNOME Blogs weblog</subtitle>
			<link rel=""self"" href=""http://blogs.gnome.org/newren/feed/""/>
			<id>http://blogs.gnome.org/newren/feed/</id>
			<updated>2007-12-09T20:09:57+00:00</updated>
		</source>
	</entry>

	<entry xml:lang=""en"">
		<title type=""html"">consistency of translations</title>
		<link href=""http://blogs.gnome.org/aklapper/2007/12/08/consistency-of-translations/""/>
		<id>http://blogs.gnome.org/aklapper/2007/12/08/consistency-of-translations/</id>
		<updated>2007-12-08T17:12:21+00:00</updated>
		<content type=""html"">
&lt;p&gt;one of the translation aims is to provide consistency of terminology. some translation teams have glossaries to achieve this (&lt;a href=&quot;http://live.gnome.org/de/StandardUebersetzungen&quot;&gt;german&lt;/a&gt;, &lt;a href=&quot;http://glossaire.traduc.org/&quot;&gt;french&lt;/a&gt;, for examples). however, reality bites.&lt;br /&gt;
i&amp;#8217;ve started to use the wonderful &lt;a href=&quot;http://www.open-tran.eu/&quot;&gt;Open-Tran.eu project&lt;/a&gt; to search for inconsistencies - enter an english term and see the results from the po files of the different GNOME modules. the german po files have at least five different translations of &amp;#8220;website&amp;#8221;, and also inconsistent translations of strings like &amp;#8220;page setup&amp;#8221;, &amp;#8220;scrollbar&amp;#8221;, &amp;#8220;mount&amp;#8221;, &amp;#8220;eject&amp;#8221; or &amp;#8220;select&amp;#8221;, to mention a few i&amp;#8217;ve searched for. perhaps other languages have a smaller variety of expressions that one can translate a string to, but i thought this information could be useful to share.&lt;/p&gt;		</content>
		<author>
			<name>Andre Klapper</name>
			<uri>http://blogs.gnome.org/aklapper</uri>
		</author>
		<source>
			<title type=""html"">andre klapper's blog. » lang-en</title>
			<subtitle type=""html"">i'm not dead yet, but i'm working on it.</subtitle>
			<link rel=""self"" href=""http://blogs.gnome.org/aklapper/category/lang-en/feed/""/>
			<id>http://blogs.gnome.org/aklapper/category/lang-en/feed/</id>
			<updated>2007-12-09T18:38:31+00:00</updated>
		</source>
	</entry>

	<entry xml:lang=""en"">
		<title type=""html"">Some dude on IRC</title>
		<link href=""http://pvanhoof.be/blog/index.php/2007/12/08/some-dude-on-irc""/>
		<id>http://pvanhoof.be/blog/index.php/2007/12/08/some-dude-on-irc</id>
		<updated>2007-12-08T17:07:59+00:00</updated>
		<content type=""html"">
&lt;img src=&quot;http://planet.gnome.org/heads/pvanhoof.png&quot; alt=&quot;&quot; align=&quot;right&quot; style=&quot;float: right;&quot;&gt;&lt;p&gt;[17:03 today] &amp;lt;juergbi&amp;gt; pvanhoof: i&amp;#8217;m reading email with a vala tinymail client now :)&lt;/p&gt;		</content>
		<author>
			<name>Philip Van Hoof</name>
			<uri>http://pvanhoof.be/blog</uri>
		</author>
		<source>
			<title type=""html"">Phenomena in the days of Philip</title>
			<subtitle type=""html"">Just another WordPress weblog</subtitle>
			<link rel=""self"" href=""http://pvanhoof.be/blog/wp-rss2.php""/>
			<id>http://pvanhoof.be/blog/wp-rss2.php</id>
			<updated>2007-12-09T11:55:36+00:00</updated>
		</source>
	</entry>

	<entry xml:lang=""en"">
		<title type=""html"">Trying to save face on hypocritical foreign policies</title>
		<link href=""http://zaheer.merali.org/articles/2007/12/08/trying-to-save-face-on-hypocritical-foreign-policies/""/>
		<id>http://zaheer.merali.org/articles/2007/12/08/trying-to-save-face-on-hypocritical-foreign-policies/</id>
		<updated>2007-12-08T12:10:16+00:00</updated>
		<content type=""html"">
&lt;img src=&quot;http://planet.gnome.org/heads/zaheer.png&quot; alt=&quot;&quot; align=&quot;right&quot; style=&quot;float: right;&quot;&gt;&lt;p&gt;US Defence secretary said &lt;em&gt;&amp;#8220;There can be little doubt that their destabilising foreign policies are a threat to the interests of the United States, to the interests of every country in the Middle East, and to the interests of all countries within the range of the ballistic missiles Iran is developing.&amp;#8221;&lt;/em&gt; (Source: &lt;a href=&quot;http://news.bbc.co.uk/1/hi/world/middle_east/7134030.stm&quot;&gt;BBC News&lt;/a&gt;)&lt;/p&gt;
&lt;p&gt;which is not very different in words to, and is less true than the following:&lt;/p&gt;
&lt;p&gt;&lt;em&gt;&amp;#8216;There can be little doubt that their destabilising foreign policies are a threat to the interests of every country in the world, especially as they are within the range of the ballistic missiles and bombs that the USA have already developed and used frequently in the past.&amp;#8217;&lt;/em&gt;&lt;/p&gt;		</content>
		<author>
			<name>Zaheer Abbas Merali</name>
			<uri>http://zaheer.merali.org</uri>
		</author>
		<source>
			<title type=""html"">Zaheer Abbas Merali</title>
			<subtitle type=""html"">Random Ramblings</subtitle>
			<link rel=""self"" href=""http://zaheer.merali.org/feed/""/>
			<id>http://zaheer.merali.org/feed/</id>
			<updated>2007-12-08T12:10:16+00:00</updated>
		</source>
	</entry>

	<entry xml:lang=""en"">
		<title type=""html"">Server outages</title>
		<link href=""http://www.barisione.org/blog.html/p=115""/>
		<id>http://www.barisione.org/blog.html/p=115</id>
		<updated>2007-12-08T12:04:53+00:00</updated>
		<content type=""html"">
&lt;img src=&quot;http://planet.gnome.org/heads/barisione.png&quot; alt=&quot;&quot; align=&quot;right&quot; style=&quot;float: right;&quot;&gt;&lt;p&gt;Half a day of downtime is bad, but two major outages in less then ten days are too much! It seems that &lt;a href=&quot;http://www.unixshell.com/&quot;&gt;Unixshell&lt;/a&gt; (barisione.org is a virtual XEN server hosted by them) is having some serious connectivity problems and they don&amp;#8217;t know what redundancy is, even if on their web page they say:&lt;/p&gt;
&lt;blockquote&gt;&lt;p&gt;
Built-in &lt;i&gt;redundancy&lt;/i&gt; through multiple &lt;i&gt;redundant&lt;/i&gt; network connections and &lt;i&gt;redundant&lt;/i&gt; router and switch configuration.
&lt;/p&gt;&lt;/blockquote&gt;
&lt;p&gt;By the way, my server is finally back online!
&lt;/p&gt;		</content>
		<author>
			<name>Marco Barisione</name>
			<uri>http://www.barisione.org/blog.html</uri>
		</author>
		<source>
			<title type=""html"">Marco Barisione's Weblog</title>
			<link rel=""self"" href=""http://www.barisione.org/blog.html?feed=rss2""/>
			<id>http://www.barisione.org/blog.html?feed=rss2</id>
			<updated>2007-12-09T20:35:15+00:00</updated>
		</source>
	</entry>

	<entry>
		<title type=""html"">New TV/New Elisa</title>
		<link href=""http://davyd.livejournal.com/232632.html""/>
		<id>http://davyd.livejournal.com/232632.html</id>
		<updated>2007-12-08T07:25:38+00:00</updated>
		<content type=""html"">
&lt;img src=&quot;http://planet.gnome.org/heads/riff.png&quot; alt=&quot;&quot; align=&quot;right&quot; style=&quot;float: right;&quot;&gt;Our old TV has been making funny noises for a while now and JB Hifi was having a sale, so the other weekend Steph and I bought a new TV (some people may have guessed this from my previous post about connecting it to the PC). The trick to getting it working with the PC was to use a DVI-HDMI cable. That worked straight away, coming up at the resolution of 1280x720 (no modelines required). It could probably be driven at 1360x768 with some work, but it's not really worth it.&lt;br /&gt;&lt;br /&gt;I upgraded the PC we use for media duties with the latest &lt;a href=&quot;http://elisa.fluendo.com/&quot;&gt;Elisa&lt;/a&gt;. The UI has gone reasonably Apple-esque, but is quite usable. No working DAAP (although it browses my Rhythmbox and will download cover art for albums). Perhaps I should experiment with UPnP as a solution instead. Really need to get a non-software based remote control, I think.&lt;br /&gt;&lt;br /&gt;&lt;center&gt;&lt;img src=&quot;http://davyd.ucc.asn.au/images/tv-elisa-2.jpg&quot; /&gt;&lt;br /&gt;&lt;i&gt;the Elisa developers said I should blog this picture&lt;/i&gt;&lt;/center&gt;&lt;br /&gt;I ordered a little unit based off the AMD Geode a month or so back (hasn't arrived yet) to replace the PC that does my Internet and possibly also replace the machine we run Elisa on, although as Matthias says, I wonder if it will be powerful enough. It turns out GStreamer already chokes on the WMV9 1080-line test videos he gave me, even on my desktop. I like the idea of replacing two PCs (one of which is only on sometimes) with one low-power device, so I hope it will be suitable. Otherwise I may have to investigate a low-noise conventional PC that can fulfill both roles (the machine I run Elisa on really is very noisy). The thing is that I'm not really a media junkie, which means I won't be spending a lot of money, which seems to be at odds with the whole media PC industry.		</content>
		<author>
			<name>Davyd Madeley</name>
			<uri>http://davyd.livejournal.com/</uri>
		</author>
		<source>
			<title type=""html"">Weblog</title>
			<subtitle type=""html"">Weblog - LiveJournal.com</subtitle>
			<link rel=""self"" href=""http://davyd.livejournal.com/data/rss""/>
			<id>http://davyd.livejournal.com/data/rss</id>
			<updated>2007-12-09T00:37:01+00:00</updated>
		</source>
	</entry>

	<entry xml:lang=""en"">
		<title type=""html"">i, on the other hand, ate like a whole bag of trail mix for dinner</title>
		<link href=""http://feeds.feedburner.com/~r/joeshaw/~3/196992380/508""/>
		<id>http://joeshaw.org/2007/12/07/508</id>
		<updated>2007-12-08T04:31:16+00:00</updated>
		<content type=""html"">
&lt;img src=&quot;http://planet.gnome.org/heads/joe.png&quot; alt=&quot;&quot; align=&quot;right&quot; style=&quot;float: right;&quot;&gt;&lt;p&gt;&lt;a href=&quot;http://well.blogs.nytimes.com/2007/12/05/a-high-price-for-healthy-food/&quot;&gt;Interesting article&lt;/a&gt; in the New York Times today.  Junk food costs only $1.76 per 1000 kcal, whereas healthy food costs $18.16.  This is in part because junk food is much heavier in sugars and other carbohydrates.  But as &lt;a href=&quot;http://www.nytimes.com/2007/04/22/magazine/22wwlnlede.t.html&quot;&gt;Michael Pollen wrote about in April&lt;/a&gt;, a large part of it is because of the massive subsidies for a specific few crops in the American farm bill.  The end result?  A package of carrots, an unprocessed food and rather straightforward crop, costs significantly more than a package of Twinkies, a massively produced mixture of food (mostly corn)-dervied chemicals.
&lt;/p&gt;		</content>
		<author>
			<name>Joe</name>
			<uri>http://joeshaw.org</uri>
		</author>
		<source>
			<title type=""html"">joe shaw</title>
			<link rel=""self"" href=""http://joeshaw.org/rss.php""/>
			<id>http://joeshaw.org/rss.php</id>
			<updated>2007-12-08T04:37:46+00:00</updated>
			<rights type=""html"">Copyright 2007</rights>
		</source>
	</entry>

	<entry xml:lang=""en-us"">
		<title type=""html"">8 Dec 2007</title>
		<link href=""http://www.advogato.org/person/mjg59/diary.html?start=88""/>
		<id>http://mjg59.livejournal.com/79244.html</id>
		<updated>2007-12-08T04:05:16+00:00</updated>
		<content type=""html"">
&lt;img src=&quot;http://planet.gnome.org/heads/mjg59.png&quot; alt=&quot;&quot; align=&quot;right&quot; style=&quot;float: right;&quot;&gt;When faced with having to give a presentation on genetic disease, what's the best antidote to the depressing facts?&lt;br /&gt;&lt;br /&gt;&lt;img src=&quot;http://www.codon.org.uk/~mjg59/tmp/comic_disease.png&quot; /&gt;&lt;br /&gt;&lt;br /&gt;Comic Sans. Of course.		</content>
		<author>
			<name>Matthew Garrett</name>
			<uri>http://www.advogato.org/person/mjg59/</uri>
		</author>
		<source>
			<title type=""html"">Advogato blog for mjg59</title>
			<subtitle type=""html"">Advogato blog for mjg59</subtitle>
			<link rel=""self"" href=""http://www.advogato.org/person/mjg59/rss.xml""/>
			<id>http://www.advogato.org/person/mjg59/rss.xml</id>
			<updated>2007-12-09T20:36:21+00:00</updated>
		</source>
	</entry>

	<entry xml:lang=""en"">
		<title type=""html"">somebody let boycottnovell know</title>
		<link href=""http://feeds.feedburner.com/~r/joeshaw/~3/196986319/507""/>
		<id>http://joeshaw.org/2007/12/07/507</id>
		<updated>2007-12-08T03:54:50+00:00</updated>
		<content type=""html"">
&lt;img src=&quot;http://planet.gnome.org/heads/joe.png&quot; alt=&quot;&quot; align=&quot;right&quot; style=&quot;float: right;&quot;&gt;&lt;p&gt;GNOME CO-FOUNDER AND NOVELL EMPLOYEE SAYS EXCEL IS &amp;#8220;&lt;a href=&quot;http://www.gnome.org/~federico/news-1998.html#1998-06-20&quot;&gt;CERTAINLY A NICE PIECE OF SOFTWARE&lt;/a&gt;.&amp;#8221;&lt;/p&gt;
&lt;p&gt;When will those Novell guys stop suckling at the Microsoft teat?  It makes me sick.
&lt;/p&gt;		</content>
		<author>
			<name>Joe</name>
			<uri>http://joeshaw.org</uri>
		</author>
		<source>
			<title type=""html"">joe shaw</title>
			<link rel=""self"" href=""http://joeshaw.org/rss.php""/>
			<id>http://joeshaw.org/rss.php</id>
			<updated>2007-12-08T04:37:46+00:00</updated>
			<rights type=""html"">Copyright 2007</rights>
		</source>
	</entry>

	<entry>
		<title type=""html"">engineer</title>
		<link href=""http://davyd.livejournal.com/232325.html""/>
		<id>http://davyd.livejournal.com/232325.html</id>
		<updated>2007-12-08T03:00:09+00:00</updated>
		<content type=""html"">
&lt;img src=&quot;http://planet.gnome.org/heads/riff.png&quot; alt=&quot;&quot; align=&quot;right&quot; style=&quot;float: right;&quot;&gt;UWA results are out early.&lt;br /&gt;&lt;br /&gt;&lt;center&gt;&lt;img src=&quot;http://davyd.ucc.asn.au/images/uwa-sc-completion.png&quot; /&gt;&lt;/center&gt;&lt;br /&gt;As for marks, I got 56 (P) for Analogue Electronics, 69 (CR) for Engineering Management and Industrial Practice, 63 (Cr) for Robotics and Automation and 68 (CR) for my final year project. A few of those marks were a little bit lower than I was hoping, especially the project mark. Unfortunately my weighted average is too low to receive honours, but this was to be expected. It probably would have helped had I not failed as much stuff earlier on in my degree. Still, at least it's done now.		</content>
		<author>
			<name>Davyd Madeley</name>
			<uri>http://davyd.livejournal.com/</uri>
		</author>
		<source>
			<title type=""html"">Weblog</title>
			<subtitle type=""html"">Weblog - LiveJournal.com</subtitle>
			<link rel=""self"" href=""http://davyd.livejournal.com/data/rss""/>
			<id>http://davyd.livejournal.com/data/rss</id>
			<updated>2007-12-09T00:37:01+00:00</updated>
		</source>
	</entry>

	<entry xml:lang=""en"">
		<title type=""html"">Negative numbers in the Google Chart API</title>
		<link href=""http://www.kryogenix.org/days/2007/12/08/negative-numbers-in-the-google-chart-api""/>
		<id>http://www.kryogenix.org/days/2007/12/08/negative-numbers-in-the-google-chart-api</id>
		<updated>2007-12-08T00:37:33+00:00</updated>
		<content type=""html"">
&lt;p&gt;Google&amp;#8217;s new &lt;a href=&quot;http://code.google.com/apis/chart/&quot;&gt;Chart API&lt;/a&gt; is a useful little thing that returns a PNG of a chart based on the URL you feed it, so you can create graphs like this:&lt;/p&gt;
&lt;p&gt;&lt;img src=&quot;http://chart.apis.google.com/chart?cht=lc&amp;#038;chs=200x125&amp;#038;chd=s:helloWorld&amp;#038;chxt=x,y&amp;#038;chxl=0:|0|10|20&quot; alt=&quot;a simple example line graph&quot; /&gt;&lt;/p&gt;
&lt;p&gt;by just specifying the &lt;code&gt;&amp;lt;img&amp;gt; src&lt;/code&gt; attribute as &lt;a href=&quot;http://chart.apis.google.com/chart?cht=lc&amp;#038;chs=200x125&amp;#038;chd=s:helloWorld&amp;#038;chxt=x,y&amp;#038;chxl=0:|0|10|20&quot;&gt;http://chart.apis.google.com/chart?cht=lc&amp;#038;chs=200&amp;#215;125&amp;#038;chd=s:helloWorld&amp;#038;chxt=x,y&amp;#038;chxl=0:|0|10|20&lt;/a&gt;&lt;/p&gt;
&lt;p&gt;However, as &lt;a href=&quot;http://gulopine.gamemusic.org/2007/12/google-chart-api.html&quot;&gt;Marty Alchin rightfully complains about&lt;/a&gt;, it doesn&amp;#8217;t handle negative numbers at all. Obviously Google, being the internet success story that it is, never has any numbers for anything that dip below zero, but not everyone&amp;#8217;s so lucky. However, there are ways to handle negative numbers in the Google Chart API. &lt;/p&gt;
&lt;p&gt;Sort of, anyway. This is a bodge. Hold your nose and dive in. I&amp;#8217;m sure Google will forcibly inject clue into their charting engine at some point, but until then you can sorta-kinda get around the problem like this.&lt;/p&gt;
&lt;p&gt;&lt;img src=&quot;http://chart.apis.google.com/chart?cht=lc&amp;#038;chs=200x125&amp;#038;chd=s:eelloWorld&amp;#038;chco=ff8000&amp;#038;chls=3,6,3&amp;#038;chxt=y&amp;#038;chxl=0:|-10|0|10&amp;#038;chg=0,50,1,0&quot; alt=&quot;a simple line graph with negative numbers&quot; /&gt;&lt;/p&gt;
&lt;p&gt;For line graphs with negative numbers, you need to do two things. First, lie about the values on the y axis (so you display on the graph that the y axis goes between -10 and 10, in the above example, even though it actually goes between 0 and 20). You&amp;#8217;ll obviously need to transform your numbers appropriately, so a data series [-10, -5, 0, 5, 10] should be fed to the graphing engine as [0, 5, 10, 15, 20]. The second thing to do is to draw a horizontal line at 0 on the y axis; if we had real negative number support then that&amp;#8217;s where the x axis should be, and so the extra drawn line &amp;#8220;stands in&amp;#8221; for it. That&amp;#8217;s easy enough to do, using the &lt;a href=&quot;http://code.google.com/apis/chart/#grid&quot;&gt;grid lines &lt;code&gt;chg&lt;/code&gt; parameter&lt;/a&gt;; just make the grid only exist horizontally, and have the grid divide the graph into two 50% parts, with &lt;code&gt;chg=0,50,1,0&lt;/code&gt;. The four parameters 0, 50, 1, 0 mean &amp;#8220;don&amp;#8217;t draw vertical grid lines (0)&amp;#8221;, &amp;#8220;draw a horizontal grid line every 50% of the y axis (50)&amp;#8221;, and &amp;#8220;make the line solid with no gaps (1,0)&amp;#8221;.&lt;/p&gt;
&lt;p&gt;It&amp;#8217;s also possible for bar graphs, although it requires a small amount more ingenuity.&lt;/p&gt;
&lt;p&gt;&lt;img src=&quot;http://chart.apis.google.com/chart?cht=bvs&amp;#038;chd=t:50,50,50,30,50|0,0,0,20,0|20,10,30,0,30&amp;#038;chco=00000000,ff0000,0000ff&amp;#038;chs=200x125&amp;#038;chg=0,51,1,0&amp;#038;chxt=y&amp;#038;chxl=0:|-50|0|50&quot; alt=&quot;a bar graph with negative numbers&quot; /&gt;&lt;/p&gt;
&lt;p&gt;This graph has negatives, right? Well, how it&amp;#8217;s done might be clearer if you see this graph:&lt;/p&gt;
&lt;p&gt;&lt;img src=&quot;http://chart.apis.google.com/chart?cht=bvs&amp;#038;chd=t:50,50,50,30,50|0,0,0,20,0|20,10,30,0,30&amp;#038;chco=006600ff,ff0000,0000ff&amp;#038;chs=200x125&amp;#038;chg=0,51,1,0&amp;#038;chxt=y&amp;#038;chxl=0:|-50|0|50&quot; alt=&quot;a bar graph without negative numbers, with the parts 'below the line' coloured in&quot; /&gt;&lt;/p&gt;
&lt;p&gt;Yep, you just stack up the bars, and make the bottom bit of the stack be transparent (note that in the first graph we have &lt;code&gt;chco=00000000,ff0000,0000ff&lt;/code&gt;, which specifies colours for each data series; the first item in there is &lt;code&gt;00000000&lt;/code&gt;, which is in format RRGGBBAA, meaning 0% red, 0% green, 0% blue, and 0% visible. Actually, it could have been &lt;code&gt;ffffff00&lt;/code&gt;, or &lt;code&gt;deaded00&lt;/code&gt;, or anything; all colours are the same when they have 0 opacity! The second graph is exactly the same, except now the transparent bars are shown in green so you can see how it&amp;#8217;s done.&lt;/p&gt;
&lt;p&gt;You&amp;#8217;ll note that all these graphs still have the &amp;#8220;real&amp;#8221; x axis (the line at the bottom of the graph) still visible. This is because there&amp;#8217;s no way to turn it off, which is unfortunate both for this fake-the-negatives approach and because you can&amp;#8217;t do decent sparkline graphs if you have to display the axes.&lt;/p&gt;
&lt;p&gt;Both of the tricks above are horrible fudges which only need to exist until the Google Chart people rediscover the minus sign on their keyboards, which I&amp;#8217;m sure is already in their bugtracker somewhere. If it&amp;#8217;s not, then here, Google, take some of these: - - - - - - - - - - - - - - - - - - - -. Hope you find them useful.&lt;/p&gt;		</content>
		<author>
			<name>Stuart Langridge</name>
			<uri>http://www.kryogenix.org/days</uri>
		</author>
		<source>
			<title type=""html"">as days pass by, by Stuart Langridge</title>
			<subtitle type=""html"">scratched tallies on the prison wall</subtitle>
			<link rel=""self"" href=""http://kryogenix.org/days/feed/""/>
			<id>http://kryogenix.org/days/feed/</id>
			<updated>2007-12-09T20:36:36+00:00</updated>
		</source>
	</entry>

	<entry xml:lang=""en-us"">
		<title type=""html"">And the Winner Is ...  Tellico</title>
		<link href=""http://blogs.sun.com/richb/entry/and_the_winner_is_tellico""/>
		<id>http://blogs.sun.com/richb/entry/and_the_winner_is_tellico</id>
		<updated>2007-12-07T23:33:51+00:00</updated>
		<content type=""html"">
&lt;img src=&quot;http://planet.gnome.org/heads/richb.png&quot; alt=&quot;&quot; align=&quot;right&quot; style=&quot;float: right;&quot;&gt;&lt;p&gt;
&lt;table&gt;
  &lt;tr&gt;
    &lt;td&gt;
    &lt;a href=&quot;http://periapsis.org/tellico/&quot;&gt;
    &lt;img src=&quot;http://periapsis.org/tellico/img/tellico-icon.png&quot; alt=&quot;&quot; /&gt;&lt;/a&gt;
    &lt;/td&gt;
    &lt;td&gt;
    &lt;p&gt;
    Continuing on from yesterday's book cataloging 
    &lt;a href=&quot;http://blogs.sun.com/richb/entry/experiences_with_alexandria_and_tellico&quot;&gt;fun&lt;/a&gt;, 
    I can tell you that I persevered and with great help from
    Robby Stephenson (thanks!), 
    I now have my book collection cataloged on my Ferrari using
    &lt;a href=&quot;http://periapsis.org/tellico/&quot;&gt;tellico&lt;/a&gt;.
    &lt;/p&gt;&lt;/td&gt;
  &lt;/tr&gt;
&lt;/table&gt;
&lt;p&gt;
So here's what I had to do, to get things to work.
Importing in RIS format: I was not conforming exactly to the RIS
specification. There needed to be exactly two spaces each side of the hyphen.
I adjusted my conversion script accordingly:
&lt;p&gt;
&lt;pre&gt;
import sys

def readBookList():
    return sys.stdin.readlines()

def writeRISEntries(lines):
    for line in lines:
        tokens = line.split('\t')
        tokensLen = len(tokens)
        sys.stdout.writelines('\n')
        sys.stdout.writelines('TY  -  BOOK\n')
        if tokensLen &gt; 37:
            sys.stdout.writelines('AU  -  ' + tokens[37] + '\n')
        if tokensLen &gt; 10:
            sys.stdout.writelines('TI  -  ' + tokens[10] + '\n')
        if tokensLen &gt; 8:
            sys.stdout.writelines('SN  -  ' + tokens[8] + '\n')
        sys.stdout.writelines('ER  -  \n')

if __name__ == &quot;__main__&quot;:
    lines = readBookList()
    writeRISEntries(lines)
&lt;/pre&gt;
&lt;p&gt;
and reconverted my &lt;em&gt;Delicious Library&lt;/em&gt; exported data and reimported it
into &lt;em&gt;tellico&lt;/em&gt;. This was very quick. At this point, &lt;em&gt;tellico&lt;/em&gt;
did nothing more than read and remember the data. There were
no pretty book cover images to go with the title, author and ISBN values.
&lt;p&gt;
To fix this, I needed to select all the books and run
&lt;code&gt;Collection-&gt;Update Entry-&gt;Amazon (US)&lt;/code&gt;.
This proceeded to check and update each book, with each update taking
about 1-2 seconds. It then tried to update the images of the books on
the shelf. As it progressed, this got slower and slower (taking upto 8
seconds per book). Apparently the graphics update is in the main thread
as the rest of the application refused to redraw when I put the window
to the back and then bought it forward again.
&lt;p&gt;
Anyhoo, I let it continue and several hours later, there were all my
book covers (or at least as many as Amazon US knows about). I quit the
application and saved the new book information. Images were saved in with
the rest of the data, and this resulted in a 10MB file. Still, it reloads
the book data quickly the next time the application is started. As there are
no books initially selected, then it doesn't have to display the book cover
image detail right away. Selecting all the books does take about twenty seconds,
but that's still faster than &lt;em&gt;Delicious Library&lt;/em&gt; on my old G4 Powerbook.
&lt;p&gt;
One of the nice features with &lt;em&gt;tellico&lt;/em&gt; is that I now know how
many books per author I have. My most popular author is Jack Vance with
52 followed by John Brunner (37), Terry Pratchett (34), Robert Silverberg (31),
Michael Moorcock (31) and Isaac Asimov (30). Do you see a pattern here.
My biggest non-SF/Fantasy authors are Martin Gardner (19) and Graham 
Greene (18).
&lt;p&gt;
I now need to spend a little time to fully read the documentation and find
out all the other features it has.
&lt;p&gt;
&lt;span&gt;
[&lt;a href=&quot;http://www.technorati.com/tag/Book+Cataloging&quot; rel=&quot;tag&quot;&gt;Technorati Tag: Book Cataloging&lt;/a&gt;]
&lt;/span&gt;
&lt;p&gt;
&lt;span&gt;
[&lt;a href=&quot;http://www.technorati.com/tag/tellico&quot; rel=&quot;tag&quot;&gt;Technorati Tag: tellico&lt;/a&gt;]
&lt;/span&gt;
&lt;p&gt;&lt;/p&gt;&lt;/p&gt;&lt;/p&gt;&lt;/p&gt;&lt;/p&gt;&lt;/p&gt;&lt;/p&gt;&lt;/p&gt;&lt;/p&gt;&lt;/p&gt;&lt;/p&gt;		</content>
		<author>
			<name>Rich Burridge</name>
			<uri>http://blogs.sun.com/richb/</uri>
		</author>
		<source>
			<title type=""html"">Rich Burridge's Blog</title>
			<subtitle type=""html"">Rich Burridge's Blog</subtitle>
			<link rel=""self"" href=""http://blogs.sun.com/richb/feed/entries/rss""/>
			<id>http://blogs.sun.com/richb/feed/entries/rss</id>
			<updated>2007-12-09T17:06:09+00:00</updated>
			<rights type=""html"">Copyright 2007</rights>
		</source>
	</entry>

	<entry xml:lang=""en"">
		<title type=""html"">Paint the Silence</title>
		<link href=""http://log.emmanuelebassi.net/archives/2007/12/paint-the-silence/""/>
		<id>http://log.emmanuelebassi.net/archives/2007/12/paint-the-silence/</id>
		<updated>2007-12-07T23:30:45+00:00</updated>
		<content type=""html"">
&lt;img src=&quot;http://planet.gnome.org/heads/ebassi.png&quot; alt=&quot;&quot; align=&quot;right&quot; style=&quot;float: right;&quot;&gt;&lt;p&gt;&lt;em&gt;Weee, long time, no blog.&lt;/em&gt;&lt;/p&gt;
&lt;p&gt;Dear Lazyweb,&lt;/p&gt;
&lt;p&gt;is it at all possible to coerce the devilspawn also known as libtool to actually be quiet when compiling and printing something like the kernel compilation outpout - that is, something like:&lt;/p&gt;
&lt;pre&gt;
  GEN autogenerated.c
  CC file1.c
  CC file2.c
  LINK output
  INSTALL output
&lt;/pre&gt;
&lt;p&gt;I know how to do that with a plain Makefile, and how to do it for autogenerated files like the enumeration types and the GLib marshallers, but I have no clue where to start to make libtool behave.&lt;/p&gt;
&lt;p&gt;Thanks,&lt;br /&gt;
 Emmanuele
&lt;/p&gt;		</content>
		<author>
			<name>Emmanuele Bassi</name>
			<uri>http://log.emmanuelebassi.net</uri>
		</author>
		<source>
			<title type=""html"">context switch</title>
			<subtitle type=""html"">Random babblings of a geek.</subtitle>
			<link rel=""self"" href=""http://log.emmanuelebassi.net/feed""/>
			<id>http://log.emmanuelebassi.net/feed</id>
			<updated>2007-12-07T23:35:46+00:00</updated>
		</source>
	</entry>

	<entry xml:lang=""en-GB"">
		<title type=""html"">2007-12-07: Friday</title>
		<link href=""http://www.gnome.org/~michael/activity.html#2007-12-07""/>
		<id>http://www.gnome.org/~michael/activity.html#2007-12-07</id>
		<updated>2007-12-07T22:27:19+00:00</updated>
		<content type=""html"">
&lt;img src=&quot;http://planet.gnome.org/heads/michael.png&quot; alt=&quot;&quot; align=&quot;right&quot; style=&quot;float: right;&quot;&gt;&lt;ul&gt;
	&lt;li&gt;Up rather early, off to the airport, checked in &amp;amp;
met JP, had breakfast together &amp;amp; saw some funky new Gnome
things: conduit, *Kit etc. Met Kent Ericson on the plane to Boston,
impressed with his example - flying coach.
	&lt;/li&gt;&lt;li&gt;Finally arrived at Boston, some nice bus driver gave me
a free lift to Terminal E, usual check-in/security mayhem tested
by JP's optimized security process with some success. Failed to find
UK taxi driver to wake up early &amp;amp; take me to P. Risborough.
On-line, trickled a little power into the battery, poked mail etc.
	&lt;/li&gt;&lt;li&gt;Ricardo it seems is burning through more yast2-gtk
pieces - which is neat, lots to look forward to in SLED11.
&lt;/li&gt;&lt;/ul&gt;		</content>
		<author>
			<name>Michael Meeks</name>
			<uri>http://www.gnome.org/~michael/</uri>
		</author>
		<source>
			<title type=""html"">Michael Meeks</title>
			<subtitle type=""html"">Stuff Michael Meeks is doing</subtitle>
			<link rel=""self"" href=""http://www.gnome.org/~michael/activity.xml""/>
			<id>http://www.gnome.org/~michael/activity.xml</id>
			<updated>2007-12-07T22:35:19+00:00</updated>
		</source>
	</entry>

	<entry>
		<title type=""html"">End of Term List</title>
		<link href=""http://fflewddur.livejournal.com/305949.html""/>
		<id>http://fflewddur.livejournal.com/305949.html</id>
		<updated>2007-12-07T21:35:06+00:00</updated>
		<content type=""html"">
&lt;img src=&quot;http://planet.gnome.org/heads/todd.png&quot; alt=&quot;&quot; align=&quot;right&quot; style=&quot;float: right;&quot;&gt;&lt;ul&gt;&lt;li&gt;Last final is over.  TA assignments are finished.  I'm on break!&lt;/li&gt;&lt;li&gt;Picked up an XO from one of the professors, I get to test my final project on the real thing!&lt;/li&gt;&lt;li&gt;Went for a short hike with my camera, now I have lots of photos to check out.&lt;/li&gt;&lt;li&gt;Dinner + movie tonight with the people from lab.&lt;/li&gt;&lt;li&gt;Show + cocktails Sunday = excuse to wear a suit!&lt;/li&gt;&lt;li&gt;Aquarium trip on Monday, yaay for more otters!&lt;/li&gt;&lt;/ul&gt;&lt;p&gt;And oh yeah, it's December and 50 degrees out.  It feels like spring break, not winter.  Splendid!&lt;/p&gt;		</content>
		<author>
			<name>Todd Kulesza</name>
			<uri>http://fflewddur.livejournal.com/</uri>
		</author>
		<source>
			<title type=""html"">/home/fflewddur</title>
			<subtitle type=""html"">/home/fflewddur - LiveJournal.com</subtitle>
			<link rel=""self"" href=""http://fflewddur.livejournal.com/data/rss""/>
			<id>http://fflewddur.livejournal.com/data/rss</id>
			<updated>2007-12-07T21:36:33+00:00</updated>
		</source>
	</entry>

	<entry xml:lang=""en"">
		<title type=""html"">Tomboy Hackfest Tonight at the Novell OSTC</title>
		<link href=""http://kubasik.net/blog/2007/12/07/tomboy-hackfest-tonight-at-the-novell-ostc/""/>
		<id>http://kubasik.net/blog/2007/12/07/tomboy-hackfest-tonight-at-the-novell-ostc/</id>
		<updated>2007-12-07T20:50:55+00:00</updated>
		<content type=""html"">
&lt;img src=&quot;http://planet.gnome.org/heads/kkubasik.png&quot; alt=&quot;&quot; align=&quot;right&quot; style=&quot;float: right;&quot;&gt;&lt;p&gt;Well be hacking it up tonight at &lt;a href=&quot;http://www.timeanddate.com/worldclock/fixedtime.html?month=12&amp;amp;day=7&amp;amp;year=2007&amp;amp;hour=18&amp;amp;min=0&amp;amp;sec=0&amp;amp;p1=220&quot;&gt;6:00PM MST&lt;/a&gt; at the Novell Open Source Technology Center. The rough TODO for the night seems to be Tags, Tasks and maybe even a backend to query Beagle. ;)  Anyways, if your in the greater Salt Lake City area, come on down! If your a little further away but want to join in anyways,  join in on #tomboy!&lt;/p&gt;
&lt;p&gt;See you tonight!&lt;/p&gt;		</content>
		<author>
			<name>Kevin Kubasik</name>
			<uri>http://kubasik.net/blog</uri>
		</author>
		<source>
			<title type=""html"">For Once I Oneder</title>
			<subtitle type=""html"">A Place For My Mind to Wander</subtitle>
			<link rel=""self"" href=""http://kubasik.net/blog/feed/""/>
			<id>http://kubasik.net/blog/feed/</id>
			<updated>2007-12-09T17:24:40+00:00</updated>
		</source>
	</entry>

	<entry xml:lang=""en"">
		<title type=""html"">Announcing D-Feet - a D-Bus Debugger</title>
		<link href=""http://www.j5live.com/?p=418""/>
		<id>http://www.j5live.com/?p=418</id>
		<updated>2007-12-07T20:20:34+00:00</updated>
		<content type=""html"">
&lt;img src=&quot;http://planet.gnome.org/heads/j5.png&quot; alt=&quot;&quot; align=&quot;right&quot; style=&quot;float: right;&quot;&gt;&lt;p&gt;&lt;img src=&quot;http://hosted.fedoraproject.org/projects/d-feet/attachment/wiki/WikiStart/d-feet.png?format=raw&quot; height=&quot;160&quot; width=&quot;480&quot; /&gt;&lt;/p&gt;
&lt;p&gt;&lt;img src=&quot;http://hosted.fedoraproject.org/projects/d-feet/attachment/wiki/WikiStart/D-Feet-screenshot-cropped.png?format=raw&quot; height=&quot;327&quot; width=&quot;510&quot; /&gt;&lt;/p&gt;
&lt;p&gt;I&amp;#8217;ve been playing around with a little tool for debugging applications on the bus.  It is far from done but I decided to release it now that it has become useful.  Right now you can see all names on the Session and System buses and get information on the objects and interfaces they export.&lt;/p&gt;
&lt;h3 id=&quot;CurrentFeatures&quot;&gt;Current Features&lt;a href=&quot;https://hosted.fedoraproject.org/projects/d-feet/#CurrentFeatures&quot; title=&quot;Link to this section&quot; class=&quot;anchor&quot;&gt; &lt;/a&gt;&lt;/h3&gt;
&lt;ul&gt;
&lt;li&gt;View names on the session and system bus&lt;/li&gt;
&lt;li&gt;View exported objects, interfaces, methods and signals&lt;/li&gt;
&lt;li&gt;View the full command line of services on the bus&lt;/li&gt;
&lt;li&gt;Execute methods with parameters on the bus and see their return values&lt;/li&gt;
&lt;/ul&gt;
&lt;h3 id=&quot;PlanedFeatures&quot;&gt;Planed Features&lt;a href=&quot;https://hosted.fedoraproject.org/projects/d-feet/#PlanedFeatures&quot; title=&quot;Link to this section&quot; class=&quot;anchor&quot;&gt; &lt;/a&gt;&lt;/h3&gt;
&lt;ul&gt;
&lt;li&gt;Attach to any address&lt;/li&gt;
&lt;li&gt;Watch, edit and play back method calls&lt;/li&gt;
&lt;li&gt;Watch signals&lt;/li&gt;
&lt;li&gt;Attach scripted actions to triggers&lt;/li&gt;
&lt;li&gt;Profiler for D-Bus calls&lt;/li&gt;
&lt;li&gt;Assistant for creating async call chains&lt;/li&gt;
&lt;/ul&gt;
&lt;h2&gt;Info&lt;/h2&gt;
&lt;p&gt;&lt;strong&gt;Homepage:&lt;/strong&gt; &lt;a href=&quot;http://hosted.fedoraproject.org/projects/d-feet/&quot;&gt;http://hosted.fedoraproject.org/projects/d-feet/&lt;/a&gt;&lt;br /&gt;
&lt;strong&gt;Tarball:&lt;/strong&gt; &lt;a href=&quot;http://johnp.fedorapeople.org/d-feet-0.1.4.tar.gz&quot;&gt;http://johnp.fedorapeople.org/d-feet-0.1.4.tar.gz&lt;/a&gt;&lt;br /&gt;
&lt;strong&gt;Fedora:&lt;/strong&gt; yum install d-feet&lt;br /&gt;
(Should be built for Rawhide, F-8 and F-7 though it may take a bit to be pushed as an update to F-8 and F-7)&lt;/p&gt;
[read this post in: &lt;a href=&quot;http://translate.google.com/translate?u=http://www.j5live.com/?p=418&amp;amp;langpair=en%7Car&quot;&gt;ar&lt;/a&gt; &lt;a href=&quot;http://translate.google.com/translate?u=http://www.j5live.com/?p=418&amp;amp;langpair=en%7Cde&quot;&gt;de&lt;/a&gt; &lt;a href=&quot;http://translate.google.com/translate?u=http://www.j5live.com/?p=418&amp;amp;langpair=en%7Ces&quot;&gt;es&lt;/a&gt; &lt;a href=&quot;http://translate.google.com/translate?u=http://www.j5live.com/?p=418&amp;amp;langpair=en%7Cfr&quot;&gt;fr&lt;/a&gt; &lt;a href=&quot;http://translate.google.com/translate?u=http://www.j5live.com/?p=418&amp;amp;langpair=en%7Cit&quot;&gt;it&lt;/a&gt; &lt;a href=&quot;http://translate.google.com/translate?u=http://www.j5live.com/?p=418&amp;amp;langpair=en%7Cja&quot;&gt;ja&lt;/a&gt; &lt;a href=&quot;http://translate.google.com/translate?u=http://www.j5live.com/?p=418&amp;amp;langpair=en%7Cko&quot;&gt;ko&lt;/a&gt; &lt;a href=&quot;http://translate.google.com/translate?u=http://www.j5live.com/?p=418&amp;amp;langpair=en%7Cpt&quot;&gt;pt&lt;/a&gt; &lt;a href=&quot;http://translate.google.com/translate?u=http://www.j5live.com/?p=418&amp;amp;langpair=en%7Cru&quot;&gt;ru&lt;/a&gt; &lt;a href=&quot;http://translate.google.com/translate?u=http://www.j5live.com/?p=418&amp;amp;langpair=en%7Czh-CN&quot;&gt;zh-CN&lt;/a&gt; ]		</content>
		<author>
			<name>John Palmieri</name>
			<uri>http://www.j5live.com</uri>
		</author>
		<source>
			<title type=""html"">J5's Blog</title>
			<subtitle type=""html"">Where the urethane hits the pavement</subtitle>
			<link rel=""self"" href=""http://www.j5live.com/wp-rss2.php""/>
			<id>http://www.j5live.com/wp-rss2.php</id>
			<updated>2007-12-08T00:23:56+00:00</updated>
		</source>
	</entry>

</feed>";
                
                Summa.Data.Parser.FeedParser parser = new Summa.Data.Parser.AtomParser("file:///home/eosten/Projects/atom.xml", xml);
                System.Console.WriteLine("Name: "+parser.Name);
                System.Console.WriteLine("URL: "+parser.Uri);
                foreach (Summa.Data.Parser.Item item in parser.Items) {
                    System.Console.WriteLine("ItemName: "+item.Title);
                }
                
                Gtk.Application.Run();
            }
        }
    }
}
