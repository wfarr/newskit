CSC = $(MCS)

FLAGS = -debug

ASSEMBLIES = \
	     -pkg:gconf-sharp-2.0   \
	     -pkg:gnome-sharp-2.0   \
	     -pkg:gtk-sharp-2.0     \
	     -r:Mono.Posix    \
	     -r:/usr/lib/mono/ndesk-dbus-1.0/NDesk.DBus.dll   \
	     -r:System.Core   \
	     -r:/usr/lib/mono/webkit-sharp/webkit-sharp.dll

SUMMA_CSFILES = \
        Summa.Core/Application.cs \
        Summa.Core/Bookmarker.cs \
        Summa.Core/Config.cs \
        Summa.Core/Util.cs \
        Summa.Data/Core.cs \
        Summa.Data/Feed.cs \
        Summa.Data/Item.cs \
        Summa.Gui/AboutDialog.cs \
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
        Summa.Gui/OpmlDialog.cs \
        Summa.Gui/StatusIcon.cs \
        Summa.Gui/TagView.cs \
        Summa.Gui/WebKitView.cs

summalibdir = $(prefix)/lib/summa
all:
	$(CSC) $(FLAGS) $(ASSEMBLIES) $(SUMMA_CSFILES) -out:summa.exe 

install-data-local: summa.exe
	$(mkinstalldirs) $(DESTDIR)$(summalibdir)
	$(INSTALL_DATA) summa.exe $(DESTDIR)$(summalibdir)/summa.exe

CLEANFILES = summa.exe

