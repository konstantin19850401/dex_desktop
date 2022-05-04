using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kassa3
{
    public class ClientItem
    {
        public string Text;
        public string Shortcut;
        public long id;
        public long cat_id;

        public ClientItem(long id, long cat_id, string Text, string Shortcut)
        {
            this.Text = Text;
            this.Shortcut = Shortcut;
            this.id = id;
            this.cat_id = cat_id;
        }

        public override string ToString()
        {
            return "[" + Shortcut + "] " + Text;
        }
    }
}
