
using Appson.Composer;
using Microsoft.Owin.Security.DataProtection;

namespace Appson.Common.AspNetIdentity.DataProtectionProvider
{
    [Component]
    [IgnoredOnAssemblyRegistration]
    public class DataProtectionProviderComponent : IDataProtectionProviderContract
    {
        private readonly IDataProtectionProvider _provider;

        public DataProtectionProviderComponent(IDataProtectionProvider provider)
        {
            _provider = provider;
        }

        public IDataProtector Create(params string[] purposes)
        {
            return _provider.Create(purposes);
        }
    }
}