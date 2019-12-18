using System;
using Xunit;
using Demo.Main;

namespace Demo.Test
{
    public class UnitTest1
    {

        private Service _service;
        public UnitTest1()
        {
            _service = new Service();

        }

        [Fact]
        public void Test1()
        {
            var number = _service.GetNumber(1);
            Assert.Equal(1,1);
        }
    }
}
