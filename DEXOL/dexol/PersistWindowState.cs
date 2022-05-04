using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace dexol
{
    public class PersistWindowState : System.ComponentModel.Component
    {
        // event info that allows form to persist extra window state data
        public delegate void WindowStateDelegate(object sender, JObject cfg);
        public event WindowStateDelegate LoadStateEvent;
        public event WindowStateDelegate SaveStateEvent;
        private Form m_parent;
        private int m_normalLeft;
        private int m_normalTop;
        private int m_normalWidth;
        private int m_normalHeight;
        private FormWindowState m_windowState;
        private bool m_allowSaveMinimized = false;
        
        public PersistWindowState(Form fparent)
        {
            Parent = fparent;
        }

        public Form Parent
        {
            set
            {
                m_parent = value;
                // subscribe to parent form's events
                m_parent.Closing += new System.ComponentModel.CancelEventHandler(OnClosing);
                m_parent.Resize += new System.EventHandler(OnResize);
                m_parent.Move += new System.EventHandler(OnMove);
                m_parent.Load += new System.EventHandler(OnLoad);
                // get initial width and height in case form is never resized
                m_normalWidth = m_parent.Width;
                m_normalHeight = m_parent.Height;
            }
            get
            {
                return m_parent;
            }
        }
        public bool AllowSaveMinimized
        {
            set
            {
                m_allowSaveMinimized = value;
            }
        }
        private void OnResize(object sender, System.EventArgs e)
        {
            // save width and height
            if (m_parent.WindowState == FormWindowState.Normal)
            {
                m_normalWidth = m_parent.Width;
                m_normalHeight = m_parent.Height;
            }
        }
        private void OnMove(object sender, System.EventArgs e)
        {
            // save position
            if (m_parent.WindowState == FormWindowState.Normal)
            {
                m_normalLeft = m_parent.Left;
                m_normalTop = m_parent.Top;
            }
            // save state
            m_windowState = m_parent.WindowState;
        }
        private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // save position, size and state
            JObject cfg = new JObject();
            cfg["Left"] = m_normalLeft;
            cfg["Top"] = m_normalTop;
            cfg["Width"] = m_normalWidth;
            cfg["Height"] = m_normalHeight;

            // check if we are allowed to save the state as minimized (not normally)
            if (!m_allowSaveMinimized)
            {
                if (m_windowState == FormWindowState.Minimized)
                    m_windowState = FormWindowState.Normal;
            }
            cfg["WindowState"] = (int)m_windowState;

            // fire SaveState event
            if (SaveStateEvent != null)
                SaveStateEvent(this, cfg);

            File.WriteAllText(Parent.Name + ".params.json", cfg.ToString(), Encoding.UTF8);

        }
        private void OnLoad(object sender, System.EventArgs e)
        {
            // attempt to read state from registry
            JObject cfg = null;
            try
            {
                cfg = JObject.Parse(File.ReadAllText(Parent.Name + ".params.json", Encoding.UTF8));
            }
            catch (Exception) 
            { 
                cfg = null;
            }

            if (cfg != null)
            {
                int left = (int)cfg["Left"];
                int top = (int)cfg["Top"];
                int width = (int)cfg["Width"];
                int height = (int)cfg["Height"];
                FormWindowState windowState = (FormWindowState)((int)cfg["WindowState"]);
                m_parent.Location = new Point(left, top);
                m_parent.Size = new Size(width, height);
                m_parent.WindowState = windowState;
                // fire LoadState event
                if (LoadStateEvent != null)
                    LoadStateEvent(this, cfg);
            }
        }
    }
}
