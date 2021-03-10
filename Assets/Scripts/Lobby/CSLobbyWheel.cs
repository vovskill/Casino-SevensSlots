using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CSLobbyWheel : MonoBehaviour, ICSTimer {

	public GameObject spinIt;
	public Image timerBoard;
	public Text timerLabel;

	private Button _button;

	private bool _enable = true;
	public bool enable {
		get { return _enable; }
		set {
			if (_enable == value)
				return;
			SetEnable (value);
		}
	}

	void Start()
	{
		_button = GetComponent <Button> ();
        SetEnable(!CSTimerManager.instance.SubsricbeToWheelTimer(this));
	}

	void OnEnable()
	{
        CSTimerManager.instance.TimerCreatedEvent += CSTimerManager_instance_timerCreatedEvent;
	}

    private void OnDisable()
    {
        enable = false;
        CSTimerManager.instance.UnsubsricbeFromWheelTimer (this);
        CSTimerManager.instance.TimerCreatedEvent -= CSTimerManager_instance_timerCreatedEvent;
    }


    private void SetEnable(bool value)
	{
        _enable = value;
        SetActiveSpinIt (value);
		_button.interactable = value;
		timerBoard.enabled = !value;
		timerLabel.enabled = !value;
	}

    private void SetActiveSpinIt(bool value)
    {
        spinIt.GetComponent<Image>().enabled = value;
        if (value)
            LeanTween.scale(spinIt, Vector3.one * 1.25f, 1f).setEaseInOutSine().setLoopPingPong(-1);
        else
            LeanTween.cancel(spinIt);
    }

	private void CreateTimer()
	{
		CSTimerManager.instance.CreateWheelTimer (24);
		enable = !CSTimerManager.instance.SubsricbeToWheelTimer (this);
	}

	public void TimerTick (CSTimer timer, double seconds)
	{
        timerLabel.text = CSUtilities.TimeFormat ((float) seconds);
	}

	public void TimerStop (CSTimer timer, double seconds)
	{
		CSTimerManager.instance.DestroyTimer (timer);
		enable = true;
	}

	void CSTimerManager_instance_timerCreatedEvent (CSTimer arg1, string arg2)
	{
		if (arg2 == "wheel")
		{
			enable = !CSTimerManager.instance.SubsricbeToWheelTimer (this);
		}
	}
}
