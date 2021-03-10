using UnityEngine;
using UnityEngine.Purchasing;

[CreateAssetMenu(fileName = "IAPProduct", menuName = "Game/Game/IAPProduct")]
[System.Serializable]
public class CSIAPProduct : ScriptableObject
{
    public string productId;
    public ProductType type;
    public float coins;
}
