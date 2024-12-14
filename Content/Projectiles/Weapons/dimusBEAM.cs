using CalamityMod.Projectiles.BaseProjectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons;




public class dimusBEAM : BaseLaserbeamProjectile //I can't believe Jharim would use a base projectile from Calamity, literal thievery! ban!!!!
{

    float laserscale = 2f;
    bool playedsound = false;

    public override string Texture => "CalRemix/Content/Projectiles/Weapons/dimusBEAM";

    public override float MaxScale => laserscale;
    public override float MaxLaserLength => 1500f;
    public override float Lifetime => 30;
    public override Color LaserOverlayColor => Color.White;
    public override Color LightCastColor => LaserOverlayColor;

    public override Texture2D LaserBeginTexture => ModContent.Request<Texture2D>("CalRemix/Content/Projectiles/Weapons/dimusBEAMstart").Value;
    public override Texture2D LaserMiddleTexture => ModContent.Request<Texture2D>("CalRemix/Content/Projectiles/Weapons/dimusBEAM").Value;
    public override Texture2D LaserEndTexture => ModContent.Request<Texture2D>("CalRemix/Content/Projectiles/Weapons/dimusBEAMend").Value;
    public override void SetStaticDefaults()
    {
        DisplayName.SetDefault("Exodimus Beam");
        Main.projFrames[Projectile.type] = 5;
        ProjectileID.Sets.DrawScreenCheckFluff[Projectile.type] = 10000;
    }
    public override void SetDefaults()
    {

        Projectile.width = Projectile.height = 119;
        Projectile.penetrate = -1;
        //Projectile.alpha = 100;
        Projectile.tileCollide = false;
        Projectile.friendly = true;

     
        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = 60;
    }


    //Animeme


    public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers fuckyou)
    {
        Mod calamity = CalRemix.CalMod;
        target.AddBuff(calamity.Find<ModBuff>("HolyFlames").Type, 150);
        target.AddBuff(BuffID.Frostburn, 150);
        target.AddBuff(BuffID.OnFire, 150);
    }

    public override void PostAI()
    {

    }

    public override bool ShouldUpdatePosition() => false;

    //When do we get an animated laser field tbh
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






}

