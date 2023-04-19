using Godot;
using Godot.Collections;
using System;

public partial class Root : Node2D
{
	public static Dictionary<string, CollisionShape2D> Borders = new Dictionary<string, CollisionShape2D>();

	public override void _Ready()
	{

		foreach (CollisionShape2D i in GetNode<StaticBody2D>("Panel Container/Drawer/Collision Area/").GetChildren())
			Borders.Add(i.Name, i);


		GetParent().Connect("size_changed", Callable.From(() => ChangeScreenSize()));
	}

	public override void _Process(double delta)
	{
		
	}

	public override void _Input(InputEvent @event)
	{
		if (Input.IsActionJustPressed("fullScreen"))
		{
			DisplayServer.WindowSetMode((DisplayServer.WindowGetMode() != DisplayServer.WindowMode.Fullscreen) 
										? DisplayServer.WindowMode.Fullscreen
										: DisplayServer.WindowMode.Windowed);
		}
	}

	public void ChangeScreenSize()
	{
		Vector2 windowSize = DisplayServer.WindowGetSize();
		GetNode<Control>("Panel Container").Size = windowSize;
		ResizeableBorders(windowSize);
		ObjectsSpawn.RigidBodies.ForEach(body => body.ApplyForce(new Vector2(0, 0)));
	}

	private void ResizeableBorders(Vector2 windowSize)
	{
		Borders["Floor"].Shape = new SegmentShape2D() { A = new Vector2(0, windowSize.Y), B = new Vector2(windowSize.X, windowSize.Y) };
		Borders["Left"].Shape = new SegmentShape2D() { A = new Vector2(0, 0), B = new Vector2(0, windowSize.Y) };
		Borders["Right"].Shape = new SegmentShape2D() { A = new Vector2(windowSize.X, 0), B = new Vector2(windowSize.X, windowSize.Y) };
	} 
}
