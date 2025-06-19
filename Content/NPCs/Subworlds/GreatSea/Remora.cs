using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using CalamityMod;
using CalRemix.Content.Items.Placeables;
using CalRemix.Core.Biomes;
using CalamityMod.BiomeManagers;
using CalRemix.Content.Items.Materials;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using CalRemix.Content.NPCs.Bosses.Origen;
using Newtonsoft.Json.Linq;
using Terraria.Audio;
using Terraria.GameContent;
using System;
using Terraria.DataStructures;

namespace CalRemix.Content.NPCs.Subworlds.GreatSea
{
    public class Remora : ModNPC
    {
        public ref float Timer => ref NPC.ai[0];

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 2;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 0;
            NPC.width = 24;
            NPC.height = 24;
            NPC.defense = 0;
            NPC.lifeMax = 100;
            NPC.value = 10000;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.dontTakeDamage = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<GrandSeaBiome>().Type };
        }

        public override void AI()
        {
            NPC.TargetClosest(true);
            if (NPC.ai[2] > 0)
            {
                NPC n = Main.npc[(int)NPC.ai[2] - 1];
                if (n == null || !n.active || n.life <= 0 || (n.type != ModContent.NPCType<BullShark>() && n.type != ModContent.NPCType<Livyatan>()))
                {
                    NPC.ai[2] = -1;
                }
                else
                {
                    NPC.rotation = NPC.DirectionTo(n.Center).ToRotation() + MathHelper.Pi;
                    NPC.spriteDirection = NPC.direction = n.spriteDirection;
                    if (NPC.localAI[0] == 0 || NPC.localAI[1] == 0)
                    {
                        Vector2 rand = Main.rand.NextVector2FromRectangle(new Rectangle((int)n.position.X + 20, (int)n.position.Y + 20, n.width - 20, n.height - 20)) - n.Center;
                        NPC.localAI[0] = rand.X;
                        NPC.localAI[1] = rand.Y;
                    }
                    else
                    {
                        NPC.Center = n.Center + n.velocity + new Vector2(NPC.localAI[0], NPC.localAI[1]).RotatedBy(n.rotation);
                    }
                }
            }
            else if (NPC.AnyNPCs(ModContent.NPCType<Livyatan>()))
            {
                NPC.rotation = Utils.AngleLerp(NPC.rotation, NPC.velocity.ToRotation() + (NPC.spriteDirection == -1 ? 0 : MathHelper.Pi), 0.1f);
                NPC.dontTakeDamage = false;
                NPC whal = Main.npc[NPC.FindFirstNPC(ModContent.NPCType<Livyatan>())];
                if (Timer > 120)
                    NPC.velocity = NPC.DirectionTo(whal.Center) * 3;
                if (NPC.Distance(whal.Center) < 80 && Timer > 120)
                {
                    NPC.ai[2] = whal.whoAmI + 1;
                }
                Timer++;
            }
            else
            {
                NPC.rotation = Utils.AngleLerp(NPC.rotation, NPC.velocity.ToRotation() + (NPC.spriteDirection == -1 ? 0 : MathHelper.Pi), 0.1f);
                NPC.dontTakeDamage = false;
                Timer++;
                if (Timer % 30 == 0 && NPC.ai[1] <= 0)
                {
                    NPC.velocity = Vector2.UnitY.RotatedByRandom(MathHelper.TwoPi) * Main.rand.NextFloat(3, 5);
                }
            }
        }

        public override void FindFrame(int frameHeight)
        {
            if (NPC.ai[2] <= 0)
            {
                NPC.frameCounter++;
                if (NPC.frameCounter > 4)
                {
                    NPC.frameCounter = 0;
                    NPC.frame.Y += frameHeight;
                }
                if (NPC.frame.Y >= frameHeight * 2)
                {
                    NPC.frame.Y = 0;
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (NPC.ai[2] <= 0)
            {
                SpriteEffects fx = NPC.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                Texture2D tex = TextureAssets.Npc[Type].Value;
                spriteBatch.Draw(tex, NPC.Center - screenPos, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, new Vector2(tex.Width / 2, tex.Height / 4), NPC.scale, fx, 0);
            }
            return false;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
        new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }
    }
}
