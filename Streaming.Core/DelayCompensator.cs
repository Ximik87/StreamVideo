using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
