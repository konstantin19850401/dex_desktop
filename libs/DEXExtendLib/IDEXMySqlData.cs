using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace DEXExtendLib
{
    public interface IDEXMySqlData
    {
        MySqlDataAdapter getDataAdapter(string query);
    }
}
