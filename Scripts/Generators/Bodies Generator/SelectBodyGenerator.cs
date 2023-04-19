using Godot;
using Godot.Collections;
using System;

public partial class SelectBodyGenerator : OptionButton
{
	Node2D GeneratorSettings;

	public override void _Ready()
	{
		Settings crutch = new Settings();
		Connect("mouse_entered", Callable.From(() => crutch.userInteractWithGUI()));
		Connect("mouse_exited", Callable.From(() => crutch.userEndInteractWithGUI()));
	}

	// Call in item_selected signal
	public void SetCurrentBodyID(long index)
	{
		
	}

	public override void _Input(InputEvent @event)
	{
		
	}
}
