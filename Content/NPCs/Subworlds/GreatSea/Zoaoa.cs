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
using CalamityMod.Particles;
using CalRemix.Content.Projectiles;

namespace CalRemix.Content.NPCs.Subworlds.GreatSea
{
    public class Zoaoa : ModNPC
    {
        public ref float Timer => ref NPC.ai[0];
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 4;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 0;
            NPC.width = 40;
            NPC.height = 40;
            NPC.defense = 0;
            NPC.lifeMax = 500;
            NPC.value = 0;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = false;
            NPC.knockBackResist = 0;
            NPC.HitSound = SoundID.NPCHit7;
            NPC.DeathSound = SoundID.NPCDeath4 with { Pitch = 0.6f };
            SpawnModBiomes = new int[1] { ModContent.GetInstance<PrimordialCavesBiome>().Type };
        }

        public override void AI()
        {
            if (NPC.direction == 0)
            {
                NPC.direction = Main.rand.NextBool().ToDirectionInt();
            }
            NPC.rotation += 0.1f * NPC.direction;

            NPC.TargetClosest(false);
            if (Main.player[NPC.target].Distance(NPC.Center) < 100)
            {
                NPC.ai[1]++;
                NPC.scale -= 0.001f;
            }
            if (NPC.ai[1] >= 90)
            {
                GeneralParticleHandler.SpawnParticle(new PulseRing(NPC.Center, Vector2.Zero, Color.White, 0.2f, 1f, 60));
                int idx = 0;
                int highestHealth = 0;
                foreach (NPC n in Main.ActiveNPCs)
                {
                    if (n.type != Type)
                    {
                        if (n.damage > 0)
                        {
                            if (!n.boss)
                            {
                                if (n.lifeMax > highestHealth)
                                {
                                    idx = n.whoAmI + 1;
                                    highestHealth = n.lifeMax;
                                }
                            }
                        }
                    }
                }
                if (idx != 0)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, NPC.DirectionTo(Main.npc[idx - 1].Center), ModContent.ProjectileType<ZoaoaLight>(), (int)(Main.npc[idx - 1].lifeMax * 0.05f), 0, ai0: idx); 
                    }
                }
                SoundEngine.PlaySound(SoundID.NPCDeath7, NPC.Center);
                NPC.active = false;
            }
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (NPC.frameCounter > 6)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y += frameHeight;
            }
            if (NPC.frame.Y >= frameHeight * 4)
            {
                NPC.frame.Y = 0;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D tex = TextureAssets.Npc[Type].Value;
            Texture2D glow = ModContent.Request<Texture2D>(Texture + "Purified").Value;
            Vector2 position = NPC.Center - Main.screenPosition;
            Vector2 origin = new Vector2(tex.Width / 2, tex.Height / 8);
            Color color = Color.Lerp(Color.DarkTurquoise, Color.White, Utils.GetLerpValue(0, 90, NPC.ai[1], true)) * 0.6f;
            SpriteEffects fx = NPC.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Vector2 scale = Vector2.One;
            for (int i = 0; i < 10; i++)
            {
                Vector2 vector2 = (MathF.PI * 2f * (float)i / 10f).ToRotationVector2() + (MathF.PI * 2f * (float)i / 10f).ToRotationVector2() * 2 * Math.Abs((float)Math.Sin(Main.GlobalTimeWrappedHourly));
                Main.spriteBatch.Draw(tex, position + vector2, NPC.frame, NPC.GetAlpha(color), NPC.rotation, origin, scale, fx, 0f);
                Main.spriteBatch.Draw(glow, position + vector2, NPC.frame, color, NPC.rotation, origin, scale, fx, 0f);
            }
            spriteBatch.Draw(tex, NPC.Center - screenPos, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, origin, NPC.scale, fx, 0);
            spriteBatch.Draw(glow, NPC.Center - screenPos, NPC.frame, Color.White * Utils.GetLerpValue(0, 90, NPC.ai[1], true), NPC.rotation, origin, NPC.scale, fx, 0);
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
