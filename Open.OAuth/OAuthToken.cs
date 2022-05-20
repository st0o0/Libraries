using System.Runtime.Serialization;

namespace Open.OAuth
{
    [DataContract]
    public class OAuthToken
    {
        [DataMember(Name = "callback_confirmed", EmitDefaultValue = false)]
        public bool CallbackConfirmed { get; set; }
        [DataMember(Name = "token", EmitDefaultValue = false)]
        public string Token { get; set; }
        [DataMember(Name = "token_secret", EmitDefaultValue = false)]
        public string TokenSecret { get; set; }
    }
}
