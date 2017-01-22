using Compositional.Composer;

namespace Appson.Common.Configuration
{
    [Contract]
    public interface IApplicationSettings
    {
        string this[string key] { get; }
        void Reload();
    }
}