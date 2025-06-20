using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using Terraria.Audio;
using System;
using CalamityMod.Items.Accessories;
using CalRemix.Core.Biomes;

namespace CalRemix.Content.NPCs.Subworlds.GreatSea
{
    public class Xiphactinus : ModNPC
    {
        public ref float Timer => ref NPC.ai[0];
        public int HeadIndex => (int)NPC.ai[2] - 1;

        public NPC Head => Main.npc[HeadIndex];

        public Vector2 savePosition => new Vector2(NPC.localAI[0], NPC.localAI[1]);

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 3;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 90;
            NPC.width = 70;
            NPC.height = 40;
            NPC.defense = 10;
            NPC.lifeMax = 10000;
            NPC.value = 0;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = false;
            NPC.knockBackResist = 0;
            NPC.HitSound = SoundID.NPCHit50 with { Pitch = -0.4f };
            NPC.DeathSound = SoundID.NPCDeath40;
            NPC.GravityIgnoresLiquid = true;
            NPC.waterMovementSpeed = 1f;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<PrimordialCavesBiome>().Type };
        }

        public override void AI()
        {
            if (NPC.ai[1] == 0)
            {
                if (!NPC.wet)
                {
                    NPC.velocity.Y = 12;
                    return;
                }
                NPC.TargetClosest(false);
                if (Timer % 100 == 0 || NPC.collideX || NPC.collideY)
                {
                    if (NPC.velocity.Length() < 1)
                    {
                        NPC.velocity = Vector2.UnitY.RotatedByRandom(MathHelper.TwoPi) * Main.rand.NextFloat(1, 3f);
                    }
                    else
                    {
                        NPC.velocity = NPC.velocity.RotatedByRandom(MathHelper.PiOver4 * 0.5f);
                    }
                }
                Timer++;
                NPC.spriteDirection = NPC.direction = NPC.velocity.X.DirectionalSign();
                NPC.rotation = Utils.AngleLerp(NPC.rotation, NPC.velocity.ToRotation() - (NPC.direction == 1 ? 0 : MathHelper.Pi), 0.1f);
                if (Main.player[NPC.target].Distance(NPC.Center) < 500 && Timer > 120 && !NPC.AnyNPCs(ModContent.NPCType<Liopleurodon>()))
                {
                    NPC.ai[1] = 1;
                    Timer = 0;
                }
            }
            else if (NPC.ai[1] == 1)
            {
                Timer++;
                NPC.velocity *= 0.97f;
                if (Timer > 40)
                {
                    NPC.ai[1] = 2;
                    Timer = 0;
                    Vector2 spawnPos = NPC.Center + NPC.DirectionTo(Main.player[NPC.target].Center) * 50;
                    SoundEngine.PlaySound(BetterSoundID.ItemGrenadeChuck with { Pitch = 0.6f }, NPC.Center);
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        int n = NPC.NewNPC(NPC.GetSource_FromThis(), (int)spawnPos.X, (int)spawnPos.Y, ModContent.NPCType<XiphactinusHead>(), ai0: NPC.whoAmI + 1);
                        Main.npc[n].direction = NPC.direction;
                        Main.npc[n].spriteDirection = NPC.spriteDirection;
                        Main.npc[n].rotation = NPC.rotation;
                        Main.npc[n].velocity = NPC.DirectionTo(Main.player[NPC.target].Center) * 8;
                        NPC.ai[2] = n + 1;
                    }
                }
                NPC.spriteDirection = Main.player[NPC.target].Center.X < NPC.Center.X ? -1 : 1;
                NPC.rotation = NPC.DirectionTo(Main.player[NPC.target].Center).ToRotation() + (NPC.spriteDirection == 1 ? 0 : MathHelper.Pi);
            }
            else if (NPC.ai[1] == 2)
            {                
                NPC.velocity *= 0.97f;
                NPC.spriteDirection = Main.player[NPC.target].Center.X < NPC.Center.X ? -1 : 1;
                NPC.rotation = NPC.DirectionTo(Main.player[NPC.target].Center).ToRotation() + (NPC.spriteDirection == 1 ? 0 : MathHelper.Pi);
            }
            else if (NPC.ai[1] == 3)
            {
                NPC.noTileCollide = true;
                Timer++;
                int swingTime = 50;
                int smashTime = 10;
                int maxtim = swingTime + smashTime + 40;
                if (Timer == 1)
                {
                    NPC.localAI[0] = NPC.Center.X;
                    NPC.localAI[1] = NPC.Center.Y;
                    SoundEngine.PlaySound(BetterSoundID.ItemToxicFlaskThrow with { Pitch = 0.6f }, NPC.Center);
                }

                if (Timer > 1)
                {
                    if (Timer < swingTime)
                    {
                        Vector2 dirToSave = savePosition - Head.Center;
                        Vector2 newPos = Head.Center + dirToSave.RotatedBy(MathHelper.Lerp(0, -MathHelper.Pi, CalamityUtils.SineInEasing(Utils.GetLerpValue(0, swingTime, Timer, true), 1)));
                        NPC.velocity = newPos - NPC.Center;
                    }
                    else if (Timer == swingTime)
                    {
                        NPC.localAI[0] = NPC.Center.X;
                        NPC.localAI[1] = NPC.Center.Y;
                    }
                    else if (Timer >= swingTime && Timer < swingTime + smashTime)
                    {
                        Vector2 newPos = Vector2.Lerp(savePosition, Head.Center, Utils.GetLerpValue(swingTime, smashTime + swingTime, Timer, true));
                        NPC.velocity = newPos - NPC.Center;
                    }
                    if (Timer >= swingTime + smashTime)
                    {
                        NPC.noTileCollide = false;
                        NPC.velocity *= 0.9f;
                        if (HeadIndex > -1)
                        {
                            SoundEngine.PlaySound(WulfrumAcrobaticsPack.GrabSound, NPC.Center);
                            Head.active = false;
                            NPC.ai[2] = 0;
                        }
                        if (Timer > maxtim)
                        {
                            Timer = 0;
                            NPC.ai[1] = 0;
                            NPC.noTileCollide = false;
                        }
                    }

                    if (HeadIndex > -1)
                    {
                        NPC.spriteDirection = Head.Center.X < NPC.Center.X ? -1 : 1;
                        NPC.rotation = NPC.DirectionTo(Head.Center).ToRotation() + (NPC.spriteDirection == 1 ? 0 : MathHelper.Pi);
                    }
                }

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
            if (NPC.frame.Y >= frameHeight * 3)
            {
                NPC.frame.Y = 0;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D tex = TextureAssets.Npc[Type].Value;
            Texture2D head = ModContent.Request<Texture2D>(Texture + "Head").Value;
            SpriteEffects fx = NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(tex, NPC.Center - screenPos, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, new Vector2(tex.Width / 2, tex.Height / Main.npcFrameCount[Type] / 2), NPC.scale, fx, 0);
            if (HeadIndex <= -1)
                spriteBatch.Draw(head, NPC.Center - screenPos + ((NPC.spriteDirection == -1 ? NPC.rotation - MathHelper.Pi : NPC.rotation).ToRotationVector2() * (NPC.spriteDirection == -1 ? 48 : 124)).RotatedBy(MathHelper.ToRadians(NPC.spriteDirection == -1 ? -5 : 2f)), head.Frame(1, 2, 0, 0), NPC.GetAlpha(drawColor), NPC.rotation, new Vector2(tex.Width / 2, tex.Height / Main.npcFrameCount[Type] / 2), NPC.scale, fx, 0);
            
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
