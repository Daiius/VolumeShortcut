using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolumeShortcut
{
    public class KeyEventArgs : EventArgs
    {
        public int KeyCode { get; }

        public KeyEventArgs(int keyCode)
        {
            KeyCode = keyCode;
        }
    }
}
