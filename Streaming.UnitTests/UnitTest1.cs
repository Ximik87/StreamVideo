using Streaming.Core;
using System;
using Xunit;

namespace Streaming.UnitTests
{
    public class UnitTest1
    {
        [Fact]
        public void DelayCompensatorTest()
        {
            // ARRANGE
            var compensator = new DelayCompensator();

            // ACT
            compensator.SetFail();

            // ASSERT
            Assert.Equal(40, compensator.Delay);
        }

        [Fact]
        public void DelayCompensatorWith4Fails_Test()
        {
            // ARRANGE
            var compensator = new DelayCompensator();

            // ACT
            compensator.SetFail();
            compensator.SetFail();
            compensator.SetFail();
            compensator.SetFail();

            // ASSERT
            Assert.Equal(320, compensator.Delay);
        }
    }


}
