namespace Linky.Client.Services;

using System.Net.Http.Json;
using Models;

public class ClientLinkManager(HttpClient client) : ILinkManager {

	public async Task<LinkCreationResponse> CreateLinkAsync(LinkCreationRequest link) {
		var response = await client.PostAsJsonAsync("link/make", link);
		return (await response.Content.ReadFromJsonAsync<LinkCreationResponse>())!;
	}
}