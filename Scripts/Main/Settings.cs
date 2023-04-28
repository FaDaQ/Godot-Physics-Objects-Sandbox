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
    public static List<Node2D> AnyEntity = new List<Node2D>(); // This List is required for you could cancel the last action

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
		ObjectsSpawn.RigidBodies.ForEach(body => body.GravityScale = Gravity);
	}
	public static void CancelAction()
	{
        AnyEntity[^1].Cancel();
		object entity = AnyEntity[^1];

		if (entity is RigidBody2D)
			ObjectsSpawn.RigidBodies.Remove(entity as RigidBody2D);
		else if (entity is ParentGenerator)
			ObjectsSpawn.Generators.Remove(entity as ParentGenerator);

        AnyEntity.RemoveAt(AnyEntity.Count - 1);
    }
}
