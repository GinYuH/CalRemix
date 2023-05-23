using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.NPCs.NormalNPCs;
using CalamityMod.CalPlayer;
using CalamityMod.NPCs.SlimeGod;
using CalamityMod.Items;
using static Terraria.ModLoader.ModContent;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityMod.Items.Accessories;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using CalamityMod.Projectiles.Magic;
using CalRemix.Projectiles.Accessories;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Items.Accessories
{
    public class TheGodfather : ModItem
    {

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            DisplayName.SetDefault("The Godfather");
            Tooltip.SetDefault("All effects of ingredients\n"+
            "Additionally:\n"+
            "Extends the duration of potion buffs by 200% and potion buffs remain active even after you die\n"+
            "Summons an evolved slime core to fight for you\n"+
            "All wild slimes will fight for you\n"+
            "Hit the Plague Pack key to launch yourself towards your cursor while ignoring damage with a 20 second cooldown\n"+
            "All weapons including true melee have a chance to fire out mini birbs, shrooms, sigils, and miniature suns\n"+
            "Removes all of the Abyss' hindering effects\n"+
            "Provides a 20% chance to revive upon dying with a cooldown of 50 seconds\n"+
            "Immunity to electric-based attacks"); 
        }

        public override void SetDefaults()
        {
            Item.defense = 20;
            Item.width = 20;
            Item.height = 20;
            Item.value = CalamityGlobalItem.Rarity15BuyPrice;
            Item.accessory = true;
            Item.rare = ModContent.RarityType<Violet>();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer calPlayer = player.Calamity();
            calPlayer.fCarapace = true;
            calPlayer.absorber = true;
            calPlayer.sponge = true;
            player.statManaMax2 += 30;
            player.buffImmune[ModContent.BuffType<ArmorCrunch>()] = true;
            CalRemixPlayer modPlayer = player.GetModPlayer<CalRemixPlayer>();
            calPlayer.amalgam = true;
            calPlayer.transformer = true;
            calPlayer.aSpark = true;
            calPlayer.hideOfDeus = true;
            calPlayer.nCore = true;
            modPlayer.godfather = true;
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
                        Projectile star = CalamityUtils.ProjectileRain(source, player.Center, 400f, 100f, 500f, 800f, 22f, ProjectileType<AstralStarMagic>(), damage, 2f, player.whoAmI);
                        if (star.whoAmI.WithinBounds(Main.maxProjectiles))
                        {
                            star.DamageType = DamageClass.Generic;
                            star.tileCollide = false;
                            star.penetrate = 1;
                        }
                        int microbeDamage = (int)player.GetBestClassDamage().ApplyTo(20);
                        int p = Projectile.NewProjectile(source, player.Center.X, player.Center.Y, 0f, 0f, ProjectileID.TruffleSpore, microbeDamage, 0f, player.whoAmI, 0f, 0f);
                        if (p.WithinBounds(Main.maxProjectiles))
                        {
                            Main.projectile[p].DamageType = DamageClass.Generic;
                            Main.projectile[p].usesLocalNPCImmunity = true;
                            Main.projectile[p].localNPCHitCooldown = 10;
                            Main.projectile[p].originalDamage = microbeDamage;
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
            }
            int brimmy = ProjectileType<CriticalSlimeCore>();

            var source2 = player.GetSource_Accessory(Item);

            if (player.whoAmI == Main.myPlayer)
            {
                int baseDamage = 600;
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
                AddIngredient<Slimelgamation>().
                AddIngredient<FungalSymbiote>().
                AddIngredient<RottenDogtooth>().
                AddIngredient<FungalClump>().
                AddIngredient<AbyssalDivingSuit>().
                AddIngredient<TheSponge>().
                AddIngredient<LuxorsGift>().
                AddIngredient<DeepDiver>().
                AddIngredient<HideofAstrumDeus>().
                AddIngredient<TheTransformer>().
                AddIngredient<BlunderBooster>().
                AddIngredient<NebulousCore>().
                AddIngredient<MiracleMatter>().
                AddTile<DraedonsForge>().
                Register();
        }
    }
}
