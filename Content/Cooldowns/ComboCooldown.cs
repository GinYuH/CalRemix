using CalamityMod.Cooldowns;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace CalRemix.Content.Cooldowns
{
    public class ComboCooldown : CooldownHandler
    {
        public static new string ID => "Combosoma";
        public override bool ShouldDisplay => true;
        public override LocalizedText DisplayName => Language.GetOrRegister("Combosama Cooldown");
        public override string Texture => "CalRemix/Content/Cooldowns/ComboCooldown";
        public override Color OutlineColor => new Color(223, 255, 143);
        public override Color CooldownStartColor => new Color(255, 64, 127);
        public override Color CooldownEndColor => new Color(235, 30, 201);
    }
}