using Godot;

namespace BrackeysGameJam2026.Empty.Jobs;

public partial class DemolitionTask : Task
{
	public override bool Do(Mob mob)
	{
		Item item = this.GetNode<Item>("/root/Node2D/Item");
		if (!mob.Reaches(item))
		{
			Vector2 movementVector = mob.Position.DirectionTo(item.Position);
			mob.MoveAndCollide(movementVector);
			return false;
		}

		item.Break();
		return true;
	}
}