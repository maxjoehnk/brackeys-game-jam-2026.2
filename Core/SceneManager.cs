using System.Collections.Generic;
using System.Linq;
using Godot;

namespace BrackeysGameJam2026.Core;

public partial class SceneManager : Node
{
	public static SceneManager Instance { get; private set; } = null!;
	private Node CurrentScene { get; set; } = null!;

	public AvailableLevel? ActiveLevel { get; private set; }

	public List<AvailableLevel> Levels { get; private set; }

	private Queue<AvailableLevel> CurrentRun { get; set; }

	public override void _Ready()
	{
		Instance = this;
		Viewport root = this.GetTree().Root;
		this.CurrentScene = root.GetChild(-1);
		this.Levels = GetAvailableLevels();
	}

	public void StartRun()
	{
		GameState.Instance.Reset();
		this.CurrentRun = new Queue<AvailableLevel>(this.Levels
			.GroupBy(l => l.Size)
			.SelectMany(g => g.OrderBy(_ => GD.Randi()).Take(5)));
		
		this.OpenNextLevel();
	}

	public void OpenMainMenu()
	{
		this.LoadScene("res://UI/MainMenu.tscn");
	}

	public void OpenSettings()
	{
		this.LoadScene("res://UI/Settings.tscn");
	}

	public void OpenNextLevel()
	{
		if (this.CurrentRun.TryDequeue(out AvailableLevel? level))
		{
			this.OpenLevel(level);
		}
	}

	private void OpenLevel(AvailableLevel level)
	{
		this.LoadScene($"res://Levels/{level.Path}");
		this.ActiveLevel = level;
	}

	public void RestartLevel()
	{
		if (this.ActiveLevel == null)
		{
			return;
		}

		this.OpenLevel(this.ActiveLevel);
	}

	private static List<AvailableLevel> GetAvailableLevels()
	{
		List<AvailableLevel> levels = ResourceLoader.ListDirectory("res://Levels")
			.Where(name => name.EndsWith(".tscn"))
			.Where(name => !name.StartsWith("_"))
			.Select(file => new AvailableLevel(file))
			.ToList();

		return levels;
	}

	private void LoadScene(string path)
	{
		this.CurrentScene.QueueFree();
		this.ActiveLevel = null;
		PackedScene scene = GD.Load<PackedScene>(path);
		this.CurrentScene = scene.Instantiate();

		this.GetTree().Root.AddChild(this.CurrentScene);
		this.GetTree().CurrentScene = this.CurrentScene;
		this.GetTree().Paused = false;
	}
}