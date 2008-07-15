// Util.cs
//
// Copyright (c) 2008 Ethan Osten
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Xml;

namespace Summa.Net {
    public static class Util {
        public static Summa.Interfaces.IFeedParser Sniff(Summa.Net.Request request) {
            Summa.Interfaces.IFeedParser parser = null;
            
            try {
                parser = new Summa.Parser.RssParser(request.Uri, request.Xml);
            } catch ( Exception e ) {
                Summa.Core.Log.Exception(e);
            }
            
            if ( parser != null ) {
                if ( parser.Name == null ) {
                    try {
                        parser = new Summa.Parser.AtomParser(request.Uri, request.Xml);
                    } catch ( Exception e ) {
                        Summa.Core.Log.Exception(e);
                    }
                }
            } else {
                try {
                    parser = new Summa.Parser.AtomParser(request.Uri, request.Xml);
                } catch ( Exception e ) {
                    Summa.Core.Log.Exception(e);
                }
            }
            
            if ( parser != null ) {
                if ( parser.Name == null ) {
                    return null;
                } else {
                    return parser;
                }
            } else {
                return null;
            }
        }
    }
}
