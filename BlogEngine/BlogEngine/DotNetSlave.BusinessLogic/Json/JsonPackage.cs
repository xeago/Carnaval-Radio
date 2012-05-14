namespace BlogEngine.Core.Json
{
    /// <summary>
    /// Json wrapper for package object
    /// </summary>
    public class JsonPackage
    {
        /// <summary>
        /// Package Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Package type
        /// </summary>
        public string PackageType { get; set; }
        /// <summary>
        /// Title
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Version
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// Desctiption
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Download count
        /// </summary>
        public int DownloadCount { get; set; }
        /// <summary>
        /// Last updated
        /// </summary>
        public string LastUpdated { get; set; }
        /// <summary>
        /// Project Website
        /// </summary>
        public string Website { get; set; }
        /// <summary>
        /// Package url in the server gallery
        /// </summary>
        public string PackageUrl { get; set; }
        /// <summary>
        /// Icon URL
        /// </summary>
        public string IconUrl { get; set; }
        /// <summary>
        ///  Authors
        /// </summary>
        public string Authors { get; set; }
        /// <summary>
        /// Tags
        /// </summary>
        public string Tags { get; set; }
    }
}