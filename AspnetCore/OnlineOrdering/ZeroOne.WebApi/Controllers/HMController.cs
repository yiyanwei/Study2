using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ZeroOne.WebApi.Controllers
{
    public class ImageItem
    {
        public int Id { get; set; }

        public string ImageUrl { get; set; }

        public string LinkUrl { get; set; }
    }

    [Route("api/[controller]")]
    public class HMController : Controller
    {
        [HttpGet("GetImageItems")]
        public async Task<List<ImageItem>> GetImageItems()
        {
            List<ImageItem> imageItems = new List<ImageItem>();
            ImageItem item = new ImageItem() { Id = 1, ImageUrl = "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1594218924914&di=70a8515e63e759222f887418723ce5c6&imgtype=0&src=http%3A%2F%2Fe.hiphotos.baidu.com%2Fbainuo%2Fwh%3D720%2C436%2Fsign%3De9953f407f310a55c471d6f385756f9d%2Fa71ea8d3fd1f413489a9a0cd231f95cad0c85ee2.jpg", LinkUrl = "https://www.baidu.com" };
            imageItems.Add(item);
            item = new ImageItem() { Id = 2, ImageUrl = "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1594218924911&di=79075a256f2936ab3c8847038e7c46e0&imgtype=0&src=http%3A%2F%2Fwww.divoer.com%2FUpFile%2F202002%2F2020021043184417.jpg", LinkUrl = "https://www.baidu.com" };
            imageItems.Add(item);
            item = new ImageItem() { Id = 3, ImageUrl = "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1594218924944&di=cf0976e27665b621b0de9113cf53af8b&imgtype=0&src=http%3A%2F%2Fwww.pcpip.cn%2Ffile%2Fupload%2F201912%2F27%2F145405691.jpg", LinkUrl = "https://www.baidu.com" };
            imageItems.Add(item);
            return await Task.FromResult(imageItems);
        }
    }
}
