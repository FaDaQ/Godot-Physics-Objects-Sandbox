using Godot;
using Godot.Collections;
using System.Threading;
using System;

public partial class AtomicBomb : RigidBody2D
{
	public float FallSpeed = 1f;
	public float ExplosiveRadius = 10f;
	public float ExplosivePower = 1000;
	public Godot.Collections.Dictionary<string, float> parametrs = new Godot.Collections.Dictionary<string, float>();

	private Array<Node> namesArr;
	private Array<Node> parametrsArr;
	private Area2D BombHitboxArea;
	private bool collisionDetected = false;
	private double _waiteableTime = 0.10;

	public override void _Ready()
	{
		BombHitboxArea = GetNode<Area2D>("Bomb Hitbox Area");
		namesArr = GetChild(2).GetChild(0).GetChildren();
		parametrsArr = GetChild(2).GetChild(1).GetChildren();

		for (int i = 0; i < namesArr.Count; i++)
		{
			parametrs.Add(((Label)namesArr[i]).Text, float.Parse(((LineEdit)parametrsArr[i]).Text.Replace('.', ',')));
		}
		Settings.AnyEntity.Add(this);
	}

	public override void _PhysicsProcess(double delta)
	{
		ReadParametrs();

		GravityScale = parametrs["Fall Speed"]; // Yes-yes, ha-ha...
		ExplosiveRadius = parametrs["Explosive Radius"];
		ExplosivePower = parametrs["Explosive Power"] * 100;

		if (BombHitboxArea.GetOverlappingBodies().Count > 0 || collisionDetected)
		{
			BombHitboxArea.GetChild<CollisionShape2D>(0).Shape = new CircleShape2D() { Radius = ExplosiveRadius * 15 }; // 15 - for not writing too big number in parametr
			_waiteableTime -= delta;
			collisionDetected = true;
			Hide();
		}
		if (_waiteableTime <= 0)
		{
			int j = 0;
			foreach (Node2D i in BombHitboxArea.GetOverlappingBodies())
			{
				if (i is RigidBody2D body)
					body.ApplyForce((j++ % 2 == 0) ? new Vector2(ExplosivePower, 0) : new Vector2(0, ExplosivePower));
#if DEBUG
				Console.WriteLine($"Atomic Bpmb Colliding: {i}");
#endif
			}
			Remove();

		}

	}
	public void ReadParametrs()
	{
		int j = 0;
		foreach (var i in parametrs)
			parametrs[i.Key] = float.Parse(((LineEdit)parametrsArr[j++]).Text.Replace('.', ','));
	}
	public void Remove()
	{
		this.Cancel();
		Settings.AnyEntity.Remove(this);
		ObjectsSpawn.RigidBodies.Remove(this);
	}

}
