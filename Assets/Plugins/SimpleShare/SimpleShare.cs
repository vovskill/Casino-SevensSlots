using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.InteropServices;

public class SimpleShare : MonoBehaviour
{
    public static SimpleShare instance = null;

    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
            Loaded();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Loaded()
    {

    }

#if UNITY_ANDROID

    private void AndroidShare(string text)
    {
        StartCoroutine(Screenshot((tex, path) =>
        {
            AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
            AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
            intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
            AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");

            AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", path);
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), text);

            intentObject.Call<AndroidJavaObject>("setType", "image/png");
            AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
            currentActivity.Call("startActivity", intentObject);
        }));
    }

    private void ShareAndroidScreenShot(string subject, string title, string text)
    {
        StartCoroutine(Screenshot((tex, path) =>
        {
            ShareAndroid(subject, title, text, path);
        }));
    }

    private void ShareAndroidRect(Rect rect, string subject, string title, string text)
    {
        StartCoroutine(CaptureRect(rect, (tex, path) =>
        {
            ShareAndroid(subject, title, text, path);
        }));
    }

    private void ShareAndroidRectTransform(RectTransform rt, string subject, string title, string text)
    {
        StartCoroutine(CaptureRect(RectTransformToRect(rt), (tex, path) =>
        {
            ShareAndroid(subject, title, text, path);
        }));
    }

    private void ShareAndroid(string subject, string title, string text, string path = null)
    {
        bool fileExists = AndroidFileExists(path);
        AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
        AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");

        intentObject.Call("setAction", intentClass.GetStatic<AndroidJavaObject>("ACTION_SEND"));

        intentObject.Call("setType", fileExists ? "image/*" : "text/plain");

        intentObject.Call("putExtra", intentClass.GetStatic<AndroidJavaObject>("EXTRA_SUBJECT"), subject);
        intentObject.Call("putExtra", intentClass.GetStatic<AndroidJavaObject>("EXTRA_TITLE"), title);
        intentObject.Call("putExtra", intentClass.GetStatic<AndroidJavaObject>("EXTRA_TEXT"), text);

        if (fileExists)
        {
            AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
            AndroidJavaObject fileObject = new AndroidJavaObject("java.io.File", path);
            AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("fromFile", fileObject);

            intentObject.Call("putExtra", intentClass.GetStatic<AndroidJavaObject>("EXTRA_STREAM"), uriObject);
        }

        AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
        currentActivity.Call("startActivity", intentObject);
    }

    private bool AndroidFileExists(string path)
    {
        if (string.IsNullOrEmpty(path))
            return false;

        return new AndroidJavaObject("java.io.File", path).Call<bool>("exists");
    }

#endif

#if UNITY_IOS

    [DllImport("__Internal")]
    private static extern void _NativeActivity(string iosPath, string message, string body, float x, float y);

    private void ShareIosScreenShot(string title, string body, float x, float y)
    {
        StartCoroutine(Screenshot((tex, path) =>
        {
            _NativeActivity(path, title, body, x, y);
        }));
    }

    private void ShareIosRect(Rect rect, string title, string body, float x, float y)
    {
        StartCoroutine(CaptureRect(rect, (tex, path) =>
        {
            _NativeActivity(path, title, body, x, y);
        }));
    }

    private void ShareIosRectTransform(RectTransform rt, string title, string body, float x, float y)
    {
        StartCoroutine(CaptureRect(RectTransformToRect(rt), (tex, path) =>
        {
            _NativeActivity(path, title, body, x, y);
        }));
    }

    private void ShareIosText(string title, string body, float x, float y)
    {
        _NativeActivity("", title, body, x, y);
    }

#endif

    public void ShareText(string title, string body, float x, float y)
    {
#if UNITY_IOS
        ShareIosText(title, body, x, y);
#elif UNITY_ANDROID
        ShareAndroid(title, title, body);
#endif
    }

    public void ShareScreenshot(string title, string body, float x, float y)
    {
#if UNITY_IOS
        ShareIosScreenShot(title, body, x, y);
#elif UNITY_ANDROID
        //ShareAndroidScreenShot(title, title, body);
        AndroidShare(body);
#endif
    }

    public void ShareRect(Rect rect, string title, string body, float x, float y)
    {
#if UNITY_IOS
        ShareIosRect(rect, title, body, x, y);
#elif UNITY_ANDROID
        ShareAndroidRect(rect, title, title, body);
#endif
    }

    public void ShareRectTransform(RectTransform rt, string title, string body, float x, float y)
    {
#if UNITY_IOS
        ShareIosRectTransform(rt, title, body, x, y);
#elif UNITY_ANDROID
        ShareAndroidRectTransform(rt, title, title, body);
#endif
    }

    private IEnumerator Screenshot(System.Action<Texture2D, string> callback = null)
    {
        return CaptureRect(new Rect(0, 0, Screen.width, Screen.height), callback);
    }

    private IEnumerator CaptureRect(Rect rect, System.Action<Texture2D, string> callback = null)
    {
        yield return new WaitForEndOfFrame();

        var tex = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);

        tex.ReadPixels(rect, 0, 0);
        tex.Apply();

        string path = SaveTexture(tex);
        Debug.Log(path);

        yield return new WaitForEndOfFrame();

        if (callback != null)
        {
            callback(tex, path);
        }
    }

    private string SaveTexture(Texture2D texture)
    {
        string path = FilePath();
        byte[] bytes = texture.EncodeToPNG();
        File.WriteAllBytes(path, bytes);
        return path;
    }

    private string FilePath(string fileName = "image_share.png")
    {
        return Path.Combine(Application.persistentDataPath, fileName);
    }

    private Rect RectTransformToRect(RectTransform rt)
    {
        Vector3[] bounds = new Vector3[4];

        rt.GetWorldCorners(bounds);

        Vector2 rectPos = RectTransformUtility.WorldToScreenPoint(Camera.main, rt.position);

        Vector2 minPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, bounds[0]);
        Vector2 maxPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, bounds[2]);

        Vector2 size = maxPosition - minPosition;

        float startX = Mathf.Clamp(rectPos.x - size.x / 2, 0, Screen.width - size.x);
        float startY = Mathf.Clamp(rectPos.y - size.y / 2, 0, Screen.height - size.y);

        return new Rect(startX, startY, size.x, size.y);
    }
}
