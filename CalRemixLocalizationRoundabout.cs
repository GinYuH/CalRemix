using System;
using System.Reflection;
using Terraria.Localization;
using Terraria.ModLoader;

//Courtesy of scalie
namespace CalRemix
{
    internal class LocalizationRewriter : ModSystem
    {
        public static readonly MethodInfo refreshInfo = typeof(LocalizationLoader).GetMethod("UpdateLocalizationFilesForMod", BindingFlags.NonPublic | BindingFlags.Static, new Type[] { typeof(Mod), typeof(string), typeof(GameCulture) });

        public override void PostSetupContent()
        {
#if DEBUG
            refreshInfo.Invoke(null, new object[] { CalRemix.instance, null, Language.ActiveCulture });
#endif
        }
    }

    internal static class LocalizationRoundabout
    {
        public static readonly PropertyInfo valueProp = typeof(LocalizedText).GetProperty("Value", BindingFlags.Public | BindingFlags.Instance);
        public static void SetDefault(this LocalizedText text, string value)
        {
#if DEBUG
            LanguageManager.Instance.GetOrRegister(text.Key, () => value);
            valueProp.SetValue(text, value);
#endif
        }
    }

}