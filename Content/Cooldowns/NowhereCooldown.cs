using CalamityMod.Cooldowns;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace CalRemix.Content.Cooldowns
{
    public class NowhereCooldown : CooldownHandler
    {
        public static new string ID => "NowhereAura";
        public override bool ShouldDisplay => true;
        public override LocalizedText DisplayName => Language.GetOrRegister("Nowhere Aura Cooldown");
        public override string Texture => "CalRemix/Content/Cooldowns/NowhereCooldown";
        public override Color OutlineColor => new Color(106, 102, 204);
        public override Color CooldownStartColor => new Color(235, 255, 168);
        public override Color CooldownEndColor => new Color(1, 0, 23);
    }
}