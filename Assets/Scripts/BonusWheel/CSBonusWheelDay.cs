using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CSBonusWheelDay : MonoBehaviour {

    public Toggle[] days;

    public int DayCount
    {
        get { return days.Length; }
    }

	public void SetDay(int day)
	{
        days[Mathf.Clamp(day, 0, DayCount - 1)].isOn = true;
	}

}
