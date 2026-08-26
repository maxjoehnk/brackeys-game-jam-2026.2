using Godot;

namespace BrackeysGameJam2026.Empty.Jobs;

public partial class RandomMoveAroundTask : Task
{
	private Side direction;

	private Vector2 MovementVector => this.direction switch
	{
		Side.Left => new Vector2(-1, 0),
		Side.Right => new Vector2(1, 0),
		Side.Top => new Vector2(0, -1),
		Side.Bottom => new Vector2(0, 1),
		_ => new Vector2()
	};

	public RandomMoveAroundTask()
	{
		this.ChangeDirection();
	}
	
	public override bool Do(Mob mob)
	{
		if (GD.Randf() > 0.9f)
		{
			this.ChangeDirection();
		}

		GD.Print("Moving " + this.direction);
		mob.MoveAndCollide(this.MovementVector);

		return false;
	}

	private void ChangeDirection()
	{
		this.direction = (Side)GD.RandRange(0, 3);
	}
}