using CalamityMod;
using CalRemix.NPCs.Bosses.BossScule;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Projectiles.Hostile
{
    public class Darkscule : ModProjectile
    {
        public static readonly SoundStyle Ogscule = new($"{nameof(CalRemix)}/Sounds/Ogscule");
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 480;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.alpha = 0;
        }
        public override void OnSpawn(IEntitySource source)
        {
            SoundEngine.PlaySound(Ogscule, null);
        }
        public override void AI()
        {
            if (!NPC.AnyNPCs(ModContent.NPCType<TheCalamity>()))
                Projectile.active = false;
            Projectile.velocity = Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(10f));
            if (Projectile.alpha < 255)
                Projectile.alpha += 255 / 180;
            if (Projectile.alpha < 255)
                return;
            double angle = Main.rand.NextDouble() * 2d * Math.PI;
            Vector2 offset = new((float)(Math.Sin(angle) * 320), (float)(Math.Cos(angle) * 320));
            Dust dust = Dust.NewDustDirect(Projectile.Center + offset - new Vector2(4, 4), 0, 0, DustID.LifeDrain);
            dust.velocity *= 0;
            foreach (Player p in Main.player)
            {
                if (p.Distance(Projectile.Center) < 320)
                {
                    for (int a = 0; a < 8; a++)
                    {
                        Projectile.NewProjectileDirect(Projectile.GetSource_Death(), Projectile.Center, (Projectile.Center.DirectionTo(p.Center) * 44f).RotatedByRandom(MathHelper.ToRadians(135f)), ModContent.ProjectileType<CalamityLaser>(), 0, 0);
                    }
                    Projectile.Kill();
                }
            }
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 5; i++)
            {
                Dust.NewDustDirect(Projectile.Center, 1, 1, DustID.LifeDrain, Main.rand.Next(-5, 6), Main.rand.Next(-5, 6), Scale: Main.rand.NextFloat() + 1f);
            }
            int index = Player.FindClosest(Projectile.Center, 1, 1);
            if (Main.player[index] == null || !index.WithinBounds(Main.maxPlayers))
                return;
            else if (Main.player[index].dead || !Main.player[index].active)
                return;
            Player target = Main.player[index];
            Projectile.NewProjectileDirect(Projectile.GetSource_Death(), Projectile.Center, Projectile.Center.DirectionTo(target.Center) * 44f, ModContent.ProjectileType<CalamityLaser>(), 0, 0);
            Projectile.NewProjectileDirect(Projectile.GetSource_Death(), Projectile.Center, (Projectile.Center.DirectionTo(target.Center) * 44f).RotatedByRandom(MathHelper.ToRadians(45f)), ModContent.ProjectileType<CalamityLaser>(), 0, 0);
        }
        public override bool CanHitPlayer(Player target)
        {
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 centered = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
            Main.EntitySpriteDraw(texture, centered, null, new Color(255, 0, 0, Projectile.alpha), 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            return false;
        }
    }
}
