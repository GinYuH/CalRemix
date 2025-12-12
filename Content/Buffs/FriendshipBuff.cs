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
                for (int i = 0; i < 2; i++)
                    Projectile.NewProjectile(player.GetSource_FromThis(), player.Center + new Vector2(Main.rand.Next(-2000, 2000), -600), Vector2.UnitY.RotatedBy(MathHelper.ToRadians(30) * Main.windSpeedTarget.DirectionalSign()).RotatedByRandom(MathHelper.ToRadians(22)) * 20, ModContent.ProjectileType<FriendshipStar>(), 1000, 1, player.whoAmI);
            }
        }
    }
}
