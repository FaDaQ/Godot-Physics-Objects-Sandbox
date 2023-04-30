using Godot;
using System;

public static class Extension
{
    public static void ConnectingUISignals_InContainer(this Container container)
    {
        Settings crutch = new Settings();

        container.Connect("mouse_entered", Callable.From(() => crutch.userInteractWithGUI()));
        container.Connect("mouse_exited", Callable.From(() => crutch.userEndInteractWithGUI()));

        foreach (var i in container.GetChildren())
        {
            i.Connect("mouse_entered", Callable.From(() => crutch.userInteractWithGUI()));
            i.Connect("mouse_exited", Callable.From(() => crutch.userEndInteractWithGUI()));
            i.Connect("hidden", Callable.From(() => crutch.userEndInteractWithGUI()));
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
#if DEBUG
        Console.WriteLine($"Removing: [{node}] [{node.Name}]");
#endif
    }
}
