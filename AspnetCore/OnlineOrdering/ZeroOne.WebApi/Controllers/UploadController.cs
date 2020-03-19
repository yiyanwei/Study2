using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.Extensions.PlatformAbstractions;
using System.IO;

namespace ZeroOne.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        [HttpPost("UploadImage")]
        public ActionResult<ZeroOne.Entity.FileInfo> UploadImage(IFormFileCollection files)
        {
            //string baseImageFilePath =  Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "/Upload/Images/Source/");
            string basePhicalPath = (PlatformServices.Default.Application.ApplicationBasePath + "/Upload/Images/Source/").Replace('\\', '/');
            if (!Directory.Exists(basePhicalPath))
            {
                Directory.CreateDirectory(basePhicalPath);
            }
            foreach (var file in this.Request.Form.Files)
            {
                var stream = file.OpenReadStream();
                //var originImage = Image.FromStream(stream);
                var filePath = Path.Combine(basePhicalPath, file.FileName);
                //originImage.Save(filePath);
                var bitmap = new Bitmap(stream);
                bitmap.Save(filePath);
            }
            return new ZeroOne.Entity.FileInfo() { Id = Guid.NewGuid() };
        }
    }
}