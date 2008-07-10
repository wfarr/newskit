// Database.cs
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
using System.Text;
using System.IO;
using System.Diagnostics;

using System.Data;
using Mono.Data.SqliteClient;

namespace Summa.Core {
    public class ChangedEventArgs : EventArgs {
        public string Uri;
        public string FeedUri;
        public string Value;
        public string ItemProperty;
        
        public ChangedEventArgs() {}
    }
    
    public class AddedEventArgs : EventArgs {
        public string Uri;
        public string FeedUri;
        
        public AddedEventArgs() {}
    }
    
    public delegate void FeedAddedHandler(object obj, AddedEventArgs e);
    public delegate void FeedDeletedHandler(object obj, AddedEventArgs e);
    public delegate void FeedChangedHandler(object obj, ChangedEventArgs e);
    
    public delegate void ItemAddedHandler(object obj, AddedEventArgs e);
    public delegate void ItemDeletedHandler(object obj, AddedEventArgs e);
    public delegate void ItemChangedHandler(object obj, ChangedEventArgs e);
    
    public class Database {
        private static string uri = Mono.Unix.Native.Stdlib.getenv("HOME")+"/.config/newskit/database.db";
        private string Uri = "URI=file://"+uri;
        private IDbConnection db;
        
        public event FeedAddedHandler FeedAdded;
        public event FeedDeletedHandler FeedDeleted;
        public event FeedChangedHandler FeedChanged;
        public event ItemAddedHandler ItemAdded;
        public event ItemDeletedHandler ItemDeleted;
        public event ItemChangedHandler ItemChanged;
        
        private Hashtable GeneratedNames;
        
        public Database() {
            Directory.CreateDirectory(Mono.Unix.Native.Stdlib.getenv("HOME")+"/.config/newskit/");
            
            bool exists = File.Exists(uri);
            
            db = new SqliteConnection("Version=3,"+Uri);
            db.Open();
            
            GeneratedNames = new Hashtable();
            
            if (!exists) {
                Initialize();
            }
            
            foreach ( string[] feed in GetFeeds() ) {
                GeneratedNames.Add(feed[1], feed[2]);
            }
        }
        
        private void NonQueryCommand(string commandtext) {
            IDbCommand dbcmd = db.CreateCommand();
            dbcmd.CommandText = commandtext;
            dbcmd.ExecuteNonQuery();
            dbcmd.Dispose();
            dbcmd = null;
        }
        
        private string EscapeParam(string parameter) {
            try {
                return parameter;
            } catch ( Exception e ) {
                Summa.Core.Log.LogException(e, "Null reference");
                return "";
            }
        }
        
        private void Initialize() {
            NonQueryCommand("create table Summa (id INTEGER PRIMARY KEY, version VARCHAR(50))");
            
            NonQueryCommand("create table Feeds (id INTEGER PRIMARY KEY, uri VARCHAR(50), generated_name VARCHAR(50), name VARCHAR(50), author VARCHAR(50), subtitle VARCHAR(50), image VARCHAR(50), license VARCHAR(50), etag VARCHAR(50), hmodified VARCHAR(50), status VARCHAR(50), tags VARCHAR(50), favicon VARCHAR(50))");
            
            NonQueryCommand(String.Format("insert into Summa values (null, {0})", @"""0""")); 
        }
        
        private string GenerateRandomName() {
            Random random = new Random();
            StringBuilder builder = new StringBuilder();
            
            for (int i=0; i < 8; i++) {
                builder.Append(Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65))));
            }
            return builder.ToString();
        }
        
        private string GetGeneratedName(string uri) {
            return (string)GeneratedNames[uri];
        }
        
        public string CreateFeed(string uri, string name, string author, string subtitle, string image, string license, string etag, string hmodified, string status, string tags, string favicon) {
            string generated_name = GenerateRandomName();
            
            IDbCommand dbcmd = db.CreateCommand();
            dbcmd.CommandText = @"insert into Feeds values (null, :uri, :genname, :name, :author, :subtitle, :image, :license, :etag, :hmodified, :status, :tags, :favicon)";
            
            SqliteParameter uri_parameter = new SqliteParameter();
            uri_parameter.Value = EscapeParam(uri);
            uri_parameter.ParameterName = @":uri";
            dbcmd.Parameters.Add(uri_parameter);
            
            SqliteParameter genname_parameter = new SqliteParameter();
            if ( generated_name != null ) {
                genname_parameter.Value = EscapeParam(generated_name);
            } else {
                genname_parameter.Value = "";
            }
            genname_parameter.ParameterName = @":genname";
            dbcmd.Parameters.Add(genname_parameter);
            
            SqliteParameter name_parameter = new SqliteParameter();
            if ( name != null ) {
                name_parameter.Value = EscapeParam(name);
            } else {
                name_parameter.Value = "";
            }
            name_parameter.ParameterName = @":name";
            dbcmd.Parameters.Add(name_parameter);
            
            SqliteParameter author_parameter = new SqliteParameter();
            if ( author != null ) {
                author_parameter.Value = EscapeParam(author);
            } else {
                author_parameter.Value = "";
            }
            author_parameter.ParameterName = @":author";
            dbcmd.Parameters.Add(author_parameter);
            
            SqliteParameter sub_parameter = new SqliteParameter();
            if ( subtitle != null ) {
                sub_parameter.Value = EscapeParam(subtitle);
            } else {
                sub_parameter.Value = "";
            }
            sub_parameter.ParameterName = @":subtitle";
            dbcmd.Parameters.Add(sub_parameter);
            
            SqliteParameter image_parameter = new SqliteParameter();
            if ( image != null ) {
                image_parameter.Value = EscapeParam(image);
            } else {
                image_parameter.Value = "";
            }
            image_parameter.ParameterName = @":image";
            dbcmd.Parameters.Add(image_parameter);
            
            SqliteParameter license_parameter = new SqliteParameter();
            if ( license!= null ) {
                license_parameter.Value = EscapeParam(license);
            } else {
                license_parameter.Value = "";
            }
            license_parameter.ParameterName = @":license";
            dbcmd.Parameters.Add(license_parameter);
            
            SqliteParameter etag_parameter = new SqliteParameter();
            if ( etag != null ) {
                etag_parameter.Value = EscapeParam(etag);
            } else {
                etag_parameter.Value = "";
            }
            etag_parameter.ParameterName = @":etag";
            dbcmd.Parameters.Add(etag_parameter);
            
            SqliteParameter hmodified_parameter = new SqliteParameter();
            if ( hmodified != null ) {
                hmodified_parameter.Value = EscapeParam(hmodified);
            } else {
                hmodified_parameter.Value = "";
            }
            hmodified_parameter.ParameterName = @":hmodified";
            dbcmd.Parameters.Add(hmodified_parameter);
            
            SqliteParameter status_parameter = new SqliteParameter();
            if ( status != null ) {
                status_parameter.Value = EscapeParam(status);
            } else {
                status_parameter.Value = "";
            }
            status_parameter.ParameterName = @":status";
            dbcmd.Parameters.Add(status_parameter);
            
            SqliteParameter tags_parameter = new SqliteParameter();
            if ( tags != null ) {
                tags_parameter.Value = EscapeParam(tags);
            } else {
                tags_parameter.Value = "";
            }
            tags_parameter.ParameterName = @":tags";
            dbcmd.Parameters.Add(tags_parameter);
            
            SqliteParameter fav_parameter = new SqliteParameter();
            if ( favicon != null ) {
                fav_parameter.Value = EscapeParam(favicon);
            } else {
                fav_parameter.Value = "";
            }
            fav_parameter.ParameterName = @":favicon";
            dbcmd.Parameters.Add(fav_parameter);
            
            dbcmd.ExecuteNonQuery();
            dbcmd.Dispose();
            dbcmd = null;
            
            NonQueryCommand("create table "+generated_name+" (id INTEGER PRIMARY KEY, title VARCHAR(50), uri VARCHAR(50), date VARCHAR(50), last_updated VARCHAR(50), author VARCHAR(50), tags VARCHAR(50), content VARCHAR(50), encuri VARCHAR(50), read VARCHAR(50), flagged VARCHAR(50))");
            
            GeneratedNames.Add(uri, generated_name);
            
            AddedEventArgs args = new AddedEventArgs();
            args.Uri = uri;
            FeedAdded(this, args);
            
            return generated_name;
        }
        
        public void DeleteFeed(string uri) {
            NonQueryCommand("drop table "+GetGeneratedName(uri));
            NonQueryCommand(@"delete from Feeds where uri="""+uri+@"""");
            
            GeneratedNames.Remove(uri);
            
            AddedEventArgs args = new AddedEventArgs();
            args.Uri = uri;
            FeedDeleted(this, args);
        }
        
        public string[] GetFeed(string uri) {
            string[] feed = new string[13];
            
            IDbCommand dbcmd = db.CreateCommand();
            dbcmd.CommandText = "select * from Feeds where uri=:uri";
            SqliteParameter param = new SqliteParameter();
            param.Value = uri;
            param.ParameterName = @":uri";
            dbcmd.Parameters.Add(param);
            IDataReader reader = dbcmd.ExecuteReader();
            
            while(reader.Read()) {
                feed[0] = reader.GetString(0).ToString(); // integer primary key
                feed[1] = reader.GetString(1); // uri
                feed[2] = reader.GetString(2); // generated_name
                feed[3] = reader.GetString(3); // name
                feed[4] = reader.GetString(4); // author
                feed[5] = reader.GetString(5); // subtitle
                feed[6] = reader.GetString(6); // image
                feed[7] = reader.GetString(7); // license
                feed[8] = reader.GetString(8); // etag
                feed[9] = reader.GetString(9); // hmodified
                feed[10] = reader.GetString(10); // status
                feed[11] = reader.GetString(11); // tags
                feed[12] = reader.GetString(12); //favicon
            }
            reader.Close();
            reader = null;
            dbcmd.Dispose();
            dbcmd = null;
            return feed;
        }
        
        public ArrayList GetFeeds() {
            ArrayList list = new ArrayList();
            
            IDbCommand dbcmd = db.CreateCommand();
            dbcmd.CommandText = "select * from Feeds";
            IDataReader reader = dbcmd.ExecuteReader();
            while(reader.Read()) {
                string[] feed = new string[13];
                
                feed[0] = reader.GetString(0).ToString(); // integer primary key
                feed[1] = reader.GetString(1); // uri
                feed[2] = reader.GetString(2); // generated_name
                feed[3] = reader.GetString(3); // name
                feed[4] = reader.GetString(4); // author
                feed[5] = reader.GetString(5); // subtitle
                feed[6] = reader.GetString(6); // image
                feed[7] = reader.GetString(7); // license
                feed[8] = reader.GetString(8); // etag
                feed[9] = reader.GetString(9); // hmodified
                feed[10] = reader.GetString(10); // status
                feed[11] = reader.GetString(11); // tags
                feed[12] = reader.GetString(12); //favicon
                
                list.Add(feed);
            }
            reader.Close();
            reader = null;
            dbcmd.Dispose();
            dbcmd = null;
            return list;
        }
        
        public bool FeedExists(string url) {
            bool exists = false;
            foreach (string[] feed in GetFeeds()) {
                if ( feed[1] == url ) {
                    exists = true;
                }
            }
            return exists;
        }
        
        public ArrayList GetPosts(string feeduri) {
            ArrayList list = new ArrayList();
            
            IDbCommand dbcmd = db.CreateCommand();
            dbcmd.CommandText = "select * from "+GetGeneratedName(feeduri);
            IDataReader reader = dbcmd.ExecuteReader();
            while(reader.Read()) {
                string[] item = new string[10];
                item[0] = reader.GetString(1); //title
                item[1] = reader.GetString(2); //uri
                item[2] = reader.GetString(3); //date
                item[3] = reader.GetString(4); //last_updated
                item[4] = reader.GetString(5); //author
                item[5] = reader.GetString(6); //tags
                item[6] = reader.GetString(7); //content
                item[7] = reader.GetString(8); //encuri
                item[8] = reader.GetString(9); //read
                item[9] = reader.GetString(10); //flagged
                
                list.Add(item);
            }
            reader.Close();
            reader = null;
            dbcmd.Dispose();
            dbcmd = null;
            return list;
        }
        
        public string[] GetItem(string feeduri, string uri) {
            string[] item = null;
            
            IDbCommand dbcmd = db.CreateCommand();
            dbcmd.CommandText = "select * from "+GetGeneratedName(feeduri)+@" where uri="""+uri+@"""";
            IDataReader reader = dbcmd.ExecuteReader();
            while(reader.Read()) {
                item = new string[10];
                item[0] = reader.GetString(1); //title
                item[1] = reader.GetString(2); //uri
                item[2] = reader.GetString(3); //date
                item[3] = reader.GetString(4); //last_updated
                item[4] = reader.GetString(5); //author
                item[5] = reader.GetString(6); //tags
                item[6] = reader.GetString(7); //content
                item[7] = reader.GetString(8); //encuri
                item[8] = reader.GetString(9); //read
                item[9] = reader.GetString(10); //flagged
            }
            reader.Close();
            reader = null;
            dbcmd.Dispose();
            dbcmd = null;
            return item;
        }
        
        public void DeleteItem(string feeduri, string uri) {
            string generated_name = GetGeneratedName(feeduri);
            string command = "delete from "+generated_name+@" where uri="""+uri+@"""";
            
            NonQueryCommand(command);
            
            AddedEventArgs args = new AddedEventArgs();
            args.Uri = uri;
            args.FeedUri = feeduri;
            ItemAdded(this, args);
        }
        
        public void AddItem(string feeduri, string title, string uri, string date, string last_updated, string author, string tags, string content, string encuri, string read, string flagged) {
            string generated_name = GetGeneratedName(feeduri);
            
            IDbCommand dbcmd = db.CreateCommand();
            dbcmd.CommandText = @"insert into "+generated_name+" values (null, :title, :uri, :date, :lastup, :author, :tags, :content, :encuri, :read, :flagged)";
            
            SqliteParameter title_parameter = new SqliteParameter();
            if ( title != null ) {
                title_parameter.Value = EscapeParam(title);
            } else {
                title_parameter.Value = "";
            }
            title_parameter.ParameterName = @":title";
            dbcmd.Parameters.Add(title_parameter);
            
            SqliteParameter uri_parameter = new SqliteParameter();
            if ( uri != null ) {
                uri_parameter.Value = EscapeParam(uri);
            } else {
                uri_parameter.Value = "";
            }
            uri_parameter.ParameterName = @":uri";
            dbcmd.Parameters.Add(uri_parameter);
            
            SqliteParameter date_parameter = new SqliteParameter();
            if ( date != null ) {
                date_parameter.Value = EscapeParam(date);
            } else {
                date_parameter.Value = "";
            }
            date_parameter.ParameterName = @":date";
            dbcmd.Parameters.Add(date_parameter);
            
            SqliteParameter lu_parameter = new SqliteParameter();
            if ( last_updated != null ) {
                lu_parameter.Value = EscapeParam(last_updated);
            } else {
                lu_parameter.Value = "";
            }
            lu_parameter.ParameterName = @":lastup";
            dbcmd.Parameters.Add(lu_parameter);
            
            SqliteParameter author_parameter = new SqliteParameter();
            if ( author != null ) {
                author_parameter.Value = EscapeParam(author);
            } else {
                author_parameter.Value = "";
            }
            author_parameter.ParameterName = @":author";
            dbcmd.Parameters.Add(author_parameter);
            
            SqliteParameter tags_parameter = new SqliteParameter();
            if ( tags != null ) {
                tags_parameter.Value = EscapeParam(tags);
            } else {
                tags_parameter.Value = "";
            }
            tags_parameter.ParameterName = @":tags";
            dbcmd.Parameters.Add(tags_parameter);
            
            SqliteParameter content_parameter = new SqliteParameter();
            if ( content!= null ) {
                content_parameter.Value = EscapeParam(content);
            } else {
                content_parameter.Value = "";
            }
            content_parameter.ParameterName = @":content";
            dbcmd.Parameters.Add(content_parameter);
            
            SqliteParameter encuri_parameter = new SqliteParameter();
            if ( encuri != null ) {
                encuri_parameter.Value = EscapeParam(encuri);
            } else {
                encuri_parameter.Value = "";
            }
            encuri_parameter.ParameterName = @":encuri";
            dbcmd.Parameters.Add(encuri_parameter);
            
            SqliteParameter read_parameter = new SqliteParameter();
            if ( read != null ) {
                read_parameter.Value = EscapeParam("False");
            } else {
                read_parameter.Value = "";
            }
            read_parameter.ParameterName = @":read";
            dbcmd.Parameters.Add(read_parameter);
            
            SqliteParameter flagged_parameter = new SqliteParameter();
            if ( flagged != null ) {
                flagged_parameter.Value = EscapeParam("False");
            } else {
                flagged_parameter.Value = "";
            }
            flagged_parameter.ParameterName = @":flagged";
            dbcmd.Parameters.Add(flagged_parameter);
            
            dbcmd.ExecuteNonQuery();
            dbcmd.Dispose();
            dbcmd = null;
            
            AddedEventArgs args = new AddedEventArgs();
            args.Uri = uri;
            args.FeedUri = feeduri;
            ItemDeleted(this, args);
        }
        
        public void ChangeFeedInfo(string feeduri, string property, string intended_value) {
            NonQueryCommand("update Feeds set "+property+@"="""+intended_value+@""" where uri="""+feeduri+@"""");
            
            ChangedEventArgs args = new ChangedEventArgs();
            args.Uri = feeduri;
            args.Value = intended_value;
            args.ItemProperty = property;
            FeedChanged(this, args);
        }
        
        public void ChangeItemInfo(string feeduri, string itemuri, string property, string intended_value) { //optimize
            NonQueryCommand("update "+GetGeneratedName(feeduri)+" set "+property+@"="""+intended_value+@""" where uri="""+itemuri+@"""");
            
            ChangedEventArgs args = new ChangedEventArgs();
            args.Uri = itemuri;
            args.FeedUri = feeduri;
            args.Value = intended_value;
            args.ItemProperty = property;
            ItemChanged(this, args);
        }
        
        public ArrayList GetTags() {
            ArrayList list = new ArrayList();
            
            foreach ( string feeduri in GetFeeds() ) {
                IDbCommand dbcmd = db.CreateCommand();
                dbcmd.CommandText = "select tags from "+GetGeneratedName(feeduri);
                IDataReader reader = dbcmd.ExecuteReader();
                while(reader.Read()) {
                    list.Add(reader.GetString(1));
                }
                reader.Close();
                reader = null;
                dbcmd.Dispose();
                dbcmd = null;
            }
            return list;
        }
    }
}
