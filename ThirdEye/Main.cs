using CameraManager;
using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Object;
using static UnityModManagerNet.UnityModManager;

namespace ThirdEye;

public static class Main
{
	public static readonly string CAMERA_NAME = "Camera (third eye)";
	public static readonly string CAMERA2_NAME = "SecondCamera (third eye)";
	public static readonly string CAMERA3_NAME = "ThirdCamera (third eye)";
	private static readonly int DEPTH_OFFSET = 10;
	private static Camera? camera;
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

		if (!VRManager.IsVREnabled())
		{
			modEntry.Logger.Log("ThirdEye only works in VR mode.");
			return true;
		}

		PlayerManager.CameraChanged += OnPlayerCameraChanged;

		return true;
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
			//   2. SecondCamera - ???, depth 1 (Layer 32)
			//   3. ThridCamera  - renders UI, depth 2 (Layer 4)
			Camera playerCamera = PlayerManager.PlayerCamera;
			Camera secondCamera = new List<Camera>(Camera.allCameras).Find(cam => cam.name == "SecondCamera");
			Camera thirdCamera = new List<Camera>(Camera.allCameras).Find(cam => cam.name == "ThirdCamera");

			if (playerCamera == null || secondCamera == null || thirdCamera == null)
			{
				// TODO: log error
				return;
			}

			// Camera
			camera = CameraAPI.CloneCamera(playerCamera, false);
			camera.gameObject.transform.SetParent(playerCamera.gameObject.transform, false);
			camera.gameObject.name = CAMERA_NAME;
			camera.stereoTargetEye = StereoTargetEyeMask.None;
			camera.depth += DEPTH_OFFSET;
			camera.fieldOfView = 60; // TODO: make a mod setting for FOV
			camera.nearClipPlane = 0.01f;

			// Camera 2
			camera2 = CameraAPI.CloneCamera(secondCamera, false);
			camera2.gameObject.transform.SetParent(secondCamera.gameObject.transform, false);
			camera2.gameObject.name = CAMERA2_NAME;
			camera2.stereoTargetEye = StereoTargetEyeMask.None;
			camera2.depth += DEPTH_OFFSET;
			camera2.fieldOfView = 60; // TODO: make a mod setting for FOV
			camera2.nearClipPlane = 0.01f;

			// Camera 3
			camera3 = CameraAPI.CloneCamera(thirdCamera, false);
			camera3.gameObject.transform.SetParent(thirdCamera.gameObject.transform, false);
			camera3.gameObject.name = CAMERA3_NAME;
			camera3.stereoTargetEye = StereoTargetEyeMask.None;
			camera3.depth += DEPTH_OFFSET;
			camera3.fieldOfView = 60; // TODO: make a mod setting for FOV
			camera3.nearClipPlane = 0.01f;
			return;
		}

		if (camera != null)
		{
			Destroy(camera.gameObject);
			camera = null;
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
}
