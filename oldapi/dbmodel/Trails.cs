using FaceRepository.Models;
using System;
using System.Collections.Generic;

namespace FaceRepository.dbmodel
{
    public partial class Trails
    {
        public int Idtrails { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Address { get; set; }
        public string Operatingagency { get; set; }
        public JsonObject<PictureInfo> Info { get; set; }
        public string Idcardno { get; set; }
    }
}
