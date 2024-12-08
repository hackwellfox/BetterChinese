using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Server;
using Vintagestory.Client;
using Vintagestory.Client.NoObf;
using Vintagestory.Common;

namespace BetterChinese;

public class BetterChineseModSystem : ModSystem {
	public static ILogger? Logger { get; private set; }
	public static HarmonyPatch? HarmonyPatch { get; set; }

	public static Config Config { get; set; }

	public override void StartPre(ICoreAPI api) {
		Logger = api.Logger;
		LoadConfig(api, Mod.Info.ModID);
	}

	public override void Dispose() {
		base.Dispose();
		HarmonyPatch?.UnPatch();
	}

	public static void LoadConfig(ICoreAPI api, string modId) {
		try {
			Config = api.LoadModConfig<Config?>("更好的汉化.json") ?? new();
		} catch {
			Config = new();
		}

		api.StoreModConfig(Config, "更好的汉化.json");
		HarmonyPatch ??= new(modId);
		HarmonyPatch.Patch();
	}

	public static void EarlyLoad(ModContainer mod, ScreenManager sm) {
		Logger = mod.Logger;
		LoadConfig(sm.api, mod.Info.ModID);
	}

	public static void EarlyUnload() { HarmonyPatch?.UnPatch(); }
}