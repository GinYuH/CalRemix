using CalRemix.Content.Projectiles.Weapons;
using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs
{
    public class TheDreamingGhostBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Dreaming Ghost");
            // Description.SetDefault("Aren't they beautiful?");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<PinkCrystallineButterfly>()] > 0 || player.ownedProjectileCounts[ModContent.ProjectileType<PurpleCrystallineButterfly>()] > 0)
            {
                player.lifeRegen += player.ownedProjectileCounts[ModContent.ProjectileType<PinkCrystallineButterfly>()] * 2;
                player.GetDamage<GenericDamageClass>() += player.ownedProjectileCounts[ModContent.ProjectileType<PurpleCrystallineButterfly>()] * 0.1f;
                player.GetModPlayer<CalRemixPlayer>().dreamingGhost = true;
            }
            if (!player.GetModPlayer<CalRemixPlayer>().dreamingGhost)
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
            else
            {
                player.buffTime[buffIndex] = 18000;
            }
        }
    }
}
