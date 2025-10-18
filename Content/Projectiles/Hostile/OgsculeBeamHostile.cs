using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Projectiles.BaseProjectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Hostile;




public class OgsculeBeamHostile : BaseLaserbeamProjectile
{

    float laserscale = 0.8f;
    bool playedsound = false;

        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

    public override float MaxScale => laserscale;
    public override float MaxLaserLength => 2000f;
    public override float Lifetime => 660;
    public override Color LaserOverlayColor => Color.Red;
    public override Color LightCastColor => new Color(1, 0, 0);

    public int NPCOwner = -1;

    public override Texture2D LaserBeginTexture => ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/Lasers/UltimaRayStart").Value;
    public override Texture2D LaserMiddleTexture => ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/Lasers/UltimaRayMid").Value;
    public override Texture2D LaserEndTexture => ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/Lasers/UltimaRayEnd").Value;
    public override void SetStaticDefaults()
    {
        // DisplayName.SetDefault("Ogscule Beam");
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
            Projectile.Center = npc.Center -Vector2.UnitY * 37;
        }
        else
        {
            Projectile.Kill();
        }
        return true;
    }

    public override bool ShouldUpdatePosition() => false;


    public override bool PreDraw(ref Color lightColor)
    {


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
            while (incrementalBodyLength + 1f < lenghtofmidlaser - laserOffset)
            {
                //The middle (for real)
                Main.EntitySpriteDraw(LaserMiddleTexture, centerr - Main.screenPosition, middlesquare, LaserOverlayColor, Projectile.rotation, LaserMiddleTexture.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0);
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

        return false;
    }
    //Shitcode
    public override void SendExtraAI(BinaryWriter writer)
    {
        writer.Write(playedsound);
    }

    public override void ReceiveExtraAI(BinaryReader reader)
    {
        playedsound = reader.ReadBoolean();
    }

    public override void OnHitPlayer(Player target, Player.HurtInfo info)
    {
        target.AddBuff(ModContent.BuffType<VulnerabilityHex>(), 120);
    }




}

