using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class CollisionLine : Line2D
{
	public override void _Ready()
	{
		AddChild(collisionArea);
	}

	public CollisionLine() { }
	public CollisionLine(int width) { Width = width; }
	public CollisionLine(Vector2[] points, int width, bool antialiased = false)
	{
		Points = points;
		Antialiased = antialiased;
		Width = width;
	}

	public void AddCollision(SegmentShape2D shape)
	{
		collisionArea.AddChild(new CollisionShape2D() { Shape = shape });
	}
	public void Remove()
	{
		GetParent().RemoveChild(this);
	}


	public StaticBody2D collisionArea = new StaticBody2D();
}

public partial class CollisionArea : StaticBody2D
{
	private List<Vector2> _currentLinePoints = new List<Vector2>();
	private List<CollisionLine> _collisionsLines = new List<CollisionLine>();
	private CollisionLine _currentLine;

	private bool _mousePressed = false;
	private bool _drawStraightLine = false;
	private Vector2 _from;
	private Vector2 _to;

	public override void _Ready()
	{
	}
	public override void _Process(double delta)
	{
	}
	public override void _Input(InputEvent @event)
	{
		if (Input.IsActionJustPressed("startDrawCollisionLine"))
			GetStartedDrawLine();
		else if (Input.IsActionJustReleased("startDrawCollisionLine"))
			EndingDrawLine();

		if (Input.IsActionJustReleased("clearCollisionLines"))
			ClearCollisionLines();

		_drawStraightLine = Input.IsActionPressed("drawStraightCollisionLine");
		
		if (@event is InputEventMouseMotion && _mousePressed)
		{
			if (!_drawStraightLine)
			{
				_to = GetViewport().GetMousePosition();
				_currentLinePoints.Add(_to);
				_currentLine.Points = _currentLinePoints.ToArray();
				_currentLine.AddCollision(new SegmentShape2D() { A = _from, B = _to });
				_from = _to;
			}
			else
			{
				_to = GetViewport().GetMousePosition();

				if (_currentLinePoints.Count < 2)
				{
					_currentLinePoints.Add(_to);
					_currentLine.AddCollision(new SegmentShape2D() { A = _from, B = _to });
				}
				else
				{
					_currentLinePoints[1] = _to;
				}
				_currentLine.Points = _currentLinePoints.ToArray();
				_currentLine.collisionArea.GetChild<CollisionShape2D>(0).Shape = new SegmentShape2D() { A = _from, B = _to };
			}
		}
		
	}

	public void GetStartedDrawLine()
	{
		_mousePressed = true;

		_currentLine = new CollisionLine(width: 3);
		if (_drawStraightLine)
			_currentLine.collisionArea.AddChild(new CollisionShape2D());
		_collisionsLines.Add(_currentLine);
		AddChild(_currentLine);

		_from = GetViewport().GetMousePosition();
		_currentLinePoints.Add(_from);
	}
	public void EndingDrawLine()
	{
		_mousePressed = false;
		_currentLinePoints.Clear();
	}
	public void ClearCollisionLines()
	{
		_collisionsLines.ForEach(line => line.Remove());
		_collisionsLines.Clear();
		ObjectsSpawn.RigidBodies.ForEach(body => body.ApplyForce(new Vector2(0, 0)));
	}
}
