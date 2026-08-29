using Godot;

namespace BrackeysGameJam2026;

public partial class GameState : Node
{
	public static GameState Instance { get; private set; } = null!;

	public override void _Ready()
	{
		Instance = this;
	}

	[Export] public int TotalLives { get; set; } = 3;
	public int Lives { get; set; } = 3;

	[Signal]
	public delegate void LifeLostEventHandler();

	[Signal]
	public delegate void LostEventHandler();

	public void Fail()
	{
		this.Lives--;
		this.EmitSignalLifeLost();
		if (this.Lives <= 0)
		{
			this.EmitSignalLost();
		}
	}

	public void Reset()
	{
		this.Lives = this.TotalLives;
	}
}