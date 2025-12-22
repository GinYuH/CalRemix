using System;
using CalRemix.Content.Tiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.NPCs.Bosses.RajahBoss
{
    public class CarrotFarmerR : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Carrot Farmer");
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
            Projectile.hostile = true;
            Projectile.friendly = false;
            AIType = ProjectileID.Bullet;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            projHitbox.Width += 16;
            projHitbox.Height += 16;

            return projHitbox.Intersects(targetHitbox);
        }

        public Rajah rajah = null;

        public override void AI()
        {
            if (rajah == null)
            {
                NPC npcBody = Main.npc[(int)Projectile.ai[0]];
                if (npcBody.type == ModContent.NPCType<Rajah>())
                {
                    rajah = (Rajah)npcBody.ModNPC;
                }
                else if (npcBody.type == ModContent.NPCType<SupremeRajah>())
                {
                    rajah = (SupremeRajah)npcBody.ModNPC;
                }
            }
            if (rajah == null)
                return;

            if (!rajah.NPC.active || rajah.NPC.life <= 0 || rajah.NPC.ai[3] != 4)
            {
                Projectile.Kill();
            }

            if (rajah.NPC.direction > 0)
            {
                Projectile.rotation += 0.35f;
                Projectile.spriteDirection = 1;
            }
            else
            {
                Projectile.rotation -= 0.35f;
                Projectile.spriteDirection = -1;
            }

            Projectile.position.X = rajah.WeaponPos.X - 95;
            Projectile.position.Y = rajah.WeaponPos.Y - 93;

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
                        int carrotType = rajah.isSupreme ? Mod.Find<ModProjectile>("CarrotEXR").Type : Mod.Find<ModProjectile>("CarrotHostile").Type;
                        if (Main.rand.Next(rajah.isSupreme ? 10 : 15) == 0)
                        {
                            int ProjID = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, (float)(Math.Sin(offsetAngle) * 6f), (float)(Math.Cos(offsetAngle) * 6f), carrotType, Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
                            Main.projectile[ProjID].Center = Projectile.Center;
                        }
                        if (Main.rand.Next(rajah.isSupreme ? 10 : 15) == 0)
                        {
                            int ProjID = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, (float)(-Math.Sin(offsetAngle) * 6f), (float)(-Math.Cos(offsetAngle) * 6f), carrotType, Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
                            Main.projectile[ProjID].Center = Projectile.Center;
                        }
                    }
                }
                Projectile.ai[1] = -0;
            }
        }
    }
}