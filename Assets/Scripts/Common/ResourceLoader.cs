﻿using UnityEngine;
using System.Collections;
using DYUnityLib;
using GameLogic;

/// <summary>
/// Resource loader.
/// 统一资源加载
/// </summary>
public class ResourceLoader : MonoBehaviour {
	public delegate void ResourceLoaderHandler(UnityEngine.Object res);

	public const string RES_FROM_WWW = "www";
	public const string RES_FROM_LOCAL = "local";
	public const string RES_FROM_RESOURCE = "res";

	private static ResourceLoader instance = null;
	public static ResourceLoader Instance {
		get {
			return instance;
		}
	}

	void Start () {
		instance = this;
	}

	void Update() {
	}

	/*
	public T Load<T>(string path) {
		return default(T);
	}
	*/
	public Coroutine Load(string path, ResourceLoaderHandler handler, string resFrom = RES_FROM_RESOURCE) {
		if (resFrom == RES_FROM_RESOURCE) {
			UnityEngine.Object resObj = Resources.Load<UnityEngine.Object> (path);
			if (handler != null) {
				handler (resObj);
			}

			return null;
		}

		return this.StartCoroutine (this.__Load (path, handler, resFrom));
	}

	private IEnumerator __Load(string path, ResourceLoaderHandler handler, string resFrom) {
		UnityEngine.Object resObj = null;
		if (resFrom == RES_FROM_LOCAL) {
			ResourceRequest loadRequest = Resources.LoadAsync<UnityEngine.Object> (path);
			while (!loadRequest.isDone) {
				yield return 0;
			}

			resObj = loadRequest.asset as UnityEngine.Object;
		} else if (resFrom == RES_FROM_WWW) {
			WWW streamRes = new WWW (AssetBundleFileMangager.FileLoadResPath + path);
			yield return streamRes;

			resObj = streamRes.assetBundle.LoadAsset<UnityEngine.Object> (path);
		}

		if (handler != null) {
			handler (resObj);
		}

		yield return 0;
	}
}