using CalRemix.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Buffs
{
    public class MicrobialClusterBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Microbial Cluster");
            // Description.SetDefault("'Where is it?'");
            Main.buffNoTimeDisplay[Type] = true;
            Main.vanityPet[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000;
            player.GetModPlayer<CalRemixPlayer>().nothing = true;
            bool petProjectileNotSpawned = player.ownedProjectileCounts[ModContent.ProjectileType<MicrobialClusterPet>()] <= 0;
            if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
            {
                Projectile.NewProjectile(player.GetSource_Buff(buffIndex), player.Center, Vector2.Zero, ModContent.ProjectileType<MicrobialClusterPet>(), 0, 0f, player.whoAmI, 0f, 0f);
            }
        }
    }
}
