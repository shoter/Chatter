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
        public void Fail()
        {
            Assert.True(false);
        }
    }
}
