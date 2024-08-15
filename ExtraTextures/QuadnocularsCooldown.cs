using CalamityMod.Cooldowns;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace CalRemix.ExtraTextures
{
    public class QuadnocularsCooldown : CooldownHandler
    {
        public static new string ID => "Quadnoculars";
        public override bool ShouldDisplay => true;
        public override LocalizedText DisplayName => Language.GetOrRegister("Quadnoculars Cooldown");
        public override string Texture => "CalRemix/ExtraTextures/QuadnocularsCooldown";
        public override Color OutlineColor => new Color(0, 0, 0);
        public override Color CooldownStartColor => new Color(100, 100, 100);
        public override Color CooldownEndColor => new Color(50, 50, 50);
    }
}