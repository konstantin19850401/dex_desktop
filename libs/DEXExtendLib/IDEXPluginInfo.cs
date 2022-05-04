using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DEXExtendLib
{
    public interface IDEXPluginInfo
    {
        string ID { get; }
        string Title { get; }
        string[] Path { get; }
        int Version { get; }
        string Description { get; }
        Bitmap getBitmap();
        void setJournalMode(DEXJournalType journalType);
    }
}
