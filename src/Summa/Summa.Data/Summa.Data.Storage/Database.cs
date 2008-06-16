using System;
using System.Collections;
using System.Text;

using System.Data;
using Mono.Data.SqliteClient;

namespace Summa {
    namespace Data {
        namespace Storage {
            public class Database {
                private string uri = "file:///home/eosten/.config/newskit/database.db";
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
                    
                    NonQueryCommand(String.Format("insert into Feeds values (null, {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12})", EscapeParam(uri), EscapeParam(generated_name), EscapeParam(name), EscapeParam(author), EscapeParam(subtitle), EscapeParam(image), EscapeParam(license), EscapeParam(etag), EscapeParam(hmodified), EscapeParam(status), EscapeParam(tags), EscapeParam(favicon)));
                    
                    NonQueryCommand("create table "+generated_name+" (id INTEGER PRIMARY KEY, title VARCHAR(50), uri VARCHAR(50), date VARCHAR(50), last_updated VARCHAR(50), author VARCHAR(50), tags VARCHAR(50), content VARCHAR(50), encuri VARCHAR(50), read VARCHAR(50), flagged VARCHAR(50))");
                    
                    return generated_name;
                }
                
                public void DeleteFeed(string uri) {
                    NonQueryCommand("drop table "+GetGeneratedName(uri));
                    NonQueryCommand(@"delete from Feeds where uri="""+uri+@"""");
                }
                
                public string[] GetFeed(string uri) {
                    string[] feed = new string[12];
                    
                    IDbCommand dbcmd = db.CreateCommand();
                    dbcmd.CommandText = "select * from Feeds where uri='"+uri+"'";
                    IDataReader reader = dbcmd.ExecuteReader();
                    while(reader.Read()) {
                        feed[0] = reader.GetString(0).ToString();
                        feed[1] = reader.GetString(1);
                        feed[2] = reader.GetString(2);
                        feed[3] = reader.GetString(3);
                        feed[4] = reader.GetString(4);
                        feed[5] = reader.GetString(5);
                        feed[6] = reader.GetString(6);
                        feed[7] = reader.GetString(7);
                        feed[8] = reader.GetString(8);
                        feed[9] = reader.GetString(9);
                        feed[10] = reader.GetString(10);
                        feed[11] = reader.GetString(11);
                        feed[12] = reader.GetString(12);
                        feed[13] = reader.GetString(13);
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
                
                public ArrayList GetPosts(string feeduri) {
                    ArrayList list = new ArrayList();
                    
                    IDbCommand dbcmd = db.CreateCommand();
                    dbcmd.CommandText = "select * from "+GetGeneratedName(feeduri);
                    IDataReader reader = dbcmd.ExecuteReader();
                    while(reader.Read()) {
                        string[] item = new string[11];
                        item[0] = reader.GetString(0);
                        item[1] = reader.GetString(1);
                        item[2] = reader.GetString(2);
                        item[3] = reader.GetString(3);
                        item[4] = reader.GetString(4);
                        item[5] = reader.GetString(5);
                        item[6] = reader.GetString(6);
                        item[7] = reader.GetString(7);
                        item[8] = reader.GetString(8);
                        item[9] = reader.GetString(9);
                        item[10] = reader.GetString(10);
                        
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
