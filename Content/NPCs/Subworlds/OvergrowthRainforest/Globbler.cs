using CalamityMod;
using CalamityMod.BiomeManagers;
using CalamityMod.DataStructures;
using CalamityMod.Graphics.Primitives;
using CalamityMod.Items.Materials;
using CalamityMod.NPCs.Abyss;
using CalamityMod.NPCs.NormalNPCs;
using CalamityMod.Sounds;
using CalRemix.Content.Items.Placeables.Banners;
using CalRemix.Content.Items.Placeables.Subworlds.Wolf;
using CalRemix.Core.Biomes.Subworlds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using XPT.Core.Audio.MP3Sharp.Decoding;

namespace CalRemix.Content.NPCs.Subworlds.OvergrowthRainforest
{
    public class Globbler : ModNPC
    {
        public NPC[] Arms = new NPC[2];

        public ref float SwingAttemptCooldown => ref NPC.ai[3];

        public ref float InitialLaunch => ref NPC.ai[2];

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 4;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 10;
            NPC.width = 40;
            NPC.height = 40;
            NPC.defense = 5;
            NPC.knockBackResist = 0.6f;
            NPC.lifeMax = 2000;
            NPC.value = Item.buyPrice(silver: 1);
            NPC.HitSound = SoundID.NPCHit25;
            NPC.DeathSound = SoundID.NPCDeath28;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.Calamity().VulnerableToHeat = true;
            NPC.Calamity().VulnerableToCold = true;
            NPC.Calamity().VulnerableToSickness = true;
            SpawnModBiomes = [ModContent.GetInstance<OvergrowthRainforestBiome>().Type, ModContent.GetInstance<BigOlBranchesBiome>().Type];
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
        new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }

        public override void AI()
        {
            for (int i = 0; i < Arms.Length; i++)
            {
                NPC n = Arms[i];
                if (n == null || !n.active || n.type != ModContent.NPCType<Globbler_Arm>())
                {
                    Arms[i] = Main.npc[NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<Globbler_Arm>(), ai0: NPC.whoAmI)];
                }
            }

            if (InitialLaunch == 0)
            {
                InitialLaunch = 1;
                NPC.velocity.X = Main.rand.NextBool().ToDirectionInt();
            }
            else
            {
                if (SwingAttemptCooldown <= 0)
                {
                    int choice = Main.rand.Next(0, 2);
                    int notChoice = (choice == 1) ? 0 : 1;
                    NPC randomArm = Arms[choice];
                    if (ValidArmNPC(randomArm, out Globbler_Arm army))
                    { 
                        if (randomArm.Center.Y > NPC.Center.Y - 28 && !Collision.SolidTiles(randomArm.position, randomArm.width * 2, randomArm.height * 2))
                        {
                            if (army.Launch())
                            {
                                SwingAttemptCooldown = 222;
                                Arms[notChoice].ModNPC<Globbler_Arm>().Release();
                            }
                        }
                    }
                }
                bool anySwining = false;
                for (int i = 0; i < Arms.Length; i++)
                {
                    NPC queriedArm = Arms[i];
                    if (ValidArmNPC(queriedArm, out Globbler_Arm army))
                    {
                        if (army.Swinging)
                        {
                            NPC.velocity = Vector2.Zero;
                            if (army.segments != null)
                            {
                                Vector2 idealPos = army.segments[0].position;
                                NPC.velocity = idealPos - NPC.Center;
                                anySwining = true;
                            }
                        }
                    }
                }
                if (!anySwining)
                {
                    if (NPC.velocity.Y < 12)
                    {
                        NPC.velocity.Y += 0.2f;
                    }
                }
            }
            SwingAttemptCooldown--;
        }

        public bool ValidArmNPC(NPC n, out Globbler_Arm globArm)
        {
            globArm = null;
            if (n == null)
                return false;
            if (!n.active)
                return false;
            if (n.type != ModContent.NPCType<Globbler_Arm>())
                return false;
            if (n.ModNPC<Globbler_Arm>() == null)
                return false;
            if (n.ModNPC<Globbler_Arm>().dad.whoAmI != NPC.whoAmI)
                return false;
            globArm = n.ModNPC<Globbler_Arm>();
            return true;
        }

        public override void FindFrame(int frameHeight)
        {
            if (NPC.frameCounter++ % 6 == 0)
            {
                NPC.frame.Y += frameHeight;
                if (NPC.frame.Y > frameHeight * 3)
                    NPC.frame.Y = 0;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D tex = TextureAssets.Npc[Type].Value;

            Vector2 drawPos = NPC.Bottom - screenPos;

            spriteBatch.Draw(tex, NPC.Center - screenPos, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, new Vector2(tex.Width / 2, tex.Height / 2 / Main.npcFrameCount[Type]), NPC.scale, 0, 0);
            return false;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.GreenBlood, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.GreenBlood, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemID.AcidDye, 1, 8, 13);
        }
    }
}
