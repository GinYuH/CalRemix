using CalamityMod.CalPlayer;
using CalamityMod.Projectiles.Summon;
using CalRemix.Content.Projectiles.Weapons;
using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs
{
    public class NowhereDragonBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            CalRemixPlayer modPlayer = player.Remix();
            if (player.ownedProjectileCounts[ModContent.ProjectileType<NowhereDragonLight>()] > 0 || player.ownedProjectileCounts[ModContent.ProjectileType<NowhereDragonDark>()] > 0)
            {
                modPlayer.nowhereDragons = true;
            }
            if (!modPlayer.nowhereDragons)
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
