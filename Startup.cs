// To configure the app to use a distributed Redis cache,
// change the preprocessor directive to 'Redis'.
// For more information, see: 
// https://docs.microsoft.com/aspnet/core/#preprocessor-directives-in-sample-code
#define  SQLServer // Redis

using System;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SampleApp
{
    public class Startup
    {
        private readonly IConfiguration _config;
        private readonly IHostingEnvironment _hostContext;

        public Startup(IConfiguration config, IHostingEnvironment hostContext)
        {
            _config = config;
            _hostContext = hostContext;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            if (_hostContext.IsProduction()) // for testing
            {
                #region snippet_AddDistributedMemoryCache
                services.AddDistributedMemoryCache();
                #endregion
            }
            else
            {
				//services.AddDistributedSqlServerCache(options =>
				//{
				//	options.ConnectionString =
				//		_config["DistCache_ConnectionString"];
				//	options.SchemaName = "dbo";
				//	options.TableName = "TestCache";

				//});

				//services.AddSession(options =>
				//{
				//	// Set a short timeout for easy testing.
				//	options.IdleTimeout = TimeSpan.FromSeconds(10);
				//	options.Cookie.HttpOnly = true;
				//	// Make the session cookie essential
				//	options.Cookie.IsEssential = true;
				//});

				//services.AddDistributedSqlServerCache(options =>
				//{
				//	options.ConnectionString =
				//		_config["DistCache_ConnectionString2"];
				//	options.SchemaName = "dbo";
				//	options.TableName = "TestCache2";

				//});




				// SET DISTRIBUTED CACHE to table TestCache
				//services.AddStackExchangeRedisCache(options =>
				//{
				//	options.Configuration = "localhost";
				//	options.InstanceName = "SampleInstance";
				//});



				// no call to `AddSqlServerCache` as we don’t want to overwrite the `IDistributedCache`
				// registration; instead, register (and configure) the SqlServerCache directly

			
				// -----------------------------------------
				// SET SESSION to table TestCache
				services.AddSingleton<SqlServerCache>();
				services.Configure<SqlServerCacheOptions>(options =>
				{
					options.ConnectionString =
						_config["DistCache_ConnectionString"];
					options.SchemaName = "dbo";
					options.TableName = "TestCache";
				});

				services.AddSession(options =>
				{
					options.IdleTimeout = TimeSpan.FromSeconds(10);
					options.Cookie.HttpOnly = true;
					options.Cookie.IsEssential = true;
				});
				services.AddTransient<ISessionStore, SqlServerSessionStore>();
				// -----------------------------------------


				// -----------------------------------------
				// SET DISTRIBUTED CACHE to table TestCache2
				services.AddSingleton<SqlServerDistributedCache>();
				services.Configure<SqlServerDistributedCacheOptions>(options =>
				{
					options.ConnectionString =
						_config["DistCache_ConnectionString2"];
					options.SchemaName = "dbo";
					options.TableName = "TestCache2";
				});
				services.AddTransient<IDistributedCache, SqlServerDistributedCacheStore>();
				// -----------------------------------------
			}



			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        #region snippet_Configure
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, 
            IApplicationLifetime lifetime, IDistributedCache cache)
        {
            lifetime.ApplicationStarted.Register(() =>
            {
                var currentTimeUTC = DateTime.UtcNow.ToString();
                byte[] encodedCurrentTimeUTC = Encoding.UTF8.GetBytes(currentTimeUTC);
                var options = new DistributedCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(20));
                cache.Set("cachedTimeUTC", encodedCurrentTimeUTC, options);
            });
        #endregion

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
			app.UseSession();
            app.UseMvc();
        }
    }
}
