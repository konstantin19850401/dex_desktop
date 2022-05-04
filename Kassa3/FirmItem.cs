using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kassa3
{
    class FirmItem
    {
        public string Text;
        public string Shortcut;
        public long id;

        public FirmItem(long id, string Text, string Shortcut)
        {
            this.Text = Text;
            this.Shortcut = Shortcut;
            this.id = id;
        }

        public override string ToString()
        {
            return "[" + Shortcut + "] " + Text;
        }
    }
}
