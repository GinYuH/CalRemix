using CalamityMod.UI.DraedonLogs;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace CalRemix.UI.Logs
{
    public class FannyLog5 : DraedonsLogGUI
    {
        public override int TotalPages => 3;
        public override string GetTextByPage()
        {
            return Page switch
            {
                0 => CalRemixHelper.LocalText("Lore.FannyLog5.0").Value,
                1 => CalRemixHelper.LocalText("Lore.FannyLog5.1").Value,
                _ => CalRemixHelper.LocalText("Lore.FannyLog5.2").Value,

            };
        }
        public override Texture2D GetTextureByPage()
        {
            return ModContent.Request<Texture2D>("CalRemix/UI/Fanny/HelperFannyHolo").Value;
        }
    }
}
