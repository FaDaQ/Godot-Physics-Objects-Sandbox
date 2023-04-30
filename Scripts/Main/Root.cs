using Godot;
using Godot.Collections;
using System;
using System.Linq;

public partial class Root : Node2D
{
	public static Dictionary<string, CollisionShape2D> Borders = new Dictionary<string, CollisionShape2D>();
	public static Control UI;

	public override void _Ready()
	{

		foreach (CollisionShape2D i in GetNode<StaticBody2D>("Panel Container/Drawer/Collision Area/").GetChildren().Take(3))
			Borders.Add(i.Name, i);
		UI = GetNode<Control>("UI");

		GetParent<Window>().Connect("size_changed", Callable.From(() => ChangeScreenSize()));
		GetParent<Window>().MinSize = new Vector2I(1075, 445);
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
		UI.Size = windowSize;
		ResizeableBorders(windowSize);
		ObjectsSpawn.RigidBodies.ForEach(body => body.ApplyForce(new Vector2(0, 0)));
		Console.WriteLine(windowSize);
	}

	private void ResizeableBorders(Vector2 windowSize)
	{
		Borders["Floor"].Transform = new Transform2D(0, new Vector2(windowSize.X / 2, windowSize.Y));
		Borders["Left"].Transform = new Transform2D(0, new Vector2(0, windowSize.Y / 2));
		Borders["Right"].Transform = new Transform2D(0, new Vector2(windowSize.X, windowSize.Y / 2));
	} 
}
