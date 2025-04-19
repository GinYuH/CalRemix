using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ReLogic.Content;
using Terraria.GameContent;

namespace CalRemix.Content.NPCs.Bosses.Oxygen
{
    public class OxygenShield : ModNPC
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Oxygen's Shield");
            this.HideFromBestiary();
        }

        public override void SetDefaults()
        {
            NPC.damage = 60;
            NPC.width = 170;
            NPC.height = 166;
            NPC.defense = 20;
            NPC.lifeMax = 8000;
            NPC.knockBackResist = 0f;
            NPC.lavaImmune = false;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit7;
            NPC.DeathSound = SoundID.NPCDeath8;
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.Calamity().VulnerableToCold = false;
            NPC.Calamity().VulnerableToWater = false;
            NPC.Calamity().VulnerableToElectricity = false;
            NPC.Calamity().VulnerableToHeat = true;
        }

        public override void AI()
        {
            Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Cloud);
            NPC.dontTakeDamage = true;
            NPC carci = Main.npc[(int)NPC.ai[0]];
            if (carci != null && carci.active && carci.type == ModContent.NPCType<Oxygen>())
            {
                NPC.position = carci.Center - NPC.Size / 2;
                NPC.rotation += 0.1f;
            }
            else
            {
                NPC.active = false;
            }
        }


        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Cloud, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Cloud, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            NPC carci = Main.npc[(int)NPC.ai[0]];
            if (carci == null || !carci.active || carci.type != ModContent.NPCType<Oxygen>())
                return false;

            Asset<Texture2D> sprite = TextureAssets.Npc[Type];
            Texture2D cigars = ModContent.Request<Texture2D>(Texture + "Bits").Value;
            Vector2 npcOffset = NPC.Center - screenPos;
            spriteBatch.EnterShaderRegion(BlendState.Additive);
            spriteBatch.Draw(sprite.Value, npcOffset, null, Color.White, NPC.rotation, sprite.Size() / 2, 1f, SpriteEffects.None, 0);
            spriteBatch.ExitShaderRegion();
            spriteBatch.Draw(cigars, npcOffset, null, NPC.GetAlpha(Lighting.GetColor(new Point((int)carci.position.X / 16, (int)carci.position.Y / 16))), NPC.rotation, sprite.Size() / 2, 1f, SpriteEffects.None, 0);
            return false;
        }
        public override bool CheckActive() => !(Main.npc[(int)NPC.ai[0]] != null && Main.npc[(int)NPC.ai[0]].active && Main.npc[(int)NPC.ai[0]].type == ModContent.NPCType<Oxygen>());
    }
}
