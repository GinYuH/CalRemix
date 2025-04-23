using CalRemix.Content.Items.Tools;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalRemix.UI
{
    public class DeliciousMeatDisplay : InfoDisplay
    {
        public override bool Active() => Main.LocalPlayer.HasItemInInventoryOrOpenVoidBag(ModContent.ItemType<DeliciousMeatTracker>());
        public override string DisplayValue(ref Color displayColor, ref Color displayShadowColor)
        {
            string s;
            CalRemixPlayer player = Main.LocalPlayer.GetModPlayer<CalRemixPlayer>();
            if (player.deliciousMeatNoLife)
            {
                displayColor = Main.DiscoColor;
                s = "☆";
            }
            else if (player.deliciousMeatPrestige > 0)
            {
                displayColor = Color.LightSkyBlue;
                s = $"☆{player.deliciousMeatPrestige}-{player.deliciousMeatRedeemed}";
            }
            else
            {
                displayColor = Color.LightSkyBlue;
                s = $"{player.deliciousMeatRedeemed}";
            }
            return s;
        }
    }
}