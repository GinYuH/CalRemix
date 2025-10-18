using CalamityMod.NPCs.ExoMechs.Ares;
using CalamityMod.Projectiles.BaseProjectiles;
using CalRemix.Content.NPCs.PandemicPanic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Hostile;




public class MaserDeathray : BaseLaserbeamProjectile
{

    float laserscale = 0.8f;

    public override float MaxScale => laserscale;
    public override float MaxLaserLength => 2000f;
    public override float Lifetime => 300;
    public override Color LaserOverlayColor => Color.Red;
    public override Color LightCastColor => new Color(1, 0, 0);

    public int NPCOwner = -1;

    public override Texture2D LaserBeginTexture => ModContent.Request<Texture2D>(Texture).Value;
    public override Texture2D LaserMiddleTexture => ModContent.Request<Texture2D>(Texture).Value;
    public override Texture2D LaserEndTexture => ModContent.Request<Texture2D>(Texture).Value;
    public override void SetStaticDefaults()
    {
        // DisplayName.SetDefault("Maser Beam");
        ProjectileID.Sets.DrawScreenCheckFluff[Projectile.type] = 10000;
    }
    public override void SetDefaults()
    {

        Projectile.width = Projectile.height = 30;
        Projectile.penetrate = -1;
        //Projectile.alpha = 100;
        Projectile.tileCollide = false;
        Projectile.hostile = true;
        Projectile.timeLeft = (int)Lifetime;

    }


    //Animeme


    public override bool PreAI()
    {
        NPC npc = Main.npc[NPCOwner];
        if (npc != null && npc.active)
        {
            Projectile.Center = npc.Center - Vector2.UnitY * 100 + Projectile.velocity * 100;
        }
        else
        {
            Projectile.Kill();
        }
        if (Projectile.timeLeft == 200)
        {
            SoundEngine.PlaySound(AresLaserCannon.LaserbeamShootSound, Projectile.Center);
        }
        else if (Projectile.timeLeft > 200)
        {
            laserscale = 0.5f;
        }
        else
        {
            laserscale = MathHelper.Lerp(laserscale, 2f, 0.05f);
        }
        if (Projectile.timeLeft <= 200)
            foreach (NPC n in PandemicPanic.ActiveNPCs)
        {
            if (n == null)
                continue;
            if (!n.active)
                continue;
            if (n.life <= 0)
                continue;
            if (n.type == ModContent.NPCType<DendtritiatorArm>())
                continue;
            if (PandemicPanic.DefenderNPCs.Contains(n.type))
            {
                float collisionPoint = 0f;
                Rectangle targetHitbox = n.getRect();
                if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), base.Projectile.Center, base.Projectile.Center + base.Projectile.velocity * LaserLength, base.Projectile.Size.Length() * base.Projectile.scale, ref collisionPoint))
                {
                    n.SimpleStrikeNPC(Projectile.damage * (Main.expertMode ? 2 : 4), n.direction, false);
                    n.GetGlobalNPC<PandemicPanicNPC>().hitCooldown = 20;
                    if (n.life <= 0)
                    {
                        PandemicPanic.DefendersKilled++;
                        if (n.type == ModContent.NPCType<Dendritiator>())
                        {
                            PandemicPanic.DefendersKilled += 4;
                        }
                    }
                }
            }
        }
        return true;
    }

    public override bool ShouldUpdatePosition() => false;


    public override bool PreDraw(ref Color lightColor)
    {

        Main.spriteBatch.End();
        Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
        Rectangle beginsquare = LaserBeginTexture.Frame(1, Main.projFrames[Projectile.type], 0, Projectile.frame);
        Rectangle middlesquare = LaserMiddleTexture.Frame(1, Main.projFrames[Projectile.type], 0, Projectile.frame);
        Rectangle endsquare = LaserEndTexture.Frame(1, Main.projFrames[Projectile.type], 0, Projectile.frame);
        float lenghtofmidlaser = LaserLength + middlesquare.Height;
        Vector2 centerr = Projectile.Center;

        //The beginning...
        Main.EntitySpriteDraw(LaserBeginTexture, Projectile.Center - Main.screenPosition, beginsquare, LaserOverlayColor, Projectile.rotation, LaserBeginTexture.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);
        //The middle!
        if (lenghtofmidlaser > 0f)
        {
            //Size variables
            float laserOffset = middlesquare.Height * Projectile.scale;
            float incrementalBodyLength = 0f;
            //Draw textures until the end of the laser
            Rectangle magic = new Rectangle(0, 0, 2, (int)(middlesquare.Height));
            while (incrementalBodyLength + 1f < lenghtofmidlaser - laserOffset)
            {
                //The middle (for real)
                Main.EntitySpriteDraw(LaserMiddleTexture, centerr - Main.screenPosition, middlesquare, LaserOverlayColor, Projectile.rotation, LaserMiddleTexture.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0);
                Main.EntitySpriteDraw(TextureAssets.MagicPixel.Value, centerr - Main.screenPosition, magic, Color.White, Projectile.rotation, new Vector2(magic.Width, magic.Height) * 0.5f, Projectile.scale, SpriteEffects.None, 0);
                //Prepare for the next laser segment (woo)
                incrementalBodyLength += laserOffset;
                centerr += Projectile.velocity * laserOffset;
                middlesquare.Y += LaserMiddleTexture.Height / Main.projFrames[Projectile.type];
                if (middlesquare.Y + middlesquare.Height > LaserMiddleTexture.Height)
                {
                    middlesquare.Y = 0;
                }
            }
        }
        //The end.
        Main.EntitySpriteDraw(LaserEndTexture, centerr - Main.screenPosition, endsquare, LaserOverlayColor, Projectile.rotation, LaserEndTexture.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);

        Main.spriteBatch.End();
        Main.spriteBatch.Begin();
        return false;
    }

    public override bool? CanDamage()
    {
        return Projectile.timeLeft < 200;
    }



}

