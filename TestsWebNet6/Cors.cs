using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestsWebNet6
{
    public record Cors
    {
        public string[] Origins { get; set; }
        public string[] Headers { get; set; }
    }
}
