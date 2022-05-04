using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace DEXExtendLib
{
    class CstPrintPreviewDialog : PrintPreviewDialog
    {
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
                return true;
            }
            
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
