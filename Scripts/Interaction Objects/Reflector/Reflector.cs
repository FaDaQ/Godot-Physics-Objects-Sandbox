using Godot;
using System;
using System.Collections.Generic;

public partial class Reflector : CharacterBody2D
{
	public float Speed = 25;
	public Vector2 Direction = new Vector2(30, 30);

	public Line2D TrajectoryLine = new Line2D();
	public List<Vector2> TrajectoryPionts = new List<Vector2>();

	private bool _setupIsOver = false;
	private Vector2 _startMousePosition;

	public override void _Ready()
	{
		Settings.AnyEntity.Add(this);
		GetChild<GpuParticles2D>(0).LocalCoords = true;
		TrajectoryLine.Width = 1;
		GetParent().AddChild(TrajectoryLine);
		_startMousePosition = GetViewport().GetMousePosition();
		if (Settings.RandomColors)
		{
			Random rnd = new Random();
			Color randomColor = new Color(rnd.NextSingle(), rnd.NextSingle(), rnd.NextSingle());
			TrajectoryLine.DefaultColor = randomColor;
			GetChild<GpuParticles2D>(0).SelfModulate = randomColor;
		}

	}

	public override void _Process(double delta)
	{
		if (_setupIsOver && !Settings.Pause)
		{
			KinematicCollision2D collision = MoveAndCollide(Direction * new Vector2((float)delta * Speed, (float)delta * Speed));
			TrajectoryPionts.Add(Position);
			TrajectoryLine.Points = TrajectoryPionts.ToArray();
			Direction = Direction.Bounce(collision.GetNormal());
		}
		else
		{
			TrajectoryLine.Points = new Vector2[] { _startMousePosition, GetViewport().GetMousePosition() };
			if (Input.IsActionJustPressed("Apply"))
			{
				_setupIsOver = true;
				Direction = TrajectoryLine.Points[1] - TrajectoryLine.Points[0];
				TrajectoryLine.Points = new Vector2[0];
			}
		}
	}

	public void Cancel()
	{
		Console.WriteLine("Yes");
	}

}
