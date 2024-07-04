using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using Terraria.GameContent.ItemDropRules;
using CalRemix.Items.Materials;
using CalamityMod.Buffs.StatDebuffs;
using Terraria.ModLoader.Utilities;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using System;

namespace CalRemix.NPCs
{
    public class EternalChampEye : ModNPC
    {
        public override string Texture => "CalRemix/NPCs/FloatingBiomass";
        public Player Target => Main.player[NPC.target];
        public ref float Timer => ref NPC.ai[0];
        public ref float State => ref NPC.ai[1];
        public override void SetStaticDefaults()
        {
            NPCID.Sets.SpecificDebuffImmunity[Type][ModContent.BuffType<KamiFlu>()] = true;
            NPCID.Sets.TrailCacheLength[Type] = 1;
            NPCID.Sets.TrailingMode[Type] = 1;
            this.HideFromBestiary();
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 30;
            NPC.height = 32;
            NPC.lifeMax = 5;
            NPC.damage = 50;
            NPC.defense = 16;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.noTileCollide = true;
        }
        public override void AI()
        {
            NPC papa = Main.npc[(int)NPC.ai[0]];
            if (papa == null || !papa.active || papa.life <= 0 || papa.type != NPC.ai[2])
            {
                NPC.active = false;
                return;
            }

            NPC.ai[1] += 2f;
            float distance = papa.width >= papa.height ? papa.width / 1.2f : papa.height / 1.2f;
            double deg = NPC.ai[1];
            double rad = deg * (Math.PI / 180);
            float hyposx = papa.Center.X - (int)(Math.Cos(rad) * distance);
            float hyposy = papa.Center.Y - (int)(Math.Sin(rad) * distance);
            NPC.position = new Vector2(hyposx, hyposy);
            NPC.life = NPC.lifeMax;

            NPC.rotation = NPC.DirectionTo(NPC.oldPosition).ToRotation() + MathHelper.Pi;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[Type].Value;
            spriteBatch.Draw(texture, NPC.Center - Main.screenPosition + new Vector2(0f, NPC.gfxOffY), null, drawColor, NPC.rotation, texture.Size() / 2f, NPC.scale, SpriteEffects.FlipHorizontally, 0f);
            return false;
        }

        public override bool CheckDead()
        {
            NPC.life = NPC.lifeMax;
            return false;
        }
    }
}
