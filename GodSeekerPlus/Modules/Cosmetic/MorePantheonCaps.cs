using Bindings = BossSequenceController.ChallengeBindings;

namespace GodSeekerPlus.Modules.Cosmetic;

[ToggleableLevel(ToggleableLevel.ChangeScene)]
[DefaultEnabled]
internal sealed class MorePantheonCaps : Module {
	private static readonly Dictionary<string, int> doorPDDict = new() {
		{ "bossDoorStateTier1", 1 },
		{ "bossDoorStateTier2", 2 },
		{ "bossDoorStateTier3", 3 },
		{ "bossDoorStateTier4", 4 },
		{ "bossDoorStateTier5", 5 }
	};

	private protected override void Load() {
		On.BossSequenceDoor.Start += SetupCaps;
		On.BossDoorChallengeCompleteUI.Start += RecordRAB;
	}

	private protected override void Unload() {
		On.BossSequenceDoor.Start -= SetupCaps;
		On.BossDoorChallengeCompleteUI.Start -= RecordRAB;
	}

	private void SetupCaps(On.BossSequenceDoor.orig_Start orig, BossSequenceDoor self) {
		if (self.bossSequence != null) {
			if (
				self.completedNoHitsDisplay == null
				&& self.gameObject.Child("Main Caps", "GG_door_cap_complete_nohits") is GameObject go
			) {
				self.completedNoHitsDisplay = go;
				Logger.LogDebug($"Radiant cap enabled for {self.bossSequence.name}");
			}

			if (doorPDDict.TryGetValue(self.playerDataString, out int num)
				&& Setting.Local.GetRABCompletion(num)
				&& self.completedNoHitsDisplay?.GetComponent<SpriteRenderer>() is SpriteRenderer sr
			) {
				sr.transform.Translate(0, 0, -0.0801f);
				sr.color = Color.black;

				Logger.LogDebug($"Radiant AB effect enabled for {self.bossSequence.name}");
			}
		}

		orig(self);
	}

	private void RecordRAB(On.BossDoorChallengeCompleteUI.orig_Start orig, BossDoorChallengeCompleteUI self) {
		BossSequenceController.BossSequenceData currentData = BossSequenceControllerR.currentData;

		if (doorPDDict.TryGetValue(currentData.playerData, out int num)) {
			bool rab = !currentData.knightDamaged
			&& currentData.bindings == (Bindings.Nail | Bindings.Shell | Bindings.Charms | Bindings.Soul);
			bool rabPrev = Setting.Local.GetRABCompletion(num);

			if (rab && !rabPrev) {
				Setting.Local.SetRABCompletion(num, true);
				Logger.LogDebug($"Radiant AB in Pantheon #{num} recorded");
			}
		}

		orig(self);
	}

}
