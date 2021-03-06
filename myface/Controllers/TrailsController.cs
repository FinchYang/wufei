﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using myface;
using System.IO;
using System.Text.RegularExpressions;
using com.baidu.ai;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace myface
{
    [Produces("application/json")]
    //  [Route("api/Trails")]
    public partial class TrailsController : Controller
    {
        public readonly ILogger<TrailsController> _log;
  

        public IConfiguration Configuration { get; }
        public TrailsController(ILogger<TrailsController> log,IConfiguration configuration)
        {
             Configuration = configuration;
            _log = log;
        }
        [HttpPost]
        [Route("cloudCompare")]
        public ReturnCode cloudCompare([FromBody]CompareFaceInput input)
        {
            try
            {
                var FaceFile1 = Path.GetTempFileName() + ".jpg";
                System.IO.File.WriteAllBytes(FaceFile1, Convert.FromBase64String(input.picture1));
                var FaceFile2 = Path.GetTempFileName() + ".jpg";
                System.IO.File.WriteAllBytes(FaceFile2, Convert.FromBase64String(input.picture2));


                return new ReturnCode { code = SmartCompare(FaceFile1, FaceFile2) ? 1 : 0, explanation = "" };
            }
            catch (Exception ex)
            {
                return new ReturnCode { code = -100, explanation = ex.Message };
            }
        }
        [HttpPost]
        [Route("smartFacesCompare")]
        public ReturnCode smartFacesCompare([FromBody]SmartCompareFaceInput input)
        {
            try
            {
                var idimage = Path.GetTempFileName() + ".jpg";
                System.IO.File.WriteAllBytes(idimage, Convert.FromBase64String(input.idimage));
                var capture = Path.GetTempFileName() + ".jpg";
                System.IO.File.WriteAllBytes(capture, Convert.FromBase64String(input.capture));
                var a = SmartCompareId(capture, idimage, input.id);
                return new ReturnCode { code = a.ok ? 1 : 0, explanation = a.some };
            }
            catch (Exception ex)
            {
                return new ReturnCode { code = -100, explanation = ex.Message };
            }
        }
        public class ReturnCode
        {
            public int code { get; set; }
            public string explanation { get; set; }
        }
        public class CompareFaceInput
        {
            public string picture1 { get; set; }
            public string picture2 { get; set; }
        }
        public class SmartCompareFaceInput
        {
            public string id { get; set; }
            public string idimage { get; set; }
            public string capture { get; set; }
        }
        public class Comparereturn
        {
            public bool ok { get; set; }
            public string some { get; set; }
        }
        Comparereturn SmartCompareId(string capturefile, string idimagefile, string id)
        {
            
            var cret = new Comparereturn { some = "ok", ok = false };
            var a = new System.Diagnostics.Process();
            a.StartInfo.UseShellExecute = false;
            a.StartInfo.RedirectStandardOutput = true;
            a.StartInfo.CreateNoWindow = true;
            var localimage = Path.Combine("what", id) + ".jpg";
            var _score = 0.73;
           // var idcompare = false;
            if (System.IO.File.Exists(localimage))
            {
                a.StartInfo.Arguments = string.Format(" \"{0}\"  \"{1}\"", localimage, capturefile);
                _score = 0.83;
            }
            else
            {
                a.StartInfo.Arguments = string.Format(" \"{0}\"  \"{1}\"", idimagefile, capturefile);
              //  idcompare = true;
            }
            a.StartInfo.FileName = "./facer";
            a.StartInfo.WorkingDirectory = ".";
            a.Start();
            var output = a.StandardOutput.ReadToEnd();
            a.WaitForExit();
            var ret = a.ExitCode;
            var reg = @"(?<=terminate)0\.[\d]+";
            var m = Regex.Match(output, reg);

            cret.some = output;
            if (m.Success)
            {
                var score = double.Parse(m.Value);
                if (score > _score)
                {
                    System.IO.File.Copy(capturefile, localimage, true);
                    // var th = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(uploadinfo));
                    // th.Start(upload);
                    //return (int)(score*100);
                    cret.ok = true;
                }
            }
            else
            {
                if (output.Contains("image1"))
                {
                    var key=Configuration["clientId"];
                    var secret=Configuration["clientSecret"];
                    var to =new tokenret();
                    if(!string.IsNullOrEmpty(key)&&!string.IsNullOrEmpty(secret)){
to = AccessToken.getAccessToken(key,secret);
            Console.WriteLine(key+"---"+secret);
                    }
                    else to = AccessToken.getAccessToken();
                    if (to.ok)
                    {
                        var req = new List<matchreq>();
                        req.Add(new matchreq
                        {
                            image = Convert.ToBase64String(System.IO.File.ReadAllBytes(idimagefile)),
                            face_type = "IDCARD",
                            image_type = "BASE64",
                            quality_control = "NONE",
                            liveness_control = "NONE",
                        });
                        req.Add(new matchreq
                        {
                            image = Convert.ToBase64String(System.IO.File.ReadAllBytes(capturefile)),
                            face_type = "LIVE",
                            image_type = "BASE64",
                            quality_control = "NONE",
                            liveness_control = "NONE",
                        });
                        var mbai = FaceMatch.match(to.access_token, JsonConvert.SerializeObject(req));
                        var bret = JsonConvert.DeserializeObject<matchresponse>(mbai);
                        if (bret.error_code == 0)
                        {
                            if (bret.result.score > 80)
                            {
                                cret.ok = true;
                                System.IO.File.Copy(capturefile, localimage, true);
                                cret.some="baidu-" + bret.result.score;
                            }
                            else
                            {
                                cret.some="baidu-" +"not ok-" + bret.result.score;
                            }
                        }
                        else
                        {

                            cret.some="baidu-" +bret.error_code + bret.error_msg;
                        }
                    }
                    else cret.some=to.access_token;
                }
            }
            return cret;
        }
        bool SmartCompare(string f1, string f2)
        {
            var a = new System.Diagnostics.Process();

            a.StartInfo.UseShellExecute = false;
            a.StartInfo.RedirectStandardOutput = true;
            a.StartInfo.CreateNoWindow = true;
            a.StartInfo.FileName = "./facer";
            a.StartInfo.WorkingDirectory = ".";
            a.StartInfo.Arguments = string.Format(" \"{0}\"  \"{1}\"", f1, f2);
            a.Start();
            var output = a.StandardOutput.ReadToEnd();
            a.WaitForExit();
            var ret = a.ExitCode;

            var reg = @"(?<=terminate)0\.[\d]{4,}";
            var m = Regex.Match(output, reg);
            if (m.Success)
            {
                var score = double.Parse(m.Value);
                // labelscore.Text = ((int)(score * 100)).ToString() + "%";
                if (score > 0.74)
                {
                    return true;
                }
            }
            _log.LogInformation("outpu:{0}", output);
            return false;
        }
        [Route("testcompare")]
        public ReturnCode testcompare()
        {
            try
            {
                var FaceFile1 = "222.jpg";
                var FaceFile2 = "233.jpg";

                return new ReturnCode { code = SmartCompare(FaceFile1, FaceFile2) ? 1 : 0, explanation = "" };
            }
            catch (Exception ex)
            {
                return new ReturnCode { code = -100, explanation = ex.Message };
            }
        }
        [HttpPost]
        [Route("PostCompared")]
        public async Task<IActionResult> PostCompared([FromBody] ComparedInfo trails)
        {
            _log.LogDebug("{0}", 111111);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _log.LogDebug("{0}", 2222);
            try
            {
                using (var db = new faceContext())
                {
                    _log.LogDebug("{0}", 3333);
                    var person = db.Person.FirstOrDefault(c => c.Idcardno == trails.id);
                    if (person == null)
                    {
                        db.Person.Add(new Person
                        {
                            Idcardno = trails.id,
                            Name = trails.name,
                            Nation = trails.nation,
                            Nationality = trails.nationality,
                            Birthday = trails.birthday,
                            Address = trails.idaddress,
                            Startdate = trails.startdate,
                            Enddate = trails.enddate,
                            Gender = trails.gender,
                            Issuer = trails.issuer,
                            Info = new PictureInfo { base64pic = trails.idphoto },
                        });
                    }
                    _log.LogDebug("{0}", Convert.ToBase64String(trails.capturephoto).Take(30));
                    _log.LogDebug("{0}", Convert.ToBase64String(trails.capturephoto).Skip(3000).Take(30));
                    _log.LogDebug("{0}", Convert.ToBase64String(trails.capturephoto).TakeLast(30));
                    db.Trails.Add(new Trails
                    {
                        TimeStamp = DateTime.Now,
                        Address = trails.address,
                        Idcardno = trails.id,
                        Operatingagency = trails.operatingagency,
                        Info = new PictureInfo { base64pic = trails.capturephoto },
                    });
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            return Ok(new commonresponse { status = 0, explanation = "ok" });
        }

    }
}