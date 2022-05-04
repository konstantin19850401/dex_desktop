using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using DEXExtendLib;

namespace DEXPlugin.Schedule.ExportMega
{

    public class MegaExport
    {
        static int DOCS_TO_EXPORT = 10;

        object toolbox;
        
        public MegaExport(object toolbox)
        {
            this.toolbox = toolbox;
        }

        public void Execute()
        {
            try
            {
                for (int rc = 0; rc < DOCS_TO_EXPORT; ++rc)
                {
                    IDEXData data = (IDEXData)toolbox;
                    DataTable t = data.getQuery("select * from `journal` where status=3 limit 0, 1");

                    if (t != null && t.Rows.Count > 0)
                    {
                        DataRow r = t.Rows[0];
                        StringList sl = new StringList();

                    }
                    else
                    {
                        break;
                    }
                }
            } 
            catch(Exception)
            {
            }
        }
    }
}
