using BrackeysGameJam2026.Core;
using Godot;

namespace BrackeysGameJam2026.UI;

public partial class MainMenu : Control
{
	private Control? PlayButton => this.FindChild("Play") as Control;
	private Control? ExitButton => this.FindChild("Exit") as Control;

	public override void _Ready()
	{
		this.PlayButton?.GrabFocus();

		if (OS.GetName() == "Web")
		{
			this.ExitButton?.Hide();
		}
	}

	public void OnPlay()
	{
		GlobalAudioPlayback.Instance.PlayButtonClick();
		if (!SceneManager.Instance.HasPlayedTutorial)
		{
			SceneManager.Instance.StartTutorial();
		}
		else
		{
			SceneManager.Instance.StartRun();
		}
	}

	public void OnOpenSettings()
	{
		GlobalAudioPlayback.Instance.PlayButtonClick();
		SceneManager.Instance.OpenSettings();
	}

	public void OnExit()
	{
		GlobalAudioPlayback.Instance.PlayButtonClick();
		this.GetTree().Quit();
	}
}