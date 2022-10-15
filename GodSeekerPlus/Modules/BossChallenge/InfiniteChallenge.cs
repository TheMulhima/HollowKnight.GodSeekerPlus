namespace GodSeekerPlus.Modules.BossChallenge;

[ToggleableLevel(ToggleableLevel.ChangeScene)]
internal sealed class InfiniteChallenge : Module {
	private BossSceneController.SetupEventDelegate? setupEvent;

	private protected override void Load() {
		On.BossSceneController.Awake += RecordSetupEvent;
		// ModHooks.BeforeSceneLoadHook doesn't support editing entry gate
		On.GameManager.BeginSceneTransition += RestartFight;
		OsmiHooks.SceneChangeHook += Cleanup;
	}

	private protected override void Unload() {
		setupEvent = null;

		On.BossSceneController.Awake -= RecordSetupEvent;
		On.GameManager.BeginSceneTransition -= RestartFight;
		OsmiHooks.SceneChangeHook -= Cleanup;
	}

	private void RecordSetupEvent(On.BossSceneController.orig_Awake orig, BossSceneController self) {
		if (!BossSequenceController.IsInSequence) {
			setupEvent = BossSceneController.SetupEvent;
		}

		orig(self);
	}

	private void RestartFight(On.GameManager.orig_BeginSceneTransition orig, GameManager self, GameManager.SceneLoadInfo info) {
		string currentSceneName = self.sceneName;
		if (
			info.SceneName == "GG_Workshop"
			&& setupEvent != null
			&& (
				Ref.HC.heroDeathPrefab.activeSelf // Death returning
				|| ( // Success returning
					Setting.Global.restartFightOnSuccess
					&& StaticVariableList.GetValue<bool>("finishedBossReturning")
				)
			)
		) {
			BossSceneController.SetupEvent = setupEvent;
			setupEvent = null;
			StaticVariableList.SetValue("finishedBossReturning", false);

			Ref.HC.EnableRenderer();
			Ref.HC.EnterWithoutInput(true);
			Ref.HC.AcceptInput();
			Ref.HC.gameObject.LocateMyFSM("Dream Return").GetVariable<FsmBool>("Dream Returning").Value = false;

			info.SceneName = currentSceneName;
			info.EntryGateName = "door_dreamEnter";
		}

		orig(self, info);
	}

	private void Cleanup(Scene prev, Scene next) {
		if (next.name == "GG_Workshop" || BossSequenceController.IsInSequence) {
			setupEvent = null;
		}
	}
}