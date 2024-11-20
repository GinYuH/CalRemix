using CalamityMod;
using CalamityMod.Items.Accessories;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalRemix.Content.Items.Lore;
using CalRemix.Content.Items.Materials;
using CalRemix.Content.Items.Placeables;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace CalRemix.Content.Items.Accessories
{
    public class IgnitedCommunity : ModItem
    {
        public int count = 0;
        public const int MaxLevel = 45;
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 10));
            ItemID.Sets.AnimatesAsSoul[Type] = true;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine line = tooltips.Find((TooltipLine t) => t.Text.Contains("{0}") && t.Text.Contains("{1}"));
            if (line != null && !ModLoader.HasMod("MagicStorage"))
            {
                string newText = line.Text.Replace("{0}",$"{count}");
                newText = newText.Replace("{1}", $"{count * 2}");
                line.Text = newText;
            }
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 22;
            Item.rare = ModContent.RarityType<CalamityRed>();
            Item.Remix().devItem = "Remix";
            Item.accessory = true;
        }
        public override void UpdateEquip(Player player)
        {
            player.Calamity().shatteredCommunity = true;
            player.Calamity().RageDamageBoost += 25f * 0.01f;
            player.GetDamage<GenericDamageClass>() += 0.1f + count * 0.01f;
            player.GetCritChance<GenericDamageClass>() += 5f;
            player.statLifeMax2 = (int)(player.statLifeMax2 * 1.1f); 
            player.wingTimeMax = (int)(player.wingTimeMax * 1.2f);
            player.statDefense += 10 + count * 2;
            player.endurance += 0.05f;
            player.lifeRegen += 2;
            player.moveSpeed += 0.1f;
        }
        public override void PostUpdate()
        {
            float essScale = Main.essScale;
            Lighting.AddLight(Item.Center, 0.92f * essScale, 0.92f * essScale, 0.22f * essScale);
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<ShatteredCommunity>(1).
                AddIngredient<LightmixBar>(5).
                AddIngredient<ConquestFragment>(220).
                AddTile<DraedonsForge>().
                Register();
        }

        public override void SaveData(TagCompound tag)
        {
            tag.Add("count", count);
        }
        public override void LoadData(TagCompound tag)
        {
            count = tag.GetInt("count");
            if (count > MaxLevel)
                count = MaxLevel;
        }
        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(count);
        }
        public override void NetReceive(BinaryReader reader)
        {
            count = reader.ReadInt32();
        }
    }
    public class IgnitedCommunityTracking : ModSystem
    {
        private static readonly List<int> ignoredRecipes = new()
        {
            ModContent.ItemType<TheInsacredTexts>(),
            ModContent.ItemType<Anomaly109>(),
            ModContent.ItemType<ElementalBar>(),
            ModContent.ItemType<HauntedBar>(),
            ModContent.ItemType<Slumbering>()
        };
        public override void PostAddRecipes()
        {
            if (!ModLoader.HasMod("MagicStorage"))
            {
                for (int i = 0; i < Recipe.numRecipes; i++)
                {
                    Recipe recipe = Main.recipe[i];
                    recipe.AddOnCraftCallback(Crafted);
                }
            }
        }
        private void Crafted(Recipe recipe, Item item, List<Item> consumedItems, Item destinationStack)
        {
            if ((item.ModItem.Mod == Mod || recipe.Mod == Mod) && !ignoredRecipes.Contains(recipe.createItem.type))
            {
                if (Main.LocalPlayer.HasItem(ModContent.ItemType<IgnitedCommunity>()))
                {
                    foreach (Item i in Main.LocalPlayer.inventory)
                    {
                        if (i.type == ModContent.ItemType<IgnitedCommunity>())
                        {
                            IgnitedCommunity ig = i.ModItem as IgnitedCommunity;
                            if (ig.count < IgnitedCommunity.MaxLevel)
                                ig.count++;
                        }
                    }
                }
                else
                {
                    for (int i = 3; i < 10; i++)
                    {
                        if (Main.LocalPlayer.armor[i].type == ModContent.ItemType<IgnitedCommunity>())
                        {
                            IgnitedCommunity ig = Main.LocalPlayer.armor[i].ModItem as IgnitedCommunity;
                            if (ig.count < IgnitedCommunity.MaxLevel)
                                ig.count++;
                        }
                    }
                }
            }
        }

    }

}
