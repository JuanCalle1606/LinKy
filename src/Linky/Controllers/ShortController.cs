namespace Linky.Controllers;

using Client.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

[ApiController]
public class ShortController(ILogger<ShortController> logger, LinkyDbContext db, HttpClient client) : ControllerBase {
	[HttpGet]
	[Route("{code}")]
	public async Task<IResult> Get(string code) {
		if (string.IsNullOrWhiteSpace(code))
		{
			logger.LogWarning("Received empty or null code");
			return Results.BadRequest("Code cannot be null or empty.");
		}

		logger.LogInformation("Received request for code {code}", code);

		var link = await db.Links.FirstOrDefaultAsync(l => l.Code == code);
		if (link != null)
		{
			logger.LogInformation("Link with code {code} found", code);
			return await ProvideLink(link);
		}

		logger.LogWarning("Link with code {code} not found, looking for alias", code);
		var alias = await db.Aliases.Include(a => a.Link).FirstOrDefaultAsync(a => a.Code == code);
		if (alias == null) return Results.NotFound();

		logger.LogInformation("Alias with code {code} found", code);
		return await ProvideLink(alias.Link);
	}

	async Task<IResult> ProvideLink(Link link) {
		if (link.Type == LinkType.Redirect)
		{
			return Results.Redirect(link.Url, true);
		}

		HttpResponseMessage response;
		try
		{
			response = await client.GetAsync(link.Url, HttpCompletionOption.ResponseHeadersRead);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Error fetching link {url}", link.Url);
			return Results.Problem($"Error fetching link: {ex.Message}");
		}

		if (!response.IsSuccessStatusCode)
		{
			logger.LogWarning("Failed to fetch link {url} with status code {statusCode}", link.Url, response.StatusCode);
			return Results.StatusCode((int)response.StatusCode);
		}

		return new ServeResponse(response);
	}

	class ServeResponse(HttpResponseMessage response) : IResult {
		public async Task ExecuteAsync(HttpContext context) {
			try
			{
				context.Response.StatusCode = (int)response.StatusCode;

				var excludedHeaders = new[] { "Transfer-Encoding", "Connection" };

				foreach (var header in response.Headers)
				{
					if (!excludedHeaders.Contains(header.Key, StringComparer.OrdinalIgnoreCase) && !context.Response.Headers.ContainsKey(header.Key))
					{
						context.Response.Headers.Append(header.Key, header.Value.ToArray());
					}
				}

				foreach (var header in response.Content.Headers)
				{
					if (!excludedHeaders.Contains(header.Key, StringComparer.OrdinalIgnoreCase) && !context.Response.Headers.ContainsKey(header.Key))
					{
						context.Response.Headers.Append(header.Key, header.Value.ToArray());
					}
				}

				context.Response.ContentType = response.Content.Headers.ContentType?.ToString();
				// Hacer streaming directo desde el cuerpo de la respuesta del HttpClient.
				await using var sourceStream = await response.Content.ReadAsStreamAsync();
				await sourceStream.CopyToAsync(context.Response.Body);
			}
			catch (Exception ex)
			{
				context.Response.StatusCode = StatusCodes.Status500InternalServerError;
				await context.Response.WriteAsync($"Error processing response: {ex.Message}");
			}
		}
	}
}