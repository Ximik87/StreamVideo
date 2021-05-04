using Microsoft.Extensions.Logging;
using Streaming.Core.Interfaces;

namespace Streaming.Core
{
    public class SeparateProcessFactory : ISeparateProcessFactory
    {
        private readonly ILoggerFactory _logger;

        public SeparateProcessFactory(ILoggerFactory logger)
        {
            _logger = logger;
        }

        public ISeparateCameraProcess Create(ICameraData camera)
        {
            var logger = _logger.CreateLogger<VideoConsumer>();
            var videoConsumer = new VideoConsumer(logger, camera);
            var separateProcess = new SeparateCameraProcess(videoConsumer, camera);
            return separateProcess;
        }
    }
}
