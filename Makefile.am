SUBDIRS = src \
    data

nkdocdir = ${prefix}/doc/Summa
nkdoc_DATA = \
	README\
	COPYING\
	AUTHORS\
	ChangeLog\
	INSTALL\
	NEWS\
	TODO

EXTRA_DIST = $(nkdoc_DATA)

summa: summa.in config.log
	sed -e "s|\@prefix\@|$(prefix)|" < summa.in > summa

bin_SCRIPTS = summa
bindir=$(prefix)/bin

dist-hook:
	for specfile in *.spec; do \
		if test -f $$specfile; then \
			cp -p $$specfile $(distdir); \
		fi \
	done

run:
	mono --debug src/Summa/summa.exe

install-data-local:
		if test -z "$(DESTDIR)" ; then \
				for p in $(gconf_DATA) ; do \
						GCONF_CONFIG_SOURCE=$(GCONF_SCHEMA_CONFIG_SOURCE) $(GCONFTOOL) --makefile-install-rule $(top_builddir)/data/$$p ; \
				done \
		fi
		gtk-update-icon-cache $(datadir)/icons/hicolor
