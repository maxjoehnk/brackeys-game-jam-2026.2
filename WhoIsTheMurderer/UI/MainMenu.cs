using Godot;
using System;
using BrackeysGameJam2026.Empty.WhoIsTheMurderer.Core;

public partial class MainMenu : Control
{
	private Control? PlayButton => this.FindChild("Play") as Control;
	
	public override void _Ready()
	{
		this.PlayButton?.GrabFocus();
	}

	public void OnPlay()
	{
		// SceneManager.Instance.StartTutorial();
		SceneManager.Instance.StartRun();
	}

	public void OnOpenSettings()
	{
		SceneManager.Instance.OpenSettings();
	}
	
	public void OnExit()
	{
		this.GetTree().Quit();
	}
}
