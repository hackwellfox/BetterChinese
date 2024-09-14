using HarmonyLib;
using Vintagestory.API.Common;

namespace BetterChinese;

public class HarmonyPatch(string harmonyId) {
	public string HarmonyId { get; set; } = harmonyId;
	public Harmony HarmonyInstance => new(HarmonyId);

	public void Patch() {
		HarmonyInstance.Patch(original: TranslationServiceLoadPatch.TranslationServiceLoadEntries,
			postfix: TranslationServiceLoadPatch.PostfixMethod);
	}

	public void UnPatch() {
		HarmonyInstance.Unpatch(original: TranslationServiceLoadPatch.TranslationServiceLoadEntries,
			patch: TranslationServiceLoadPatch.PostfixMethod);
	}
}