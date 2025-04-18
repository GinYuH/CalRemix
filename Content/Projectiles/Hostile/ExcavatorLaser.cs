using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Hostile
{
    public class ExcavatorLaser : ModProjectile
    {
        public float TelegraphDelay
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public NPC NPCToStickTo => Main.npc.IndexInRange((int)Projectile.ai[1]) && Main.npc[(int)Projectile.ai[1]].active ? Main.npc[(int)Projectile.ai[1]] : null;

        public Vector2 OriginalVelocity;

        public const float TelegraphTotalTime = 60f;

        public const float TelegraphFadeTime = 10f;
        
        public const float TelegraphWidth = 4200f;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Wulfrum Beam");
            ProjectileID.Sets.DrawScreenCheckFluff[Projectile.type] = 10000;
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.Opacity = 0f;
            Projectile.penetrate = -1;
            Projectile.MaxUpdates = 3;
            Projectile.timeLeft = 780;
            CooldownSlot = ImmunityCooldownID.Bosses;
            Projectile.Calamity().DealsDefenseDamage = true;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteVector2(OriginalVelocity);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            OriginalVelocity = reader.ReadVector2();
        }

        public override void AI()
        {
            // Vanish if the NPC to stick to is not present.
            if (NPCToStickTo is null)
            {
                Projectile.Kill();
                return;
            }

            // Determine the relative opacities for each player based on their distance.
            // This has a lower bound of 0.35 to prevent the laser from going completely invisible and players getting hit by cheap shots.
            if (Projectile.localAI[0] == 0f)
            {
                Projectile.localAI[0] = 1f;
                Projectile.netUpdate = true;
            }
            // Fade in after telegraphs have faded.
            if (TelegraphDelay > TelegraphTotalTime)
            {
                // Fade in.
                Projectile.Opacity = MathHelper.Clamp(Projectile.Opacity + 0.08f, 0f, 1f);

                // If an old velocity is in reserve, set the true velocity to it and make it as "taken" by setting it to <0,0>
                if (OriginalVelocity != Vector2.Zero)
                {
                    Projectile.velocity = OriginalVelocity;
                    OriginalVelocity = Vector2.Zero;
                    
                    Projectile.netUpdate = true;
                }
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            }
            
            // Otherwise, be sure to save the velocity the projectile started with. It will be set again when the telegraph is over.
            else if (OriginalVelocity == Vector2.Zero)
            {
                OriginalVelocity = Projectile.velocity;
                Projectile.velocity = Vector2.Zero;
                Projectile.netUpdate = true;
                Projectile.rotation = OriginalVelocity.ToRotation() + MathHelper.PiOver2;
            }
            else
                Projectile.Center = NPCToStickTo.Center;

            if (Projectile.FinalExtraUpdate())
                TelegraphDelay++;
        }

        public override bool CanHitPlayer(Player target) => TelegraphDelay > TelegraphTotalTime;

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 255, 255, Projectile.alpha);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (TelegraphDelay >= TelegraphTotalTime)
                return true;

            Texture2D laserTelegraph = ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/LaserWallTelegraphBeam").Value;

            float laserWidth = 2f;
            if (TelegraphDelay < TelegraphFadeTime)
                laserWidth = Utils.Remap(TelegraphDelay, 0f, TelegraphFadeTime, 0f, 2f);
            if (TelegraphDelay > TelegraphTotalTime - TelegraphFadeTime)
                laserWidth = Utils.Remap(TelegraphDelay - TelegraphTotalTime, -TelegraphFadeTime, 0f, 2f, 0f);

            Vector2 scaleInner = new(TelegraphWidth / laserTelegraph.Width, laserWidth);
            Vector2 origin = laserTelegraph.Size() * new Vector2(0f, 0.5f);
            Vector2 scaleOuter = scaleInner * new Vector2(1f, 1.6f);

            // Iterate through lime and yellow.
            Color colorOuter = Color.Lerp(Color.Lime, Color.Yellow, TelegraphDelay / TelegraphTotalTime * 2f % 1f);
            Color colorInner = Color.Lerp(colorOuter, Color.White, 0.75f);

            colorOuter *= 0.7f;
            colorInner *= 0.7f;
            colorInner.A = 50;

            Main.EntitySpriteDraw(laserTelegraph, Projectile.Center - Main.screenPosition, null, colorOuter, OriginalVelocity.ToRotation(), origin, scaleOuter, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(laserTelegraph, Projectile.Center - Main.screenPosition, null, colorInner, OriginalVelocity.ToRotation(), origin, scaleInner, SpriteEffects.None, 0);
            return false;
        }
    }
}
