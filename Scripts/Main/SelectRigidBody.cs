using Godot;
using System;

public partial class SelectRigidBody : OptionButton
{
    public override void _Ready()
    {
        Settings.currentBodyID = Selected;
        Settings.currentObjectName = Text;
        string bodySceneName = Settings.currentObjectName.Substring(Settings.currentObjectName.IndexOf(":") + 2);

        ObjectsSpawn.bodyScene = (PackedScene)ResourceLoader.Load($"res://Scenes/RigidBodies/{bodySceneName}.tscn");
    }

    public void SetCurrentBodyID(long index)
	{
		Settings.currentBodyID = (int)index;
		Settings.currentObjectName = GetItemText((int)index);
		string bodySceneName = Settings.currentObjectName.Substring(Settings.currentObjectName.IndexOf(":") + 2);

        ObjectsSpawn.bodyScene = (PackedScene)ResourceLoader.Load($"res://Scenes/RigidBodies/{bodySceneName}.tscn");
	}
}
