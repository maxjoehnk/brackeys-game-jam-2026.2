using Godot;
using Godot.Collections;
using Task = BrackeysGameJam2026.Empty.Jobs.Task;

public partial class Mob : CharacterBody2D
{
	[Export] public Array<string> Quotes { get; set; } = [];

	[Export] public Array<Task> Tasks { get; set; } = [];
	
	private Task CurrentTask => this.Tasks[0];
	
	private PanelContainer SpeechBubble => this.GetNode<PanelContainer>("SpeechBubble");
	private Label SpeechBubbleText => this.GetNode<Label>("SpeechBubble/Label");
	
	private Area2D HitDetector => this.GetNode<Area2D>("HitDetector");

	public void Interact()
	{
		this.SpeechBubbleText.Text = this.Quotes[(int)(GD.Randi() % this.Quotes.Count)];
		this.SpeechBubble.Visible = true;
		
		SceneTreeTimer timer = this.GetTree().CreateTimer(3, processAlways:false);
		timer.Timeout += () => this.SpeechBubble.Visible = false;
	}

	public void DoTask()
	{
		if (this.CurrentTask.Do(this))
		{
			this.Tasks.Remove(this.CurrentTask);
		}
	}

	public bool Reaches(Item item)
	{
		return this.HitDetector.OverlapsBody(item);
	}
}
