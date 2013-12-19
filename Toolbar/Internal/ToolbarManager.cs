﻿/*
Copyright (c) 2013, Maik Schreiber
All rights reserved.

Redistribution and use in source and binary forms, with or without modification,
are permitted provided that the following conditions are met:

1. Redistributions of source code must retain the above copyright notice, this
   list of conditions and the following disclaimer.

2. Redistributions in binary form must reproduce the above copyright notice,
   this list of conditions and the following disclaimer in the documentation
   and/or other materials provided with the distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE
FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Toolbar {
	[KSPAddonFixed(KSPAddon.Startup.EveryScene, true, typeof(ToolbarManager))]
	public partial class ToolbarManager : MonoBehaviour, IToolbarManager {
		private static readonly string settingsFile = KSPUtil.ApplicationRootPath + "GameData/toolbar-settings.dat";

		private RenderingManager renderingManager;
		private Toolbar toolbar = new Toolbar();
		private ConfigNode settings;

		internal ToolbarManager() {
			Instance = this;

			toolbar.onChange += toolbarChanged;

			GameEvents.onGameSceneLoadRequested.Add(gameSceneLoadRequested);

			GameObject.DontDestroyOnLoad(this);
		}

		internal void OnDestroy() {
			GameEvents.onGameSceneLoadRequested.Remove(gameSceneLoadRequested);
		}

		internal void OnGUI() {
			if (!showGUI()) {
				return;
			}

			toolbar.draw();
		}

		internal void Update() {
			toolbar.update();
		}

		private void toolbarChanged() {
			saveSettings();
		}

		private void gameSceneLoadRequested(GameScenes scene) {
			if (isRelevantGameScene(scene)) {
				loadSettings(scene);
			}
		}

		private void loadSettings(GameScenes scene) {
			Debug.Log("loading toolbar settings (" + scene + ")");

			ConfigNode root = loadSettings();
			if (root.HasNode("toolbars")) {
				toolbar.loadSettings(root.GetNode("toolbars"), scene);
			}
		}

		private ConfigNode loadSettings() {
			if (settings == null) {
				settings = ConfigNode.Load(settingsFile) ?? new ConfigNode();
			}
			return settings;
		}

		private void saveSettings() {
			GameScenes scene = HighLogic.LoadedScene;
			Debug.Log("saving toolbar settings (" + scene + ")");

			ConfigNode root = loadSettings();
			toolbar.saveSettings(root.getOrCreateNode("toolbars"), scene);
			root.Save(settingsFile);
		}

		private bool showGUI() {
			if (!isRelevantGameScene(HighLogic.LoadedScene)) {
				return false;
			}

			if (renderingManager == null) {
				renderingManager = (RenderingManager) GameObject.FindObjectOfType(typeof(RenderingManager));
			}

			if (renderingManager != null) {
				GameObject o = renderingManager.uiElementsToDisable.FirstOrDefault();
				return (o == null) || o.activeSelf;
			}

			return false;
		}

		private bool isRelevantGameScene(GameScenes scene) {
			return (scene != GameScenes.LOADING) && (scene != GameScenes.LOADINGBUFFER) &&
				(scene != GameScenes.PSYSTEM) && (scene != GameScenes.CREDITS);
		}

		public IButton add(string ns, string id) {
			Button button = new Button(ns, id);
			toolbar.add(button);

			return button;
		}
	}
}
