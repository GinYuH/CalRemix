using CalamityMod.Items.Weapons.Ranged;
using CalRemix.Content.Items.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Hostile
{
    public class MercuryBlob : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 600;
        }
        public override void AI()
        {
            if (Projectile.ai[2] == 0)
            {
                if (Projectile.direction == 0)
                {
                    Projectile.direction = Main.rand.NextBool().ToDirectionInt();
                }
                Projectile.rotation += 0.03f * (Projectile.whoAmI % 2 == 0).ToDirectionInt();

                NPC n = Main.npc[(int)Projectile.ai[0]];
                if (!n.active || n.life < 0)
                {
                    Projectile.Kill();
                }
                else
                {
                    Projectile.ai[1]++;
                    if (Projectile.ai[1] == 120)
                    {
                        SoundEngine.PlaySound(GildedGauntlet.RocketSound with { Volume = 0.8f }, Projectile.Center);
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            Projectile.NewProjectile(n.GetSource_FromThis(), n.Center, n.Center.DirectionTo(Projectile.Center) * 35, ModContent.ProjectileType<MercuryRocket>(), Projectile.damage, 1, ai0: Projectile.whoAmI);
                        }
                    }
                }
            }
            else
            {
                Projectile.ai[1]++;
                Projectile.alpha = 255;
                if (Projectile.ai[1] > 10)
                {
                    Projectile.active = false;
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Type].Value;
            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, tex.Size() / 2, Vector2.One * Projectile.scale + 0.2f * new Vector2(MathF.Cos(Main.GlobalTimeWrappedHourly * 5), MathF.Sin(Main.GlobalTimeWrappedHourly * 5)), Projectile.direction == 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally);
            return false;
        }

        public override bool CanHitPlayer(Player target)
        {
            return false;
        }
    }
}