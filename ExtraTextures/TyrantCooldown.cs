using CalamityMod.Cooldowns;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace CalRemix.ExtraTextures
{
    public class TyrantCooldown : CooldownHandler
    {
        public static new string ID => "Tyrant";
        public override bool ShouldDisplay => true;
        public override LocalizedText DisplayName => Language.GetOrRegister("Tyrant Cooldown");
        public override string Texture => "CalRemix/ExtraTextures/TyrantCooldown";
        public override Color OutlineColor => new Color(0, 49, 82);
        public override Color CooldownStartColor => new Color(201, 0, 0);
        public override Color CooldownEndColor => new Color(102, 0, 0);
    }
}