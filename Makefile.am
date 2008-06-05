SUBDIRS = src 

nkdocdir = ${prefix}/doc/NewsKit
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

