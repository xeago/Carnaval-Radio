using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using BlogEngine.Core;
using ExtensionMethods;

public partial class StandardSite : System.Web.UI.MasterPage
{
  protected void Page_Load(object sender, EventArgs e)
  {
      litHeaderImages.Text = getHeaderImages();
  }

  private int i = 1;
  private string getHeaderImages()
  {
      //       <img src="<%=Utils.AbsoluteWebRoot %>Upload/Headers/1.jpg" />
      //<img src="<%=Utils.AbsoluteWebRoot %>Upload/Headers/2.jpg" />
      var sb = new StringBuilder();
      foreach (string s in Directory.GetFiles(Server.MapPath("./Upload/Headers/")))
      {
          var f = new FileInfo(s);
          if (f.Extension.Contains("jpg") || f.Extension.Contains("jpeg") || f.Extension.Contains("png"))
          {
              sb.AppendFormat(
                  "<img src=\"{0}{1}{2}\" alt=\"slide {3}\" width=\"940\" height=\"289\" />",
                  Utils.AbsoluteWebRoot, "Upload/Headers/", f.Name, i);
              i++;
          }
      }

      return sb.ToString();
  }
}
