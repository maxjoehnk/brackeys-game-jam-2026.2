namespace BrackeysGameJam2026.Empty.Jobs;

public partial class DemolitionTask : Task
{
	public override void Activate(Mob mob)
	{
		Item item = this.GetNode<Item>("/root/Node2D/Item");
		mob.NavigateTo(item.GlobalTransform.Origin);
	}

	public override bool Do(Mob mob)
	{
		Item item = this.GetNode<Item>("/root/Node2D/Item");
		if (!mob.Reaches(item))
		{
			return false;
		}

		item.Break();
		return true;
	}
}