using CalamityMod;
using CalamityMod.Buffs.StatDebuffs;
using CalRemix.Content.NPCs.Subworlds.Sealed;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Hostile
{
    public class OmegaReticle : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/DraedonsArsenal/AnomalysNanogunScope";

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.aiStyle = -1;
            Projectile.hostile = true;
            Projectile.alpha = 255;
            Projectile.timeLeft = 36000;
        }

        public override void AI()
        {
            if (!NPC.AnyNPCs(ModContent.NPCType<SkeletronOmega>()))
            {
                Projectile.Kill();
                return;
            }
            int prime = NPC.FindFirstNPC(ModContent.NPCType<SkeletronOmega>());
            if (Main.npc[prime].ai[1] != 1)
            {
                Projectile.Kill();
            }
            Projectile.alpha -= 40;

            Player p = Main.player[(int)Projectile.ai[0]];

            if (!p.active || p.dead)
            {
                Projectile.Kill();
                return;
            }

            if (Projectile.alpha > 0)
            {
                Projectile.rotation += 0.1f;
            }
            else if (Projectile.ai[1] < 100)
            {
                Projectile.Center = Vector2.Lerp(Projectile.Center, p.Center + p.velocity * 5, 0.3f) + Vector2.UnitY.RotatedBy(Projectile.ai[1] * 0.1f + Main.GameUpdateCount * 0.05f) * 20;
            }

            Projectile.ai[1]++;
        }

        public override bool? CanDamage()
        {
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 centered = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
            Main.spriteBatch.Draw(texture, centered, null, Color.Lerp(Color.Orange, Color.Red, Utils.GetLerpValue(0, 100, Projectile.ai[1], true)) * Projectile.Opacity, Projectile.rotation, texture.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}