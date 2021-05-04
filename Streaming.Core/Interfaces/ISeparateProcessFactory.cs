namespace Streaming.Core.Interfaces
{
    public interface ISeparateProcessFactory
    {
        ISeparateCameraProcess Create(ICameraData camera);
    }
}
