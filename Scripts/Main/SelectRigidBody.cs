using Godot;
using System;

public partial class SelectRigidBody : OptionButton
{
	// Call in item_selected signal
	public void SetCurrentBodyID(long index)
	{
		Settings.currentBodyID = (int)index;
		Settings.currentBodyName = GetItemText((int)index);
		ObjectsSpawn.bodyScene = (PackedScene)ResourceLoader.Load($"res://Scenes/RigidBodies/{Settings.currentBodyName}.tscn");
	}
}
