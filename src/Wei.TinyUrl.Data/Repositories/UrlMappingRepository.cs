using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wei.Repository;
using Wei.TinyUrl.Data.Entities;

namespace Wei.TinyUrl.Data.Repositories
{
    public class UrlMappingRepository : Repository<UrlMapping>, IUrlMappingRepository
    {
        readonly IMemoryCache _cache;
        readonly IUnitOfWork _unitOfWork;
        public UrlMappingRepository(TinyUrlDbContext context,
            IUnitOfWork unitOfWork,
            IMemoryCache cache) : base(context)
        {
            _cache = cache;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> GenerateTinyUrl(string url, string key)
        {
            if (!_cache.TryGetValue(key, out Client client)) throw new Exception(" key is error");
            if (await QueryNoTracking.AnyAsync(x => x.Url.Equals(url, StringComparison.CurrentCultureIgnoreCase))) return (await QueryNoTracking.FirstOrDefaultAsync(x => x.Url == url))?.Code;
            var code = Utils.GenerateCode(6);
            while (!await IsUnique(code))
            {
                code = Utils.GenerateCode(6);
            }
            var entity = new UrlMapping { Code = code, Url = url, Source = client.Name };
            var now = DateTime.Now;

            if (client.Days.HasValue) entity.ExpiryTime = now.AddDays(client.Days.Value);
            await InsertAsync(entity);
            //清空已过期的链接
            HardDelete(x => x.ExpiryTime.HasValue && x.ExpiryTime.Value < now);
            await _unitOfWork.SaveChangesAsync();
            return entity.Code;
        }

        public async Task<string> GetUrlByCode(string code)
        {
            var now = DateTime.Now;
            return (await QueryNoTracking.Where(x => x.IsDelete == false).OrderByDescending(x => x.Id).FirstOrDefaultAsync(x => x.Code == code && (!x.ExpiryTime.HasValue || x.ExpiryTime.HasValue && x.ExpiryTime.Value > now)))?.Url;
        }

        private async Task<bool> IsUnique(string code)
        {
            var now = DateTime.Now;
            if (await QueryNoTracking.AnyAsync(x => x.Code == code && x.IsDelete == false && (!x.ExpiryTime.HasValue || x.ExpiryTime.HasValue && x.ExpiryTime.Value > now))) return false;
            return true;
        }
    }

    public interface IUrlMappingRepository : IRepository<UrlMapping>
    {
        /// <summary>
        /// 生成短链接
        /// </summary>
        Task<string> GenerateTinyUrl(string url, string key);

        /// <summary>
        /// 根据Code获取长链接
        /// </summary>
        Task<string> GetUrlByCode(string code);
    }
}
