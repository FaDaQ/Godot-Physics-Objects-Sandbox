using Godot;
using System;
using System.Collections.Generic;

public partial class ObjectsSpawn : Node2D
{
	public static List<RigidBody2D> RigidBodies = new List<RigidBody2D>();
	public static List<ParentGenerator> Generators = new List<ParentGenerator>();

	public static PackedScene bodyScene;


	public override void _Ready()
	{
		bodyScene = (PackedScene)ResourceLoader.Load($"res://Scenes/RigidBodies/{Settings.currentObjectName}.tscn");
	}
	public override void _Process(double delta)
	{
	}
	public override void _Input(InputEvent @event)
	{
		if (Input.IsActionJustPressed("spawnRigidBody") && !Settings.userIsInteractGUI)
		{
			_AddObject();
		}
		else if (Input.IsActionPressed("spamBodies"))
		{
			_AddObject();
		}
		else if (Input.IsActionJustPressed("clearBodies"))
		{
			ClearBodies();
		}
		else if (Input.IsActionJustReleased("clearGenerators"))
		{
			ClearGenerators();
		}
	}
	private void _AddObject()
	{
        string bodySceneName = Settings.currentObjectName.Substring(Settings.currentObjectName.IndexOf(":") + 2);
        if (Settings.currentObjectName.Contains("Body: "))
		{
			RigidBody2D body = (RigidBody2D)bodyScene.Instantiate();
			body.Position = GetViewport().GetMousePosition();

			body.GetChild<Sprite2D>(0).Scale = Settings.Scale;
			body.GetChild<Node2D>(1).Scale = Settings.Scale; // Change collision Scale

			PhysicsMaterial uniquePhysicMaterial = new PhysicsMaterial();
			uniquePhysicMaterial.Bounce = Settings.physicsParametrs["Bounce"];
			uniquePhysicMaterial.Friction = Settings.physicsParametrs["Friction"];

			body.GravityScale = Settings.Gravity;
			body.Mass = Settings.physicsParametrs["Mass"];
			body.AddConstantTorque(Settings.physicsParametrs["Torque"] * 100 * body.Mass);
			//body.Inertia = Settings.physicsParametrs["Inertia"];
			body.PhysicsMaterialOverride = uniquePhysicMaterial;

			RigidBodies.Add(body);
			AddChild(body);
		}
		else if (Settings.currentObjectName.Contains("Generator: "))
		{
            PackedScene generatorScene = (PackedScene)ResourceLoader.Load($"res://Scenes/Generators/{bodySceneName}.tscn");
			ParentGenerator generator = (ParentGenerator)generatorScene.Instantiate();
			generator.Position = GetViewport().GetMousePosition();

			Generators.Add(generator);
			AddChild(generator);
		}
		else if (Settings.currentObjectName.Contains("Interaction: "))
		{
			if (bodySceneName == "Atomic Bomb")
			{
				PackedScene bombScene = (PackedScene)ResourceLoader.Load($"res://Scenes/InteractionObjects/{bodySceneName}.tscn");
                RigidBody2D body = (RigidBody2D)bombScene.Instantiate();
                body.Position = GetViewport().GetMousePosition();

                RigidBodies.Add(body);
                AddChild(body);
            }
		}
	}
	public void ClearListAndDeleteChilds<T>(List<T> list)
	{
		list.ForEach(generator => RemoveChild(generator as Node));
		list.Clear();
	}

	public void ClearBodies() { ClearListAndDeleteChilds(RigidBodies); }
	public void ClearGenerators() { ClearListAndDeleteChilds(Generators); }
}
