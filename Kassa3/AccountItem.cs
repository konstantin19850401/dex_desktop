using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kassa3
{
    class AccountItem
    {
        public string Text;
        public string CurrencyTitle;
        public string Shortcut;
        public long id;
        public long firmId;
        public long currId;
        public string bankTag;

        public AccountItem(long id, string Text, string Shortcut, long firmId, long currId, string CurrencyTitle, string bankTag)
        {
            this.Text = Text;
            this.Shortcut = Shortcut;
            this.id = id;
            this.firmId = firmId;
            this.currId = currId;
            this.CurrencyTitle = CurrencyTitle;
            this.bankTag = bankTag;
        }

        public override string ToString()
        {
            return "[" + Shortcut + "] " + Text + " - " + CurrencyTitle;
        }
    }
}
