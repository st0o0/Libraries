using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Open.Flickr
{
    [DataContract]
    public class PhotosetsResult
    {
        [DataMember(Name = "photosets", IsRequired = false)]
        public Photosets? Photosets { get; set; }

        [DataMember(Name = "stat", IsRequired = false)]
        public string? Stat { get; set; }
    }

    [DataContract]
    public class PhotosetResult
    {
        [DataMember(Name = "photoset", IsRequired = false)]
        public Photoset2? Photoset { get; set; }

        [DataMember(Name = "stat", IsRequired = false)]
        public string? Stat { get; set; }
    }

    [DataContract]
    public class PhotosResult
    {
        [DataMember(Name = "photos", IsRequired = false)]
        public Photoset? Photos { get; set; }

        [DataMember(Name = "stat", IsRequired = false)]
        public string? Stat { get; set; }
    }

    [DataContract]
    public class PhotoResult
    {
        [DataMember(Name = "photo", IsRequired = false)]
        public Photo2? Photo { get; set; }

        [DataMember(Name = "stat", IsRequired = false)]
        public string? Stat { get; set; }
    }

    public class PhotoSizeResult
    {
        [DataMember(Name = "sizes", IsRequired = false)]
        public PhotoSizeCollection? Sizes { get; set; }

        [DataMember(Name = "stat", IsRequired = false)]
        public string? Stat { get; set; }
    }

    [DataContract]
    public class Photosets
    {
        [DataMember(Name = "cancreate", IsRequired = false)]
        public int CanCreate { get; set; }

        [DataMember(Name = "page", IsRequired = false)]
        public int Page { get; set; }

        [DataMember(Name = "pages", IsRequired = false)]
        public int Pages { get; set; }

        [DataMember(Name = "perpage", IsRequired = false)]
        public int PerPage { get; set; }

        [DataMember(Name = "total", IsRequired = false)]
        public int Total { get; set; }

        [DataMember(Name = "photoset", IsRequired = false)]
        public Photoset[]? Photoset { get; set; }
    }

    [DataContract]
    public class Photoset
    {
        [DataMember(Name = "id", IsRequired = false)]
        public string? Id { get; set; }

        [DataMember(Name = "primary", IsRequired = false)]
        public string? Primary { get; set; }

        [DataMember(Name = "secret", IsRequired = false)]
        public string? Secret { get; set; }

        [DataMember(Name = "server", IsRequired = false)]
        public string? Server { get; set; }

        [DataMember(Name = "farm", IsRequired = false)]
        public int Farm { get; set; }

        [DataMember(Name = "photos", IsRequired = false)]
        public int Photos { get; set; }

        [DataMember(Name = "videos", IsRequired = false)]
        public int Videos { get; set; }

        [DataMember(Name = "title", IsRequired = false)]
        public Text? Title { get; set; }

        [DataMember(Name = "description", IsRequired = false)]
        public Text? Description { get; set; }

        [DataMember(Name = "needs_interstitial", IsRequired = false)]
        public int NeedsInterstitial { get; set; }

        [DataMember(Name = "visibility_can_see_set", IsRequired = false)]
        public int VisibilityCanSeeSet { get; set; }

        [DataMember(Name = "count_views", IsRequired = false)]
        public int CountViews { get; set; }

        [DataMember(Name = "count_comments", IsRequired = false)]
        public int CountComments { get; set; }

        [DataMember(Name = "can_comment", IsRequired = false)]
        public int CanComment { get; set; }

        [DataMember(Name = "date_create", IsRequired = false)]
        public string? DateCreate { get; set; }

        [DataMember(Name = "date_update", IsRequired = false)]
        public string? DateUpdate { get; set; }

        [DataMember(Name = "owner", IsRequired = false)]
        public string? Owner { get; set; }

        [DataMember(Name = "ownername", IsRequired = false)]
        public string? OwnerName { get; set; }

        [DataMember(Name = "photo", IsRequired = false)]
        public Photo[]? List { get; set; }

        [DataMember(Name = "page", IsRequired = false)]
        public int Page { get; set; }

        [DataMember(Name = "pages", IsRequired = false)]
        public int Pages { get; set; }

        [DataMember(Name = "perpage", IsRequired = false)]
        public int PerPage { get; set; }

        [DataMember(Name = "total", IsRequired = false)]
        public int Total { get; set; }
    }

    [DataContract]
    public class Photoset2
    {
        [DataMember(Name = "id", IsRequired = false)]
        public string? Id { get; set; }

        [DataMember(Name = "primary", IsRequired = false)]
        public string? Primary { get; set; }

        [DataMember(Name = "secret", IsRequired = false)]
        public string? Secret { get; set; }

        [DataMember(Name = "server", IsRequired = false)]
        public string? Server { get; set; }

        [DataMember(Name = "farm", IsRequired = false)]
        public int Farm { get; set; }

        [DataMember(Name = "photos", IsRequired = false)]
        public int Photos { get; set; }

        [DataMember(Name = "videos", IsRequired = false)]
        public int Videos { get; set; }

        [DataMember(Name = "title", IsRequired = false)]
        public string? Title { get; set; }

        [DataMember(Name = "needs_interstitial", IsRequired = false)]
        public bool NeedsInterstitial { get; set; }

        [DataMember(Name = "visibility_can_see_set", IsRequired = false)]
        public bool VisibilityCanSeeSet { get; set; }

        [DataMember(Name = "count_views", IsRequired = false)]
        public int CountViews { get; set; }

        [DataMember(Name = "count_comments", IsRequired = false)]
        public int CountComments { get; set; }

        [DataMember(Name = "can_comment", IsRequired = false)]
        public bool CanComment { get; set; }

        [DataMember(Name = "date_create", IsRequired = false)]
        public string? DateCreate { get; set; }

        [DataMember(Name = "date_update", IsRequired = false)]
        public string? DateUpdate { get; set; }

        [DataMember(Name = "owner", IsRequired = false)]
        public string? Owner { get; set; }

        [DataMember(Name = "ownername", IsRequired = false)]
        public string? OwnerName { get; set; }

        [DataMember(Name = "photo", IsRequired = false)]
        public Photo[]? List { get; set; }

        [DataMember(Name = "page", IsRequired = false)]
        public int Page { get; set; }

        [DataMember(Name = "pages", IsRequired = false)]
        public int Pages { get; set; }

        [DataMember(Name = "perpage", IsRequired = false)]
        public int PerPage { get; set; }

        [DataMember(Name = "total", IsRequired = false)]
        public int Total { get; set; }
    }

    [DataContract]
    public class Photo
    {
        [DataMember(Name = "id", IsRequired = false)]
        public string? Id { get; set; }

        [DataMember(Name = "secret", IsRequired = false)]
        public string? Secret { get; set; }

        [DataMember(Name = "server", IsRequired = false)]
        public string? Server { get; set; }

        [DataMember(Name = "farm", IsRequired = false)]
        public int Farm { get; set; }

        [DataMember(Name = "title", IsRequired = false)]
        public string? Title { get; set; }

        [DataMember(Name = "description", IsRequired = false)]
        public string? Description { get; set; }

        [DataMember(Name = "isprimary", IsRequired = false)]
        public string? IsPrimary { get; set; }

        [DataMember(Name = "ispublic", IsRequired = false)]
        public int IsPublic { get; set; }

        [DataMember(Name = "isfriend", IsRequired = false)]
        public int IsFriend { get; set; }

        [DataMember(Name = "isfamily", IsRequired = false)]
        public int IsFamily { get; set; }

        [DataMember(Name = "safety_level", IsRequired = false)]
        public string? SafetyLevel { get; set; }

        [DataMember(Name = "content_type", IsRequired = false)]
        public string? ContentType { get; set; }

        [DataMember(Name = "hidden", IsRequired = false)]
        public string? Hidden { get; set; }

        [DataMember(Name = "latitude", IsRequired = false)]
        public double Latitude { get; set; }

        [DataMember(Name = "longitude", IsRequired = false)]
        public double Longitude { get; set; }

        [DataMember(Name = "dateupload", IsRequired = false)]
        public string? DateUpload { get; set; }

        [DataMember(Name = "owner", IsRequired = false)]
        public string? Owner { get; set; }

        [DataMember(Name = "ownername", IsRequired = false)]
        public string? OwnerName { get; set; }
    }

    [DataContract]
    public class Photo2
    {
        [DataMember(Name = "id", IsRequired = false)]
        public string? Id { get; set; }

        [DataMember(Name = "secret", IsRequired = false)]
        public string? Secret { get; set; }

        [DataMember(Name = "server", IsRequired = false)]
        public string? Server { get; set; }

        [DataMember(Name = "farm", IsRequired = false)]
        public int Farm { get; set; }

        [DataMember(Name = "dateuploaded", IsRequired = false)]
        public string? DateUploaded { get; set; }

        [DataMember(Name = "isfavorite", IsRequired = false)]
        public int Isfavorite { get; set; }

        [DataMember(Name = "license", IsRequired = false)]
        public int License { get; set; }

        [DataMember(Name = "safety_level", IsRequired = false)]
        public int SafetyLevel { get; set; }

        [DataMember(Name = "rotation", IsRequired = false)]
        public int Rotation { get; set; }

        [DataMember(Name = "originalsecret", IsRequired = false)]
        public string? OriginalSecret { get; set; }

        [DataMember(Name = "originalformat", IsRequired = false)]
        public string? OriginalFormat { get; set; }

        [DataMember(Name = "owner", IsRequired = false)]
        public User2? Owner { get; set; }

        [DataMember(Name = "title", IsRequired = false)]
        public Text? Title { get; set; }

        [DataMember(Name = "description", IsRequired = false)]
        public Text? Description { get; set; }

        [DataMember(Name = "comments", IsRequired = false)]
        public Number? Comments { get; set; }

        [DataMember(Name = "visibility", IsRequired = false)]
        public Visibility? Visibility { get; set; }

        [DataMember(Name = "usage", IsRequired = false)]
        public Usage? Usage { get; set; }

        // note that this seems to only ever have one item in it, so is a faux collection
        [DataMember(Name = "urls", IsRequired = false)]
        public Urls? Urls { get; set; }
    }

    [DataContract]
    public class PhotoSize
    {
        [DataMember(Name = "label")]
        public string? Label { get; set; }

        [DataMember(Name = "width")]
        public int Width { get; set; }

        [DataMember(Name = "height")]
        public int Height { get; set; }

        [DataMember(Name = "source")]
        public string? Source { get; set; }

        [DataMember(Name = "media")]
        public string? Media { get; set; }
    }

    [DataContract]
    public class PhotoSizeCollection
    {
        [DataMember(Name = "canblog")]
        public bool CanBlog { get; set; }

        [DataMember(Name = "canprint")]
        public bool CanPrint { get; set; }

        [DataMember(Name = "candownload")]
        public bool CanDownload { get; set; }

        [DataMember(Name = "size")]
        public List<PhotoSize>? Sizes { get; set; }
    }

    [DataContract]
    public class Number
    {
        [DataMember(Name = "_content", IsRequired = false)]
        public int Value { get; set; }
    }

    public class Url
    {
        [DataMember(Name = "type", IsRequired = false)]
        public string? Type { get; set; }

        public string? _content { get; set; }

        public string? Value => _content;
    }

    public class Urls
    {
        [DataMember(Name = "url", IsRequired = false)]
        public List<Url>? Url { get; set; }
    }

    [DataContract]
    public class Usage
    {
        [DataMember(Name = "candownload")]
        public bool CanDownload { get; set; }

        [DataMember(Name = "canblog")]
        public bool CanBlog { get; set; }

        [DataMember(Name = "canprint")]
        public bool CanPrint { get; set; }

        [DataMember(Name = "canshare")]
        public bool CanShare { get; set; }
    }

    [DataContract]
    public class Visibility
    {
        [DataMember(Name = "ispublic", IsRequired = false)]
        public int IsPublic { get; set; }

        [DataMember(Name = "isfriend", IsRequired = false)]
        public int IsFriend { get; set; }

        [DataMember(Name = "isfamily", IsRequired = false)]
        public int IsFamily { get; set; }
    }
}