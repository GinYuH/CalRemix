using CalamityMod.Projectiles.Magic;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using tModPorter;

namespace CalRemix.Content.NPCs.Bosses.RebornModPhoenixes.RebornModMiracleVine
{
    public class MiracleLeaf : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.aiStyle = ProjAIStyleID.Vilethorn;
            Projectile.hostile = true;
            Projectile.penetrate = 3;
            Projectile.scale = 1;
            Projectile.width = 15;
            Projectile.height = 15;
            Projectile.alpha = 255;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            //Projectile.pretendType = 7;
            //Projectile.maxUpdates = 2;
        }

        public override void AI()
        {
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
            if (Projectile.ai[0] == 0f)
            {
                #region not last frame
                Projectile.alpha -= 50;
                if (Projectile.alpha <= 0)
                {
                    Projectile.alpha = 0;
                    Projectile.ai[0] = 1f;
                    if (Projectile.ai[1] == 0f)
                    {
                        Projectile.ai[1] += 1f;
                        Projectile.position += Projectile.velocity * 1f;
                    }
                    #region if not last thorn
                    if (Projectile.type == ModContent.ProjectileType<MiracleVines>() && Main.myPlayer == Projectile.owner)
                    {
                        int num14 = Projectile.type;
                        #region should be last thorn
                        if (Projectile.ai[1] >= 13f) // change for max parts
                        {
                            num14 = ModContent.ProjectileType<BrimstoneHellfireballFriendly>();
                        }
                        else
                            num14 = ModContent.ProjectileType<BrimstoneHellfireballFriendly>();
                        #endregion
                        int num15 = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.position.X + Projectile.velocity.X + (float)(Projectile.width / 2), Projectile.position.Y + Projectile.velocity.Y + (float)(Projectile.height / 2)), Projectile.velocity, num14, Projectile.damage, Projectile.knockBack, Projectile.owner);
                        Main.projectile[num15].damage = Projectile.damage;
                        Main.projectile[num15].ai[1] = Projectile.ai[1] + 1f;
                        NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, num15, 0f, 0f, 0f, 0);
                        #region funtime
                        /*
                                                                Vector2 holdme = new Vector2(Projectile.velocity.X,Projectile.velocity.Y);
                                                                Projectile.velocity=RotateByRightAngle(holdme);
                                                                num15 = Projectile.NewProjectile(Projectile.position.X + Projectile.velocity.X + (float)(Projectile.width / 2), Projectile.position.Y + Projectile.velocity.Y + (float)(Projectile.height / 2), Projectile.velocity.X, Projectile.velocity.Y, num14, Projectile.damage, Projectile.knockBack, Projectile.owner);
                                                                Main.projectile[num15].damage = Projectile.damage;
                                                                Main.projectile[num15].ai[1] = Projectile.ai[1] + 1f;
                                                                NetMessage.SendData(27, -1, -1, "", num15, 0f, 0f, 0f, 0);
                                                                Projectile.velocity=RotateByLeftAngle(holdme);
                                                                num15 = Projectile.NewProjectile(Projectile.position.X + Projectile.velocity.X + (float)(Projectile.width / 2), Projectile.position.Y + Projectile.velocity.Y + (float)(Projectile.height / 2), Projectile.velocity.X, Projectile.velocity.Y, num14, Projectile.damage, Projectile.knockBack, Projectile.owner);
                                                                Main.projectile[num15].damage = Projectile.damage;
                                                                Main.projectile[num15].ai[1] = Projectile.ai[1] + 1f;
                                                                NetMessage.SendData(27, -1, -1, "", num15, 0f, 0f, 0f, 0);
                         */
                        #endregion
                        return;
                    }
                    #endregion
                }
                #endregion
            }
            else
            {
                #region makes it pretty
                if (Projectile.alpha < 170 && Projectile.alpha + 5 >= 170)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.CorruptGibs, Projectile.velocity.X * 0.025f, Projectile.velocity.Y * 0.025f, 170, default(Color), 1.2f);
                    }
                    Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Demonite, 0f, 0f, 170, default(Color), 1.1f);
                }
                #endregion
                Projectile.alpha += 5;
                if (Projectile.alpha >= 255)
                {
                    Projectile.Kill();
                    return;
                }
            }
        }

        #region math is fun!        
        public Vector2 RotateByRightAngle(Vector2 vector)
        {
            return new Vector2(vector.Y, -vector.X);
        }
        public Vector2 RotateByLeftAngle(Vector2 vector)
        {
            return new Vector2(-vector.Y, vector.X);
        }
        /*
        public Vector2 MyRotate(Vector2 vector,float rot)
        {
            float (
            px = cos(rot) * (px-ox) - sin(rot) * (py-oy) + ox;
            py = sin(rot) * (px-ox) + cos(rot) * (py-oy) + oy;
        }
        */
        public Vector2 RotateAboutOrigin(Vector2 point, float rotation)
        {
            if (rotation < 0)
                rotation += (float)(Math.PI * 4);
            Vector2 u = point; //point relative to origin  

            if (u == Vector2.Zero)
                return point;

            float a = (float)Math.Atan2(u.Y, u.X); //angle relative to origin  
            a += rotation; //rotate  

            //u is now the new point relative to origin  
            u = u.Length() * new Vector2((float)Math.Cos(a), (float)Math.Sin(a));
            return u;
        }
        #endregion
    }
}
