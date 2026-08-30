using Godot;
using System;

public partial class TutorialOverlay : PanelContainer
{
	public override void _Ready()
	{
		this.GrabFocus();
	}

	public override void _GuiInput(InputEvent @event)
	{
		if (Input.IsActionJustPressedByEvent("clear_tutorial", @event))
		{
			this.FadeOut();
		}
	}

	public void FadeOut()
	{
		Tween tween = this.CreateTween();
		tween.TweenProperty(this, "modulate", new Color(0xffffff00), 0.5f);
		tween.Play();
		this.ReleaseFocus();
		this.FocusMode = FocusModeEnum.None;
		this.MouseFilter = MouseFilterEnum.Ignore;
		tween.Finished += this.Hide;
	}
}
