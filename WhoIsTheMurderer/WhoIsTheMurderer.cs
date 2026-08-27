using Godot;
using System;
using System.Linq;

[Tool]
public partial class WhoIsTheMurderer : Node2D
{
	[Export] public int Columns { get; set; }
	
	[Export] public int Rows { get; set; }

	private Label TextBox => this.GetNode<Label>("CanvasLayer/PanelContainer/MarginContainer/Label");
	private TextureRect CharacterBox => this.GetNode<TextureRect>("CanvasLayer/PanelContainer/MarginContainer/Panel/TextureRect");
	
	private Button AccuseButton => this.GetNode<Button>("CanvasLayer/PanelContainer/MarginContainer/Button");

	private Character? selected;
	
	private Heart Heart => this.GetNode<Heart>("CanvasLayer/HFlowContainer/Heart");

	public override void _Ready()
	{
		if (Engine.IsEditorHint())
		{
			return;
		}
		foreach (Character child in this.GetNode("Characters").GetChildren().Cast<Character>())
		{
			child.Click += (hint) => this.OnCharacterClicked(child, hint);
		}

		this.AccuseButton.Pressed += this.OnAccuseButtonPressed;
	}

	private void OnCharacterClicked(Character character, string hint)
	{
		this.GetNode<MarginContainer>("CanvasLayer/PanelContainer/MarginContainer").Visible = true;
		this.TextBox.Text = hint;
		this.CharacterBox.Texture = character.Asset.Texture;
		this.selected = character;
	}

	private void OnAccuseButtonPressed()
	{
		if (this.selected?.Murderer != true)
		{
			GD.Print("Du hast den Täter nicht erraten!");
			this.Heart.Pop();
			// TODO: Leben verlieren?
			return;
		}
		
		GD.Print("Du hast den Täter erraten!");
	}
}
