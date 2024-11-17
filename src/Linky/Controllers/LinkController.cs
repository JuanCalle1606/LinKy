namespace Linky.Controllers;

using Client.Services;
using Client.Models;
using Microsoft.AspNetCore.Mvc;

[Route("link")]
public class LinkController(ILinkManager linkManager) {

	[HttpPost]
	[Route("make")]
	public async Task<LinkCreationResponse> CreateLinkAsync([FromBody] LinkCreationRequest request) => await linkManager.CreateLinkAsync(request);
}