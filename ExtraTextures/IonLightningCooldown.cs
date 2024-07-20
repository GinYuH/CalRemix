using CalamityMod.Cooldowns;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace CalRemix.ExtraTextures
{
    public class IonLightningCooldown : CooldownHandler
    {
        public static new string ID => "Ionic";
        public override bool ShouldDisplay => true;
        public override LocalizedText DisplayName => Language.GetOrRegister("Ionic Lightning Cooldown");
        public override string Texture => "CalRemix/ExtraTextures/IonLightningCooldown";
        public override Color OutlineColor => new Color(229, 255, 0);
        public override Color CooldownStartColor => new Color(0, 200, 255);
        public override Color CooldownEndColor => new Color(9, 172, 217);
    }
}