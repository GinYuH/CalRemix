using CalamityMod.Cooldowns;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace CalRemix.Content.Cooldowns
{
    public class ParadiseHealCooldown : CooldownHandler
    {
        public static new string ID => "ParadiseHeal";
        public override bool ShouldDisplay => true;
        public override LocalizedText DisplayName => Language.GetOrRegister("Paradise Heal Cooldown");
        public override string Texture => "CalRemix/Content/Cooldowns/ParadiseHealCooldown";
        public override Color OutlineColor => new Color(251, 255, 194);
        public override Color CooldownStartColor => new Color(246, 255, 115);
        public override Color CooldownEndColor => new Color(241, 255, 31);
    }
}