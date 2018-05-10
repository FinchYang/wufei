
using System;
using System.Collections.Generic;

namespace myface
{
    public partial class Noidloghis
    {
        public int Noidloghis1 { get; set; }
        public string Idcardno { get; set; }
        public JsonObject<PictureInfoNoid> Capturephoto { get; set; }
        public bool Compared { get; set; }
        public short Result { get; set; }
        public string Businessnumber { get; set; }
        public DateTime Stamp { get; set; }
    }
}
