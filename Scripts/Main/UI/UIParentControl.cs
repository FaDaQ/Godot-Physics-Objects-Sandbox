using Godot;
using System;

public partial class UIParentControl : Control
{
	public override void _Ready()
	{
		this.ConnectingUISignals();
	}
}
