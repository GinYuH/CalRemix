using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using System;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using CalRemix.Core.Biomes;
using CalRemix.Content.Projectiles.Weapons;
using CalRemix.UI;
using CalRemix.Content.Items.Misc;
using CalamityMod;
using Terraria.ID;
using CalRemix.Core.World;

namespace CalRemix.Content.NPCs.Subworlds.Sealed
{
    [AutoloadHead]
    public class VigorCloak : QuestNPC
    {
        public Player Target => Main.player[NPC.target];
        public ref float Timer => ref NPC.ai[0];
        public ref float State => ref NPC.ai[1];

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 102;
            NPC.height = 94;
            NPC.lifeMax = 2000;
            NPC.damage = 0;
            NPC.defense = 8;
            NPC.friendly = true;
            NPC.noGravity = false;
            NPC.HitSound = null;
            NPC.DeathSound = null;
            NPC.knockBackResist = 0f;
            NPC.dontTakeDamage = true;
            NPC.noTileCollide = false;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<VolcanicFieldBiome>().Type };
        }
        public override void AI()
        {
            base.AI();
            NPC.TargetClosest();
            NPC.spriteDirection = NPC.direction = -1;
            Timer++;
            if (NPCDialogueUI.NotFinishedTalking(NPC) && Timer % 18 == 0)
            {
                SoundEngine.PlaySound(SoundID.NPCDeath13 with { Pitch = Main.rand.NextFloat(-1.4f, -0.8f) }, NPC.Center);
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D tex = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Vigor").Value;
            Texture2D head = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/VigorHead").Value;
            Texture2D jaw = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/VigorJaw").Value;
            Texture2D cloak = TextureAssets.Npc[Type].Value;
            Vector2 jawOrig = new Vector2(18, 4);
            Vector2 headOrig = new Vector2(22, 46);
            float jawRot = 0;
            if (NPCDialogueUI.NotFinishedTalking(NPC))
            {
                jawRot = MathF.Sin(Main.GlobalTimeWrappedHourly * 12) * 0.25f;
            }
            spriteBatch.Draw(tex, NPC.Center - screenPos, null, NPC.GetAlpha(drawColor), NPC.rotation, tex.Size() / 2, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            spriteBatch.Draw(head, NPC.Center - new Vector2(20 * -NPC.spriteDirection, 60) - screenPos, null, NPC.GetAlpha(drawColor), 0, headOrig, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            spriteBatch.Draw(jaw, NPC.Center - new Vector2(18 * -NPC.spriteDirection, 70) - screenPos, null, NPC.GetAlpha(drawColor), jawRot, jawOrig, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            spriteBatch.Draw(cloak, NPC.Center - screenPos - new Vector2(-8, 30), null, NPC.GetAlpha(drawColor), NPC.rotation, cloak.Size() / 2, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            return false;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
        new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }

        public override bool NeedSaving()
        {
            return true;
        }

        public override bool CheckActive()
        {
            return false;
        }
    }
}
