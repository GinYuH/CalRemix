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
using Terraria;
using CalRemix.Content.Projectiles.Accessories;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Accessories
{
    public class TheGodfather : ModItem
    {

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            // DisplayName.SetDefault("The Godfather");
            /* Tooltip.SetDefault("All effects of ingredients\n"+
            "Additionally:\n"+
            "Extends the duration of potion buffs by 200% and potion buffs remain active even after you die\n"+
            "Summons an evolved slime core to fight for you\n"+
            "All wild slimes will fight for you\n"+
            "Hit the Plague Pack key to launch yourself towards your cursor while ignoring damage with a 20 second cooldown\n"+
            "All weapons including true melee have a chance to fire out mini birbs, shrooms, sigils, and miniature suns\n"+
            "Removes all of the Abyss' hindering effects\n"+
            "Provides a 20% chance to revive upon dying with a cooldown of 50 seconds\n"+
            "Immunity to electric-based attacks"); */ 
        }

        public override void SetDefaults()
        {
            Item.defense = 20;
            Item.width = 20;
            Item.height = 20;
            Item.value = CalamityGlobalItem.RarityVioletBuyPrice;
            Item.accessory = true;
            Item.rare = ModContent.RarityType<BurnishedAuric>();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalRemixPlayer modPlayer = player.GetModPlayer<CalRemixPlayer>();
            CalamityPlayer calPlayer = player.Calamity();
            GetModItem(ItemType<TheAbsorber>()).UpdateAccessory(player, hideVisual);
            GetModItem(ItemType<TheAmalgam>()).UpdateAccessory(player, hideVisual);
            GetModItem(ItemType<TheTransformer>()).UpdateAccessory(player, hideVisual);
            GetModItem(ItemType<HideofAstrumDeus>()).UpdateAccessory(player, hideVisual);
            GetModItem(ItemType<NebulousCore>()).UpdateAccessory(player, hideVisual);
            GetModItem(ItemType<AmalgamatedBrain>()).UpdateAccessory(player, hideVisual);
            GetModItem(ItemType<BlunderBooster>()).UpdateAccessory(player, hideVisual);
            GetModItem(ItemType<DeepDiver>()).UpdateAccessory(player, hideVisual);
            GetModItem(ItemType<LuxorsGift>()).UpdateAccessory(player, hideVisual);
            GetModItem(ItemType<RottenDogtooth>()).UpdateAccessory(player, hideVisual);
            GetModItem(ItemType<FungalSymbiote>()).UpdateAccessory(player, hideVisual);
            GetModItem(ItemType<AbyssalDivingSuit>()).UpdateAccessory(player, hideVisual);

            modPlayer.godfather = true;

            { 
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
