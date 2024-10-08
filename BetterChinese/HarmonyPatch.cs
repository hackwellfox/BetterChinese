using Cairo;
using HarmonyLib;
using Vintagestory.API.Client;

namespace BetterChinese;

public class HarmonyPatch(string harmonyId) {
	public string HarmonyId { get; set; } = harmonyId;
	public Harmony HarmonyInstance => new(HarmonyId);

	public void Patch() {
		UnPatch();
		HarmonyInstance.Patch(original: TranslationServiceLoadPatch.OriginalMethod,
			postfix: TranslationServiceLoadPatch.PostfixMethod);
		HarmonyInstance.Patch(original: ForcedTranslation.ContextTextExtentsMethod,
			prefix: ForcedTranslation.ContextTextExtentsPreFixMethod);
		HarmonyInstance.Patch(original: ForcedTranslation.TextDrawUtilDrawTextLineMethod,
			prefix: ForcedTranslation.TextDrawUtilDrawTextLinePreFixMethod);
	}

	public void UnPatch() {
		HarmonyInstance.Unpatch(original: TranslationServiceLoadPatch.OriginalMethod,
			patch: TranslationServiceLoadPatch.PostfixMethod);
		HarmonyInstance.Unpatch(original: ForcedTranslation.ContextTextExtentsMethod,
			patch: ForcedTranslation.ContextTextExtentsPreFixMethod);
		HarmonyInstance.Unpatch(original: ForcedTranslation.TextDrawUtilDrawTextLineMethod,
			patch: ForcedTranslation.TextDrawUtilDrawTextLinePreFixMethod);
	}
}