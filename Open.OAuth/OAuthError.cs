using System.Runtime.Serialization;

namespace Open.OAuth
{
    [DataContract]
    public class OAuthError
    {
        [DataMember(Name = "error")]
        public string Error { get; set; }
    }
}