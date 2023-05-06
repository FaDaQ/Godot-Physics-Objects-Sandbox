using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class BodiesGenerator : Generator
{
	private Array<Node> namesArr;
	private Array<Node> parametrsArr;
	public Godot.Collections.Dictionary<string, float> parametrs = new Godot.Collections.Dictionary<string, float>();
	private string _bodyName;
	private Vector2 _bodyScale;
	Random rnd = new Random();

	public override void _Ready()
	{
		base._Ready();
		
		namesArr = GetChild(0).GetChild(0).GetChild(0).GetChildren();
		parametrsArr = GetChild(0).GetChild(0).GetChild(1).GetChildren();

		for (int i = 0; i < namesArr.Count; i++)
		{
			parametrs.Add(((Label)namesArr[i]).Text, float.Parse(((LineEdit)parametrsArr[i]).Text.Replace('.', ',')));
		}
		_bodyScale = new Vector2(parametrs["Scale"], parametrs["Scale"]);
	}
	public override void _Process(double delta)
	{
		if (!setupOver)
		{
			ReadParametrs();
		}
	}
	public override void Generate()
	{
		RigidBody2D body = (RigidBody2D)_bodyScene.Instantiate();
		body.Position = _generatorPosition;

		body.GetChild<Sprite2D>(0).Scale = _bodyScale;
		body.GetChild<Node2D>(1).Scale = _bodyScale; // Change collision Scale

		PhysicsMaterial uniquePhysicMaterial = new PhysicsMaterial();
		uniquePhysicMaterial.Bounce = parametrs["Bounce"];
		uniquePhysicMaterial.Friction = parametrs["Friction"];

		body.GravityScale = Settings.Gravity;
		body.Mass = parametrs["Mass"];
		//body.Inertia = Settings.physicsParametrs["Inertia"];
		body.PhysicsMaterialOverride = uniquePhysicMaterial;
		body.AddConstantTorque(parametrs["Torque"] * 100);

		if (Settings.RandomColors)
			body.GetChild<Sprite2D>(0).SelfModulate = new Color(rnd.NextSingle(), rnd.NextSingle(), rnd.NextSingle());

		ObjectsSpawn.Objects.Add(body);
		Settings.AnyEntity.Add(body);

		_parent.AddChild(body);
	}
	public override void SetupOver()
	{
		GetChild<PanelContainer>(0).Hide();

		_bodyScene = (PackedScene)ResourceLoader.Load($"res://Scenes/RigidBodies/{_bodyName}.tscn");

		ObjectsSpawn.Generators.Add(this);
		setupOver = true;
		_timer.Start();
	}
	public override void ReadParametrs()
	{
		base.ReadParametrs();

		int j = 0;
		foreach (var i in parametrs)
			parametrs[i.Key] = float.Parse(((LineEdit)parametrsArr[j++]).Text.Replace('.', ','));
		
		_bodyName = GetNode<OptionButton>("Generator Settings Panel/Select Rigid Body").Text;
		_bodyScale = new Vector2(parametrs["Scale"], parametrs["Scale"]);
	}
}
