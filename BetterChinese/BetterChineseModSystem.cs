using Vintagestory.API.Common;

namespace BetterChinese;

public class BetterChineseModSystem : ModSystem {
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
}