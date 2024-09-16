using CalamityMod.Cooldowns;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace CalRemix.ExtraTextures
{
    public class SoulExplosionCooldown : CooldownHandler
    {
        public static new string ID => "SoulExplosion";
        public override bool ShouldDisplay => true;
        public override LocalizedText DisplayName => Language.GetOrRegister("SoulExplosion Cooldown");
        public override string Texture => "CalRemix/ExtraTextures/SoulExplosionCooldown";
        public override Color OutlineColor => new Color(0, 255, 255);
        public override Color CooldownStartColor => new Color(0, 0, 0);
        public override Color CooldownEndColor => new Color(0, 0, 0);
    }
}