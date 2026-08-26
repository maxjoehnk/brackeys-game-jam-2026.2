using Godot;

public partial class EyeOverlay : Control
{
	private Control Up => this.GetNode<Control>("Up");
	private Control Down => this.GetNode<Control>("Down");

	private const float Duration = 1f;
	private const Tween.TransitionType TransitionType = Tween.TransitionType.Back;

	[Signal]
	public delegate void EyesOpeningEventHandler();

	[Signal]
	public delegate void EyesOpenedEventHandler();

	[Signal]
	public delegate void EyesClosedEventHandler();
	
	public void Open()
	{
		float height = this.GetViewportRect().Size.Y;
		
		Tween tween = this.CreateTween();
		tween.Parallel().TweenProperty(this.Up, "position", new Vector2(0, -1 * height), Duration).SetTrans(TransitionType);
		tween.Parallel().TweenProperty(this.Down, "position", new Vector2(0, height), Duration).SetTrans(TransitionType);
		
		tween.Finished += this.EmitSignalEyesOpened;
		
		tween.Play();
		
		this.EmitSignalEyesOpening();
	}

	public void Close()
	{
		float height = this.GetViewportRect().Size.Y / 2;
		Tween tween = this.CreateTween();
		tween.Parallel().TweenProperty(this.Up, "position", new Vector2(0, -1 * height), Duration).SetTrans(TransitionType);
		tween.Parallel().TweenProperty(this.Down, "position", new Vector2(0, 1 * height), Duration).SetTrans(TransitionType);
		
		tween.Finished += this.EmitSignalEyesClosed;
		
		tween.Play();
	}
}
