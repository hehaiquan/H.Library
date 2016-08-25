using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace ConsoleApplication1
{
    public class DateTimeFormatSettings
    {
        public string LongDatePattern { get; set; }
        public string LongTimePattern { get; set; }
        public string ShortDatePattern { get; set; }
        public string ShortTimePattern { get; set; }


        public DateTimeFormatSettings(IConfiguration configuration)
        {
            this.LongDatePattern = configuration["LongDatePattern"];
            this.LongTimePattern = configuration["LongTimePattern"];
            this.ShortDatePattern = configuration["ShortDatePattern"];
            this.ShortTimePattern = configuration["ShortTimePattern"];
        }
    }
}
