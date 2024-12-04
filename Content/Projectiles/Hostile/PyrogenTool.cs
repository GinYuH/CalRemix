using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalRemix.Content.NPCs.Bosses.Pyrogen;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Hostile
{
    public class PyrogenTool : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pyro Tool");
            Main.projFrames[Projectile.type] = 6;
        }
        public override string Texture => "CalRemix/Content/NPCs/Bosses/Pyrogen/PyrogenShield";

        public override void SetDefaults()
        {
            Projectile.Calamity().DealsDefenseDamage = true;
            Projectile.width = 20;
            Projectile.height = 10;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 240; //killed by hostile projectiles being despawned, as opposed to on timer
            CooldownSlot = ImmunityCooldownID.Bosses;
        }
        public override void AI()
        {
            if (Projectile.localAI[0] == 0)
            {
                Projectile.frame = Main.rand.Next(0, 6);
                Projectile.localAI[0] = 1;
            }

            Lighting.AddLight(Projectile.Center, 1f, 1.6f, 0f);
            if (!Main.dedServ)
            {
                if (Main.rand.NextBool(10))
                {
                    Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f);
                }
            }

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<Dragonfire>(), 120);
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f);
                d.velocity = new Vector2(Main.rand.Next(-4, 5), Main.rand.Next(-4, 5));
            }
        }

        public override bool PreDraw(ref Color drawColor)
        {
            Texture2D sprite = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 npcOffset = Projectile.Center - Main.screenPosition;
            Main.EntitySpriteDraw(PyrogenShield.BloomTexture.Value, npcOffset, sprite.Frame(1, Main.projFrames[Type], 0, Projectile.frame), Color.White with { A = 0 }, Projectile.rotation, new Vector2(sprite.Width / 2, sprite.Height / 12), 1f, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(sprite, npcOffset, sprite.Frame(1, Main.projFrames[Type], 0, Projectile.frame), Projectile.GetAlpha(drawColor), Projectile.rotation, new Vector2(sprite.Width / 2, sprite.Height / 12), 1f, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(PyrogenShield.Glow.Value, npcOffset, sprite.Frame(1, Main.projFrames[Type], 0, Projectile.frame), Color.White, Projectile.rotation, new Vector2(sprite.Width / 2, sprite.Height / 12), 1f, SpriteEffects.None, 0);
            return false;
        }
    }
}