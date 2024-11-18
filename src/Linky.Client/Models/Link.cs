namespace Linky.Client.Models;

using System.ComponentModel.DataAnnotations;

public class Link {

	public uint Uid { get; set; }

	[MaxLength(2048)]
	public required string Url { get; set; }

	[MaxLength(32)]
	public string Code { get; set; } = string.Empty;

	public LinkType Type { get; set; }

	public HashSet<Alias> Aliases { get; set; } = [];

	public record Alias(string Code);
}