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

	public void OnChange()
	{
		if (OnSettingsChagned != null)
		{
			OnSettingsChagned();
		}
	}

	public override void Save(UnityModManager.ModEntry modEntry)
	{
		Save(this, modEntry);
	}
}
