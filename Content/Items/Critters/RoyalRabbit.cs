using CalamityMod;
using CalRemix;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;


namespace CalRemix.Content.Items.Critters
{
    public class RoyalRabbit : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Royal Rabbit");
            // Tooltip.SetDefault("Under direct protection by the Pouncing Punisher");
        }

        public override void SetDefaults()
        {
            Item.width = 36;
            Item.height = 30;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = 8;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useStyle = 4;
            Item.consumable = true;
            Item.makeNPC = (short)ModContent.NPCType<NPCs.RoyalRabbit>();
        }

        public override void PostUpdate()
        {
            if (Item.lavaWet)
            {
                Player player = Main.player[Player.FindClosest(Item.Center, Item.width, Item.height)];
                int bunnyKills = ++NPC.killCount[Item.NPCtoBanner(NPCID.Bunny)];
                if (bunnyKills % 100 == 0 && bunnyKills < 1000)
                {
                    if (Main.netMode != 1)
                    {
                        CalamityUtils.DisplayLocalizedText("Mods.CalRemix.Dialog.RoyalRabbit.1", new Color(107, 137, 179));
                    }

                    SoundEngine.PlaySound(new SoundStyle("CalRemix/Content/NPCs/Bosses/RajahBoss/RajahRoarSound"), player.Center);
                    CalRemixNPC.SpawnRajah(player, new Vector2(player.Center.X, player.Center.Y - 2000));

                }

                if (bunnyKills % 100 == 0 && bunnyKills >= 1000)
                {
                    if (Main.netMode != 1)
                    {
                        if (Main.netMode == 0)
                        {
                            Main.NewText(Language.GetTextValue("Mods.CalRemix.Dialog.RoyalRabbit.2", player.name.ToUpper()), new Color(107, 137, 179));
                        }
                        else if (Main.netMode == 2)
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
