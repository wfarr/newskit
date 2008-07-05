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
	     -r:System.Web

SUMMA_CSFILES = \
        Summa.Core/Application.cs \
        Summa.Core/Config.cs \
        Summa.Core/Database.cs \
        Summa.Core/DBus.cs \
        Summa.Core/Exceptions.cs \
        Summa.Core/Log.cs \
        Summa.Core/Updater.cs \
        Summa.Data/Core.cs \
        Summa.Data/Feed.cs \
        Summa.Data/Item.cs \
        Summa.Gui/AboutDialog.cs \
        Summa.Gui/Actions.cs \
        Summa.Gui/AddFeedDialog.cs \
        Summa.Gui/AddTagDialog.cs \
        Summa.Gui/Browser.cs \
        Summa.Gui/ConfigDialog.cs \
        Summa.Gui/DeleteConfirmationDialog.cs \
        Summa.Gui/DieuBookmarker.cs \
        Summa.Gui/FeedPropertiesDialog.cs \
        Summa.Gui/FeedView.cs \
        Summa.Gui/FirstRun.cs \
        Summa.Gui/ItemView.cs \
        Summa.Gui/NativeBookmarker.cs \
        Summa.Gui/OpmlDialog.cs \
        Summa.Gui/StatusIcon.cs \
        Summa.Gui/TagView.cs \
        Summa.Gui/TagWindow.cs \
        Summa.Gui/TotemMediaPlayer.cs \
        Summa.Gui/WebKitView.cs \
        Summa.Interfaces/IBookmarker.cs \
        Summa.Interfaces/IFeedParser.cs \
        Summa.Interfaces/IMediaPlayer.cs \
        Summa.Net/Request.cs \
        Summa.Net/Util.cs \
        Summa.Parser/AtomParser.cs \
        Summa.Parser/Item.cs \
        Summa.Parser/MigoRfc822DateTime.cs \
        Summa.Parser/OpmlParser.cs \
        Summa.Parser/RssParser.cs

summalibdir = $(prefix)/lib/summa
all:
	$(CSC) $(FLAGS) $(ASSEMBLIES) $(SUMMA_CSFILES) -out:summa.exe 

install-data-local: summa.exe
	$(mkinstalldirs) $(DESTDIR)$(summalibdir)
	$(INSTALL_DATA) summa.exe $(DESTDIR)$(summalibdir)/summa.exe

CLEANFILES = summa.exe

