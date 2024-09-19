using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.Client;
using Vintagestory.Client.NoObf;
using Vintagestory.Common;

namespace BetterChinese;

public class BetterChineseModSystem : ModSystem {
	public static HarmonyPatch? HarmonyPatch;

	public override void StartPre(ICoreAPI api) {
		HarmonyPatch ??= new(Mod.Info.ModID);
		HarmonyPatch.Patch();
	}

	public override void Dispose() {
		base.Dispose();
		HarmonyPatch?.UnPatch();
	}

	public static void EarlyLoad(ModContainer mod, ScreenManager sm) {
		HarmonyPatch ??= new(mod.Info.ModID);
		HarmonyPatch.Patch();
	}

	public static void EarlyUnload() { HarmonyPatch?.UnPatch(); }
}