using CalamityMod;
using CalamityMod.Items.Accessories;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalRemix.Items.Materials;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace CalRemix.Items.Accessories
{
    public class IgnitedCommunity : ModItem
    {
        public int count = 0;
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 10));
            ItemID.Sets.AnimatesAsSoul[Type] = true;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine line = tooltips.Find((TooltipLine t) => t.Name.Equals("ItemName"));
            if (line != null)
                line.OverrideColor = CalamityUtils.ColorSwap(Color.OrangeRed, Color.Gold, 3f);
            TooltipLine line2 = tooltips.Find((TooltipLine t) => t.Text.Contains("Maxes out at 30% damage and 60 defense"));
            if (line2 != null)
            {
                TooltipLine lineAdd = new TooltipLine(Mod, "CalRemix:IgnitedStats", $"Remix Items crafted: {count} ({count}% damage and {count * 2} defense)");
                lineAdd.OverrideColor = CalamityUtils.ColorSwap(Color.OrangeRed, Color.Gold, 3f);
                tooltips.Insert(tooltips.IndexOf(line2) + 1, lineAdd);
            }
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 22;
            Item.rare = ModContent.RarityType<CalamityRed>();
            Item.Calamity().donorItem = true;
            Item.accessory = true;
        }
        public override void UpdateEquip(Player player)
        {
            player.GetDamage<GenericDamageClass>() += 0.35f;
            player.statDefense += 35;
            player.GetCritChance<GenericDamageClass>() += 7f;
            player.endurance += 0.07f;
            player.statLifeMax2 = (int)(player.statLifeMax2 * 1.12);
            player.lifeRegen += 4;
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
                AddIngredient<ConquestFragment>(2200).
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
            if (count > 30)
                count = 30;
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
    /*
     * 
            player.GetDamage<GenericDamageClass>() += 0.15f + (0.01f * count);
            player.statDefense += 15 + (2 * count);
					Further increases damage and defense whenever Calamity Remix items are crafted
					Maxes out at 30% damage and 60 defense
    public class IgnitedCommunityTracking : ModSystem
    {
        public override void PostAddRecipes()
        {
            for (int i = 0; i < Recipe.numRecipes; i++)
            {
                Recipe recipe = Main.recipe[i];
                recipe.AddOnCraftCallback(Crafted);
            }
        }

        private void Crafted(Recipe recipe, Item item, List<Item> consumedItems, Item destinationStack)
        {
            if ((item.ModItem.Mod == Mod || recipe.Mod == Mod) && !recipe.HasResult(ModContent.ItemType<TheInsacredTexts>()) && Main.LocalPlayer.HasItem(ModContent.ItemType<IgnitedCommunity>()))
            {
                foreach (Item i in Main.LocalPlayer.inventory)
                {
                    if (i.type == ModContent.ItemType<IgnitedCommunity>())
                    {
                        IgnitedCommunity ig = i.ModItem as IgnitedCommunity;
                        if (ig.count < 30)
                            ig.count++;
                    }
                }
            }
        }

    }
    */
}
