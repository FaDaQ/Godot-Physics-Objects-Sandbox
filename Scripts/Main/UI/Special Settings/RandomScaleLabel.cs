using Godot;
using System;

public partial class RandomScaleLabel : Label
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Settings.RandomScale)
			Visible = true;
		else
			Visible = false;

		if (Visible)
		{
			Text = "Max Random Scale: " + Settings.MaxRandomScale;
		}
	}
}
