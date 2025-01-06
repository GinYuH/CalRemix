using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using System.IO;
using CalRemix.Content.Buffs;
using CalamityMod.CalPlayer;
using CalRemix.Content.NPCs.Bosses.BossScule;
using Terraria.Localization;

namespace CalRemix.Content.Projectiles.Hostile
{
    public class CalamityLaser : ModProjectile
    {
        public Vector2 OldVelocity;

        public const float TelegraphTotalTime = 35f;

        public const float TelegraphFadeTime = 5f;

        public const float TelegraphWidth = 2400f;

        public const float FadeTime = 20f;

        public float TelegraphDelay
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }
        public float Mode
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Projectile.type] = 10000;
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            CooldownSlot = 1;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteVector2(OldVelocity);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            OldVelocity = reader.ReadVector2();
        }

        public override void AI()
        {
            if (!NPC.AnyNPCs(ModContent.NPCType<TheCalamity>()))
                Projectile.active = false;
            if (Projectile.localAI[0] == 0f)
            {
                Projectile.localAI[0] = 1f;
                Projectile.netUpdate = true;
            }
            if (TelegraphDelay > 35f)
            {
                foreach (Player p in Main.player)
                {
                    if (p.Hitbox.Intersects(Projectile.Hitbox) && p.Remix().calamitizedHitCooldown <= 0)
                    {
                        p.Remix().calamitizedHitCooldown = 60;
                        if (Main.myPlayer == p.whoAmI && !p.HasBuff<Calamitized>())
                            SoundEngine.PlaySound(CalamityPlayer.DefenseDamageSound, p.Center);
                        if (p.Remix().calamitizedCounter <= 0)
                            p.AddBuff(ModContent.BuffType<Calamitized>(), 1800);
                        else
                        {
                            if (Main.myPlayer == p.whoAmI)
                            {
                                if (p.Remix().calamitizedCounter == 1)
                                    CombatText.NewText(p.getRect(), Color.Red, CalRemixHelper.LocalText("NPCs.TheCalamity.Laser1").Value, true);
                                else
                                    CombatText.NewText(p.getRect(), Color.Red, Language.GetOrRegister("Mods.CalRemix.NPCs.TheCalamity.Laser2").Format(p.Remix().calamitizedCounter), true);
                            }
                            p.Remix().calamitizedCounter--;
                        }
                        Projectile.Kill();
                    }
                }
                if (OldVelocity != Vector2.Zero)
                {
                    Projectile.velocity = OldVelocity;
                    OldVelocity = Vector2.Zero;
                    Projectile.netUpdate = true;
                }

                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            }
            else if (OldVelocity == Vector2.Zero)
            {
                OldVelocity = Projectile.velocity;
                Projectile.velocity = Vector2.Zero;
                Projectile.netUpdate = true;
                Projectile.rotation = OldVelocity.ToRotation() + MathHelper.PiOver2;
            }
            TelegraphDelay++;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 255, 255, 0) * Projectile.Opacity;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (TelegraphDelay >= 35f || Mode == 1)
                return true;
            Texture2D value = ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/LaserWallTelegraphBeam").Value;
            float y = 2f;
            if (TelegraphDelay < 5f)
                y = MathHelper.Lerp(0f, 2f, TelegraphDelay / 5f);
            if (TelegraphDelay > 30f)
                y = MathHelper.Lerp(2f, 0f, (TelegraphDelay - 30f) / 5f);

            Vector2 vector = new Vector2(2400f / (float)value.Width, y);
            Vector2 origin = value.Size() * new Vector2(0f, 0.5f);
            Vector2 scale = vector * new Vector2(1f, 1.6f);
            Color color = Color.Lerp(Color.White, Color.DarkRed, TelegraphDelay / 35f * 2f % 1f);
            Color color2 = Color.Lerp(color, Color.Red, 0.75f);
            color *= 0.7f;
            color2 *= 0.7f;
            Main.EntitySpriteDraw(value, Projectile.Center - Main.screenPosition, null, color2, OldVelocity.ToRotation(), origin, vector, SpriteEffects.None);
            Main.EntitySpriteDraw(value, Projectile.Center - Main.screenPosition, null, color, OldVelocity.ToRotation(), origin, scale, SpriteEffects.None);
            return false;
        }
    }
}
