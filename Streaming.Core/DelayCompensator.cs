namespace Streaming.Core
{
    public class DelayCompensator
    {
        private int _currentDelay = 100;
        private int _retryCount = 0;
        public int Delay => _currentDelay;

        public void SetFail()
        {
            if (_retryCount >= 3)
            {
                _currentDelay += 50;
                _retryCount = 0;
            }
            else
            {
                _retryCount++;
            }

        }
    }
}
