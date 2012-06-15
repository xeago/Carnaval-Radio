#region Using

using System;
using System.Collections;
using System.Drawing;
using System.Text;
using System.Web;
using BlogEngine.Core.Web.Controls;
using Guestbook;

#endregion

public partial class guestbook : BlogBasePage
{

    public string ciMsgPerPage = "10";
    public int ciPage;
    public string csPath = HttpContext.Current.Server.MapPath(".");
    private int liStart;
    private int liEnd;
    private int i;
    private ArrayList laFiles;
    private string lsError;
    private gbSerialize coSerialize;
    private string lsPath;

    private readonly string _csConfigPath = HttpContext.Current.Server.MapPath(".") + "\\App_Data\\Guestbook\\config";
    private readonly string _csPath = HttpContext.Current.Server.MapPath(".") + "\\App_Data\\Guestbook";

    protected void Page_Load(object sender, EventArgs e)
    {
        ciPage = GSDlib.Utils.NullableInt(Request.QueryString["page"]) ?? 1;

        LoadMessages();

        lblNavigation.Text = GenerateNavigation(laFiles.Count, ciPage, "Gastenboek", Convert.ToInt16(ciMsgPerPage));
    }

    private void LoadMessages()
    {
        laFiles = new ArrayList();
        lsError = String.Empty;
        i = 0;
        liStart = 0;
        liEnd = 0;
        coSerialize = new gbSerialize(csPath + "\\App_Data\\Guestbook");
        lsPath = Request.ApplicationPath;

        laFiles = coSerialize.GetFileNames();
        Utility.GetStartAndEnd(ref liStart, ref liEnd, laFiles.Count, ciPage, Convert.ToInt16(ciMsgPerPage));

        StringBuilder messages = new StringBuilder();
        for (i = liStart; i > liEnd; i--)
        {
            var loMessage = coSerialize.DeserializeMessage(laFiles[i - 1].ToString(), ref lsError);
            messages.Append(coSerialize.DisplayMessage(loMessage, lsPath, csPath));
        }
        MsgDisplay.Text = messages.ToString();
    }

    private string GetVisitorIP()
    {
        var lsIpAddress = string.Empty;

        lsIpAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        if (lsIpAddress == string.Empty) lsIpAddress = Request.ServerVariables["REMOTE_ADDR"];

        return lsIpAddress;
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        var loValidation = new gbValidation();

        if (!GSDlib.Utils.IsValidMailAddress(youremail.Text))
        {
            lblResponse.ForeColor = Color.Red;
            lblResponse.Text = Resources.labels.emailIsInvalid;
            lblVerifyCode.ForeColor = Color.Red;
            lblVerifyCode.ToolTip = Resources.labels.emailIsInvalid;
            return;
        }

        if (loValidation.IsSpam(yourmessage.Text, _csConfigPath, GetVisitorIP()))
        {
            lblResponse.ForeColor = Color.Red;
            lblResponse.Text = Resources.labels.possibleSpam;
            return;
        }

        var lsSerialize = new gbSerialize(_csPath);
        int liId = lsSerialize.GetNextId() + 1;
        var loMessage = new gbMessage();
        var loUbbCode = new UBBCode(yourmessage.Text);

        var loCleanName = gbValidation.CleanString(yourname.Text);
        var loCleanEmail = gbValidation.CleanString(youremail.Text);

        
        loMessage.ID = liId;
        loMessage.SubmitDate = string.Format("{0} {1}", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString());
        loMessage.Name = loCleanName;
        loMessage.Email = loCleanEmail;
        loMessage.Message = loUbbCode.HTMLCode;
        loMessage.ResponseToMessage = new gbMessage() { ID = 1 };
        
        lsSerialize.SerializeMessage(loMessage);
        LoadMessages();

        yourname.Text = "";
        youremail.Text = "";
        yourmessage.Text = "";
    }

    public string GenerateNavigation(int liTotalCount, int liPage, string psPageName,
                                        int piMsgPerPage)
    {
        StringBuilder lsNavigation = new StringBuilder();
        decimal ldRatio = liTotalCount / Convert.ToDecimal(piMsgPerPage);
        int i = 0;

        if (ldRatio < 1)
        {
            lsNavigation.Append("<a href=\"Gastenboek.aspx\">1</a>");
        }
        else
        {
            for (i = 1; i <= Utility.RoundUp(ldRatio); i++)
            {
                lsNavigation.AppendFormat("<a href=\"{0}.aspx?page={1}\">{2}</a> ", psPageName, i, i);
            }
        }
        return lsNavigation.ToString();
    }
}