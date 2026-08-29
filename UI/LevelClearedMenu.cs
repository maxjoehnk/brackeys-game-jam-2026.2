using BrackeysGameJam2026.Core;
using Godot;

namespace BrackeysGameJam2026.UI;

public partial class LevelClearedMenu : Control
{
	public void OnNextLevel()
	{
		GlobalAudioPlayback.Instance.PlayButtonClick();
		SceneManager.Instance.OpenNextLevel();
	}

	public void OnExit()
	{
		GlobalAudioPlayback.Instance.PlayButtonClick();
		SceneManager.Instance.OpenMainMenu();
	}
}