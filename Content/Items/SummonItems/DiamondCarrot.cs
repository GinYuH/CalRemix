using CalamityMod;
using CalRemix.Content.NPCs.Bosses.RajahBoss;
using CalRemix.Core.World;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.SummonItems
{
    public class DiamondCarrot : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Ten Carat Carrot");
            ItemID.Sets.SortingPriorityBossSpawns[Item.type] = 13; // This helps sort inventory know this is a boss summoning item.
            //Tooltip.SetDefault(@"The fury of the Raging Rajah can be felt radiating from this ornate carrot...\nNon-consumable");
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.rare = 9;
            //AARarity = 14;
            Item.value = Item.sellPrice(0, 0, 0, 0);
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = 4;
            Item.noUseGraphic = true;
            Item.consumable = false;
            Item.UseSound = new SoundStyle("CalRemix/Content/NPCs/Bosses/RajahBoss/RajahRoarSound");
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            foreach (TooltipLine line2 in list)
            {
                if (line2.Mod == "Terraria" && line2.Name == "ItemName")
                {
                    line2.OverrideColor = new Color(255, 22, 0);
                }
            }
        }

        public override bool CanUseItem(Player player)
        {
            return !(NPC.AnyNPCs(ModContent.NPCType<Rajah>()) ||
                NPC.AnyNPCs(ModContent.NPCType<SupremeRajah>()));
        }

        public override bool? UseItem(Player player)
        {
            if (!RemixDowned.downedRajahsRevenge)
            {
                if (Main.netMode != 1) CalamityUtils.DisplayLocalizedText("Mods.CalRemix.Dialog.DiamondCarrot.1", new Color(107, 137, 179));
            }
            else
            {
                string Name;
                if (Main.netMode != 0)
                {
                    Name = "Terrarians";
                }
                else
                {
                    Name = Main.LocalPlayer.name;
                }
                Name += '!';
                if (Main.netMode != 1) {
                    if (Main.netMode == 0)
                    {
                        Main.NewText(Language.GetTextValue("Mods.CalRemix.Dialog.DiamondCarrot.2", Name), new Color(107, 137, 179));
                    }
                    else
                    {
                        ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Mods.CalRemix.Dialog.DiamondCarrot.2", Name), new Color(107, 137, 179));
                    }
                }
            }
            int overrideDirection = Main.rand.Next(2) == 0 ? -1 : 1;
            Vector2 spawnPos = player.Center + new Vector2(MathHelper.Lerp(500f, 800f, (float)Main.rand.NextDouble()) * overrideDirection, -1200);
            CalamityUtils.SpawnBossBetter(spawnPos, ModContent.NPCType<SupremeRajah>());
            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe;
            recipe = CreateRecipe(1);
            recipe.AddIngredient(null, "GoldenCarrot", 1);
            //recipe.AddIngredient(null, "UnstableSingularity", 3);
            //recipe.AddIngredient(null, "CrucibleScale", 3);
            //recipe.AddIngredient(null, "DreadScale", 3);
            recipe.AddIngredient(ItemID.Diamond, 5);
            //recipe.AddTile(null, "ACS");
            recipe.Register();
            recipe = CreateRecipe(1);
            recipe.AddIngredient(null, "PlatinumCarrot", 1);
            //recipe.AddIngredient(null, "UnstableSingularity", 3);
            //recipe.AddIngredient(null, "CrucibleScale", 3);
            //recipe.AddIngredient(null, "DreadScale", 3);
            recipe.AddIngredient(ItemID.Diamond, 5);
            //recipe.AddTile(null, "ACS");
            recipe.Register();
        }
    }
}