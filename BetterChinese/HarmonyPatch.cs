using HarmonyLib;

namespace BetterChinese;

public class HarmonyPatch(string harmonyId) {
	public string HarmonyId { get; set; } = harmonyId;
	public Harmony HarmonyInstance => new(HarmonyId);

	public void Patch() {
		HarmonyInstance.Patch(original: TranslationServiceLoadPatch.OriginalMethod,
			postfix: TranslationServiceLoadPatch.PostfixMethod);
	}

	public void UnPatch() {
		HarmonyInstance.Unpatch(original: TranslationServiceLoadPatch.OriginalMethod,
			patch: TranslationServiceLoadPatch.PostfixMethod);
	}
}