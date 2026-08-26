using System.Linq;
using Godot;
using Godot.Collections;

public partial class Prototype : Node2D
{
	private const int MobCount = 2;
	
	private bool EyesClosed { get; set; }
	
	private bool EyesActuallyClosed { get; set; }

	private CharacterBody2D Player => this.GetNode<CharacterBody2D>("Player");
	private Area2D HitDetector => this.GetNode<Area2D>("Player/HitDetector");

	private Array<Mob> Characters => [.. this.GetNode<Node>("Characters").GetChildren().Cast<Mob>()];

	private EyeOverlay EyeOverlay => this.GetNode<EyeOverlay>("Container");

	public override void _Ready()
	{
		this.EyeOverlay.EyesOpening += () => this.EyesActuallyClosed = false;
		this.EyeOverlay.EyesClosed += () => this.EyesActuallyClosed = true;
		this.EyeOverlay.Visible = true;
	}

	public override void _Process(double delta)
	{
		this.HandleInteraction();
		this.HandleToggleEyes();
	}

	public override void _PhysicsProcess(double delta)
	{
		this.ExecuteCharacterTasks();
		this.MovePlayer();
	}

	private void MovePlayer()
	{
		float horizontal = Input.GetAxis("left", "right");
		float vertical = Input.GetAxis("up", "down");

		this.Player.MoveAndCollide(new Vector2(horizontal, vertical));
	}

	private void HandleInteraction()
	{
		if (Input.IsActionJustPressed("interact"))
		{
			foreach (Mob character in this.Characters)
			{
				if (this.HitDetector.OverlapsBody(character))
				{
					character.Interact();
				}
			}
		}
	}

	private void HandleToggleEyes()
	{
		if (Input.IsActionJustPressed("toggle_eyes"))
		{
			this.EyesClosed = !this.EyesClosed;
			if (this.EyesClosed)
			{
				this.EyeOverlay.Close();
			}
			else
			{
				this.EyeOverlay.Open();
			}
		}
	}

	private void ExecuteCharacterTasks()
	{
		if (!this.EyesActuallyClosed)
		{
			return;
		}
		
		foreach (Mob character in this.Characters)
		{
			character.DoTask();
		}
	}
}