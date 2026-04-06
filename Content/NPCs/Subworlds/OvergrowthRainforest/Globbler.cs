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
using System.Xml.Serialization;
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
            NPC.Calamity().VulnerableToHeat = true;
            NPC.Calamity().VulnerableToCold = true;
            NPC.Calamity().VulnerableToSickness = true;
            SpawnModBiomes = [ModContent.GetInstance<OvergrowthRainforestBiome>().Type];
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
        new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }

        public override void AI()
        {
            NPC.velocity = Vector2.Zero;
            for (int i = 0; i < Arms.Length; i++)
            {
                NPC n = Arms[i];
                if (n == null || !n.active || n.type != ModContent.NPCType<Globbler_Arm>())
                {
                    Arms[i] = Main.npc[NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<Globbler_Arm>(), ai0: NPC.whoAmI)];
                }
            }

            if (NPC.ai[2] == 0)
            {
                NPC.ai[2] = 1;
                NPC.velocity.X = Main.rand.NextBool().ToDirectionInt();
            }
            else
            {
                if (NPC.ai[3] <= 0)
                {
                    int choice = Main.rand.Next(0, 2);
                    NPC randomArm = Arms[choice];
                    if (randomArm != null)
                    if (randomArm.ModNPC != null)
                    {
                        if (randomArm.ModNPC<Globbler_Arm>() != null)
                        {
                            Globbler_Arm army = randomArm.ModNPC<Globbler_Arm>();

                            army.Launch();
                            NPC.ai[3] = 30;
                            NPC.localAI[0] = choice;
                        }
                    }
                }
                for (int i = 0; i < Arms.Length; i++)
                {
                    NPC queriedArm = Arms[i];
                    if (queriedArm != null)
                    if (queriedArm.ModNPC != null)
                    {
                        if (queriedArm.ModNPC<Globbler_Arm>() != null)
                        {
                            Globbler_Arm army = queriedArm.ModNPC<Globbler_Arm>();

                            if (army.Swinging)
                            {
                                NPC.Center = army.segments[0].position;
                            }
                        }
                    }
                }
            }
            NPC.ai[3]--;

            int choicer = (int)NPC.localAI[0];
            if (Arms[choicer] != null && Arms[choicer].active && Arms[choicer].type == ModContent.NPCType<Globbler_Arm>())
            {
                NPC npcArm = Arms[choicer];
                Globbler_Arm globArm = npcArm.ModNPC<Globbler_Arm>();
                if (globArm.Latched)
                {
                    int other = (choicer == 1) ? 0 : 1;
                    Arms[other].ModNPC<Globbler_Arm>().Release();
                }
            }

        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D tex = TextureAssets.Npc[Type].Value;

            Vector2 drawPos = NPC.Bottom - screenPos;

            spriteBatch.Draw(tex, NPC.Center - screenPos, null, NPC.GetAlpha(drawColor), NPC.rotation, new Vector2(tex.Width / 2, tex.Height / 2), NPC.scale, 0, 0);
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
