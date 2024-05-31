using CalamityMod.Items.Materials;
using CalamityMod.Items.Accessories;
using CalamityMod.Rarities;
using CalamityMod.Items;
using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items.Placeables.Plates;
using CalamityMod.Items.Placeables.Ores;

namespace CalRemix.Items.Accessories
{
    public class CalamitousSoulArtifact : ModItem
    {
        internal const float MaxBonus = 7f;
        internal const float MaxDistance = 640f;
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            DisplayName.SetDefault("Calamitous Soul Artifact");
            Tooltip.SetDefault("Pain\n" +
            "Killed enemies summon stationary Brimstone Hearts that explode after 10 seconds\n" +
            "The hearts can also block projectiles and can be collected for health\n" +
            "Grants a minion slot for every minion summoned that takes up multiple slots\n" +
            "Boosts melee speed by 20%, ranged velocity by 35%, rogue damage by 45%, max minions by 8, and reduces mana cost by 75%\n" +
            "Increases damage based on proximity to enemy"); 
        }

        public override void SetDefaults()
        {
            Item.width = 36;
            Item.height = 56;
            Item.value = CalamityGlobalItem.RarityVioletBuyPrice;
            Item.rare = ModContent.RarityType<Violet>();
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.maxMinions += 8;
            player.GetModPlayer<CalRemixPlayer>().cart = true;
            float bonus = CalculateBonus(player);
            player.GetDamage<GenericDamageClass>() += bonus;
            ModContent.GetModItem(ModContent.ItemType<DimensionalSoulArtifact>()).UpdateAccessory(player, hideVisual);
            ModContent.GetModItem(ModContent.ItemType<EldritchSoulArtifact>()).UpdateAccessory(player, hideVisual);
            ModContent.GetModItem(ModContent.ItemType<PhantomicArtifact>()).UpdateAccessory(player, hideVisual);
            ModContent.GetModItem(ModContent.ItemType<AuricSoulArtifact>()).UpdateAccessory(player, hideVisual);
        }
        private static float CalculateBonus(Player player)
        {
            float bonus = 0f;

            int closestNPC = -1;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC nPC = Main.npc[i];
                if (nPC.active && !nPC.friendly && (nPC.damage > 0 || nPC.boss) && !nPC.dontTakeDamage)
                {
                    closestNPC = i;
                    break;
                }
            }
            float distance = -1f;
            for (int j = 0; j < Main.maxNPCs; j++)
            {
                NPC nPC = Main.npc[j];
                if (nPC.active && !nPC.friendly && (nPC.damage > 0 || nPC.boss) && !nPC.dontTakeDamage)
                {
                    float distance2 = Math.Abs(nPC.position.X + (float)(nPC.width / 2) - (player.position.X + (float)(player.width / 2))) + Math.Abs(nPC.position.Y + (float)(nPC.height / 2) - (player.position.Y + (float)(player.height / 2)));
                    if (distance == -1f || distance2 < distance)
                    {
                        distance = distance2;
                        closestNPC = j;
                    }
                }
            }

            if (closestNPC != -1)
            {
                NPC actualClosestNPC = Main.npc[closestNPC];

                float generousHitboxWidth = Math.Max(actualClosestNPC.Hitbox.Width / 2f, actualClosestNPC.Hitbox.Height / 2f);
                float hitboxEdgeDist = actualClosestNPC.Distance(player.Center) - generousHitboxWidth;

                if (hitboxEdgeDist < 0)
                    hitboxEdgeDist = 0;

                if (hitboxEdgeDist < MaxDistance)
                {
                    bonus = MathHelper.Lerp(0f, MaxBonus, 1f - (hitboxEdgeDist / MaxDistance));

                    if (bonus > MaxBonus)
                        bonus = MaxBonus;
                }
            }

            return bonus;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<AuricSoulArtifact>(1).
                AddIngredient<DimensionalSoulArtifact>(1).
                AddIngredient<PhantomicArtifact>(1).
                AddIngredient<EldritchSoulArtifact>(1).
                AddIngredient<Havocplate>(60).
                AddIngredient<AshesofAnnihilation>(5).
                AddIngredient<ExodiumCluster>(25).
                AddTile(TileID.DemonAltar).
                Register();
        }
    }
}
