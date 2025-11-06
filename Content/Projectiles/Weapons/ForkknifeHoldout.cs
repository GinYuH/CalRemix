using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Projectiles.Rogue;
using CalRemix.Content.Items.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class ForkknifeHoldout : ModProjectile
    {
        public ref float Timer => ref Projectile.ai[0];
        public Player Owner => Main.player[Projectile.owner];

        public override void SetDefaults()
        {
            Projectile.width = 80;
            Projectile.height = 80;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 360;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 16;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
        }
        public override void AI()
        {
            if (Owner.channel)
                Projectile.timeLeft = 2;
            else
                Projectile.Kill();
            Timer++;

            float pullback = 10;
            float wait = 10 + pullback;
            float stab = 6 + wait;
            float waittwo = 4 + stab;
            float localTimer = Timer % stab;
            float totalStabs = 3;
            float spinTime = 60;

            Vector2 backPos = Owner.Center + Main.MouseWorld.DirectionTo(Owner.Center) * 30;
            Vector2 frontPos = Owner.Center + Owner.Center.DirectionTo(Main.MouseWorld) * 100;
            Vector2 finalPos = Owner.Center;

            bool rotOverride = false;            

            if (Timer < totalStabs * waittwo - 2)
            {
                if (localTimer < pullback)
                {
                    Vector2 frStart = Timer <= pullback ? finalPos : frontPos;
                    finalPos = Vector2.Lerp(frStart, backPos, Utils.GetLerpValue(0, pullback, localTimer, true));
                }
                else if (localTimer < wait)
                {
                    finalPos = backPos;
                }
                else if (localTimer < stab)
                {
                    if (localTimer == wait)
                    {
                        SoundEngine.PlaySound(new SoundStyle("CalRemix/Assets/Sounds/SwiftSlice"), Projectile.Center);
                    }
                    finalPos = Vector2.Lerp(backPos, frontPos, Utils.GetLerpValue(wait, stab, localTimer, true));
                }
                else if (localTimer < waittwo)
                {
                    finalPos = frontPos;
                }
            }
            else
            {
                if (Projectile.ai[1] % 10 == 0)
                {
                    SoundEngine.PlaySound(new SoundStyle("CalRemix/Assets/Sounds/SwiftSlice") with { Pitch = -0.4f }, Projectile.Center);
                }
                Projectile.ai[1]++;
                if (Projectile.ai[1] > spinTime)
                {
                    Projectile.ai[1] = 0;
                    Timer = 0;
                }
                else
                {
                    Projectile.rotation += Owner.direction * 0.5f;
                    float stopGrow = 20;
                    float startShrink = 40;
                    rotOverride = true;
                    if (Projectile.ai[1] < stopGrow)
                        Projectile.scale = MathHelper.Lerp(1, 3, Utils.GetLerpValue(0, stopGrow, Projectile.ai[1], true));
                    else if (Projectile.ai[1] < startShrink)
                        Projectile.scale = 3;
                    else
                        Projectile.scale = MathHelper.Lerp(3, 1, Utils.GetLerpValue(startShrink, spinTime - 1, Projectile.ai[1], true));
                }
            }

            Projectile.width = Projectile.height = (int)(ContentSamples.ProjectilesByType[Type].width * Projectile.scale);

            Projectile.Center = finalPos;
            Projectile.spriteDirection = Projectile.direction = Owner.DirectionTo(Main.MouseWorld).X.DirectionalSign();
            if (!rotOverride)
                Projectile.rotation = Owner.DirectionTo(Main.MouseWorld).ToRotation();

        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Rectangle sourceRectangle = new Rectangle(0, 0, texture.Width, texture.Height);
            Vector2 origin = sourceRectangle.Size() / 2f;
            Main.spriteBatch.Draw(texture,
                Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
                sourceRectangle, lightColor, Projectile.rotation, origin, Projectile.scale, Projectile.spriteDirection == -1 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0f);
            return false;
        }
    }
}


