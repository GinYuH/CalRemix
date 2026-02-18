using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace CalRemix.Content.NPCs
{
    public class RoyalRabbit : ModNPC
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Royal Rabbit");
            Main.npcFrameCount[NPC.type] = 7;
        }
        public override void SetDefaults()
        {
            NPCID.Sets.TownCritter[NPC.type] = true;
            NPC.width = 28;
            NPC.height = 24;
            NPC.defense = 0;
            NPC.lifeMax = 100;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.5f;
            NPC.npcSlots = 0f;
            NPC.aiStyle = NPCAIStyleID.Passive;
            AIType = NPCID.Bunny;  //npc behavior
            AnimationType = NPCID.Bunny;
            NPC.dontTakeDamageFromHostiles = false;
            Banner = NPC.type;
            BannerItem = ItemID.BunnyBanner;
            NPC.catchItem = (short)ModContent.ItemType<Items.Critters.RoyalRabbit>();
            NPC.rarity = 6;
        }

        public override void OnKill()
        {
            Player player = Main.player[Player.FindClosest(NPC.Center, NPC.width, NPC.height)];
            int bunnyKills = NPC.killCount[Item.NPCtoBanner(NPCID.Bunny)];
            if (bunnyKills % 100 == 0 && bunnyKills < 1000)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    CalamityUtils.BroadcastLocalizedText("Mods.CalRemix.Dialog.RoyalRabbit.1", new Color(107, 137, 179));
                }

                SoundEngine.PlaySound(new SoundStyle("CalRemix/Content/NPCs/Bosses/RajahBoss/RajahRoarSound"), NPC.Center);
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
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, 77, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("RoyalRabbit1").Type, 1f);
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.OverworldDayGrassCritter.Chance * (NPC.downedGolemBoss ? .005f : 0f);
        }
        
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return false;
        }
    }
}