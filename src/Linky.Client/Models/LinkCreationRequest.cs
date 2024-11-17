namespace Linky.Client.Models;

public class LinkCreationRequest {
	public string Url { get; set; } = string.Empty;

	public string Alias { get; set; } = string.Empty;

	public LinkType Type { get; set; }
}