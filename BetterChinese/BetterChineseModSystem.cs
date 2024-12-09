using System.IO;
using Newtonsoft.Json;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
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
			Config = LoadModConfig<Config?>("更好的汉化.json") ?? new();
		} catch {
			Config = new();
		}

		StoreModConfig(Config, "更好的汉化.json");
		HarmonyPatch ??= new(modId);
		HarmonyPatch.Patch();
	}

	public static void EarlyLoad(ModContainer mod, ScreenManager sm) {
		Logger = mod.Logger;
		LoadConfig(sm.api, mod.Info.ModID);
	}

	public static void EarlyUnload() { HarmonyPatch?.UnPatch(); }

	public static T? LoadModConfig<T>(string filename) {
		var path = Path.Combine(GamePaths.ModConfig, filename);
		return File.Exists(path) ? JsonConvert.DeserializeObject<T>(File.ReadAllText(path)) : default;
	}

	public static void StoreModConfig<T>(T jsonSerializeableData, string filename) {
		FileInfo fileInfo = new FileInfo(Path.Combine(GamePaths.ModConfig, filename));
		GamePaths.EnsurePathExists(fileInfo.Directory?.FullName);
		File.WriteAllText(fileInfo.FullName, JsonConvert.SerializeObject(jsonSerializeableData, Formatting.Indented));
	}
}