// FeedPropertiesDialog.cs
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

using Summa.Core;
using Summa.Data;
using Summa.Gui;

namespace Summa.Gui {
    public class FeedPropertiesDialog : Window {
        public ISource feed;
        
        private VBox vbox;
        private Notebook notebook;
        private ButtonBox bbox;
        
        private VBox general_vbox;
        private Entry entry_name;
        private Entry entry_author;
        private Entry entry_subtitle;
        private Entry entry_image;
        private Entry entry_url;
        
        private TreeView tv_tags;
        public ListStore store_tags;
        private CellRendererToggle cr_toggle;
        
        public FeedPropertiesDialog(ISource f) : base(WindowType.Toplevel) {
            feed = f;
            
            Title = "\""+feed.Name+"\" Properties";
            Icon = feed.Favicon;
            BorderWidth = 5;
            DeleteEvent += OnClose;
            
            vbox = new VBox();
            vbox.Spacing = 6;
            Add(vbox);
            
            notebook = new Notebook();
            vbox.PackStart(notebook, false, false, 0);
            
            bbox = new HButtonBox();
            bbox.Layout = ButtonBoxStyle.End;
            vbox.PackStart(bbox, false, false, 0);
            
            AddGeneralTab();
            AddTagsTab();
            AddCloseButton();
        }
        
        private void AddGeneralTab() {
            general_vbox = new VBox();
            general_vbox.BorderWidth = 5;
            general_vbox.Spacing = 10;
            
            Frame properties_frame = new Frame();
            Label properties_label = new Label();
            properties_label.Markup = ("<b>Properties</b>");
            properties_label.UseUnderline = true;
            properties_frame.LabelWidget = properties_label;
            properties_frame.LabelXalign = 0.0f;
            properties_frame.LabelYalign = 0.5f;
            properties_frame.Shadow = ShadowType.None;
            general_vbox.PackStart(properties_frame, false, false, 0);
            
            Alignment properties_alignment = new Alignment(0.0f, 0.0f, 1.0f, 1.0f);
            properties_alignment.TopPadding = (uint)(properties_frame == null ? 0 : 5);
            properties_alignment.LeftPadding = 12;
            properties_frame.Add(properties_alignment);
            
            Table properties_table = new Table(2, 4, false);
            properties_table.BorderWidth = 5;
            properties_table.ColumnSpacing = 6;
            properties_table.RowSpacing = 6;
            properties_alignment.Add(properties_table);
            
            Label name_label = new Label("Name: ");
            name_label.SetAlignment(0.0F, 0.5F);
            properties_table.Attach(name_label, 0, 1, 0, 1);
            
            entry_name = new Entry(feed.Name);
            properties_table.Attach(entry_name, 1, 2, 0, 1);
            
            Label author_label = new Label("Author: ");
            author_label.SetAlignment(0.0F, 0.5F);
            properties_table.Attach(author_label, 0, 1, 1, 2);
            
            entry_author = new Entry(feed.Author);
            properties_table.Attach(entry_author, 1, 2, 1, 2);
            
            Label subtitle_label = new Label("Subtitle: ");
            subtitle_label.SetAlignment(0.0F, 0.5F);
            properties_table.Attach(subtitle_label, 0, 1, 2, 3);
            
            entry_subtitle = new Entry(feed.Subtitle);
            properties_table.Attach(entry_subtitle, 1, 2, 2, 3);
            
            Label image_label = new Label("Image: ");
            image_label.SetAlignment(0.0F, 0.5F);
            properties_table.Attach(image_label, 0, 1, 3, 4);
            
            entry_image = new Entry(feed.Image);
            properties_table.Attach(entry_image, 1, 2, 3, 4);
            
            Frame source_frame = new Frame();
            source_frame.Sensitive = false;
            Label source_label = new Label();
            source_label.Markup = ("<b>Source</b>");
            source_label.UseUnderline = true;
            source_frame.LabelWidget = source_label;
            source_frame.LabelXalign = 0.0f;
            source_frame.LabelYalign = 0.5f;
            source_frame.Shadow = ShadowType.None;
            general_vbox.PackStart(source_frame, false, false, 0);
            
            Alignment source_alignment = new Alignment(0.0f, 0.0f, 1.0f, 1.0f);
            source_alignment.TopPadding = (uint)(source_frame == null ? 0 : 5);
            source_alignment.LeftPadding = 12;
            source_frame.Add(source_alignment);
            
            Table source_table = new Table(2, 4, false);
            source_table.BorderWidth = 5;
            source_table.ColumnSpacing = 6;
            source_table.RowSpacing = 6;
            source_alignment.Add(source_table);
            
            Label url_label = new Label("URL: ");
            url_label.SetAlignment(0.0F, 0.5F);
            source_table.Attach(url_label, 0, 1, 0, 1);
            
            entry_url = new Entry(feed.Url);
            source_table.Attach(entry_url, 1, 2, 0, 1);
            
            notebook.AppendPage(general_vbox, new Label("General"));
        }
        
        private void AddTagsTab() {
            Table tags_table = new Table(1, 2, false);
            tags_table.BorderWidth = 5;
            tags_table.ColumnSpacing = 10;
            tags_table.RowSpacing = 10;
            
            ScrolledWindow tags_swin = new ScrolledWindow();
            tags_swin.ShadowType = ShadowType.In;
            tags_swin.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
            tags_table.Attach(tags_swin, 0, 1, 0, 1);
            
            cr_toggle = new CellRendererToggle();
            cr_toggle.Activatable = true;
            cr_toggle.Toggled += new ToggledHandler(OnCrToggleToggled);
            tv_tags = new TreeView();
            tags_swin.Add(tv_tags);
            
            store_tags = new ListStore(typeof(bool), typeof(string));
            tv_tags.Model = store_tags;
            
            // set up the columns for the view
            TreeViewColumn column_Read = new TreeViewColumn("Use", cr_toggle, "active", 0);
            tv_tags.AppendColumn(column_Read);
            
            TreeViewColumn column_Name = new TreeViewColumn("Title", new CellRendererText(), "text", 1);
            tv_tags.AppendColumn(column_Name);
            
            foreach ( string tag in Feeds.GetTags() ) {
                TreeIter iter = store_tags.Append();
                if ( feed.Tags.Contains(tag) ) {
                    store_tags.SetValue(iter, 0, true);
                } else {
                    store_tags.SetValue(iter, 0, false);
                }
                store_tags.SetValue(iter, 1, tag);
            }
            
            ButtonBox tags_bbox = new HButtonBox();
            tags_bbox.Layout = ButtonBoxStyle.End;
            tags_table.Attach(tags_bbox, 0, 1, 1, 2);
            
            Button add_button = new Button(Stock.Add);
            add_button.Clicked += new EventHandler(OnAddButtonClicked);
            tags_bbox.PackStart(add_button);
            
            notebook.AppendPage(tags_table, new Label("Tags"));
        }
        
        private void OnAddButtonClicked(object obj, EventArgs args) {
            Window win = new AddTagDialog(this);
            win.ShowAll();
        }
        
        private void OnCrToggleToggled(object obj, ToggledArgs args) {
            TreeIter iter;
            store_tags.GetIter(out iter, new TreePath(args.Path));
            
            if ( (bool)store_tags.GetValue(iter, 0) ) {
                store_tags.SetValue(iter, 0, false);
                feed.RemoveTag((string)store_tags.GetValue(iter, 1));
            } else {
                store_tags.SetValue(iter, 0, true);
                feed.AppendTag((string)store_tags.GetValue(iter, 1));
            }
        }
        
        private void AddCloseButton() {
            Button close_button = new Button(Stock.Close);
            close_button.Clicked  += new EventHandler(OnClose);
            bbox.PackStart(close_button);
        }
        
        private void OnClose(object obj, EventArgs args) {
            feed.Name = entry_name.Text;
            feed.Author = entry_author.Text;
            feed.Subtitle = entry_subtitle.Text;
            feed.Image = entry_image.Text;
            
            Destroy();
        }
    }
}
