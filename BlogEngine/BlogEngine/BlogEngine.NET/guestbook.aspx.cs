#region Using

using System;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using BlogEngine.Core;
using BlogEngine.Core.Web.Controls;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Guestbook;

#endregion

public partial class guestbook : BlogBasePage
{

    public bool cbOrder;
    public string ciMsgPerPage = "10";
    public int ciPage;
    public string csPath = HttpContext.Current.Server.MapPath(".");

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ciPage = Convert.ToInt32(Request.QueryString["page"]);
        }
        catch (Exception ex)
        {
            ciPage = 1;
        }

        cbOrder = Request.QueryString["order"] == "desc";

        try
        {
            var laFiles = new ArrayList();
            string lsError = String.Empty;
            int i = 0;
            int liStart = 0;
            int liEnd = 0;
            var coSerialize = new gbSerialize(csPath + "\\App_Data\\Guestbook");
            string lsPath = Request.ApplicationPath;

            laFiles = coSerialize.GetFileNames();
            Utility.GetStartAndEnd(ref liStart, ref liEnd, laFiles.Count, ciPage, cbOrder, Convert.ToInt16(ciMsgPerPage));

            if (cbOrder)
            {
                for (i = liStart; i < liEnd; i++)
                {
                    var loMessage = new gbMessage();
                    loMessage = coSerialize.DeserializeMessage(laFiles[i].ToString(), ref lsError);
                    MsgDisplay.Text += coSerialize.DisplayMessage(loMessage, lsPath, csPath);
                }
            }
            else
            {
                for (i = liStart; i > liEnd; i--)
                {
                    var loMessage = new gbMessage();
                    loMessage = coSerialize.DeserializeMessage(laFiles[i - 1].ToString(), ref lsError);
                    MsgDisplay.Text += coSerialize.DisplayMessage(loMessage, lsPath, csPath);
                }
            }

            lblNavigation.Text = Utility.GenerateNavigation(laFiles.Count, ciPage, cbOrder, "List",
                                                            Convert.ToInt16(ciMsgPerPage));
        }
        catch (Exception ex)
        {
            Response.Write("<b>Error</b>: " + ex.Message);
        }
    }

	/// <summary>
	/// Handles the Click event of the btnSend control.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
	private void btnSend_Click(object sender, EventArgs e)
	{
	}

}