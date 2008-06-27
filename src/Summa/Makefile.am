CSC = $(MCS)

FLAGS = -debug

ASSEMBLIES = \
	     -pkg:gconf-sharp-2.0 \
	     -pkg:gnome-sharp-2.0 \
	     -pkg:gtk-sharp-2.0 \
	     -pkg:ndesk-dbus-1.0 \
	     -pkg:webkit-sharp-1.0 \
	     -r:Mono.Data.SqliteClient \
	     -r:Mono.Posix \
	     -r:System.Core \
	     -r:System.Data

SUMMA_CSFILES = \
        Summa.Core/Application.cs \
        Summa.Core/Bookmarker.cs \
        Summa.Core/Config.cs \
        Summa.Core/Exceptions.cs \
        Summa.Core/MediaPlayer.cs \
        Summa.Core/Updater.cs \
        Summa.Core/Util.cs \
        Summa.Data/Core.cs \
        Summa.Data/Feed.cs \
        Summa.Data/Item.cs \
        Summa.Data/Summa.Data.Storage/Database.cs \
        Summa.Data/Summa.Data.Parser/AtomParser.cs \
        Summa.Data/Summa.Data.Parser/FeedParser.cs \
        Summa.Data/Summa.Data.Parser/Item.cs \
        Summa.Data/Summa.Data.Parser/MigoRfc822DateTime.cs \
        Summa.Data/Summa.Data.Parser/RssParser.cs \
        Summa.Net/Feed.cs \
        Summa.Net/Request.cs \
        Summa.Gui/AboutDialog.cs \
        Summa.Gui/Actions.cs \
        Summa.Gui/AddTagDialog.cs \
        Summa.Gui/AddWindow.cs \
        Summa.Gui/Browser.cs \
        Summa.Gui/ConfigDialog.cs \
        Summa.Gui/DeleteConfirmationDialog.cs \
        Summa.Gui/DieuBookmarker.cs \
        Summa.Gui/FeedPropertiesDialog.cs \
        Summa.Gui/FeedView.cs \
        Summa.Gui/Firstrun.cs \
        Summa.Gui/ItemView.cs \
        Summa.Gui/NativeBookmarker.cs \
        Summa.Gui/OpmlDialog.cs \
        Summa.Gui/StatusIcon.cs \
        Summa.Gui/TagView.cs \
        Summa.Gui/TagWindow.cs \
        Summa.Gui/TotemMediaPlayer.cs \
        Summa.Gui/WebKitView.cs

summalibdir = $(prefix)/lib/summa
all:
	$(CSC) $(FLAGS) $(ASSEMBLIES) $(SUMMA_CSFILES) -out:summa.exe 

install-data-local: summa.exe
	$(mkinstalldirs) $(DESTDIR)$(summalibdir)
	$(INSTALL_DATA) summa.exe $(DESTDIR)$(summalibdir)/summa.exe

CLEANFILES = summa.exe

