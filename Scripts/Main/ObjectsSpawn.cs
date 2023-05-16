using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class ObjectsSpawn : Node2D
{
	public static List<Node2D> Objects = new List<Node2D>();
	public static List<Generator> Generators = new List<Generator>();

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
		else if (Input.IsActionJustPressed("cancelAction"))
		{
			Settings.CancelAction();
			Settings.userIsInteractGUI = false;
		}
	}
	private void _AddObject()
	{
		string bodySceneName = Settings.currentObjectName.Substring(Settings.currentObjectName.IndexOf(":") + 2);
		Random rnd = new Random();
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
			body.AddConstantTorque(Settings.physicsParametrs["Torque"] * 100 * body.Mass); // 100 - In order not to use huge numbers
			//body.Inertia = Settings.physicsParametrs["Inertia"];
			body.PhysicsMaterialOverride = uniquePhysicMaterial;

			if (Settings.RandomColors)
				body.GetChild<Sprite2D>(0).SelfModulate = new Color(rnd.NextSingle(), rnd.NextSingle(), rnd.NextSingle());
			else
				body.GetChild<Sprite2D>(0).SelfModulate = Settings.BodyColor;
			if (Settings.RandomTorque)
				body.ConstantTorque = rnd.Next(-Settings.MaxRandomTorque, Settings.MaxRandomTorque) * 100 * body.Mass;
			if (Settings.RandomScale)
			{
				float scaleValue = rnd.NextSingle() * (Settings.MaxRandomScale - Settings.MinRandomScale) + Settings.MinRandomScale;
				body.GetChild<Sprite2D>(0).Scale = new Vector2(scaleValue, scaleValue);
				body.GetChild<Node2D>(1).Scale = new Vector2(scaleValue, scaleValue);
			}

			Objects.Add(body);
			Settings.AnyEntity.Add(body);
			AddChild(body);
		}
		else if (Settings.currentObjectName.Contains("Generator: "))
		{
			PackedScene generatorScene = (PackedScene)ResourceLoader.Load($"res://Scenes/Generators/{bodySceneName}.tscn");
			Generator generator = (Generator)generatorScene.Instantiate();
			generator.Position = GetViewport().GetMousePosition();

			Generators.Add(generator);
			AddChild(generator);
		}
		else if (Settings.currentObjectName.Contains("Interaction: "))
		{
			PackedScene interactionScene = (PackedScene)ResourceLoader.Load($"res://Scenes/InteractionObjects/{bodySceneName}.tscn");
			Node2D interactionObject = null;
			interactionObject = (Node2D)interactionScene.Instantiate();
			interactionObject.Position = GetViewport().GetMousePosition();
			Objects.Add(interactionObject);
			AddChild(interactionObject);
		}
	}
	public void ClearListAndDeleteChilds<T>(List<T> list)
	{
		list.ForEach(generator => RemoveChild(generator as Node));
		list.Clear();
	}

	public void ClearBodies()
	{
		Settings.AnyEntity.ForEach(node => { if (node is PhysicsBody2D || node is CollisionObject2D) node.Cancel(); });
		for (int i = Objects.Count - 1; i >= 0; i--)
			Settings.AnyEntity.Remove(Objects[i]);
		Objects.Clear();
	}
	public void ClearGenerators()
	{
		Settings.AnyEntity.ForEach(node => { if (node is Generator generator) generator.Cancel(); });

		List<Generator> list = Settings.AnyEntity.OfType<Generator>().ToList();
		foreach (var i in list)
			Settings.AnyEntity.Remove(i);

		Generators.Clear();
	}
	public void ClearAllObjects() // Connecting to Clear All Button
	{
		ClearBodies();
		ClearGenerators();
		CollisionArea.ClearCollisionLines();
		Settings.AnyEntity.ForEach(node => node.Cancel());
		Settings.AnyEntity.Clear();
		foreach (Node2D i in GetChildren())
			i.Cancel();
	}
}
