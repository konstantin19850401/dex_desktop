using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Data;
//using MySql.Data.MySqlClient;

namespace DEXExtendLib
{
    public interface IDEXData
    {
        DataTable getQuery(string Query);
        DataTable getQuery(string Query, params object[] parameters);
        DataTable getTable(string TableName);

        int runQuery(string Query);
        int runQuery(string Query, params object[] parameters);

        long runQueryReturnLastInsertedId(string Query);
        long runQueryReturnLastInsertedId(string Query, params object[] parameters);

        void setDataReference(string TableName, string Key, bool DoIncrement);
        int getDataReference(string TableName, string Key);
        void clearDataReference(string TableName, string Key);

        void setDataHint(string hintType, string hintValue);
        string[] getDataHint(string hintType);

        ArrayList checkDocumentCriticals(ArrayList fields, IDEXDocumentData document);
        void setDocumentCriticals(ArrayList fields, IDEXDocumentData document, bool reset);

        string EscapeString(string src);

        bool AccessRemoteServer { get; }
        string PasspUserDb { get; }
        string PasspNameDb { get; }
        string PasspPassDb { get; }
        string PasspHostDb { get; }
    }
}
