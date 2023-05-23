using CalamityMod.Items;
using CalamityMod.NPCs.NormalNPCs;
using CalamityMod.CalPlayer;
using CalamityMod.NPCs.SlimeGod;
using CalamityMod.Items.Materials;
using CalRemix.Projectiles.Accessories;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;
using Terraria.ModLoader;

namespace CalRemix.Items.Accessories
{
    public class Assortegelatin : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            DisplayName.SetDefault("Assortegelatin");
            Tooltip.SetDefault("Summons slimes to fight for you based on your current biome\n"+
            "All wild slimes will fight for you\n"+
            "Pacifies all slimes, even bosses"); 
        }

        public override void SetDefaults()
        {
            Item.width = 42;
            Item.height = 36;
            Item.value = CalamityGlobalItem.Rarity10BuyPrice;
            Item.rare = ItemRarityID.Red;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalRemixPlayer modPlayer = player.GetModPlayer<CalRemixPlayer>();
            CalamityPlayer calPlayer = player.GetModPlayer<CalamityPlayer>();
            modPlayer.nuclegel = true;
            modPlayer.assortegel = true;
            calPlayer.royalGel = true;

            if (!CalamityMod.Events.BossRushEvent.BossRushActive)
            {
                player.npcTypeNoAggro[NPCType<CrimsonSlimeSpawn>()] = true;
                player.npcTypeNoAggro[NPCType<CrimsonSlimeSpawn2>()] = true;
                player.npcTypeNoAggro[NPCType<CorruptSlimeSpawn>()] = true;
                player.npcTypeNoAggro[NPCType<CorruptSlimeSpawn2>()] = true;
                player.npcTypeNoAggro[NPCType<AeroSlime>()] = true;
                player.npcTypeNoAggro[NPCType<BloomSlime>()] = true;
                player.npcTypeNoAggro[NPCType<CalamityMod.NPCs.Crags.CharredSlime>()] = true;
                player.npcTypeNoAggro[NPCType<CalamityMod.NPCs.Astral.AstralSlime>()] = true;
                player.npcTypeNoAggro[NPCType<CalamityMod.NPCs.PlagueEnemies.PestilentSlime>()] = true;
                player.npcTypeNoAggro[NPCType<CryoSlime>()] = true;
                player.npcTypeNoAggro[NPCType<PerennialSlime>()] = true;
                player.npcTypeNoAggro[NPCType<NPCs.AuricSlime>()] = true;
                player.npcTypeNoAggro[NPCID.SlimeSpiked] = true;
                player.npcTypeNoAggro[NPCID.QueenSlimeMinionBlue] = true;
                player.npcTypeNoAggro[NPCID.QueenSlimeMinionPink] = true;
                player.npcTypeNoAggro[NPCID.QueenSlimeMinionPurple] = true;
                player.npcTypeNoAggro[1] = true;
                player.npcTypeNoAggro[16] = true;
                player.npcTypeNoAggro[59] = true;
                player.npcTypeNoAggro[71] = true;
                player.npcTypeNoAggro[81] = true;
                player.npcTypeNoAggro[138] = true;
                player.npcTypeNoAggro[121] = true;
                player.npcTypeNoAggro[122] = true;
                player.npcTypeNoAggro[141] = true;
                player.npcTypeNoAggro[147] = true;
                player.npcTypeNoAggro[183] = true;
                player.npcTypeNoAggro[184] = true;
                player.npcTypeNoAggro[204] = true;
                player.npcTypeNoAggro[225] = true;
                player.npcTypeNoAggro[244] = true;
                player.npcTypeNoAggro[302] = true;
                player.npcTypeNoAggro[333] = true;
                player.npcTypeNoAggro[335] = true;
                player.npcTypeNoAggro[334] = true;
                player.npcTypeNoAggro[336] = true;
                player.npcTypeNoAggro[537] = true;
            }
            int brimmy = ProjectileType<AssortMinion>();

            var source = player.GetSource_Accessory(Item);

            if (player.whoAmI == Main.myPlayer)
            {
                int baseDamage = 400;
                int swordDmg = (int)player.GetTotalDamage<SummonDamageClass>().ApplyTo(baseDamage);
                if (player.ownedProjectileCounts[brimmy] < 1)
                {
                    var sword = Projectile.NewProjectileDirect(source, player.Center, Vector2.Zero, brimmy, swordDmg, 2f, Main.myPlayer);
                    sword.originalDamage = baseDamage;
                }
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.RoyalGel).
                AddIngredient(ItemID.VolatileGelatin).
                AddIngredient(ItemType<NucleateGello>()).
                AddIngredient(ItemType<MeldConstruct>()).
                AddIngredient(ItemID.LunarBar, 10).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
