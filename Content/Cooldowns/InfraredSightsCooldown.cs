using CalamityMod.Cooldowns;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace CalRemix.Content.Cooldowns
{
    public class InfraredSightsCooldown : CooldownHandler
    {
        public static new string ID => "InfraredSights";
        public override bool ShouldDisplay => true;
        public override LocalizedText DisplayName => Language.GetOrRegister("Infrared Sights Cooldown");
        public override string Texture => "CalRemix/Content/Cooldowns/InfraredSightsCooldown";
        public override Color OutlineColor => new Color(0, 0, 0);
        public override Color CooldownStartColor => new Color(100, 100, 100);
        public override Color CooldownEndColor => new Color(50, 50, 50);
    }
}