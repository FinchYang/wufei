namespace FaceRepository.Controllers
{
    public class NoidInput
        {
            public string id { get; set; }
        public string name { get; set; }
        public byte[] pic1 { get; set; }
        public byte[] pic2 { get; set; }
        public byte[] pic3 { get; set; }
    }
         public class NoidResult
        {
            public string id { get; set; }
            public CompareResult status { get; set; }
        }
        public enum CompareResult{
            unknown,success,failure,uncertainty
        }
         public class NoidResultInput
        {
            public string businessnumber { get; set; }
        }
}