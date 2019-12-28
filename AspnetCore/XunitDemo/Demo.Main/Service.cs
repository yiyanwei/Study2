using System;
using System.Collections.Generic;

namespace Demo.Main
{
    public class Service
    {
        public int GetNumber(int param)
        {
            return param;
        }

        public UserInfo GetUserInfo(int id)
        {
            return null;
        }

        public IList<UserInfo> GetUserInfos()
        {
            return new List<UserInfo>();
        }
        
    }
}
