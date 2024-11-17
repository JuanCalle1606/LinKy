namespace Linky.Client.Services;

using Models;

public interface ILinkManager {
	public Task<LinkCreationResponse> CreateLinkAsync(LinkCreationRequest link);		
}