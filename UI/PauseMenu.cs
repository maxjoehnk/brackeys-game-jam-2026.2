using Godot;
using System;
using BrackeysGameJam2026.Core;

public partial class PauseMenu : Control
{
	public override void _Input(InputEvent @event)
	{
		if (Input.IsActionJustPressedByEvent("menu", @event))
		{
			this.OnResume();
		}
	}

	public void OnResume()
	{
		GlobalAudioPlayback.Instance.PlayButtonClick();
		this.GetTree().Paused = false;
		this.Visible = false;
	}

	public void OnExit()
	{
		GlobalAudioPlayback.Instance.PlayButtonClick();
		SceneManager.Instance.OpenMainMenu();
	}
}
