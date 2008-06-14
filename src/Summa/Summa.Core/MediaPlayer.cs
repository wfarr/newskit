using System;

namespace Summa {
    namespace Core {
        public interface MediaPlayer {
            bool GetPlaying();
            
            void Play(string uri);
            bool Pause();
            void Clear();
        }
    }
}
