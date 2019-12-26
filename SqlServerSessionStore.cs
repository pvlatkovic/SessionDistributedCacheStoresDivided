using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Session;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.SqlServer;
using Microsoft.Extensions.Logging;

namespace SampleApp
{
	public class SqlServerSessionStore : ISessionStore
	{
		private readonly IDistributedCache _cache;
		private readonly ILoggerFactory _loggerFactory;

		public SqlServerSessionStore(SqlServerCache cache, ILoggerFactory loggerFactory)
		{
			_cache = cache ?? throw new ArgumentNullException(nameof(cache));
			_loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
		}


		public ISession Create(string sessionKey, TimeSpan idleTimeout, TimeSpan ioTimeout, Func<bool> tryEstablishSession, bool isNewSessionKey)
		{
			if (string.IsNullOrEmpty(sessionKey))
				throw new ArgumentNullException(nameof(sessionKey));
			if (tryEstablishSession == null)
				throw new ArgumentNullException(nameof(tryEstablishSession));

			return new DistributedSession(_cache, sessionKey, idleTimeout, ioTimeout, tryEstablishSession, _loggerFactory, isNewSessionKey);
		}
	}
}
