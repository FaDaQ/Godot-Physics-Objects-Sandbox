using Godot;
using System;

public partial class CircleJoints : RigidBody2D
{
	[Export]
	private NodePath _body1Path;

	[Export]
	private NodePath _body2Path;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var joint = new PinJoint2D();
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
