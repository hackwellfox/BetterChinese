using System;
using System.Collections.Generic;
using System.IO;
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
	static private readonly AssetCategory GameLang = new("game/lang", false, EnumAppSide.Universal);

	static private readonly MethodInfo TranslationServiceLoadEntries =
		typeof(TranslationService).GetMethod("LoadEntries", BindingFlags.NonPublic | BindingFlags.Instance)!;

	public static readonly MethodInfo OriginalMethod =
		typeof(TranslationService).GetMethod(nameof(TranslationService.Load))!;

	public static readonly MethodInfo PostfixMethod =
		typeof(TranslationServiceLoadPatch).GetMethod("Postfix")!;

	static private void LoadEntries(
		TranslationService translationService,
		string folderPath,
		string domain,
		ILogger logger,
		Dictionary<string, KeyValuePair<Regex, string>> regexCache,
		Dictionary<string, string> wildcardCache,
		Dictionary<string, string> entries) {
		var currentPath = Path.Combine(folderPath, domain, AssetCategory.lang.Code);
		if (!Directory.Exists(currentPath)) {
			return;
		}

		foreach (var file in Directory.GetFiles(currentPath)) {
			if (!file.EndsWith($"{ClientSettings.Language}.json")) continue;
			try {
				TranslationServiceLoadEntries.Invoke(
					translationService,
					[
						entries,
						regexCache,
						wildcardCache,
						JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(file)),
						domain
					]);
			} catch (Exception ex) {
				logger.Error("Failed to load language file: " + file);
				logger.Error(ex);
			}
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
		___assetManager.Origins.Foreach(assetOrigin => {
			foreach (var directory in Directory.GetDirectories(assetOrigin.OriginPath)) {
				foreach (var domain in Directory.GetDirectories(directory)) {
					LoadEntries(__instance, directory, Path.GetFileName(domain), logger, regexCache, wildcardCache, entryCache);
				}
			}
		});
	}
}