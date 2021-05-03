using System.Collections.Generic;
using AutoFixture;
using Moq;
using Streaming.Core;
using Streaming.Core.Interfaces;
using Xunit;

namespace Streaming.UnitTests
{
    public class LinkContainerTests
    {
        private readonly Fixture _fixture = new Fixture();

        [Fact]
        public void GetContentTest_Success()
        {
            // ARRANGE
            var expectedList = _fixture.Create<List<CameraInfo>>();
            var parser = new Mock<ILinkParser>();
            parser.Setup(t => t.Parse()).Returns(expectedList);

            var linkContainer = new LinkContainer(parser.Object);

            // ACT
            linkContainer.GetContent();

            // ASSERT
            Assert.Equal(expectedList, linkContainer.CameraInfos);
        }

        [Fact]
        public void GetContentTest_WithEmptyCollection()
        {
            // ARRANGE
            var expectedList = new List<CameraInfo>();
            var parser = new Mock<ILinkParser>();
            parser.Setup(t => t.Parse()).Returns(expectedList);

            var linkContainer = new LinkContainer(parser.Object);

            // ACT
            linkContainer.GetContent();

            // ASSERT
            Assert.Empty(linkContainer.CameraInfos);
        }
    }
}
