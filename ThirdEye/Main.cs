using CameraManager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Object;
using UnityModManagerNet;
using static UnityModManagerNet.UnityModManager;
using static UnityModManagerNet.UnityModManager.ModSettings;

namespace ThirdEye;

public static class Main
{
	public static Settings settings { get; private set; } = new Settings();
	public static AmalgamCamera? camera { get; private set; }
	public static readonly RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 32);
	private static readonly Dictionary<string, bool> renderRequests = new Dictionary<string, bool>();

	public static void RequestRender(string id, bool on)
	{
		renderRequests[id] = on;
	}

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
		if (camera == null) { return; }
		camera.fieldOfView = settings.fieldOfView;
		camera.nearClipPlane = settings.nearClipPlane;
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
			GameObject gameObject = new GameObject() { name = "ThirdEye" };

			camera = gameObject.AddComponent<AmalgamCamera>();
			camera.Init(CameraManager.CameraType.All);
			camera.enabled = false;
			camera.gameObject.transform.SetParent(PlayerManager.PlayerCamera.gameObject.transform, false);
			camera.stereoTargetEye = StereoTargetEyeMask.None;
			camera.targetTexture = renderTexture;
			OnSettingsChanged();

			gameObject.AddComponent<ThirdEyeRenderer>();

			return;
		}

		if (camera == null) { return; }
		Destroy(camera);
		camera = null;
	}

	class ThirdEyeRenderer : MonoBehaviour
	{
		public IEnumerator Start()
		{
			while (this.enabled)
			{
				yield return new WaitForEndOfFrame();

				if (camera == null) { continue; }

				if (settings.showOnPC || renderRequests.ContainsValue(true))
				{
					camera.Render();
				}

				if (settings.showOnPC)
				{
					Graphics.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), renderTexture);
				}
			}
		}
	}
}
