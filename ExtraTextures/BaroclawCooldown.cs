using CalamityMod.Cooldowns;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace CalRemix.ExtraTextures
{
    public class BaroclawCooldown : CooldownHandler
    {
        public static new string ID => "Baroclaw";
        public override bool ShouldDisplay => true;
        public override LocalizedText DisplayName => Language.GetOrRegister("Baro Cooldown");
        public override string Texture => "CalRemix/ExtraTextures/BaroCooldown";
        public override Color OutlineColor => new Color(107, 255, 124);
        public override Color CooldownStartColor => new Color(28, 30, 102);
        public override Color CooldownEndColor => new Color(61, 64, 173);
    }
}