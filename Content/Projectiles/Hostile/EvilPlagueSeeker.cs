using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalRemix.Content.NPCs.Minibosses;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Hostile
{
	public class EvilPlagueSeeker : ModProjectile
	{
        public ref float Owner => ref Projectile.ai[0];
        public bool activated = false;
        public override void SetStaticDefaults() 
        {
			// DisplayName.SetDefault("Plague Seeker");
		}
		public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.hostile = true;
            Projectile.timeLeft = 480;
        }
		public override void AI()
		{
            if (!Main.dedServ)
            {
                for (int i = 0; i < 2; i++)
                {
                    Dust dust = Dust.NewDustDirect(Projectile.position, 0, 0, DustID.TerraBlade, 0f, 0f);
                    dust.noGravity = true;
                    dust.velocity *= 0;
                }
            }
            if (Main.npc[(int)Owner] == null || !((int)Owner).WithinBounds(Main.maxNPCs))
            {
                Owner = NPC.FindFirstNPC(ModContent.NPCType<PlagueEmperor>());
                Projectile.velocity = Projectile.velocity.RotatedByRandom(Math.PI / 6);
                return;
            }
            if (!Check())
            {
                Projectile.velocity = Projectile.velocity.RotatedByRandom(Math.PI / 6);
                return;
            }
            int index = Player.FindClosest(Projectile.Center, 1, 1);
            if (Main.player[index] == null || !index.WithinBounds(Main.maxPlayers))
            {
                Projectile.velocity = Projectile.velocity.RotatedByRandom(Math.PI / 6);
                return;
            }
            else if (Main.player[index].dead || !Main.player[index].active)
            {
                Projectile.velocity = Projectile.velocity.RotatedByRandom(Math.PI / 6);
                return;
            }
            activated = true;
            Player target = Main.player[index];
            if (Projectile.timeLeft < 450)
            {
                if (Projectile.WithinRange(target.Center, 240f) && Collision.CanHit(Projectile.Center, 1, 1, target.Center, 1, 1))
                {
                    Projectile.extraUpdates = Projectile.Calamity().defExtraUpdates + 1;
                    Vector2 vector = (target.Center - Projectile.Center).SafeNormalize(Vector2.UnitY);
                    Projectile.velocity = (Projectile.velocity * 20f + vector * 12f) / (20f + 1f);
                }
            }
        }
        public override bool CanHitPlayer(Player target)
        {
            return activated;
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<Plague>(), 120);
            Projectile.Kill();
        }
        private bool Check()
        {
            if (Main.npc[(int)Owner].type != ModContent.NPCType<PlagueEmperor>())
                return false;
            PlagueEmperor emperor = Main.npc[(int)Owner].ModNPC as PlagueEmperor;
            return emperor.activated;
        }
    }
}