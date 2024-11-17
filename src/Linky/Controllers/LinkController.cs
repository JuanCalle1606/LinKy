namespace Linky.Controllers;

using Client.Services;
using Client.Models;
using Microsoft.AspNetCore.Mvc;

[Route("link")]
public class LinkController(ILinkManager linkManager) {

	[HttpPost]
	[Route("make")]
	public async Task<IResult> CreateLinkAsync([FromBody] LinkCreationRequest request) {
		var response = await linkManager.CreateLinkAsync(request);

		return Results.Ok(response);
	}
}