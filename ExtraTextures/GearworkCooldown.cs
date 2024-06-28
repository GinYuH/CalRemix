using CalamityMod.Cooldowns;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace CalRemix.ExtraTextures
{
    public class GearworkCooldown : CooldownHandler
    {
        public static new string ID => "Gearwork";
        public override bool ShouldDisplay => true;
        public override LocalizedText DisplayName => Language.GetOrRegister("Gearwork Cooldown");
        public override string Texture => "CalRemix/ExtraTextures/GearworkCooldown";
        public override Color OutlineColor => new Color(153, 132, 28);
        public override Color CooldownStartColor => new Color(84, 110, 83);
        public override Color CooldownEndColor => new Color(34, 56, 34);
    }
}