using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Util;
using Vintagestory.Client.NoObf;

// ReSharper disable InconsistentNaming

namespace BetterChinese;

public static class TranslationServiceLoadPatch {
	static private readonly AssetCategory GameLang = new AssetCategory("game/lang", false, EnumAppSide.Universal);

	public static readonly MethodInfo TranslationServiceLoadEntries =
		typeof(TranslationService).GetMethod("LoadEntries", BindingFlags.NonPublic | BindingFlags.Instance)!;

	public static readonly MethodInfo PostfixMethod =
		typeof(TranslationServiceLoadPatch).GetMethod("Postfix")!;

	static private void LoadEntries(
		TranslationService translationService,
		IAsset asset,
		ILogger logger,
		Dictionary<string, KeyValuePair<Regex, string>> regexCache,
		Dictionary<string, string> wildcardCache,
		Dictionary<string, string> entries) {
		try {
			TranslationServiceLoadEntries?.Invoke(
				translationService,
				[
					entries,
					regexCache,
					wildcardCache,
					JsonConvert.DeserializeObject<Dictionary<string, string>>(asset.ToText()),
					"game"
				]);
		} catch (Exception ex) {
			logger.Error("Failed to load language file: " + asset.Name);
			logger.Error(ex);
		}
	}

	public static void Postfix(
		TranslationService __instance,
		ref IAssetManager ___assetManager,
		ref ILogger ___logger,
		ref Dictionary<string, string> ___entryCache,
		ref Dictionary<string, KeyValuePair<Regex, string>> ___regexCache,
		ref Dictionary<string, string> ___wildcardCache) {
		var entryCache = ___entryCache;
		var regexCache = ___regexCache;
		var wildcardCache = ___wildcardCache;
		var logger = ___logger;
		___assetManager.Origins.Foreach(list => {
			foreach (var asset in list.GetAssets(GameLang)
				.Where(a => a.Name.Equals($"{ClientSettings.Language}.json"))) {
				LoadEntries(__instance, asset, logger, regexCache, wildcardCache, entryCache);
			}
		});
		___assetManager.Origins.Foreach(list => {
			foreach (var asset in list.GetAssets(AssetCategory.lang)
				.Where(a => a.Name.Equals($"game-{ClientSettings.Language}.json"))) {
				LoadEntries(__instance, asset, logger, regexCache, wildcardCache, entryCache);
			}
		});
	}
}