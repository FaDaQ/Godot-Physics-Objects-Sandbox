using Godot;
using System;

public partial class Drawer : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("pause") && GetTree().Paused == false)
		{
			Settings.Pause = true;
			GetTree().Paused = true;
		}
		else if (Input.IsActionJustPressed("pause") && GetTree().Paused == true)
		{
			Settings.Pause = false;
			GetTree().Paused = false;
		}
	}
}
