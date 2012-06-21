namespace App_Code
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;
    using System.Web;
    using System.Web.Services;
    using System.Web.Script.Services;

    using BlogEngine.Core;
    using BlogEngine.Core.Json;

    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    public class postShout : WebService
    {
        string xmlfile = HttpContext.Current.Server.MapPath(@"./shouts.xml");

        [WebMethod]
        public JsonResponse SubmitMessage(string name, string message)
        {
            try
            {
                XDocument xDoc = XDocument.Load(xmlfile);

                int count = xDoc.Root.Elements().Count() + 1;
                xDoc.Element("shouts").Add(new XElement("shout",
                                                new XAttribute("id", count),
                                                new XElement("name", name),
                                                new XElement("message", message)));
                xDoc.Save(xmlfile);

                if (xDoc.Root.Elements().Count() > 30)
                {
                    xDoc.Root.FirstNode.Remove();
                    xDoc.Save(xmlfile);
                }

                return new JsonResponse() { Success = true };
            }
            catch (Exception e)
            {
                return new JsonResponse() { Success = false };
            }
        }

        [WebMethod]
        public JsonResponse DeleteMessage(int id)
        {
            try
            {
                XDocument xDoc = XDocument.Load(xmlfile);
                xDoc.Root.Elements().Where(x => x.Attribute("id").Value == id.ToString()).Remove();

                xDoc.Save(xmlfile);

                return new JsonResponse() { Success = true };
            }
            catch (Exception e)
            {
                return new JsonResponse() { Success = false };
            }
        }
    }
}