using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameBackData", menuName = "Game/Lobby/GameBackData")]
public class CSGameBackData : ScriptableObject {

	public string sceneName;
	public Sprite sprite;
	public int experience;

}
