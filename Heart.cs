using Godot;

namespace BrackeysGameJam2026;

[Tool]
public partial class Heart : TextureRect
{
	private AnimatedSprite2D Animation => this.GetNode<AnimatedSprite2D>("Pop");

	private Texture2D fullHeart = GD.Load<Texture2D>("res://Assets/heart.png");
	private Texture2D emptyHeart = GD.Load<Texture2D>("res://Assets/heart-empty-export.png");

	public override void _Ready()
	{
		this.Texture = this.fullHeart;
	}

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

	public void SetFull()
	{
		this.Texture = this.fullHeart;
	}

	public void SetEmpty()
	{
		this.Texture = this.emptyHeart;
	}
}