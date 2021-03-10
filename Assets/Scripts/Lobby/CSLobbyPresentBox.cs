using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSLobbyPresentBox : MonoBehaviour {

    void Awake()
    {
        transform.localScale = Vector3.zero;
    }

    private void Start()
    {
        Appear();
    }

    void Appear()
    {
        Vector3 startPosition = transform.position;
        Vector3 offset = new Vector3(-2f, 0.5f);
        transform.position = startPosition + offset;
        float duration = 1f;

        JumpTo(gameObject, startPosition, 1f, 1f, duration).setEaseOutSine();
        LeanTween.scale(gameObject, Vector3.one, duration).setEaseOutSine().setOnComplete(RepeatAction);
    }

    private void Disappear(System.Action callback = null)
    {
        LeanTween.cancel(gameObject);
        LeanTween.alpha(transform as RectTransform, 0f, 0.2f);
        if (callback != null)
        {
            callback();
        }
    }

    void RepeatAction()
    {
        LeanTween.scale(gameObject, Vector3.one * 0.8f, 1f).setLoopPingPong(-1).setEaseInSine();
    }

    private LTDescr JumpTo(GameObject obj, Vector3 to, float height, float jumps, float duration)
    {
        Vector3 start = transform.position;
        Vector3 delta = to - start;         return LeanTween.value(obj, 0f, 1f, duration).setOnUpdate(delegate (float t) {
            float frac = (t * jumps) % 1f;
            float y = (height * 4f * frac * (1f - frac)) + delta.y * t;
            transform.position = start + new Vector3(delta.x * t, y);
        });
    }

    private LTDescr JumpBy(GameObject obj, Vector3 to, float height, float jumps, float duration)
    {
        Vector3 start = transform.position;
        Vector3 delta = to;         return LeanTween.value(obj, 0f, 1f, duration).setOnUpdate(delegate (float t) {
            float frac = (t * jumps) % 1f;
            float y = (height * 4f * frac * (1f - frac)) + delta.y * t;
            transform.position = start + new Vector3(delta.x * t, y);
        });
    }

    public void OnPresent()
    {
        CSTimerManager.instance.CreatePresentTimer(30);
        Disappear();
    }

}
