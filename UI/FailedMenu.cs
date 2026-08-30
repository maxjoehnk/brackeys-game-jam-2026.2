using BrackeysGameJam2026.Core;
using Godot;

namespace BrackeysGameJam2026.UI;

public partial class FailedMenu : Panel
{
	private Control RetryButton => this.GetNode<Control>("PanelContainer/VBoxContainer/Retry");
	
	public void OnRestart()
	{
		GlobalAudioPlayback.Instance.PlayButtonClick();
		SceneManager.Instance.StartRun();
	}

	public void OnExit()
	{
		GlobalAudioPlayback.Instance.PlayButtonClick();
		SceneManager.Instance.OpenMainMenu();
	}

	public void Focus()
	{
		this.RetryButton.GrabFocus();
	}
}