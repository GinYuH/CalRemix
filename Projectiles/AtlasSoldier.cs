using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent;
using ReLogic.Content;
using Terraria.Graphics.Shaders;
using CalamityMod.DataStructures;
using CalRemix.Buffs;
using CalamityMod;

namespace CalRemix.Projectiles
{
    public class AtlasSoldier : ModProjectile
    {
        public ref float AIState => ref Projectile.ai[0];
        public ref float MissTimer => ref Projectile.ai[1];
        public Player Owner => Main.player[Projectile.owner];

        public int gun;
        public int flighttimer;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Atlas Soldier");
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 108;
            Projectile.height = 94;
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

        public override void AI()
        {
            Owner.AddBuff(ModContent.BuffType<AtlasSoldierBuff>(), 3600);

            // Verify player/minion state integrity. The minion cannot stay alive if the
            // owner is dead or if the caller of the AI is invalid.
            if (Projectile.type != ModContent.ProjectileType<AtlasSoldier>())
                return;

            if (Owner.dead)
                Owner.GetModPlayer<CalRemixPlayer>().soldier = false;
            if (Owner.GetModPlayer<CalRemixPlayer>().soldier)
                Projectile.timeLeft = 2;

            NPC target = Projectile.Center.MinionHoming(950f, Owner, true);
            if (target is null)
                AIState = 1;
            else if (AIState != 3)
            {
                AIState = 2;
            }
            switch (AIState)
            {
                case 1:
                    Projectile.tileCollide = true;
                    flighttimer = 0;
                    //if (Owner.Center.Distance(Projectile.Center) > 300)
                    if (Math.Abs(Owner.position.X - Projectile.position.X) > 300)
                    {
                        Projectile.rotation = Projectile.velocity.X >= 0 ? -MathHelper.PiOver2 :MathHelper.Pi;
                        Projectile.direction = Projectile.velocity.X >= 0 ? 1 : -1;
                        Projectile.spriteDirection = Projectile.velocity.X >= 0 ? 1 : -1;
                        float idealx = MathHelper.Lerp(Projectile.position.X, Owner.Center.X, 0.01f);
                        float idealy = MathHelper.Lerp(Projectile.position.Y, Owner.Center.Y, 0.01f);
                        Projectile.position = new Vector2(idealx, idealy);
                        gun++;
                        if (gun >= 5)
                        {
                            Terraria.Audio.SoundEngine.PlaySound(CalamityMod.Sounds.CommonCalamitySounds.LaserCannonSound with { Volume = CalamityMod.Sounds.CommonCalamitySounds.LaserCannonSound.Volume - 0.1f }, Projectile.Center);
                            Projectile p = Main.projectile[Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(0, 10), ModContent.ProjectileType<CalamityMod.Projectiles.Summon.AstrageldonLaser>(), Projectile.damage, Projectile.knockBack)];
                            p.tileCollide = false;
                            if (p.whoAmI.WithinBounds(Main.maxProjectiles))
                            {
                                p.originalDamage = Projectile.damage;
                            }
                            gun = 0;
                        }
                    }
                    else
                    {
                        Projectile.rotation = 0;
                        Projectile.spriteDirection = Owner.position.X - Projectile.position.X >= 0 ? -1 : 1;
                        Gravity();
                        Projectile.velocity.X = 0;
                    }
                    break;
                case 2:
                    Projectile.tileCollide = true;
                    Projectile.rotation = 0;
                    Projectile.spriteDirection = target.position.X - Projectile.position.X >= 0 ? -1 : 1;
                    gun++;
                    if (gun >= 5)
                    {
                        Vector2 position = Projectile.Center;
                        Vector2 targetPosition = target.Center;
                        Vector2 direction = targetPosition - position;
                        direction.Normalize();
                        Terraria.Audio.SoundEngine.PlaySound(CalamityMod.Sounds.CommonCalamitySounds.LaserCannonSound with { Volume = CalamityMod.Sounds.CommonCalamitySounds.LaserCannonSound.Volume - 0.1f }, Projectile.Center);
                        int p = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, direction * 10, ModContent.ProjectileType<CalamityMod.Projectiles.Summon.AstralProbeRound>(), Projectile.damage, Projectile.knockBack, Owner.whoAmI, target.whoAmI, 0f);
                        if (p.WithinBounds(Main.maxProjectiles))
                        {
                            Main.projectile[p].originalDamage = Projectile.damage;
                        }
                        gun = 0;
                    }
                    if (target.Center.Distance(Projectile.Center) > 600)
                    {
                        MissTimer++;
                        if (MissTimer > 180)
                        {
                            gun = 0;
                            AIState = 3;
                            MissTimer = 0;
                        }
                    }
                    Gravity();
                    Projectile.velocity.X = 0;
                    break;
                case 3:
                    Projectile.tileCollide = false;
                    gun++;
                    if (gun >= 5)
                    {
                        Terraria.Audio.SoundEngine.PlaySound(CalamityMod.Sounds.CommonCalamitySounds.LaserCannonSound with { Volume = CalamityMod.Sounds.CommonCalamitySounds.LaserCannonSound.Volume - 0.1f }, Projectile.Center);
                        Projectile p = Main.projectile[Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(0, 10), ModContent.ProjectileType<CalamityMod.Projectiles.Summon.AstrageldonLaser>(), Projectile.damage, Projectile.knockBack)];
                        p.tileCollide = false;
                        if (p.whoAmI.WithinBounds(Main.maxProjectiles))
                        {
                            p.originalDamage = Projectile.damage;
                        }
                        flighttimer += 3;
                        gun = 0;
                    }
                    if (target.Center.Distance(Projectile.Center) < 600)
                    {
                        gun = 0;
                        flighttimer = 0;
                        AIState = 2;
                    }
                    Projectile.rotation = Projectile.velocity.X >= 0 ? -MathHelper.PiOver2 : -3 * MathHelper.PiOver2;
                    Projectile.direction = Projectile.velocity.X >= 0 ? 1 : -1;
                    Projectile.spriteDirection = Projectile.velocity.X >= 0 ? 1 : -1;
                    float targx = MathHelper.Lerp(Projectile.position.X, target.position.X, 0.01f);
                    float targy = MathHelper.Lerp(Projectile.position.Y, target.position.Y - flighttimer, 0.01f);
                    Projectile.position = new Vector2(targx, targy);
                    break;
            }
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
    }
}
