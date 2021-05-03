using System.Collections.Generic;
using System.IO;
using Moq;
using Streaming.Core;
using Streaming.Core.Interfaces;
using Xunit;

namespace Streaming.UnitTests
{
    public class SeparateCameraProcessTests
    {
        [Fact]
        public void StartTest_Success()
        {
            // ARRANGE
            var videoConsumer = new Mock<IVideoConsumer>();
            videoConsumer.SetupAdd(m => m.NewFrame += (sender, e) => { });
            var cameraData = new Mock<ICameraData>();
            var cameraProcess = new SeparateCameraProcess(videoConsumer.Object, cameraData.Object);

            // ACT
            cameraProcess.Start();

            // ASSERT
            videoConsumer.Verify(t => t.Start(), Times.Once);
            videoConsumer.VerifyAdd(t => t.NewFrame += It.IsAny<NewFrameEventHandler>());
            videoConsumer.VerifyNoOtherCalls();
        }

        [Fact]
        public void StopTest_Success()
        {
            // ARRANGE
            var videoConsumer = new Mock<IVideoConsumer>();
            videoConsumer.SetupRemove(m => m.NewFrame -= (sender, e) => { });
            var cameraData = new Mock<ICameraData>();
            var cameraProcess = new SeparateCameraProcess(videoConsumer.Object, cameraData.Object);

            // ACT
            cameraProcess.Stop();

            // ASSERT
            videoConsumer.Verify(t => t.Stop(), Times.Once);
            videoConsumer.VerifyRemove(t => t.NewFrame -= It.IsAny<NewFrameEventHandler>());
            videoConsumer.VerifyNoOtherCalls();
        }

        [Fact]
        public  void SetFrameTest()
        {
            // ARRANGE
            var expectedFrame = new List<Stream>();
            var videoConsumer = new Mock<IVideoConsumer>();
            videoConsumer.SetupAdd(m => m.NewFrame += (sender, e) => { });
            var cameraData = new Mock<ICameraData>();
            cameraData
                .SetupSet(p => p.Image = It.IsAny<Stream>())
                .Callback<Stream>(t => expectedFrame.Add(t));
            var cameraProcess = new SeparateCameraProcess(videoConsumer.Object, cameraData.Object);

            // ACT
            cameraProcess.Start();
            videoConsumer.Raise(m => m.NewFrame += null, this, new NewFrameEventArgs(new MemoryStream()));

            // ASSERT
            videoConsumer.Verify(t => t.Start(), Times.Once);
            videoConsumer.VerifyAdd(t => t.NewFrame += It.IsAny<NewFrameEventHandler>());
            Assert.Single(expectedFrame);
            videoConsumer.VerifyNoOtherCalls();
        }
    }


}
