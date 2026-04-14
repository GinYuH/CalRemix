using CalamityMod;
using CalRemix.Core.Biomes;
using CalRemix.Core.Biomes.Subworlds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.NPCs.Subworlds.OvergrowthRainforest
{
    public class GigamothPupae : ModNPC
    {
        public ref float Timer => ref NPC.ai[0];

        public override void SetStaticDefaults()
        {
            this.HideFromBestiary();
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 0;
            NPC.width = 60;
            NPC.height = 80;
            NPC.defense = 80;
            NPC.lifeMax = 30000;
            NPC.lavaImmune = true;
            NPC.noTileCollide = false;
            NPC.HitSound = BetterSoundID.HitLeechBrainFlesh with { Pitch = -0.3f };
            NPC.DeathSound = BetterSoundID.DeathBrainofCthulhu;
        }

        public override void AI()
        {
            NPC.rotation += NPC.velocity.X * 0.1f;
            if (NPC.direction == 0)
            {
                NPC.spriteDirection = NPC.direction = Main.rand.NextBool().ToDirectionInt();
            }
            Timer++;

            if (Timer >= 300)
            {
                NPC.Transform(ModContent.NPCType<Gigamoth>());
                CalRemixHelper.DustExplosionOutward(NPC.Center, dustID: DustID.GemRuby, speed: Main.rand.NextFloat(10, 20), alpha: 0, color: default, scale: 1);
                SoundEngine.PlaySound(BetterSoundID.DeathBubbleShieldPop, NPC.Center);
                Main.LocalPlayer.Calamity().GeneralScreenShakePower += 10;
            }
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.GreenBlood, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.GreenBlood, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D tex = TextureAssets.Npc[Type].Value;
            Vector2 scale = Vector2.One;
            float animSpeed = Timer * 0.05f;
            scale += new Vector2(MathF.Cos(animSpeed), MathF.Sin(animSpeed)) * MathHelper.Lerp(0, 0.3f, Utils.GetLerpValue(0, 300, Timer, true)) * 0.3f;
            spriteBatch.Draw(tex, NPC.Center - screenPos, null, NPC.GetAlpha(drawColor), NPC.rotation, tex.Size() / 2, NPC.scale * scale, NPC.FlippedEffects(), 0);
            float alpha = MathHelper.Lerp(0, 0.8f, Utils.GetLerpValue(60, 300, Timer, true));
            CalamityUtils.EnterShaderRegion(Main.spriteBatch);
            Color outlineColor = Color.Red;
            Vector3 outlineHSL = Main.rgbToHsl(outlineColor);
            GameShaders.Misc["CalamityMod:BasicTint"].UseOpacity(alpha);
            GameShaders.Misc["CalamityMod:BasicTint"].UseColor(Main.hslToRgb(1 - outlineHSL.X, outlineHSL.Y, outlineHSL.Z));
            GameShaders.Misc["CalamityMod:BasicTint"].Apply();
            spriteBatch.Draw(tex, NPC.Center - screenPos, null, Color.White, NPC.rotation, tex.Size() / 2, new Vector2(NPC.scale) + scale * 0.2f, NPC.FlippedEffects(), 0);
            CalamityUtils.ExitShaderRegion(Main.spriteBatch);
            return false;
        }
    }
}
