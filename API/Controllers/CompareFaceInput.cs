namespace API.Controllers
{public class commonresponse
        {
            public int status { get; set; }
            public string explanation { get; set; }
        }
    public partial class ValuesController
    {
        public class CompareFaceInput
    {
        public string picture1 { get; set; }
        public string picture2 { get; set; }
    }
    }
}
