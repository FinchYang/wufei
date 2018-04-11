
using System;
using System.Collections.Generic;

namespace API.dbmodel
{ public class ComparedInfo
    {
        public string id { get; set; }
        public string name { get; set; }
        public string nation { get; set; }
        public string nationality { get; set; }
        public string address { get; set; }
        public string idaddress { get; set; }
        public string operatingagency { get; set; }
        public string issuer { get; set; }
        public string gender { get; set; }
        public string birthday { get; set; }
        public string startdate { get; set; }
        public string enddate { get; set; }
        public byte[] idphoto { get; set; }
        public byte[] capturephoto { get; set; }
    }
     public class PictureInfo
    {
        public byte[] base64pic { get; set; }
    }
    public class PictureInfoNoid
    {
        public byte[] base64pic1 { get; set; }
        public byte[] base64pic2 { get; set; }
        public byte[] base64pic3 { get; set; }
    }
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
