using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DEXExtendLib;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DEXPlugin.Document.Beeline.DOL2.Contract
{
    public partial class ActivateSim : Form
    {
        public ActivateSim()
        {
            InitializeComponent();
        }

        string gcheckCode = "", guid = "", gpfBase = "", gnodejsserver = "", gcurrentBase = "", gdexUid = "", gsernum = "";
        Object gtoolbox;
        public ActivateSim(Object toolbox, string boxType, string sernum, string snb, string soc, DataTable dtPlans, string checkCode, string uid, string pfBase, string dexUid, string nodejsserver, string currentBase) 
        {
            InitializeComponent();
            try
            {
                gcheckCode = checkCode;
                guid = uid;
                gpfBase = pfBase;
                gnodejsserver = nodejsserver;
                gcurrentBase = currentBase;
                gdexUid = dexUid;
                gsernum = sernum;
                gtoolbox = toolbox;
                // запросим точек SCAD для данного субдилера
                IDEXServices idis = (IDEXServices)toolbox;

                bool isOnline = ((IDEXUserData)toolbox).isOnline;
                //string currentBase = ((IDEXUserData)toolbox).currentBase;
                //string dexUid = "";
                //if (isOnline) dexUid = ((IDEXUserData)toolbox).UID;

                //string ssss = ((IDEXUserData)toolbox).UID;


                JObject packet = new JObject();
                packet["com"] = "dexdealer.adapters.beeline";
                packet["subcom"] = "apiGetScadPoints";
                packet["client"] = "dexol";
                packet["data"] = new JObject();
                packet["data"]["vendor"] = "beeline";
                packet["data"]["dexuid"] = dexUid;
                packet["data"]["base"] = currentBase;
                packet["data"]["icc"] = sernum;
                packet["data"]["checkCode"] = checkCode;
                JObject obj = JObject.Parse(idis.sendRequest("GET", nodejsserver, "3020", "/dexdealer/beeline/cmd?packet=" + JsonConvert.SerializeObject(packet) + "&uid=" + uid + "&clientType=dexol&dexolUid=" + dexUid, 1));

                //JObject obj = JObject.Parse(idis.sendRequest("GET", dexServer, "3000", "/adapters/getScadPointByDexUid?data={\"pfBase\":\"" + pfBase + "\",\"dexUid\":\"" + dexUid + "\"}&uid=" + uid + "&clientType=dexol&dexolUid=" + dexUid, 1));
                int status = Convert.ToInt32(obj["data"]["status"].ToString());
                if (status == -1)
                {
                    cbScadPoints.Visible = false;
                    label4.Visible = false;
                    cbNumbers.Enabled = false;
                    label5.Text = "Субдилер не имеет точки продаж. Обратитесь в офис.";
                }
                else if (status == 1)
                {
                    label5.Visible = false;
                    cbScadPoints.Items.Clear();

                    foreach (JObject jo in obj["data"]["pointList"])
                    {
                        cbScadPoints.Items.Add(new StringTagItem(jo["address"].ToString(), jo["scadPointCode"].ToString()));
                    }

                    //for (int i = 0; i < obj["data"]["pointList"].Count(); i++)
                    //{
                        //cbScadPoints.Items.Add(new StringTagItem(obj["data"]["pointList"][i.ToString()]["address"].ToString(), obj["data"]["pointList"][i.ToString()]["code"].ToString()));
                    //    cbScadPoints.Items.Add(new StringTagItem(obj["data"]["pointList"][i.ToString()]["address"].ToString(), obj["data"]["pointList"][i.ToString()]["scadPointCode"].ToString()));
                    //}



                    // узнаем список тп, который можно подключить для этой динамической сим

                    JObject accessTP = new JObject();
                    bool ifSimActivated = false;
                    if ("U".Equals(boxType))
                    {
                        try
                        {
                            packet = new JObject();
                            packet["com"] = "dexdealer.adapters.beeline";
                            packet["subcom"] = "apiGetSuitableDynamicSIM";
                            packet["client"] = "dexol";
                            packet["data"] = new JObject();
                            packet["data"]["vendor"] = "beeline";
                            packet["data"]["dexuid"] = dexUid;
                            packet["data"]["base"] = currentBase;
                            packet["data"]["icc"] = sernum;
                            packet["data"]["checkCode"] = checkCode;
                            accessTP = JObject.Parse(idis.sendRequest("GET", nodejsserver, "3020", "/dexdealer/beeline/cmd?packet=" + JsonConvert.SerializeObject(packet) + "&uid=" + uid + "&clientType=dexol&dexolUid=" + dexUid, 1));
                            if (Convert.ToInt32(accessTP["data"]["status"].ToString()) != 1)
                            {
                                if (accessTP["data"]["RUGetSuitablePricePlansDynamicSIMResult"].ToString() != "")
                                {
                                    MessageBox.Show(accessTP["data"]["RUGetSuitablePricePlansDynamicSIMResult"].ToString());
                                    ifSimActivated = true;
                                }
                            }
                        }
                        catch (Exception)
                        {
                            string ss = "dsc";
                        }
                    }







                    //если доступна только одна точка скад, то выберем ее
                    if (cbScadPoints.Items.Count == 1)
                    {
                        cbScadPoints.SelectedIndex = 0;
                    }

                    // займемся тарифными планами
                    cbPlans.Items.Clear();
                    cbPlans.Items.Add(new StringTagItem("", StringTagItem.VALUE_ANY));

                    if ("U".Equals(boxType) && !ifSimActivated)
                    {
                        foreach (DataRow dr in dtPlans.Rows)
                        {
                            foreach (JObject jo in accessTP["data"]["list"])
                            {
                                if (jo["$attributes"]["SOC"].ToString() == dr["plan_id"].ToString())
                                {
                                    cbPlans.Items.Add(new StringTagItem(dr["title"].ToString(), dr["plan_id"].ToString()));
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (DataRow dr in dtPlans.Rows)
                        {
                            cbPlans.Items.Add(new StringTagItem(dr["title"].ToString(), dr["plan_id"].ToString()));
                        }
                    }


                    

                    StringTagItem.SelectByTag(cbPlans, soc, false);
                    if ("D".Equals(boxType))
                    {
                        cbPlans.Enabled = false;
                        label3.Text = "Тарифный план не доступен для выбора";
                    }
                    else if ("U".Equals(boxType))
                    {
                        label3.Text = "Тарифный план доступен для выбора";
                    }

                    // проверим, не активирована ли сим, для этого переберем все варианты точек скад для данного суба
                    bool ifActivated = false;
                    foreach (JObject jo in obj["data"]["pointList"])
                    {
                        packet = new JObject();
                        packet["com"] = "dexdealer.adapters.beeline";
                        packet["subcom"] = "apiGetStatusActivationOfSim";
                        packet["client"] = "dexol";
                        packet["data"] = new JObject();
                        packet["data"]["vendor"] = "beeline";
                        packet["data"]["dexuid"] = dexUid;
                        packet["data"]["base"] = currentBase;
                        packet["data"]["icc"] = sernum;
                        packet["data"]["checkCode"] = checkCode;
                        packet["data"]["skadPoint"] = jo["scadPointCode"].ToString();
                        obj = JObject.Parse(idis.sendRequest("GET", nodejsserver, "3020", "/dexdealer/beeline/cmd?packet=" + JsonConvert.SerializeObject(packet) + "&uid=" + uid + "&clientType=dexol&dexolUid=" + dexUid, 1));

                        if (obj["data"]["filialName"].ToString() != "")
                        {   
                            // сим была активирована на данну точку, запретим выбор точек, номеров, тарифных планов и выберем необходимое в них
                            ifActivated = true;
                            cbScadPoints.Enabled = false;
                            StringTagItem.SelectByTag(cbScadPoints, jo["scadPointCode"].ToString(), true);

                            cbNumbers.Enabled = false;
                            cbNumbers.Items.Add(new StringTagItem(obj["data"]["ctn"].ToString(), obj["data"]["ctn"].ToString()));
                            cbNumbers.SelectedIndex = 0;

                            cbPlans.Enabled = false;
                            StringTagItem.SelectByTag(cbPlans, obj["data"]["soc"].ToString(), true);

                            
                            break;
                        }
                    }



                    if (!ifActivated)
                    {
                        // запросим список номеров для активации сим-карты
                        //obj = JObject.Parse(idis.sendRequest("GET", dexServer, "3000", "/adapters/rUGetBeautifulNumberList?data={\"pfBase\":\"" + pfBase + "\",\"icc\":\"" + sernum + "\",\"checkCode\":\"" + checkCode + "\",\"dexUid\":\"" + dexUid + "\"}&uid=" + uid + "&clientType=dexol&dexolUid=" + dexUid, 0));
                        packet = new JObject();
                        packet["com"] = "dexdealer.adapters.beeline";
                        packet["subcom"] = "apiGetBeautifulNumberList";
                        packet["client"] = "dexol";
                        packet["data"] = new JObject();
                        packet["data"]["vendor"] = "beeline";
                        packet["data"]["dexuid"] = dexUid;
                        packet["data"]["base"] = currentBase;
                        packet["data"]["icc"] = sernum;
                        packet["data"]["checkCode"] = checkCode;
                        if (!isOnline) packet["data"]["ignoreUid"] = 1;
                        else packet["data"]["ignoreUid"] = 0;

                        obj = JObject.Parse(idis.sendRequest("GET", nodejsserver, "3020", "/dexdealer/beeline/cmd?packet=" + JsonConvert.SerializeObject(packet) + "&uid=" + uid + "&clientType=dexol&dexolUid=" + dexUid, 1));
                        cbNumbers.Items.Clear();

                        if (Convert.ToInt32(obj["data"]["status"].ToString()) == -6)
                        {
                            MessageBox.Show(obj["data"]["message"].ToString());
                            this.Close();
                        }
                        else
                        {
                            try
                            {
                                string ll = obj["data"]["data"][0.ToString()]["CTN"].ToString();
                            }
                            catch (Exception) { }

                            foreach (JObject jo in obj["data"]["list"])
                            {
                                cbNumbers.Items.Add(new StringTagItem(jo["CTN"].ToString(), jo["CTN"].ToString()));
                            }

                            //for (int i = 0; i < obj["data"]["data"].Count(); i++)
                            //{
                            //    cbNumbers.Items.Add(new StringTagItem(obj["data"]["data"][i.ToString()]["CTN"].ToString(), i.ToString()));
                            //}

                            //точки scad
                            try
                            {
                                if (obj["data"]["status"].ToString().Equals("20"))
                                {
                                    //StringTagItem.getSelectedTag(cbScadPoints, "");
                                    StringTagItem.SelectByTag(cbScadPoints, obj["data"]["scadPointIfSimActivated"].ToString(), true);
                                    cbScadPoints.Enabled = false;
                                }
                                else if (obj["data"]["status"].ToString().Equals("35"))
                                {
                                    MessageBox.Show(obj["data"]["message"].ToString());
                                }
                            }
                            catch (Exception) { }
                        }
                    }
                    
                }

                
               

                //ширина комбо с точкой SCAD
                /*
                int newWidth = cbScadPoints.DropDownWidth;
                int width = cbScadPoints.DropDownWidth;
                System.Drawing.Font font = cbScadPoints.Font;
                System.Drawing.Graphics g = cbScadPoints.CreateGraphics();
                int vertScrollBarWidth = (cbScadPoints.Items.Count > cbScadPoints.MaxDropDownItems) ?
                  System.Windows.Forms.SystemInformation.VerticalScrollBarWidth : 0;
                foreach (object item in (cbScadPoints).Items)
                {
                    string s = cbScadPoints.GetItemText(item);
                    newWidth = (int)g.MeasureString(s, font).Width + vertScrollBarWidth;
                    if (width < newWidth)
                    {
                        width = newWidth;
                    }
                }
                cbScadPoints.DropDownWidth = width;
                */
                cbScadPoints.DropDownWidth = comboBoxDropDownWidth(cbScadPoints);

                //ширина комбо тарифных планов
                cbPlans.DropDownWidth = comboBoxDropDownWidth(cbPlans);
            }
            catch (Exception) { }
            //return data;
        }

        private int comboBoxDropDownWidth(ComboBox cb)
        {
            int newWidth = cb.DropDownWidth;
            int width = cb.DropDownWidth;
            System.Drawing.Font font = cb.Font;
            System.Drawing.Graphics g = cb.CreateGraphics();
            int vertScrollBarWidth = (cb.Items.Count > cb.MaxDropDownItems) ?
              System.Windows.Forms.SystemInformation.VerticalScrollBarWidth : 0;
            foreach (object item in (cb).Items)
            {
                string s = cb.GetItemText(item);
                newWidth = (int)g.MeasureString(s, font).Width + vertScrollBarWidth;
                if (width < newWidth)
                {
                    width = newWidth;
                }
            }


            return width;
        }
 
        private void button1_Click(object sender, EventArgs e)
        {
            string err = "";
            if (cbScadPoints.SelectedIndex == -1)
            {
                err += "Вы не выбрали номер точки SCAD; ";
            }
            if (cbNumbers.SelectedIndex == -1) 
            {
                err += "Вы не выбрали номер для привязки!";
            }
            if (err.Length > 0) 
            {
                MessageBox.Show(err);
            } 
            else
            {
                try
                {
                    IDEXServices idis = (IDEXServices)gtoolbox;
                    string soc = StringTagItem.getSelectedTag(cbPlans, "");
                    string msisdn = StringTagItem.getSelectedTag(cbNumbers, "");
                    string scadPointCode = StringTagItem.getSelectedTag(cbScadPoints, "");

                    JObject packet = new JObject();
                    packet["com"] = "dexdealer.adapters.beeline";
                    packet["subcom"] = "apiActivateDynamicSIM2";
                    packet["client"] = "dexol";
                    packet["data"] = new JObject();
                    packet["data"]["vendor"] = "beeline";
                    packet["data"]["dexuid"] = gdexUid;
                    packet["data"]["base"] = gcurrentBase;
                    packet["data"]["soc"] = soc;
                    packet["data"]["beautifulCTN"] = msisdn;
                    packet["data"]["icc"] = gsernum;
                    packet["data"]["checkCode"] = gcheckCode;
                    packet["data"]["skadPointCode"] = scadPointCode;


                    JObject obj = JObject.Parse(idis.sendRequest("GET", gnodejsserver, "3020", "/dexdealer/beeline/cmd?packet=" + JsonConvert.SerializeObject(packet) + "&uid=" + guid + "&clientType=dexol&dexolUid=" + gdexUid, 1));

                     int status = Convert.ToInt32(obj["data"]["status"].ToString());
                     if (status != -1 && status != -2 && status != -5 && status != -4 && status != -7)
                     {
                         DialogResult = DialogResult.OK;
                     }
                     else
                     {
                         
                         string message = obj["data"]["RUCheckDynamicSIMResult"].ToString();
                         if (status == -7) message = obj["data"]["activationError"].ToString();
                         MessageBox.Show(message);
                     }

                }
                catch (Exception)
                {
                }

                
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                BindingNumber bnnum = new BindingNumber();
                if (bnnum.ShowDialog() == DialogResult.OK)
                {
                    int total = cbNumbers.Items.Count;
                    cbNumbers.Items.Add(new StringTagItem(bnnum.tbBindingNumber.Text, total.ToString()));
                    StringTagItem.SelectByTag(cbNumbers, total.ToString(), false);
                }
            }
            catch (Exception) { }
        }
    }
}
