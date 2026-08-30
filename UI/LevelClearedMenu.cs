using BrackeysGameJam2026.Core;
using Godot;

namespace BrackeysGameJam2026.UI;

public partial class LevelClearedMenu : Control
{
	private Button NextLevelButton => this.GetNode<Button>("PanelContainer/VBoxContainer/NextLevel");
	private Button NewRunButton => this.GetNode<Button>("PanelContainer/VBoxContainer/NewRun");
	
	public override void _Ready()
	{
		bool hasNextLevel = SceneManager.Instance.HasNextLevel();
		this.NextLevelButton.Visible = hasNextLevel;
		this.NewRunButton.Visible = !hasNextLevel;
	}

	public void OnNewRun()
	{
		GlobalAudioPlayback.Instance.PlayButtonClick();
		SceneManager.Instance.StartRun();
	}

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