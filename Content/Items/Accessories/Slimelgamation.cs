using CalamityMod.NPCs.NormalNPCs;
using CalamityMod.CalPlayer;
using CalamityMod.NPCs.SlimeGod;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Magic;
using CalamityMod.Rarities;
using CalamityMod.Items;
using CalamityMod;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria;
using Microsoft.Xna.Framework;
using CalRemix.Content.Projectiles.Accessories;
using Terraria.ID;
using Terraria.ModLoader;

using static Terraria.ModLoader.ModContent;
using Terraria.DataStructures;

namespace CalRemix.Content.Items.Accessories
{
    public class Slimelgamation : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
             // DisplayName.SetDefault("Slimelgamation");
             /* Tooltip.SetDefault("Extends the duration of potion buffs by 200% and potion buffs remain active even after you die\n"+
            "15% increased damage\n"+
            "Summons an evolved slime core to fight for you\n"+
            "All wild slimes will fight for you\n"+
            "Shade and brimstone flames rain down when you are hit\n"+
            "Nearby enemies receive a variety of debuffs when you are hit\n"+
            "Life drains over time in exchange for a significant damage boost as health decreases\n"+
            "You leave behind a trail of spores, toxic clouds, plague bees, and brine\n"+
            "Standing still generates an aura that slows enemies, and increases defense and life regen\n"+
            "All weapons have a chance to fire out mini birbs and miniature suns\n"+
            "50% of the items stats are granted to team members"); */
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 10));
            ItemID.Sets.AnimatesAsSoul[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 34;
            Item.accessory = true;
            Item.value = CalamityGlobalItem.RarityVioletBuyPrice;
            Item.rare = ModContent.RarityType<Violet>();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer calPlayer = player.Calamity();
            CalRemixPlayer modPlayer = player.GetModPlayer<CalRemixPlayer>();
            calPlayer.amalgam = true;
            player.GetDamage<GenericDamageClass>() += 0.15f;

            if (player.immune)
            {
                var source = player.GetSource_Accessory(Item);
                if (player.miscCounter % 6 == 0)
                {
                    if (player.whoAmI == Main.myPlayer)
                    {
                        int damage = (int)player.GetBestClassDamage().ApplyTo(300);
                        Projectile rain = CalamityUtils.ProjectileRain(source, player.Center, 400f, 100f, 500f, 800f, 22f, ProjectileType<AuraRain>(), damage, 2f, player.whoAmI);
                        if (rain.whoAmI.WithinBounds(Main.maxProjectiles))
                        {
                            rain.DamageType = DamageClass.Generic;
                            rain.tileCollide = false;
                            rain.penetrate = 1;
                        }
                    }
                }
            }
            modPlayer.nuclegel = true;
            calPlayer.royalGel = true;
            modPlayer.amalgel = true;
            if (!CalamityMod.Events.BossRushEvent.BossRushActive)
            {
                player.npcTypeNoAggro[NPCType<CrimsonSlimeSpawn>()] = true;
                player.npcTypeNoAggro[NPCType<CrimsonSlimeSpawn2>()] = true;
                player.npcTypeNoAggro[NPCType<CorruptSlimeSpawn>()] = true;
                player.npcTypeNoAggro[NPCType<CorruptSlimeSpawn2>()] = true;
                player.npcTypeNoAggro[NPCType<AeroSlime>()] = true;
                player.npcTypeNoAggro[NPCType<BloomSlime>()] = true;
                player.npcTypeNoAggro[NPCType<CalamityMod.NPCs.Crags.InfernalCongealment>()] = true;
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
            int brimmy = ProjectileType<SlimeCore>();

            var source2 = player.GetSource_Accessory(Item);

            if (!modPlayer.godfather)
            if (player.whoAmI == Main.myPlayer)
            {
                int baseDamage = 400;
                int swordDmg = (int)player.GetTotalDamage<SummonDamageClass>().ApplyTo(baseDamage);
                if (player.ownedProjectileCounts[brimmy] < 1)
                {
                    var sword = Projectile.NewProjectileDirect(source2, player.Center, Vector2.Zero, brimmy, swordDmg, 2f, Main.myPlayer);
                    sword.originalDamage = baseDamage;
                }
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<TheAmalgam>().
                AddIngredient<Assortegelatin>().
                AddIngredient<YharonSoulFragment>(5).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
