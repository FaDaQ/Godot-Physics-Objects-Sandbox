using Godot;
using System;

public partial class SelectGeneratorBody : OptionButton
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Settings crutch = new Settings();
		Connect("mouse_entered", Callable.From(() => crutch.userInteractWithGUI()));
		Connect("mouse_exited", Callable.From(() => crutch.userEndInteractWithGUI()));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
