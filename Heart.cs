using Godot;
using System;

public partial class Heart : TextureRect
{
	private AnimatedSprite2D Animation => this.GetNode<AnimatedSprite2D>("Pop");

	private Texture2D emptyHeart = GD.Load<Texture2D>("res://Assets/heart-empty-export.png");

	public void Pop()
	{
		this.Animation.Visible = true;
		this.Animation.Play();
		this.Animation.AnimationFinished += () =>
		{
			this.Animation.Visible = false;
			this.Texture = this.emptyHeart;
		};
	}
}