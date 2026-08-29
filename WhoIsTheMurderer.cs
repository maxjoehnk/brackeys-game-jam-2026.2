using Godot;
using System.Linq;
using Godot.Collections;

namespace BrackeysGameJam2026;

[Tool]
public partial class WhoIsTheMurderer : Node2D
{
	[Export] public int Columns { get; set; } = 3;

	private PackedScene CharacterSlot = GD.Load<PackedScene>("res://CharacterSlot.tscn");

	private Label TextBox => this.GetNode<Label>("CanvasLayer/PanelContainer/Label");
	private TextureRect CharacterBox => this.GetNode<TextureRect>("CanvasLayer/PanelContainer/Panel/TextureRect");

	private Button AccuseButton => this.GetNode<Button>("CanvasLayer/PanelContainer/Button");

	private Character? selected;

	private Array<Heart> Hearts => [.. this.GetNode("CanvasLayer/HFlowContainer").GetChildren().Cast<Heart>()];

	private GridContainer CharacterContainer => this.GetNode<GridContainer>("CenterContainer/Characters");

	private Control PauseMenu => (this.FindChild("PauseMenu") as Control)!;
	private Control WinMenu => (this.FindChild("LevelClearedMenu") as Control)!;
	private Control LostMenu => (this.FindChild("FailedMenu") as Control)!;

	private int lives;

	public override void _Ready()
	{
		this.CharacterContainer.Columns = this.Columns;
		Array<Character> characters = [.. this.GetNode("Characters").GetChildren().Cast<Character>()];
		foreach (Character _ in characters)
		{
			Node slot = this.CharacterSlot.Instantiate();
			this.CharacterContainer.AddChild(slot);
		}

		if (Engine.IsEditorHint())
		{
			return;
		}

		for (int i = 0; i < GameState.Instance.TotalLives; i++)
		{
			if (i < GameState.Instance.Lives)
			{
				this.Hearts[i].SetFull();
			}
			else
			{
				this.Hearts[i].SetEmpty();
			}
		}

		for (int i = 0; i < characters.Count; i++)
		{
			Character character = characters[i];
			Node slot = this.CharacterContainer.GetChild(i);
			character.CustomMinimumSize = new Vector2(128, 128);
			character.SizeFlagsHorizontal = Control.SizeFlags.ShrinkCenter;
			character.SizeFlagsVertical = Control.SizeFlags.ShrinkCenter;
			character.GetParent().RemoveChild(character);
			slot.AddChild(character);
		}

		this.GetNode<Control>("CanvasLayer/PanelContainer/Panel").Visible = false;
		this.AccuseButton.Visible = false;
		this.PauseMenu.Visible = false;
		this.WinMenu.Visible = false;
		this.LostMenu.Visible = false;

		GameState.Instance.LifeLost += this.OnUpdateLives;
		GameState.Instance.Lost += this.OnLost;
		characters.FirstOrDefault()?.GrabFocus();
		foreach (Character child in characters)
		{
			child.Pressed += () => this.OnCharacterClicked(child);
		}

		this.AccuseButton.Pressed += this.OnAccuseButtonPressed;
	}

	public override void _ExitTree()
	{
		this.AccuseButton.Pressed -= this.OnAccuseButtonPressed;
		GameState.Instance.LifeLost -= this.OnUpdateLives;
		GameState.Instance.Lost -= this.OnLost;
	}

	private void OnUpdateLives()
	{
		this.Hearts[GameState.Instance.Lives].Pop();
	}

	private void OnLost()
	{
		this.GetTree().Paused = true;
		this.LostMenu.Visible = true;
	}

	public override void _Input(InputEvent @event)
	{
		if (Input.IsActionJustPressedByEvent("menu", @event))
		{
			this.GetTree().Paused = true;
			this.PauseMenu.Visible = true;
		}
	}

	private void OnCharacterClicked(Character character)
	{
		this.GetNode<Control>("CanvasLayer/PanelContainer/Panel").Visible = true;
		this.AccuseButton.Visible = true;
		this.TextBox.Text = character.Hint;
		this.CharacterBox.Texture = character.Texture;
		this.selected = character;
	}

	private void OnAccuseButtonPressed()
	{
		GlobalAudioPlayback.Instance.PlayButtonClick();
		if (this.selected?.Murderer != true)
		{
			GameState.Instance.Fail();
			return;
		}

		this.WinMenu.Visible = true;
		this.GetTree().Paused = true;
	}
}