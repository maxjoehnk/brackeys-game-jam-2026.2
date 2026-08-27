using System.Linq;
using BrackeysGameJam2026.Empty.Jobs;
using Godot;
using Godot.Collections;

public partial class Prototype : Node2D
{
	private const float PlayerSpeed = 100f;
	
	private const int MobCount = 5;

	private readonly PackedScene MobScene = GD.Load<PackedScene>("res://Mob.tscn");
	
	private bool EyesClosed { get; set; }

	private CharacterBody2D Player => this.GetNode<CharacterBody2D>("Player");
	private Area2D HitDetector => this.GetNode<Area2D>("Player/HitDetector");

	private Array<Mob> Characters => [.. this.GetNode<Node>("Characters").GetChildren().Cast<Mob>()];

	private EyeOverlay EyeOverlay => this.GetNode<EyeOverlay>("Container");

	private readonly Array<PackedScene> MobTasks = [
		GD.Load<PackedScene>("res://Tasks/DemolitionTask.tscn"),
		GD.Load<PackedScene>("res://Tasks/RandomMoveAroundTask.tscn"),
	];

	public override void _Ready()
	{
		this.EyeOverlay.EyesOpening += () =>
		{
				foreach (Mob character in this.Characters)
				{
					character.BlendIn();
				}
		};
		this.EyeOverlay.EyesClosed += () =>
		{
				foreach (Mob character in this.Characters)
				{
					character.GetActive();
				}
		};
		this.EyeOverlay.Visible = true;

		this.SpawnMobs();
	}

	private void SpawnMobs()
	{
		for (int i = 0; i < MobCount; i++)
		{
			Mob mob = this.MobScene.Instantiate<Mob>();
			Task task = this.MobTasks[GD.RandRange(0, this.MobTasks.Count - 1)].Instantiate<Task>();
			mob.AddChild(task);
			mob.Tasks = [task];
			mob.Position = new Vector2(GD.RandRange(0, 1920), GD.RandRange(0, 1080));
			this.GetNode<Node>("Characters").AddChild(mob);
		}
	}

	public override void _Process(double delta)
	{
		this.HandleInteraction();
		this.HandleToggleEyes();
	}

	public override void _PhysicsProcess(double delta)
	{
		this.MovePlayer();
	}

	private void MovePlayer()
	{
		float horizontal = Input.GetAxis("left", "right");
		float vertical = Input.GetAxis("up", "down");

		this.Player.Velocity = new Vector2(horizontal, vertical) * PlayerSpeed;
		
		this.Player.MoveAndSlide();
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
					break;
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
}