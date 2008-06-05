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

SUMMA_CSFILES = AboutDialog.cs \
		AddWindow.cs \
		Application.cs \
		Bookmarker.cs \
		Browser.cs \
		DieuBookmarker.cs \
		Feed.cs \
		FeedView.cs \
		Firstrun.cs \
		Item.cs \
		ItemView.cs \
		NewsKit.cs \
		OpmlDialog.cs \
		StatusIcon.cs \
		TagView.cs \
		Util.cs \
		WebKitView.cs

summalibdir = $(prefix)/lib/summa
all:
	$(CSC) $(FLAGS) $(ASSEMBLIES) $(SUMMA_CSFILES) -out:summa.exe 

install-data-local: summa.exe
	$(mkinstalldirs) $(DESTDIR)$(summalibdir)
	$(INSTALL_DATA) summa.exe $(DESTDIR)$(summalibdir)/summa.exe

CLEANFILES = summa.exe

