using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum CSLoadingState {
    In,
    Out
}

public class CSLFLoading : MonoBehaviour {

    public CSLFProgressBar progressBar;

    public static CSLoadingState state;

    private void Start()
    {
        LeanTween.delayedCall(1f, Load);
    }

    private void OnDestroy()
    {
        LeanTween.cancel(gameObject);
    }

    protected virtual void Load()
    {
        switch (state)
        {
            case CSLoadingState.In: Load("LuckyFarm"); break;
            case CSLoadingState.Out: Load("GameLobby"); break;
            default: break;
        }
    }

    protected void Load(string sceneName)
    {
        LeanTween.cancel(gameObject);
        LeanTween.value(gameObject, 0f, 1f, 0.9f).setOnUpdate((v) => {
            progressBar.value = v;
        });

        CSSceneManager.instance.LoadScene(sceneName, (percent, completed) => {
            //progressBar.value = percent;
        });
    }

}
