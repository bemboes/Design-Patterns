using UnityEngine;
using System.Collections;
using Digicrafts.Animation;
using Digicrafts.EditorUI;

#if UNITY_EDITOR
using UnityEditor;
#endif



	public class CrystalUnlit : MonoBehaviour {

		public Color color = Color.white;
		[ColorUsageAttribute(true,true,0f,8f,0.125f,3f)]
		public Color glowColor = Color.white;
		public bool glowColorLink = false;
		public float opacity = 0.9f;
		public float reflection = 0.7f;
		public float refraction = 0.7f;
		public float fresnel = 0.5f;
		public float ambient = 1.0f;
		[MinMaxSlider (0f, 3f)]
		public Vector2 glowMinMax = new Vector2(0,0.5f);
		public float glowAnimationTime = 1;
		public float glowAnimationOffset = 0;
		public float glowAnimationDelay = 0;
		public float rotateAnimationTime = 0;

		// Var for animation
		private HSBColor _HSBColor;
		private float _glowColorBrightness;
		private float _rotationStep = 0;
		private float _glowAnimationStep = 0;
		private float _glowDirection = 1;
		private float _glowDelay = 0;

		// Use this for initialization
		void Start () {								

			_glowDelay=glowAnimationOffset;

			// Set GI
			if(glowAnimationTime<=0||glowMinMax.x==glowMinMax.y){
				SetEmission(glowMinMax.y);
			} else {
				SetEmission(glowMinMax.x);
			}
		}

		// Update is called once per frame
		void Update () {

			// rotate animation
			if(rotateAnimationTime>0){			
				_rotationStep =  Time.deltaTime/rotateAnimationTime*360;
				gameObject.transform.RotateAround(gameObject.transform.position,Vector3.up,_rotationStep);
			}

			// glow animation
			if(glowAnimationTime>0){
				
				if(glowMinMax.x!=glowMinMax.y){						

					if(_glowDelay>0){
						_glowDelay-=Time.deltaTime;
					} else {
						float glow = glowMinMax.x;
						float step = _glowAnimationStep/glowAnimationTime;
						if(_glowDirection>0){
							glow = (glowMinMax.y - glowMinMax.x) * Easing.EaseOut(step,EasingType.Quintic) + glowMinMax.x;
						} else {
							glow = (glowMinMax.y - glowMinMax.x) * (1-step) + glowMinMax.x;
						}

						SetEmission(glow);
						//
						_glowAnimationStep+=Time.deltaTime;
						// Set glow step
						if(_glowAnimationStep>glowAnimationTime){
							_glowAnimationStep=0;
							_glowDirection=-_glowDirection;
							if(_glowDirection<0)
								_glowDelay=glowAnimationDelay;
						}
					}
				} else {
					SetEmission(glowMinMax.y);
				} 
			}				
		
		}

		#if UNITY_EDITOR
		public void OnValidate(){			
//			Debug.Log("OnValidate: "+ 1 + " shared: " + gameObject.GetComponent<MeshRenderer>().sharedMaterial);
			if(Application.isEditor&&Application.isPlaying){
                // Id ont know why, but it isnt working.
                // Material mat = obj.GetComponent<MeshRenderer>().sharedMaterial;

                gameObject.GetComponent<MeshRenderer>().sharedMaterial.color = color;
				gameObject.GetComponent<MeshRenderer>().sharedMaterial.SetColor("_EmissionColor", glowColor);
				gameObject.GetComponent<MeshRenderer>().sharedMaterial.SetFloat("_Emission", glowMinMax.y);
				gameObject.GetComponent<MeshRenderer>().sharedMaterial.SetFloat("_Opacity", opacity);
				gameObject.GetComponent<MeshRenderer>().sharedMaterial.SetFloat("_AmbientLight", ambient);
				gameObject.GetComponent<MeshRenderer>().sharedMaterial.SetFloat("_ReflectionStrength", reflection);
				gameObject.GetComponent<MeshRenderer>().sharedMaterial.SetFloat("_RefractionStrength", refraction);

            } else {
				
				if(Selection.activeGameObject&&PrefabUtility.GetPrefabType(Selection.activeGameObject)==PrefabType.PrefabInstance){
					foreach(GameObject obj in Selection.gameObjects){
						Material mat = obj.GetComponent<MeshRenderer>().sharedMaterial;
						mat.color=color;
						mat.SetColor("_EmissionColor", glowColor);
						mat.SetFloat("_Emission", glowMinMax.y);
						mat.SetFloat("_Opacity", opacity);
						mat.SetFloat("_AmbientLight", ambient);
						mat.SetFloat("_ReflectionStrength", reflection);
						mat.SetFloat("_RefractionStrength", refraction);
						mat.SetFloat("_FresnelStrength", fresnel);
						// Set glow brightness
						Color finalColor = glowColor * Mathf.LinearToGammaSpace (glowMinMax.y);
						DynamicGI.SetEmissive(gameObject.GetComponent<MeshRenderer>(),finalColor);
						obj.GetComponent<MeshRenderer>().sharedMaterial = new Material(mat);
					}
				}
			}				
		}
		#endif

		// Setter 	
		public void SetEmission(float value) {
			gameObject.GetComponent<MeshRenderer>().material.SetFloat("_Emission", value);
			Color finalColor = glowColor * Mathf.LinearToGammaSpace (value);
			DynamicGI.SetEmissive(gameObject.GetComponent<MeshRenderer>(),finalColor);
		}			
	}
