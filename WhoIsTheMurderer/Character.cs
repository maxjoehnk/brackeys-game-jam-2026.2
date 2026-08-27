using Godot;

[Tool]
public partial class Character : StaticBody2D
{
	[Export] public string Hint { get; set; }
	[Export] public bool Murderer { get; set; }
	
	[Signal]
	public delegate void ClickEventHandler(string hint);

	public Sprite2D Asset => this.GetNode<Sprite2D>("Sprite2D");

	[Export]
	public Texture2D Texture
	{
		get => this.Asset.Texture;
		set => this.Asset.Texture = value;
	}

	public override void _InputEvent(Viewport viewport, InputEvent @event, int shapeIdx)
	{
		if (@event is InputEventMouseButton { Pressed: true })
		{
			this.EmitSignalClick(this.Hint);
		}
	}
}
