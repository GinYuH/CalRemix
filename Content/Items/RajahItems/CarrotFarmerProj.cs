using System;
using CalRemix.Content.Tiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.RajahItems
{
    public class CarrotFarmerProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Carrot Farmer");
        }

        public override void SetDefaults()
        {
            Projectile.width = 160;
            Projectile.height = 156;
            Projectile.aiStyle = 0;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ownerHitCheck = true;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 26;
            AIType = ProjectileID.Bullet;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            projHitbox.Width += 16;
            projHitbox.Height += 16;

            return projHitbox.Intersects(targetHitbox);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 8;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            Player player = Main.player[Projectile.owner];
            if (target.Center.X < player.Center.X)
            {
                modifiers.HitDirectionOverride = -1;
            }
            else
            {
                modifiers.HitDirectionOverride = 1;
            }
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (player.dead)
            {
                Projectile.Kill();
            }

            if (player.direction > 0)
            {
                Projectile.rotation += 0.35f;
                Projectile.spriteDirection = 1;
            }
            else
            {
                Projectile.rotation -= 0.35f;
                Projectile.spriteDirection = -1;
            }

            player.heldProj = Projectile.whoAmI;
            Projectile.position.X = player.Center.X - 80;
            Projectile.position.Y = player.Center.Y - 73;

            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X + 20, Projectile.Center.Y, -15f, 0f, Mod.Find<ModProjectile>("CarrotFarmerDamage").Type, Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X - 20, Projectile.Center.Y, 15f, 0f, Mod.Find<ModProjectile>("CarrotFarmerDamage").Type, Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);

            if (Projectile.timeLeft == 13)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X + 20, Projectile.Center.Y, -15f, 0f, Mod.Find<ModProjectile>("CarrotFarmerDamage2").Type, (int)(Projectile.damage * .35), Projectile.knockBack, Projectile.owner, 0f, 0f);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X - 20, Projectile.Center.Y, 15f, 0f, Mod.Find<ModProjectile>("CarrotFarmerDamage2").Type, (int)(Projectile.damage * .35), Projectile.knockBack, Projectile.owner, 0f, 0f);
            }

            if (Projectile.timeLeft < 8)
            {
                Projectile.alpha -= 28;
            }

            Projectile.ai[1]++;
            if (Projectile.ai[1] >= 16)
            {
                for (int u = 0; u < 10; u++)
                {
                    int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<CarrotDust>(), Main.rand.Next((int)-5f, (int)5f), Main.rand.Next((int)-5f, (int)5f), 0);
                    Main.dust[dust].noGravity = true;
                }
                float spread = 12f * 0.0174f;
                double startAngle = Math.Atan2(Projectile.velocity.X, Projectile.velocity.Y) - spread / 2;
                double deltaAngle = spread / 30f;
                double offsetAngle;
                int i;
                if (Projectile.owner == Main.myPlayer)
                {
                    for (i = 0; i < 30; i++)
                    {
                        offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;
                        if (Main.rand.Next(15) == 0)
                        {
                            int ProjID = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, (float)(Math.Sin(offsetAngle) * 6f), (float)(Math.Cos(offsetAngle) * 6f), Mod.Find<ModProjectile>("Carrot").Type, Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
                        }
                        if (Main.rand.Next(15) == 0)
                        {
                            int ProjID = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, (float)(-Math.Sin(offsetAngle) * 6f), (float)(-Math.Cos(offsetAngle) * 6f), Mod.Find<ModProjectile>("Carrot").Type, Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
                        }
                    }
                }
                Projectile.ai[1] = -0;
            }
        }
    }

    public class CarrotFarmerDamage : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";
        public override void SetDefaults()
        {
            Projectile.width = 160;
            Projectile.height = 156;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ownerHitCheck = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 8;
            AIType = ProjectileID.Bullet;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            Player player = Main.player[Projectile.owner];

            if (!ModLoader.TryGetMod("ThoriumMod", out Mod thorium))
                return;

            if (!thorium.TryFind("HealerDamage", out DamageClass healer))
                return;

            if (Main.rand.Next(100) <= player.GetCritChance(healer)) //((ModSupportPlayer)player.GetModPlayer(Mod, "ModSupportPlayer")).Thorium_radiantCrit)
            {
                modifiers.SetCrit();
            }
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            Projectile.position.X = player.Center.X - (Projectile.width / 2f);
            Projectile.position.Y = player.Center.Y - (Projectile.height / 2f);
        }
    }

    public class CarrotFarmerDamage2 : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";
        public override void SetDefaults()
        {
            Projectile.width = 130;
            Projectile.height = 128;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ownerHitCheck = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 4;
            AIType = ProjectileID.Bullet;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            Player player = Main.player[Projectile.owner];

            if (!ModLoader.TryGetMod("ThoriumMod", out Mod thorium))
                return;

            if (!thorium.TryFind("HealerDamage", out DamageClass healer))
                return;         

            if (Main.rand.Next(100) <= player.GetCritChance(healer))
            {
                modifiers.SetCrit();
            }
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            Projectile.position.X = player.Center.X - (Projectile.width / 2f);
            Projectile.position.Y = player.Center.Y - (Projectile.height / 2f);
        }
    }


    public class CarrotFarmerEffect : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";
        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.aiStyle = -1;
            Projectile.tileCollide = false;
            Projectile.ownerHitCheck = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 24;
        }

        public static Vector2 RotateVector(Vector2 origin, Vector2 vecToRot, float rot)
        {
            float newPosX = (float)(Math.Cos(rot) * (vecToRot.X - origin.X) - Math.Sin(rot) * (vecToRot.Y - origin.Y) + origin.X);
            float newPosY = (float)(Math.Sin(rot) * (vecToRot.X - origin.X) + Math.Cos(rot) * (vecToRot.Y - origin.Y) + origin.Y);
            return new Vector2(newPosX, newPosY);
        }

        public Vector2 rotVec = new Vector2(0, 65);
        public float rot = 0f;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (player.direction > 0)
            {
                rot += 0.20f;
            }
            else
            {
                rot -= 0.20f;
            }

            Projectile.Center = player.Center + new Vector2(-8f, -8f) + RotateVector(default, rotVec, rot + (Projectile.ai[0] * (6.28f / 2)));

            for (int m = 0; m < 5; m++)
            {
                float velX = Projectile.velocity.X / 3f * m;
                float velY = Projectile.velocity.Y / 3f * m;
                int dustID = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<CarrotDust>(), 0, 0, 0);
                Main.dust[dustID].position.X = Projectile.Center.X - velX;
                Main.dust[dustID].position.Y = Projectile.Center.Y - velY;
                Main.dust[dustID].velocity *= 0f;
                Main.dust[dustID].alpha = 180;
                Main.dust[dustID].noGravity = true;
                Main.dust[dustID].scale = 0.8f;
            }
        }
    }
}