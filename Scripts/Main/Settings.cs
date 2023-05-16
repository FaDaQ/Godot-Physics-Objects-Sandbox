using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class Settings : Node
{
	public static bool Pause = false;
	public static int currentBodyID = 0;
	public static string currentObjectName = "Circle";
	public static Vector2 Scale;
	

	public static float Gravity = 1f;
	public static float Bounce = 0.5f;
	public static Godot.Collections.Dictionary<string, float> physicsParametrs = new Godot.Collections.Dictionary<string, float>()
	{
		{ "Mass", 2f },
		{ "Friction", 1f },
		{ "Bounce", 0.5f },
		{ "Torque", 0f },
		{ "Inertia", 1f }, // Don't work
		{ "Scale", 1f }
	};

	public static bool userIsInteractGUI = false;
	public static bool RandomColors = false;
	public static bool RandomTorque = false;
	public static int MaxRandomTorque = 0;
	public static bool RandomScale = false;
	public static float MaxRandomScale = 1f;
	public static float MinRandomScale = 0.2f;

	public static readonly Godot.Collections.Dictionary<string, Color> Colors = new Godot.Collections.Dictionary<string, Color>()
	{
		{ "Red", new Color(1, 0, 0) },
		{ "Blue", new Color(0, 0, 1) },
		{ "Deffault", new Color(1, 1, 1) },
		{ "Black", new Color(0, 0, 0) },
		{ "Green", new Color(0, 1, 0) },
		{ "Orange", new Color(1, 0.52f, 0) },
		{ "Yellow", new Color(1, 1, 0) }
	};
	public static string BodyColorName;
	public static Color BodyColor = Colors["Deffault"];
	

	public static List<Node2D> AnyEntity = new List<Node2D>(); // This List is required for you could cancel the last action
	public static Vector2 WindowSize;

	// Call in mouse_entered signal for fix bug with bodies spawn
	public void userInteractWithGUI()
	{
		userIsInteractGUI = true;
	}
	// Call in mouse_exited signal
	public void userEndInteractWithGUI()
	{
		userIsInteractGUI = false;
	}
	// Call in text_changed signal in Gravity LineEdit
	public void GravityChanged(string new_text)
	{
		Gravity = Convert.ToSingle(new_text.Replace(".", ","));
		ObjectsSpawn.Objects.ForEach(node => { if (node is RigidBody2D body) body.GravityScale = Gravity; });
	}
	public static void CancelAction()
	{
		AnyEntity[^1].Cancel();
		Node2D entity = AnyEntity[^1];

		if (entity is PhysicsBody2D || entity is CollisionObject2D)
			ObjectsSpawn.Objects.Remove(entity);
		else if (entity is Generator)
			ObjectsSpawn.Generators.Remove(entity as Generator);

		AnyEntity.RemoveAt(AnyEntity.Count - 1);
	}
	#region Colors
	public void ApplyRandomColors(bool button_pressed) // Called from "toggled" signal in UI/Special Settings Panel/Special Settings VBox/Random Torque
	{
		RandomColors = button_pressed;
		Console.WriteLine("Random Color:" + ((RandomColors) ? "On" : "Off"));
	}
	public void ApplyColor(long index) // Connecting in Actions Panel to Body Color OptionButton
	{
		BodyColorName = GetParent().GetNode<OptionButton>("UI/Actions Panel/HBox/Actions/Body Color").GetItemText((int)index);
		BodyColor = Colors[BodyColorName];

	}
	#endregion
	#region Random Torque
	public void ApplyRandomTorque(bool button_pressed)
	{
		RandomTorque = button_pressed;
		GetParent().GetNode<HSlider>("UI/Special Settings Panel/Special Settings VBox/Random Torque Slider").Visible = button_pressed;
	}
	public void SetMaxRandomTorque(double value)
	{
		MaxRandomTorque = Convert.ToInt32(value);
	}
	#endregion
	#region RandomScale
	public void ApplyRandomScale(bool button_pressed)
	{
		RandomScale = button_pressed;
		GetParent().GetNode<HSlider>("UI/Special Settings Panel/Special Settings VBox/Random Scale Slider").Visible = button_pressed;
	}
	public void SetMaxRandomScale(double value)
	{
		MaxRandomScale = Convert.ToSingle(value);
	}
	#endregion
}
