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

﻿using UnityEngine;
using System.Collections;

public class SubtitleDemo : MonoBehaviour {

	TextMesh m_RawMarkerText = null;
	TextMesh m_SubtitleText = null;

#if (UNITY_WP8 || UNITY_IOS || UNITY_ANDROID || UNITY_SWITCH) && !UNITY_EDITOR
	private const string instructionString = "Tap the screen to start demo";
#else

#if UNITY_PS4 || UNITY_PSP2
	private const string activateButtonName = "X";
#elif UNITY_XBOXONE
	private const string activateButtonName = "A";
#else
	private const string activateButtonName = "e";
#endif

	private const string instructionString = "Press \'" + activateButtonName + "\' on the button to start demo";
#endif

	// Array containing the subtitles.
	private static string[] ms_EnglishSubtitles = new string[]
	{
		"",
		"In this tutorial...",
		"...we will look at creating...",
		"...actor-mixers...",
		"...and control buses.",
		"We will also look at the...",
		"...actor-mixer and master-mixer structures...",
		"...and how to manage these structures efficiently.",
		"END OF DEMO."
	};

	// Use this for initialization
	void Start ()
	{
		// Get the subtitle text component here, to avoid doing it every callback.
		TextMesh[] foundText = gameObject.GetComponentsInChildren<TextMesh>();
		foreach (TextMesh text in foundText)
		{
			if (text.name == "Subtitle Text")
			{
				m_SubtitleText = text;
			}
			if (text.name == "Raw Marker Text")
			{
				m_RawMarkerText = text;
			}

			if (text.name == "Instruction Text")
			{
				text.text = SubtitleDemo.instructionString;
			}
		}
	}

#if !UNITY_SERVER
	void MarkerCallback(AkEventCallbackMsg callbackInfo)
	{
		switch (callbackInfo.type)
		{
			case AkCallbackType.AK_Marker:
				var MarkerCallbackInfo = callbackInfo.info as AkMarkerCallbackInfo;
				m_RawMarkerText.text = MarkerCallbackInfo.strLabel;
				m_SubtitleText.text = ms_EnglishSubtitles[MarkerCallbackInfo.uIdentifier];
				break;
		}
	}
#endif
}
