using CalamityMod.Cooldowns;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace CalRemix.Content.Cooldowns
{
    public class SnowgraveCooldown : CooldownHandler
    {
        public static new string ID => "Snowgrave";
        public override bool ShouldDisplay => true;
        public override LocalizedText DisplayName => Language.GetOrRegister("Snowgrave Cooldown");
        public override string Texture => "CalRemix/Content/Cooldowns/SnowgraveCooldown";
        public override Color OutlineColor => new Color(224, 253, 255);
        public override Color CooldownStartColor => new Color(201, 251, 255);
        public override Color CooldownEndColor => new Color(137, 225, 232);
    }
}