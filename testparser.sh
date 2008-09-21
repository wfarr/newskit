#!/bin/bash

gmcs -r:Mono.Posix -r:System.Drawing -pkg:ndesk-dbus-1.0 -pkg:ndesk-dbus-glib-1.0 src/Summa.Net.ConnectionState.cs src/Summa.Net.INetworkManager.cs src/Summa.Net.NetworkManager.cs src/MigoRfc822DateTime.cs src/NewsKit.AtomParser.cs src/NewsKit.Core.cs src/NewsKit.Exceptions.cs src/NewsKit.Globals.cs src/NewsKit.IFeedParser.cs src/NewsKit.Item.cs src/NewsKit.OpmlParser.cs src/NewsKit.RdfParser.cs src/NewsKit.Request.cs src/NewsKit.RssParser.cs src/NewsKit.TestBed.cs -out:src/newskit.exe
mono src/newskit.exe
