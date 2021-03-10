using UnityEngine;
using TMPro;

public class CSLobbyDayReward : MonoBehaviour {

    public TextMeshProUGUI rewardLabel;
    public float reward;

    private void Awake()
    {
        rewardLabel.text = "<sprite=0> " + CSUtilities.FormatNumber(reward);
    }
}
