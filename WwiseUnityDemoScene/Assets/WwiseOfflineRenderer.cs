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

﻿#if UNITY_2019_4_OR_NEWER
public class WwiseOfflineRenderer : UnityEngine.MonoBehaviour
{
#if !UNITY_SERVER
	public string Folder = "ScreenshotFolder";
	public float FrameRate = 25.0f;
	public bool StartWithOfflineRenderingEnabled = false;

	[UnityEngine.SerializeField]
	private bool _isOfflineRendering = false;

	private System.IO.DirectoryInfo _directoryInfo = null;
	private System.IO.BinaryWriter _binaryWriter = null;

	private System.Collections.Generic.Dictionary<uint, float[]> _audioBuffers = new System.Collections.Generic.Dictionary<uint, float[]>();
	private ulong _outputDeviceId = 0;

	private void Start()
	{
		_outputDeviceId = AkUnitySoundEngine.GetOutputID(AkUnitySoundEngine.AK_INVALID_UNIQUE_ID, 0);
		if (StartWithOfflineRenderingEnabled)
			EnableOfflineRendering();
	}

	private void OnDestroy()
	{
		DisableOfflineRendering();
	}

	public void EnableOfflineRendering()
	{
		if (_isOfflineRendering)
			return;

#if UNITY_EDITOR
		AkSoundEngineController.Instance.DisableEditorLateUpdate();
#endif

		_directoryInfo = System.IO.Directory.CreateDirectory(Folder);
		_binaryWriter = new System.IO.BinaryWriter(System.IO.File.Open(_directoryInfo.FullName + System.IO.Path.DirectorySeparatorChar + "audio.raw", System.IO.FileMode.Create));

		var sampleRate = AkUnitySoundEngine.GetSampleRate();
		AkChannelConfig channelConfig = new AkChannelConfig();
		Ak3DAudioSinkCapabilities audioSinkCapabilities = new Ak3DAudioSinkCapabilities();
		AkUnitySoundEngine.GetOutputDeviceConfiguration(_outputDeviceId, channelConfig, audioSinkCapabilities);
		print(string.Format("Output directory: {0}, Initial Frame Number: {1}, Main Audio Device ID: {2}, Sample Rate: {3}, Channels: {4}",
			_directoryInfo.FullName, UnityEngine.Time.frameCount, _outputDeviceId, sampleRate, channelConfig.uNumChannels));

		AkUnitySoundEngine.ClearCaptureData();
		AkUnitySoundEngine.StartDeviceCapture(_outputDeviceId);
		SetOfflineRenderingFrameTime();
		AkUnitySoundEngine.SetOfflineRendering(true);
		_isOfflineRendering = true;
	}

	public void DisableOfflineRendering()
	{
		if (!_isOfflineRendering)
			return;

		if (_binaryWriter != null)
		{
			AkUnitySoundEngine.StopDeviceCapture(_outputDeviceId);
			_binaryWriter.Dispose();
			_binaryWriter = null;
		}

#if UNITY_EDITOR
		AkSoundEngineController.Instance.EnableEditorLateUpdate();
#endif

		AkUnitySoundEngine.SetOfflineRendering(false);
		_isOfflineRendering = false;
		SetOfflineRenderingFrameTime();
	}

	private void SetOfflineRenderingFrameTime()
	{
		var frameTime = _isOfflineRendering && FrameRate != 0.0f ? 1.0f / FrameRate : 0.0f;
		UnityEngine.Time.captureDeltaTime = frameTime;
		AkUnitySoundEngine.SetOfflineRenderingFrameTime(frameTime);
	}

	private void Update()
	{
		SetOfflineRenderingFrameTime();

		if (!_isOfflineRendering)
			return;

		UnityEngine.ScreenCapture.CaptureScreenshot(string.Format("{0}/{1:D04} shot.png", Folder, UnityEngine.Time.frameCount));

		if (_binaryWriter == null)
			return;

		var sampleCount = AkUnitySoundEngine.UpdateCaptureSampleCount(_outputDeviceId);
		if (sampleCount <= 0)
			return;

		float[] buffer = null;
		if (!_audioBuffers.TryGetValue(sampleCount, out buffer))
		{
			buffer = new float[sampleCount];
			_audioBuffers[sampleCount] = buffer;
		}

		if (buffer == null)
			return;

		var count = AkUnitySoundEngine.GetCaptureSamples(_outputDeviceId, buffer, (uint)buffer.Length);
		if (count <= 0)
			return;

		for (var i = 0; i < count; ++i)
			_binaryWriter.Write(buffer[i]);
	}

#if UNITY_EDITOR
	[UnityEditor.CustomEditor(typeof(WwiseOfflineRenderer))]
	private class Editor : UnityEditor.Editor
	{
		private UnityEditor.SerializedProperty Folder;
		private UnityEditor.SerializedProperty FrameRate;
		private UnityEditor.SerializedProperty StartWithOfflineRenderingEnabled;
		private UnityEditor.SerializedProperty _isOfflineRendering;

		private void OnEnable()
		{
			Folder = serializedObject.FindProperty("Folder");
			FrameRate = serializedObject.FindProperty("FrameRate");
			StartWithOfflineRenderingEnabled = serializedObject.FindProperty("StartWithOfflineRenderingEnabled");
			_isOfflineRendering = serializedObject.FindProperty("_isOfflineRendering");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			UnityEditor.EditorGUILayout.PropertyField(Folder, new UnityEngine.GUIContent("Folder"));
			UnityEditor.EditorGUILayout.PropertyField(FrameRate, new UnityEngine.GUIContent("Frame Rate"));

			UnityEngine.GUILayout.Space(UnityEditor.EditorGUIUtility.standardVerticalSpacing);

			var wasEnabled = UnityEngine.GUI.enabled;
			UnityEngine.GUI.enabled = !UnityEditor.BuildPipeline.isBuildingPlayer;

			if (UnityEditor.EditorApplication.isPlaying)
			{
				var isOfflineRendering = _isOfflineRendering.boolValue;
				if (UnityEngine.GUILayout.Button(isOfflineRendering ? "Disable Offline Rendering" : "Enable Offline Rendering"))
				{
					var offlineRenderer = serializedObject.targetObject as WwiseOfflineRenderer;
					if (isOfflineRendering)
						offlineRenderer.DisableOfflineRendering();
					else
						offlineRenderer.EnableOfflineRendering();
				}
			}
			else
			{
				var startWithOfflineRenderingEnabled = StartWithOfflineRenderingEnabled.boolValue;
				if (UnityEngine.GUILayout.Button(startWithOfflineRenderingEnabled ? "Start With Offline Rendering Disabled" : "Start With Offline Rendering Enabled"))
					StartWithOfflineRenderingEnabled.boolValue = !startWithOfflineRenderingEnabled;
			}

			UnityEngine.GUI.enabled = wasEnabled;

			serializedObject.ApplyModifiedProperties();
		}
	}
#endif
#endif
}
#endif // UNITY_2019_4_OR_NEWER
