using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Streaming.Core;
using Xunit;

namespace Streaming.UnitTests
{
    public class CameraInfoParserTests
    {
        [Fact]
        public void ParseTest()
        {
            // ARRANGE
            var parser = new CameraInfoParser();

            // ACT
            parser.Parse();

            // ASSERT
            Assert.Equal(6, parser.CameraInfos.Count());
        }

        [Fact]
        public void ParseTest2()
        {
            // ARRANGE

            // ACT

            // ASSERT
        }
    }
}
