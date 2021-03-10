using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CSBonusWheelAnimation : MonoBehaviour {

	public ParticleSystem wheelParticle;
	public CanvasGroup canvas;
	public float duration;
	public GameObject content;
    public GameObject ring;
	private Image _background;


	private int _scaleId = 0;
	private int _alphaId = 0;
	private int _alphaBoardId = 0;
	private GameObject _particle;
    private CSGlowAnimation _glow;

	private bool _active;
	public bool active {
		get {return _active; }
		set {
			if (value == _active)
				return;
			_active = value;
			CanvasStatus (_active);
		}
	}

	void Awake()
	{
        _glow = ring.GetComponent<CSGlowAnimation>();
		_background = GetComponent <Image> ();
	}

	public void Appear()
	{
		active = true;
		AlphaBackground (0.75f);
		AlphaBoard (1f);
		Scale (LeanTweenType.easeOutBack);
	}

	public void Disappear()
	{
		Scale (LeanTweenType.easeInBack);
		AlphaBoard (0f);
		AlphaBackground (0f).setOnComplete (() => {
			active = false;
		});
	}

	private LTDescr Scale(LeanTweenType type)
	{
		LeanTween.cancel (_scaleId);

		float scale = 0f;
		switch (type) {
		case LeanTweenType.easeInBack:  scale = 0f; break; 
		case LeanTweenType.easeOutBack: scale = 1f; break;
		default:break;
		}

		content.transform.localScale = Vector3.one * (scale > 0.5f ? 0f : 1f);
		LTDescr action = LeanTween.scale (content, Vector3.one * scale, duration).setEase (type).setIgnoreTimeScale (true);
		_scaleId = action.id;
		return action;
	}

	private LTDescr AlphaBackground(float value)
	{
		LeanTween.cancel (_alphaId);

		LTDescr action = Alpha (_background, value, 0.4f);
		_alphaId = action.id;
		return action;
	}

	private LTDescr AlphaBoard(float value)
	{
		LeanTween.cancel (_alphaBoardId);

		CanvasGroup bcanvas = content.GetComponent <CanvasGroup> ();
		bcanvas.alpha = (value > 0.5f ? 0f : 1f);

		LTDescr action = LeanTween.alphaCanvas (content.GetComponent <CanvasGroup> (), value, 0.3f).setIgnoreTimeScale (true);
		_alphaBoardId = action.id;
		return action;
	}

	private LTDescr Alpha(Image image, float value, float dur)
	{
		Color color = image.color;
		color.a = (value > 0.5f ? 0f : color.a);
		image.color = color;

		return LeanTween.value (image.gameObject, color.a, value, dur).setOnUpdate (delegate(float obj) {
			color.a = obj;
			image.color = color;
		}).setIgnoreTimeScale (true);
	}

	private void CanvasStatus(bool value)
	{
		canvas.blocksRaycasts = value;
		canvas.interactable = value;
        //_glow.enable = value;
		if (value)
			AddPaticle ();
		else
			DestroyPaticle ();
	}

	private void AddPaticle()
	{
		_particle = Instantiate (wheelParticle).gameObject;
	}

	private void DestroyPaticle()
	{
		Destroy (_particle);
	}
}
