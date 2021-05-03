using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Streaming.Core;
using Xunit;

namespace Streaming.FuncTests
{
    public class FunctionalTests
    {
        [Fact]
        public void ParserTest_Success()
        {
            // ARRANGE
            var loader = new HtmlContentLoader();
            var parser = new CameraInfoParser(loader);

            // ACT
            var result = parser.Parse();

            // ASSERT
            Assert.Equal(6, result.Count());
        }

        [Fact]
        public void LoaderTest_Success()
        {
            // ARRANGE
            var loader = new HtmlContentLoader();

            // ACT
            var result = loader.GetHtmlContent();

            // ASSERT
            Assert.Contains("html", result);
            Assert.True(result.Length > 0);
        }

        [Fact]
        public async Task VideoConsumerTest_Success()
        {
            // ARRANGE
            var frames = new List<Stream>();
            var consumer = new VideoConsumer("http://77.22.100.19:88/mjpg/video.mjpg");
            consumer.NewFrame += (sender, e) =>
            {
                frames.Add(e.Frame);
            };


            // ACT
            consumer.Start();
            await Task.Delay(TimeSpan.FromSeconds(2));

            // ASSERT
            Assert.True(frames.Count > 0);
            ValidateFrames(frames);
        }

        private static void ValidateFrames(List<Stream> frames)
        {
            foreach (var frame in frames)
            {
                var bytes = new byte[frame.Length];
                frame.Read(bytes, 0, (int)frame.Length);
                // JPEG The header of the file is: FF D8 FF
                // tail is: FF D9
                Assert.Equal("FF", bytes[0].ToString("X"));
                Assert.Equal("D8", bytes[1].ToString("X"));
                Assert.Equal("FF", bytes[2].ToString("X"));
                Assert.Equal("FF", bytes[bytes.Length - 2].ToString("X"));
                Assert.Equal("D9", bytes[bytes.Length - 1].ToString("X"));
            }
        }
    }
}
