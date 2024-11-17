namespace Linky.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
public class ShortController(ILogger<ShortController> logger) : ControllerBase {	
	
	
	[HttpGet]
	[Route("{code}")]
	public IResult Get(string code) {
		logger.LogInformation("Received request for code {code}", code);
		
		return Results.NotFound();
	}
	
}