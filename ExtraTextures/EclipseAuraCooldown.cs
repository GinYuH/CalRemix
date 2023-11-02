using CalamityMod.Cooldowns;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace CalRemix
{
    public class EclipseAuraCooldown : CooldownHandler
    {
        public static new string ID => "EclipseAura";
        public override bool ShouldDisplay => true;
        public override LocalizedText DisplayName => Language.GetOrRegister("Eclipse Aura Cooldown");
        public override string Texture => "CalamityMod/Cooldowns/EclipseEvade";
        public override Color OutlineColor => new Color(152, 206, 248);
        public override Color CooldownStartColor => new Color(255, 192, 71);
        public override Color CooldownEndColor => new Color(255, 255, 151);
    }
}