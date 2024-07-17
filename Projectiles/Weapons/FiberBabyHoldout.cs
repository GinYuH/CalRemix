using CalamityMod;
using CalRemix.ExtraTextures;
using CalRemix.Items.Weapons;
using CalRemix.NPCs.Bosses.Carcinogen;
using CalRemix.Projectiles.Hostile;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Projectiles.Weapons
{
    public class FiberBabyHoldout : ModProjectile
    {
        public Player Owner => Main.player[Projectile.owner];
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fiber Baby");
            Main.projFrames[Type] = 4;
        }
        public override void SetDefaults()
        {
            Projectile.width = 42;
            Projectile.height = 50;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Magic;
        }
        public override void AI()
        {
            int totalTime = 60;
            if (!Owner.active || Owner.dead || Owner.HeldItem.type != ModContent.ItemType<FiberBaby>())
            {
                Projectile.Kill();
                return;
            }
            Projectile.timeLeft = 2;
            if (Main.myPlayer == Owner.whoAmI)
            {
                Projectile.Center = Owner.Center + Owner.DirectionTo(Main.MouseWorld) * 16f;
                Projectile.spriteDirection = (Main.MouseWorld.X < Owner.Center.X) ? -1 : 1;
                Owner.ChangeDir(Projectile.spriteDirection);
                Projectile.netUpdate = true;
                Owner.heldProj = Projectile.whoAmI;
                Projectile.rotation = Projectile.DirectionTo(Main.MouseWorld).ToRotation();
                Projectile.rotation += Projectile.spriteDirection == -1 ? MathHelper.Pi : 0;
                Owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Owner.HandPosition.Value.DirectionTo(Projectile.Center).ToRotation() + (Projectile.spriteDirection == -1 ? MathHelper.Pi + 1f : -1f));
            }
            if (Owner.controlUseItem)
            {
                if (Projectile.ai[0] <= 0 && Owner.CheckMana(6, true))
                {
                    Projectile.ai[0] = totalTime;    
                }
                else
                {
                    Owner.SetDummyItemTime(2);
                }
            }
            Projectile.ai[0]--;

            int animationSpeed = 6;
            int totalLength = 59;

            if (Projectile.ai[0] == totalLength)
            {
                Projectile.frame = 1;
            }
            else if (Projectile.ai[0] == totalLength - animationSpeed)
            {
                Projectile.frame = 2;
            }
            else if (Projectile.ai[0] == totalLength - animationSpeed * 2)
            {
                SoundEngine.PlaySound(Carcinogen.DeathSound with { Pitch = 1.8f, Volume = 0.6f }, Projectile.Center);
                if (Owner == Main.LocalPlayer)
                {
                    int projCount = Main.rand.Next(3, 7);
                    int randomness = 5;
                    int speed = 11;
                    for (int i = 0; i < projCount; i++)
                    {
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.DirectionTo(Main.MouseWorld) * speed + Main.rand.NextVector2Circular(randomness, randomness), ModContent.ProjectileType<AsbestosDropFriendly>(), Projectile.damage, Projectile.knockBack, Owner.whoAmI);
                    }
                    for (int i = 0; i < projCount * 5; i++)
                    {
                        Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.Dirt, Projectile.DirectionTo(Main.MouseWorld) * speed * 0.75f + Main.rand.NextVector2Circular(randomness, randomness));
                        d.noGravity = true;
                    }
                }
                Projectile.frame = 3;
            }
            else if (Projectile.ai[0] == totalLength - animationSpeed * 4)
            {
                Projectile.frame = 2;
            }
            else if (Projectile.ai[0] == totalLength - animationSpeed * 5)
            {
                Projectile.frame = 1;
            }
            else if (Projectile.ai[0] <= totalLength - animationSpeed * 6)
            {
                Projectile.frame = 0;
            }

        }
        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }
    }
}