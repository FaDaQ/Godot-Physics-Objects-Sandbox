using Godot;
using Godot.Collections;
using System.Threading;
using System;

public partial class AtomicBomb : Area2D
{
	public float FallSpeed = 1f;
	public float ExplosiveRadius = 10f;
	public float ExplosivePower = 1000;
	public Godot.Collections.Dictionary<string, float> parametrs = new Godot.Collections.Dictionary<string, float>();

	private Array<Node> namesArr;
	private Array<Node> parametrsArr;
	private CollisionShape2D BombHitbox;
	private bool collisionDetected = false;
	private double _waiteableTime = 0.1;

	public override void _Ready()
	{
		BombHitbox = GetNode<CollisionShape2D>("Bomb Hitbox");
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
		if (bool.Parse(GetMeta("setupIsOver").AsString()))
		{
			Position += new Vector2(0, (FallSpeed += ((float)delta * 9.81f)/0.01f) * (float)delta);
		}
		else
		{
			ReadParametrs();

			FallSpeed = parametrs["Fall Speed"];
			ExplosiveRadius = parametrs["Explosive Radius"];
			ExplosivePower = parametrs["Explosive Power"];
		}
		if (GetOverlappingBodies().Count > 0 && !collisionDetected)
		{
			collisionDetected = true;
			BombHitbox.Shape = new CircleShape2D() { Radius = ExplosiveRadius * 10 };
		}
		else if (collisionDetected)
		{
			_waiteableTime -= delta;
		}
		if (_waiteableTime <= 0)
		{
			Explode();
			Remove();
		}
	}
	public void Explode()
	{
		var bodies = GetOverlappingBodies();
		foreach (var body in bodies)
		{
			if (body is RigidBody2D rigid)
			{
				
				var direction = (body.Position - Position).Normalized();
				
				var distance = Position.DistanceTo(body.Position);
				var force = ExplosivePower * (1 - distance / ExplosiveRadius);
				
				rigid.ApplyCentralImpulse(-direction * force);
			}
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
		ObjectsSpawn.Objects.Remove(this);
	}
}
