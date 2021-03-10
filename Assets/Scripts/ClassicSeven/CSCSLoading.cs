using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSCSLoading : CSLFLoading {

    protected override void Load()
    {
        switch (state)
        {
            case CSLoadingState.In: Load("SevenSlots"); break;
            case CSLoadingState.Out: Load("GameLobby"); break;
            default: break;
        }
    }

}
