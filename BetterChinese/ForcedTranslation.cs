using System.Reflection;
using Cairo;
using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.API.Config;

namespace BetterChinese;

public static class ForcedTranslation {
	public static readonly MethodInfo TextDrawUtilDrawTextLineMethod =
		AccessTools.Method(typeof(TextDrawUtil), nameof(TextDrawUtil.DrawTextLine));

	public static readonly MethodInfo ContextTextExtentsMethod =
		AccessTools.Method(typeof(Context), nameof(Context.TextExtents), [typeof(string)]);

	public static readonly MethodInfo TextDrawUtilDrawTextLinePreFixMethod =
		AccessTools.Method(typeof(ForcedTranslation), nameof(TextDrawUtilDrawTextLinePreFix));

	public static readonly MethodInfo ContextTextExtentsPreFixMethod =
		AccessTools.Method(typeof(ForcedTranslation), nameof(ContextTextExtentsMethodPreFix));

	public static bool Highlight { get; set; }

	public static void Translation(ref string text) {
		text = Lang.Get(text);
		if (Highlight) {
			text = $"『{text}』";
		}
	}

	public static void TextDrawUtilDrawTextLinePreFix(ref string text) {
		if (string.IsNullOrWhiteSpace(text)) return;
		Translation(ref text);
	}

	public static void ContextTextExtentsMethodPreFix(ref string s) {
		if (string.IsNullOrWhiteSpace(s)) return;
		Translation(ref s);
	}
}