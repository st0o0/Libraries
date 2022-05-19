using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlickrLibrary.Settings.Interfaces
{
    public interface IFlickrSettings
    {
        string APIKey { get; init; }
        string SharedKey { get; init; }
    }
}