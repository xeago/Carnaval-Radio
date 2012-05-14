using System.Collections.Generic;
using System.Linq;
using BlogEngine.Core.Packaging;

namespace BlogEngine.Core.Json
{
    /// <summary>
    /// Json Packages
    /// </summary>
    public class JsonPackages
    {
        /// <summary>
        /// Package count
        /// </summary>
        public static int Count { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pkgType"></param>
        /// <param name="page"></param>
        /// <param name="sortOrder"></param>
        /// <param name="searchVal"></param>
        /// <returns></returns>
        public static List<JsonPackage> GetPage(string pkgType, int page = 1, PackageManager.OrderType sortOrder = PackageManager.OrderType.Newest, string searchVal = "")
        {
            var retPkgs = new List<JsonPackage>();

            var packages = PackageManager.GetPackages(pkgType, page, sortOrder, searchVal);

            if(packages == null)
            {
                return null;
            }

            Count = packages.Count();

            foreach (var p in packages)
            {
                var jp = new JsonPackage
                {
                    Id = p.Id,
                    PackageType = pkgType,
                    Authors = string.IsNullOrEmpty(p.Authors) ? "unknown" : p.Authors,
                    Description = p.Description,
                    DownloadCount = p.DownloadCount,
                    LastUpdated = p.LastUpdated.ToString("dd MMM yyyy"),
                    Title = p.Title,
                    Version = p.Version,
                    Website = p.ProjectUrl,
                    Tags = p.Tags,
                    IconUrl = p.IconUrl
                };

                if (!string.IsNullOrWhiteSpace(p.GalleryDetailsUrl))
                {
                    switch (p.PackageType)
                    {
                        case "Theme":
                            jp.PackageUrl = "http://dnbegallery.org/cms/List/Themes/" + p.Id;
                            break;
                        case "Extension":
                            jp.PackageUrl = "http://dnbegallery.org/cms/List/Extensions/" + p.Id;
                            break;
                        case "Widget":
                            jp.PackageUrl = "http://dnbegallery.org/cms/List/Widgets/" + p.Id;
                            break;
                    }
                }
                
                retPkgs.Add(jp);
            }

            return retPkgs;
        }
    }
}
