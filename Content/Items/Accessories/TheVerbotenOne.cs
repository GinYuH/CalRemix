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
using CalamityMod.Projectiles.Magic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Content.Projectiles;
using CalRemix.Content.Cooldowns;

namespace CalRemix.Content.Items.Accessories
{
    [AutoloadEquip(EquipType.Wings)]
    public class TheVerbotenOne : ModItem
    {

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            // DisplayName.SetDefault("The Verboten One");
            /* Tooltip.SetDefault("Ultimatum\n"+
            "Provides all effects of its ingredients"); */ 
            ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(500, 14f, 3.2f);
        }

        public override void SetDefaults()
        {
            Item.defense = 20;
            Item.width = 20;
            Item.height = 20;
            Item.value = CalamityGlobalItem.RarityHotPinkBuyPrice;
            Item.accessory = true;
            Item.rare = ModContent.RarityType<HotPink>();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer calPlayer = player.Calamity();
            CalRemixPlayer modPlayer = player.GetModPlayer<CalRemixPlayer>();

            GetModItem(ItemType<TheGodfather>()).UpdateAccessory(player, hideVisual);
            if (!hideVisual)
                GetModItem(ItemType<Slimelgamation>()).UpdateAccessory(player, hideVisual);
            GetModItem(ItemType<AbyssalDivingSuit>()).UpdateAccessory(player, hideVisual);
            GetModItem(ItemType<ThrowersGauntlet>()).UpdateAccessory(player, hideVisual);
            GetModItem(ItemType<QuiverofMadness>()).UpdateAccessory(player, hideVisual);
            GetModItem(ItemType<MagnaCore>()).UpdateAccessory(player, hideVisual);
            if (!hideVisual)
                GetModItem(ItemType<ZenithArcanum>()).UpdateAccessory(player, hideVisual);
            GetModItem(ItemType<RoguesLootbox>()).UpdateAccessory(player, hideVisual);
            if (!hideVisual)
                GetModItem(ItemType<Calamity>()).UpdateAccessory(player, hideVisual);
            GetModItem(ItemType<DraedonsHeart>()).UpdateAccessory(player, hideVisual);
            GetModItem(ItemType<StatisVoidSash>()).UpdateAccessory(player, hideVisual);
            GetModItem(ItemType<AsgardianAegis>()).UpdateAccessory(player, hideVisual);
            GetModItem(ItemType<ReaperToothNecklace>()).UpdateAccessory(player, hideVisual);
            GetModItem(ItemType<ChaliceOfTheBloodGod>()).UpdateAccessory(player, hideVisual);
            GetModItem(ItemType<RampartofDeities>()).UpdateAccessory(player, hideVisual);
            if (!hideVisual)
                GetModItem(ItemType<UniversalStone>()).UpdateAccessory(player, hideVisual);
            GetModItem(ItemType<YharimsGift>()).UpdateAccessory(player, hideVisual);
            GetModItem(ItemType<OccultSkullCrown>()).UpdateAccessory(player, hideVisual);
            GetModItem(ItemType<HolyMantle>()).UpdateAccessory(player, hideVisual);
            GetModItem(ItemType<TheCommunity>()).UpdateAccessory(player, hideVisual);
            GetModItem(ItemType<AmbrosialAmpoule>()).UpdateAccessory(player, hideVisual);
            GetModItem(ItemType<CalamitousSoulArtifact>()).UpdateAccessory(player, hideVisual);
            GetModItem(ItemType<Radiance>()).UpdateAccessory(player, hideVisual);
            ModContent.GetModItem(ModContent.ItemType<CalamityMod.Items.Accessories.Nanotech>()).UpdateAccessory(player, hideVisual);

            var source = player.GetSource_Accessory(Item);
            modPlayer.tvo = true;
            if (hideVisual)
                modPlayer.tvohide = true;
            else
                modPlayer.tvohide = false;

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
            for (int i = 0; i < BuffLoader.BuffCount; i++) 
            {
                if (i != BuffID.PotionSickness && Main.debuff[i])
                player.buffImmune[i] = true;
            }
            if (player.wingTime < 500)
            {
                player.wingTime = 500;
            }
            player.noFallDmg = true;
            player.manaCost *= 0f;


            if (Main.LocalPlayer.HasBuff(BuffID.ManaSickness))
            {
                if (!Main.LocalPlayer.HasCooldown(MagnaCoreCooldown.ID) && player.active)
                {
                    float variance = MathHelper.TwoPi / 8;
                    for (int i = 0; i < 8; i++)
                    {
                        Vector2 velocitye = new Vector2(0f, 10f);
                        velocitye = velocitye.RotatedBy(variance * i);
                        int p = Projectile.NewProjectile(player.GetSource_Accessory(Item), player.Center, velocitye, ModContent.ProjectileType<RainbowComet>(), 20000, 0, player.whoAmI);
                        if (Main.projectile.IndexInRange(p))
                            Main.projectile[p].originalDamage = 20000;
                        Main.projectile[p].GetGlobalProjectile<CalRemixProjectile>().tvoproj = true;
                    }
                    Main.LocalPlayer.AddCooldown(MagnaCoreCooldown.ID, CalamityUtils.SecondsToFrames(10));
                }
            }
        }
        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            ascentWhenFalling = 1f;
            ascentWhenRising = 0.175f;
            maxCanAscendMultiplier = 1.2f;
            maxAscentMultiplier = 3.25f;
            constantAscend = 0.15f;
        }

        public override void AddRecipes()
        {
            { 
            CreateRecipe().
                AddIngredient<TheGodfather>().
                AddIngredient<ThrowersGauntlet>().
                AddIngredient<QuiverofMadness>().
                AddIngredient<MagnaCore>().
                AddIngredient<ZenithArcanum>().
                AddIngredient<CalamityMod.Items.Accessories.Nanotech>().
                AddIngredient<RoguesLootbox>().
                AddIngredient<StatisVoidSash>().
                AddIngredient<AsgardianAegis>().
                AddIngredient<ReaperToothNecklace>().
                AddIngredient<ChaliceOfTheBloodGod>().
                AddIngredient<RampartofDeities>().
                AddIngredient<UniversalStone>().
                AddIngredient<Calamity>().
                AddIngredient<DraedonsHeart>().
                AddIngredient<YharimsGift>().
                AddIngredient<OccultSkullCrown>().
                AddIngredient<HolyMantle>().
                AddIngredient<CalamityMod.Items.Accessories.Wings.SeraphTracers>().
                AddIngredient<TheCommunity>().
                AddIngredient<CalamitousSoulArtifact>().
                AddIngredient<AmbrosialAmpoule>().
                AddIngredient(ItemID.CelestialShell).
                AddIngredient<ShadowspecBar>(50).
                AddTile<DraedonsForge>().
                Register();
            }
            { 
            CreateRecipe().
                AddIngredient<TheGodfather>().
                AddIngredient<ThrowersGauntlet>().
                AddIngredient<QuiverofMadness>().
                AddIngredient<MagnaCore>().
                AddIngredient<ZenithArcanum>().
                AddIngredient<CalamityMod.Items.Accessories.Nanotech>().
                AddIngredient<RoguesLootbox>().
                AddIngredient<StatisVoidSash>().
                AddIngredient<AsgardianAegis>().
                AddIngredient<ReaperToothNecklace>().
                AddIngredient<ChaliceOfTheBloodGod>().
                AddIngredient<RampartofDeities>().
                AddIngredient<UniversalStone>().
                AddIngredient<Calamity>().
                AddIngredient<DraedonsHeart>().
                AddIngredient<YharimsGift>().
                AddIngredient<OccultSkullCrown>().
                AddIngredient<HolyMantle>().
                AddIngredient<CalamityMod.Items.Accessories.Wings.SeraphTracers>().
                AddIngredient<ShatteredCommunity>().
                AddIngredient<CalamitousSoulArtifact>().
                AddIngredient<AmbrosialAmpoule>().
                AddIngredient(ItemID.CelestialShell).
                AddIngredient<ShadowspecBar>(50).
                AddTile<DraedonsForge>().
                Register();
            }
        }
    }
}
