using System.Collections.Generic;
using Compositional.Composer;

namespace Appson.Common.Configuration
{
    [Contract]
    public interface IApplicationSettingsLoader
    {
        string Load(string key);
        IDictionary<string, string> LoadAll();
    }
}