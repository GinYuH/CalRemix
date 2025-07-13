using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.Buffs.StatDebuffs;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using System;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using CalRemix.Core.Biomes;

namespace CalRemix.Content.NPCs.Subworlds.Sealed
{
    public class DreadonFriendly : ModNPC
    {
        public override string Texture => "CalRemix/Content/NPCs/Subworlds/Sealed/Dreadon";
        public Player Target => Main.player[NPC.target];
        public ref float Timer => ref NPC.ai[0];
        public ref float State => ref NPC.ai[1];

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 54;
            NPC.height = 86;
            NPC.lifeMax = 20000;
            NPC.damage = 0;
            NPC.defense = 8;
            NPC.friendly = true;
            NPC.noGravity = false;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = BetterSoundID.ItemElectricFizzleExplosion;
            NPC.noTileCollide = false;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<SealedFieldsBiome>().Type };
        }
        public override void AI()
        {
            NPC.TargetClosest();
            NPC.spriteDirection = NPC.direction;
            NPC.frameCounter++;
            if (NPC.frameCounter > 7 * 6)
                NPC.frameCounter = 0;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[Type].Value;
            SpriteEffects fx = NPC.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            Texture2D head = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Subworlds/Sealed/DreadonHead").Value;

            int armLength = 26;
            int legLength = 24;
            Vector2 legOffset = new Vector2(4, 20);
            Vector2 armOffset = new Vector2(16, 10);

            float armRot = MathHelper.ToRadians(50);

            armRot += MathF.Sin(Main.GlobalTimeWrappedHourly * 10) * 0.5f;

            DrawLimb(spriteBatch, legOffset, legLength, 0);
            DrawLimb(spriteBatch, legOffset with { X = -legOffset.X}, legLength, 0);
            DrawLimb(spriteBatch, armOffset, armLength, -armRot, true);
            DrawLimb(spriteBatch, armOffset with { X = -armOffset.X}, armLength, armRot, true);

            float bodyOffset = MathF.Sin(Main.GlobalTimeWrappedHourly * 10) * 2;
            float headOffset = 12 + MathF.Sin(Main.GlobalTimeWrappedHourly * 10 + 1) * 2;

            float headRot = MathF.Sin(Main.GlobalTimeWrappedHourly * 5) * 0.25f;

            spriteBatch.Draw(texture, NPC.Center - Main.screenPosition + new Vector2(0f, NPC.gfxOffY + texture.Height / 14 + bodyOffset), texture.Frame(1, 7, 0, (int)Utils.Lerp(0, 6, Utils.GetLerpValue(0, 7 * 6, NPC.frameCounter, true))), drawColor, NPC.rotation, new Vector2(texture.Width / 2, texture.Height / 7), NPC.scale, fx, 0f);
            spriteBatch.Draw(head, NPC.Center - Main.screenPosition + new Vector2(0f, NPC.gfxOffY - texture.Height / 14 + headOffset), null, drawColor, headRot, new Vector2(head.Width / 2, head.Height), NPC.scale, fx, 0f);
            return false;
        }

        public void DrawLimb(SpriteBatch spriteBatch, Vector2 pos, float lenght, float rot, bool arm = false)
        {
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, NPC.Center - Main.screenPosition + pos, new Rectangle(0, 0, 4, (int)lenght), Color.Black, rot, new Vector2(2, 0), NPC.scale, 0, 0);

            if (arm)
            {
                for (int i = 0; i < 2; i++)
                {
                    spriteBatch.Draw(TextureAssets.MagicPixel.Value, NPC.Center - Main.screenPosition + pos + Vector2.UnitY.RotatedBy(rot) * lenght * 0.6f, new Rectangle(0, 0, 4, 10), Color.Black, rot + MathHelper.Lerp(-MathHelper.PiOver4, MathHelper.PiOver4, i / 1f), new Vector2(2, 0), NPC.scale, 0, 0);
                }
            }
            else
            {
                spriteBatch.Draw(TextureAssets.MagicPixel.Value, NPC.Center - Main.screenPosition + pos + Vector2.UnitY.RotatedBy(rot) * lenght, new Rectangle(0, 0, 4, 10), Color.Black, rot - MathHelper.PiOver2 * pos.X.DirectionalSign(), new Vector2(2, 0), NPC.scale, 0, 0);
            }

        }

        public override bool CheckActive()
        {
            return false;
        }
        public override bool NeedSaving()
        {
            return true;
        }
    }
}
