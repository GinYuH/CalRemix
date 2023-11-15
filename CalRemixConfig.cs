using Microsoft.Xna.Framework;
using System;
using System.ComponentModel;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader.Config;

namespace CalRemix
{
    [BackgroundColor(49, 32, 36, 216)]
    public class CalRemixConfig : ModConfig
    {
        public static CalRemixConfig Instance;
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Header("$Mods.CalRemix.CalRemixConfig.Headers.CalRemixHeader")]
        //[Label("$Mods.CalRemix.CalRemixConfig.FannyToggle.Label")]
        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(true)]
        //[Tooltip("Enables/disables Fanny.")]
        public bool FannyToggle { get; set; }
    }
}