using System.Collections.Generic;
using Appson.Composer;

namespace Appson.Common.GeneralComponents.Configuration
{
    [Contract]
    public interface IApplicationSettingsLoader
    {
        string Load(string key);
        IDictionary<string, string> LoadAll();
    }
}