using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Globalization;

namespace Kassa3
{
    public class CurrencyDescriptor
    {
        public int id;
        public double value;

        public CurrencyDescriptor(int id, double value)
        {
            this.id = id;
            this.value = value;
        }
    }

    public class KassaRecord
    {
        public int id;
        public int op_id;
        public DateTime r_date;
        public double r_sum;
        public string r_prim;
        public int srctype, src_acc_id = -1, src_client_id = -1;
        public double src_curr_value = -1;
        public int dsttype, dst_acc_id = -1, dst_client_id = -1;
        public double dst_curr_value = -1;
        public int user_cr = -1, user_ch = -1;
        public int unreadId = -1;
        public bool deleted = false;

        public static KassaRecord fromDataRow(DataRow row)
        {
            KassaRecord kr = new KassaRecord();
            kr.id = Convert.ToInt32(row["id"]);
            kr.op_id = Convert.ToInt32(row["op_id"]);
            try
            {
                kr.r_date = DateTime.ParseExact(row["r_date"].ToString(), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None);
            }
            catch (Exception) { }

            kr.r_sum = Convert.ToDouble(row["r_sum"]);
            try
            {
                kr.r_prim = row["r_prim"].ToString();
            }
            catch (Exception) { }


            kr.srctype = Convert.ToInt32(row["srctype"]);
            if (!row.IsNull("src_acc_id")) kr.src_acc_id = Convert.ToInt32(row["src_acc_id"]);
            if (!row.IsNull("src_client_id")) kr.src_client_id = Convert.ToInt32(row["src_client_id"]);
            if (!row.IsNull("src_curr_value")) kr.src_curr_value = Convert.ToDouble(row["src_curr_value"]);

            kr.dsttype = Convert.ToInt32(row["dsttype"]);
            if (!row.IsNull("dst_acc_id")) kr.dst_acc_id = Convert.ToInt32(row["dst_acc_id"]);
            if (!row.IsNull("dst_client_id")) kr.dst_client_id = Convert.ToInt32(row["dst_client_id"]);
            if (!row.IsNull("dst_curr_value")) kr.dst_curr_value = Convert.ToDouble(row["dst_curr_value"]);

            try
            {
                if (!row.IsNull("user_cr")) kr.user_cr = Convert.ToInt32(row["user_cr"]);
            }
            catch (Exception) { }
            try
            {
                if (!row.IsNull("user_ch")) kr.user_ch = Convert.ToInt32(row["user_ch"]);
            }
            catch (Exception) { }

            if (!row.IsNull("nr_id")) kr.unreadId = Convert.ToInt32(row["nr_id"]);
            if (!row.IsNull("deleted")) kr.deleted = Convert.ToBoolean(row["deleted"]);

            return kr;        
        }
    }
    
    public enum DataAction { NEW, EDIT, DELETE };

    public interface DataFlow
    {
        void OnJournalChanged(KassaRecord oldRecord, KassaRecord newRecord, DataAction action);
    }
}
