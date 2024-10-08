using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Server;
using Vintagestory.Client;
using Vintagestory.Client.NoObf;
using Vintagestory.Common;

namespace BetterChinese;

public class BetterChineseModSystem : ModSystem {
	public static HarmonyPatch? HarmonyPatch { get; set; }

	public override void StartPre(ICoreAPI api) {
		api.ChatCommands.Create("text")
			.WithArgs(api.ChatCommands.Parsers.Word("highlight"))
			.WithDescription("为每段可翻译文本额外添加『』")
			.RequiresPrivilege(Privilege.chat)
			.HandleWith(args => {
				if (args[0] is "on") {
					ForcedTranslation.Highlight = true;
				}
				if (args[0] is "off") {
					ForcedTranslation.Highlight = false;
				}
				return TextCommandResult.Success();
			});
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