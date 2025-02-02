namespace GodSeekerPlus.Settings;

[PublicAPI]
public sealed class LocalSettings {
	public bool boundNail = false;
	public bool boundHeart = false;
	public bool boundCharms = false;
	public bool boundSoul = false;

	private int rabCompletion = 0;

	[JsonProperty(PropertyName = nameof(rabCompletion))]
	public int RABCompletion {
		get => rabCompletion;
		set => rabCompletion = ((value & 0b11111) == value) ? value : 0;
	}

	private int selectedP5Segment = 0;

	[JsonProperty(PropertyName = nameof(selectedP5Segment))]
	public int SelectedP5Segment {
		get => selectedP5Segment;
		set => selectedP5Segment = value.Clamp(0, Modules.BossChallenge.SegmentedP5.segments.Length - 1);
	}

	#region RAB Completions Getter/Setter

	public bool GetRABCompletion(int num) => num is >= 1 and <= 5
		? (RABCompletion & (1 << (num - 1))) != 0
		: throw new ArgumentOutOfRangeException(nameof(num));

	internal void SetRABCompletion(int num, bool completed) {
		if (num is < 1 or > 5) {
			throw new ArgumentOutOfRangeException(nameof(num));
		}

		int mask = 1 << (num - 1);
		if (completed) {
			RABCompletion |= mask;
		} else {
			RABCompletion &= ~mask;
		}
	}

	#endregion

	public void IncreamentP5SegmentSelection() {
		selectedP5Segment++;

		if (selectedP5Segment >= Modules.BossChallenge.SegmentedP5.segments.Length) {
			selectedP5Segment = 0;
		}
	}
}
