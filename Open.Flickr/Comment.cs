using System.Runtime.Serialization;

namespace Open.Flickr
{
    [DataContract]
    public class Comment
    {
        [DataMember(Name = "id", IsRequired = false)]
        public string? Id { get; set; }

        [DataMember(Name = "author", IsRequired = false)]
        public string? Author { get; set; }

        [DataMember(Name = "authorname", IsRequired = false)]
        public string? AuthorName { get; set; }

        [DataMember(Name = "iconserver", IsRequired = false)]
        public string? IconServer { get; set; }

        [DataMember(Name = "iconfarm", IsRequired = false)]
        public int IconFarm { get; set; }

        [DataMember(Name = "datecreate", IsRequired = false)]
        public string? CreatedDate { get; set; }

        [DataMember(Name = "permalink", IsRequired = false)]
        public string? Permalink { get; set; }

        [DataMember(Name = "_content", IsRequired = false)]
        public string? Text { get; set; }
    }

    [DataContract]
    public class Comments
    {
        [DataMember(Name = "photo_id", IsRequired = false)]
        public string? PhotoId { get; set; }

        [DataMember(Name = "comment", IsRequired = false)]
        public Comment[]? List { get; set; }
    }

    [DataContract]
    public class CommentResult
    {
        [DataMember(Name = "comment", IsRequired = false)]
        public Comment? Comment { get; set; }

        [DataMember(Name = "stat", IsRequired = false)]
        public string? Stat { get; set; }
    }

    [DataContract]
    public class CommentsResult
    {
        [DataMember(Name = "comments", IsRequired = false)]
        public Comments? Comments { get; set; }

        [DataMember(Name = "stat", IsRequired = false)]
        public string? Stat { get; set; }
    }
}