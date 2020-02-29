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
        /// 登录之后获取有效凭证token
        /// </summary>
        /// <param name="request">登录请求对象</param>
        /// <returns></returns>
        private async Task<object> GetToken(UserLoginRequest request)
        {

            UserInfo model = await this._service.UserLogin(request);

            if (model != null)
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                var key = System.Text.Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);
                //授权时间
                var authTime = DateTime.Now;
                int seconds = _jwtSettings.ExpireSecond;
                //过期时间
                var expiresAt = authTime.AddSeconds(seconds);
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
        [HttpPost("UserLogin")]
        public async Task<object> UserLogin(UserLoginRequest request)
        {
            return await this.GetToken(request);
        }
    }
}