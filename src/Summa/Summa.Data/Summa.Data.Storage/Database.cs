using System;
using System.Collections;
using System.Text;

using System.Data;
using Mono.Data.SqliteClient;

namespace Summa {
    namespace Data {
        namespace Storage {
            public class Database {
                private string uri = "/home/eosten/.config/newskit/database.db";
                private const string Uri = "URI=file:///home/eosten/.config/newskit/database.db";
                private IDbConnection db;
                
                public Database() {
                    bool exists = System.IO.File.Exists(uri);
                    
                    db = new SqliteConnection(Uri);
                    db.Open();
                    
                    if (!exists) {
                        Initialize();
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
                    string to_insert = @"''{0}''";
                    
                    string param = System.Security.SecurityElement.Escape(parameter);
                    
                    return String.Format(to_insert, param);
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
                    IDbCommand dbcmd = db.CreateCommand();
                    dbcmd.CommandText = "select generated_name from Feeds where uri='"+uri+"'";
                    IDataReader reader = dbcmd.ExecuteReader();
                    string name = null;
                    while(reader.Read()) {
                        name = reader.GetString(0);
                    }
                    reader.Close();
                    reader = null;
                    dbcmd.Dispose();
                    dbcmd = null;
                    return name;
                }
                
                public string CreateFeed(string uri, string name, string author, string subtitle, string image, string license, string etag, string hmodified, string status, string tags, string favicon) {
                    string generated_name = GenerateRandomName();
                    
                    NonQueryCommand(String.Format("insert into Feeds values (null, {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11})", EscapeParam(uri), EscapeParam(generated_name), EscapeParam(name), EscapeParam(author), EscapeParam(subtitle), EscapeParam(image), EscapeParam(license), EscapeParam(etag), EscapeParam(hmodified), EscapeParam(status), EscapeParam(tags), EscapeParam(favicon)));
                    
                    NonQueryCommand("create table "+generated_name+" (id INTEGER PRIMARY KEY, title VARCHAR(50), uri VARCHAR(50), date VARCHAR(50), last_updated VARCHAR(50), author VARCHAR(50), tags VARCHAR(50), content VARCHAR(50), encuri VARCHAR(50), read VARCHAR(50), flagged VARCHAR(50))");
                    
                    return generated_name;
                }
                
                public void DeleteFeed(string uri) {
                    NonQueryCommand("drop table "+GetGeneratedName(uri));
                    NonQueryCommand(@"delete from Feeds where uri="""+uri+@"""");
                }
                
                public string[] GetFeed(string uri) {
                    string[] feed = new string[13];
                    
                    IDbCommand dbcmd = db.CreateCommand();
                    dbcmd.CommandText = "select * from Feeds where uri='"+uri+"'";
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
                        list.Add(GetFeed(reader.GetString(0)));
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
                    ArrayList list = GetPosts(feeduri);
                    
                    foreach (string[] item in list) {
                        if ( item[0] == uri ) {
                            return item;
                        }
                    }
                    return null;
                }
                
                public void AddItem(string feeduri, string title, string uri, string date, string last_updated, string author, string tags, string content, string encuri, string read, string flagged) {
                    string generated_name = GetGeneratedName(feeduri);
                    
                    NonQueryCommand(String.Format("insert into "+generated_name+" values (null, {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9})", EscapeParam(title), EscapeParam(uri), EscapeParam(date), EscapeParam(last_updated), EscapeParam(author), EscapeParam(tags), EscapeParam(content), EscapeParam(encuri), EscapeParam(read), EscapeParam(flagged)));
                }
                
                public void ChangeFeedInfo(string feeduri, string property, string intended_value) {
                    NonQueryCommand("update Feeds set "+property+@"="""+intended_value+@""" where uri="""+feeduri+@"""");
                }
                
                public void ChangeItemInfo(string feeduri, string itemuri, string property, string intended_value) {
                    NonQueryCommand("update "+GetGeneratedName(feeduri)+" set "+property+@"="""+intended_value+@""" where uri="""+itemuri+@"""");
                }
            }
        }
    }
}
