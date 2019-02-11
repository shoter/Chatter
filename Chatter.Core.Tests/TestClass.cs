using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Chatter.Core.Tests
{
    public class TestClass
    {
        [Fact]
        public void fail()
        {
            Assert.True(false);
        }

        [Fact]
        public void superFail()
        {
            throw new Exception("ASDASDASD");
        }
    }
}
