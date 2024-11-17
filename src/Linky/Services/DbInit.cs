namespace Linky.Services;

using Models;
using Microsoft.EntityFrameworkCore;

public class DbInit(IServiceScopeFactory scopeFactory) : BackgroundService {

	protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
		using var scope = scopeFactory.CreateScope();
		var db = scope.ServiceProvider.GetRequiredService<LinkyDbContext>();

		await db.Database.MigrateAsync(cancellationToken: stoppingToken);
	}
}