using Godot;
using System;
using System.Collections.Generic;

// Generating a line collision taking into account its thickness
public partial class Line2DCollision : StaticBody2D
{
	public override void _Ready()
	{
		GenerateCollisionShapes(GetChild<Line2D>(0), this);
	}

	public void GenerateCollisionShapes(Line2D line, Node parent)
	{
		float halfWidth = line.Width / 2;
		Vector2[] linePoints = line.Points;

		for (int i = 0; i < linePoints.Length - 1; i++)
		{
			Vector2 currentPoint = linePoints[i];
			Vector2 nextPoint = linePoints[i + 1];
			Vector2 direction = (nextPoint - currentPoint).Normalized();
			Vector2 perpendicular = new Vector2(-direction.Y, direction.X);

			Vector2 topLeft = currentPoint + perpendicular * halfWidth;
			Vector2 topRight = nextPoint + perpendicular * halfWidth;
			Vector2 bottomRight = nextPoint - perpendicular * halfWidth;
			Vector2 bottomLeft = currentPoint - perpendicular * halfWidth;

			ConvexPolygonShape2D shape = new ConvexPolygonShape2D();
			shape.Points = (new Vector2[] { topLeft, topRight, bottomRight, bottomLeft });

			CollisionShape2D collisionShape = new CollisionShape2D();
			collisionShape.Shape = shape;
			parent.AddChild(collisionShape);
		}
	}
}
