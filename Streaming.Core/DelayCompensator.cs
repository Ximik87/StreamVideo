namespace Streaming.Core
{
    public class DelayCompensator
    {
        int currentDelay = 100;
        public int Delay => currentDelay;
        private bool isSecond = false;
        public void SetFail()
        {
            if (isSecond)
            {
                currentDelay += 50;
                isSecond = false;
            }
            else
            {
                isSecond = true;
            }

        }
    }
}
