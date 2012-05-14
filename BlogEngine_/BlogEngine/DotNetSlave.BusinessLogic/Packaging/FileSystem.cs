using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Web;
using BlogEngine.Core.Json;

namespace BlogEngine.Core.Packaging
{
    /// <summary>
    /// Class for packaging IO
    /// </summary>
    public class FileSystem
    {
        /// <summary>
        /// Copy package files
        /// </summary>
        /// <param name="pkgId">Package Id</param>
        /// <param name="version">Package Version</param>
        public static void CopyPackageFiles(string pkgId, string version)
        {
            //TODO: implement also for extensions and widgets, add "lib" handling
            var src = HttpContext.Current.Server.MapPath(Utils.ApplicationRelativeWebRoot +
                string.Format("App_Data/packages/{0}.{1}/content", pkgId, version));

            var tgt = HttpContext.Current.Server.MapPath(Utils.ApplicationRelativeWebRoot);

            var source = new DirectoryInfo(src);
            var target = new DirectoryInfo(tgt);

            Utils.CopyDirectoryContents(source, target);
        }

        /// <summary>
        /// Remove package files
        /// </summary>
        /// <param name="pkgId">Package Id</param>
        /// <param name="version">Package Version</param>
        public static void RemovePackageFiles(string pkgId, string version)
        {
            //TODO: implement also for extensions and widgets
            var pkg = HttpContext.Current.Server.MapPath(Utils.ApplicationRelativeWebRoot +
                string.Format("themes/{0}", pkgId));

            if (Directory.Exists(pkg))
                ForceDeleteDirectory(pkg); // Directory.Delete(pkg, true);

            // remove package itself
            pkg = HttpContext.Current.Server.MapPath(Utils.ApplicationRelativeWebRoot +
                string.Format("App_Data/packages/{0}.{1}", pkgId, version));

            if (Directory.Exists(pkg))
                ForceDeleteDirectory(pkg); // Directory.Delete(pkg, true);
        }

        /// <summary>
        /// get theme manifest from xml file
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static JsonPackage GetThemeManifest(string id)
        {
            var jp = new JsonPackage { Id = id };
            var themeUrl = string.Format("{0}themes/{1}/theme.xml", Utils.ApplicationRelativeWebRoot, id);
            var themePath = HttpContext.Current.Server.MapPath(themeUrl);
            try
            {
                if(File.Exists(themePath))
                {
                    var textReader = new XmlTextReader(themePath);
                    textReader.Read();

                    while (textReader.Read())
                    {
                        textReader.MoveToElement();
                        
                        if (textReader.Name == "description")
                            jp.Description = textReader.ReadString();

                        if (textReader.Name == "authors")
                            jp.Authors = textReader.ReadString();

                        if (textReader.Name == "website")
                            jp.Website = textReader.ReadString();

                        if (textReader.Name == "version")
                            jp.Version = textReader.ReadString();

                        if (textReader.Name == "iconurl")
                            jp.IconUrl = textReader.ReadString();
                    }
                    return jp;
                }
            }
            catch (Exception ex)
            {
                Utils.Log("Packaging.FileSystem.GetThemeManifest", ex);
            }
            return null;
        }

        /// <summary>
        /// Write theme manifest to xml file
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="description">description</param>
        /// <param name="authors">authors</param>
        /// <param name="url">project url</param>
        /// <returns></returns>
        public static JsonPackage WriteThemeManifest(string id, string description = "", string authors = "", string url = "", string version = "", string iconUrl = "")
        {
            if (string.IsNullOrEmpty(authors)) authors = "Unknown";

            var jp = new JsonPackage {Id = id, Description = description, Authors = authors, Website = url};
            var themeUrl = string.Format("{0}themes/{1}/theme.xml", Utils.ApplicationRelativeWebRoot, id);
            var themePath = HttpContext.Current.Server.MapPath(themeUrl);
            try
            {
                var textWriter = new XmlTextWriter(themePath, null) 
                    {Formatting = Formatting.Indented, Indentation = 4};

                textWriter.WriteStartDocument();
                textWriter.WriteStartElement("metadata");

                textWriter.WriteElementString("id", id);
                textWriter.WriteElementString("description", description);
                textWriter.WriteElementString("authors", authors);
                textWriter.WriteElementString("website", url);
                textWriter.WriteElementString("version", version);

                #region Thumbnail

                var thumbnail = Utils.ApplicationRelativeWebRoot + "pics/Theme.png";
                var customPng = string.Format("{0}themes/{1}/theme.png", Utils.ApplicationRelativeWebRoot, id);

                if (File.Exists(HttpContext.Current.Server.MapPath(customPng)))
                    thumbnail = customPng;

                if (!string.IsNullOrEmpty(iconUrl))
                    thumbnail = iconUrl;

                textWriter.WriteElementString("iconurl", thumbnail);
                jp.IconUrl = thumbnail;

                #endregion

                textWriter.WriteEndDocument();
                textWriter.Close();
            }
            catch (Exception ex)
            {
                Utils.Log("Packaging.FileSystem.WriteThemeManifest", ex);
                return null;
            }

            return jp;
        }

        static void ForceDeleteDirectory(string path)
        {
            DirectoryInfo fol;
            var fols = new Stack<DirectoryInfo>();
            var root = new DirectoryInfo(path);
            fols.Push(root);
            while (fols.Count > 0)
            {
                fol = fols.Pop();
                fol.Attributes = fol.Attributes & ~(FileAttributes.Archive | FileAttributes.ReadOnly | FileAttributes.Hidden);
                foreach (DirectoryInfo d in fol.GetDirectories())
                {
                    fols.Push(d);
                }
                foreach (FileInfo f in fol.GetFiles())
                {
                    f.Attributes = f.Attributes & ~(FileAttributes.Archive | FileAttributes.ReadOnly | FileAttributes.Hidden);
                    f.Delete();
                }
            }
            root.Delete(true);
        }
    }
}
