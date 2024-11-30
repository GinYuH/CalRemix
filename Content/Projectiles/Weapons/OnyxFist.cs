using CalamityMod;
using CalRemix.Content.Buffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class OnyxFist : ModProjectile
    {
        public ref float Timer => ref Projectile.ai[0];
        public Player Owner => Main.player[Projectile.owner];
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionSacrificable[Type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 48;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.minion = true;
            Projectile.minionSlots = 1;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 22;
            Projectile.tileCollide = false;
        }
        public override void AI()
        {
            Projectile.CheckMinionCondition(ModContent.BuffType<OnyxFistBuff>(), Owner.GetModPlayer<CalRemixPlayer>().onyxFist);
            Projectile.MinionAntiClump();
            Projectile.rotation = Projectile.velocity.ToRotation();
            NPC target = Projectile.Center.MinionHoming(1600, Owner, false);
            if (target != null && target.CanBeChasedBy())
            {
                Timer++;
                if (Timer <= 60)
                    Timer = 0;
                if (Timer % 2 == 0)
                    Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.ShadowbeamStaff, Scale: 1f + Main.rand.NextFloat());
                Vector2 vector = Projectile.SafeDirectionTo(target.Center, Vector2.UnitY);
                Projectile.velocity = (Projectile.velocity * 20f + vector * 12f) / 21f;
            }
            else
            {
                if (Vector2.Distance(Owner.Center, Projectile.Center) > 2400)
                    Projectile.Center = Owner.Center;
                if (Vector2.Distance(Owner.Center, Projectile.Center) > 64)
                {
                    Vector2 vector = Projectile.SafeDirectionTo(Owner.Center, Vector2.UnitY);
                    Projectile.velocity = (Projectile.velocity * 20f + vector * 12f) / 21f;
                }
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.myPlayer == Owner.whoAmI)
                SoundEngine.PlaySound(SoundID.DD2_WyvernDiveDown);
            Projectile.Center = Owner.Center;
            Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(135f));
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            SpriteEffects spriteEffects = Projectile.direction > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically;
            Vector2 centered = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
            Main.EntitySpriteDraw(texture, centered, null, new Color(255, 255, 255, Projectile.alpha), Projectile.rotation, texture.Size() / 2, Projectile.scale, spriteEffects, 0);
            return false;
        }
    }
}