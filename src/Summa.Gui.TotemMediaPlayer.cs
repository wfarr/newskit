// TotemMediaPlayer.cs
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
using System.Diagnostics;

using Summa.Core;
using Summa.Gui;

namespace Summa.Gui {
    public class TotemMediaPlayer : IMediaPlayer {
        private Process process;
        private bool still_alive;
        private bool playing;
        
        public bool GetPlaying() {
            if ( still_alive && !playing ) {
                return playing;
            } else {
                return still_alive;
            }
        }
        
        public TotemMediaPlayer() {
            still_alive = false;
        }
        
        public void Play(string uri) {
            if ( !playing && still_alive ) {
                Process.Start("totem --play-pause");
            } else {
                still_alive = true;
                process = Process.Start(String.Format("totem {0}", uri));
                process.Exited += new EventHandler(OnExited);
            }
        }
        
        public bool Pause() {
            Process.Start("totem --pause");
            playing = false;
            return true;
        }
        
        public void Clear() {
            if ( still_alive ) {
                process.Close();
            }
            playing = false;
            still_alive = false;
        }
        
        private void OnExited(object obj, EventArgs args) {
            process.Close();
            still_alive = false;
            playing = false;
        }
    }
}
