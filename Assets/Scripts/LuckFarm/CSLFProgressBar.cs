using UnityEngine;
using UnityEngine.UI;

public class CSLFProgressBar : MonoBehaviour {

    private float _value = 0;
    public float value {
        get { return _value; }
        set { SetValue(value); }
    }
    public RectTransform bar;
    public Text text;

    private void SetValue(float v)
    {
        _value = Mathf.Clamp(v, 0f, 1f);
        bar.localPosition = new Vector3(-bar.sizeDelta.x * (1f - _value), 0f);
        text.text = string.Format("{0:0.0}%", _value * 100f);
    }
}
