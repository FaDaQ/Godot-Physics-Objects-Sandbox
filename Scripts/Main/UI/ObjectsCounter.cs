using Godot;
using System;

public partial class ObjectsCounter : Label
{
	private int _currentCount = ObjectsSpawn.RigidBodies.Count;
	public override void _Ready()
	{
		Text = "Objects: 0";
	}

	public override void _Process(double delta)
	{

		Text = $"Objects: {ObjectsSpawn.RigidBodies.Count}";

	}
}
