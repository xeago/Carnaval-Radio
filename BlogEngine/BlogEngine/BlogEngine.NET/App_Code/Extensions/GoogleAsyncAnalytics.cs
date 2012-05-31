using System;
using BlogEngine.Core.Web.Controls;
using BlogEngine.Core;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web;
using BlogEngine.Core.Web.Extensions;
using System.Web.Security;  // Required for BE 2.x when using ExtensionSettings.

#region *** DISCLAIMER ***
/// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
/// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
/// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
/// PARTICULAR PURPOSE.
/// 
/// IN NO EVENT SHALL CHRIS BLANKENSHIP @ WWW.DSCODUC.COM 
/// OR WERNER VAN DEVENTER @ WWW.BRUTALDEV.COM BE
/// LIABLE FOR ANY SPECIAL, INDIRECT OR CONSEQUENTIAL DAMAGES OR ANY
/// DAMAGES WHATSOEVER RESULTING FROM LOSS OF USE, DATA OR PROFITS,
/// WHETHER IN AN ACTION OF CONTRACT, NEGLIGENCE OR OTHER TORTIOUS
/// ACTION, ARISING OUT OF OR IN CONNECTION WITH THE USE OR PERFORMANCE
/// OF THIS CODE OR INFORMATION.
#endregion

/// <summary>
/// Werner van Deventer
/// www.brutaldev.com
/// Improvements to Chris Blankenship's code (www.chrisbcode.com) to introduce an async option and fixed for BE 2.x.
/// 
/// Adds Google Analytics Tracking Code (with async option) to your website with options to disable tracking for logged in users.
/// </summary>
[Extension("Adds Google Analytics code to BlogEngine.NET.", "1.1", "<a target=\"_blank\" href=\"http://www.brutaldev.com\">Werner van Deventer</a>")]
public class GoogleAsyncAnalytics
{
  private const string ExtensionName = "GoogleAsyncAnalytics";
  static protected ExtensionSettings _settings = null;
  
  static protected string Key
  {
    get
    {
      return _settings.GetSingleValue("trackerID");
    }
  }

  static protected bool Async
  {
    get
    {
      if (!String.IsNullOrEmpty(_settings.GetSingleValue("isAsync")))
      {
        return Convert.ToBoolean(_settings.GetSingleValue("isAsync"));
      }
      return true;
    }
  }

  static protected bool DisableForLoggedInUsers
  {
    get
    {
      if (!String.IsNullOrEmpty(_settings.GetSingleValue("disableForLoggedInUsers")))
      {
        return Convert.ToBoolean(_settings.GetSingleValue("disableForLoggedInUsers"));
      }
      return false;
    }
  }

  public GoogleAsyncAnalytics()
  {
    BlogEngine.Core.Post.Serving += new EventHandler<ServingEventArgs>(PrepareGoogleAnalytics);
    BlogEngine.Core.Page.Serving += new EventHandler<ServingEventArgs>(PrepareGoogleAnalytics);

    ExtensionSettings settings = new ExtensionSettings(ExtensionName);
    settings.AddParameter("trackerID", "Google Tracking Code", 20, true);
    settings.AddParameter("isAsync", "Use new asynchronous script");
    settings.AddParameter("disableForLoggedInUsers", "Disable when users are logged in");

    // Load generic default value to illustrate the format
    settings.AddValue("trackerID", "UA-XXXXXXX-Y");
    settings.AddValue("isAsync", true);
    settings.AddValue("disableForLoggedInUsers", false);

    settings.Help = "<br />Add Google Analytics to your website.<br />Generate a code for your site by going to https://www.google.com/analytics.<br /><br />The script will get generated at the top of the page, it is recommended that you use the asynchronous script, if you experience problems switch to the synchronous version. Using the synchronous script may delay start-up time for each page as it waits for a response from Google.<br /><br />Disable tracking for logged on users if only admins are logging in and updating posts and/or comments. This allows for more accurate tracking of real site usage.";
    settings.IsScalar = true;
    
    _settings = ExtensionManager.InitSettings(ExtensionName, settings);
  }

  private void PrepareGoogleAnalytics(object sender, ServingEventArgs e)
  {
    if (e.Location == ServingLocation.Feed) return;

    // Check if the user is logged and and whether we should disable the tracking script.
    if (DisableForLoggedInUsers && Membership.GetUser() != null) return;    
    
    HttpContext context = HttpContext.Current;
    if (context.CurrentHandler is System.Web.UI.Page)
    {
      // Make sure we haven't already added the script to the page header.
      if (context.Items[ExtensionName] == null)
      {
        // prep page handler
        var page = (System.Web.UI.Page)context.CurrentHandler;

        // Add Google Tracker
        var trackerScript = new StringBuilder();

        trackerScript.AppendLine("");
        trackerScript.AppendLine("<!-- Begin Google scripts -->");
        trackerScript.AppendLine("<script type=\"text/javascript\">");

        if (!Async)
        {
          // Normal tracker code.
          trackerScript.AppendLine("<script type=\"text/javascript\">");
          trackerScript.AppendLine("var gaJsHost = ((\"https:\" == document.location.protocol) ? \"https://ssl.\" : \"http://www.\");");
          trackerScript.AppendLine("document.write(unescape(\"%3Cscript src='\" + gaJsHost + \"google-analytics.com/ga.js' type='text/javascript'%3E%3C/script%3E\"));");
          trackerScript.AppendLine("</script>");
          
          trackerScript.AppendLine("if (typeof(_gat) == \"object\")");
          trackerScript.AppendLine("{");
          trackerScript.AppendLine("var pageTracker = _gat._getTracker(\"" + Key + "\");");
          trackerScript.AppendLine("pageTracker._initData();");
          trackerScript.AppendLine("pageTracker._trackPageview();");
          trackerScript.AppendLine("}");
        }
        else
        {
          // Async tracker code.
          trackerScript.AppendLine("var _gaq = _gaq || [];");
          trackerScript.AppendLine("_gaq.push(['_setAccount', '" + Key + "']);");
          trackerScript.AppendLine("_gaq.push(['_trackPageview']);");

          trackerScript.AppendLine("(function() {");
          trackerScript.AppendLine("  var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;");
          trackerScript.AppendLine("  ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';");
          trackerScript.AppendLine("  var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);");
          trackerScript.AppendLine("})();");
        }
        
        trackerScript.AppendLine("</script>");
        trackerScript.AppendLine("<!-- End Google scripts -->");
        trackerScript.AppendLine("");

        // finish up                
        page.ClientScript.RegisterStartupScript(page.GetType(), "googleAnalyticsScripts", trackerScript.ToString(), false);
        context.Items[ExtensionName] = 1;
      }
    }
  }
}
