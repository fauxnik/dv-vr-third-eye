using System;
using UnityModManagerNet;

namespace ThirdEye;

public class Settings : UnityModManager.ModSettings, IDrawable
{

	public static Action? OnSettingsChagned;

	[Draw("Field of view", DrawType.Slider, Min = 40f, Max = 80f)]
	public float fieldOfView = 60f;

	[Draw("Near clip plane", DrawType.Slider, Min = 0.01f, Max = 0.1f)]
	public float nearClipPlane = 0.01f;

	[Draw("Show the camera on the PC monitor? (Enabling this may impact performance.)")]
	public bool showOnPC = false;

	public void OnChange() { OnSettingsChagned?.Invoke(); }

	public override void Save(UnityModManager.ModEntry modEntry)
	{
		Save(this, modEntry);
	}
}
