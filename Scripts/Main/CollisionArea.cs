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
	public static List<CollisionLine> CollisionsLines = new List<CollisionLine>();
	private CollisionLine _currentLine;

	private bool _mousePressed = false;
	private bool _drawStraightLine = false;
	private Vector2 _from;
	private Vector2 _to;

	public static Area2D MouseCollisionArea;
	public static RigidBody2D MouseSelectedObject;

	public override void _Ready()
	{
		MouseCollisionArea = GetNode<Area2D>("Mouse Position Area");
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
		if (Input.IsActionJustReleased("deleteBody") && MouseSelectedObject != null)
		{
			Settings.AnyEntity.Remove(MouseSelectedObject);
			ObjectsSpawn.Objects.Remove(MouseSelectedObject);
			MouseSelectedObject.Cancel();
		}

		_drawStraightLine = Input.IsActionPressed("drawStraightCollisionLine");

		if (@event is InputEventMouseMotion)
		{
			Vector2 mousePosition = GetViewport().GetMousePosition();
			if (_mousePressed)
			{
				if (!_drawStraightLine)
				{
					_to = mousePosition;
					_currentLinePoints.Add(_to);
					_currentLine.Points = _currentLinePoints.ToArray();
					_currentLine.AddCollision(new SegmentShape2D() { A = _from, B = _to });
					_from = _to;
				}
				else
				{
					_to = mousePosition;

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
			MouseCollisionArea.Position = mousePosition;
		}
		
	}
	public void GetStartedDrawLine()
	{
		_mousePressed = true;

		_currentLine = new CollisionLine(width: 3);
		if (_drawStraightLine)
			_currentLine.collisionArea.AddChild(new CollisionShape2D());
		CollisionsLines.Add(_currentLine);
		Settings.AnyEntity.Add(_currentLine);
		AddChild(_currentLine);

		_from = GetViewport().GetMousePosition();
		_currentLinePoints.Add(_from);
	}
	public void EndingDrawLine()
	{
		_mousePressed = false;
		_currentLinePoints.Clear();
	}
	public static void ClearCollisionLines()
	{

		Settings.AnyEntity.ForEach(node =>
		{
			if (node is CollisionLine) node.Cancel();
		});
		for (int i = CollisionsLines.Count - 1; i >= 0; i--)
			Settings.AnyEntity.Remove(CollisionsLines[i]);
		CollisionsLines.Clear();
		ObjectsSpawn.Objects.ForEach(body => ((RigidBody2D)body).ApplyForce(new Vector2(0, 0)));
	}
	public void ClickClearCollisionLines()
	{
		ClearCollisionLines();
	}
	public void GetObjectsUnderMouse(Node2D body)
	{
		if (body is RigidBody2D rigidBody)
			MouseSelectedObject = rigidBody;
		else
			MouseSelectedObject = null;
	}

}
