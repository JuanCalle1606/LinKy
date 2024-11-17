namespace Linky.Client.Pages;

using Models;

public partial class Home {
	bool _busy;
	
	string? _error;
	
	Link? _shortenedLink;
	
	string _baseUrl = string.Empty;

	readonly LinkCreationRequest _link = new()
	{
		Url = string.Empty,
		Alias = string.Empty,
		Type = LinkType.Redirect
	};

	public string Link => _baseUrl + "/" + _shortenedLink?.Code;

	async Task CreateLink() {
		_error = null;
		_shortenedLink = null;
		_busy = true;
		var response = await LinkManager.CreateLinkAsync(_link);
		_baseUrl = response.BaseUrl;
		if (response.Success) {
			_shortenedLink = response.Link;
			_error = null;
		} else {
			_error = response.Error + " " + response.BaseUrl;
		}
		_busy = false;
	}
}