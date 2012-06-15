using System;

namespace Guestbook
{
    public class WordWrap
    {
        private string m_StringOriginal;
        private string m_StringSplit;

        public WordWrap()
        {
            m_StringOriginal = "";
            m_StringSplit = "";
        }

        public string Wrap(string lsString, int liMaxLength)
        {
            m_StringSplit = Wrap(lsString, liMaxLength, " ");
            return m_StringSplit;
        }

        public string Wrap(string lsString, int liMaxLength, string lsCharToAppend)
        {
            Array laString = null;
            m_StringOriginal = lsString;

            laString = lsString.Split(' ');

            return m_StringSplit;
        }
    }
}