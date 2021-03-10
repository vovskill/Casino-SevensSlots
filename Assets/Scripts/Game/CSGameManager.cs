using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class CSGameManager : MonoBehaviour {

	public static CSGameManager instance = null;
    [HideInInspector] public bool expandWild;

    [Header("Links")]
    public string androidAppLink = "market://details?id=YOUR_ID";
    public string iosAppLink = "itms-apps://itunes.apple.com/app/idYOUR_ID";

    [Header("Other")]
    public string supportEmail = "mirkobil.mirpayziev@gmail.com";

	void Awake ()
	{
		if (instance == null)
		{
			DontDestroyOnLoad (gameObject);
			instance = this;
            Loaded();
		}
		else if (instance != this)
		{
			Destroy (gameObject);
		}
	}

    private void Loaded()
    {
        Application.targetFrameRate = 60;
    }


    public void Rate()
    {
#if UNITY_ANDROID
        Application.OpenURL(androidAppLink);
#elif UNITY_IPHONE
        Application.OpenURL(iosAppLink);
#endif
    }


}
