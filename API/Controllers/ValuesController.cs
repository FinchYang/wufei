using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GrainInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Orleans;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public partial class ValuesController : Controller
    {
      //  public readonly ILogger<ValuesController> _log;
        private IClusterClient client;
        
        public ValuesController(IClusterClient client)
        {
            this.client = client;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<string> Get(int id)
        {
            var grain = this.client.GetGrain<IValueGrain>(id);
            return await grain.GetValue();
        }
 public async Task< ReturnCode> Post([FromBody]CompareFaceInput input)
        {
            try
            {
                var faces = new FaceSource();
                 faces.FaceFile1 = Path.GetTempFileName()+".jpg";
               System.IO. File.WriteAllBytes(faces.FaceFile1, Convert.FromBase64String(input.picture1));
                 faces.FaceFile2 = Path.GetTempFileName()+".jpg";
              System.IO.  File.WriteAllBytes(faces.FaceFile2, Convert.FromBase64String(input.picture2));
                // Log.InfoFormat(" post {0}", 111);
                // var config = Orleans.Runtime.Configuration.ClientConfiguration.LocalhostSilo(30000);
                // Log.InfoFormat(" post {0}", 222);
                // GrainClient.Initialize(config);
                // Log.InfoFormat(" post {0}", 333);
                // var friend = GrainClient.GrainFactory.GetGrain<IFaceCompare>("extentedkey");
                // Log.InfoFormat("file1={0},file2={1}", faces.FaceFile1, faces.FaceFile2);
                // var result = await friend.SayHello(JsonConvert.SerializeObject(faces));

                // Log.InfoFormat(" post {0}", 444);
                var grain = this.client.GetGrain<IValueGrain>(1);
           var result= await grain.CompareFace(JsonConvert.SerializeObject(faces));
                return new ReturnCode { code = result, explanation = "" };
            }
            catch (Exception ex)
            {
                return new ReturnCode { code = -100, explanation = ex.Message };
            }
        }
        // PUT api/values/5
        [HttpPost("{id}")]
        public async Task Post(int id, [FromBody]string value)
        {
            var grain = this.client.GetGrain<IValueGrain>(id);
            await grain.SetValue(value);
        }
    }
}
