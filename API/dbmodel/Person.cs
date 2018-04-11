
using System;
using System.Collections.Generic;

namespace API.dbmodel
{
    public partial class Person
    {
        public int Idperson { get; set; }
        public string Idcardno { get; set; }
        public string Name { get; set; }
        public string Nation { get; set; }
        public string Birthday { get; set; }
        public string Nationality { get; set; }
        public string Address { get; set; }
        public string Startdate { get; set; }
        public string Enddate { get; set; }
        public JsonObject<PictureInfo> Info { get; set; }
        public string Issuer { get; set; }
        public string Gender { get; set; }
    }
}
