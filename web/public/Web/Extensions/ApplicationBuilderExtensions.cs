using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace SilverLeaf.Public.Web.Mvc.Extensions
{
	public static class ApplicationBuilderExtensions
	{
		public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsLocal() || env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
			}

			return app;
		}
	}
}
