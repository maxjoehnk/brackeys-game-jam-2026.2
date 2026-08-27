using System.Linq;
using Godot;
using Godot.Collections;
using Task = BrackeysGameJam2026.Empty.Jobs.Task;

public partial class Mob : CharacterBody2D
{
	private const float Speed = 100f;
	
	[Export] public Array<string> Quotes { get; set; } = [];

	[Export] public Array<Task> Tasks { get; set; } = [];
	
	private Task CurrentTask => this.Tasks.FirstOrDefault();
	
	private PanelContainer SpeechBubble => this.GetNode<PanelContainer>("SpeechBubble");
	private Label SpeechBubbleText => this.GetNode<Label>("SpeechBubble/Label");
	
	private Area2D HitDetector => this.GetNode<Area2D>("HitDetector");
	
	private NavigationAgent2D Navigation => this.GetNode<NavigationAgent2D>("NavigationAgent2D");

	private bool isActive;

	public override void _Ready()
	{
		this.SpeechBubble.Visible = false;
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);

		if (this.isActive)
		{
			this.DoTask();
		}

		if (this.Navigation.IsNavigationFinished() || !this.isActive)
		{
			return;
		}
		
		Vector2 nextPathPosition = this.Navigation.GetNextPathPosition();
		Vector2 currentPosition = this.GlobalTransform.Origin;

		this.Velocity = currentPosition.DirectionTo(nextPathPosition) * Speed;
		this.MoveAndSlide();
	}

	public void BlendIn()
	{
		this.isActive = false;
		this.CurrentTask?.Stop(this);
	}

	public void GetActive()
	{
		this.isActive = true;
		this.CurrentTask?.Activate(this);
	}

	public void NavigateTo(Vector2 target)
	{
		this.Navigation.TargetPosition = target;
	}

	public void Interact()
	{
		this.SpeechBubbleText.Text = this.Quotes[(int)(GD.Randi() % this.Quotes.Count)];
		this.SpeechBubble.Visible = true;
		
		SceneTreeTimer timer = this.GetTree().CreateTimer(3, processAlways:false);
		timer.Timeout += () => this.SpeechBubble.Visible = false;
	}

	private void DoTask()
	{
		if (this.CurrentTask?.Do(this) ?? false)
		{
			this.Tasks.Remove(this.CurrentTask);
		}
	}

	public bool Reaches(Item item)
	{
		return this.HitDetector.OverlapsBody(item);
	}
}
