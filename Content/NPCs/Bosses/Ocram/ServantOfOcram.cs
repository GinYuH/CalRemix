using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.NPCs.Bosses.Ocram
{
    public class ServantOfOcram : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 2;
        }

        public override void SetDefaults()
        {
            //NPC.TypeName = "Servant of Ocram"; dont forget to carry the lowercase o in of
            NPC.width = 20;
            NPC.height = 20;
            //NPC.aiStyle = 5; reimplemented ai for highest accuracy (and mod compat!)
            NPC.damage = 35;
            NPC.defense = 5;
            NPC.lifeMax = 130;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
        }

        public override void AI()
        {
            if (NPC.target == Main.maxPlayers || Main.player[NPC.target].dead)
            {
                NPC.TargetClosest();
            }
            float num;
            float num2;

            Lighting.AddLight(NPC.position, new Vector3(1f, 1f, 1f));
            num = 9f;
            num2 = 0.1f;

            int num3 = (int)(NPC.position.X + (NPC.width / 2)) & -8;
            int num4 = (int)(NPC.position.Y + (NPC.height / 2)) & -8;
            // til you can use & to round to nearest instance of number. cool stuff
            float num5 = ((int)(Main.player[NPC.target].position.X + 10) & -8) - num3;
            float num6 = ((int)(Main.player[NPC.target].position.Y + 21) & -8) - num4;
            float num7 = num5 * num5 + num6 * num6;
            float num8 = num7;
            bool flag = false;

            if (num7 == 0f)
            {
                num5 = NPC.velocity.X;
                num6 = NPC.velocity.Y;
            }
            else
            {
                if (num7 > 360000f)
                {
                    flag = true;
                }
                num7 = num / (float)Math.Sqrt(num7);
                num5 *= num7;
                num6 *= num7;
            }
            
            if (Main.player[NPC.target].dead)
            {
                num5 = NPC.direction * num * 0.5f;
                num6 = num * -0.5f;
            }

            if (NPC.velocity.X < num5)
            {
                NPC.velocity.X += num2;
                if (NPC.velocity.X < 0f && num5 > 0f)
                {
                    NPC.velocity.X += num2;
                }
            }
            else if (NPC.velocity.X > num5)
            {
                NPC.velocity.X -= num2;
                if (NPC.velocity.X > 0f && num5 < 0f)
                {
                    NPC.velocity.X -= num2;
                }
            }

            if (NPC.velocity.Y < num6)
            {
                NPC.velocity.Y += num2;
                if (NPC.velocity.Y < 0f && num6 > 0f)
                {
                    NPC.velocity.Y += num2;
                }
            }
            else if (NPC.velocity.Y > num6)
            {
                NPC.velocity.Y -= num2;
                if (NPC.velocity.Y > 0f && num6 < 0f)
                {
                    NPC.velocity.Y -= num2;
                }
            }

            NPC.rotation = (float)Math.Atan2(NPC.velocity.Y, NPC.velocity.X) - 1.57f;

            if (Main.rand.Next(48) == 0)
            {
                Dust dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y + (NPC.height / 4)), NPC.width, (NPC.height / 2), DustID.Blood, NPC.velocity.X, 2.0f);
                if (dust != null)
                {
                    dust.velocity.X *= 0.5f;
                    dust.velocity.Y *= 0.1f;
                }
            }

            if ((Main.dayTime || Main.player[NPC.target].dead))
            {
                NPC.velocity.Y -= num2 * 2f;
                if (NPC.timeLeft > 10)
                {
                    NPC.timeLeft = 10;
                }
            }

            if (((NPC.velocity.X > 0f && NPC.oldVelocity.X < 0f) || (NPC.velocity.X < 0f && NPC.oldVelocity.X > 0f) || (NPC.velocity.Y > 0f && NPC.oldVelocity.Y < 0f) || (NPC.velocity.Y < 0f && NPC.oldVelocity.Y > 0f)) && !NPC.justHit)
            {
                NPC.netUpdate = true;
            }
        }

        public override void FindFrame(int frameHeight)
        {
            if ((NPC.frameCounter += 1f) >= 8f)
            {
                NPC.frame.Y += frameHeight;
                NPC.frameCounter = 0f;
            }
            if (NPC.frame.Y >= frameHeight * Main.npcFrameCount[NPC.type])
            {
                NPC.frame.Y = 0;
            }
        }

        public override void OnHitByProjectile(Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            OnHitAndDeathStuff(hit, damageDone);
        }

        public override void OnHitByItem(Player player, Item item, NPC.HitInfo hit, int damageDone)
        {
            OnHitAndDeathStuff(hit, damageDone);
        }

        public void OnHitAndDeathStuff(NPC.HitInfo hit, int damageDone)
        {
            if (NPC.life > 0)
            {
                for (int num57 = 0; num57 < damageDone / NPC.lifeMax * 50.0; num57++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1);
                }
            }
            else
            {
                for (int num58 = 0; num58 < 16; num58++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection / 2, -2);
                }
            }
        }
    }
}
