using Streaming.Core;
using System;
using Xunit;

namespace Streaming.UnitTests
{
    public class DelayCompensatorTests
    {
        [Fact]
        public void DelayCompensatorTest()
        {
            // ARRANGE
            var compensator = new DelayCompensator();

            // ACT
            compensator.SetFail();

            // ASSERT
            Assert.Equal(100, compensator.Delay);
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
            Assert.Equal(200, compensator.Delay);
        }
    }


}