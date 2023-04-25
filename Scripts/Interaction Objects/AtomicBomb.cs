using Godot;
using System;

public partial class AtomicBomb : RigidBody2D
{
	public float FallSpees = 100f;
	public float ExplosiveRadius = 20f;
	private Godot.Collections.Array<Node2D> _collidedObjects;

	private double _waitTime = 0.2;
	private double _currentTime = 0;

	public override void _Ready()
	{
		
	}

	public override void _PhysicsProcess(double delta)
	{
		_collidedObjects = GetCollidingBodies();
		if (_collidedObjects.Count > 0)
		{
			GetChild<CollisionShape2D>(1).Shape = new CircleShape2D() { Radius = ExplosiveRadius * 10 };
			_currentTime += delta;
		}
		if (_currentTime >= _waitTime)
		{
			foreach (var i in _collidedObjects)
			{
				if (i is RigidBody2D body)
					body.ApplyImpulse(new Vector2(100000, 1000));
			}
			GetParent().RemoveChild(this);
			ObjectsSpawn.RigidBodies.Remove(this);
			Console.WriteLine("Yes");
		}
	}

}
