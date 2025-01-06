using CalamityMod.UI.DraedonLogs;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace CalRemix.UI.Logs
{
    public class FannyLog3 : DraedonsLogGUI
    {
        public override int TotalPages => 4;
        public override string GetTextByPage()
        {
            return Page switch
            {
                0 => CalRemixHelper.LocalText("Lore.FannyLog3.0").Value,
                1 => CalRemixHelper.LocalText("Lore.FannyLog3.1").Value,
                2 => CalRemixHelper.LocalText("Lore.FannyLog3.2").Value,
                _ => CalRemixHelper.LocalText("Lore.FannyLog3.3").Value,
            };
        }
        public override Texture2D GetTextureByPage()
        {
            return ModContent.Request<Texture2D>("CalRemix/UI/Fanny/HelperFannyHolo").Value;
        }
    }
}
