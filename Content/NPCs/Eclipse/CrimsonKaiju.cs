using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using Terraria.ModLoader.Utilities;
using Microsoft.Xna.Framework;
using CalamityMod.Items.Materials;
using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using ReLogic.Content;
using Terraria.DataStructures;
using CalRemix.Content.Projectiles.Hostile;
using Terraria.ModLoader.Core;
using Terraria.Audio;
using CalamityMod.Tiles.Astral;
using CalRemix.Content.Items.Accessories;
using CalRemix.Content.Items.Weapons;
using CalamityMod.World;
using CalRemix.Content.Items.Placeables.Relics;
using CalRemix.Content.Items.Placeables.Trophies;

namespace CalRemix.Content.NPCs.Eclipse
{
    public class CrimsonKaiju : ModNPC
    {
        public int wingsFrameCounter = 0;
        public int legsFrameCounter = 0;
        public int wingsFrame = 0;
        public int legsFrame = 0;

        public ref float Phase => ref NPC.ai[0];
        public ref float Timer => ref NPC.ai[1];
        public ref float AIMisc => ref NPC.ai[2];

        public static SoundStyle RedHit = new SoundStyle("CalRemix/Assets/Sounds/RedHit");
        public static SoundStyle RedDead = new SoundStyle("CalRemix/Assets/Sounds/RedDead");
        public static SoundStyle RedSpawn = new SoundStyle("CalRemix/Assets/Sounds/RedSpawn");

        public enum Attacks
        {
            SpawnAnimation = 0,
            Normal = 1,
            Grab = 2,
            Fly = 3,
            Stepback = 4,
            Sniper = 5,
            Teleport = 6,
            Despawning = 7
        }

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Crimson Kaiju");
            NPCID.Sets.MustAlwaysDraw[Type] = true;
            Main.npcFrameCount[NPC.type] = 6;
            // wings - 6
            // legs - 7
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 180;
            NPC.height = 200;
            NPC.lifeMax = 120000;
            NPC.damage = 90;
            NPC.defense = 35;
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(gold: 30, silver: 50);
            NPC.HitSound = null;
            NPC.DeathSound = RedDead;
            NPC.alpha = 255;
            NPC.dontTakeDamage = true;
        }

        public override void AI()
        {
            NPC.TargetClosest();
            if (!NPC.HasPlayerTarget)
            {
                Phase = (int)Attacks.Despawning;
            }
            else if (Phase != (int)Attacks.SpawnAnimation && Phase != (int)Attacks.Despawning)
            {
                NPC.alpha = 0;
            }
            switch (Phase)
            {
                case (int)Attacks.SpawnAnimation:
                    {
                        NPC.damage = 0;
                        if (Timer < 121 && Timer % 60 == 0)
                        {
                            SoundEngine.PlaySound(BetterSoundID.ItemMinecartCling with { Pitch = -1f, Volume = 2 });
                        }
                        Timer++;
                        if (Timer > 210)
                        {
                            SoundEngine.PlaySound(RedSpawn);
                            Timer = 0;
                            Phase = (int)Attacks.Normal;
                            NPC.dontTakeDamage = false;
                        }
                    }
                    break;
                case (int)Attacks.Normal:
                    {
                        Timer++;
                        float speed = Math.Max(10, Main.player[NPC.target].velocity.X * 0.9f);
                        NPC.noGravity = true;
                        NPC.noTileCollide = true;
                        bool closeToPlayer = false;
                        if (Math.Abs(NPC.Center.X - Main.player[NPC.target].Center.X) < 50f)
                        {
                            closeToPlayer = true;
                        }
                        if (closeToPlayer)
                        {
                            NPC.velocity.X *= 0.9f;
                            if (NPC.velocity.X > -0.1 && NPC.velocity.X < 0.1)
                            {
                                NPC.velocity.X = 0f;
                            }
                        }
                        else
                        {
                            if (NPC.direction > 0)
                            {
                                NPC.velocity.X = (NPC.velocity.X * 20f + speed) / 21f;
                            }
                            if (NPC.direction < 0)
                            {
                                NPC.velocity.X = (NPC.velocity.X * 20f - speed) / 21f;
                            }
                        }
                        TileClipping();
                        if (Main.player[NPC.target].position.Y < NPC.Center.Y - 300)
                        {
                            Timer = 0;
                            Phase = (int)Attacks.Fly;
                        }
                        if (Timer > 600 + Main.rand.NextFloat(-100, 100))
                        {
                            Timer = 0;
                            Phase = (int)Attacks.Stepback;
                        }
                        if (NPC.getRect().Distance(Main.player[NPC.target].Center) < 100)
                        {
                            Timer = 0;
                            Phase = (int)Attacks.Grab;
                        }
                    }
                    break;
                case (int)Attacks.Grab:
                    {
                        NPC.dontTakeDamage = true;
                        Timer++;
                        Vector2 handPos = NPC.Center + new Vector2(30 * NPC.direction, 0);
                        TileClipping();
                        if ((Timer > 60 && AIMisc != 1)  || AIMisc == 1)
                        {
                            AIMisc = 1;
                            handPos = NPC.Center + new Vector2(30 * NPC.direction, -64);
                        }
                        Main.player[NPC.target].position = Vector2.Lerp(Main.player[NPC.target].position, handPos, 0.2f);
                        if (NPC.frame.Y > 680)
                        {
                            Main.player[NPC.target].KillMe(PlayerDeathReason.ByNPC(NPC.whoAmI), 999999, 1);
                            Timer = 0;
                            AIMisc = 0;
                            Phase = (int)Attacks.Despawning;
                        }
                        NPC.velocity.X *= 0.9f;
                    }
                    break;
                case (int)Attacks.Fly:
                    {
                        Timer++;
                        NPC.noGravity = true;
                        int target = NPC.target;
                        int direction = NPC.direction;
                        if (NPC.netUpdate && target == NPC.target && direction == NPC.direction)
                        {
                            NPC.netUpdate = false;
                        }
                        float xSpeed = 0.5f;
                        float ySpeed = 0.5f;
                        float xSpeedMax = 12f;
                        float ySpeedMax = 12f;
                        float maxHorizontalDistance = 100f;
                        float maxDistance = 100f;
                        float horizontalDistance = Math.Abs(NPC.position.X + (float)(NPC.width / 2) - (Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2)));
                        float verticalDistance = Main.player[NPC.target].position.Y - (float)(NPC.height / 2);
                        if (horizontalDistance > maxHorizontalDistance)
                        {
                            if (NPC.direction == -1 && NPC.velocity.X > 0f - xSpeedMax)
                            {
                                NPC.velocity.X -= xSpeed;
                                if (NPC.velocity.X > xSpeedMax)
                                {
                                    NPC.velocity.X -= xSpeed;
                                }
                                else if (NPC.velocity.X > 0f)
                                {
                                    NPC.velocity.X -= xSpeed / 2f;
                                }
                                if (NPC.velocity.X < 0f - xSpeedMax)
                                {
                                    NPC.velocity.X = 0f - xSpeedMax;
                                }
                            }
                            else if (NPC.direction == 1 && NPC.velocity.X < xSpeedMax)
                            {
                                NPC.velocity.X += xSpeed;
                                if (NPC.velocity.X < 0f - xSpeedMax)
                                {
                                    NPC.velocity.X += xSpeed;
                                }
                                else if (NPC.velocity.X < 0f)
                                {
                                    NPC.velocity.X += xSpeed / 2f;
                                }
                                if (NPC.velocity.X > xSpeedMax)
                                {
                                    NPC.velocity.X = xSpeedMax;
                                }
                            }
                        }
                        if (horizontalDistance > maxDistance)
                        {
                            verticalDistance -= maxDistance / 2f;
                        }
                        if (NPC.position.Y < verticalDistance)
                        {
                            NPC.velocity.Y += ySpeed;
                            if (NPC.velocity.Y < 0f)
                            {
                                NPC.velocity.Y += ySpeed;
                            }
                        }
                        else
                        {
                            NPC.velocity.Y -= ySpeed;
                            if (NPC.velocity.Y > 0f)
                            {
                                NPC.velocity.Y -= ySpeed;
                            }
                        }
                        if (NPC.velocity.Y < 0f - ySpeedMax)
                        {
                            NPC.velocity.Y = 0f - ySpeedMax;
                        }
                        if (NPC.velocity.Y > ySpeedMax)
                        {
                            NPC.velocity.Y = ySpeedMax;
                        }
                        if (Main.player[NPC.target].position.Y > NPC.Center.Y && Main.tile[(int)(NPC.Bottom.X / 16), (int)(NPC.Bottom.Y / 16) + 1].IsTileSolid())
                        {
                            Timer = 0;
                            Phase = (int)Attacks.Normal;
                        }
                        if (Timer > 600 + Main.rand.NextFloat(-100, 100))
                        {
                            Timer = 0;
                            Phase = (int)Attacks.Stepback;
                        }
                        if (NPC.getRect().Distance(Main.player[NPC.target].Center) < 100)
                        {
                            Timer = 0;
                            Phase = (int)Attacks.Grab;
                        }
                    }
                    break;
                case (int)Attacks.Stepback:
                    {
                        float speed = 8f;
                        NPC.noGravity = true;
                        NPC.noTileCollide = true;
                        bool closeToPlayer = false;
                        if (Math.Abs(NPC.Center.X - Main.player[NPC.target].Center.X) < 50f)
                        {
                            closeToPlayer = true;
                        }
                        if (closeToPlayer)
                        {
                            NPC.velocity.X *= 0.9f;
                            if (NPC.velocity.X > -0.1 && NPC.velocity.X < 0.1)
                            {
                                NPC.velocity.X = 0f;
                            }
                        }
                        else
                        {
                            if (NPC.direction > 0)
                            {
                                NPC.velocity.X = (NPC.velocity.X * 20f - speed) / 21f;
                            }
                            if (NPC.direction < 0)
                            {
                                NPC.velocity.X = (NPC.velocity.X * 20f + speed) / 21f;
                            }
                        }
                        TileClipping();
                        if (NPC.getRect().Distance(Main.player[NPC.target].Center) < 100)
                        {
                            Phase = (int)Attacks.Grab;
                        }
                        Timer++;
                        if (Timer > 360 || Math.Abs(Main.player[NPC.target].position.X - NPC.position.X) > Main.screenWidth * 0.75f)
                        {
                            Timer = 0;
                            bool roll = Main.rand.NextBool();
                            Phase = roll ? (int)Attacks.Sniper : (int)Attacks.Teleport;
                        }
                    }
                    break;
                case (int)Attacks.Sniper:
                    {
                        NPC.dontTakeDamage = true;
                        Timer++;
                        NPC.velocity.X *= 0.9f;
                        float distMultiplier = 1f; // how far the hook shoots out relative to the player
                        float hookDist = Main.player[NPC.target].Distance(NPC.Center) * distMultiplier; // the final distance the hook shoots
                        if (Main.player[NPC.target].Distance(NPC.Center) < 560 && AIMisc == 0)
                        {
                            AIMisc = 1;
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                int p = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, NPC.DirectionTo(Main.player[NPC.target].Center), ModContent.ProjectileType<CrimsonKaijuHand>(), 0, 0, Main.player[NPC.target].whoAmI, NPC.whoAmI, Main.player[NPC.target].whoAmI);
                                Projectile proj = Main.projectile[p];
                                proj.localAI[2] = hookDist;
                            }
                        }
                        TileClipping();
                        if (Timer > 180 && !CalamityUtils.AnyProjectiles(ModContent.ProjectileType<CrimsonKaijuHand>()))
                        {
                            Timer = 0;
                            Phase = (int)Attacks.Normal;
                            NPC.dontTakeDamage = false;
                        }
                    }
                    break;
                case (int)Attacks.Teleport:
                    {
                        NPC.dontTakeDamage = true;
                        if (Timer == 0)
                        {
                            float dist = NPC.Center.X - Main.player[NPC.target].Center.X;
                            dist *= -1;
                            NPC.Center = NPC.Center + Vector2.UnitX * dist * 2;
                        }
                        Timer++;
                        NPC.velocity.X *= 0.9f;
                        if (Main.player[NPC.target].Distance(NPC.Center) < 560)
                        {
                            float distMultiplier = 1f; // how far the hook shoots out relative to the player
                            float hookDist = Main.player[NPC.target].Distance(NPC.Center) * distMultiplier; // the final distance the hook shoots
                            if (Main.player[NPC.target].Distance(NPC.Center) < 560 && AIMisc == 0)
                            {
                                AIMisc = 1;
                                if (Main.netMode != NetmodeID.MultiplayerClient)
                                {
                                    int p = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, NPC.DirectionTo(Main.player[NPC.target].Center), ModContent.ProjectileType<CrimsonKaijuHand>(), 0, 0, Main.player[NPC.target].whoAmI, NPC.whoAmI, Main.player[NPC.target].whoAmI);
                                    Projectile proj = Main.projectile[p];
                                    proj.localAI[2] = hookDist;
                                }
                            }
                        }
                        TileClipping();
                        if (Timer > 180 && !CalamityUtils.AnyProjectiles(ModContent.ProjectileType<CrimsonKaijuHand>()))
                        {
                            Timer = 0;
                            Phase = (int)Attacks.Normal;
                            NPC.dontTakeDamage = false;
                        }

                    }
                    break;
                case (int)Attacks.Despawning:
                    {
                        NPC.velocity *= 0.9f;
                        NPC.alpha += 5;
                        if (NPC.alpha >= 255)
                        {
                            NPC.active = false;
                        }
                    }
                    break;
            }
            NPC.spriteDirection = NPC.direction;
        }

        public void TileClipping()
        {
            int npcToWidth = 80;
            int npcToHeight = 20;
            Vector2 npcCenter = new Vector2(NPC.Center.X - npcToWidth / 2, NPC.position.Y + NPC.height - npcToHeight);
            bool below = false;
            if (NPC.position.X < Main.player[NPC.target].position.X && NPC.position.X + NPC.width > Main.player[NPC.target].position.X + Main.player[NPC.target].width && NPC.position.Y + NPC.height < Main.player[NPC.target].position.Y + Main.player[NPC.target].height - 16f)
            {
                below = true;
            }
            if (below)
            {
                NPC.velocity.Y += 0.5f;
            }
            else if (Collision.SolidCollision(npcCenter, npcToWidth, npcToHeight))
            {
                if (NPC.velocity.Y > 0f)
                {
                    NPC.velocity.Y = 0f;
                }
                if (NPC.velocity.Y > -0.2)
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
                if (NPC.velocity.Y < 0.1)
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

        public override bool CheckActive()
        {
            return !NPC.HasPlayerTarget;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Eclipse,
        new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }

        public override void FindFrame(int frameHeight)
        {
            if (Phase == (int)Attacks.Grab)
            {
                NPC.frameCounter += 1.0;

                if (NPC.frameCounter > 6.0)
                {
                    NPC.frameCounter = 0.0;
                    NPC.frame.Y += frameHeight;
                }
                if (AIMisc == 0)
                {
                    if (NPC.frame.Y > frameHeight * 3)
                    {
                        NPC.frame.Y = 0;
                    }
                }
                else
                {
                    if (NPC.frame.Y > frameHeight * 5)
                    {
                        NPC.frame.Y = frameHeight * 5;
                    }
                }
            }

            if (CalamityUtils.AnyProjectiles(ModContent.ProjectileType<CrimsonKaijuHand>()))
            {
                NPC.frame.Y = frameHeight * 5;
            }    

            if (Phase == (int)Attacks.Fly)
            {
                wingsFrameCounter += 1;
                if (wingsFrameCounter > 6.0)
                {
                    wingsFrameCounter = 0;
                    wingsFrame += 1;
                }
                if (wingsFrame > 5)
                {
                    wingsFrame = 0;
                }
            }
            if (NPC.ai[0] != (int)Attacks.Fly)
            {
                if (NPC.velocity.X != 0)
                {
                    legsFrameCounter += 1;
                    if (legsFrameCounter > 6.0)
                    {
                        legsFrameCounter = 0;
                        legsFrame += 1;
                    }
                    if (legsFrame > 6)
                    {
                        legsFrame = 0;
                    }
                }
                else
                {
                    legsFrame = 0;
                }
            }
            NPC.soundDelay--;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.soundDelay <= 0)
            {
                SoundEngine.PlaySound(RedHit, NPC.Center);
                NPC.soundDelay = 10;
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!DownedBossSystem.downedDoG || NPC.AnyNPCs(ModContent.NPCType<CrimsonKaiju>()))
                return 0f;

            return SpawnCondition.SolarEclipse.Chance * 0.01f;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<DarksunFragment>(), 1, 35, 45);
            npcLoot.AddIf(() => Phase != (int)Attacks.Fly, ModContent.ItemType<RedWings>(), 20);
            npcLoot.AddIf(() => Phase == (int)Attacks.Fly, ModContent.ItemType<RedWings>());
            npcLoot.Add(ModContent.ItemType<GodKillerEX>());
            npcLoot.Add(ModContent.ItemType<RedTrophy>(), 10);
            npcLoot.AddIf(DropHelper.RevAndMaster, ModContent.ItemType<RedRelic>());
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            bool spawnAnimation = Phase == (int)Attacks.SpawnAnimation;
            if (Phase != (int)Attacks.Despawning)
            {
                float textScale = 4f;
                int spacing = 16;
                Asset<Texture2D> R = ModContent.Request<Texture2D>(Texture + "Text_R");
                Asset<Texture2D> U = ModContent.Request<Texture2D>(Texture + "Text_U");
                Asset<Texture2D> N = ModContent.Request<Texture2D>(Texture + "Text_N");
                float uHalfWidth = U.Width() / 2 * textScale;
                Color letterColor = spawnAnimation ? Color.White : Color.Red;
                spriteBatch.Draw(R.Value, new Vector2(Main.screenWidth / 2 - uHalfWidth - spacing - R.Width() * textScale, Main.screenHeight / 10) + new Vector2(Main.rand.Next(-4, 4), Main.rand.Next(-4, 4)), null, letterColor, 0f, Vector2.Zero, textScale, SpriteEffects.None, 1f);
                if ((Timer > 60 && spawnAnimation) || !spawnAnimation)
                    spriteBatch.Draw(U.Value, new Vector2(Main.screenWidth / 2 - uHalfWidth, Main.screenHeight / 10) + new Vector2(Main.rand.Next(-4, 4), Main.rand.Next(-4, 4)), null, letterColor, 0f, Vector2.Zero, textScale, SpriteEffects.None, 1f);
                if ((Timer > 120 && spawnAnimation) || !spawnAnimation)
                    spriteBatch.Draw(N.Value, new Vector2(Main.screenWidth / 2 + uHalfWidth + spacing, Main.screenHeight / 10) + new Vector2(Main.rand.Next(-4, 4), Main.rand.Next(-4, 4)), null, letterColor, 0f, Vector2.Zero, textScale, SpriteEffects.None, 1f);
            }
            Asset<Texture2D> sprite = TextureAssets.Npc[Type];
            Asset<Texture2D> wings = ModContent.Request<Texture2D>(Texture + "Wings");
            Asset<Texture2D> legs = ModContent.Request<Texture2D>(Texture + "Legs");

            Vector2 origin = new Vector2(sprite.Value.Width / 2, sprite.Value.Height / Main.npcFrameCount[NPC.type] / 2);
            Vector2 wingsorigin = new Vector2(wings.Value.Width / 2, wings.Value.Height / 6 / 2);
            Vector2 legsorigin = new Vector2(legs.Value.Width / 2, legs.Value.Height / 7 / 2);

            Rectangle wingsFrameRec = wings.Frame(1, 6, 0, wingsFrame);
            Rectangle legsFrameRec = legs.Frame(1, 7, 0, legsFrame);

            Vector2 wingsOffset = new Vector2(-100 * NPC.spriteDirection, -30);
            Vector2 legsOffset = new Vector2(0, 60);

            Vector2 npcOffset = NPC.Center - screenPos;

            SpriteEffects fx = NPC.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            spriteBatch.Draw(legs.Value, npcOffset + legsOffset, legsFrameRec, NPC.GetAlpha(drawColor), 0f, legsorigin, 1f, fx, 1f);
            spriteBatch.Draw(sprite.Value, npcOffset, NPC.frame, NPC.GetAlpha(drawColor), 0f, origin, 1f, fx, 1f);

            if (Phase == (int)Attacks.Fly)
            {
                spriteBatch.Draw(wings.Value, npcOffset + wingsOffset, wingsFrameRec, NPC.GetAlpha(drawColor), 0f, wingsorigin, 1f, fx, 1f);
            }
            return false;
        }
    }
}