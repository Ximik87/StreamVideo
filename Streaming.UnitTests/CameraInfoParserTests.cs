using System.Linq;
using Microsoft.Extensions.Logging;
using Moq;
using Streaming.Core;
using Streaming.Core.Interfaces;
using Xunit;

namespace Streaming.UnitTests
{
    public class CameraInfoParserTests
    {
        [Fact]
        public void ParseTest_Success()
        {
            // ARRANGE
            var expectedUrl = "http://62.225.227.45:8080/mjpg/video.mjpg";
            var expectedTitle = "View Axis CCTV IP camera online in Germany, Ahaus";

            var html = GetHtmlTemplate(expectedTitle, expectedUrl);

            var loader = new Mock<IHtmlContentLoader>();
            var logger = new Mock<ILogger>();
            loader.Setup(t => t.GetHtmlContent()).Returns(html);
            var parser = new CameraInfoParser(loader.Object, logger.Object);

            // ACT
            var result = parser.Parse();

            // ASSERT
            Assert.Single(result);
            Assert.Equal(expectedTitle, result.Single().Title);
            Assert.Equal(expectedUrl, result.Single().Url);
        }

        [Fact]
        public void ParseTest_WithException()
        {
            // ARRANGE
            var expectedUrl = "http://62.225.227.45:8080/mjpg/video.mjpg";
            var expectedTitle = "View Axis CCTV IP camera online in Germany, Ahaus";

            var html = GetHtmlTemplate(expectedTitle, expectedUrl).Substring(4, 126);
            var logger = new Mock<ILogger>();
            var loader = new Mock<IHtmlContentLoader>();
            loader.Setup(t => t.GetHtmlContent()).Returns(html);
            var parser = new CameraInfoParser(loader.Object, logger.Object);

            // ACT
            var result = parser.Parse();

            // ASSERT
        }


        private static string GetHtmlTemplate(string title, string url)
        {
            return $@"<div class=""row thumbnail-items"">
                        <div class=""col-xs-12 col-sm-6 col-md-4 col-lg-4"">
                            <div class=""thumbnail-item"">
                                <a class=""thumbnail-item__wrap"" href=""/en/view/918422/"" title=""View CCTV IP camera online in germany, Ahaus"">
                                    <div class=""thumbnail-item__preview"">
                                        <img id=""image918422"" 
                                             class=""thumbnail-item__img img-responsive"" 
                                             src=""{url}"" 
                                             title=""{title}"" alt="""" />
                                    </div>
                                    <div class=""thumbnail-item__caption"">
                                        <p>Watch Axis camera in Germany,Ahaus</p>
                                    </div>
                                </a>
                            <div class=""admin-buttons"">
                            </div>
                        </div>
                      </div>
                      </div>
                      <div class=""textcenter"">";
        }
    }
}
