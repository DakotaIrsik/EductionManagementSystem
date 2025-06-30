using Microsoft.AspNetCore.Hosting;

namespace SilverLeaf.Public.Web.Mvc.Extensions
{
	public static class HostingEnvironmentExtensions
	{
		public static bool IsLocal(this IHostingEnvironment env)
		{
			return env.IsEnvironment("Local");
		}
	}
}
