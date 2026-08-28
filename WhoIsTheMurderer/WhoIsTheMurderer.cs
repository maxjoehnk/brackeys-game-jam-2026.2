using Godot;
using System.Linq;

[Tool]
public partial class WhoIsTheMurderer : Node2D
{
	[Export]
	public int Columns
	{
		get => this.CharacterContainer.Columns;
		set => this.CharacterContainer.Columns = value;
	}
	
	[Export] public int Rows { get; set; }

	private Label TextBox => this.GetNode<Label>("CanvasLayer/PanelContainer/MarginContainer/Label");
	private TextureRect CharacterBox => this.GetNode<TextureRect>("CanvasLayer/PanelContainer/MarginContainer/Panel/TextureRect");
	
	private Button AccuseButton => this.GetNode<Button>("CanvasLayer/PanelContainer/MarginContainer/Button");

	private Character? selected;
	
	private Heart Heart => this.GetNode<Heart>("CanvasLayer/HFlowContainer/Heart");
	
	private GridContainer CharacterContainer => this.GetNode<GridContainer>("Characters");

	public override void _Ready()
	{
		if (Engine.IsEditorHint())
		{
			return;
		}
		this.CharacterContainer.GetChildren().Cast<Character>().First().GrabFocus();
		foreach (Character child in this.CharacterContainer.GetChildren().Cast<Character>())
		{
			child.Pressed += () => this.OnCharacterClicked(child);
		}

		this.AccuseButton.Pressed += this.OnAccuseButtonPressed;
	}

	private void OnCharacterClicked(Character character)
	{
		this.GetNode<MarginContainer>("CanvasLayer/PanelContainer/MarginContainer").Visible = true;
		this.TextBox.Text = character.Hint;
		this.CharacterBox.Texture = character.Texture;
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
