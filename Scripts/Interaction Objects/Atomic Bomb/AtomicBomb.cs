using Godot;
using Godot.Collections;
using System;

public partial class AtomicBomb : RigidBody2D
{
	public float FallSpeed = 1f;
	public float ExplosiveRadius = 10f;
	public float ExplosivePower = 1000;
	private Godot.Collections.Array<Node2D> _collidedObjects;

	private double _waitTime = 0.05;
	private double _currentTime = 0;

	private Array<Node> namesArr;
	private Array<Node> parametrsArr;
	public Godot.Collections.Dictionary<string, float> parametrs = new Godot.Collections.Dictionary<string, float>();
	private string _bodyName;
	private Vector2 _bodyScale;

	public override void _Ready()
	{
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

		_collidedObjects = GetCollidingBodies();
		if (_collidedObjects.Count > 0)
		{
			Hide();
			GetChild<CollisionShape2D>(1).Shape = new CircleShape2D() { Radius = ExplosiveRadius * 10 }; // Why 10? Just...
			_currentTime += delta;
		}
		if (_currentTime >= _waitTime)
		{
			foreach (var i in _collidedObjects)
			{
				if (i is RigidBody2D body)
					body.ExplosionReact(ExplosivePower);
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
		GetParent().RemoveChild(this);
		Settings.AnyEntity.Remove(this);
		ObjectsSpawn.RigidBodies.Remove(this);
	}

}
