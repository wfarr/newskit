// NativeTheme.cs
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
using System.Collections;

namespace Summa.Gui {
    public class NativeTheme : Summa.Core.ITheme {
        public string Name {
            get { return "Native"; }
        }
        public string Uri {
            get {
                return "";
            }
            set {}
        }
        
        private const string FeedTheme = @"<html>
  <head>
    <meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8""/>
    <title>${title}</title>
    <base href=""${url}"" />
  </head>
  <body>
    <table border=""1"" cellpadding=""3"" rules=""groups"" width=""100%"">
      <tr bgcolor=""#bbbbff""><th align=""left""><a href=""${url}""><font color=""#ffffff"" size=""+1"">${title}</font></a></th></tr>
      <tr bgcolor=""#eeeeee""><td align=""left"">${author}</td></tr>
    </table>

    <img src=""${image}"">
  </body>
</html>";
        
        private const string ItemTheme = @"<html>
  <head>
    <meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8""/>
    <title>${title}</title>
    <base href=""${url}"" />
  </head>
  <body>
    <table border=""1"" cellpadding=""3"" rules=""groups"" width=""100%"">
      <tr bgcolor=""#bbbbff""><th align=""left""><a href=""${url}""><font color=""#ffffff"" size=""+1"">${title}</font></a></th></tr>
      <tr bgcolor=""#eeeeee""><td align=""left"">${author}</td></tr>
    </table>

    ${text}
  </body>
</html>";
        
        public string MakeHtml(Summa.Data.ISource feed) {
            string themed_string = FeedTheme;
            
            themed_string = themed_string.Replace("${title}", feed.Name);
            themed_string = themed_string.Replace("${url}", feed.Url);
            themed_string = themed_string.Replace("${author}", feed.Author);
            themed_string = themed_string.Replace("${image}", feed.Image);
            
            return themed_string;
        }
        
        public string MakeHtml(Summa.Data.Item item) {
            string themed_string = ItemTheme;
            
            themed_string = themed_string.Replace("${title}", item.Title);
            themed_string = themed_string.Replace("${url}", item.Uri);
            themed_string = themed_string.Replace("${author}", item.Author);
            themed_string = themed_string.Replace("${text}", item.Contents);
            
            return themed_string;
        }
    }
}
