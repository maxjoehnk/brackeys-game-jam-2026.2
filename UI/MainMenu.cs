using BrackeysGameJam2026.Core;
using Godot;

namespace BrackeysGameJam2026.UI;

public partial class MainMenu : Control
{
	private Control? PlayButton => this.FindChild("Play") as Control;
	
	public override void _Ready()
	{
		this.PlayButton?.GrabFocus();
	}

	public void OnPlay()
	{
		GlobalAudioPlayback.Instance.PlayButtonClick();
		// SceneManager.Instance.StartTutorial();
		SceneManager.Instance.StartRun();
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
