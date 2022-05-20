using System.Runtime.Serialization;

namespace Open.Flickr
{
    [DataContract]
    public class UserResponse
    {
        [DataMember(Name = "user", IsRequired = false)]
        public User? User { get; set; }

        [DataMember(Name = "stat", IsRequired = false)]
        public string? Stat { get; set; }

        [DataMember(Name = "code", IsRequired = false)]
        public int Code { get; set; }

        [DataMember(Name = "message", IsRequired = false)]
        public string? Message { get; set; }
    }

    [DataContract]
    public class Text
    {
        [DataMember(Name = "_content", IsRequired = false)]
        public string? Content { get; set; }
    }

    [DataContract]
    public class User
    {
        [DataMember(Name = "id", IsRequired = false)]
        public string? Id { get; set; }

        [DataMember(Name = "username", IsRequired = false)]
        public Text? UserName { get; set; }
    }

    [DataContract]
    public class User2
    {
        [DataMember(Name = "id", IsRequired = false)]
        public string? Id { get; set; }

        [DataMember(Name = "username", IsRequired = false)]
        public string? UserName { get; set; }

        [DataMember(Name = "realname", IsRequired = false)]
        public string? RealName { get; set; }

        [DataMember(Name = "location", IsRequired = false)]
        public string? Location { get; set; }

        [DataMember(Name = "iconserver", IsRequired = false)]
        public string? IconServer { get; set; }

        [DataMember(Name = "iconfarm", IsRequired = false)]
        public int IconFarm { get; set; }

        [DataMember(Name = "path_alias", IsRequired = false)]
        public string? PathAlias { get; set; }
    }
}