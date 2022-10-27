using CalamityMod.Cooldowns;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace CalRemix
{
    public class MagnaCoreCooldown : CooldownHandler
    {
        public static new string ID => "MagnaCore";
        public override bool ShouldDisplay => true;
        public override string DisplayName => ("Magna Orb Cooldown");
        public override string Texture => "CalRemix/ExtraTextures/MagnaCooldown";
        public override Color OutlineColor => new Color(208, 237, 133);
        public override Color CooldownStartColor => new Color(70, 86, 110);
        public override Color CooldownEndColor => new Color(109, 143, 194);
    }
}