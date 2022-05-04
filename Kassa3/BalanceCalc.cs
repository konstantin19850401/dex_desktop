using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kassa3
{
    class BalanceItem
    {
        public int currId;
        public double balance;

        public BalanceItem(int currId, double startBalance)
        {
            this.currId = currId;
            this.balance = startBalance;
        }
    }

    class BalanceCalcCur
    {
        public Dictionary<int, BalanceItem> accounts = new Dictionary<int, BalanceItem>(); // AccID : Acc
        public Dictionary<int, double> currencies = new Dictionary<int, double>(); // CurrID : CurrValue

        public void AddAccount(int accId, int currId, double startBalance)
        {
            accounts[accId] = new BalanceItem(currId, startBalance);
        }

        public void ProcessOperation(double r_sum, int srctype, int src_acc_id, double src_curr_value, int dsttype, int dst_acc_id, double dst_curr_value)
        {
            if (srctype == 0 && dsttype == 0) // Со счёта на счёт
            {
                if (accounts.ContainsKey(src_acc_id)) accounts[src_acc_id].balance -= r_sum;
                if (accounts.ContainsKey(dst_acc_id)) accounts[dst_acc_id].balance += r_sum * src_curr_value / dst_curr_value;
            }
            else if (srctype == 0) // Со счёта контрагенту
            {
                if (accounts.ContainsKey(src_acc_id)) accounts[src_acc_id].balance -= r_sum;
            }
            else if (dsttype == 0) // От контрагента на счёт
            {
                if (accounts.ContainsKey(dst_acc_id)) accounts[dst_acc_id].balance += r_sum;
            }
        }
    }

    class BankTagItem
    {
        public int currId;
        public string bankTag;

        public BankTagItem(int currId, string bankTag)
        {
            this.currId = currId;
            this.bankTag = bankTag;
        }
    }

    class BalanceCalcBT
    {
        public Dictionary<int, BankTagItem> accounts = new Dictionary<int, BankTagItem>(); // AccID : Acc
        public Dictionary<int, string> currnames; // CurrID : CurrTitle
        
        public Dictionary<int, Dictionary<string, Dictionary<int, double>>> fgroups =
            new Dictionary<int, Dictionary<string, Dictionary<int, double>>>(); //firmid, <banktag <CurrID, balance>>

        public Dictionary<int, int> firmsaccs = new Dictionary<int, int>(); // AccID : FirmID


        public void AddAccount(int accId, int currId, string bankTag)
        {
            accounts[accId] = new BankTagItem(currId, bankTag);
        }

        public Dictionary<string, string> getBankTagFields() // <field name, field title>
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            foreach (KeyValuePair<int, Dictionary<string, Dictionary<int, double>>> grp in fgroups)
            {
                foreach (KeyValuePair<string, Dictionary<int, double>> btg in grp.Value)
                {
                    foreach (KeyValuePair<int, double> currid in btg.Value)
                    {
                        string fname = btg.Key.GetHashCode().ToString() + "_" + currid.Key.ToString();
                        if (!ret.ContainsKey(fname))
                        {
                            string ftitle = btg.Key;
                            if (currnames.ContainsKey(currid.Key)) ftitle += " (" + currnames[currid.Key] + ")";
                            ret[fname] = ftitle;
                        }
                    }
                }
            }

            return ret;
        }
        
        void ProcessBalance(int accid, double summ)
        {
            if (accounts.ContainsKey(accid))
            {
                string bankTag = accounts[accid].bankTag;
                int firmid = -1;
                foreach (KeyValuePair<int, int> kvp in firmsaccs)
                {
                    if (kvp.Key == accid)
                    {
                        firmid = kvp.Value;
                        break;
                    }
                }

                if (firmid > -1)
                {
                    // Найдена фирма, фингруппа и валюта счёта
                    if (!fgroups.ContainsKey(firmid)) fgroups[firmid] = new Dictionary<string, Dictionary<int, double>>();
                    if (!fgroups[firmid].ContainsKey(bankTag)) fgroups[firmid][bankTag] = new Dictionary<int, double>();
                    if (!fgroups[firmid][bankTag].ContainsKey(accounts[accid].currId)) fgroups[firmid][bankTag][accounts[accid].currId] = 0;
                    fgroups[firmid][bankTag][accounts[accid].currId] += summ;
                }
            }
        }

        public void ProcessOperation(double r_sum, int srctype, int src_acc_id, double src_curr_value, int dsttype, int dst_acc_id, double dst_curr_value)
        {
            if (srctype == 0 && dsttype == 0) // Со счёта на счёт
            {
                if (accounts.ContainsKey(src_acc_id)) ProcessBalance(src_acc_id, -r_sum);
                if (accounts.ContainsKey(dst_acc_id)) ProcessBalance(dst_acc_id, r_sum * src_curr_value / dst_curr_value);
            }
            else if (srctype == 0) // Со счёта контрагенту
            {
                if (accounts.ContainsKey(src_acc_id)) ProcessBalance(src_acc_id, -r_sum);
            }
            else if (dsttype == 0) // От контрагента на счёт
            {
                if (accounts.ContainsKey(dst_acc_id)) ProcessBalance(dst_acc_id, r_sum);
            }
        }

    }
}
