using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace DEXExtendLib
{
    [ToolboxBitmap(typeof(System.Windows.Forms.DateTimePicker))] //Show datetimepicker icon 
    public class DateEdit : ComboBox
    {
        static string m_regex = @"[0-9]";
        // use MinValue as NullValue
        public static readonly DateTime NullValue = DateTime.MinValue;

        private string m_format;		// display format (mask with input chars replaced by input char)
        private string m_convert;		// used to convert DateTime to string
        private char m_inpChar = '_';
        private int m_caret;
        private Dictionary<int, int> m_posNdx;

        #region events
        public delegate void InvalidDateEventHandler(object sender, EventArgs e);
        public delegate void ValidDateEventHandler(object sender, EventArgs e);

        public event InvalidDateEventHandler InvalidDate;
        public event ValidDateEventHandler ValidDate;

        [Category("Behavior")]
        protected virtual void OnInvalidDate(EventArgs e)
        {
            if (InvalidDate != null)
                InvalidDate(this, e);
        }
        [Category("Behavior")]
        protected virtual void OnValidDate(EventArgs e)
        {
            if (ValidDate != null)
                ValidDate(this, e);
        }
        #endregion events

        #region Public
        public DateEdit()
        {
            BuildFormat();
            base.MaxLength = m_format.Length;
            BuildPosNdx();
            base.Text = m_format;

            // disable context menu since it bypasses Ctrl+V handler
            this.ContextMenu = new ContextMenu();
        }
        [Description("Sets the Input Char default '_'"), Category("Behavior"),
        RefreshProperties(RefreshProperties.All)]
        public char InputChar
        {
            // default '_'
            get { return m_inpChar; }
            set
            {
                m_inpChar = value;
                BuildFormat();
            }
        }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override string Text
        {
            get { return base.Text; }
            set
            {
                DateTime dt;
                if (DateTime.TryParse(value, out dt))
                    this.Value = dt;
                else
                    this.Value = NullValue;
            }
        }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public bool IsValid
        {
            get
            {
                try
                {
                    // null is valid
                    if (base.Text == m_format)
                        return true;

                    DateTime ret = DateTime.Parse(base.Text);
                }
                catch
                {
                    return false;
                }

                return true;
            }
        }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public DateTime Value
        {
            get
            {
                // TODO: What to return if Null not allowed and invalid value?
                //	a) error?
                //	b) Null?
                DateTime ret;
                try
                {
                    ret = DateTime.Parse(base.Text);
                }
                catch
                {
                    ret = NullValue;
                }

                return ret;
            }
            set
            {
                if (value == NullValue)
                    base.Text = m_format;
                else
                    base.Text = value.ToString(m_convert);	// TODO: must format using current culture!!!

                OnValidDate(EventArgs.Empty);
            }
        }
        #endregion Public

        #region Overriding
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // NOTES: 
            //	1) break; causes warnings below
            //	2) m_caret tracks caret location, always the start of selected char
            //	3) code below is based on MaskedEdit, since our format is fixed
            //		there may be optimizations possible
            int strt = base.SelectionStart;
            int len = base.SelectionLength;
            int end = strt + base.SelectionLength - 1;
            string s = base.Text;
            int p;

            // handle startup, runs once
            if (m_format[strt] != m_inpChar)
            {
                strt = Next(-1);
                len = 1;
            }

            switch (keyData)
            {
                case Keys.Left:
                case Keys.Up:
                    p = Prev(strt);
                    if (p != strt)
                    {
                        base.SelectionStart = p;
                        base.SelectionLength = 1;
                    }
                    m_caret = p;
                    return true;
                case Keys.Left | Keys.Shift:
                case Keys.Up | Keys.Shift:
                    if ((strt < m_caret) || (strt == m_caret && len <= 1))
                    {
                        // enlarge left
                        p = Prev(strt);
                        base.SelectionStart -= (strt - p);
                        base.SelectionLength = len + (strt - p);
                    }
                    else
                    {
                        // shrink right
                        base.SelectionLength = len - (end - Prev(end));
                    }
                    return true;
                case Keys.Right:
                case Keys.Down:
                    p = Next(strt);
                    if (p != strt)
                    {
                        base.SelectionStart = p;
                        base.SelectionLength = 1;
                    }
                    m_caret = p;
                    return true;
                case Keys.Right | Keys.Shift:
                case Keys.Down | Keys.Shift:
                    if (strt < m_caret)
                    {
                        // shrink left
                        p = Next(strt);
                        base.SelectionStart += (p - strt);
                        base.SelectionLength = len - (p - strt);
                    }
                    else if (strt == m_caret)
                    {
                        // enlarge right
                        p = Next(end);
                        base.SelectionLength = len + (p - end);
                    }
                    return true;
                case Keys.Delete:
                    // delete selection, replace with input format
                    base.Text = s.Substring(0, strt) + m_format.Substring(strt, len) + s.Substring(strt + len);
                    base.SelectionStart = strt;
                    base.SelectionLength = 1;
                    m_caret = strt;
                    return true;
                case Keys.Home:
                case Keys.Left | Keys.Control:
                case Keys.Home | Keys.Control:
                    base.SelectionStart = Next(-1);
                    base.SelectionLength = 1;
                    m_caret = base.SelectionStart;
                    return true;
                case Keys.Home | Keys.Shift:
                    if (strt <= m_caret && len <= 1)
                    {
                        // enlarge left
                        p = Next(-1);
                        base.SelectionStart -= (strt - p);
                        base.SelectionLength = len + (strt - p);
                    }
                    else
                    {
                        // shrink right
                        p = Next(-1);
                        base.SelectionStart = p;
                        base.SelectionLength = (m_caret - p) + 1;
                    }
                    return true;
                case Keys.End:
                case Keys.Right | Keys.Control:
                case Keys.End | Keys.Control:
                    base.SelectionStart = Prev(base.MaxLength);
                    base.SelectionLength = 1;
                    m_caret = base.SelectionStart;
                    return true;
                case Keys.End | Keys.Shift:
                    if (strt < m_caret)
                    {
                        // shrink left
                        p = Prev(base.MaxLength);
                        base.SelectionStart = m_caret;
                        base.SelectionLength = (p - m_caret + 1);
                    }
                    else if (strt == m_caret)
                    {
                        // enlarge right
                        p = Prev(base.MaxLength);
                        base.SelectionLength = len + (p - end);
                    }
                    return true;
                case Keys.V | Keys.Control:
                case Keys.Insert | Keys.Shift:
                    // paste from clipboard
                    IDataObject iData = Clipboard.GetDataObject();

                    // assemble new text
                    string t = s.Substring(0, strt)
                        + (string)iData.GetData(DataFormats.Text)
                        + s.Substring(strt + len);

                    // check if data to be pasted is convertable to inputType
                    DateTime dt; ;
                    try
                    {
                        dt = DateTime.Parse(t);
//                        base.Text = dt.ToString("MM/dd/yyyy");
                        base.Text = dt.ToString(m_convert);

                        // reset selection
                        base.SelectionStart = strt;
                        base.SelectionLength = len;
                    }
                    catch
                    {
                        // do nothing
                    }

                    return true;
                default:
                    return base.ProcessCmdKey(ref msg, keyData);
            }
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            // reset selection to include input chars
            int strt = base.SelectionStart;
            int orig = strt;
            int len = base.SelectionLength;

            // reset selection start
            if (strt == base.MaxLength || m_format[strt] != m_inpChar)
            {
                // reset start
                if (Next(strt) == strt)
                    strt = Prev(strt);
                else
                    strt = Next(strt);

                base.SelectionStart = strt;
            }

            // reset selection length
            if (len < 1)
                base.SelectionLength = 1;
            else if (m_format[orig + len - 1] != m_inpChar)
            {
                len += Next(strt + len) - (strt + len);
                base.SelectionLength = len;
            }

            m_caret = strt;
            base.OnMouseUp(e);
        }
        protected override void OnLeave(EventArgs e)
        {
            // validate entry
            autoCompleteText();
            try
            {
                if (this.Text == m_format)
                    return;
                DateTime dt = DateTime.Parse(this.Text);
                OnValidDate(EventArgs.Empty);
            }
            catch
            {
                // fire InvalidDate event
                OnInvalidDate(EventArgs.Empty);
            }
            finally
            {
                base.OnLeave(EventArgs.Empty);
            }
        }
        private void autoCompleteText()
        {
            string s = this.Text;
            string sep = DateTimeFormatInfo.CurrentInfo.DateSeparator;
            string patern = m_inpChar.ToString() + m_inpChar + sep + m_inpChar + m_inpChar +
                sep + m_inpChar + m_inpChar + m_inpChar + m_inpChar;

            if (s.EndsWith(patern.Substring(1)))
                this.Text = "0" + s.Substring(0, 1) + sep + DateTime.Now.Month.ToString("00") + sep + DateTime.Now.Year.ToString("0000");
            else if (s.EndsWith(patern.Substring(3)))
                this.Text = s.Substring(0, 3) + DateTime.Now.Month.ToString("00") + sep + DateTime.Now.Year.ToString("0000");
            else if (s.EndsWith(patern.Substring(6)))
                this.Text = s.Substring(0, 6) + DateTime.Now.Year.ToString("0000");
            else
            {
                DateTime dt;
                if (DateTime.TryParse(s.Replace(m_inpChar.ToString(), ""), out dt))
                    this.Text = dt.ToShortDateString();
            }

        }
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            int strt = base.SelectionStart;
            int len = base.SelectionLength;
            int p;

            // Handle Backspace -> replace previous char with inpchar and select
            if (e.KeyChar == 0x08)
            {
                string s = base.Text;
                p = Prev(strt);
                if (p != strt)
                {
                    base.Text = s.Substring(0, p) + m_inpChar.ToString() + s.Substring(p + 1);
                    base.SelectionStart = p;
                    base.SelectionLength = 1;

                }
                m_caret = p;
                e.Handled = true;
                return;
            }

            // update display if valid char entered
            if (IsValidChar(e.KeyChar, (int)m_posNdx[strt]))
            {
                // assemble new text
                string t = "";
                t = base.Text.Substring(0, strt);
                t += e.KeyChar.ToString();

                if (strt + len != base.MaxLength)
                {
                    t += m_format.Substring(strt + 1, len - 1);
                    t += base.Text.Substring(strt + len);
                }
                else
                    t += m_format.Substring(strt + 1);

                base.Text = t;

                // select next input char
                strt = Next(strt);
                base.SelectionStart = strt;
                m_caret = strt;
                base.SelectionLength = 1;
            }
            e.Handled = true;
        }
        protected override void OnDropDown(EventArgs e)
        {
            Form parent = this.FindForm();
            Calendar calendar = new Calendar(this);
            if (parent == null)
            {
                calendar.Location = this.PointToScreen(new System.Drawing.Point(this.Left + 3, this.Bottom + 3));
            }
            else if (this.Parent is ToolStrip)
            {
                calendar.Location = parent.PointToScreen(new System.Drawing.Point(this.Left, this.Parent.Height + this.Bottom));
            }
            else
            {
                calendar.Location = this.Parent.PointToScreen(new System.Drawing.Point(this.Left + 3, this.Bottom + 3));
//                calendar.Location = parent.PointToScreen(new System.Drawing.Point(this.Left + 3, this.Bottom + 3));
            }

            this.DropDownHeight = 1;
            this.DropDownWidth = calendar.Width;
            if (!calendar.Visible)
            {
                if (this.Value == NullValue)
                    calendar.MonthCalendar.SetDate(DateTime.Now);
                else
                    calendar.MonthCalendar.SetDate(this.Value);
                calendar.Show(parent);
            }
            base.OnDropDown(e);
        }
        #endregion Overriding

        #region private
        private static bool IsValidChar(char input, int pos)
        {
            // validate input char against mask
            return Regex.IsMatch(input.ToString(), m_regex);
        }
        private int Prev(int startPos)
        {
            // return previous input char position
            // returns current position if no input chars to the left
            int strt = startPos;
            int ret = strt;

            while (strt > 0)
            {
                strt--;
                if (m_format[strt] == m_inpChar)
                    return strt;
            }
            return ret;
        }
        private int Next(int startPos)
        {
            // return next input char position
            // returns current position if no input chars to the left
            int strt = startPos;
            int ret = strt;

            while (strt < base.MaxLength - 1)
            {
                strt++;
                if (m_format[strt] == m_inpChar)
                    return strt;
            }

            return ret;
        }
        private void BuildFormat()
        {
            // this builds the m_format string based on current regional settings
            //	EN example "M/d/yyyy" with default input char '_' produces "__/__/____"

            m_format = "";
            m_convert = "";
            string pat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
            string sep = DateTimeFormatInfo.CurrentInfo.DateSeparator;

            int pos = 0;
            string match = pat[pos].ToString();
            int cont = 0;
            while (cont < 3)
            {
                switch (match)
                {
                    case "M":
                        m_format += m_inpChar.ToString() + m_inpChar.ToString() + sep;
                        m_convert += "MM" + sep;
                        break;
                    case "d":
                        m_format += m_inpChar.ToString() + m_inpChar.ToString() + sep;
                        m_convert += "dd" + sep;
                        break;
                    case "y":
                        m_format += m_inpChar.ToString() + m_inpChar.ToString() + m_inpChar.ToString() + m_inpChar.ToString() + sep;
                        m_convert += "yyyy" + sep;
                        break;
                    default:
                        break;
                }

                // move to next Mdy char
                pos = pat.IndexOf(sep, pos) + 1;
                match = pat[pos].ToString();
                cont++;
            }

            // strip final seperator
            m_format = m_format.Substring(0, m_format.Length - 1);
            m_convert = m_convert.Substring(0, m_convert.Length - 1);
        }
        private void BuildPosNdx()
        {
            // used to build position translation map from mask string
            //	and input format
            string s = m_format;

            // reset index
            if (m_posNdx == null)
                m_posNdx = new Dictionary<int, int>();
            else
                m_posNdx.Clear();

            int cnt = 0;

            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == m_inpChar)
                    m_posNdx.Add(cnt, i);

                cnt++;
            }
        }
        #endregion private

        #region Calendar
        protected class Calendar : Form
        {
            private System.Windows.Forms.MonthCalendar mCalendar;
            private DateEdit dateEdit;
            private bool dataChanged;

            public Calendar(DateEdit dEdit)
            {
                dateEdit = dEdit;
                InitializeComponent();
            }
            private void InitializeComponent()
            {
                this.mCalendar = new System.Windows.Forms.MonthCalendar();
                this.SuspendLayout();
                // 
                // monthCalendar1
                // 
                this.mCalendar.Dock = System.Windows.Forms.DockStyle.Fill;
                this.mCalendar.Location = new System.Drawing.Point(0, 0);
                this.mCalendar.Name = "monthCalendar1";
                this.mCalendar.TabIndex = 0;
                this.mCalendar.ShowWeekNumbers = false;
                this.mCalendar.DateSelected += new DateRangeEventHandler((sender, e) =>
                {
                    dataChanged = true;
                    dateEdit.Value = e.Start;
                });
                this.mCalendar.MouseUp += new MouseEventHandler((sender, e) =>
                {
                    if (this.dataChanged) this.Close();
                });
                // 
                // Form
                // 
                this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
                this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.ClientSize = new System.Drawing.Size(159, 159);
                this.ControlBox = false;
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
                this.KeyPreview = true;
                this.ShowInTaskbar = false;
                this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
                this.Name = "Calendar";
                this.BackColor = mCalendar.BackColor;

                this.Controls.Add(this.mCalendar);
                this.ResumeLayout(false);
            }
            public MonthCalendar MonthCalendar
            { get { return mCalendar; } }
            protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
            {
                switch (keyData)
                {
                    case Keys.Escape:
                        this.Close();
                        return true;
                    default:
                        return base.ProcessCmdKey(ref msg, keyData);
                }
            }
            protected override void OnLeave(EventArgs e)
            {
                this.Close();
            }
            protected override void OnDeactivate(EventArgs e)
            {
                this.Close();
            }
        }
        #endregion Calendar
    }
}
