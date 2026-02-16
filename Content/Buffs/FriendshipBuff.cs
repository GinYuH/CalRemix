using CalamityMod;
using CalRemix.Content.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs
{
    public class FriendshipBuff : ModBuff
    {
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<CalRemixPlayer>().friendship = true;

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                float num = ((Entity)player).position.X + (float)Main.rand.Next(-1200, 1200);
                float num2 = ((Entity)player).position.Y - (float)Main.rand.Next(500, 800);
                Projectile.NewProjectile(player.GetSource_FromThis(), num, num2, 2f, 10f, ModContent.ProjectileType<FriendshipStar>(), 150, 3f, ((Entity)player).whoAmI, 0f, 0f);
            }
        }
    }
}
