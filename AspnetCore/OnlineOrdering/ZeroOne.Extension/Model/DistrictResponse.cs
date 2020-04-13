using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroOne.Extension.Model
{
    public class DistrictResponse
    {
        public string status { get; set; }

        public string info { get; set; }

        public string infocode { get; set; }

        public string count { get; set; }

        public Suggestion suggestion { get; set; }

        public List<Districts> districts { get; set; }
    }

    public class Suggestion
    {
        public List<string> keywords { get; set; }

        public List<string> cities { get; set; }
    }

    public class Districts
    {
        public string citycode { get; set; }

        public string adcode { get; set; }

        public string name { get; set; }

        public string center { get; set; }

        public string level { get; set; }

        public List<Districts> districts { get; set; }
    }
}
