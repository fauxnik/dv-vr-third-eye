using UnityEngine;

namespace CameraManager;

internal static class Utilities
{
	internal static void ClearTexture(RenderTexture tex, bool clearDepth = true, bool clearColor = true, Color? backgroundColor = null, float depth = 1)
	{
		Color solidColor = backgroundColor ?? Color.clear;
		RenderTexture rt = RenderTexture.active;
		RenderTexture.active = tex;
		GL.Clear(clearDepth, clearColor, solidColor, depth);
		RenderTexture.active = rt;
	}
}
