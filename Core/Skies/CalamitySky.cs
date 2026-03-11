using CalRemix.Content.NPCs.Bosses.BossScule;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace CalRemix.Core.Scenes
{
	public class CalamitySky : CustomSky
	{
		private bool _isActive;
		private NPC npc;
        public override void Update(GameTime gameTime)
        {
        }

		public override Color OnTileColor(Color inColor)
        {
            return inColor;
		}

		public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            if (!NPCMatches())
                return;
            spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), new Color(0, 0, 0, npc.alpha));
        }

		public override float GetCloudAlpha() 
		{
			return 0f;
		}

		public override void Activate(Vector2 position, params object[] args)
        {
            foreach (NPC n in Main.npc)
            {
                if (n.type == ModContent.NPCType<TheCalamity>() && NPC.AnyNPCs(ModContent.NPCType<TheCalamity>()))
                    npc = n;
            }
            _isActive = true;
		}

		public override void Deactivate(params object[] args) 
		{
			_isActive = false;
		}

		public override void Reset() 
		{
			_isActive = false;
		}

		public override bool IsActive() 
		{
			return _isActive;
		}
		private bool NPCMatches()
        {
			if (npc == null)
				return false;
			if (npc != null && npc.type != ModContent.NPCType<TheCalamity>())
				return false;
			return true;
        }
	}
}
