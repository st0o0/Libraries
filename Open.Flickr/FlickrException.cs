using System;

namespace Open.Flickr
{
    public class FlickrException : Exception
    {
        public FlickrException(int code, string? message)
            : base(message)
        {
            Code = code;
        }

        public int Code { get; private set; }
    }
}