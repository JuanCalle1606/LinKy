namespace Linky.Client.Models;

using System.ComponentModel.DataAnnotations;

public class Link {
	
	public UInt128 Uid { get; set; }
	
	[MaxLength(2048)]
	public required string Url { get; set; }
	
	[MaxLength(32)]
	public required string Code { get; set; }
	
	public LinkType Type { get; set; }
}