namespace GodSeekerPlus.Settings;

public sealed class GlobalSettings {
	[JsonIgnore]
	private readonly Dictionary<string, bool> modules =
		ModuleHelper.GetDefaultModuleStateDict();


	[JsonIgnore]
	private float fastSuperDashSpeedMultiplier = 1.5f;

	[JsonIgnore]
	private int lagTime = 50;

	[JsonIgnore]
	private int lifebloodAmount = 5;

	[JsonIgnore]
	private int soulAmount = 99;

	public bool gpzEnterType = false;


	[JsonProperty(PropertyName = "features")]
	public Dictionary<string, bool> Modules {
		get => modules;
		set {
			foreach (KeyValuePair<string, bool> pair in value) {
				if (modules.ContainsKey(pair.Key)) {
					modules[pair.Key] = pair.Value;
				}
			}
		}
	}

	[JsonProperty(PropertyName = nameof(fastSuperDashSpeedMultiplier))]
	public float FastSuperDashSpeedMultiplier {
		get => fastSuperDashSpeedMultiplier;
		set => fastSuperDashSpeedMultiplier = MiscUtil.Clamp(value, 1f, 2f);
	}

	[JsonProperty(PropertyName = nameof(lagTime))]
	public int LagTime {
		get => lagTime;
		set => lagTime = MiscUtil.Clamp(value, 0, 1000);
	}

	[JsonProperty(PropertyName = nameof(lifebloodAmount))]
	public int LifebloodAmount {
		get => lifebloodAmount;
		set => lifebloodAmount = MiscUtil.Clamp(value, 0, 35);
	}

	[JsonProperty(PropertyName = nameof(soulAmount))]
	public int SoulAmount {
		get => soulAmount;
		set => soulAmount = MiscUtil.Clamp(value, 0, 198);
	}
}
