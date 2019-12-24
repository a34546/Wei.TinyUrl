using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Wei.TinyUrl.Data.Repositories;

namespace Wei.TinyUrl.Api.Controllers
{
    [Route("/")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        readonly IUrlMappingRepository _urlMappingRepository;
        readonly string _serverUrl;
        public HomeController(IUrlMappingRepository urlMappingRepository,
            IConfiguration configuration)
        {
            _serverUrl = configuration.GetSection("ServerUrl").Value;
            _urlMappingRepository = urlMappingRepository;
        }

        [HttpGet()]
        public string Hello()
        {
            return $@"
                    领健短网址平台
                    生成短网址使用示例：
                    GET：{_serverUrl}/api/create?url=http://www.baidu.com&key=123456789
                    返回结果：{_serverUrl}/WPysCS
                    测试key:123456789，有效期为1天
                    ";
        }

        /// <summary>
        /// 生成短网址
        /// </summary>
        /// <param name="url">需要生成的长网址</param>
        /// <param name="key">密匙</param>
        /// <param name="monthCount">有效多少个月，为null,为永久</param>
        /// <returns></returns>
        [HttpGet("api/create")]
        public async Task<IActionResult> GenerateTinyUrl(string url, string key)
        {
            if (string.IsNullOrEmpty(key)) return BadRequest("key is not null");
            if (string.IsNullOrEmpty(url)) return BadRequest("url is not null");
            if (!url.Contains(".")) return BadRequest("url is error");
            try
            {
                var code = await _urlMappingRepository.GenerateTinyUrl(url, key);
                return Ok($"{_serverUrl}/{code}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}