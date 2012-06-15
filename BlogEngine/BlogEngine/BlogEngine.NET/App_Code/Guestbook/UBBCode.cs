using System;
using System.Text.RegularExpressions;

namespace Guestbook
{
    public class UBBCode
    {
        private readonly string m_HTML_String;
        private string m_UBB_String;

        public UBBCode(string lsUBBString)
        {
            m_UBB_String = lsUBBString;
            m_HTML_String = ParseUBBCode(m_UBB_String);
        }

        public string HTMLCode
        {
            get { return m_HTML_String; }
        }

        private string CleanUBBString()
        {
            string lsUBBString = "";
            m_UBB_String = m_UBB_String.Replace("&", "&amp;");
            m_UBB_String = m_UBB_String.Replace("<", "&lt;");
            m_UBB_String = m_UBB_String.Replace(">", "&gt;");
            m_UBB_String = m_UBB_String.Replace(" ", "&nbsp;");
            m_UBB_String = m_UBB_String.Replace(Environment.NewLine, "<br>");
            lsUBBString = m_UBB_String;
            return lsUBBString;
        }

        private string ParseUBBCode(string loUBBString)
        {
            loUBBString = CleanUBBString();

            var loRegex = new Regex("(\\[IMG\\])(.[^\\[]*)(\\[\\/IMG\\])", RegexOptions.IgnoreCase);
            loUBBString = loRegex.Replace(loUBBString, "<a href=\"$2\" target=_blank><IMG SRC=\"$2\" border=0></a>");

            loRegex = new Regex("(\\[URL\\])(http:\\/\\/.[^\\[]*)(\\[\\/URL\\])", RegexOptions.IgnoreCase);
            loUBBString = loRegex.Replace(loUBBString, "<A HREF=\"$2\" TARGET=_blank>$2</A>");

            loRegex = new Regex("(\\[URL\\])(.[^\\[]*)(\\[\\/URL\\])", RegexOptions.IgnoreCase);
            loUBBString = loRegex.Replace(loUBBString, "<A HREF=\"http://$2\" TARGET=_blank>$2</A>");

            loRegex = new Regex("(\\[URL=(http:\\/\\/.[^\\[]*)\\])(.[^\\[]*)(\\[\\/URL\\])", RegexOptions.IgnoreCase);
            loUBBString = loRegex.Replace(loUBBString, "<A HREF=\"$2\" TARGET=_blank>$3</A>");

            loRegex = new Regex("(\\[URL=(.[^\\[]*)\\])(.[^\\[]*)(\\[\\/URL\\])", RegexOptions.IgnoreCase);
            loUBBString = loRegex.Replace(loUBBString, "<A HREF=\"http://$2\" TARGET=_blank>$3</A>");

            loRegex = new Regex("(\\[EMAIL\\])(\\S+\\@.[^\\[]*)(\\[\\/EMAIL\\])", RegexOptions.IgnoreCase);
            loUBBString = loRegex.Replace(loUBBString, "<A HREF=\"mailto:$2\">$2</A>");

            loRegex = new Regex("(\\[EMAIL=(\\S+\\@.[^\\[]*)\\])(.[^\\[]*)(\\[\\/EMAIL\\])", RegexOptions.IgnoreCase);
            loUBBString = loRegex.Replace(loUBBString, "<A HREF=\"mailto:$2\" TARGET=_blank>$3</A>");

            loRegex = new Regex("^(HTTP://[A-Za-z0-9\\./=\\?%\\-&_~`@':+!]+)", RegexOptions.IgnoreCase);
            loUBBString = loRegex.Replace(loUBBString, "<a target=_blank href=$1>$1</a>");

            loRegex = new Regex("(HTTP://[A-Za-z0-9\\./=\\?%\\-&_~`@':+!]+)$", RegexOptions.IgnoreCase);
            loUBBString = loRegex.Replace(loUBBString, "<a target=_blank href=$1>$1</a>");

            loRegex = new Regex("[^>=\"](HTTP://[A-Za-z0-9\\./=\\?%\\-&_~`@':+!]+)", RegexOptions.IgnoreCase);
            loUBBString = loRegex.Replace(loUBBString, "<a target=_blank href=$1>$1</a>");

            loRegex = new Regex("^(FTP://[A-Za-z0-9\\./=\\?%\\-&_~`@':+!]+)", RegexOptions.IgnoreCase);
            loUBBString = loRegex.Replace(loUBBString, "<a target=_blank href=$1>$1</a>");

            loRegex = new Regex("(FTP://[A-Za-z0-9\\./=\\?%\\-&_~`@':+!]+)$", RegexOptions.IgnoreCase);
            loUBBString = loRegex.Replace(loUBBString, "<a target=_blank href=$1>$1</a>");

            loRegex = new Regex("[^>=\"](FTP://[A-Za-z0-9\\.\\/=\\?%\\-&_~`@':+!]+)", RegexOptions.IgnoreCase);
            loUBBString = loRegex.Replace(loUBBString, "<a target=_blank href=$1>$1</a>");

            loRegex = new Regex("(\\[I\\])(.[^\\[]*)(\\[\\/I\\])", RegexOptions.IgnoreCase);
            loUBBString = loRegex.Replace(loUBBString, "<i>$2</i>");

            loRegex = new Regex("(\\[U\\])(.[^\\[]*)(\\[\\/U\\])", RegexOptions.IgnoreCase);
            loUBBString = loRegex.Replace(loUBBString, "<u>$2</u>");

            loRegex = new Regex("(\\[B\\])(.[^\\[]*)(\\[\\/B\\])", RegexOptions.IgnoreCase);
            loUBBString = loRegex.Replace(loUBBString, "<b>$2</b>");

            loRegex = new Regex("(\\[SIZE=1\\])(.[^\\[]*)(\\[\\/SIZE\\])", RegexOptions.IgnoreCase);
            loUBBString = loRegex.Replace(loUBBString, "<font size=1>$2</font>");

            loRegex = new Regex("(\\[SIZE=2\\])(.[^\\[]*)(\\[\\/SIZE\\])", RegexOptions.IgnoreCase);
            loUBBString = loRegex.Replace(loUBBString, "<font size=2>$2</font>");

            loRegex = new Regex("(\\[SIZE=3\\])(.[^\\[]*)(\\[\\/SIZE\\])", RegexOptions.IgnoreCase);
            loUBBString = loRegex.Replace(loUBBString, "<font size=3>$2</font>");

            loRegex = new Regex("(\\[SIZE=4\\])(.[^\\[]*)(\\[\\/SIZE\\])", RegexOptions.IgnoreCase);
            loUBBString = loRegex.Replace(loUBBString, "<font size=4>$2</font>");

            loRegex = new Regex("(\\[CENTER\\])(.[^\\[]*)(\\[\\/CENTER\\])", RegexOptions.IgnoreCase);
            loUBBString = loRegex.Replace(loUBBString, "<center>$2</center>");

            return loUBBString;
        }
    }
}