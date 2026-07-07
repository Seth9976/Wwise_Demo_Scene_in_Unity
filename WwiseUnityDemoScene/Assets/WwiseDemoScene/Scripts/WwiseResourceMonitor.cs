/*******************************************************************************
The content of this file includes portions of the proprietary AUDIOKINETIC Wwise
Technology released in source code form as part of the game integration package.
The content of this file may not be used without valid licenses to the
AUDIOKINETIC Wwise Technology.
Note that the use of the game engine is subject to the Unity(R) Terms of
Service at https://unity3d.com/legal/terms-of-service
 
License Usage
 
Licensees holding valid licenses to the AUDIOKINETIC Wwise Technology may use
this file in accordance with the end user license agreement provided with the
software or, alternatively, in accordance with the terms contained
in a written agreement between you and Audiokinetic Inc.
Copyright (c) 2026 Audiokinetic Inc.
*******************************************************************************/

﻿public class WwiseResourceMonitor : UnityEngine.MonoBehaviour
{
#if !UNITY_SERVER
	private AkResourceMonitorDataSummary ResourceMonitorDataSummary;

	private void OnEnable()
	{
		ResourceMonitorDataSummary = new AkResourceMonitorDataSummary();
		AkUnitySoundEngine.StartResourceMonitoring();
	}

	private void OnDisable()
	{
		AkUnitySoundEngine.StopResourceMonitoring();
	}

	private void Update()
	{
		AkUnitySoundEngine.GetResourceMonitorDataSummary(ResourceMonitorDataSummary);
	}

	private readonly UnityEngine.Vector2 TextOrigin = new UnityEngine.Vector2(10, 10);
	private readonly UnityEngine.Rect TextBox = new UnityEngine.Rect(0, 0, 200, 20);
	private readonly float VerticalSpacing = 5;

	private void Label(ref UnityEngine.Rect rect, string text)
	{
		UnityEngine.GUI.Label(rect, text, UnityEngine.GUI.skin.button);
		rect.y += rect.height + VerticalSpacing;
	}

	private void OnGUI()
	{
		var rect = TextBox;
		rect.position += TextOrigin;
		var savedContentColor = UnityEngine.GUI.contentColor;
		UnityEngine.GUI.contentColor = UnityEngine.Color.white;
		Label(ref rect, string.Format("{0,-14}{1,14:F4} %", "Total CPU:", ResourceMonitorDataSummary.totalCPU));
		Label(ref rect, string.Format("{0,-14}{1,14:F4} %", "Plug-in CPU:", ResourceMonitorDataSummary.pluginCPU));
		Label(ref rect, string.Format("{0,-16}{1,16}", "Active Events:", ResourceMonitorDataSummary.nbActiveEvents));
		UnityEngine.GUI.contentColor = savedContentColor;
	}
#endif
}
