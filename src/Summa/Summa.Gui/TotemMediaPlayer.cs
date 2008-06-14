using System;

namespace Summa {
    namespace Gui {
        public class TotemMediaPlayer : Summa.Core.MediaPlayer {
            private System.Diagnostics.Process process;
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
                    System.Diagnostics.Process.Start("totem --play-pause");
                } else {
                    still_alive = true;
                    process = System.Diagnostics.Process.Start(String.Format("totem {0}", uri));
                    process.Exited += new EventHandler(OnExited);
                }
            }
            
            public bool Pause() {
                System.Diagnostics.Process.Start("totem --pause");
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
}
