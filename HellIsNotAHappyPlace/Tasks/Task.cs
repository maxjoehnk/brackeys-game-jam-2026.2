using Godot;

namespace BrackeysGameJam2026.Empty.Jobs;

public abstract partial class Task : Node
{
	public virtual void Activate(Mob mob)
	{
	}
	
	public abstract bool Do(Mob mob);

	public virtual void Stop(Mob mob)
	{
	}
}