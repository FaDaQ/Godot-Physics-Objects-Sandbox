using Godot;
using System;

public partial class RandomTorqueLabel : Label
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Settings.RandomTorque)
			Visible = true;
		else
			Visible = false;

		if (Visible)
		{
			Text = "Max Random Torque: " + Settings.MaxRandomTorque;
		}
	}
}
