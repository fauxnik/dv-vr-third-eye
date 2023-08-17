using CameraManager;
using System;
using UnityEngine;
using static UnityEngine.Object;
using UnityModManagerNet;
using static UnityModManagerNet.UnityModManager;
using static UnityModManagerNet.UnityModManager.ModSettings;

namespace ThirdEye;

public static class Main
{
	public static Settings settings = new Settings();
	private static AmalgamCamera? thirdEye;

	// Unity Mod Manage Wiki: https://wiki.nexusmods.com/index.php/Category:Unity_Mod_Manager
	private static bool Load(ModEntry modEntry)
	{
		ModEntry? cameraManagerEntry = FindMod("CameraManager");
		if (cameraManagerEntry == null || cameraManagerEntry.Active == false)
		{
			modEntry.Logger.LogException(new Exception("Thrid Eye requires Camera Manager, but it either isn't installed or isn't active."));
			return false;
		}

		try
		{
			settings = Load<Settings>(modEntry);
		}
		catch (Exception e)
		{
			modEntry.Logger.LogException("Error loading mod settings:", e);
			settings = new Settings();
		}
		modEntry.OnGUI = settings.Draw;
		modEntry.OnSaveGUI = settings.Save;
		Settings.OnSettingsChagned += OnSettingsChanged;

		if (!VRManager.IsVREnabled())
		{
			modEntry.Logger.Log("Third Eye only works in VR mode.");
			return true;
		}

		PlayerManager.CameraChanged += OnPlayerCameraChanged;

		return true;
	}

	private static void OnSettingsChanged()
	{
		if (thirdEye == null) { return; }
		thirdEye.fieldOfView = settings.fieldOfView;
		thirdEye.nearClipPlane = settings.nearClipPlane;
	}

	private static void OnPlayerCameraChanged()
	{
		SetupThirdEye(false);
		SetupThirdEye(true);
	}

	private static void SetupThirdEye(bool on)
	{
		if (on)
		{
			thirdEye = new GameObject() { name = "ThirdEye" }.AddComponent<AmalgamCamera>();
			thirdEye.Init(CameraManager.CameraType.All);
			thirdEye.gameObject.transform.SetParent(PlayerManager.PlayerCamera.gameObject.transform, false);
			thirdEye.stereoTargetEye = StereoTargetEyeMask.None;
			thirdEye.fieldOfView = settings.fieldOfView;
			thirdEye.nearClipPlane = settings.nearClipPlane;
			return;
		}

		if (thirdEye == null) { return; }
		Destroy(thirdEye);
		thirdEye = null;
	}
}
