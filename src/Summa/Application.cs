/* Application.cs
 *
 * Copyright (C) 2008  Ethan Osten
 *
 * This library is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation, either version 2.1 of the License, or
 * (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with this library.  If not, see <http://www.gnu.org/licenses/>.
 *
 * Author:
 *     Ethan Osten <senoki@gmail.com>
 */

using System;
using NewsKit;
using Gtk;

namespace Summa {
    public class MainClass {
        public MainClass() {
        }
        
        public static void Main() {
            Application.Init();
            
            Summa.Browser browse = new Summa.Browser();
            Summa.StatusIcon si = new Summa.StatusIcon(browse);
            
            browse.ShowAll();
            
            Application.Run();
        }
    }
}
