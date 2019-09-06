using System.Linq;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using VRUI;
using CustomUI.MenuButton;
using Logger = CustomAvatar.Util.Logger;
using IPA.Utilities;

namespace CustomAvatar
{
	class AvatarUI
	{
		private FlowCoordinator _flowCoordinator = null;

		public AvatarUI()
		{
			SceneManager.sceneLoaded += OnSceneLoaded;
		}

		~AvatarUI()
		{
			SceneManager.sceneLoaded -= OnSceneLoaded;
		}

		private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			if (scene.name == "MenuCore")
			{
				if (Plugin.Instance.AvatarLoader.Avatars.Count == 0)
				{
					Logger.Log("[CustomAvatarsPlugin] No avatars found. Button not created.");
				}
				else
				{
					AddMainButton();
					Logger.Log("[CustomAvatarsPlugin] Creating Avatars Button.");
				}
			}
		}

		private void AddMainButton()
		{
			MenuButtonUI.AddButton("Avatars", delegate ()
			{
				var mainFlowCoordinator = Resources.FindObjectsOfTypeAll<MainFlowCoordinator>().First();
				if (_flowCoordinator == null)
				{
					var flowCoordinator = new GameObject("AvatarListFlowCoordinator").AddComponent<AvatarListFlowCoordinator>();
					flowCoordinator.OnContentCreated = (content) =>
					{
						content.onBackPressed = () =>
						{
							mainFlowCoordinator.InvokePrivateMethod("DismissFlowCoordinator", new object[] { flowCoordinator, null, false });
						};
						return "Avatar Select";
					};
					_flowCoordinator = flowCoordinator;
				}
				mainFlowCoordinator.InvokePrivateMethod("PresentFlowCoordinator", new object[] { _flowCoordinator, null, false, false });
			});
		}
	}
}
