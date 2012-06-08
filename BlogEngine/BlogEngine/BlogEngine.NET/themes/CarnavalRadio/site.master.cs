using System;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using BlogEngine.Core;

public partial class StandardSite : System.Web.UI.MasterPage
{
  protected void Page_Load(object sender, EventArgs e)
  {
		if (Security.IsAuthenticated)
		{
            aUser.InnerText = "Welcome " + Page.User.Identity.Name + "!";
			aLogin.InnerText = Resources.labels.logoff;
			aLogin.HRef = Utils.RelativeWebRoot + "Account/login.aspx?logoff";
		}
		else
		{
			aLogin.HRef = Utils.RelativeWebRoot + "Account/login.aspx";
			aLogin.InnerText = Resources.labels.login;
		}

      litMenu.Text = buildMenu("");

  }

    private string buildMenu(string currentPage)
    {
        StringBuilder menu = new StringBuilder();

        foreach (var parentPage in BlogEngine.Core.Page.Pages.Where(p => !p.HasParentPage))
        {
            menu.AppendFormat("<li class=\"page_item\"><a href=\"{0}\" title=\"{1}\">{1}</a>", parentPage.RelativeLink, parentPage.Title);

            if (parentPage.HasChildPages)
            {
                menu.Append("<ul class=\"sub-menu\">");
                foreach (
                    var childPage in
                        BlogEngine.Core.Page.Pages.Where(p => p.Parent == parentPage.Id))
                {
                    menu.AppendFormat(
                        "<li class=\"page_item\"><a href=\"{0}\" title=\"{1}\">{1}</a></li>",
                        childPage.RelativeLink, childPage.Title);
                }
                menu.AppendFormat("</ul>");
            }

            menu.Append("</li>");
        }

        return menu.ToString();
    }

}
