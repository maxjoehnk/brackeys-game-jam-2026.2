using Godot;
using System.Linq;
using BrackeysGameJam2026.UI;
using Godot.Collections;

namespace BrackeysGameJam2026;

[Tool]
public partial class WhoIsTheMurderer : Node2D
{
	[Export] public int Columns { get; set; } = 3;

	private PackedScene CharacterSlot = GD.Load<PackedScene>("res://CharacterSlot.tscn");

	private Label TextBox => this.GetNode<Label>("CanvasLayer/PanelContainer/Label");
	private TextureRect CharacterBox => this.GetNode<TextureRect>("CanvasLayer/PanelContainer/Panel/TextureRect");

	private Button AccuseButton => this.GetNode<Button>("CanvasLayer/PanelContainer/AccuseButton");

	private Character? selected;

	private Array<Heart> Hearts => [.. this.GetNode("CanvasLayer/HFlowContainer").GetChildren().Cast<Heart>()];

	private GridContainer CharacterContainer => this.GetNode<GridContainer>("CenterContainer/Characters");

	private Control PauseMenu => (this.FindChild("PauseMenu") as Control)!;
	private LevelClearedMenu WinMenu => (this.FindChild("LevelClearedMenu") as LevelClearedMenu)!;
	private FailedMenu LostMenu => (this.FindChild("FailedMenu") as FailedMenu)!;
	private Control WrongAccusationOverlay => (this.FindChild("WrongAccusationOverlay") as Control)!;
	
	private AudioStreamPlayer AudioPlayer => this.GetNode<AudioStreamPlayer>("OneShotPlayer");

	private Texture2D XboxTexture = GD.Load<AtlasTexture>("res://Assets/UI/AccuseXbox.tres");
	private Texture2D PlaystationTexture = GD.Load<AtlasTexture>("res://Assets/UI/AccusePlaystation.tres");
	private Texture2D KeyboardTexture = GD.Load<AtlasTexture>("res://Assets/UI/AccuseKeyboard.tres");
	private InputType inputType = InputType.Keyboard;

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
		this.WrongAccusationOverlay.Modulate = new Color(0xffffff00);

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
		this.LostMenu.Focus();
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton or InputEventKey)
		{
			this.inputType = InputType.Keyboard;
		}else if (@event is InputEventJoypadButton)
		{
			this.SetInputTypeForDevice(@event.Device);
		}
		this.UpdateAccuseIcon();

		if (Input.IsActionJustPressedByEvent("menu", @event))
		{
			this.GetTree().Paused = true;
			this.PauseMenu.Visible = true;
		}

		if (Input.IsActionJustPressedByEvent("accuse", @event) && this.selected != null)
		{
			this.OnAccuseButtonPressed();
		}
	}
	
	private void SetInputTypeForDevice(int deviceId)
	{
		string controllerName = Input.GetJoyName(deviceId);
		if (controllerName.Contains("PS") || controllerName.Contains("DualShock") ||
		    controllerName.Contains("PlayStation"))
		{
			this.inputType = InputType.PlayStation;
		}
		else
		{
			this.inputType = InputType.GenericController;
		}
	}

	private void UpdateAccuseIcon()
	{
		Texture2D texture = this.inputType switch
		{
			InputType.Keyboard => this.KeyboardTexture,
			InputType.PlayStation => this.PlaystationTexture,
			_ => this.XboxTexture,
		};
		this.AccuseButton.GetChild<TextureRect>(0).Texture = texture;
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
			if (this.selected?.FalseAccusedSample != null)
			{
				this.AudioPlayer.Stream = this.selected?.FalseAccusedSample;
				this.AudioPlayer.Play();
			}
			GameState.Instance.Fail();

			this.ShowAccusationOverlay();
			SceneTreeTimer timer = this.GetTree().CreateTimer(5);
			timer.Timeout += this.HideAccusationOverlay;

			return;
		}

		this.WinMenu.Visible = true;
		this.WinMenu.Focus();
		this.GetTree().Paused = true;
	}

	private void ShowAccusationOverlay()
	{
		this.FadeAccusationOverlay(new Color(0xffffffff));
	}

	private void HideAccusationOverlay()
	{
		this.FadeAccusationOverlay(new Color(0xffffff00));
	}

	private void FadeAccusationOverlay(Color color)
	{
		Tween tween = this.CreateTween();
		tween.TweenProperty(this.WrongAccusationOverlay, "modulate", color, 0.5f);
		tween.Play();
	}

	enum InputType
	{
		GenericController = 0,
		PlayStation = 1,
		Keyboard = 2,
	}
}