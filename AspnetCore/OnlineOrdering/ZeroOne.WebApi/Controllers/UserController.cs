using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using ZeroOne.Entity;
using ZeroOne.Application;

namespace ZeroOne.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private JwtSettings _jwtSettings;
        private IUserInfoService _service;
        public UserController(IOptions<JwtSettings> jwtSettings, IUserInfoService service)
        {
            this._jwtSettings = jwtSettings.Value;
            this._service = service;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userAccount"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        private async Task<object> GetToken(string userAccount, string pwd)
        {

            User_Info model = await this._service.GetUserByAccount(userAccount, pwd);

            if (model != null)
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                var key = System.Text.Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);
                var authTime = DateTime.Now;//授权时间
                var expiresAt = authTime.AddDays(30);//过期时间
                var tokenDescripor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(JwtClaimTypes.Audience,_jwtSettings.Audience),
                    new Claim(JwtClaimTypes.Issuer,_jwtSettings.Issuer),
                    new Claim(JwtClaimTypes.Name, model.Name),
                    new Claim(JwtClaimTypes.Id, model.Id.ToString()),
                    new Claim(JwtClaimTypes.PhoneNumber, model.MobileNum)
                }),
                    Expires = expiresAt,
                    //对称秘钥SymmetricSecurityKey
                    //签名证书(秘钥，加密算法)SecurityAlgorithms
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescripor);
                var tokenString = tokenHandler.WriteToken(token);
                var result = new
                {
                    access_token = tokenString,
                    token_type = "Bearer",
                    profile = new
                    {
                        id = model.Id.ToString(),
                        name = model.Name,
                        phone = model.MobileNum,
                        auth_time = authTime,
                        expires_at = expiresAt
                    }
                };
                return result;
            }
            return null;
        }

        [AllowAnonymous]
        [HttpGet("UserLogin")]
        public async Task<ActionResult<object>> UserLogin(string account, string pwd)
        {
            var token = await this.GetToken(account, pwd);
            if (token == null)
            {
                return NoContent();
            }
            return token;
        }
    }
}