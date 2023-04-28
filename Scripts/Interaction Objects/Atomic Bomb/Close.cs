using Godot;
using System;

public partial class Close : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.ConnectingUISugnals_Single();
		Connect("pressed", Callable.From(() =>
		{
			Settings.AnyEntity.Remove(GetParent().GetParent<AtomicBomb>());
			GetParent().GetParent<AtomicBomb>().Remove();
		}));
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
