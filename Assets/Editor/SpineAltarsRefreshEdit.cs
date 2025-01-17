﻿using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Text.RegularExpressions;
using System.IO;

/// <summary>
/// Formula edit.
/// </summary>
public class SpineAltarsRefreshEdit : EditorWindow {
	private const string PATH = "路径";
	private const string SCAL = "大小";
	private const string TYEP_KW = "SkeletonData";

	private string path = "note/";
	private string scale = "0.5";
	private float fscale = 0.5f;


	[MenuItem("RHY/spine的.asset统一刷新")]
	static void Init () {
		SpineAltarsRefreshEdit window = (SpineAltarsRefreshEdit)EditorWindow.GetWindow (typeof(SpineAltarsRefreshEdit));
		window.Show ();
	}

	void OnGUI() {
		EditorGUILayout.BeginVertical ();

		this.path = EditorGUILayout.TextField (PATH, this.path);
		this.scale = EditorGUILayout.TextField (SCAL, this.scale);
		if (GUILayout.Button ("Reflesh")) {
			this.RefleshAsset ();
		}

		EditorGUILayout.EndVertical ();
	}

	private void __RefleshAsset(string path) {
		SkeletonDataAsset sda = Resources.Load<SkeletonDataAsset> (path);
		sda.scale = this.fscale;
		sda.defaultMix = 0f;

		EditorUtility.SetDirty(sda);

		//EditorApplication.SaveAssets ();
	}

	private void RefleshAsset() {
		EditorSettings.serializationMode = SerializationMode.ForceText;
		//string path = AssetDatabase.GetAssetPath (Selection.activeObject);
		string path = Application.dataPath + "/Resources/" + this.path;
		if (string.IsNullOrEmpty (path)) {
			return;
		}

		this.fscale = float.Parse (this.scale);
		Debug.Log ("Reflesh all Spine SkeletonData.atlas with scale " + this.fscale + " in " + path);

		int startIndex = 0;
		string guid = AssetDatabase.AssetPathToGUID (path);
		List<string> withoutExtensions = new List<string> (){ ".asset" };
		string[] files = Directory.GetFiles (path, "*.*", SearchOption.AllDirectories).Where (s => withoutExtensions.Contains (Path.GetExtension (s).ToLower ())).ToArray ();

		Dictionary<string, string> _nameToPath = new Dictionary<string, string> ();
		EditorApplication.update = delegate() {
			string file = files [startIndex];

			bool isCancel = EditorUtility.DisplayCancelableProgressBar ("匹配资源中", file, (float)startIndex / (float)files.Length);
			if (Regex.IsMatch (File.ReadAllText (file), guid)) {
				string[] _pn = file.Split ('/');
				string _fname = _pn [_pn.Length - 1];
				string _fShortName = _fname.Split ('.') [0];
				if (_fShortName.Contains(TYEP_KW)) {
					string _shortPath = file.Replace (path, "");
					_shortPath = _shortPath.Replace ("/Resources/", "");
					_shortPath = _shortPath.Replace (_fname, "");
					/*
					SkeletonDataAsset sda;
					sda.GetAnimationStateData().DefaultMix = 0f;
					sda.scale = _scale;
					*/
					string _resPath = this.path + _shortPath + _fShortName;
					this.__RefleshAsset(_resPath);
					Debug.Log (file, AssetDatabase.LoadAssetAtPath<Object> (GetRelativeAssetsPath (file)));
				}
			}

			startIndex++;
			if (isCancel || startIndex >= files.Length) {
				EditorUtility.ClearProgressBar ();
				EditorApplication.update = null;
				startIndex = 0;
				Debug.Log ("Spine asset搜索完毕");
			}
		};
	}

	static private string GetRelativeAssetsPath(string path) {
		return "Assets" + Path.GetFullPath (path).Replace (Path.GetFullPath (Application.dataPath), "").Replace ('\\', '/');
	}
}