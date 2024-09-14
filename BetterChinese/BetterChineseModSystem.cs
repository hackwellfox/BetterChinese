using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.API.Server;
using Vintagestory.API.Config;
using Vintagestory.API.Common;
using Vintagestory.Client;
using Vintagestory.Common;

namespace BetterChinese;

public class BetterChineseModSystem : ModSystem {
	public static ScreenManager? ScreenManager;
	public static HarmonyPatch? HarmonyPatch;

	public override void StartPre(ICoreAPI api) {
		if (HarmonyPatch is not null) return;
		HarmonyPatch = new(Mod.Info.ModID);
		HarmonyPatch.Patch();
	}

	public override void Dispose() {
		base.Dispose();
		HarmonyPatch?.UnPatch();
	}

	public static void EarlyLoad(ModContainer mod, ScreenManager screenManager) {
		ScreenManager = screenManager;
		if (HarmonyPatch is not null) return;
		HarmonyPatch = new(mod.Info.ModID);
		HarmonyPatch.Patch();
	}

	public static void EarlyUnload() { HarmonyPatch?.UnPatch(); }
}