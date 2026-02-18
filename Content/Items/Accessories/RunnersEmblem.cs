using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Accessories
{
    public class RunnersEmblem : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = Item.height = 28;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<RunnersEmblemPlayer>().RunnersEmblem = true;
        }
    }
    public class RunnersEmblemPlayer : ModPlayer
    {
        public bool RunnersEmblem = false;

        public override void ResetEffects()
        {
            RunnersEmblem = false;
        }

        public override void PostUpdateRunSpeeds()
        {
            if (Player.mount.Active || !RunnersEmblem)
            {
                return;
            }
            Player.runAcceleration *= 1.5f;
            Player.maxRunSpeed *= 1.15f;
            Player.accRunSpeed *= 1.15f;
            Player.runSlowdown *= 1.5f;
        }
    }
}
