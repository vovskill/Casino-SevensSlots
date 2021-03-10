using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CSLobbyDailyReward : MonoBehaviour {

    public Button collect;
    public CSLobbyDayReward[] days;
    public CSBankCoinPanel coinPanel;
    private int _day = 0;

    private void Start()
    {
        CSGameSettings settings = CSGameSettings.instance;
        if (settings.AvalibleDailyReward())
        {
            _day = settings.RewardDay(days.Length);
            Enable(true);
        }
    }

    public void OnCollect()
    {
        CSLobbyDayReward day = days[_day];
        coinPanel.Add(day.reward ,day.transform as RectTransform);
        CSGameSettings.instance.DailyRewardCollected();
        Enable(false);
    }

    private void Enable(bool value)
    {
        days[_day].GetComponent<Toggle>().isOn = value;
        collect.interactable = value;
    }
}
