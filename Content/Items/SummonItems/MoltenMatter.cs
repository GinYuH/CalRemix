using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using CalamityMod.Rarities;
using CalRemix.Content.NPCs.Bosses.Pyrogen;
using CalRemix.Content.Items.Placeables.Plates.Molten;
using CalRemix.Content.Tiles;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using CalRemix.World;

namespace CalRemix.Content.Items.SummonItems
{
    public class MoltenMatter : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Molten Matter");
            // Tooltip.SetDefault("Summons Pyrogen when used in the Underworld");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ItemID.Sets.SortingPriorityBossSpawns[Type] = 12;
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 1;
            Item.value = 0;
            Item.rare = ModContent.RarityType<Turquoise>();
            Item.useAnimation = 30;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = false;
        }
        public override bool CanUseItem(Player player)
        {
            return !NPC.AnyNPCs(ModContent.NPCType<Pyrogen>()) && (player.ZoneUnderworldHeight || ProfanedDesert.scorchedWorld);
        }
        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                SoundEngine.PlaySound(SoundID.Roar, player.position);
                CalRemixHelper.SpawnClientBossRandomPos(ModContent.NPCType<Pyrogen>(), player.Center);
            }
            return true;
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frameI, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Color overlay = Main.zenithWorld ? Color.Blue : Color.White;
            spriteBatch.Draw(texture, position, null, overlay, 0f, origin, scale, 0, 0);
            return false;
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Color overlay = Main.zenithWorld ? Color.Blue : lightColor;
            spriteBatch.Draw(texture, Item.position - Main.screenPosition, null, overlay, 0f, Vector2.Zero, 1f, 0, 0);
            return false;
        }

        public override void UpdateInventory(Player player)
        {
            if (Main.zenithWorld)
                Item.SetNameOverride("Modicum Matter");
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            if (Main.zenithWorld)
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Text.Contains("Pyrogen"))
                {
                        list[i].Text.Replace("Pyrogen", "Cryogen");
                }
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<MoltenNavyplate>(20)
                .AddIngredient<MoltenCinderplate>(20)
                .AddIngredient<MoltenHavocplate>(20)
                .AddIngredient<MoltenPlagueplate>(20)
                .AddIngredient<MoltenElumplate>(20)
                .AddIngredient<MoltenOnyxplate>(20)
                .AddTile(ModContent.TileType<AncientConsole>())
                .Register();
        }
    }
}