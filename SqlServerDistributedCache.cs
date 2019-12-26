using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.SqlServer;
using Microsoft.Extensions.Options;

namespace SampleApp
{
		
		public class SqlServerDistributedCache : IDistributedCache
		{
			private SqlServerCache sqlServerCache;
			public SqlServerDistributedCache(IOptions<SqlServerDistributedCacheOptions> options) // this is the key point, we are using another instance of SqlServerOptions
			{
				sqlServerCache = new SqlServerCache(options);
			}

		public byte[] Get(string key)
		{
			return sqlServerCache.Get(key);
		}

		public Task<byte[]> GetAsync(string key, CancellationToken token = default(CancellationToken))
		{
			return sqlServerCache.GetAsync(key, token);
		}

		public void Refresh(string key)
		{
			sqlServerCache.Refresh(key);
		}

		public Task RefreshAsync(string key, CancellationToken token = default(CancellationToken))
		{
			return sqlServerCache.RefreshAsync(key, token);
		}

		public void Remove(string key)
		{
			sqlServerCache.Remove(key);
		}

		public Task RemoveAsync(string key, CancellationToken token = default(CancellationToken))
		{
			return sqlServerCache.RemoveAsync(key, token);
		}

		public void Set(string key, byte[] value, DistributedCacheEntryOptions options)
		{
			sqlServerCache.Set(key, value, options);
		}

		public Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options, CancellationToken token = default(CancellationToken))
		{
			return sqlServerCache.SetAsync(key, value, options, token);
		}
	}
}
