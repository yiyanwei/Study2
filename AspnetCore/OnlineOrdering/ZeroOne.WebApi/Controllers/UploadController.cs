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
using ZeroOne.Application;
using ZeroOne.Entity;

namespace ZeroOne.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        protected IFileInfoService FileService { get; set; }
        protected UploadSettings UploadSettings { get; set; }
        public UploadController(IFileInfoService fileService, IOptions<UploadSettings> options)
        {
            this.FileService = fileService;
            this.UploadSettings = options.Value;
        }

        //[HttpPost("UploadImage")]
        //public ActionResult<ZeroOne.Entity.FileInfo> UploadImage(IFormFileCollection files)
        //{

        //    //图片的根目录
        //    string imgRootPath = this.UploadSettings.SourceImgRootPath;
        //    //string baseImageFilePath =  Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "/Upload/Images/Source/");
        //    string basePhyicalPath = (PlatformServices.Default.Application.ApplicationBasePath + imgRootPath).Replace('\\', '/');
        //    if (!Directory.Exists(basePhyicalPath))
        //    {
        //        Directory.CreateDirectory(basePhyicalPath);
        //    }
        //    foreach (var file in this.Request.Form.Files)
        //    {
        //        var stream = file.OpenReadStream();
        //        //var originImage = Image.FromStream(stream);
        //        var filePath = Path.Combine(basePhyicalPath, file.FileName);
        //        //originImage.Save(filePath);
        //        var bitmap = new Bitmap(stream);
        //        bitmap.Save(filePath);
        //    }
        //    return new ZeroOne.Entity.FileInfo() { Id = Guid.NewGuid() };
        //}

        /// <summary>
        /// 上传图片并且生成缩略图
        /// </summary>
        /// <param name="files">文件对象</param>
        /// <param name="limitSize">图片限制大小，如果原图宽度大于高度，则以宽度缩略；反之，则以高度缩略</param>
        /// <returns></returns>
        [HttpPost("UploadImageAndGenerateThum")]
        public async Task<FileInfoUploadResult> UploadImageAndGenerateThum([FromForm]List<IFormFile> files, [FromForm]int? limitSize)
        {
            if (this.Request.Form.Files == null || this.Request.Form.Files.Count <= 0)
            {
                throw new Exception();
            }
            if (!limitSize.HasValue)
            {
                throw new Exception();
            }
            string currentDirectory = Path.Combine(Directory.GetCurrentDirectory(), @"MyStaticFiles/Upload");
            //图片的根目录
            string imgRootPath = this.UploadSettings.SourceImgRootPath;
            //缩略图的根目录
            string thumRootPath = this.UploadSettings.ThumbnailImgRootPath;
            //string baseImageFilePath =  Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "/Upload/Images/Source/");
            string imgBasePhyicalPath = (currentDirectory + imgRootPath).Replace('\\', '/');
            //生成缩略图地址
            string thumBasePhyicalPath = (currentDirectory + thumRootPath).Replace('\\', '/');
            if (!Directory.Exists(imgBasePhyicalPath))
            {
                Directory.CreateDirectory(imgBasePhyicalPath);
            }
            //判断缩略图目录是否存在
            if (!Directory.Exists(thumBasePhyicalPath))
            {
                Directory.CreateDirectory(thumBasePhyicalPath);
            }
            ZeroOne.Entity.FileInfo fileInfo = null;
            List<ZeroOne.Entity.FileInfo> fileList = new List<ZeroOne.Entity.FileInfo>();
            //上传文件返回对象
            FileInfoUploadResult result = new FileInfoUploadResult();
            //result.SourceFileUrls = new List<string>();
            //result.TargetFileUrls = new List<string>();
            result.FileInfosResult = new List<FileInfoResult>();
            FileInfoResult fileResult = new FileInfoResult();
            Guid uploadId = Guid.NewGuid();
            result.UploadId = uploadId;
            DateTime now = DateTime.Now;
            foreach (var file in this.Request.Form.Files)
            {
                var stream = file.OpenReadStream();
                string fileExt = System.IO.Path.GetExtension(file.FileName);
                string sourceFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + fileExt;
                //var originImage = Image.FromStream(stream);
                var filePath = Path.Combine(imgBasePhyicalPath, sourceFileName);
                //originImage.Save(filePath);
                var bitmap = new Bitmap(stream);
                bitmap.Save(filePath);
                //生成缩略图
                Image thumImg = null;
                if (bitmap.Height > bitmap.Width)
                {
                    thumImg = bitmap.GenerateThumbImage(EThumbnailWay.Height, limitSize.Value);
                }
                else
                {
                    thumImg = bitmap.GenerateThumbImage(EThumbnailWay.Width, width: limitSize.Value);
                }
                string thumFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + fileExt;
                var thumImgFilePath = Path.Combine(thumBasePhyicalPath, thumFileName);
                thumImg.Save(thumImgFilePath);
                //释放图片资源
                thumImg.Dispose();
                bitmap.Dispose();
                //文件对象
                fileInfo = new Entity.FileInfo();
                fileInfo.Id = Guid.NewGuid();
                fileInfo.UploadId = uploadId;
                fileInfo.CreationTime = now;
                fileInfo.ContentType = file.ContentType;
                
                fileInfo.FileExt = fileExt;
                fileInfo.FileName = file.FileName.Replace(fileExt, string.Empty);
                fileInfo.SourceFileUrl = imgRootPath + sourceFileName;
                fileInfo.TargetFileUrl = thumRootPath + thumFileName;
                fileList.Add(fileInfo);
                fileResult = new FileInfoResult();
                fileResult.Id = fileInfo.Id.Value;
                fileResult.Name = fileInfo.FileName;
                fileResult.SourceUrl = imgRootPath + sourceFileName;
                fileResult.Url = thumRootPath + thumFileName;
                result.FileInfosResult.Add(fileResult);
            }
            int affectRows = await this.FileService.AddEntityListAsync(fileList);
            return result;
        }
    }
}