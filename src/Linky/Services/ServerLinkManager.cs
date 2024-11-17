namespace Linky.Services;

using Client.Models;
using Client.Services;
using Microsoft.EntityFrameworkCore;
using Models;

public class ServerLinkManager(LinkyDbContext db, IHttpContextAccessor  context) : ILinkManager {

	string BaseUrl =>  context.HttpContext?.Request.Scheme + "://" + context.HttpContext?.Request.Host.Value;
	
	public async Task<LinkCreationResponse> CreateLinkAsync(LinkCreationRequest link) {
		// parameter checks
		if (string.IsNullOrWhiteSpace(link.Url)) return Error("Url is required.");
		if (Enum.IsDefined(typeof(LinkType), link.Type) == false) return Error("Invalid link type.");
		
		// try to find exactly the same link
		var existing = await db.Links.FirstOrDefaultAsync(l => l.Code == link.Alias && l.Url == link.Url && l.Type == link.Type);
		if (existing != null) return new() { Link = existing, BaseUrl = BaseUrl };
		

		return Error("Failed to create link.");
	}

	LinkCreationResponse Error(string msg) {
		return new() { Error = msg, BaseUrl = BaseUrl };
	}
}