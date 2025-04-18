using CalamityMod;
using CalamityMod.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
	public class SkeeterBullet : ModProjectile
	{
		public override void SetStaticDefaults() 
        {
			// DisplayName.SetDefault("Sunskater");
            Main.projFrames[Projectile.type] = 4;
        }
		public override void SetDefaults() 
        {
            Projectile.width = 8;
			Projectile.height = 8;
			Projectile.aiStyle = 1;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Ranged; 
			Projectile.penetrate = 1;
			Projectile.timeLeft = 120;
			Projectile.ignoreWater = false;
			Projectile.tileCollide = true;
            AIType = ProjectileID.Bullet;
		}
        public override void OnSpawn(IEntitySource source)
        {
            SoundEngine.PlaySound(SoundID.NPCHit51);
            NPC npc = Projectile.Center.MinionHoming(1750f, Main.player[Projectile.owner], false);
            if (npc != null)
            {
                Projectile.velocity = npc.velocity + Projectile.DirectionTo(npc.Center) * 20f;
            }
        }
        public override void AI()
		{
            Projectile.frameCounter++;
            Projectile.frame = Projectile.frameCounter;
            if (Projectile.frameCounter >= 3)
            {
                Projectile.frameCounter = 0;
            }
            Projectile.spriteDirection = Projectile.direction;
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item14);
            for (int i = 0; i < Main.rand.Next(2, 4); i++)
            {
                int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, -Vector2.Normalize(Projectile.oldVelocity).RotatedByRandom(MathHelper.ToRadians(180)), ModContent.ProjectileType<InfernalKrisCinder>(), Projectile.damage / 4, Projectile.knockBack, Projectile.owner);
                Main.projectile[proj].DamageType = DamageClass.Ranged;
                Main.projectile[proj].Name = "Cinder Shard";
            }
        }
    }
}