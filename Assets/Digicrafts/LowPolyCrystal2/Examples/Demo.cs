using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Digicrafts.Animation;

public class Demo : MonoBehaviour {

	public GameObject crystal;
	public ColorPicker baseColorPicker;
	public ColorPicker glowColorPicker;
	public Button baseColorButton;
	public Button glowColorButton;

	private float _glowStrength = 1.0f;
	private Color _glowColor;
	private Color _baseColor;
	bool _glowAnimationEnabled=true;
	private float _glowAnimationTime=8;
	private float _glowAnimationStep = 0;
	private float _glowDirection = 1;

	// Use this for initialization
	void Start () {	

		baseColorPicker.gameObject.SetActive(false);
		glowColorPicker.gameObject.SetActive(false);

		_glowColor = new Color(0.7f,1,1);
		_baseColor = new Color(0.4f,0.78f,1);

		setBaseColor(_baseColor);
		setGlowColor(_glowColor);

		baseColorPicker.CurrentColor=_baseColor;
		glowColorPicker.CurrentColor=_glowColor;
	}
	
	// Update is called once per frame
	void Update () {

		// loop each crystal
		foreach(Transform child in crystal.transform)
		{
			child.gameObject.transform.Rotate(new Vector3(0,0,1));

			if(_glowAnimationEnabled){
				float glow;
				float step = _glowAnimationStep/_glowAnimationTime;
				if(_glowDirection==1){
					glow =  _glowStrength * Easing.EaseOut(step,EasingType.Sine);
				} else {
					glow = _glowStrength * (1-step);
				}
				child.gameObject.GetComponent<MeshRenderer>().material.SetFloat("_Emission", glow);

				//
				_glowAnimationStep+=Time.deltaTime;
				// Set glow step
				if(_glowAnimationStep>_glowAnimationTime){
					_glowAnimationStep=0;
					_glowDirection=-_glowDirection;
				}
			} else {
				child.gameObject.GetComponent<MeshRenderer>().material.SetFloat("_Emission", _glowStrength);
			}
		}
	}
		
	public void setGlow(float value) {
		foreach(Transform child in crystal.transform)
		{
			_glowStrength=value;
			child.gameObject.GetComponent<MeshRenderer>().material.SetFloat("_Emission", _glowStrength);
		}
	}

	public void toggleBaseColor() {				
		glowColorPicker.gameObject.SetActive(false);
		baseColorPicker.gameObject.SetActive(!baseColorPicker.gameObject.activeSelf);


	}

	public void toggleGlowColor() {
		baseColorPicker.gameObject.SetActive(false);
		glowColorPicker.gameObject.SetActive(!glowColorPicker.gameObject.activeSelf);
	}

	public void setGlowEnabled(bool value) {
		_glowAnimationEnabled=value;
	}

	public void setGlowColor(Color c) {
		
		_glowColor.r=c.r;
		_glowColor.g=c.g;
		_glowColor.b=c.b;
		foreach(Transform child in crystal.transform)
		{			
			child.gameObject.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", _glowColor);
		}

		ColorBlock cb = glowColorButton.colors;
		cb.normalColor=cb.highlightedColor=cb.pressedColor=_glowColor;
		glowColorButton.colors=cb;
	}

	public void setBaseColor(Color c) {
		
		_baseColor.r=c.r;
		_baseColor.g=c.g;
		_baseColor.b=c.b;
		foreach(Transform child in crystal.transform)
		{
			child.gameObject.GetComponent<MeshRenderer>().material.color=_baseColor;//.SetColor("_Color", _baseColor);
		}
		ColorBlock cb = baseColorButton.colors;
		cb.normalColor=cb.highlightedColor=cb.pressedColor=_baseColor;
		baseColorButton.colors=cb;
	}

	public void setOpacity(float value) {
		foreach(Transform child in crystal.transform)
		{
			child.gameObject.GetComponent<MeshRenderer>().material.SetFloat("_Opacity", value);
		}
	}

	public void setReflectionStrength(float value) {
		foreach(Transform child in crystal.transform)
		{
			child.gameObject.GetComponent<MeshRenderer>().material.SetFloat("_ReflectionStrength", value);
		}
	}

	public void setRefractionStrength(float value) {
		foreach(Transform child in crystal.transform)
		{
			child.gameObject.GetComponent<MeshRenderer>().material.SetFloat("_RefractionStrength", value);
		}
	}

}
