using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CSGameLobby : MonoBehaviour {

	public CSBonusWheel wheel;
    public RectTransform  themesContent;
    private bool _onload = true;

    private void Start()
    {
        FindLastSelectedTheme();
    }

    public void OnWheel()
	{
		wheel.Appear ();
	}

    public void OnPlay(GameObject sender)
    {
        if (_onload) return;
        if (!sender.GetComponent<Toggle>().isOn)
            return;
        CSGameSettings.instance.selectedTheme = sender.GetComponent<CSGameBack>().data.sceneName;
        CSLFLoading.state = CSLoadingState.In;
        SceneManager.LoadScene(sender.GetComponent<CSGameBack>().data.sceneName);
    }

    public void OnComingSoon()
    {

    }

    private void FindLastSelectedTheme()
    {
        string selected = CSGameSettings.instance.selectedTheme;
        if (selected == string.Empty)
        {
            _onload = false;
            return;
        }
        for (int i = 0; i < themesContent.childCount; i++)
        {
            Transform t = themesContent.GetChild(i);
            CSGameBack gameBack = t.GetComponent<CSGameBack>();
            if (gameBack == null)
                continue;

            if (gameBack.data.sceneName == selected)
            {
                t.GetComponent<Toggle>().isOn = true;
                _onload = false;
                break;
            }
        }
    }

}
