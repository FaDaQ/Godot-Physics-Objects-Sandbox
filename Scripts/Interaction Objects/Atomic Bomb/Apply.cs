using Godot;
using System;

public partial class Apply : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Connect("pressed", Callable.From(() => 
		{
			GetParent<PanelContainer>().Hide();
			GetParent().GetParent<RigidBody2D>().Freeze = false;
		}));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
