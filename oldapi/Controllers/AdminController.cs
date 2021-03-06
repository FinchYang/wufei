﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FaceRepository.Controllers
{
    [Produces("application/json")]
    public class AdminController : Controller
    {
        public readonly ILogger<AdminController> _log;

        public AdminController(ILogger<AdminController> log)
        {
            _log = log;
        }
        [HttpGet]
        [Route("GetFaceDesktopUpdatePackage")]
        public IActionResult GetFaceDesktopUpdatePackage(long version)
        {
            // var ret = new UpdateInfo();
            try
            {
                var ppath = "installer";
                //  Log.Info("path:" + ppath);
                if (!Directory.Exists(ppath))
                {
                    Directory.CreateDirectory(ppath);
                }
                var di = new DirectoryInfo(ppath).GetFiles();

                foreach (FileInfo fileInfo in di)
                {
                    if (fileInfo.Name.Contains("FaceDesktop"))
                    {
                        var tmp = fileInfo.Name.Replace(".", "");
                        var reg = new Regex(@"\d+");
                        var m = reg.Match(tmp).ToString();
                        if (long.Parse(m) > version)
                        {
                            //ret.Name = fileInfo.Name;
                            //ret.Date = fileInfo.CreationTime.ToLocalTime().ToString("F");
                            return Ok(Convert.ToBase64String(System.IO.File.ReadAllBytes(fileInfo.FullName)));
                          //  return Ok(System.IO.File.ReadAllBytes(fileInfo.FullName));
                          //  return Ok("hahah");
                            // break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _log.LogError(string.Format("FaceDesktop,GetNoticeUpdatePackage error:{0}", version), ex);
                return NotFound(ex.Message);
            }

            return NotFound(string.Empty);
        }
    }

   
}