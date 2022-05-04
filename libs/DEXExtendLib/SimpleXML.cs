using System;
using System.Xml;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;

namespace DEXExtendLib
{
    public class SimpleXML
    {
        private string ftext;

        public string Name;
        public Dictionary<string, string> Attributes;
        public ArrayList Child;

        public SimpleXML this[string nname]
        {
            get
            {
                if (nname != null && nname.Length > 0)
                {
                    return this.GetNodeByPath(nname, true);
                }
                return null;
            }
            set
            {
                if (nname != null && nname.Length > 0 && value != null)
                {
                    SimpleXML c = this.GetNodeByPath(nname, true);
                    c.Text = value.Text;
                    c.Attributes = new Dictionary<string, string>(value.Attributes);
                }
            }
        }

        /// <summary>
        /// Возвращает ключ Attribute[key] или def, если такого ключа нет.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public string SafeAttribute(string key, string def)
        {
            if (Attributes.ContainsKey(key)) return Attributes[key];
            return def;
        }

        /// <summary>
        /// Возвращает новый объект со свойствами текущего
        /// </summary>
        /// <returns></returns>
        public SimpleXML Clone(bool cloneChildren)
        {
            SimpleXML ret = new SimpleXML(Name);
            ret.Text = ftext;
            foreach (KeyValuePair<string, string> kvp in Attributes)
            {
                ret.Attributes[kvp.Key] = kvp.Value;
            }

            if (cloneChildren && ChildCount > 0)
            {
                foreach (SimpleXML ch in Child)
                {
                    ch.Clone(cloneChildren).Parent = ret;
                }
            }

            return ret;
        }

        public string Text
        {
            get
            {
                return (ftext == null) ? "" : ftext;
            }
            set
            {
                ftext = value;
            }
        }

        public byte[] Binary
        {
            get
            {
                try
                {
                    return System.Convert.FromBase64String(ftext);
                }
                catch (Exception)
                {
                }
                return null;
            }
            set
            {
                try
                {
                    ftext = System.Convert.ToBase64String(value, 0, value.Length, Base64FormattingOptions.InsertLineBreaks);
                }
                catch (Exception)
                {
                    ftext = "";
                }
            }
        }

        public bool isBinary()
        {
            try
            {
                byte[] b = System.Convert.FromBase64String(this.Text);
                return b != null && b.Length > 0;
            }
            catch (Exception)
            {
            }
            return false;
        }
        
        public int ChildCount
        {
            get
            {
                try
                {
                    return Child.Count;
                }
                catch (Exception)
                {
                }
                return 0;
            }
        }

        private SimpleXML FParent = null;

        public SimpleXML Parent
        {
            get
            {
                return FParent;
            }

            set
            {
                if (FParent != null)
                {
                    FParent.Child.Remove(this);
                }

                if (value is SimpleXML)
                {
                    FParent = value;
                    if (FParent != null)
                    {
                        FParent.Child.Add(this);
                    }
                }
                else
                {
                    FParent = null;
                }
            }
        }

        public SimpleXML(string NodeName)
        {
            Name = NodeName;
            Text = "";
            Attributes = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
            Child = new ArrayList();
            Parent = null;
        }

        public SimpleXML(XmlNode node)
        {
            if (node != null)
            {
                Attributes = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
                Child = new ArrayList();
                Parent = null;

                Name = node.Name;
                
                Text = node.Value;

                if (node.Attributes != null)
                {
                    foreach (XmlAttribute attr in node.Attributes)
                    {
                        Attributes[attr.Name] = attr.Value;
                    }
                }

                if (node.HasChildNodes)
                {
                    foreach (XmlNode cnode in node.ChildNodes)
                    {
                        if (cnode.NodeType == XmlNodeType.Text)
                        {
                            Text = cnode.Value;
                        }
                        else
                        {
                            SimpleXML nchild = new SimpleXML(cnode);
                            nchild.Parent = this;
                        }
                    }
                }
            }
        }

        public SimpleXML CreateChild(string NodeName)
        {
            SimpleXML child = new SimpleXML(NodeName);
            child.Parent = this;
            return child;
        }

        public ArrayList GetChildren(string NodeName)
        {
            ArrayList ret = new ArrayList();
            foreach (SimpleXML node in Child)
            {
                if (node.Name.Equals(NodeName, StringComparison.InvariantCultureIgnoreCase))
                {
                    ret.Add(node);
                }
            }

            return ret;
        }

        public SimpleXML GetNodeByPath(string NodePath, bool CreateIfNotExists)
        {
            if (NodePath == null || NodePath == "")
            {
                return null;
            }

            string[] bpath = NodePath.Split(new Char[] {'\\'});
            if (bpath == null || bpath.Length < 1)
            {
                return null;
            }

            ArrayList cpath = new ArrayList();
            foreach (string dpath in bpath)
            {
                if (dpath != null && dpath.Trim().Length > 0)
                {
                    cpath.Add(dpath);
                }
            }

            SimpleXML ret = this;
            foreach(string path in cpath)
            {
                ArrayList curnodes = ret.GetChildren(path);
                if (curnodes.Count > 0)
                {
                    ret = (SimpleXML)curnodes[0];
                }
                else
                {
                    if (CreateIfNotExists)
                    {
                        SimpleXML nret = new SimpleXML(path);
                        nret.Parent = ret;
                        ret = nret;
                    }
                    else
                    {
                        ret = null;
                        break;
                    }
                }
            }

            return ret;
        }

        public static SimpleXML LoadXml(string text)
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.LoadXml(text);

                if (doc.DocumentElement != null)
                {
                    return new SimpleXML(doc.DocumentElement);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
            }
            return null;
        }

        public XmlNode SaveNode(XmlDocument doc, bool isRoot)
        {
           
            XmlNodeType docType = /*(isRoot) ? XmlNodeType.Document :*/ XmlNodeType.Element;

            XmlNode n = doc.CreateNode(docType, Name, null);
            try
            {
                if (ChildCount < 1 && !isRoot) n.InnerText = Text;

                if (Attributes != null && Attributes.Count > 0)
                {
                    foreach (KeyValuePair<string, string> kvp in Attributes)
                    {
                        XmlAttribute xattr = doc.CreateAttribute(kvp.Key);
                        xattr.Value = kvp.Value;
                        n.Attributes.Append(xattr);
                    }
                }

                if (ChildCount > 0)
                {
                    foreach (SimpleXML cnode in Child)
                    {
                        n.AppendChild(cnode.SaveNode(doc, false));
                    }
                }
            }
            catch (Exception) {
                //string dddff = "";
            }
            return n;
        }

        public static string SaveXml(SimpleXML document)
        {
            
            XmlDocument doc = new XmlDocument();
            
            if (document != null)
            {
                XmlNode node = document.SaveNode(doc, true);
                if (node != null)
                {
                    doc.AppendChild(node);
                }
            }

            XmlDeclaration xd = doc.CreateXmlDeclaration("1.0", "Utf-8", null);
            doc.InsertBefore(xd, doc.DocumentElement);

            
            return doc.OuterXml;
        }
    }

}
