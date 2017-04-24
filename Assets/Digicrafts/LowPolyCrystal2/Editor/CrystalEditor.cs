#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System;

namespace Digicrafts.Crystal {

	[CustomEditor(typeof(CrystalUnlit))]
	[CanEditMultipleObjects]
	public class CrystalEditor : Editor {

		CrystalUnlit _target;

		public void OnEnable() {			
			_target = (CrystalUnlit)target;
		}

		public override void OnInspectorGUI()
		{				
			serializedObject.Update();	
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Basic",EditorStyles.boldLabel);
			EditorGUILayout.Separator();
			EditorGUILayout.PropertyField(serializedObject.FindProperty("color"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("glowColorLink"));
			if(_target.glowColorLink) {
				GUI.enabled=false;
				_target.glowColor=_target.color;
			}
			EditorGUILayout.PropertyField(serializedObject.FindProperty("glowColor"));
			GUI.enabled=true;
			EditorGUILayout.Slider(serializedObject.FindProperty("opacity"),0,1);
			EditorGUILayout.Slider(serializedObject.FindProperty("reflection"),0,1);
			EditorGUILayout.Slider(serializedObject.FindProperty("refraction"),0,1);
			EditorGUILayout.Slider(serializedObject.FindProperty("fresnel"),0,1);
			EditorGUILayout.Separator();
			EditorGUILayout.LabelField("Animation",EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("glowMinMax"),GUILayout.Height(0));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("glowAnimationTime"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("glowAnimationDelay"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("glowAnimationOffset"));
			EditorGUILayout.HelpBox("0 will disable the animation.",MessageType.Info);

			serializedObject.ApplyModifiedProperties();
		}

	}
}
#endif