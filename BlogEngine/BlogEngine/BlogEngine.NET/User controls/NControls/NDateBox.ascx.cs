using System;
using System.Web.Script.Serialization;
using System.Web.UI;
using GSDlib;

namespace NControls
{
    public partial class NDateBox : UserControl
    {
        public NDateBox()
        {
            enabled = true;
            enableDispose = false;
            displayCalendar = true;
            displayWeekColumn = true;
            timeSpan = new TimeSpan();
        }

        private bool enabled;
        public bool Enabled
        {
            get
            {
                return enabled;
            }
            set
            {
                enabled = value;
                TextBoxDate.Enabled = value;
            }
        }

        private bool displayCalendar;
        public bool DisplayCalendar
        {
            get { return displayCalendar; }
            set { displayCalendar = value; }
        }

        private bool displayWeekColumn;
        public bool DisplayWeekColumn
        {
            get { return displayWeekColumn; }
            set { displayWeekColumn = value; }
        }

        private bool enableDispose;
        public bool EnableDispose
        {
            get
            {
                return enableDispose;
            }
            set
            {
                enableDispose = value;
                imgDispose.Visible = value;
            }
        }

        

        public void SetDate()
        {
            SetDate(DateTime.Now);
        }
        public void SetDate(DateTime? dt)
        {
            this.Date = dt;
        }

        public bool IsValidDate
        {
            get
            { return this.Date.HasValue; }
        }

        private TimeSpan timeSpan;
        public TimeSpan TimeSpan
        {
            get { return timeSpan; }
            set { timeSpan = value; }
        }

        public DateTime? Date
        {
            get
            {
                DateTime? dt = Utils.NullableDateTime(TextBoxDate.Text);
                return dt.HasValue ? dt.Value.Add(this.timeSpan) : dt;
            }
            set
            {
                if (value.HasValue)
                { TextBoxDate.Text = string.Format("{0:dd-MM-yyyy}", value); }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string GSDateBoxOptions = jss.Serialize(new
            {
                disabled = !enabled,
                showOn = (enabled ? (displayCalendar ? "button" : "focus") : null),
                buttonImage = (enabled ? (displayCalendar ?  "~/Admin/images/datepicker/calendar.png" : null) : null),
                buttonImageOnly = (enabled ? displayCalendar : false),
                showWeek = displayWeekColumn,
                changeMonth = true,
                changeYear = true,
                yearRange = "c-120:c+10",
                constrainInput = true
            });

            //string css = "<link href=\"" + Page.ClientScript.GetWebResourceUrl(this.GetType(),
            //    "Datebox.css") + "\" type=\"text/css\" rel=\"stylesheet\" />";

            //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "cssFile", css, false);

            this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "jsEnable" + this.UniqueID, string.Format(
                "$(document).ready(function() {{ $('#{0}').datepicker({1}); }} );",
                TextBoxDate.ClientID,
                GSDateBoxOptions),
            true);

            if (enabled && enableDispose)
            { imgDispose.Attributes["onclick"] = string.Format("$('#{0}').val('');", TextBoxDate.ClientID); }
            else
            { imgDispose.Visible = false; }
        }
    }
}
