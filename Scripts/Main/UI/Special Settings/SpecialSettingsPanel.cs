using Godot;
using System;

public partial class SpecialSettingsPanel : PanelContainer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<Button>("Close").Connect("pressed", Callable.From(() => HidePanel()));
	}
	public override void _Process(double delta)
	{
	}

	public void HidePanel()
	{
		Visible = !Visible;
	}
}
