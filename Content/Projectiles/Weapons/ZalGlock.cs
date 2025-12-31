using CalamityMod.CalPlayer;
using CalamityMod.NPCs.NormalNPCs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.Projectiles;
using Terraria.Audio;
using CalamityMod.Systems.Collections;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class ZalGlock : ModProjectile, ILocalizedModType
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

        public override void SetDefaults()
        {
            Projectile.width = 6;
            Projectile.height = 6;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 120;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.extraUpdates = 1;
            if (CalRemixAddon.Thorium != null)
            {
                Projectile.DamageType = CalRemixAddon.Thorium.Find<DamageClass>("BardDamage");
            }
        }

        public override void AI()
        {
            if (Projectile.ai[0] == 0)
            {
                SoundEngine.PlaySound(new SoundStyle("CalRemix/Assets/Sounds/Ocarina") with { Pitch = MathHelper.Lerp(-0.5f, 1, Utils.GetLerpValue(0, 1000, Main.MouseWorld.Distance(Projectile.Center), true))});
                Projectile.ai[0] = 1;
            }
            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] > 3f)
            {
                for (int num134 = 0; num134 < 10; num134++)
                {
                    float x = Projectile.position.X - Projectile.velocity.X / 10f * (float)num134;
                    float y = Projectile.position.Y - Projectile.velocity.Y / 10f * (float)num134;
                    int dust = Dust.NewDust(new Vector2(x, y), 1, 1, DustID.RainbowTorch, 0f, 0f, 0, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), 1f);
                    Main.dust[dust].alpha = Projectile.alpha;
                    Main.dust[dust].position.X = x;
                    Main.dust[dust].position.Y = y;
                    Main.dust[dust].position = Main.dust[dust].position + Vector2.UnitY.RotatedBy(Projectile.localAI[0]) * 10;
                    Main.dust[dust].velocity *= 0f;
                    Main.dust[dust].noGravity = true;
                }
            }
            CalamityUtils.HomeInOnNPC(Projectile, true, 1000, 22, 22f);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (target.type == NPCID.TargetDummy)
            {
                return;
            }

            int randAmt = (CalamityPlayer.areThereAnyDamnBosses || CalamityNPCTypeSets.AquaticScourge.Contains(target.type)) ? 8 : 10;
            int nullBuff = Main.rand.Next(randAmt);
            //if (!target.boss)
            {
                switch (nullBuff)
                {
                    case 0:
                        target.damage += 20;
                        break;
                    case 1:
                        target.damage -= 20;
                        break;
                    case 2:
                        target.knockBackResist = 0f;
                        break;
                    case 3:
                        target.knockBackResist = 1f;
                        break;
                    case 4:
                        target.defense += 10;
                        break;
                    case 5:
                        target.defense -= 10;
                        break;
                    case 6:
                        target.velocity.Y = Main.rand.NextBool() ? 30f : -30f;
                        break;
                    case 7:
                        target.velocity.X = Main.rand.NextBool() ? 30f : -30f;
                        break;
                    case 8:
                        target.scale *= 5f;
                        break;
                    case 9:
                        target.scale *= 0.1f;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
