using Godot;
using System.Linq;
using BrackeysGameJam2026.Empty.WhoIsTheMurderer;
using Godot.Collections;

[Tool]
public partial class WhoIsTheMurderer : Node2D
{
	[Export] public int StartHearts { get; set; } = 3;

	[Export] public int Columns { get; set; } = 3;

	private PackedScene CharacterSlot = GD.Load<PackedScene>("res://WhoIsTheMurderer/CharacterSlot.tscn");
	
	private Label TextBox => this.GetNode<Label>("CanvasLayer/PanelContainer/Label");
	private TextureRect CharacterBox => this.GetNode<TextureRect>("CanvasLayer/PanelContainer/Panel/TextureRect");
	
	private Button AccuseButton => this.GetNode<Button>("CanvasLayer/PanelContainer/Button");

	private Character? selected;
	
	private Array<Heart> Hearts => [.. this.GetNode("CanvasLayer/HFlowContainer").GetChildren().Cast<Heart>() ];
	
	private GridContainer CharacterContainer => this.GetNode<GridContainer>("CenterContainer/Characters");

	private int lives;

	public override void _Ready()
	{
		this.lives = this.StartHearts;
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

		GameState.Instance.LifeLost += () =>
		{
			this.Hearts[GameState.Instance.Lives].Pop();
		};
		characters.FirstOrDefault()?.GrabFocus();
		foreach (Character child in characters)
		{
			child.Pressed += () => this.OnCharacterClicked(child);
		}

		this.AccuseButton.Pressed += this.OnAccuseButtonPressed;
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
		if (this.selected?.Murderer != true)
		{
			GameState.Instance.Fail();
			return;
		}
		
		GD.Print("Du hast den Täter erraten!");
	}
}
