﻿using CameraManager;
using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Object;
using UnityModManagerNet;
using static UnityModManagerNet.UnityModManager;
using static UnityModManagerNet.UnityModManager.ModSettings;

namespace ThirdEye;

public static class Main
{
	public static Settings settings = new Settings();
	public static readonly string CAMERA1_NAME = "Camera (third eye)";
	public static readonly string CAMERA2_NAME = "SecondCamera (third eye)";
	public static readonly string CAMERA3_NAME = "ThirdCamera (third eye)";
	private static readonly int DEPTH_OFFSET = 10;
	private static Camera? camera1;
	private static Camera? camera2;
	private static Camera? camera3;

	// Unity Mod Manage Wiki: https://wiki.nexusmods.com/index.php/Category:Unity_Mod_Manager
	private static bool Load(ModEntry modEntry)
	{
		ModEntry? cameraManagerEntry = FindMod("CameraManager");
		if (cameraManagerEntry == null || cameraManagerEntry.Active == false)
		{
			modEntry.Logger.LogException(new Exception("ThridEye requires CameraManager, but it either isn't installed or isn't active."));
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
			modEntry.Logger.Log("ThirdEye only works in VR mode.");
			return true;
		}

		PlayerManager.CameraChanged += OnPlayerCameraChanged;

		return true;
	}

	private static void OnSettingsChanged()
	{
		Camera?[] cameras = { camera1, camera2, camera3 };
		foreach (Camera? camera in cameras)
		{
			if (camera == null) { continue; }
			camera.fieldOfView = settings.fieldOfView;
			camera.nearClipPlane = settings.nearClipPlane;
		}
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
			// Derail Valley uses 3 cameras
			//   1. Camera (eye) - renders most things, depth 0 (Layers 267386643)
			//   2. SecondCamera - renders ???, depth 1 (Layer 32)
			//   3. ThridCamera  - renders UI, depth 2 (Layer 4)
			Camera playerCamera = PlayerManager.PlayerCamera;
			Camera secondCamera = new List<Camera>(Camera.allCameras).Find(cam => cam.name == "SecondCamera");
			Camera thirdCamera = new List<Camera>(Camera.allCameras).Find(cam => cam.name == "ThirdCamera");

			if (playerCamera == null || secondCamera == null || thirdCamera == null)
			{
				// TODO: log error
				return;
			}

			camera1 = CloneCamera(playerCamera, CAMERA1_NAME);
			camera2 = CloneCamera(secondCamera, CAMERA2_NAME);
			camera3 = CloneCamera(thirdCamera, CAMERA3_NAME);
			return;
		}

		if (camera1 != null)
		{
			Destroy(camera1.gameObject);
			camera1 = null;
		}
		if (camera2 != null)
		{
			Destroy(camera2.gameObject);
			camera2 = null;
		}
		if (camera3 != null)
		{
			Destroy(camera3.gameObject);
			camera3 = null;
		}
	}

	private static Camera CloneCamera(Camera source, string name)
	{
		Camera camera = CameraAPI.CloneCamera(source, false);
		camera.gameObject.transform.SetParent(source.gameObject.transform, false);
		camera.gameObject.name = name;
		camera.stereoTargetEye = StereoTargetEyeMask.None;
		camera.depth += DEPTH_OFFSET;
		camera.fieldOfView = settings.fieldOfView;
		camera.nearClipPlane = settings.nearClipPlane;
		return camera;
	}
}
