using System;
using System.Collections.Generic;

namespace API.dbmodel
{
    public partial class Organization
    {
        public int Idorganization { get; set; }
        public string Businessnumber { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }
}
