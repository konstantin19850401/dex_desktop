using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Printing;

namespace DEXExtendLib
{
    public interface IDEXPrinter
    {
        /// <summary>
        /// Загрузка установок принтера из конфига. В случае, если в конфиге нет установок, применяются текущие установки системы.
        /// </summary>
        /// <returns></returns>
        PrinterSettings LoadPrinterSettings();

        /// <summary>
        /// Сохранение установок принтера в конфиге.
        /// </summary>
        /// <param name="ps">Установки принтера для сохранения</param>
        void SavePrinterSettings(PrinterSettings ps);
    }
}
