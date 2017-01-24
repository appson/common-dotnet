
using Appson.Composer;

namespace Appson.Common.GeneralComponents.Configuration
{
    [Contract]
    public interface IApplicationSettings
    {
        string this[string key] { get; }
        void Reload();
    }
}