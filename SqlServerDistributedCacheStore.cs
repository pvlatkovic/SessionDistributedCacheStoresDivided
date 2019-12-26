using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace SampleApp
{
	public class SqlServerDistributedCacheStore : IDistributedCache  //depends on SqlServerDistributedCache
	{
		private SqlServerDistributedCache _cache;
		public SqlServerDistributedCacheStore(SqlServerDistributedCache cache)
		{
			_cache = cache;
		}

		public byte[] Get(string key)
		{
			return _cache.Get(key);
		}

		public Task<byte[]> GetAsync(string key, CancellationToken token = default(CancellationToken))
		{
			return _cache.GetAsync(key, token);
		}

		public void Refresh(string key)
		{
			_cache.Refresh(key);
		}

		public Task RefreshAsync(string key, CancellationToken token = default(CancellationToken))
		{
			return _cache.RefreshAsync(key, token);
		}

		public void Remove(string key)
		{
			_cache.Remove(key);
		}

		public Task RemoveAsync(string key, CancellationToken token = default(CancellationToken))
		{
			return _cache.RemoveAsync(key, token);
		}

		public void Set(string key, byte[] value, DistributedCacheEntryOptions options)
		{
			_cache.Set(key, value, options);
		}

		public Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options, CancellationToken token = default(CancellationToken))
		{
			return _cache.SetAsync(key, value, options, token);
		}
	}
}
