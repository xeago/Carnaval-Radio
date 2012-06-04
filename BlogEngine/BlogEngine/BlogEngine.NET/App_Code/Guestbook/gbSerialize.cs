using System;
using System.Collections;
using System.Configuration;
using System.IO;
using System.Xml.Serialization;

namespace Guestbook
{
    public class gbSerialize
    {
        private readonly string csPath;

        public gbSerialize(string psPath)
        {
            csPath = psPath;
        }

        public ArrayList SortArrayList(ArrayList paListUnsorted)
        {
            var loSortedArrayList = new ArrayList();
            var loSortedArrayListString = new ArrayList();
            var laArray = new ArrayList(2);
            string lsExtension = ".xml";
            int i = 0;

            for (i = 0; i <= paListUnsorted.Count - 1; i++)
            {
                paListUnsorted[i] = paListUnsorted[i].ToString().Replace(lsExtension, "");
                if (Utility.IsNumeric(paListUnsorted[i]))
                {
                    loSortedArrayList.Add(Convert.ToInt32(paListUnsorted[i]));
                }
            }

            loSortedArrayList.Sort(new Comparer());

            for (i = 0; i <= loSortedArrayList.Count - 1; i++)
            {
                loSortedArrayListString.Add(loSortedArrayList[i] + ".xml");
            }

            return loSortedArrayListString;
        }

        public ArrayList GetFileNames()
        {
            var loMessages = new DirectoryInfo(csPath);
            FileInfo[] loMessage = loMessages.GetFiles("*.xml");
            var loFileNames = new ArrayList();
            int i = 0;

            for (i = 0; i <= loMessage.Length - 1; i++)
            {
                loFileNames.Add(loMessage[i].Name);
            }
            return SortArrayList(loFileNames);
        }

        public int GetNextId()
        {
            var laList = new ArrayList();
            string lsFile = String.Empty;
            int liId = 0;
            laList = GetFileNames();

            foreach (string lsFile2 in laList)
            {
                lsFile = lsFile2.Replace(".xml", "");
                if (Convert.ToInt32(lsFile) > liId)
                {
                    liId = Convert.ToInt32(lsFile);
                }
            }
            return liId;
        }

        public string SerializeMessage(gbMessage loMessage)
        {
            string loResponse = "";
            try
            {
                var loMessageSerialize = new XmlSerializer(typeof(gbMessage));
                var loWriteStream = new StreamWriter(csPath + "\\" + loMessage.ID + ".xml");

                loMessageSerialize.Serialize(loWriteStream, loMessage);
                loResponse = "Message Stored Successfully!";
                loWriteStream.Close();
            }
            catch (Exception ex)
            {
                loResponse = "Error: " + ex.Message;
            }
            return loResponse;
        }

        public gbMessage DeserializeMessage(string lsId, ref string lsError)
        {
            string loResponse = "";
            var gbMessage = new gbMessage();
            try
            {
                var loMessage = new XmlSerializer(typeof(gbMessage));
                var loStreamReader = new StreamReader(csPath + "\\" + lsId);
                gbMessage = (gbMessage)loMessage.Deserialize(loStreamReader);
                loResponse = "Message Retrieved Successfully!";
                loStreamReader.Close();
            }
            catch (Exception ex)
            {
                loResponse = "Error: " + ex.Message;
            }
            return gbMessage;
        }

        public string DisplayMessage(gbMessage loMessage, string lsPath, string csPath)
        {
            string lsTemplate = "";
            string lsError = String.Empty;
            lsTemplate = GetFileContents(csPath + "\\Templates\\Guestbook\\message_box.html", lsError);

            lsTemplate = lsTemplate.Replace("{TPLPATH}", lsPath + "/Templates/Guestbook"); // + "/");
            lsTemplate = lsTemplate.Replace("{ID}", loMessage.ID.ToString());
            lsTemplate = lsTemplate.Replace("{SUBMITDATE}", loMessage.SubmitDate);
            lsTemplate = lsTemplate.Replace("{NAME}", loMessage.Name);
            lsTemplate = lsTemplate.Replace("{EMAIL}", loMessage.Email);
            lsTemplate = lsTemplate.Replace("{MESSAGE}", loMessage.Message);
            return lsTemplate;
        }

        public string GetFileContents(string FullPath, string ErrInfo)
        {
            string strContents = "";
            StreamReader objReader = default(StreamReader);
            try
            {
                objReader = new StreamReader(FullPath);
                strContents = objReader.ReadToEnd();
                objReader.Close();
            }
            catch (Exception Ex)
            {
                ErrInfo = Ex.Message;
            }
            return strContents;
        }

        public bool SaveTextToFile(string strData, string FullPath, string ErrInfo)
        {
            bool bAns = false;
            StreamWriter objReader = default(StreamWriter);

            try
            {
                objReader = new StreamWriter(FullPath);
                objReader.Write(strData);
                objReader.Close();
                bAns = true;
            }
            catch (Exception Ex)
            {
                ErrInfo = Ex.Message;
                bAns = false;
            }
            return bAns;
        }
    }
}