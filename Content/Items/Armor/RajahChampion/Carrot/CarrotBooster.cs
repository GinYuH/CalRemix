using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Armor.RajahChampion.Carrot
{
    public class CarrotBooster : ModItem
    {
        public override void SetStaticDefaults()
        {
            //Tooltip.SetDefault("Etheral, but crunchy.");
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 4));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
            ItemID.Sets.ItemIconPulse[Item.type] = true;
            ItemID.Sets.ItemNoGravity[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 999;
        }

        public static Color Rainbow2 => CalamityUtils.MulticolorLerp(Main.LocalPlayer.miscCounter % 100 / 100f, Color.Green, Color.Blue, Color.Red);

        public override Color? GetAlpha(Color lightColor)
        {
            return Rainbow2;
        }

        public override void PostUpdate()
        {
            Lighting.AddLight(Item.Center, Rainbow2.ToVector3() * 0.55f * Main.essScale);
        }

        public override void GrabRange(Player player, ref int grabRange)
        {
            grabRange += 100;
        }

        public override bool OnPickup(Player player)
        {
            SoundEngine.PlaySound(SoundID.Grab, player.position);
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                player.GetModPlayer<CalRemixPlayer>().CarrotLevelup();
            }
            Item.TurnToAir();
            return true;
        }
    }
}