using System.Web;
using System.Web.UI;

namespace Guestbook
{
    public sealed class Alert
    {
        public static void Show(string psMessage)
        {
            // Cleans the message to allow single quotation marks 
            string lsCleanMessage = psMessage.Replace("'", "'");
            string lsScript = "<script type=\"text/javascript\">alert('" + lsCleanMessage + "');</script>";

            // Gets the executing web page 
            var loPage = HttpContext.Current.CurrentHandler as Page;

            // Checks if the handler is a Page and that the script isn't allready on the Page 
            if (loPage != null && !loPage.ClientScript.IsClientScriptBlockRegistered("alert"))
            {
                loPage.ClientScript.RegisterClientScriptBlock(typeof (Alert), "alert", lsScript);
            }
        }
    }
}