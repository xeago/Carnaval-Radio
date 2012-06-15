using System;
using System.Text.RegularExpressions;
using System.Xml;

namespace Guestbook
{
    public class gbValidation
    {
        public bool IsSpam(string psMessage, string psConfigPath, string psIP)
        {
            return IsBadWord(psMessage, psConfigPath) || IsIPBanned(psIP, psConfigPath);
        }

        public bool IsBadWord(string lsMessage, string lsConfigPath)
        {
            bool lbReturn = false;
            var loXMLDoc = new XmlDocument();
            XmlNodeList loXMLNodes = default(XmlNodeList);

            loXMLDoc.Load(lsConfigPath + "\\BadWords.xml");

            loXMLNodes = loXMLDoc.GetElementsByTagName("word");
            foreach (XmlNode loXMLNode in loXMLNodes)
            {
                return lsMessage.IndexOf(loXMLNode.InnerText) >= 0;
            }
            return false;
        }

        public bool IsIPBanned(string lsIP, string lsConfigPath)
        {
            bool lbReturn = false;
            var loXMLDoc = new XmlDocument();
            XmlNodeList loXMLNodes = default(XmlNodeList);

            loXMLDoc.Load(lsConfigPath + "\\BannedIP.xml");

            if ((lsIP != null) && lsIP.Length > 0)
            {
                loXMLNodes = loXMLDoc.GetElementsByTagName("IP");
                foreach (XmlNode loXMLNode in loXMLNodes)
                {
                    if (lsIP == loXMLNode.InnerText)
                    {
                        lbReturn = true;
                        break; // TODO: might not be correct. Was : Exit For
                    }
                }
            }

            return lbReturn;
        }

        public static string CleanString(string psString)
        {
            string lsString = "";

            lsString = psString.Replace("&", "&amp;");
            lsString = lsString.Replace("<", "&lt;");
            lsString = lsString.Replace(">", "&gt;");
            lsString = lsString.Replace(" ", "&nbsp;");
            lsString = lsString.Replace(Environment.NewLine, "<br>");

            return lsString;
        }

        public void BanThisIP(string lsIP, string lsConfigPath)
        {
            // Add IP Address to BannedIP.xml
        }
    }
}