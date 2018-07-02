using System;
using System.Threading;
using System.Threading.Tasks;
using Appson.Common.GeneralComponents.Cache;
using Appson.Common.GeneralComponents.Cache.Components;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Appson.Common.GeneralComponents.Tests
{
    [TestClass]
    public class AyncAutoLoadItemCacheTests
    {
        private AsyncAutoLoadItemCache<string, int> _cache;
        private const int CacheItemValue = 0;
        private Mock<IAsyncCacheItemLoader<string, int>> _mockItemLoader;
        private Mock<ICacheValueCopier<int>> _mockCopier;

        [TestInitialize]
        public void Initialize()
        {
            _mockItemLoader = new Mock<IAsyncCacheItemLoader<string, int>>();
            _mockItemLoader.Setup(loader => loader.Load(It.IsAny<string>()))
                .Returns(Task.FromResult(CacheItemValue))
                .Verifiable();

            _mockCopier = new Mock<ICacheValueCopier<int>>();
            _mockCopier.Setup(copier => copier.Copy(It.IsAny<int>())).Returns(CacheItemValue);

            _cache = new AsyncAutoLoadItemCache<string, int>
            {
                ItemLoader = _mockItemLoader.Object,
                Copier = _mockCopier.Object,
                MaximumLifetimeSeconds = 2
            };
        }

        [TestMethod]
        public async Task GetItem_ColdCache_ShouldInvokeLoaderOnce()
        {
            // Act
            var item  = await _cache.GetItem("foo");

            // Assert
            Assert.AreEqual(CacheItemValue, item);
            _mockItemLoader.Verify(l => l.Load(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public async Task GetItem_HotCache_ShouldNotInvokeLoaderTwice()
        {
            // Arrange
            await _cache.GetItem("foo");

            // Act
            await _cache.GetItem("foo");

            // Assert
            _mockItemLoader.Verify(l => l.Load(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public async Task GetItem_ExpiredHotCache_ShouldInvokeLoaderTwice()
        {
            // Arrange
            await _cache.GetItem("foo");
            Thread.Sleep(TimeSpan.FromSeconds(_cache.MaximumLifetimeSeconds));

            // Arct
            await _cache.GetItem("foo");

            // Assert
            _mockItemLoader.Verify(l => l.Load(It.IsAny<string>()), Times.Exactly(2));
        }
    }
}
