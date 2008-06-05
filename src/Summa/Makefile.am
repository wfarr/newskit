CSC = $(MCS)

FLAGS = /debug

ASSEMBLIES = \
	     -r:gconf-sharp   \
	     -r:gdk-sharp     \
	     -r:gnome-sharp   \
	     -r:gtk-sharp     \
	     -r:Mono.Posix    \
	     -r:NDesk.DBus    \
	     -r:pango-sharp   \
	     -r:System.Core   \
	     -r:webkit-sharp

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
all: $(SUMMA_CSFILES) $(CSC) $(FLAGS) -lib:$(top_srcdir)/lib $(RESOURCES) $(SUMMA_LIBS) $(ASSEMBLIES) $(SUMMA_CSFILES) -out:summa.exe

install-data-local: summa.exe
	$(mkinstalldirs) $(DESTDIR)$(summalibdir)
	$(INSTALL_DATA) summa.exe $(DESTDIR)$(summalibdir)/summa.exe

CLEANFILES = summa.exe

