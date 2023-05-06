using Godot;
using System;

public partial class ObjectsCounter : Label
{
	private int _currentCount = ObjectsSpawn.Objects.Count;
	public override void _Ready()
	{
		Text = "Objects: 0";
	}

	public override void _Process(double delta)
	{
		if (_currentCount != ObjectsSpawn.Objects.Count)
		{
			Text = $"Objects: {ObjectsSpawn.Objects.Count}";
			_currentCount = ObjectsSpawn.Objects.Count;
		}


	}
}
