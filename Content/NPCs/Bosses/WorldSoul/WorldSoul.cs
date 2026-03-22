using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.NPCs.Bosses.WorldSoul
{
    [AutoloadBossHead]
    public class WorldSoul : ModNPC
    {
        public ref float AttackType => ref NPC.ai[0];
        public ref float Timer => ref NPC.ai[1];

        public ref Player Target => ref Main.player[NPC.target];

        public enum AttackTypes
        {
            PhaseTransition = -1,
            None = 0,
            Move = 1,
            MineRing = 2,
            ProjectileVomit = 3,
            LightningOrb = 4,
            Ritual = 5,
            HereticSpears = 6,
            SpinningRingProjectiles = 7,
            AncientDoomEsqueMines = 8
        }

        public override void SetDefaults()
        {
            NPC.width = 40;
            NPC.height = 280;

            NPC.aiStyle = -1;
            NPC.damage = 0;
            NPC.defense = 0;
            NPC.lifeMax = 77000000;
            NPC.HitSound = SoundID.NPCHit54;
            NPC.DeathSound = SoundID.NPCDeath52;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.timeLeft = 22500;
            NPC.boss = true;
            NPC.value = Item.buyPrice(platinum: 100);
            for (int i = 0; i < BuffLoader.BuffCount; i++)
            {
                NPC.buffImmune[i] = true;
            }
            NPC.frame = new Rectangle(0, 0, NPC.width, NPC.height);
        }

        #region ai
        public override void AI()
        {
            NPC.timeLeft = 1;
        }
        #endregion

        #region drawing
        private const int SHEET_FRAME_AMT_X = 22;
        private const int SHEET_FRAME_AMT_Y = 1;
        private int frameToUse = 0;
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[Type].Value;
            int frameWidth = (int)texture.Size().X / SHEET_FRAME_AMT_X;
            int frameHeight = (int)texture.Size().Y / SHEET_FRAME_AMT_Y;
            Texture2D textureGlow = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Bosses/WorldSoul/WorldSoul_Glow").Value;

            int newFrameToUse = frameToUse;
            if (Main.timeForVisualEffects % 4 == 0)
            {
                newFrameToUse = Main.rand.Next(0, SHEET_FRAME_AMT_X);
                // avoid rolling the same frame twice
                while (frameToUse == newFrameToUse)
                    newFrameToUse = Main.rand.Next(0, SHEET_FRAME_AMT_X);
                frameToUse = newFrameToUse;
            }

            float floatOffset = (float)(Math.Sin(Main.GlobalTimeWrappedHourly) * 15);

            Vector2 pos = NPC.Center - screenPos;
            Vector2 posToUse = new Vector2(pos.X, pos.Y + floatOffset);
            Rectangle frame = texture.Frame(SHEET_FRAME_AMT_X, 1, frameToUse, 0);
            Vector2 origin = new Vector2(frameWidth / 2, frameHeight / 2);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.Transform);
            spriteBatch.Draw(textureGlow, posToUse, frame, Color.LimeGreen, NPC.rotation, origin, NPC.scale, 0, 0);
            spriteBatch.Draw(texture, posToUse, frame, Color.White, NPC.rotation, origin, NPC.scale, 0, 0);
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, default, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.Transform);

            return false;
        }
        #endregion
    }
}
