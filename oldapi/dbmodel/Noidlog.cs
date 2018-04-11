using FaceRepository.Models;
using System;
using System.Collections.Generic;

namespace FaceRepository.dbmodel
{
    public partial class Noidlog
    {
        public int Idnoidlog { get; set; }
        public string Idcardno { get; set; }
        public JsonObject<PictureInfoNoid> Capturephoto { get; set; }
        public bool Compared { get; set; }
        public short Result { get; set; }
        public string Businessnumber { get; set; }
        public bool Notified { get; set; }
    }
}
