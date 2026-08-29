using System.Text.RegularExpressions;
using Godot;

namespace BrackeysGameJam2026.Core;

public partial class AvailableLevel
{
	public string Path { get; }

	public string Name { get; }

	public Vector2I Size =>  new(this.Columns, this.Rows);
	
	public int Rows { get; }
	public int Columns { get; }

	public AvailableLevel(string path)
	{
		this.Path = path;
		Match match = LevelNameRegex().Match(path);
		this.Name = match.Groups["Name"].Value;
		if (match.Groups["Size"].Success)
		{
			string[] size = match.Groups["Size"].Value.Substring(1).Split("x");
			this.Columns = int.Parse(size[0]);
			this.Rows = int.Parse(size[1]);
		}
	}

	[GeneratedRegex("((?<Id>[0-9]+)_)?(?<Name>[^_]*)(?<Size>_[0-9]x[0-9])?.tscn",
		RegexOptions.Compiled | RegexOptions.IgnoreCase)]
	private static partial Regex LevelNameRegex();
}