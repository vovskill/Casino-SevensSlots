using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CSBonusWheel : MonoBehaviour {

	public int rotate;
	public float duration;
	public RectTransform wheel;
	public CSBonusWheelDay day;
    public CSLobbyWheelAlert alert;

    private float _reward;
	private int _multiplier = 0;
	private CSBonusWheelAnimation _animation;

	void Awake()
	{
		_animation = GetComponent <CSBonusWheelAnimation> ();
	}


	public void Appear()
	{
		_animation.Appear();

        int today = CSGameSettings.instance.RewardDay(day.DayCount);
        _multiplier = today + 1;
        day.SetDay(today);
	}

	public void Disappear()
	{
		CSTimerManager.instance.CreateWheelTimer (24);
		_animation.Disappear ();
	}

	Vector3 VectorWithAngle(float angle, float length)
	{
		angle = Mathf.Deg2Rad * angle;
		return length * new Vector3 (Mathf.Sin (angle), Mathf.Cos (angle));
	}

	public void OnSpin()
	{
        List<float> rewards = Rewards ();
		int idx = Random.Range (0, rewards.Count);
		_reward = rewards [idx];

		float delta = 360f / rewards.Count;
		float angle = delta * idx - wheel.eulerAngles.z;

		Rotate (-1f * (rotate * 360f - angle), duration);
	}

	private void Rotate(float angle, float dur)
	{
        CSSoundManager.instance.Play("bicycle_wheel");
		_animation.canvas.interactable = false;
        LeanTween.rotate(wheel, angle, dur).setOnComplete(Reward).setEaseInOutSine();
	}

	private void Reward()
	{
        CSSoundManager.instance.Stop("bicycle_wheel");
        CSSoundManager.instance.Play("spin_stop");
		LeanTween.delayedCall (0.5f, () => {
            alert.Appear (_reward * (uint)_multiplier);
		});
	}

    private List<float> Rewards()
	{
        return new List<float> () {
			100000f,
			300000f,
			75000f,
			10000f,
			1500f,
			1500f,
			2500f,
			12000f,
			3000f,
			10000f,
			25000f,
			5000f,
			1000f,
			7500f,
			50000f
		};
	}
}
