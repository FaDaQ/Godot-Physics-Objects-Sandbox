using Godot;
using Godot.Collections;
using System;

public partial class AtomicBombSettingsPanel : PanelContainer
{
	public override void _Ready()
	{
		this.ConnectingUISignals_InContainer();
	}
}
