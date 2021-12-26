namespace GodSeekerPlus;

public sealed partial class GodSeekerPlus : Mod {
	public static GodSeekerPlus Instance { get; private set; }

	public override string GetVersion() => MiscUtil.GetVersion();

	private readonly ModuleManager moduleManager = new();

	public override void Initialize() {
		if (Instance != null) {
			Logger.LogWarn("Attempting to initialize multiple times, operation rejected");
			return;
		}

		Instance = this;

		moduleManager.LoadModules();
	}

	public void Unload() {
		moduleManager.UnloadModules();

		Instance = null;
	}

	internal bool ModuleEnabled<T>() where T : Module
		=> moduleManager.ModuleEnabled<T>();
}
