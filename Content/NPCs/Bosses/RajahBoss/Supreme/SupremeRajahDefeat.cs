using CalamityMod;
using CalRemix.Core.World;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Chat;
using Terraria.Localization;
using Terraria.ModLoader;


namespace CalRemix.Content.NPCs.Bosses.RajahBoss.Supreme
{
    [AutoloadBossHead]
    public class SupremeRajahDefeat : ModNPC
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Rajah Rabbit");
            Main.npcFrameCount[NPC.type] = 9;
        }

        public override void SetDefaults()
        {
            NPC.width = 130;
            NPC.height = 220;
            NPC.aiStyle = -1;
            NPC.damage = 0;
            NPC.defense = 90;
            NPC.lifeMax = 50000;
            NPC.knockBackResist = 0f;
            NPC.npcSlots = 1000f;
            NPC.dontTakeDamage = true;
            NPC.boss = true;
            NPC.netAlways = true;
            Music = MusicLoader.GetMusicSlot(ModContent.GetInstance<CalamityMod.CalamityMod>(), "Sounds/Music/Silence");
            NPC.noTileCollide = false;
        }

        public override void AI()
        {
            if (NPC.velocity.Y == 0 && Main.netMode != 1)
            {
                NPC.ai[0]++;
            }

            if (NPC.ai[0] == 120)
            {
                if (Main.netMode != 1) CalamityUtils.DisplayLocalizedText("Mods.CalRemix.Dialog.SupremeRajah.Defeat.1", new Color(107, 137, 179));
            }
            if (NPC.ai[0] == 240)
            {
                if (Main.netMode != 1) CalamityUtils.DisplayLocalizedText("Mods.CalRemix.Dialog.SupremeRajah.Defeat.2", new Color(107, 137, 179));
            }
            if (NPC.ai[0] == 360)
            {
                if (Main.netMode != 1) CalamityUtils.DisplayLocalizedText("Mods.CalRemix.Dialog.SupremeRajah.Defeat.3", new Color(107, 137, 179));
            }
            if (NPC.ai[0] == 480)
            {
                if (Main.netMode != 1) CalamityUtils.DisplayLocalizedText("Mods.CalRemix.Dialog.SupremeRajah.Defeat.4", new Color(107, 137, 179));
            }
            if (NPC.ai[0] >= 600)
            {
                NPC.ai[1] = 1;
                Music = MusicLoader.GetMusicSlot(CalRemix.instance, "Assets/Music/ThinkAboutIt");
                NPC.netUpdate = true;
            }
            if (NPC.ai[0] == 600)
            {
                if (Main.netMode != 1) CalamityUtils.DisplayLocalizedText("Mods.CalRemix.Dialog.SupremeRajah.Defeat.Elipse", new Color(107, 137, 179));
            }
            if (NPC.ai[0] >= 840)
            {
                NPC.ai[1] = 2;
                NPC.netUpdate = true;
            }
            if (NPC.ai[0] == 840)
            {
                if (Main.netMode != 1) CalamityUtils.DisplayLocalizedText("Mods.CalRemix.Dialog.SupremeRajah.Defeat.5", new Color(107, 137, 179));
            }
            if (NPC.ai[0] == 960)
            {
                if (Main.netMode != 1) CalamityUtils.DisplayLocalizedText("Mods.CalRemix.Dialog.SupremeRajah.Defeat.6", new Color(107, 137, 179));
            }
            if (NPC.ai[0] == 1080)
            {
                if (Main.netMode != 1) CalamityUtils.DisplayLocalizedText("Mods.CalRemix.Dialog.SupremeRajah.Defeat.7", new Color(107, 137, 179));
            }
            if (NPC.ai[0] == 1200)
            {
                if (Main.netMode != 1) CalamityUtils.DisplayLocalizedText("Mods.CalRemix.Dialog.SupremeRajah.Defeat.8", new Color(107, 137, 179));
            }
            if (NPC.ai[0] == 1380)
            {
                if (Main.netMode != 1) CalamityUtils.DisplayLocalizedText("Mods.CalRemix.Dialog.SupremeRajah.Defeat.9", new Color(107, 137, 179));
            }
            if (NPC.ai[0] == 1540)
            {
                if (Main.netMode != 1) CalamityUtils.DisplayLocalizedText("Mods.CalRemix.Dialog.SupremeRajah.Defeat.10", new Color(107, 137, 179));
            }
            if (NPC.ai[0] == 1660)
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
                Name += '?';
                if (Main.netMode != 1)
                {
                    if (Main.netMode == 0)
                    {
                        Main.NewText(Language.GetTextValue("Mods.CalRemix.Dialog.SupremeRajah.Defeat.11", Name), new Color(107, 137, 179));
                    }
                    else if (Main.netMode == 2)
                    {
                        ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Mods.CalRemix.Dialog.SupremeRajah.Defeat.11", Name), new Color(107, 137, 179));
                    }
                }
            }
            if (NPC.ai[0] == 1780)
            {
                if (Main.netMode != 1) CalamityUtils.DisplayLocalizedText("Mods.CalRemix.Dialog.SupremeRajah.Defeat.12", new Color(107, 137, 179));
            }
            if (NPC.ai[0] == 1900)
            {
                if (Main.netMode != 1) CalamityUtils.DisplayLocalizedText("Mods.CalRemix.Dialog.SupremeRajah.Defeat.13", new Color(107, 137, 179));
            }
            if (NPC.ai[0] == 2020)
            {
                if (Main.netMode != 1) CalamityUtils.DisplayLocalizedText("Mods.CalRemix.Dialog.SupremeRajah.Defeat.14", new Color(107, 137, 179));
            }
            if (NPC.ai[0] >= 2180)
            {
                if (Main.netMode != 1) CalamityUtils.DisplayLocalizedText("Mods.CalRemix.Dialog.SupremeRajah.Defeat.15", new Color(107, 137, 179));
                RemixDowned.downedRajahsRevenge = true;
                if (Main.netMode != 1) CalamityUtils.DisplayLocalizedText("Mods.CalRemix.Dialog.SupremeRajah.Defeat.16", Color.Green);
                int p = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, ModContent.ProjectileType<SupremeRajahLeave>(), 0, 0, Main.myPlayer);
                Main.projectile[p].position = NPC.position;
                NPC.active = false;
                NPC.netUpdate = true;
            }
        }

        public override void FindFrame(int frameHeight)
        {
            if (NPC.ai[1] == 0)
            {
                if (NPC.frameCounter++ > 15)
                {
                    NPC.frameCounter = 0;
                    NPC.frame.Y += frameHeight;
                }
                if (NPC.frame.Y > frameHeight * 3)
                {
                    NPC.frame.Y = 0;
                }
            }
            else if (NPC.ai[1] == 1)
            {
                NPC.frame.Y = frameHeight * 4;
            }
            else if (NPC.ai[1] == 2)
            {
                if (NPC.frameCounter++ > 15)
                {
                    NPC.frameCounter = 0;
                    NPC.frame.Y += frameHeight;
                }
                if (NPC.frame.Y > frameHeight * 8 || NPC.frame.Y < frameHeight * 5)
                {
                    NPC.frame.Y = frameHeight * 5;
                }
            }
        }
    }
}