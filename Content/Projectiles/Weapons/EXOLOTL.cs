using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Content.Buffs;
using CalamityMod;
using CalamityMod.Projectiles.Summon;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.Audio;
using CalamityMod.Projectiles.DraedonsArsenal;
using CalamityMod.Items.Weapons.DraedonsArsenal;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class EXOLOTL : ModProjectile
    {
        public ref float AIState => ref Projectile.ai[0];
        public ref float MissTimer => ref Projectile.ai[1];
        public Player Owner => Main.player[Projectile.owner];
        public int Jump;
        public int Gun;
        public int Laser;
        public bool Prepare;
        public bool Flying;
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 2;
            ProjectileID.Sets.MinionSacrificable[Type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Type] = true;
            ProjectileID.Sets.TrailingMode[Type] = 2;
            ProjectileID.Sets.TrailCacheLength[Type] = 4;
        }
        public override void SetDefaults()
        {
            Projectile.width = 62;
            Projectile.height = 62;
            Projectile.netImportant = true;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.minionSlots = 3f;
            Projectile.timeLeft = 18000;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.timeLeft *= 5;
            Projectile.tileCollide = true;
            Projectile.minion = true;
            Projectile.DamageType = DamageClass.Summon;
        }
        public override bool? CanHitNPC(NPC target) => false;
        public override void AI()
        {
            Projectile.CheckMinionCondition(ModContent.BuffType<EXOLOTLBuff>(), Owner.Remix().exolotl);
            NPC target = Projectile.Center.MinionHoming(1425f, Owner, true);
            if (target is null)
                AIState = 1;
            else if (AIState != 3)
            {
                AIState = 2;
            }
            switch (AIState)
            {
                case 1:
                    Gravity();
                    Projectile.frame = 0;
                    if (Math.Abs(Owner.position.X - Projectile.position.X) > 300 || (Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height) && Math.Abs(Owner.position.X - Projectile.position.X) > 120))
                    {
                        Flying = true;
                        Projectile.tileCollide = false;
                        Projectile.direction = Projectile.velocity.X >= 0 ? 1 : -1;
                        Projectile.rotation = ((Projectile.velocity.X >= 0) ? 0 : MathHelper.Pi) + Projectile.velocity.ToRotation();
                        Projectile.velocity = (Owner.Center - Projectile.Center) / 56;
                        Projectile.frame = 1;
                        Gun++;
                        if (Gun >= 5)
                        {
                            Vector2 velocity = Vector2.Normalize(Projectile.velocity) * -10f;
                            SoundEngine.PlaySound(CalamityMod.Sounds.CommonCalamitySounds.LaserCannonSound with { Volume = CalamityMod.Sounds.CommonCalamitySounds.LaserCannonSound.Volume - 0.1f }, Projectile.Center);
                            Projectile p = Main.projectile[Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + (Vector2.UnitX * ((Projectile.velocity.X >= 0) ? -1 : 1) * Projectile.width).RotatedBy(Projectile.rotation), velocity, ModContent.ProjectileType<CalamityMod.Projectiles.Summon.AstrageldonLaser>(), Projectile.damage, Projectile.knockBack)];
                            p.tileCollide = false;
                            if (p.whoAmI.WithinBounds(Main.maxProjectiles))
                            {
                                p.originalDamage = Projectile.damage;
                            }
                            Gun = 0;
                        }
                    }
                    else
                    {
                        Flying = false;
                        Projectile.direction = Owner.position.X < Projectile.position.X ? -1 : 1;
                        Projectile.velocity.X = 0;
                        Projectile.rotation = 0;
                        if (Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
                        {
                            Projectile.tileCollide = true;
                            Jump++;
                            if (Jump >= Main.rand.NextFloat(90f, 120f))
                            {
                                Projectile.tileCollide = false;
                                Projectile.velocity = Vector2.UnitY * Main.rand.NextFloat(-6f, -8f);
                                Jump = 0;
                            }
                        }
                        else
                        {
                            Jump = 0;
                            Projectile.tileCollide = true;
                            if (Main.rand.NextBool(10000))
                                SoundEngine.PlaySound(WulfrumDroid.RandomChirpSound with { PitchRange = (0.75f, 1f) }, Projectile.Center);

                        }
                    }
                    break;
                case 2:
                    Flying = false;
                    Projectile.tileCollide = true;
                    Projectile.rotation = 0;
                    Projectile.frame = 0;
                    Projectile.direction = target.position.X < Projectile.position.X ? -1 : 1;
                    Gun++;
                    Jump++;
                    bool targetClose = target.Center.Distance(Projectile.Center) < 480 && !Collision.CanHit(target.Center, 1, 1, Projectile.TopLeft, 1, 1) && !Collision.CanHit(target.Center, 1, 1, Projectile.TopRight, 1, 1);
                    if (Gun >= (targetClose ? 2 : 5))
                    {
                        Vector2 center = (Projectile.direction < 0) ? Projectile.TopLeft : Projectile.TopRight;
                        int type = Main.rand.Next(new int[] { ModContent.ProjectileType<AtlasMunitionsLaser>(), ModContent.ProjectileType<AtlasMunitionsLaserOverdrive>(), });
                        SoundEngine.PlaySound(CalamityMod.Sounds.CommonCalamitySounds.LaserCannonSound with { Volume = CalamityMod.Sounds.CommonCalamitySounds.LaserCannonSound.Volume - 0.1f }, Projectile.Center);
                        int p = Projectile.NewProjectile(Projectile.GetSource_FromThis(), center, center.DirectionTo(target.Center) * 10, type, Projectile.damage, Projectile.knockBack, Owner.whoAmI, target.whoAmI, 0f);
                        if (p.WithinBounds(Main.maxProjectiles))
                        {
                            Main.projectile[p].DamageType = DamageClass.Summon;
                            Main.projectile[p].originalDamage = Projectile.damage;
                            Main.projectile[p].tileCollide = !targetClose;
                        }
                        Gun = 0;
                    }
                    if (Jump >= Main.rand.NextFloat(120f, 300f))
                    {
                        Projectile.velocity = new Vector2((target.position.X < Projectile.position.X ? Main.rand.NextFloat(-6f, -2f) : Main.rand.NextFloat(2f, 6f)), Main.rand.NextFloat(-4f, -8f));
                        Jump = 0;
                        if (Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
                            Projectile.tileCollide = false;
                        if (!Prepare)
                            Prepare = true;
                    }
                    if (Prepare)
                    {
                        if (Laser >= 12)
                        {
                            if (Main.rand.NextBool(2))
                            {
                                Vector2 center = (Projectile.direction < 0) ? Projectile.TopLeft : Projectile.TopRight;
                                int type = Main.rand.Next(new int[] { ModContent.ProjectileType<LaserRifleShot>(), ModContent.ProjectileType<PulsePistolShot>(), });
                                SoundEngine.PlaySound(PulseRifle.FireSound with { Volume = CalamityMod.Sounds.CommonCalamitySounds.LaserCannonSound.Volume - 0.1f }, Projectile.Center);
                                int p = Projectile.NewProjectile(Projectile.GetSource_FromThis(), center, center.DirectionTo(target.Center) * 5f, type, Projectile.damage * 3, Projectile.knockBack, Owner.whoAmI, target.whoAmI, 0f);
                                if (p.WithinBounds(Main.maxProjectiles))
                                {
                                    Main.projectile[p].DamageType = DamageClass.Summon;
                                    Main.projectile[p].originalDamage = Projectile.damage;
                                    Main.projectile[p].tileCollide = !targetClose;
                                }
                            }
                            Prepare = false;
                            Laser = 0;
                        }
                        else
                            Laser++;
                    }
                    if (target.Center.Distance(Projectile.Center) >= 800 || target.Center.Distance(Projectile.Center) >= 480 && !(Collision.CanHit(target.Center, 1, 1, Projectile.TopLeft, 1, 1) && Collision.CanHit(target.Center, 1, 1, Projectile.TopRight, 1, 1) && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height)))
                    {
                        MissTimer++;
                        if (MissTimer > 120)
                        {
                            Gun = 0;
                            AIState = 3;
                            MissTimer = 0;
                        }
                    }
                    Gravity();
                    Projectile.velocity.X = 0;
                    break;
                case 3:
                    Flying = true;
                    Projectile.tileCollide = false;
                    Projectile.direction = Projectile.velocity.X >= 0 ? 1 : -1;
                    Projectile.rotation = ((Projectile.velocity.X >= 0) ? 0 : MathHelper.Pi) + Projectile.velocity.ToRotation();
                    Projectile.velocity = (target.Center - Projectile.Center) / 56;
                    Projectile.frame = 1;
                    Gun++;
                    if (Gun >= 5)
                    {
                        Vector2 velocity = Vector2.Normalize(Projectile.velocity) * -10f;
                        SoundEngine.PlaySound(CalamityMod.Sounds.CommonCalamitySounds.LaserCannonSound with { Volume = CalamityMod.Sounds.CommonCalamitySounds.LaserCannonSound.Volume - 0.1f }, Projectile.Center);
                        Projectile p = Main.projectile[Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + (Vector2.UnitX * ((Projectile.velocity.X >= 0) ? -1 : 1) * Projectile.width).RotatedBy(Projectile.rotation), velocity, ModContent.ProjectileType<CalamityMod.Projectiles.Summon.AstrageldonLaser>(), Projectile.damage, Projectile.knockBack)];
                        p.tileCollide = false;
                        if (p.whoAmI.WithinBounds(Main.maxProjectiles))
                        {
                            p.originalDamage = Projectile.damage;
                        }
                        Gun = 0;
                    }
                    if (target.Center.Distance(Projectile.Center) < 360 || target.Center.Distance(Projectile.Center) < 720 && Collision.CanHit(target.Center, 1, 1, Projectile.TopLeft, 1, 1) && Collision.CanHit(target.Center, 1, 1, Projectile.TopRight, 1, 1) && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
                    {
                        Gun = 0;
                        Jump = 0;
                        AIState = 2;
                    }
                    break;
            }
            Projectile.spriteDirection = Projectile.direction;
        }

        public void Gravity()
        {
            if (Projectile.velocity.Y < 9.8)
                Projectile.velocity.Y += 0.25f;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity.X *= 0.9f;
            return false;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            fallThrough = Projectile.Bottom.Y < Owner.Top.Y;
            return true;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (Flying)
                CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Type], lightColor);
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Texture2D texture2 = ModContent.Request<Texture2D>(Texture + "glow").Value;
            Rectangle rect = new(0, texture.Height / 2 * Projectile.frame, texture.Width, texture.Height / 2);
            SpriteEffects spriteEffect = (Projectile.spriteDirection > 0) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), rect, lightColor, Projectile.rotation, new Vector2(texture.Width, texture.Height / 2) / 2f, Projectile.scale, spriteEffect, 0f);
            Main.spriteBatch.Draw(texture2, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), rect, new Color(255, 255, 255, 255), Projectile.rotation, new Vector2(texture.Width, texture.Height / 2) / 2f, Projectile.scale, spriteEffect, 0f);
            return false;
        }
    }
}
