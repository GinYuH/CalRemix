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
using Microsoft.CodeAnalysis.Host.Mef;
using Terraria.DataStructures;
using CalRemix.Content.Projectiles.Hostile;
using CalRemix.Content.Items.Weapons;
using CalRemix.Content.Items.Placeables.Subworlds.GreatSea;

namespace CalRemix.Content.NPCs.Subworlds.GreatSea
{
    public class Liopleurodon : ModNPC
    {

        public static SoundStyle HitSound = new SoundStyle("CalRemix/Assets/Sounds/LioHit");
        public static SoundStyle DeathSound = new SoundStyle("CalRemix/Assets/Sounds/LioDeath");
        public static SoundStyle PassiveSound = new SoundStyle("CalRemix/Assets/Sounds/LioPassive", 2) with { MaxInstances = 0 };
        public static SoundStyle AngrySound = new SoundStyle("CalRemix/Assets/Sounds/LioAggro", 3) with { MaxInstances = 0 };
        public ref float Timer => ref NPC.ai[0];
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 4;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 180;
            NPC.width = 160;
            NPC.height = 100;
            NPC.defense = 56;
            NPC.lifeMax = 50000;
            NPC.value = 0;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = false;
            NPC.knockBackResist = 0;
            NPC.HitSound = HitSound;
            NPC.DeathSound = DeathSound;
            NPC.GravityIgnoresLiquid = true;
            NPC.waterMovementSpeed = 1f;
            NPC.rarity = 3;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<PrimordialCavesBiome>().Type };
        }

        public override void AI()
        {
            if (!NPC.wet)
            {
                NPC.velocity.Y = 12;
                return;
            }
            if (NPC.ai[1] == 0)
            {
                NPC.TargetClosest(false);
                if (Timer % 210 == 0 || NPC.collideX || NPC.collideY)
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
                NPC.rotation = Utils.AngleLerp(NPC.rotation, NPC.velocity.ToRotation() - (NPC.direction == 1 ? 0 : MathHelper.Pi), 0.1f);
                NPC.spriteDirection = NPC.direction = NPC.velocity.X.DirectionalSign();
                if (NPC.life < NPC.lifeMax)
                {
                    NPC.ai[1] = 1;
                    Timer = 0;
                }
                if (Main.rand.NextBool(300) && NPC.localAI[0] <= 0)
                {
                    SoundEngine.PlaySound(PassiveSound, NPC.Center);
                    NPC.localAI[0] = 100;
                }
            }
            else if (NPC.ai[1] == 1)
            {
                Timer++;
                if (Main.rand.NextBool(300) && NPC.localAI[0] <= 0)
                {
                    SoundEngine.PlaySound(AngrySound, NPC.Center);
                    NPC.localAI[0] = 100;
                }
                Vector2 projection = NPC.Center + NPC.velocity.SafeNormalize(Vector2.UnitY).RotatedBy(-MathHelper.PiOver4 * NPC.direction * 0.5f) * (NPC.direction == -1 ? 240 : 190);
                Rectangle rect = new Rectangle((int)projection.X, (int)projection.Y, NPC.direction == -1 ? 100 : 80, 60);
                if (Main.player[NPC.target].getRect().Intersects(rect) && NPC.localAI[1] <= 0)
                {
                    SoundEngine.PlaySound(AngrySound, NPC.Center);
                    NPC.localAI[1] = 300;
                }
                if (NPC.localAI[1] > 200)
                {
                    Main.player[NPC.target].Center = rect.Bottom();
                    if (Timer % 20 == 0 && !Main.player[NPC.target].dead)
                    {
                        SoundEngine.PlaySound(BetterSoundID.ItemMinecartCling with { Pitch = 0.4f, Volume = 2 });
                        Main.player[NPC.target].RemoveAllIFrames();
                        Main.player[NPC.target].Hurt(PlayerDeathReason.ByNPC(NPC.whoAmI), NPC.damage, NPC.direction);

                        for (int i = 0; i < 40; i++)
                        {
                            int d = Dust.NewDust(Main.player[NPC.target].Center, 1, 1, DustID.Blood, Scale: Main.rand.NextFloat(1f, 3f));
                            Dust de = Main.dust[d];
                            de.position += Main.rand.NextVector2Circular(-15, 15);
                            de.velocity = Main.player[NPC.target].Center.DirectionTo(de.position) * Main.rand.Next(4, 10);
                        }
                    }
                }
                //int d = Dust.NewDust(rect.TopLeft(), rect.Width, rect.Height, DustID.BlueCrystalShard);
                //Main.dust[d].noGravity = true;
                if (NPC.HasPlayerTarget)
                {
                    Player target = Main.player[NPC.target];
                    float speed = 0.55f;
                    int xBuffer = 60;
                    int yBuffer = 60;
                    int maxX = 6;
                    int maxY = 4;
                    if (NPC.position.X > target.position.X + xBuffer)
                    {
                        NPC.velocity.X -= speed;
                        if (NPC.velocity.X <= -maxX)
                        {
                            NPC.velocity.X = -maxX;
                        }
                    }
                    else if (NPC.position.X < target.position.X - xBuffer)
                    {
                        NPC.velocity.X += speed;
                        if (NPC.velocity.X > maxX)
                        {
                            NPC.velocity.X = maxX;
                        }
                    }
                    else
                    {
                        NPC.velocity.X *= 0.998f;
                    }
                    if (NPC.position.Y > target.position.Y + yBuffer)
                    {
                        NPC.velocity.Y -= speed;
                        if (NPC.velocity.Y <= -maxY)
                        {
                            NPC.velocity.Y = -maxY;
                        }
                    }
                    else if (NPC.position.Y < target.position.Y - yBuffer)
                    {
                        NPC.velocity.Y += speed;
                        if (NPC.velocity.Y > maxY)
                        {
                            NPC.velocity.Y = maxY;
                        }
                    }
                    else
                    {
                        NPC.velocity.Y *= 0.998f;
                    }
                }
                else
                {
                    NPC.velocity.Y -= 1;
                    if (NPC.velocity.Y <= -30)
                    {
                        NPC.velocity.Y = -30;
                    }
                }
                if (NPC.velocity.Length() <= 1)
                {
                    if (NPC.velocity == Vector2.Zero)
                    {
                        NPC.velocity = Vector2.One;
                    }
                    NPC.velocity *= 4f;
                }

                NPC.rotation = NPC.velocity.ToRotation() + (NPC.direction == -1 ? MathHelper.Pi : 0);
                if (NPC.velocity.X > 0)
                {
                    NPC.direction = 1;
                }
                else
                {
                    NPC.direction = -1;
                }
                NPC.spriteDirection = NPC.direction;

                if (Timer > 240 && NPC.localAI[1] < 200)
                {
                    Timer = 0;
                    NPC.ai[1] = 2;
                    NPC.localAI[0] = 0;
                    NPC.localAI[1] = 0;
                    NPC.direction = NPC.spriteDirection = NPC.DirectionTo(Main.player[NPC.target].Center).X.DirectionalSign();
                }
            }
            else if (NPC.ai[1] == 2)
            {
                NPC.velocity *= 0.97f;
                int salvoAmount = 5;
                int salvoDelay = 5;
                Timer++;
                if (Timer < 50)
                {
                    float idealRot = (NPC.direction == 1 ? MathHelper.PiOver4 : 3 * MathHelper.PiOver4) + (NPC.direction == -1 ? MathHelper.Pi : 0);
                    NPC.rotation = Utils.AngleLerp(NPC.rotation, idealRot, 0.05f);
                }
                if (Timer == 50)
                {
                    NPC.localAI[0] = 100;
                }
                if (Timer > 50 && Timer < 50 + (salvoAmount * salvoDelay) && Timer % salvoDelay == 0)
                {
                    SoundEngine.PlaySound(BetterSoundID.ItemBubbleGun2); 
                    Vector2 projection = NPC.Center + (NPC.rotation + (NPC.direction == -1 ? MathHelper.ToRadians(190) : 0)).ToRotationVector2() * (NPC.direction == -1 ? 240 : 190);
                    Rectangle rect = new Rectangle((int)projection.X, (int)projection.Y, NPC.direction == -1 ? 100 : 80, 60);
                    for (int i = 0; i < Main.rand.Next(6, 12); i++)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), rect.Center.ToVector2(), new Vector2(Main.rand.NextFloat(1, 20) * NPC.direction, Main.rand.NextFloat(2, 10)), ModContent.ProjectileType<LioBubble>(), CalRemixHelper.ProjectileDamage(200, 280), 1f, ai0: NPC.target);
                        }
                    }
                }
                if (Timer > 50 + (salvoAmount * salvoDelay) + 120)
                {
                    Timer = 0;
                    NPC.ai[1] = 1;
                }
            }
            NPC.localAI[0]--;
            NPC.localAI[1]--;
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (NPC.frameCounter > 8)
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
            Texture2D head = ModContent.Request<Texture2D>(Texture + "Jaw").Value;
            SpriteEffects fx = NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(tex, NPC.Center - screenPos, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, new Vector2(tex.Width / 2, tex.Height / Main.npcFrameCount[Type] / 2), NPC.scale, fx, 0);
            float baseRot = NPC.ai[1] == 1 ? MathHelper.ToRadians(15) : 0;
            float jawRot = baseRot;
            float roarLength = 100;
            float jawOpen = roarLength - 5;
            float jawPause = jawOpen - 10;
            float jawClose = jawPause - 44;
            float finish = jawClose - 15;
            if (NPC.localAI[1] <= 200)
            {
                if (NPC.localAI[0] > jawPause)
                {
                    jawRot = MathHelper.Lerp(baseRot, MathHelper.PiOver4, CalamityUtils.SineInEasing(Utils.GetLerpValue(jawOpen, jawPause, NPC.localAI[0], true), 1));
                }
                else if (NPC.localAI[0] <= jawPause && NPC.localAI[0] >= jawClose)
                {
                    jawRot = MathHelper.PiOver4 + MathF.Sin(Main.GlobalTimeWrappedHourly * 22) * 0.1f;
                }
                else if (NPC.localAI[0] < jawClose)
                {
                    jawRot = MathHelper.Lerp(MathHelper.PiOver4, baseRot, CalamityUtils.SineOutEasing(Utils.GetLerpValue(jawClose, finish, NPC.localAI[0], true), 1));
                }
            }
            else
            {
                jawRot = MathF.Sin(Timer * 0.3f) * 0.5f + 0.5f;
            }
            spriteBatch.Draw(head, NPC.Center - screenPos + NPC.scale * new Vector2(98 * NPC.spriteDirection, -30).RotatedBy(NPC.rotation), null, NPC.GetAlpha(drawColor), NPC.rotation + NPC.spriteDirection * jawRot, new Vector2(NPC.spriteDirection == 1 ? head.Width - 124 : 124, 29), NPC.scale, fx, 0);
            return false;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
        new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<Pliobrine>(), 2);
            npcLoot.Add(ModContent.ItemType<Chert>(), 1, 100, 255);
            npcLoot.Add(ModContent.ItemType<Darkstone>(), 1, 100, 255);
            npcLoot.Add(ModContent.ItemType<Schist>(), 1, 100, 255);
        }
    }
}
