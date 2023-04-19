using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class Settings : Node
{
	public static bool Pause = false;
	public static int currentBodyID = 0;
	public static string currentBodyName = "Circle";
	public static Vector2 Scale;
	

	public static float Gravity = 1f;
	public static float Bounce = 0.5f;
	public static Godot.Collections.Dictionary<string, float> physicsParametrs = new Godot.Collections.Dictionary<string, float>()
	{
		{ "Mass", 2f },
		{ "Friction", 1f },
		{ "Bounce", 0.5f },
		{ "Inertia", 1f }, // Don't work
		{ "Scale", 1f }
	};

	public static bool userIsInteractGUI = false;

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
		Gravity = Convert.ToInt32(new_text);
		ObjectsSpawn.RigidBodies.ForEach(body => body.GravityScale = Gravity);
	}
}
