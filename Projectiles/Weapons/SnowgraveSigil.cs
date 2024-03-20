using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.Items;
using CalamityMod.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Projectiles.Weapons
{
    public class SnowgraveSigil : ModProjectile
    {
        public override string Texture => "CalRemix/UI/ElementalSystem/Cold";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Snowgrave");
        }
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.timeLeft = 780;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
        }
        public override void AI()
        {
            if (Main.player[Projectile.owner] == null || !Main.player[Projectile.owner].active)
            {
                Projectile.Kill();
                return;
            }
            Projectile.ai[0]++;
            if (Projectile.ai[0] > 180)
            {
                Projectile.ai[1]++;
                for (int j = 0; j < 3; j++)
                {
                    int damageToDo = 10;
                    if (CalamityPlayer.areThereAnyDamnBosses)
                    {
                        damageToDo = Projectile.damage;
                    }
                    else
                    {
                        for (int i = 0; i < Main.maxNPCs; i++)
                        {
                            NPC n = Main.npc[i];
                            if (n.active && !n.boss && n != null && n.type != ModContent.NPCType<CalamityMod.NPCs.NormalNPCs.SuperDummyNPC>())
                            {
                                if (n.lifeMax * (1 + n.Calamity().DR) > damageToDo)
                                {
                                    damageToDo = (int)(n.lifeMax * (2 - n.Calamity().DR));
                                }
                            }
                        }
                    }
                    int projType = Main.rand.NextBool(3) ? ProjectileID.NorthPoleSnowflake : ProjectileID.Blizzard;
                    int p = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.position.X + Main.rand.Next(-200, 200), Projectile.position.Y), new Vector2(0, Main.rand.Next(-40, -20)), projType, damageToDo, 1f, Projectile.owner);
                    Main.projectile[p].DamageType = DamageClass.Magic;
                    Main.projectile[p].tileCollide = false;
                }

                Particle mist = new MediumMistParticle(new Vector2(Projectile.position.X + Main.rand.Next(-200, 200), Projectile.position.Y), new Vector2(0, Main.rand.Next(-180, -140)), new Color(172, 238, 255), new Color(145, 170, 188), Main.rand.NextFloat(1.15f, 1.45f), 245 - Main.rand.Next(50), 0.02f);
                GeneralParticleHandler.SpawnParticle(mist);

                if (Projectile.ai[1] % 2 == 0)
                {
                    Particle mist2 = new MediumMistParticle(new Vector2(Projectile.position.X + Main.rand.Next(-200, 200), Projectile.position.Y), Vector2.Zero, new Color(172, 238, 255), new Color(145, 170, 188), Main.rand.NextFloat(2.15f, 2.45f), 245 - Main.rand.Next(50), 0.02f);
                    GeneralParticleHandler.SpawnParticle(mist2);
                }
            }
            if (Projectile.ai[0] <= 180)
            { 
                if (Projectile.owner == Main.myPlayer)
                {
                    Projectile.position = new Vector2(Main.MouseWorld.X, Main.MouseWorld.Y + 100);
                }
            }
        }

        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            Main.player[Projectile.owner].AddCooldown(SnowgraveCooldown.ID, CalamityUtils.SecondsToFrames(60));
        }
    }
}