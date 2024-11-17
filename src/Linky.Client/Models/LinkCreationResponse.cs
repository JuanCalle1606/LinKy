namespace Linky.Client.Models;

public class LinkCreationResponse {
	public Link? Link { get; set; }

	public string Error { get; set; } = string.Empty;

	public bool Success => Link != null;
}