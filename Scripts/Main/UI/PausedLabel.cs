using Godot;
using System;

public partial class PausedLabel : Label
{
	
	public override void _Ready()
	{
	}
	public override void _Process(double delta)
	{
		Visible = Settings.Pause;
	}
}
