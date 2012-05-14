using System;
using System.IO;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using BlogEngine.Core.GalleryServer;
using BlogEngine.Core.Json;
using NuGet;

namespace BlogEngine.Core.Packaging
{
    /// <summary>
    /// Package manager
    /// </summary>
    public class PackageManager
    {
        // "http://localhost/feed/FeedService.svc";
        // "http://dnbegallery.org/feed/FeedService.svc";
        private static readonly string _feedUrl = BlogSettings.Instance.GalleryFeedUrl; 

        private static IPackageRepository _repository
        {
            get
            {
                return PackageRepositoryFactory.Default.CreateRepository(
                    new PackageSource(_feedUrl, "Default"));
            }
        }

        /// <summary>
        /// Type of sort order
        /// </summary>
        public enum OrderType
        {
            /// <summary>
            /// Most downloaded
            /// </summary>
            Downloads,
            /// <summary>
            /// Newest
            /// </summary>
            Newest,
            /// <summary>
            /// Heighest rated
            /// </summary>
            Rating,
            /// <summary>
            /// Alphabetical
            /// </summary>
            Alphanumeric
        }

        /// <summary>
        /// Gallery pager
        /// </summary>
        public static Pager GalleryPager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pkgType"></param>
        /// <param name="page"></param>
        /// <param name="sortOrder"></param>
        /// <param name="searchVal"></param>
        /// <returns></returns>
        public static IQueryable<PublishedPackage> GetPackages(string pkgType, int page = 1, OrderType sortOrder = OrderType.Newest, string searchVal = "")
        {
            var srs = new PackagingSource {FeedUrl = _feedUrl};

            //TODO: add setting for gallery page size
            var pageSize = BlogSettings.Instance.PostsPerPage;
            var cnt = 0;

            try
            {
                var retPackages = new List<PublishedPackage>();
                var pkgList = GetPublishedPackages(srs);

                foreach (var pkg in pkgList.ToList())
                {
                    if(pkgType != "all"){
                        if (pkg.PackageType != pkgType) continue;
                    }

                    if (!pkg.IsLatestVersion) continue;

                    // for themes, get screenshot isetead of icon
                    // also get screenshot if icon is missing for package
                    if ((pkg.PackageType == "Theme" || string.IsNullOrEmpty(pkg.IconUrl)) && pkg.Screenshots != null && pkg.Screenshots.Count > 0)
                        pkg.IconUrl = pkg.Screenshots[0].ScreenshotUri;

                    // if both icon and screenshot missing, get default image for package type
                    if(string.IsNullOrEmpty(pkg.IconUrl))
                    {
                        switch (pkg.PackageType)
                        {
                            case "Theme":
                                pkg.IconUrl = 
                                    "http://dnbegallery.org/cms/Themes/OrchardGallery/Content/Images/themeDefaultIcon.png";
                                break;
                            case "Extension":
                                pkg.IconUrl = 
                                    "http://dnbegallery.org/cms/Themes/OrchardGallery/Content/Images/extensionDefaultIcon.png";
                                break;
                            case "Widget":
                                pkg.IconUrl =
                                    "http://dnbegallery.org/cms/Themes/OrchardGallery/Content/Images/widgetDefaultIcon.png";
                                break;
                        }
                    }

                    if (!string.IsNullOrEmpty(pkg.IconUrl) && !pkg.IconUrl.StartsWith("http:"))
                        pkg.IconUrl = "http://dnbegallery.org" + pkg.IconUrl;

                    if(string.IsNullOrEmpty(searchVal))
                    {
                        retPackages.Add(pkg);
                        cnt++;
                    }
                    else
                    {
                        if(pkg.Title.IndexOf(searchVal, StringComparison.OrdinalIgnoreCase) != -1
                           || pkg.Description.IndexOf(searchVal, StringComparison.OrdinalIgnoreCase) != -1
                           || (!string.IsNullOrWhiteSpace(pkg.Tags) && pkg.Tags.IndexOf(searchVal, StringComparison.OrdinalIgnoreCase) != -1))
                        {
                            retPackages.Add(pkg);
                            cnt++;
                        }
                    }
                }
                
                GalleryPager = new Pager(page, cnt);

                if (cnt > 0)
                {
                    switch (sortOrder)
                    {
                        case OrderType.Downloads:
                            retPackages = retPackages
                                .OrderByDescending(p => p.DownloadCount).ThenBy(p => p.Title)
                                .Skip((page - 1) * pageSize).Take(pageSize).ToList();
                            break;
                        case OrderType.Rating:
                            retPackages = retPackages
                                .OrderByDescending(p => p.Rating).ThenBy(p => p.Title)
                                .Skip((page - 1) * pageSize).Take(pageSize).ToList();
                            break;
                        case OrderType.Newest:
                            retPackages = retPackages
                                .OrderByDescending(p => p.LastUpdated).ThenBy(p => p.Title)
                                .Skip((page - 1) * pageSize).Take(pageSize).ToList();
                            break;
                        case OrderType.Alphanumeric:
                            retPackages = retPackages.OrderBy(p => p.Title)
                                .Skip((page - 1) * pageSize).Take(pageSize).ToList();
                            break;
                    }
                }
                return retPackages.AsQueryable();
            }
            catch (Exception)
            {
                GalleryPager = null;
                return null;
            }
        }

        /// <summary>
        /// Installed packages
        /// </summary>
        /// <param name="pkgType">Package type</param>
        /// <returns>List of installed packages</returns>
        public static List<JsonPackage> InstalledPackages(string pkgType)
        {
            var installedThemes = new Dictionary<string, JsonPackage>();
            var path = HttpContext.Current.Server.MapPath(string.Format("{0}themes/", Utils.ApplicationRelativeWebRoot));

            // read themes directory
            foreach (var d in Directory.GetDirectories(path))
            {
                var index = d.LastIndexOf(Path.DirectorySeparatorChar) + 1;
                var themeId = d.Substring(index);

                // first try to pull theme metadata from manifest file
                var p = FileSystem.GetThemeManifest(themeId);
                if(p == null)
                {
                    p = new JsonPackage();
                    p.Id = themeId;
                }

                if (p.Id != BlogSettings.Instance.Theme && 
                    p.Id != BlogSettings.Instance.MobileTheme &&
                    p.Id != "RazorHost")
                {
                    if(string.IsNullOrEmpty(p.IconUrl)) 
                        p.IconUrl = DefaultIconUrl(p);
                    installedThemes.Add(p.Id, p);
                }
            }

            // add package metadata from online repository
            var srs = new PackagingSource { FeedUrl = _feedUrl };
            try
            {
                var pkgList = GetPublishedPackages(srs);

                foreach (var pkg in pkgList.ToList())
                {
                    if (pkg.PackageType != pkgType || !pkg.IsLatestVersion) continue;

                    if(installedThemes.ContainsKey(pkg.Id))
                    {
                        installedThemes[pkg.Id].Title = pkg.Title;
                        installedThemes[pkg.Id].Description = pkg.Description;
                        installedThemes[pkg.Id].Tags = pkg.Tags;
                        installedThemes[pkg.Id].Authors = pkg.Authors;
                        installedThemes[pkg.Id].Website = pkg.ProjectUrl;
                        installedThemes[pkg.Id].LastUpdated = pkg.LastUpdated.ToString("dd MMM yyyy");
                        installedThemes[pkg.Id].Version = pkg.Version;

                        if (pkg.PackageType == "Theme" && pkg.Screenshots != null && pkg.Screenshots.Count > 0)
                            installedThemes[pkg.Id].IconUrl = string.Format("http://dnbegallery.org{0}", pkg.Screenshots[0].ScreenshotUri);
                    }
                }

                // return combined result
                return installedThemes.Select(t => t.Value).ToList();
            }
            catch (Exception)
            {
                // no connection - return local
                return installedThemes.Select(t => t.Value).ToList();
            }
        }

        /// <summary>
        /// Json package representing current selected theme
        /// </summary>
        /// <param name="themeId">Theme id</param>
        /// <returns>Json package</returns>
        public static JsonPackage GetCurrentTheme(string themeId)
        {
            // first try to pull theme metadata from manifest file
            var jp = FileSystem.GetThemeManifest(themeId);

            if (jp != null)
            {
                if (string.IsNullOrEmpty(jp.IconUrl)) 
                    jp.IconUrl = DefaultIconUrl(jp);
                return jp;
            }
                

            // if no manifest file, check themes in online gallery
            // and if found create manifest file using package info
            var srs = new PackagingSource { FeedUrl = _feedUrl };
            var gPkg = GetPublishedPackages(srs).ToList().Where(p => p.Id == themeId).FirstOrDefault();
            if(gPkg != null)
            {
                if (gPkg.Screenshots != null && gPkg.Screenshots.Count > 0)
                    gPkg.IconUrl = string.Format("http://dnbegallery.org{0}", gPkg.Screenshots[0].ScreenshotUri);

                jp = FileSystem.WriteThemeManifest(gPkg.Id, gPkg.Description, gPkg.Authors, gPkg.ProjectUrl, gPkg.Version, gPkg.IconUrl);
                if (jp != null) 
                    return jp;
            }

            // generate default blank manifest if both methods failed
            var jpw = FileSystem.WriteThemeManifest(themeId);
            if (jpw != null) jp = jpw;

            // return default values if theme folder read only
            if (jp == null)
                jp = new JsonPackage {Id = themeId, Authors = "Unknown", IconUrl = Utils.ApplicationRelativeWebRoot + "pics/Theme.png" };
            
            return jp;
        }

        /// <summary>
        /// Install package
        /// </summary>
        /// <param name="pkgId"></param>
        public static JsonResponse InstallPackage(string pkgId)
        {
            try
            {
                var packageManager = new NuGet.PackageManager(
                    _repository,
                    new DefaultPackagePathResolver(_feedUrl),
                    new PhysicalFileSystem(HttpContext.Current.Server.MapPath(Utils.ApplicationRelativeWebRoot + "App_Data/packages"))
                );

                var package = _repository.FindPackage(pkgId);

                packageManager.InstallPackage(package, false);

                FileSystem.CopyPackageFiles(package.Id, package.Version.ToString());

                // reset cache
                Blog.CurrentInstance.Cache.Remove("Installed-Themes");
            }
            catch (Exception ex)
            {
                Utils.Log("PackageManager.InstallPackage", ex);
                return new JsonResponse { Success = false, Message = "Error installing package, see logs for details" };
            }

            return new JsonResponse { Success = true, Message = "Package successfully installed" };
        }

        /// <summary>
        /// Uninstall package
        /// </summary>
        /// <param name="pkgId"></param>
        /// <returns></returns>
        public static JsonResponse UninstallPackage(string pkgId)
        {
            try
            {
                var packageManager = new NuGet.PackageManager(
                    _repository,
                    new DefaultPackagePathResolver(_feedUrl),
                    new PhysicalFileSystem(HttpContext.Current.Server.MapPath(Utils.ApplicationRelativeWebRoot + "App_Data/packages"))
                );

                var package = _repository.FindPackage(pkgId);

                if(package == null)
                    return new JsonResponse { Success = false, Message = "Package " + pkgId + " not found" };

                packageManager.UninstallPackage(package, true);

                FileSystem.RemovePackageFiles(package.Id, package.Version.ToString());

                // reset cache
                Blog.CurrentInstance.Cache.Remove("Installed-Themes");
            }
            catch (Exception ex)
            {
                Utils.Log("PackageManager.UninstallPackage", ex);
                return new JsonResponse { Success = false, Message = "Error uninstalling package, see logs for details" };
            }

            return new JsonResponse { Success = true, Message = "Package successfully uninstalled" };
        }

        static string DefaultIconUrl(JsonPackage pkg)
        {
            var url = string.Format("{0}themes/{1}/theme.png",
                Utils.ApplicationRelativeWebRoot, pkg.Id);

            var path = HttpContext.Current.Server.MapPath(url);
            return File.Exists(path) ? url : Utils.ApplicationRelativeWebRoot + "pics/Theme.png";
        }

        static IEnumerable<PublishedPackage> GetPublishedPackages(PackagingSource packagingSource = null)
        {
            return (new[] { packagingSource })
            .SelectMany(
                source =>
                {
                    var galleryFeedContext = new GalleryFeedContext(new Uri(_feedUrl)) { IgnoreMissingProperties = true };
                    return galleryFeedContext.Packages.Expand("Screenshots");
                }
            );
        }

    }
}
