using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSZLLoading : CSLFLoading {

    protected override void Load()
    {
        switch (state)
        {
            case CSLoadingState.In: Load("Zombieland"); break;
            case CSLoadingState.Out: Load("GameLobby"); break;
            default: break;
        }
    }
}
