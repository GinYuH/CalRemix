using CalamityMod.Items;
using CalamityMod.NPCs.AstrumAureus;
using CalamityMod.NPCs.SlimeGod;
using CalRemix.Projectiles.Accessories;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;
using Terraria.ModLoader;

namespace CalRemix.Items.Accessories
{
    public class NucleateGello : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            DisplayName.SetDefault("Nucleate Gello");
            Tooltip.SetDefault("Summons a nucleate to fight for you\n"+
            "Pacifies all boss servant slimes"); 
        }

        public override void SetDefaults()
        {
            Item.width = 42;
            Item.height = 36;
            Item.value = CalamityGlobalItem.Rarity5BuyPrice;
            Item.rare = ItemRarityID.Pink;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalRemixPlayer modPlayer = player.GetModPlayer<CalRemixPlayer>();
            modPlayer.nuclegel = true;
            if (!CalamityMod.Events.BossRushEvent.BossRushActive)
            {
                player.npcTypeNoAggro[NPCType<CrimsonSlimeSpawn>()] = true;
                player.npcTypeNoAggro[NPCType<CrimsonSlimeSpawn2>()] = true;
                player.npcTypeNoAggro[NPCType<CorruptSlimeSpawn>()] = true;
                player.npcTypeNoAggro[NPCType<CorruptSlimeSpawn2>()] = true;
                player.npcTypeNoAggro[NPCID.SlimeSpiked] = true;
                player.npcTypeNoAggro[NPCID.QueenSlimeMinionBlue] = true;
                player.npcTypeNoAggro[NPCID.QueenSlimeMinionPink] = true;
                player.npcTypeNoAggro[NPCID.QueenSlimeMinionPurple] = true;
            }
            int brimmy = ProjectileType<NucleateGelloMinion>();

            var source = player.GetSource_Accessory(Item);

            if (player.whoAmI == Main.myPlayer)
            {
                int baseDamage = 60;
                int swordDmg = (int)player.GetTotalDamage<SummonDamageClass>().ApplyTo(baseDamage);
                if (player.ownedProjectileCounts[brimmy] < 1)
                {
                    var sword = Projectile.NewProjectileDirect(source, player.Center, Vector2.Zero, brimmy, swordDmg, 2f, Main.myPlayer);
                    sword.originalDamage = baseDamage;
                }
            }
        }
    }
}
