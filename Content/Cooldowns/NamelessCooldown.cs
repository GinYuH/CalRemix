using CalamityMod.Cooldowns;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace CalRemix.Content.Cooldowns
{
    public class NamelessCooldown : CooldownHandler
    {
        public static new string ID => "Nameless";
        public override bool ShouldDisplay => true;
        public override LocalizedText DisplayName => Language.GetOrRegister("Nameless Blink Cooldown");
        public override string Texture => "CalRemix/Content/Cooldowns/NamalessCooldown";
        public override Color OutlineColor => new Color(255, 186, 253);
        public override Color CooldownStartColor => new Color(255, 74, 250);
        public override Color CooldownEndColor => new Color(232, 26, 226);
    }
}