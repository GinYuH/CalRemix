using CalRemix.Content.NPCs.Bosses.RebornModPhoenixes.Vernix;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using tModPorter;

namespace CalRemix.Content.NPCs.Bosses.RebornModPhoenixes.RebornModMiracleVine
{
    public class MiracleVines : ModProjectile
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
            //Projectile.MaxUpdates = 2;

            // the demon that comes when you call its name...
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override bool PreAI()
        {
            // this is maxupdates = 2, for any other tconfig porters out there
            // well... i think it is. it looks about the same, so its good enough for me
            if (Projectile.ai[2] % 2 == 0)
            {
                Projectile.ai[2]++;
                return true;
            }
            else
            {
                Projectile.ai[2]++;
                return false;
            }
        }

        public override void AI()
        {
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
            if (Projectile.ai[0] == 0f)
            {
                #region phoenix shit
                for (int num154 = 0; num154 < 1; num154++)
                {
                    int num155 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.CorruptGibs, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100, default, 1f);
                    if (Main.rand.Next(3) != 0 || true)
                    {
                        Main.dust[num155].noGravity = true;
                        Main.dust[num155].scale *= 3f;
                        Dust expr_6767_cp_0 = Main.dust[num155];
                        expr_6767_cp_0.velocity.X = expr_6767_cp_0.velocity.X * 2f;
                        Dust expr_6785_cp_0 = Main.dust[num155];
                        expr_6785_cp_0.velocity.Y = expr_6785_cp_0.velocity.Y * 2f;
                    }
                    Main.dust[num155].scale *= 1.5f;
                    Dust expr_67BC_cp_0 = Main.dust[num155];
                    expr_67BC_cp_0.velocity.X = expr_67BC_cp_0.velocity.X * 1.2f;
                    Dust expr_67DA_cp_0 = Main.dust[num155];
                    expr_67DA_cp_0.velocity.Y = expr_67DA_cp_0.velocity.Y * 1.2f;
                    Main.dust[num155].scale *= (float)((((Projectile.ai[0] + 1) / 2) * 25) / 50);

                }
                #endregion
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
                    if (Main.myPlayer == Projectile.owner)
                    {
                        int num14 = Projectile.type;
                        #region should be last thorn
                        if (Projectile.type == ModContent.ProjectileType<PerennialFlowerMineVine>()) // dont wanna reload sorry
                            num14 = ModContent.ProjectileType<PerennialFlowerMineVine>();
                        else
                            num14 = ModContent.ProjectileType<MiracleVines>();
                        #endregion
                        /*
                                                                int num15 = Projectile.NewProjectile(Projectile.position.X + Projectile.velocity.X + (float)(Projectile.width / 2), Projectile.position.Y + Projectile.velocity.Y + (float)(Projectile.height / 2), Projectile.velocity.X, Projectile.velocity.Y, num14, Projectile.damage, Projectile.knockBack, Projectile.owner);
                                                                Main.projectile[num15].damage = Projectile.damage;
                                                                Main.projectile[num15].ai[1] = Projectile.ai[1] + 1f;
                                                                NetMessage.SendData(27, -1, -1, "", num15, 0f, 0f, 0f, 0);
                         */
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
                        #region more funtime
                        /*
                                                                Vector2 holdme = new Vector2(Projectile.velocity.X,Projectile.velocity.Y);
                                                                Projectile.velocity=RotateAboutOrigin(holdme,(float)((Math.PI*7)/4f));
                                                                num15 = Projectile.NewProjectile(Projectile.position.X + Projectile.velocity.X + (float)(Projectile.width / 2), Projectile.position.Y + Projectile.velocity.Y + (float)(Projectile.height / 2), Projectile.velocity.X, Projectile.velocity.Y, num14, Projectile.damage, Projectile.knockBack, Projectile.owner);
                                                                Main.projectile[num15].damage = Projectile.damage;
                                                                Main.projectile[num15].ai[1] = Projectile.ai[1] + 1f;
                                                                NetMessage.SendData(27, -1, -1, "", num15, 0f, 0f, 0f, 0);
                                                                Projectile.velocity.X = holdme.X;
                                                                Projectile.velocity.Y = holdme.Y;
                                                                Projectile.velocity=RotateAboutOrigin(holdme,(float)((Math.PI*1)/4f));
                                                                num15 = Projectile.NewProjectile(Projectile.position.X + Projectile.velocity.X + (float)(Projectile.width / 2), Projectile.position.Y + Projectile.velocity.Y + (float)(Projectile.height / 2), Projectile.velocity.X, Projectile.velocity.Y, num14, Projectile.damage, Projectile.knockBack, Projectile.owner);
                                                                Main.projectile[num15].damage = Projectile.damage;
                                                                Main.projectile[num15].ai[1] = Projectile.ai[1] + 1f;
                                                                NetMessage.SendData(27, -1, -1, "", num15, 0f, 0f, 0f, 0);
                         */
                        #endregion
                        #region most fun ever
                        /*
                        if((int)(Projectile.ai[1])%8<4)
                        {
                                                                Vector2 holdme = new Vector2(Projectile.velocity.X,Projectile.velocity.Y);
                                                                Projectile.velocity=RotateAboutOrigin(holdme,(float)((Math.PI*1)/8f));
                                                                int num15 = Projectile.NewProjectile(Projectile.position.X + Projectile.velocity.X + (float)(Projectile.width / 2), Projectile.position.Y + Projectile.velocity.Y + (float)(Projectile.height / 2), Projectile.velocity.X, Projectile.velocity.Y, num14, Projectile.damage, Projectile.knockBack, Projectile.owner);
                                                                Main.projectile[num15].damage = Projectile.damage;
                                                                Main.projectile[num15].ai[1] = Projectile.ai[1] + 1f;
                                                                NetMessage.SendData(27, -1, -1, "", num15, 0f, 0f, 0f, 0);
                        }
                        else
                        {
                                                                Vector2 holdme = new Vector2(Projectile.velocity.X,Projectile.velocity.Y);
                                                                Projectile.velocity=RotateAboutOrigin(holdme,(float)((Math.PI*15)/8f));
                                                                int num15 = Projectile.NewProjectile(Projectile.position.X + Projectile.velocity.X + (float)(Projectile.width / 2), Projectile.position.Y + Projectile.velocity.Y + (float)(Projectile.height / 2), Projectile.velocity.X, Projectile.velocity.Y, num14, Projectile.damage, Projectile.knockBack, Projectile.owner);
                                                                Main.projectile[num15].damage = Projectile.damage;
                                                                Main.projectile[num15].ai[1] = Projectile.ai[1] + 1f;
                                                                NetMessage.SendData(27, -1, -1, "", num15, 0f, 0f, 0f, 0);
                        }
                         */
                        #endregion
                        int z1 = Main.rand.Next(2, 3);
                        if (Projectile.ai[1] % 3 != 0) 
                            z1 = 1;
                        else 
                            z1 = (6 - (int)(Projectile.ai[1] / 3)) / 2;
                        for (int zzz = 0; zzz < z1; zzz++)
                        {
                            int z2 = Main.rand.Next(-1, 2);
                            if (z2 < 0) z2 += 16;
                            Vector2 holdme = new Vector2(Projectile.velocity.X, Projectile.velocity.Y);
                            holdme = RotateAboutOrigin(holdme, (float)((Math.PI * z2) / 8f));
                            int num15 = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.position.X + holdme.X + (float)(Projectile.width / 2), Projectile.position.Y + holdme.Y + (float)(Projectile.height / 2)), holdme, num14, Projectile.damage, Projectile.knockBack, Projectile.owner);
                            Main.projectile[num15].damage = Projectile.damage;
                            Main.projectile[num15].ai[1] = Projectile.ai[1] + 1f;
                            Main.projectile[num15].friendly = Projectile.friendly;
                            Main.projectile[num15].hostile = Projectile.hostile;
                            NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, num15, 0f, 0f, 0f, 0);
                        }
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
                    /*
                    for (int j = 0; j < 3; j++)
                    {
                        Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 18, Projectile.velocity.X * 0.025f, Projectile.velocity.Y * 0.025f, 170, default(Color), 1.2f);
                    }
                    Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 14, 0f, 0f, 170, default(Color), 1.1f);

                     */
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
