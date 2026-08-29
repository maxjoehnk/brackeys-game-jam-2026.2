using BrackeysGameJam2026.Core;
using Godot;

namespace BrackeysGameJam2026.UI;

public partial class FailedMenu : Panel
{
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
}