using Godot;
using System;

public partial class Item : RigidBody2D
{
	private Sprite2D Sprite => this.GetNode<Sprite2D>("Sprite2D");
	
	public void Break()
	{
		this.Sprite.RegionRect = new Rect2(256, 0, 256, 256);
	}
}
