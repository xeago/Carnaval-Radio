// --------------------------------------------------------------------------------------------------------------------
// <summary>
//   The edit.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Widgets.Twitter
{
    using System;
    using System.Web;

    using App_Code.Controls;
    using BlogEngine.Core;

    /// <summary>
    /// The edit.
    /// </summary>
    public partial class Edit : WidgetEditBase
    {
        #region Constants and Fields

        /// <summary>
        /// The twitter settings cache key.
        /// </summary>
        private const string TwitterSettingsCacheKey = "twitter-settings"; // same key used in widget.ascx.cs.

        #endregion

        #region Public Methods

        /// <summary>
        /// Saves this the basic widget settings such as the Title.
        /// </summary>
        public override void Save()
        {
            var settings = this.GetSettings();
            settings["account"] = this.txtAccount.Text.Trim();
            settings["hashtags"] = this.txtHashtags.Text.Trim();
            settings["maxitems"] = this.txtTwits.Text.Trim();
            settings["pollinginterval"] = this.txtPolling.Text.Trim();
            settings["followmetext"] = this.txtFollowMe.Text.Trim();
            this.SaveSettings(settings);

            // Don't need to clear Feed out of cache because when the Settings are cleared,
            // the last modified date (i.e. TwitterSettings.LastModified) will reset to
            // DateTime.MinValue and Twitter will be re-queried.
            Blog.CurrentInstance.Cache.Remove(TwitterSettingsCacheKey);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            var settings = this.GetSettings();
            if (settings.ContainsKey("account"))
            {
                this.txtAccount.Text = settings["account"].Trim();
                this.txtHashtags.Text = settings["hashtags"].Trim();
                this.txtTwits.Text = settings["maxitems"].Trim();
                this.txtPolling.Text = settings["pollinginterval"].Trim();
                this.txtFollowMe.Text = settings["followmetext"].Trim();
            }

            base.OnInit(e);
        }

        #endregion
    }
}