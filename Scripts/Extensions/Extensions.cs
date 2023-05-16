using Godot;
using System;

public static class Extension
{
    public static void ConnectingUISignals(this Control control)
    {
        control.Connect("mouse_entered", Callable.From(() => Settings.userIsInteractGUI = true));
        control.Connect("mouse_exited", Callable.From(() => Settings.userIsInteractGUI = false));
        control.Connect("hidden", Callable.From(() => Settings.userIsInteractGUI = false));

        foreach (Node child in control.GetChildren())
        {
            if (child is Control childControl)
            {
                childControl.Connect("mouse_entered", Callable.From(() => Settings.userIsInteractGUI = true));
                childControl.Connect("mouse_exited", Callable.From(() => Settings.userIsInteractGUI = false));
                childControl.Connect("hidden", Callable.From(() => Settings.userIsInteractGUI = false));
            }
            if (child is Button button && button.Name == "Close")
                button.Connect("pressed", Callable.From(() => button.GetParent<Control>().Hide()));
            if (child.GetChildCount() > 0)
            {
                Console.WriteLine(">>>" + child.Name);
                ConnectChildSignals(child);
            }
        }
    }
    private static void ConnectChildSignals(Node parent)
    {
        foreach (Node child in parent.GetChildren())
        {
            if (child is Control childControl)
            {
                childControl.Connect("mouse_entered", Callable.From(() => Settings.userIsInteractGUI = true));
                childControl.Connect("mouse_exited", Callable.From(() => Settings.userIsInteractGUI = false));
                childControl.Connect("hidden", Callable.From(() => Settings.userIsInteractGUI = false));
            }
            if (child is Button button && button.Name == "Close")
                button.Connect("pressed", Callable.From(() => button.GetParent<Control>().Hide()));
            if (child.GetChildCount() > 0)
            {
                Console.WriteLine(">>>" + child.Name);
                ConnectChildSignals(child);
            }
        }
    }
    // Remove node from this Parent
    public static void Cancel(this Node2D node)
    {
#if DEBUG
        Console.WriteLine($"Removing: [{node}] [{node.Name}] [Parent: [{node.GetParent()}] [{node.GetParent().Name}]]");
#endif
        if (node is Reflector reflector)
            reflector.TrajectoryLine.GetParent().RemoveChild(reflector.TrajectoryLine);
        node.GetParent().RemoveChild(node);
    }
    
}
