using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Placeables
{
    class RoyalBunnyCage : ModItem
    {
        public override void SetDefaults()
        {

            Item.width = 24;
            Item.height = 22;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<Tiles.RoyalBunnyCage>(); //put your CustomBlock Tile name
        }

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Royal Bunny Cage");
            // Tooltip.SetDefault("");
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(Mod, "RoyalRabbit", 1);
            recipe.AddIngredient(ItemID.Terrarium, 1);
            recipe.AddRecipeGroup("AnyGoldBar", 20);
            recipe.Register();
        }

        public override void PostUpdate()
        {
            if (Item.lavaWet)
            {
                Player player = Main.player[Player.FindClosest(Item.Center, Item.width, Item.height)];
                int bunnyKills = ++NPC.killCount[Item.NPCtoBanner(NPCID.Bunny)];
                if (bunnyKills % 100 == 0 && bunnyKills < 1000)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        CalamityUtils.BroadcastLocalizedText("Mods.CalRemix.Dialog.RoyalRabbit.1", new Color(107, 137, 179));
                    }

                    SoundEngine.PlaySound(new SoundStyle("CalRemix/Content/NPCs/Bosses/RajahBoss/RajahRoarSound"), player.Center);
                    CalRemixNPC.SpawnRajah(player, new Vector2(player.Center.X, player.Center.Y - 2000));

                }

                if (bunnyKills % 100 == 0 && bunnyKills >= 1000)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        if (Main.netMode == NetmodeID.SinglePlayer)
                        {
                            Main.NewText(Language.GetTextValue("Mods.CalRemix.Dialog.RoyalRabbit.2", player.name.ToUpper()), new Color(107, 137, 179));
                        }
                        else if (Main.netMode == NetmodeID.Server)
                        {
                            ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Mods.CalRemix.Dialog.RoyalRabbit.2", player.name.ToUpper()), new Color(107, 137, 179));
                        }
                    }

                    SoundEngine.PlaySound(new SoundStyle("CalRemix/Content/NPCs/Bosses/RajahBoss/RajahRoarSound"), player.Center);
                    CalRemixNPC.SpawnRajah(player, new Vector2(player.Center.X, player.Center.Y - 2000));
                }

                Item.active = false;
            }
        }
    }
}
