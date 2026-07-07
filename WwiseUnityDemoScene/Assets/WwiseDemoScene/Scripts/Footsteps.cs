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

public class Footsteps : MonoBehaviour
{
#if !UNITY_SERVER
	public AK.Wwise.Event FootStepEvent = new AK.Wwise.Event();

	const float walkStepInterval = 0.5f;
	float currentStepTime = 0.0f;

	Vector3 lastPos = new Vector3(0, 0, 0);

	private void Awake()
	{
		lastPos = transform.position;
	}

	private void FixedUpdate()
	{
		bool isMoving = (lastPos.x != transform.position.x || lastPos.z != transform.position.z);
		lastPos = transform.position;

		currentStepTime += Time.fixedDeltaTime;

		if (isMoving && currentStepTime > walkStepInterval)
		{
			FootStepEvent.Post(gameObject);
			currentStepTime = 0.0f;
		}
	}
#endif
}
