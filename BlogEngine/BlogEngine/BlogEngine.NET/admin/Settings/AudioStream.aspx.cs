using System.Text;
using BlogEngine.Core.Web.Extensions;
using Google.GData.Photos;

namespace admin.Settings
{
    using System;
    using Resources;
    using App_Code;
    using Page = System.Web.UI.Page;

    public partial class Picasa : Page
    {
        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event to initialize the page.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            WebUtils.CheckRightsForAdminSettingsPage(false);

            Page.MaintainScrollPositionOnPostBack = true;
            Page.Title = labels.settings;

            base.OnInit(e);
        }

        static protected ExtensionSettings Settings;

        protected void Page_Load(object sender, EventArgs e)
        {
            // load settings
            Settings = ExtensionManager.GetSettings("AudioStream");

            if (!Page.IsPostBack)
            {
                
                TxbHighStream.Text = Settings.GetSingleValue("HighStream");
                TxbLowStream.Text = Settings.GetSingleValue("LowStream");
                BindForm();
            }
        }

        protected void BtnSaveClick(object sender, EventArgs e)
        {
            if (Settings != null)
            {
                Settings.UpdateScalarValue("HighStream", TxbHighStream.Text);
                Settings.UpdateScalarValue("LowStream", TxbLowStream.Text);

                ExtensionManager.SaveSettings("AudioStream", Settings);
            }
            Response.Redirect(Request.RawUrl);
        }

        protected void BindForm()
        {
            btnSave.Text = labels.saveSettings;
        }

    }
}