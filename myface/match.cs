using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace com.baidu.ai
{
    public class matchreq
    {
        public string image { get; set; }
        public string image_type { get; set; }
        public string face_type { get; set; }
        public string quality_control { get; set; }
        public string liveness_control { get; set; }
    }
    public class matchresponse
    {
        public int error_code { get; set; }
        public scoreret result { get; set; }
        public string error_msg { get; set; }
    }
    public class scoreret
    {
        public float score { get; set; }
    }
    public class FaceMatch
    {
        // 人脸对比
        public static string match(string token,string req)
        {
          //  var re = new matchresponse();
            string host = "https://aip.baidubce.com/rest/2.0/face/v3/match?access_token=" + token;
            Encoding encoding = Encoding.Default;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(host);
            request.Method = "post";
            request.KeepAlive = true;
         //   String str = "[{\"image\":\"sfasq35sadvsvqwr5q...\",\"image_type\":\"BASE64\",\"face_type\":\"LIVE\",\"quality_control\":\"LOW\",\"liveness_control\":\"HIGH\"},{\"image\":\"sfasq35sadvsvqwr5q...\",\"image_type\":\"BASE64\",\"face_type\":\"IDCARD\",\"quality_control\":\"LOW\",\"liveness_control\":\"HIGH\"}]";
            byte[] buffer = encoding.GetBytes(req);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default);
            string result = reader.ReadToEnd();
           
            Console.WriteLine("人脸对比:");
            Console.WriteLine(result);
            return result;
        }
    }
}
