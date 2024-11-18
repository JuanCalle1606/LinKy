namespace Linky.Models;

using Client.Models;
using Microsoft.EntityFrameworkCore;

public class LinkyDbContext(DbContextOptions<LinkyDbContext> options) : DbContext(options) {

	public DbSet<Link> Links { get; set; }

	public DbSet<Link.Alias> Aliases { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder) {
		modelBuilder.Entity<Link>().HasKey(l => l.Uid);
		modelBuilder.Entity<Link>().HasIndex(l => new
		{
			l.Url,
			l.Type
		}).IsUnique();

		modelBuilder.Entity<Link.Alias>().HasKey(a => a.Code);
		modelBuilder.Entity<Link.Alias>().Property(p=>p.Code)
			.HasMaxLength(22);
		modelBuilder.Entity<Link.Alias>().Property("LinkUid").IsRequired();
	}

}