using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroOne.Application
{
    public class CountService
    {
        private int _count = 0;
        public int GetLatestCount()
        {
            return ++_count;
        }
    }
}
