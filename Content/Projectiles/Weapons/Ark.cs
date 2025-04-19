using CalamityMod;
using CalamityMod.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class Ark : ModProjectile
    {
        public override string Texture => "CalRemix/Content/Projectiles/Weapons/Ark1";

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Ark");
            Main.projFrames[Projectile.type] = 5;
        }
        public override void SetDefaults()
        {
            Projectile.width = 640;
            Projectile.height = 640;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 120;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            Player p = Main.player[Projectile.owner];
            if (p != null && p.active && p.controlUseItem && !p.CCed)
            {
                Projectile.Center = Main.player[Projectile.owner].Center;
                Projectile.velocity = Vector2.Zero;
                Projectile.timeLeft = 22;
            }
            else
            {
                Projectile.active = false;
                return;
            }
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 0)
            {
                Projectile.ai[0]++;
                if (Projectile.ai[0] > 5)
                {
                    Projectile.ai[0] = 0;
                    Projectile.frame++;
                }    
                if (Projectile.frame > 4 && Projectile.ai[1] == 0)
                {
                    Projectile.frame = 0;
                    Projectile.ai[1] = 1;
                    SoundEngine.PlaySound(BetterSoundID.ItemDeathSickle, Projectile.Center);
                    int count = 8;
                    for (int i = 0; i < count; i++)
                    {
                        float div = MathHelper.TwoPi / (float)count;
                        int propa = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.DirectionTo(Main.MouseWorld).RotatedBy(div * i) * 16, ModContent.ProjectileType<CalamityMod.Projectiles.Melee.EonBolt>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0, MathHelper.Pi * 0.022f);
                        Main.projectile[propa].timeLeft = 150;
                    }
                }
                if (Projectile.frame > 5 && Projectile.ai[1] == 1)
                {
                    Projectile.frame = 0;
                    Projectile.ai[1] = 0;
                    SoundEngine.PlaySound(BetterSoundID.ItemDeathSickle with { Pitch = 0.2f }, Projectile.Center);
                }
                Projectile.frameCounter = 0;
            }
            Projectile.spriteDirection = Projectile.direction = p.direction;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture2D = TextureAssets.Projectile[Type].Value;
            Texture2D alt = ModContent.Request<Texture2D>("CalRemix/Content/Projectiles/Weapons/Ark2").Value;

            bool useAlt = Projectile.ai[1] == 1;

            Main.EntitySpriteDraw(useAlt ? alt : texture2D, Projectile.Center - Main.screenPosition, useAlt ? alt.Frame(6, 6, (int)Projectile.ai[0], Projectile.frame) : texture2D.Frame(6, 5, (int)Projectile.ai[0], Projectile.frame), lightColor, Projectile.rotation, useAlt ? new Vector2(alt.Width / 12, alt.Height / 12) : new Vector2(texture2D.Width / 12, texture2D.Height / 10), Projectile.scale, Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            return false;
        }
    }
}