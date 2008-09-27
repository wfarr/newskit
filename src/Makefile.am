CSC = $(MCS)

FLAGS = -debug

ASSEMBLIES = \
	     -pkg:gconf-sharp-2.0 \
	     -pkg:gnome-sharp-2.0 \
	     -pkg:gtk-sharp-2.0 \
	     -pkg:ndesk-dbus-1.0 \
	     -pkg:ndesk-dbus-glib-1.0 \
	     -pkg:webkit-sharp-1.0 \
	     -r:Mono.Data.SqliteClient \
	     -r:Mono.Posix \
	     -r:System.Core \
	     -r:System.Data \
	     -r:System.Web \
	     -r:System.Drawing

SUMMA_CSDISTFILES = \
    Summa.Data.ISource.cs \
    NewsKit.RssCommon.cs \
    Summa.Data.Item.cs \
    Summa.Data.Search.cs \
    NewsKit.AtomParser.cs \
    Summa.Gui.AboutDialog.cs \
    NewsKit.Core.cs \
    Summa.Gui.Actions.cs \
    NewsKit.Exceptions.cs \
    Summa.Gui.AddFeedDialog.cs \
    NewsKit.Globals.cs \
    Summa.Gui.AddTagDialog.cs \
    NewsKit.IFeedParser.cs \
    Summa.Gui.Browser.cs \
    NewsKit.Item.cs \
    Summa.Gui.ConfigDialog.cs \
    NewsKit.OpmlParser.cs \
    Summa.Gui.DeleteConfirmationDialog.cs \
    NewsKit.RdfParser.cs \
    Summa.Gui.DieuBookmarker.cs \
    NewsKit.Request.cs \
    Summa.Gui.FeedPropertiesDialog.cs \
    NewsKit.RssParser.cs \
    Summa.Gui.FeedView.cs \
    Summa.Gui.FirstRun.cs \
    Summa.Core.Application.cs \
    Summa.Gui.IDieu.cs \
    Summa.Core.Config.cs \
    Summa.Gui.ItemNotebook.cs \
    Summa.Core.Database.cs \
    Summa.Gui.ItemView.cs \
    Summa.Core.DBus.cs \
    Summa.Core.EventArgs.cs \
    Summa.Gui.MessageDialog.cs \
    Summa.Core.Exceptions.cs \
    Summa.Gui.NativeBookmarker.cs \
    Summa.Core.IBookmarker.cs \
    Summa.Gui.NativeTheme.cs \
    Summa.Core.IMediaPlayer.cs \
    Summa.Gui.NotificationBar.cs \
    Summa.Gui.OpmlDialog.cs \
    Summa.Core.ITheme.cs \
    Summa.Gui.StatusIcon.cs \
    Summa.Core.Log.cs \
    Summa.Gui.TagView.cs \
    Summa.Core.LogList.cs \
    Summa.Gui.TagWindow.cs \
    Summa.Core.Notifier.cs \
    Summa.Gui.TotemMediaPlayer.cs \
    Summa.Core.ThemeManager.cs \
    Summa.Gui.WebKitView.cs \
    Summa.Core.Updater.cs \
    Summa.Net.ConnectionState.cs \
    Summa.Data.Core.cs \
    Summa.Net.INetworkManager.cs \
    Summa.Data.Feed.cs \
    Summa.Net.NetworkManager.cs \
    Notifications.Global.cs \
    Notifications.Notification.cs

summalibdir = $(prefix)/lib/summa
all:
	$(CSC) $(FLAGS) $(ASSEMBLIES) $(SUMMA_CSDISTFILES) -out:summa.exe 

install-data-local: summa.exe
	$(mkinstalldirs) $(DESTDIR)$(summalibdir)
	$(INSTALL_DATA) summa.exe $(DESTDIR)$(summalibdir)/summa.exe

CLEANFILES = summa.exe

