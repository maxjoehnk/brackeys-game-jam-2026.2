using BrackeysGameJam2026.Core;
using Godot;

namespace BrackeysGameJam2026.UI;

public partial class LevelClearedMenu : Control
{
	public void OnNextLevel()
	{
		SceneManager.Instance.OpenNextLevel();
	}

	public void OnExit()
	{
		SceneManager.Instance.OpenMainMenu();
	}
}