using System.Text.RegularExpressions;
using Godot;

namespace BrackeysGameJam2026.Core;

public partial class AvailableLevel
{
	public string Path { get; }

	public Vector2I Size { get; }
	
	public int Rows { get; }
	public int Columns { get; }

	public AvailableLevel(string path)
	{
		this.Path = path;
		Match match = LevelNameRegex().Match(path);
		string[] size = match.Groups["Size"].Value.Split("x");
		this.Columns = int.Parse(size[0]);
		this.Rows = int.Parse(size[1]);
		this.Size = new Vector2I(this.Columns, this.Rows);
	}

	[GeneratedRegex("((?<Id>[0-9]+)_)?_(?<Size>[0-9]x[0-9]).tscn",
		RegexOptions.Compiled | RegexOptions.IgnoreCase)]
	private static partial Regex LevelNameRegex();

	public override string ToString()
	{
		return $"{nameof(this.Path)}: {this.Path}, {nameof(this.Size)}: {this.Size}";
	}
}