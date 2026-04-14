using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using CalamityMod;
using CalamityMod.BiomeManagers;
using CalRemix.Content.Items.Placeables.Banners;
using Microsoft.Xna.Framework;
using System;
using Terraria.Enums;
using CalamityMod.NPCs.NormalNPCs;
using Microsoft.Xna.Framework.Graphics;
using CalRemix.Content.Items.Placeables.Subworlds.Wolf;
using Terraria.GameContent;
using CalamityMod.Items.Materials;
using CalRemix.Core.Biomes.Subworlds;
using CalamityMod.Items.Weapons.Rogue;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using CalamityMod.DataStructures;
using CalRemix.Content.Projectiles.Hostile;
using CalamityMod.Projectiles.Typeless;
using Terraria.Audio;
using CalamityMod.NPCs.Ravager;
using Terraria.DataStructures;
using System.IO;

namespace CalRemix.Content.NPCs.Subworlds.OvergrowthRainforest
{
    public class GigamothLarva : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 1;
        }

        public static List<Vector2> segments = new();

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 210;
            NPC.width = 60;
            NPC.height = 100;
            NPC.defense = 50;
            NPC.lifeMax = 30000;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            NPC.behindTiles = true;
            NPC.value = Item.buyPrice(gold: 1);
            NPC.HitSound = BetterSoundID.HitPossessed with { Pitch = -0.8f };
            NPC.DeathSound = BetterSoundID.DeathTortoise with { Pitch = -0.5f }; 
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
            int segSize = 100;
            if (segments.Count <= 0)
            {
                for (int i = -2; i <= 2; i++)
                {
                    segments.Add(new Vector2(i * segSize * -NPC.spriteDirection, 0));
                }
            }
            NPC.TargetClosest();
            Player targ = Main.player[NPC.target];
            if (NPC.velocity.X != 0)
            NPC.spriteDirection = NPC.direction = -NPC.velocity.X.DirectionalSign();

            if (NPC.ai[0] == 0)
            {
                NPC.ai[1] = 1;
                if (NPC.ai[2] <= 0 && NPC.velocity.X == 0)
                {
                    NPC.velocity.X = (int)Main.rand.NextBool().ToDirectionInt() * 2;
                }
                if (NPC.velocity.X != 0)
                {
                    if (Main.rand.NextBool(120))
                    {
                        NPC.velocity.X = 0;
                        NPC.ai[2] = Main.rand.Next(60, 300);
                    }
                }
                if (NPC.HasSight(targ.Center) && NPC.Distance(targ.Center) < 420)
                {
                    NPC.netUpdate = true;
                    NPC.ai[0] = 1;
                }
                NPC.ai[2]--;
                DefaultSegments(segSize);
            }
            else if (NPC.ai[0] == 1)
            {
                bool silk = Main.player[NPC.target].webbed;
                NPC.velocity.X = NPC.DirectionTo(targ.Center).X.DirectionalSign() * (silk ? 8 : 3);
                NPC.ai[2]++;
                if (NPC.ai[2] > 120 && NPC.ai[2] % 8 == 0)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Vector2 shoot = NPC.Center + Vector2.UnitX * -NPC.direction * 240;
                        Vector2 vel = shoot.DirectionTo(targ.Center).RotatedByRandom(MathHelper.PiOver4) * 16;
                        if (Math.Sign(vel.X) == NPC.direction)
                            vel.X *= -1;
                        SoundEngine.PlaySound(BetterSoundID.ItemDreadnautSpit with { Pitch = 0.5f }, NPC.Center);
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), shoot, vel, ModContent.ProjectileType<GigaSilk>(), CalRemixHelper.ProjectileDamage(180, 320), 1);
                    }
                }
                if (NPC.ai[2] > 260)
                {
                    NPC.netUpdate = true;
                    NPC.ai[0] = 2;
                    NPC.ai[2] = 0;
                }
                DefaultSegments(segSize);
            }
            else if (NPC.ai[0] == 2)
            {
                NPC.velocity.X = 0;
                NPC.ai[2]++;
                int liftUp = 20;
                int smash = liftUp + 10;
                int wait = smash + 30;

                if (NPC.ai[2] < liftUp)
                {
                    NPC.ai[1] = Utils.GetLerpValue(0, liftUp, NPC.ai[2], true);


                    bool left = NPC.spriteDirection == 1;

                    int x1 = segSize * -2;
                    int x2 = -segSize;
                    int x3 = segSize;
                    int x4 = segSize;

                    int y1 = left ? -220 : 0;
                    int y2 = left ? -320 : 0;
                    int y3 = left ? 0 : -320;
                    int y4 = left ? 0 : -220;

                    LerpBezier(new(x1, y1), new(x2, y2), new(0, 0), new(x3, y3), new(x4, y4));
                }
                else if (NPC.ai[2] >= liftUp)
                {
                    NPC.ai[1] = CalamityUtils.ExpInEasing(Utils.GetLerpValue(liftUp, smash, NPC.ai[2], true), 1);
                    DefaultSegments(segSize);
                }
                if (NPC.ai[2] == smash)
                {
                    NPC.netUpdate = true;
                    SoundEngine.PlaySound(SoundID.DD2_MonkStaffGroundImpact with { Pitch = -0.5f }, NPC.Center);
                    SoundEngine.PlaySound(new SoundStyle("CalRemix/Assets/Sounds/PopExplosion") with { Pitch = -0.7f, Volume = 12f }, NPC.Center);
                    Main.LocalPlayer.Calamity().GeneralScreenShakePower += 5;
                    Point pt = (NPC.Center + segments[0]).ToTileCoordinates();
                    int searchAreaX = 15;
                    int searchAreaY = 5;
                    for (int i = pt.X - searchAreaX; i < pt.X + searchAreaX; i++)
                    {
                        for (int j = pt.Y - searchAreaY; j < pt.Y + searchAreaY; j++)
                        {
                            Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
                            Tile above = CalamityUtils.ParanoidTileRetrieval(i, j - 1);
                            if (t.IsTileSolidGround() && (!above.IsTileSolidGround() || !above.HasTile))
                            {
                                WorldGen.KillTile(i, j, true, true);
                            }
                        }
                    }
                    Rectangle rect = Utils.CenteredRectangle(NPC.Center + segments[0], new Vector2(searchAreaX, searchAreaY) * 32);
                    foreach (Player p in Main.ActivePlayers)
                    {
                        if (p.getRect().Intersects(rect))
                        {
                            p.Hurt(PlayerDeathReason.ByNPC(NPC.whoAmI), NPC.damage, 0, dodgeable: false);
                        }
                    }

                }
                if (NPC.ai[2] > wait)
                {
                    NPC.ai[2] = 0;
                    NPC.ai[0] = 1;
                    NPC.netUpdate = true;
                }
            }
            else if (NPC.ai[0] == 3)
            {
                if (NPC.ai[2] == 0)
                {
                    SoundEngine.PlaySound(BetterSoundID.DeathPillarShield, NPC.Center);
                }
                NPC.ai[2]++;
                if (NPC.ai[2] >= 60)
                {
                    NPC.life = NPC.lifeMax;
                    NPC.Transform(ModContent.NPCType<GigamothPupae>());
                }
                DefaultSegments(segSize);
                NPC.ai[1] = 1;
                NPC.netUpdate = true;
            }

            if (NPC.Remix().GreenAI[0]++ > CalamityUtils.SecondsToFrames(30) && NPC.ai[0] > 0 && NPC.ai[0] != 3)
            {
                NPC.ai[0] = 3;
                NPC.ai[2] = 0;
                NPC.netUpdate = true;
            }

            int num858 = 80;
            int num859 = 20;
            Vector2 position4 = new Vector2(NPC.Center.X - (float)(num858 / 2), NPC.position.Y + (float)NPC.height - (float)num859);

            if (Collision.SolidCollision(position4, num858, num859))
            {
                if (NPC.velocity.Y > 0f)
                {
                    NPC.velocity.Y = 0f;
                }
                if ((double)NPC.velocity.Y > -0.2)
                {
                    NPC.velocity.Y -= 0.025f;
                }
                else
                {
                    NPC.velocity.Y -= 0.2f;
                }
                if (NPC.velocity.Y < -4f)
                {
                    NPC.velocity.Y = -4f;
                }
            }
            else
            {
                if (NPC.velocity.Y < 0f)
                {
                    NPC.velocity.Y = 0f;
                }
                if ((double)NPC.velocity.Y < 0.1)
                {
                    NPC.velocity.Y += 0.025f;
                }
                else
                {
                    NPC.velocity.Y += 0.5f;
                }
            }
            if (NPC.velocity.Y > 10f)
            {
                NPC.velocity.Y = 10f;
            }
        }

        public void DefaultSegments(int segSize)
        {
            LerpBezier(new(-2 * segSize, 0), new(-segSize, 0), new(0, 0), new(segSize, 0), new(segSize * 2, 0));
        }

        public void LerpBezier(params Vector2[] values)
        {
            if (NPC.spriteDirection == -1)
                values.Reverse();
            for (int i = 0; i < values.Length; i++)
            {
                segments[i] = Vector2.Lerp(segments[i], values[i], NPC.ai[1]);
            }
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return false;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(NPC.Remix().GreenAI[0]);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            NPC.Remix().GreenAI[0] = reader.ReadSingle();
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D tex = TextureAssets.Npc[Type].Value;
            Texture2D body = ModContent.Request<Texture2D>(Texture + "_Body").Value;
            Texture2D tail = ModContent.Request<Texture2D>(Texture + "_Tail").Value;
            Texture2D glow = ModContent.Request<Texture2D>(Texture + "_Glow").Value;

            BezierCurve cv = new BezierCurve(segments.ToArray());
            List<Vector2> finSeg = cv.GetPoints(11);

            for (int i = finSeg.Count - 1; i >= 0; i--)
            {
                Texture2D finale = i == 0 ? tex : i == (finSeg.Count - 1) ? tail : body;
                float xSpeed = NPC.velocity.X == 0 ? MathF.Cos(Main.GlobalTimeWrappedHourly * 2) * i : 0;
                float ySpeed = NPC.ai[0] == 3 ? 0 : (NPC.velocity.X != 0 ? MathF.Sin(i + Main.GlobalTimeWrappedHourly * 10) : 0);
                float shrinkMult = NPC.ai[0] == 3 ? MathHelper.Lerp(1, 0, NPC.ai[2] / 60f) : 1;

                Vector2 pos = NPC.Center + finSeg[i] * shrinkMult;

                Point p = pos.ToTileCoordinates();

                if (NPC.ai[0] != 2)
                    pos = new Vector2((pos.X + xSpeed), pos.Y + ySpeed * 5);

                Color ce = Lighting.GetColor(pos.ToTileCoordinates());

                float rot = NPC.rotation;
                if (NPC.spriteDirection == 1)
                {
                    if (i > 0)
                    {
                        rot = finSeg[i].DirectionTo(finSeg[i - 1]).ToRotation() + (MathHelper.Pi + (NPC.spriteDirection == -1 ? MathHelper.Pi : 0));
                    }
                }
                else
                {
                    if (i < finSeg.Count - 2)
                    {
                        rot = finSeg[i + 1].DirectionTo(finSeg[i]).ToRotation() + (MathHelper.Pi + (NPC.spriteDirection == -1 ? MathHelper.Pi : 0));
                    }
                }

                spriteBatch.Draw(finale, pos - screenPos, null, NPC.GetAlpha(ce), rot, finale.Size() / 2, NPC.scale, NPC.FlippedEffects(), 0);
                if (i == 0)
                {
                    spriteBatch.Draw(glow, pos - screenPos, null, Color.White, rot, finale.Size() / 2, NPC.scale, NPC.FlippedEffects(), 0);
                }
            }

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
            npcLoot.Add(ModContent.ItemType<LeviathanTeeth>(), 3);
        }
    }
}
