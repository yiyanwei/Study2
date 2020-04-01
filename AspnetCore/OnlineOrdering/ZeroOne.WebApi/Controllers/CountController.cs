using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ZeroOne.WebApi.Hubs;

namespace ZeroOne.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountController : Controller
    {
        private readonly IHubContext<CountHub> _countHub;
        public CountController(IHubContext<CountHub> countHub)
        {
            this._countHub = countHub;
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            await _countHub.Clients.All.SendAsync("someFunc", new { random = "abcd" });
            //202：请求已被接受并处理，但是还没有处理完成
            return Accepted(1);
        }
    }
}