using System.Threading.Tasks;
using Appson.Common.GeneralComponents.Cache;
using Appson.Composer;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Appson.Common.GeneralComponents.Tests
{
    [TestClass]
    public class AutoLoadAsyncItemCacheTests
    {
        private IAsyncItemCache<string, int> _cache;
        private ComponentContext _composer;

        [TestInitialize]
        public void Initialize()
        {
            
        }

        [TestMethod]
        public async Task GetItem_ColdCache_ShouldInvokeLoader()
        {

        }

        [TestMethod]
        public async Task GetItem_HotCache_ShouldNotInvokeLoader()
        {

        }

        [TestMethod]
        public async Task GetItem_ExpiredHotCache_ShouldInvokeLoader()
        {

        }
    }
}
