using BrackeysGameJam2026.Core;
using Godot;

namespace BrackeysGameJam2026.UI;

public partial class FailedMenu : Panel
{
	public void OnRestart()
	{
		SceneManager.Instance.StartRun();
	}

	public void OnExit()
	{
		SceneManager.Instance.OpenMainMenu();
	}
}