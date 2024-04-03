using CalamityMod;
using CalamityMod.Projectiles.Summon;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Buffs;
using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;

namespace CalRemix.Projectiles.Accessories
{
    public class CosmicConflict : ModProjectile
    {
        int idealpos = 1;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cosmic Crystal Conflict");
            Main.projFrames[Projectile.type] = 2;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 240;
            Projectile.netImportant = true;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.minionSlots = 0;
            Projectile.timeLeft = 18000;
            Projectile.penetrate = -1;
            Projectile.timeLeft *= 5;
            Projectile.minion = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Summon;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            CalRemixPlayer modPlayer = player.GetModPlayer<CalRemixPlayer>();
            player.AddBuff(ModContent.BuffType<CosmicConflictBuff>(), 3600);
            bool flag64 = Projectile.type == ModContent.ProjectileType<CosmicConflict>();
            if (flag64)
            {
                if (modPlayer.tvohide)
                {
                    Projectile.active = false;
                }
                if (player.dead)
                {
                    modPlayer.crystalconflict = false;
                }
                if (modPlayer.crystalconflict)
                {
                    Projectile.timeLeft = 2;
                }
            }

            NPC targ = CalamityUtils.MinionHoming(Projectile.Center, 22222, Main.player[Projectile.owner]);
            if (targ != null && targ.active)
            {
                NPC npc = targ;
                Projectile.direction = npc.Center.X - Projectile.Center.X >= 0 ? -1 : 1;
                Projectile.spriteDirection = npc.Center.X - Projectile.Center.X >= 0 ? -1 : 1;
                Projectile.localAI[0]++;
                if (Projectile.localAI[0] < 300)
                {
                    Projectile.localAI[1]++;
                    Projectile.ChargingMinionAI(1600f, 1800f, 2500f, 400f, 1, 30f, 24f, 12f, new Vector2(0f, -60f), 30f, 10f, true, true);
                    if (Main.myPlayer == Projectile.owner && Projectile.localAI[1] >= 30)
                    {
                        SoundEngine.PlaySound(SoundID.Item105 with { Volume = SoundID.Zombie105.Volume - 0.4f }, Projectile.position);
                        int p = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(Main.rand.Next(-30, 30), Main.rand.Next(-30, 30)), ModContent.ProjectileType<CosmicBlast>(), (int)(Projectile.damage * 0.2f), 0f, Projectile.owner);
                        if (Main.projectile.IndexInRange(p))
                            Main.projectile[p].originalDamage = (int)(Projectile.damage * 0.2f);
                        Main.projectile[p].GetGlobalProjectile<CalRemixProjectile>().uniproj = true;
                        Projectile.localAI[1] = 0;
                    }
                }
                else if (Projectile.localAI[0] >= 300 && Projectile.localAI[0] < 600)
                {
                    if (Projectile.localAI[0] == 300)
                        Projectile.localAI[1] = 0;
                    Projectile.localAI[1]++;
                    Projectile.frame = 1;
                    int num412 = 1;
                    float num413 = 25f;
                    float num414 = 1.2f;
                    float distanceX = 420f * Projectile.spriteDirection;
                    float yoffset = 0f;

                    Vector2 vector40 = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
                    float num415 = npc.position.X + (float)(npc.width / 2) + (float)(num412 * distanceX) - vector40.X;
                    float num416 = npc.position.Y + (float)(npc.height / 2) + yoffset - vector40.Y;
                    float num417 = (float)System.Math.Sqrt(num415 * num415 + num416 * num416);
                    num417 = num413 / num417;
                    num415 *= num417;
                    num416 *= num417;
                    if (Projectile.velocity.X < num415)
                    {
                        Projectile.velocity.X += num414;
                        if (Projectile.velocity.X < 0f && num415 > 0f)
                        {
                            Projectile.velocity.X += num414;
                        }
                    }
                    else if (Projectile.velocity.X > num415)
                    {
                        Projectile.velocity.X -= num414;
                        if (Projectile.velocity.X > 0f && num415 < 0f)
                        {
                            Projectile.velocity.X -= num414;
                        }
                    }
                    if (Projectile.velocity.Y < num416)
                    {
                        Projectile.velocity.Y += num414;
                        if (Projectile.velocity.Y < 0f && num416 > 0f)
                        {
                            Projectile.velocity.Y += num414;
                        }
                    }
                    else if (Projectile.velocity.Y > num416)
                    {
                        Projectile.velocity.Y -= num414;
                        if (Projectile.velocity.Y > 0f && num416 < 0f)
                        {
                            Projectile.velocity.Y -= num414;
                        }
                    }                    

                    if (Projectile.localAI[1] >= 90)
                    {
                        SoundEngine.PlaySound(CalamityMod.Sounds.CommonCalamitySounds.GaussWeaponFire, npc.Center);
                        for (int i = 0; i < 12; i++)
                        {
                            double deg = 30 * i;
                            double rad = deg * (Math.PI / 180);
                            double dist = 200;
                            Vector2 spawn;
                            spawn.X = npc.Center.X - (int)(Math.Cos(rad) * dist) - 4;
                            spawn.Y = npc.Center.Y - (int)(Math.Sin(rad) * dist) - 4;

                            Vector2 targetPosition = npc.Center;
                            Vector2 direction = targetPosition - spawn;
                            direction.Normalize();

                            int p = Projectile.NewProjectile(Projectile.GetSource_FromAI(), spawn, direction * 15, ModContent.ProjectileType<EndoIceShard>(), (int)(Projectile.damage * 0.05f), 0f, Main.myPlayer);
                            if (p.WithinBounds(Main.maxProjectiles))
                            {
                                Main.projectile[p].originalDamage = (int)(Projectile.damage * 0.05f);
                            }
                            Main.projectile[p].GetGlobalProjectile<CalRemixProjectile>().uniproj = true;
                            Projectile.localAI[1] = 0;
                        }
                    }
                }
                else if (Projectile.localAI[0] >= 600 && Projectile.localAI[0] < 900)
                {
                    if (Projectile.localAI[0] == 600)
                    {
                        Projectile.localAI[1] = 0;
                    }
                    
                    Projectile.localAI[1]++;
                    Projectile.frame = 0;
                    float lerpx = MathHelper.Lerp(Projectile.position.X, npc.position.X + 500 * idealpos, 0.1f);
                    float lerpY = MathHelper.Lerp(Projectile.position.Y, npc.position.Y - 200, 0.1f);
                    Projectile.position = new Vector2(lerpx, lerpY);
                    if (Main.myPlayer == Projectile.owner)
                    {
                        if (Projectile.localAI[1] % 20 == 0)
                        {
                            int p = Projectile.NewProjectile(Projectile.GetSource_FromAI(), new Vector2(npc.Center.X + Main.rand.Next(-64, 64), npc.Center.Y + 200), new Vector2(Main.rand.Next(-20, 20), -40f), ModContent.ProjectileType<CalamityMod.Projectiles.Melee.DNA>(), (int)(Projectile.damage * 0.2f), 0, Main.myPlayer, 0f, 0f);
                            int p2 = Projectile.NewProjectile(Projectile.GetSource_FromAI(), new Vector2(npc.Center.X + Main.rand.Next(-16, 16), npc.Center.Y + 200), new Vector2(Main.rand.Next(-20, 20), -40f), ModContent.ProjectileType<CalamityMod.Projectiles.Melee.DNA>(), (int)(Projectile.damage * 0.2f), 0, Main.myPlayer, 0f, 0f);
                            if (Main.projectile.IndexInRange(p))
                                Main.projectile[p].originalDamage = Projectile.originalDamage;
                            if (Main.projectile.IndexInRange(p2))
                                Main.projectile[p2].originalDamage = Projectile.originalDamage;
                            Main.projectile[p].DamageType = DamageClass.Summon;
                            Main.projectile[p2].DamageType = DamageClass.Summon;
                            Main.projectile[p].GetGlobalProjectile<CalRemixProjectile>().uniproj = true;
                            Main.projectile[p2].GetGlobalProjectile<CalRemixProjectile>().uniproj = true;
                        }
                    }
                    if (Projectile.localAI[1] % 60 == 0)
                    {
                        SoundEngine.PlaySound(CalamityMod.NPCs.PlaguebringerGoliath.PlaguebringerGoliath.BarrageLaunchSound, Projectile.Center);
                        idealpos *= -1;
                    }
                }
                else if (Projectile.localAI[0] >= 900 && Projectile.localAI[0] < 1200)
                {
                    if (Projectile.localAI[0] == 900)
                        Projectile.localAI[1] = 0;
                    Projectile.localAI[1]++;
                    Projectile.frame = 1;
                    int num412 = 1;
                    float num413 = 25f;
                    float num414 = 1.2f;
                    float distanceX = 520f * Projectile.spriteDirection;
                    float yoffset = 0f;

                    Vector2 vector40 = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
                    float num415 = npc.position.X + (float)(npc.width / 2) + (float)(num412 * distanceX) - vector40.X;
                    float num416 = npc.position.Y + (float)(npc.height / 2) + yoffset - vector40.Y;
                    float num417 = (float)System.Math.Sqrt(num415 * num415 + num416 * num416);
                    num417 = num413 / num417;
                    num415 *= num417;
                    num416 *= num417;
                    if (Projectile.velocity.X < num415)
                    {
                        Projectile.velocity.X += num414;
                        if (Projectile.velocity.X < 0f && num415 > 0f)
                        {
                            Projectile.velocity.X += num414;
                        }
                    }
                    else if (Projectile.velocity.X > num415)
                    {
                        Projectile.velocity.X -= num414;
                        if (Projectile.velocity.X > 0f && num415 < 0f)
                        {
                            Projectile.velocity.X -= num414;
                        }
                    }
                    if (Projectile.velocity.Y < num416)
                    {
                        Projectile.velocity.Y += num414;
                        if (Projectile.velocity.Y < 0f && num416 > 0f)
                        {
                            Projectile.velocity.Y += num414;
                        }
                    }
                    else if (Projectile.velocity.Y > num416)
                    {
                        Projectile.velocity.Y -= num414;
                        if (Projectile.velocity.Y > 0f && num416 < 0f)
                        {
                            Projectile.velocity.Y -= num414;
                        }
                    }

                    if (Projectile.localAI[1] % 60 == 0)
                    {
                        SoundEngine.PlaySound(CalamityMod.Sounds.CommonCalamitySounds.LargeWeaponFireSound, Projectile.Center);
                        Vector2 dest = npc.Center - Projectile.Center;
                        dest.Normalize();
                        Vector2 laserVel = dest * 10;
                        Vector2 spawnloc = new Vector2(Projectile.Center.X + 120 * -Projectile.spriteDirection, Projectile.Center.Y - 60);
                        int p = Projectile.NewProjectile(Projectile.GetSource_FromThis(), spawnloc, laserVel, ModContent.ProjectileType<CalamityMod.Projectiles.Melee.GalileosPlanet>(), Projectile.damage * 2, 0f, Projectile.owner);
                        if (Main.projectile.IndexInRange(p))
                            Main.projectile[p].originalDamage = Projectile.originalDamage;
                        Main.projectile[p].DamageType = DamageClass.Summon;
                        Main.projectile[p].tileCollide = false;
                        Main.projectile[p].GetGlobalProjectile<CalRemixProjectile>().uniproj = true;
                        Projectile.netUpdate = true;
                    }
                }
                else
                {
                    Projectile.frame = 0;
                    Projectile.localAI[1] = 0;
                    Projectile.localAI[0] = 0;
                }
            }
            else
            {
                Projectile.FloatingPetAI(false, 0.02f);
                Projectile.localAI[0] = 0;
                Projectile.localAI[1] = 0;
                Projectile.frame = 0;
            }
        }

        public override void PostDraw(Color lightColor)
        {
            Texture2D deusheadsprite;
            Player player = Main.player[Projectile.owner];
            deusheadsprite = (ModContent.Request<Texture2D>("CalRemix/Projectiles/Accessories/EarthElementalArm").Value);
            Color deusheadalpha = Color.White;
            float size = 1;
            for (int i = 0; i < 5; i++)
            {
                switch (i)
                {
                    case 0:
                        deusheadsprite = (ModContent.Request<Texture2D>("CalRemix/Projectiles/Accessories/CosmicCrystal_Air").Value);
                        if (Projectile.localAI[0] < 300 && player.HasMinionAttackTargetNPC)
                        {
                            size = 1.5f;
                        }
                        break;
                    case 1:
                        deusheadsprite = (ModContent.Request<Texture2D>("CalRemix/Projectiles/Accessories/CosmicCrystal_Ice").Value);
                        if (Projectile.localAI[0] >= 300 && Projectile.localAI[0] < 600 && player.HasMinionAttackTargetNPC)
                        {
                            size = 1.5f;
                        }
                        break;
                    case 2:
                        deusheadsprite = (ModContent.Request<Texture2D>("CalRemix/Projectiles/Accessories/CosmicCrystal_Earth").Value);
                        size = 1.25f;
                        break;
                    case 3:
                        deusheadsprite = (ModContent.Request<Texture2D>("CalRemix/Projectiles/Accessories/CosmicCrystal_Fire").Value);
                        if (Projectile.localAI[0] >= 900 && Projectile.localAI[0] < 1200 && player.HasMinionAttackTargetNPC)
                        {
                            size = 1.5f;
                        }
                        break;
                    case 4:
                        deusheadsprite = (ModContent.Request<Texture2D>("CalRemix/Projectiles/Accessories/CosmicCrystal_Nature").Value);
                        if (Projectile.localAI[0] >= 600 && Projectile.localAI[0] < 900 && player.HasMinionAttackTargetNPC)
                        {
                            size = 1.5f;
                        }
                        break;
                    default:
                        deusheadsprite = (ModContent.Request<Texture2D>("CalamityMod/Projectiles/Accessories/InvisibleProj").Value);
                        break;
                }

                double deg = 45 * i;
                double rad = deg * (Math.PI / 180);
                double dist = 50;
                Vector2 spawn;
                spawn.X = Projectile.Center.X - (int)(Math.Cos(rad) * dist) + 70;
                spawn.Y = Projectile.position.Y - (int)(Math.Sin(rad) * dist) - 4;

                Main.EntitySpriteDraw(deusheadsprite, spawn - Main.screenPosition, null, deusheadalpha, 0, Utils.Size(new Rectangle(0, 0, deusheadsprite.Width, deusheadsprite.Height)) / 2f, size, SpriteEffects.None, 0);

            }
        }
    }
}
