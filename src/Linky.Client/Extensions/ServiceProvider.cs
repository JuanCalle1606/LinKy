namespace Linky.Client.Extensions;

using Radzen;

public static class ServiceProvider {
	public static IServiceCollection AddSharedServices(this IServiceCollection services) {

		services.AddRadzenComponents();
		
		return services;
	}
}