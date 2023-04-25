using Godot;
using System;

public partial class ParametrsChange_ConnectingSignals : VBoxContainer
{
	public override void _Ready()
	{
		foreach (var i in GetChildren())
		{
			Settings crutch = new Settings();
			i.Connect("mouse_entered", Callable.From(() => crutch.userInteractWithGUI()));
			i.Connect("mouse_exited", Callable.From(() => crutch.userEndInteractWithGUI()));
		}
		
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
