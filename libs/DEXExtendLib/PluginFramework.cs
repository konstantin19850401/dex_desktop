using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Windows.Forms;

namespace DEXExtendLib
{
    public class PluginFramework
    {
        public const int PLUGIN_TYPE_DICTIONARY = 0;
        public const int PLUGIN_TYPE_DOCUMENT = 1;
        public const int PLUGIN_TYPE_REPORT = 2;
        public const int PLUGIN_TYPE_FUNCTION = 3;
        public const int PLUGIN_TYPE_SCHEDULE = 4;
        public const int PLUGIN_TYPE_JOURNALHOOK = 5;

        private static PluginFramework pf = null;
        
        private ArrayList dictionaries;
        private ArrayList documents;
        private ArrayList reports;
        private ArrayList functions;
        private ArrayList schedules;
        private ArrayList journalhooks;

        object toolbox;

        private PluginFramework(object toolbox)
        {
            this.toolbox = toolbox;
            dictionaries = new ArrayList();
            documents = new ArrayList();
            reports = new ArrayList();
            functions = new ArrayList();
            schedules = new ArrayList();
            journalhooks = new ArrayList();
        }

        public static PluginFramework getFramework(object toolbox)
        {
            if (pf == null) pf = new PluginFramework(toolbox);
            return pf;
        }

        public ArrayList getDictionaries()
        {
            return dictionaries;            
        }

        public ArrayList getDocuments()
        {
            return documents;
        }

        public ArrayList getReports()
        {
            return reports;
        }

        public ArrayList getFunctions()
        {
            return functions;
        }

        public ArrayList getSchedules()
        {
            return schedules;
        }

        public ArrayList getJournalhooks()
        {
            return journalhooks;
        }

        public IDEXPluginDocument getDocumentByID(string DocID)
        {
            foreach (IDEXPluginDocument item in documents)
            {
                if (item.ID.Equals(DocID)) return item;
            }

            return null;
        }

        public Dictionary<string, string> getDocumentFields(string DocID)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            foreach (IDEXPluginDocument item in documents)
            {
                if (DocID == null || item.ID.Equals(DocID))
                {
                    Dictionary<string, string> f = item.GetDocumentFields(toolbox);
                    if (f != null)
                    {
                        foreach (KeyValuePair<string, string> kvp in f)
                        {
                            ret[kvp.Key] = kvp.Value;
                        }
                    }
                }
            }

            return ret;
        }

        public void ScanPlugins(string directory)
        {
            dictionaries.Clear();
            documents.Clear();
            reports.Clear();
            functions.Clear();
            schedules.Clear();
            journalhooks.Clear();

            if (Directory.Exists(directory))
            {
                foreach (string curfn in Directory.GetFiles(directory, "DEXPlugin.*.dll"))
                {
                    try
                    {
                        Assembly curass = Assembly.LoadFile(curfn);
                        if (curass != null)
                        {
                            foreach (Type curt in curass.GetTypes())
                            {
                                foreach (Type curi in curt.GetInterfaces())
                                {
                                    object plg = null;
                                    if (curi.Equals(typeof(IDEXPluginDictionary)))
                                    {
                                        IDEXPluginDictionary dic = (IDEXPluginDictionary)Activator.CreateInstance(curt);
                                        dictionaries.Add(dic);
                                        plg = dic;
                                    }
                                    if (curi.Equals(typeof(IDEXPluginDocument)))
                                    {
                                        IDEXPluginDocument doc = (IDEXPluginDocument)Activator.CreateInstance(curt);

                                        /*
                                        IDEXDocumentPlans ss = (IDEXDocumentPlans)Activator.CreateInstance(curt);
                                        ss.setPlans(too);
                                        */


                                        documents.Add(doc);
                                        plg = doc;
                                    }
                                    if (curi.Equals(typeof(IDEXPluginReport)))
                                    {
                                        IDEXPluginReport rep = (IDEXPluginReport)Activator.CreateInstance(curt);
                                        reports.Add(rep);
                                        plg = rep;
                                    }
                                    if (curi.Equals(typeof(IDEXPluginFunction)))
                                    {
                                        IDEXPluginFunction fnc = (IDEXPluginFunction)Activator.CreateInstance(curt);
                                        functions.Add(fnc);
                                        plg = fnc;
                                    }
                                    if (curi.Equals(typeof(IDEXPluginSchedule)))
                                    {
                                        IDEXPluginSchedule sch = (IDEXPluginSchedule)Activator.CreateInstance(curt);
                                        schedules.Add(sch);
                                        plg = sch;
                                    }
                                    if (curi.Equals(typeof(IDEXPluginJournalhook)))
                                    {
                                        IDEXPluginJournalhook shk = (IDEXPluginJournalhook)Activator.CreateInstance(curt);
                                        journalhooks.Add(shk);
                                        plg = shk;
                                    }

                                    if (plg is IDEXPluginStartup)
                                    {
                                        ((IDEXPluginStartup)plg).Startup(toolbox);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }

        public int FillButtons(int PluginType, ToolStrip parent, EventHandler handler, bool doInsert)
        {
            if (parent == null) return 0;

            ArrayList plugs = null;
            String namebase = "";

            if (PluginType == PLUGIN_TYPE_DICTIONARY)
            {
                plugs = dictionaries;
                namebase = "dictionariesbutton";
            }

            if (PluginType == PLUGIN_TYPE_DOCUMENT)
            {
                plugs = documents;
                namebase = "documentsbutton";
            }

            if (PluginType == PLUGIN_TYPE_REPORT)
            {
                plugs = reports;
                namebase = "reportsbutton";
            }

            if (PluginType == PLUGIN_TYPE_FUNCTION)
            {
                plugs = functions;
                namebase = "functionsbutton";
            }

            int cnt = 0;
            if (plugs != null)
            {
                foreach (Object plg in plugs)
                {
                    if (plg is IDEXPluginInfo)
                    {
                        ToolStripButton i = new ToolStripButton();
                        i.Name = namebase + cnt.ToString();
                        i.Text = "";
                        i.ToolTipText = ((IDEXPluginInfo)plg).Title + "\n" + ((IDEXPluginInfo)plg).Description;
                        i.Image = ((IDEXPluginInfo)plg).getBitmap();
                        i.Tag = plg;
                        i.Click += handler;
                        if (doInsert)
                        {
                            parent.Items.Insert(0, i);
                        }
                        else
                        {
                            parent.Items.Add(i);
                        }
                        cnt++;
                    }
                }
            }

            return cnt;
        }

        public void FillMenu(int PluginType, ToolStripDropDownItem parent, EventHandler handler, bool setup)
        {
            FillMenu(PluginType, parent, handler, null, setup);
        }

        public void FillMenu(int PluginType, ToolStripDropDownItem parent, EventHandler handler, string[] IDFilter, bool setup)
        {
            if (parent == null) return;
            
            ArrayList plugs = null;
            String namebase = "";

            if (PluginType == PLUGIN_TYPE_DICTIONARY) {
                plugs = dictionaries;
                namebase = "dictionariesitem";
            }

            if (PluginType == PLUGIN_TYPE_DOCUMENT)
            {
                plugs = documents;
                namebase = "documentsitem";
            }

            if (PluginType == PLUGIN_TYPE_REPORT)
            {
                plugs = reports;
                namebase = "reportsitem";
            }

            if (PluginType == PLUGIN_TYPE_FUNCTION)
            {
                plugs = functions;
                namebase = "functionsitem";
            }

            if (PluginType == PLUGIN_TYPE_SCHEDULE)
            {
                plugs = schedules;
                namebase = "schedulesitem";
            }

            if (PluginType == PLUGIN_TYPE_JOURNALHOOK)
            {
                plugs = schedules;
                namebase = "journalhooksitem";
            }

            int cnt = 1;

            if (plugs != null)
            {
                if (setup) namebase = "setup" + namebase;
                    
                foreach (Object plg in plugs)
                {
                    if (plg is IDEXPluginInfo && (!setup || (plg is IDEXPluginSetup)))
                    {
                        bool addPlug = IDFilter == null;

                        if (IDFilter != null)
                        {
                            string plgid = ((IDEXPluginInfo)plg).ID;
                            foreach (string s in IDFilter)
                            {
                                if (s.Equals(plgid))
                                {
                                    addPlug = true;
                                    break;
                                }
                            }
                        }

                        if (addPlug)
                        {
                            ToolStripDropDownItem cur = parent;
                            string[] path = ((IDEXPluginInfo)plg).Path;

                            if (path != null && path.Length > 0)
                            {
                                foreach (string pathitem in path)
                                {
                                    ToolStripMenuItem child = null;
                                    if (cur.HasDropDownItems)
                                    {
                                        foreach (ToolStripMenuItem nchild in cur.DropDownItems)
                                        {
                                            if (nchild.Text.Equals(pathitem))
                                            {
                                                child = nchild;
                                                break;
                                            }
                                        }
                                    }

                                    if (child == null)
                                    {
                                        child = new ToolStripMenuItem();
                                        child.Text = pathitem;
                                        child.Name = namebase + cnt.ToString();
                                        cur.DropDownItems.Add(child);
                                        cnt++;

                                    }

                                    cur = child;
                                }
                            }

                            ToolStripMenuItem i = new ToolStripMenuItem();
                            i.Text = ((IDEXPluginInfo)plg).Title;
                            i.Name = namebase + cnt.ToString();
                            i.ToolTipText = ((IDEXPluginInfo)plg).Description;
                            i.Image = ((IDEXPluginInfo)plg).getBitmap();
                            i.Tag = plg;
                            i.Click += handler;
                            cur.DropDownItems.Add(i);

                            cnt++;
                        }
                    }
                }
            }
        }
    }
}
