using System.IO;
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
                string path = Server.MapPath(@"~/AudioStreams/");
                writeOut(TxbHighStream.Text, TxbLowStream.Text, path);
            }
            Response.Redirect(Request.RawUrl);
        }

        #region Twan & Pascal stream creator
        public static void writeOut(string high, string low, string path)
        {
            if (!high.ToLower().StartsWith(@"http:")) high = "http://" + high;
            if (!low.ToLower().StartsWith(@"http:")) low = "http://" + low;

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            writePls(high, path, "Carnaval-Radio");
            writePls(low, path, "Carnaval-Radio.low");

            writeAsx(high, path, "Carnaval-Radio");
            writeAsx(low, path, "Carnaval-Radio.low");

            writeJson(high, low, path, "Carnaval-Radio");
        }

        private static void writeJson(string highserver, string lowserver, string path, string file)
        {
            file += ".json";
            using (StreamWriter sw = new StreamWriter(path + file))
            {
                sw.WriteLine("{\"high\":\"" + highserver + "\",\"low\":\"" + lowserver + "\"}");
            }
        }

        private static void writeAsx(string server, string path, string file)
        {
            file += ".asx";
            using (StreamWriter sw = new StreamWriter(path + file))
            {
                sw.WriteLine("<asx version = \"3.0\">" +
                                @"<Title>Carnaval-Radio.nl 2012</Title>" +
                                @"<Author>http://www.carnaval-radio.nl</Author>" +
                                "<MoreInfo href=\"http:////www.carnaval-radio.nl\" />" +
                                "<entry>" +
                                    "<ref href=\"http:////" + server + "\" />" +
                                    "<Title>Carnaval-Radio.nl 2012</Title> " +
                                    @"<Author>http://www.carnaval-radio.nl</Author>" +
                                "</entry>" +
                                "</asx>");

            }
        }

        private static void writePls(string server, string path, string file)
        {
            file += ".pls";
            using (StreamWriter sw = new StreamWriter(path + file))
            {
                sw.WriteLine("[playlist]");
                sw.WriteLine("NumberOfEntries=1");
                sw.WriteLine("File1=" + server);
            }

        }
        #endregion

        protected void BindForm()
        {
            btnSave.Text = labels.saveSettings;
        }

    }
}