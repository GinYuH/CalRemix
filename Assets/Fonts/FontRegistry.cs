using ReLogic.Content;
using ReLogic.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalRemix.Assets.Fonts;

public class FontRegistry : ModSystem
{
    // hevily reffed (stolen) from wrath of the gods. if it aint broke, dont fix it!
    // (also its simple stuff and i dont see why it shouldnt be standard across mods lol. full credit to lucille!)
    
    // "Historically Calamity received errors when attempting to load fonts on Linux systems for their MGRR boss HP bar.
    // Out of an abundance of caution, this mod implements the same solution as them and only uses the font on windows operating systems."
    // -Bluecille Blarma, developer of Wrath of the Blods
    // (Formerly Grominic Grarma, developer of Greenfernum)
    public static bool CanLoadFonts => Environment.OSVersion.Platform == PlatformID.Win32NT;
    public static FontRegistry Instance => ModContent.GetInstance<FontRegistry>();

    public DynamicSpriteFont TimesNewRomanText
    {
        get
        {
            if (Main.netMode != NetmodeID.Server && CanLoadFonts)
                return Mod.Assets.Request<DynamicSpriteFont>("Assets/Fonts/TimesNewRomanText", AssetRequestMode.ImmediateLoad).Value;

            return FontAssets.MouseText.Value;
        }
    }
}
