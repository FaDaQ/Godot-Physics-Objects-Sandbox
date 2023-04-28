using Godot;
using System;

public static class Extension
{
    public static void ExplosionReact(this RigidBody2D body, float explosionPower)
    {
        int j = 1;
        foreach (var i in body.GetCollidingBodies())
        {
            if (i is RigidBody2D collidingBody)
                if (j % 2 == 0)
                    collidingBody.ApplyForce(new Vector2(explosionPower, 0), new Vector2(0, explosionPower));
                else
                    collidingBody.ApplyForce(new Vector2(0, explosionPower), new Vector2(explosionPower, 0));

            j++;
        }
    }

    public static void ConnectingUISignals_InContainer(this Container container)
    {
        Settings crutch = new Settings();

        container.Connect("mouse_entered", Callable.From(() => crutch.userInteractWithGUI()));
        container.Connect("mouse_exited", Callable.From(() => crutch.userEndInteractWithGUI()));

        foreach (var i in container.GetChildren())
        {
            i.Connect("mouse_entered", Callable.From(() => crutch.userInteractWithGUI()));
            i.Connect("mouse_exited", Callable.From(() => crutch.userEndInteractWithGUI()));
            if (i is Button button)
                button.Connect("hidden", Callable.From(() => crutch.userEndInteractWithGUI()));
        }
    }

    public static void ConnectingUISugnals_Single(this Control node)
    {
        Settings crutch = new Settings();
        node.Connect("mouse_entered", Callable.From(() => crutch.userInteractWithGUI()));
        node.Connect("mouse_exited", Callable.From(() => crutch.userEndInteractWithGUI()));
        if (node is Button button)
            button.Connect("hidden", Callable.From(() => crutch.userEndInteractWithGUI()));
    }

    // Remove node from this Parent
    public static void Cancel(this Node2D node)
    {
        node.GetParent().RemoveChild(node);
    }
}
