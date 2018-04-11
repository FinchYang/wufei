using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using API.dbmodel;

namespace API.Controllers
{
    [Produces("application/json")]
    //  [Route("api/Trails")]
    public partial class TrailsController : Controller
    {
        public readonly ILogger<TrailsController> _log;

        public TrailsController(ILogger<TrailsController> log)
        {
            _log = log;
        }

        [HttpPost]
        [Route("PostCompared")]
        public async Task<IActionResult> PostCompared([FromBody] ComparedInfo trails)
        {
             _log.LogDebug("{0}",111111);
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