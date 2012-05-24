using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

public partial class site : System.Web.UI.MasterPage
{
  protected void Page_Load(object sender, EventArgs e)
  {
		if (Page.User.Identity.IsAuthenticated)
		{
			aLogin.InnerText = Resources.labels.logoff;
			aLogin.HRef = BlogEngine.Core.Utils.RelativeWebRoot + "login.aspx?logoff";
		}
		else
		{
			aLogin.HRef = BlogEngine.Core.Utils.RelativeWebRoot + "login.aspx";
			aLogin.InnerText = Resources.labels.login;
		}
  }

  protected string is_current_page_item(string test_current)
  { 
    if(Request.Url.ToString().Equals(test_current,StringComparison.OrdinalIgnoreCase))
        return "current_page_item";
    else
        return string.Empty;
  
  }

  protected string GetColoredHeading(string title)
  {
      string[] splitTitle;
      splitTitle = title.Split(' ');

      if (splitTitle.Length == 1)
          return title;
      else
      {
          string result = splitTitle[0];
          int iCounter = 0;
          for (int i = 1; i < splitTitle.Length; i++)
          {
              iCounter++;
              switch (iCounter)
              {
                  case 1:
                      result += string.Format(@"<span class=""green"">{0}</span>", splitTitle[i]);
                      break;
                  case 3:
                      result += string.Format(@"<span class=""green"">{0}</span>", splitTitle[i]);
                      iCounter = 0;
                      break;
                  default:
                      result += splitTitle[i];
                      break;
              }
          }

          return result;
      }

  }
}
