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
using ZeroOne.Extension;
using ZeroOne.Extension.Model;
using Microsoft.Extensions.Options;

namespace ZeroOne.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        protected UploadSettings UploadSettings { get; set; }
        public UploadController(IOptions<UploadSettings> options)
        {
            this.UploadSettings = options.Value;
        }

        [HttpPost("UploadImage")]
        public ActionResult<ZeroOne.Entity.FileInfo> UploadImage(IFormFileCollection files)
        {

            //图片的根目录
            string imgRootPath = this.UploadSettings.SourceImgRootPath;
            //string baseImageFilePath =  Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "/Upload/Images/Source/");
            string basePhyicalPath = (PlatformServices.Default.Application.ApplicationBasePath + imgRootPath).Replace('\\', '/');
            if (!Directory.Exists(basePhyicalPath))
            {
                Directory.CreateDirectory(basePhyicalPath);
            }
            foreach (var file in this.Request.Form.Files)
            {
                var stream = file.OpenReadStream();
                //var originImage = Image.FromStream(stream);
                var filePath = Path.Combine(basePhyicalPath, file.FileName);
                //originImage.Save(filePath);
                var bitmap = new Bitmap(stream);
                bitmap.Save(filePath);
            }
            return new ZeroOne.Entity.FileInfo() { Id = Guid.NewGuid() };
        }

        /// <summary>
        /// 上传图片并且生成缩略图
        /// </summary>
        /// <param name="files">文件对象</param>
        /// <param name="parameter">上传文件附加数据</param>
        /// <returns></returns>
        [HttpPost("UploadImageAndGenerateThum")]
        public ActionResult<ZeroOne.Entity.FileInfo> UploadImageAndGenerateThum(IFormFileCollection files,ImageUploadParameter parameter)
        {

            //图片的根目录
            string imgRootPath = this.UploadSettings.SourceImgRootPath;
            //缩略图的根目录
            string thumRootPath = this.UploadSettings.ThumbnailImgRootPath;
            //string baseImageFilePath =  Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "/Upload/Images/Source/");
            string imgBasePhyicalPath = (PlatformServices.Default.Application.ApplicationBasePath + imgRootPath).Replace('\\', '/');
            //生成缩略图地址
            string thumBasePhyicalPath = (PlatformServices.Default.Application.ApplicationBasePath + thumRootPath).Replace('\\', '/');
            if (!Directory.Exists(imgBasePhyicalPath))
            {
                Directory.CreateDirectory(imgBasePhyicalPath);
            }
            //判断缩略图目录是否存在
            if (!Directory.Exists(thumBasePhyicalPath))
            {
                Directory.CreateDirectory(thumBasePhyicalPath);
            }
            foreach (var file in this.Request.Form.Files)
            {
                var stream = file.OpenReadStream();
                //var originImage = Image.FromStream(stream);
                var filePath = Path.Combine(imgBasePhyicalPath, file.FileName);
                //originImage.Save(filePath);
                var bitmap = new Bitmap(stream);
                bitmap.Save(filePath);
                //生成缩略图
                var thumImg = bitmap.GenerateThumbImage(parameter);
                var thumImgFilePath = Path.Combine(thumBasePhyicalPath, file.FileName);
                thumImg.Save(thumImgFilePath);
                //释放图片资源
                thumImg.Dispose();
                bitmap.Dispose();
            }
            return new ZeroOne.Entity.FileInfo() { Id = Guid.NewGuid() };
        }
    }
}