using System.Reflection;
using Modding;

namespace GodSeekerPlus {
	public class GodSeekerPlus : Mod, IGlobalSettings<GlobalSettings>, ILocalSettings<LocalSettings> {
		public static GodSeekerPlus LoadedInstance { get; private set; }
		public override string GetVersion() => "0.1.0";

		public GlobalSettings GlobalSettings { get; set; } = new GlobalSettings();
		public LocalSettings LocalSettings { get; set; } = new LocalSettings();


		public override void Initialize() {
			if (LoadedInstance != null) {
				return;
			}

			LoadedInstance = this;

			Hook();
		}

		public void Unload() {
			LoadedInstance = null;

			UnHook();
		}


		private void Hook() {}

		private void UnHook() {}


		public void OnLoadGlobal(GlobalSettings s) => GlobalSettings = s;

		public GlobalSettings OnSaveGlobal() => GlobalSettings;

		public void OnLoadLocal(LocalSettings s) => LocalSettings = s;

		public LocalSettings OnSaveLocal() => LocalSettings;
	}
}
