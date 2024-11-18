namespace Linky.Services;

using Client.Models;
using Client.Services;
using Microsoft.EntityFrameworkCore;
using Models;
using SimpleBase;

public class ServerLinkManager(LinkyDbContext db, IHttpContextAccessor context, ILogger<ServerLinkManager> logger) : ILinkManager {

	string BaseUrl => context.HttpContext?.Request.Scheme + "://" + context.HttpContext?.Request.Host.Value;

	public async Task<LinkCreationResponse> CreateLinkAsync(LinkCreationRequest link) {
		// parameter checks
		if (string.IsNullOrWhiteSpace(link.Url)) return Error("Url is required.");
		if (Enum.IsDefined(typeof(LinkType), link.Type) == false) return Error("Invalid link type.");

		//check if the alias is already taken
		if (await db.Aliases.AnyAsync(l => l.Code == link.Alias)) return Error("Alias is already taken.");
		
		// check if the alias is an existing link
		if (await db.Links.AnyAsync(l=>l.Code == link.Alias)) return Error("Alias cannot be the same as an existing link.");

		// try to find exactly the same link
		var existing = await db.Links.Include(i => i.Aliases).FirstOrDefaultAsync(l => l.Url == link.Url && l.Type == link.Type);
		if (existing != null)
		{
			if (!string.IsNullOrWhiteSpace(link.Alias) && existing.Aliases.Add(new(link.Alias)))
			{
				await db.SaveChangesAsync();
			}
			return new() { Link = existing, BaseUrl = BaseUrl };
		}

		// create a new link
		var newLink = new Link
		{
			Url = link.Url,
			Type = link.Type
		};
		// add to database to get the Uid
		db.Links.Add(newLink);
		await db.SaveChangesAsync();

		// generate a code
		newLink.Code = Base58.Ripple.Encode(BitConverter.GetBytes(newLink.Uid));
		
		if (!string.IsNullOrWhiteSpace(link.Alias))
		{
			newLink.Aliases.Add(new(link.Alias));
		}
		await db.SaveChangesAsync();

		return new() { Link = newLink, BaseUrl = BaseUrl };
	}


	LinkCreationResponse Error(string msg) {
		return new() { Error = msg, BaseUrl = BaseUrl };
	}
}