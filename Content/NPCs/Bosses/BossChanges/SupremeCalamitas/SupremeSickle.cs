using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using Terraria.Graphics.Shaders;
using CalamityMod.Graphics.Primitives;
using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;

namespace CalRemix.Content.NPCs.Bosses.BossChanges.SupremeCalamitas
{
    public class SupremeSickle : ModProjectile
    {
        public ref float IdealPosX => ref Projectile.ai[0];
        public ref float IdealPosY => ref Projectile.ai[1];
        public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 60;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.aiStyle = -1;
        }
        public override void AI()
        {
            Projectile.rotation += 0.75f;

            Projectile.velocity += new Vector2(IdealPosX, IdealPosY) * 0.5f;
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<VulnerabilityHex>(), 120);
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f);
                d.velocity = new Vector2(Main.rand.Next(-4, 5), Main.rand.Next(-4, 5));
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return true;
        }
    }
}
