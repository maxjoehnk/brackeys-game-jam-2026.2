using Godot;

namespace BrackeysGameJam2026.Empty.Jobs;

public abstract partial class Task : Node
{
	public abstract bool Do(Mob mob);
}