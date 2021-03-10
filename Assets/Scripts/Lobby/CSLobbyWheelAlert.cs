using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CSLobbyWheelAlert : CSAlertRewardAnim {
    
    public CSBonusWheel mainWheel;

    public void Appear(float win, System.Action callback = null)
    {
        _reward = win;
        Appear(callback);
    }

    public override void OnCollect()
    {
        AddCoins();
        Disappear(mainWheel.Disappear);
    }
}
