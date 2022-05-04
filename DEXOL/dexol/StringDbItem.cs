using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dexol
{
    class StringDbItem
    {
        public string title, dbname, unit_uid, unit_foreign_id, unit_title, unit_documentstate, unit_data;
        public string[] doctypes;
        
        //"uid", "foreign_id", "title", "documentstate", "data"

        public StringDbItem(string title, string dbname, string[] doctypes, 
            string unit_uid, string unit_foreign_id, string unit_title, 
            string unit_documentstate, string unit_data)
        {
            this.title = title;
            this.dbname = dbname;
            this.doctypes = doctypes;
            this.unit_uid = unit_uid;
            this.unit_foreign_id = unit_foreign_id;
            this.unit_title = unit_title;
            this.unit_documentstate = unit_documentstate;
            this.unit_data = unit_data;
        }

        public override string ToString()
        {
            return title;
        }
    }
}
