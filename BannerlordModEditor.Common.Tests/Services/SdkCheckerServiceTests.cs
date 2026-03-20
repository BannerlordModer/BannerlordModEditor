using System;
using System.Threading.Tasks;
using Xunit;
using BannerlordModEditor.Common.Services;

namespace BannerlordModEditor.Common.Tests.Services
{
    public class SdkCheckerServiceTests
    {
        private readonly SdkCheckerService _service;

        public SdkCheckerServiceTests()
        {
            _service = new SdkCheckerService();
        }

        [Fact]
        public void GetDownloadUrl_ReturnsValidUrl()
        {
            var url = _service.GetDownloadUrl();
            Assert.NotNull(url);
            Assert.Contains("dotnet", url);
        }

        [Fact]
        public async Task CheckSdkAsync_ReturnsValidInfo()
        {
            var info = await _service.CheckSdkAsync();

            Assert.NotNull(info);
            Assert.NotNull(info.RecommendedVersion);
            Assert.NotNull(info.DownloadUrl);
        }

        [Fact]
        public void CheckSdk_ReturnsValidInfo()
        {
            var info = _service.CheckSdk();

            Assert.NotNull(info);
            Assert.NotNull(info.RecommendedVersion);
            Assert.NotNull(info.DownloadUrl);
        }

        [Fact]
        public void IsSdkInstalled_ReturnsBoolean()
        {
            var result = _service.IsSdkInstalled();
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void GetSdkVersion_ReturnsStringOrNull()
        {
            var result = _service.GetSdkVersion();
            Assert.True(result == null || !string.IsNullOrEmpty(result));
        }

        [Fact]
        public void GetInstalledVersions_ReturnsList()
        {
            var versions = _service.GetInstalledVersions();
            Assert.NotNull(versions);
        }
    }
}
