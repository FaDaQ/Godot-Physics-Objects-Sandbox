using Godot;
using System;

public partial class ParametrsChange_ConnectingSignals : VBoxContainer
{
	public override void _Ready()
	{
		this.ConnectingUISignals_InContainer();
		
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
