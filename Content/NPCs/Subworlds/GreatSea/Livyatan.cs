using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using CalamityMod;
using CalRemix.Content.Items.Placeables;
using CalRemix.Core.Biomes;
using CalamityMod.BiomeManagers;
using CalRemix.Content.Items.Materials;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using CalRemix.Content.NPCs.Bosses.Origen;
using Newtonsoft.Json.Linq;
using Terraria.Audio;
using Terraria.GameContent;
using System;
using CalamityMod.Particles;
using Microsoft.CodeAnalysis.Host.Mef;
using Terraria.DataStructures;
using CalRemix.Content.Projectiles.Hostile;
using System.Collections.Generic;
using CalamityMod.InverseKinematics;

namespace CalRemix.Content.NPCs.Subworlds.GreatSea
{
    public class Livyatan : ModNPC
    {
        public ref float Timer => ref NPC.ai[0];

        public ref float CurrentPhase => ref NPC.ai[1];
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 1;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 180;
            NPC.width = 200;
            NPC.height = 200;
            NPC.defense = 56;
            NPC.lifeMax = 2000000;
            NPC.value = 1000000;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = false;
            NPC.knockBackResist = 0;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.GravityIgnoresLiquid = true;
            NPC.waterMovementSpeed = 1f;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<PrimordialCavesBiome>().Type };
        }

        public override void AI()
        {
            if (NPC.localAI[0] == 0)
            {
                NPC.localAI[0] = 1;
            }
            if (Main.LocalPlayer.controlUseItem)
            {
                handIK = new LimbCollection(new CyclicCoordinateDescentUpdateRule(0.07f, MathHelper.ToRadians(60)), 96f, 142f);
                tailIK = new LimbCollection(new CyclicCoordinateDescentUpdateRule(0.07f, MathHelper.ToRadians(60)), 202f, 248f);
            }

                handIK.Limbs[0].Rotation = GetIKRotationClamp((float)handIK.Limbs[0].Rotation, MathHelper.Pi, MathHelper.PiOver2);
            if (Main.LocalPlayer.selectedItem == 0)
                handIK.Update(NPC.Center + new Vector2(NPC.spriteDirection * 160, 40), Main.MouseWorld);
                tailIK.Limbs[0].Rotation = GetIKRotationClamp((float)tailIK.Limbs[0].Rotation, MathHelper.ToRadians(270), MathHelper.ToRadians(120));
            if (Main.LocalPlayer.selectedItem == 1)
                tailIK.Update(NPC.Center + new Vector2(NPC.spriteDirection * -170, 0), Main.MouseWorld);
            if (Main.LocalPlayer.selectedItem == 2)
                NPC.localAI[2] = NPC.Center.DirectionTo(Main.MouseWorld).ToRotation();
            if (CurrentPhase == 0)
            {
            }
            else if (CurrentPhase == 1)
            {
            }
            else if (CurrentPhase == 2)
            {
            }
        }

        public float GetIKRotationClamp(float baseRotation, float min, float max)
        {
            float flipRot = NPC.spriteDirection == -1 ? MathHelper.Pi : 0;
            if (NPC.spriteDirection == -1)
            {
                return MathHelper.Clamp(baseRotation, NPC.spriteDirection * min + flipRot, NPC.spriteDirection * max + flipRot);
            }
            else
            {
                return MathHelper.Clamp(baseRotation, NPC.spriteDirection * max + flipRot, NPC.spriteDirection * min + flipRot);
            }
            return MathHelper.Clamp(baseRotation, NPC.spriteDirection * min + flipRot, NPC.spriteDirection * max + flipRot);
        }

        public LimbCollection handIK = new LimbCollection(new CyclicCoordinateDescentUpdateRule(0.07f, MathHelper.ToRadians(60)), 96f, 142f);
        public Vector2 handDest = new Vector2();

        public LimbCollection tailIK = new LimbCollection(new CyclicCoordinateDescentUpdateRule(0.07f, MathHelper.ToRadians(60)), 202f, 248f);
        public Vector2 tailDest = new Vector2();

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D tex = TextureAssets.Npc[Type].Value;
            Texture2D head = ModContent.Request<Texture2D>(Texture + "Head").Value;
            Texture2D jaw = ModContent.Request<Texture2D>(Texture + "Mouth").Value;
            Texture2D arm = ModContent.Request<Texture2D>(Texture + "Arm").Value;
            Texture2D hand = ModContent.Request<Texture2D>(Texture + "Hand").Value;
            Texture2D pupil = ModContent.Request<Texture2D>(Texture + "Pupil").Value;
            Texture2D eye = ModContent.Request<Texture2D>(Texture + "Eye").Value;
            Texture2D bodyLower = ModContent.Request<Texture2D>(Texture + "BodyLower").Value;
            Texture2D tail = ModContent.Request<Texture2D>(Texture + "Tail").Value;
            SpriteEffects fx = NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            DrawPiece(spriteBatch, jaw, screenPos, new Vector2(180, 50), new Vector2(334, 33), drawColor, rotationOverride: NPC.localAI[2]);
            spriteBatch.Draw(tex, NPC.Center - screenPos, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, new Vector2(tex.Width / 2, tex.Height / Main.npcFrameCount[Type] / 2), NPC.scale, fx, 0);
            DrawPiece(spriteBatch, head, screenPos, new Vector2(260, 40), new Vector2(329, 136), drawColor);
            DrawPiece(spriteBatch, eye, screenPos, new Vector2(333, -3), new Vector2(10, 3), drawColor);
            DrawPiece(spriteBatch, pupil, screenPos, new Vector2(333, -3), new Vector2(3, 3), drawColor);

            for (int i = 0; i < handIK.Limbs.Length; i++)
            {
                Texture2D touse = (i == 0) ? arm : hand;
                Vector2 origin = (i == 0) ? new Vector2(30, 24) : new Vector2(35, 19);
                float offset = (i == 0) ? -MathHelper.PiOver4 * 3 : MathHelper.ToRadians(200);
                if (NPC.spriteDirection == -1)
                {
                    offset = -offset + MathHelper.Pi;
                }
                Limb limb = handIK.Limbs[i];
                spriteBatch.Draw(touse, limb.ConnectPoint - screenPos, null, NPC.GetAlpha(drawColor), (float)limb.Rotation + offset, GetPieceOrigin(touse.Width, origin), NPC.scale, fx, 0);
            }
            for (int i = 0; i < tailIK.Limbs.Length; i++)
            {
                Texture2D touse = (i == 0) ? bodyLower : tail;
                Vector2 origin = (i == 0) ? new Vector2(31, 52) : new Vector2(15, 43);
                float offset = (i == 0) ? MathHelper.ToRadians(180) : MathHelper.ToRadians(180);
                if (NPC.spriteDirection == -1)
                {
                    offset = -offset + MathHelper.Pi;
                }
                Limb limb = tailIK.Limbs[i];
                spriteBatch.Draw(touse, limb.ConnectPoint - screenPos, null, NPC.GetAlpha(drawColor), (float)limb.Rotation + offset, GetPieceOrigin(touse.Width, origin), NPC.scale, fx, 0);
            }
            return false;
        }

        public void DrawPiece(SpriteBatch sb, Texture2D tex, Vector2 screenPos, Vector2 offset, Vector2 originOffset, Color drawColor, float rotationOverride = 0)
        {
            SpriteEffects fx = NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            sb.Draw(tex, GetPiecePosition(screenPos, offset), null, NPC.GetAlpha(drawColor), NPC.rotation + NPC.spriteDirection * rotationOverride, GetPieceOrigin(tex.Width, originOffset), NPC.scale, fx, 0);
        }

        public Vector2 GetPiecePosition(Vector2 screenPos, Vector2 offset)
        {
            return NPC.Center - screenPos + NPC.scale * new Vector2(offset.X * NPC.spriteDirection, offset.Y).RotatedBy(NPC.rotation);
        }

        public Vector2 GetPieceOrigin(float baseWidth, Vector2 originalOffset)
        {
            return new Vector2(NPC.spriteDirection == 1 ? baseWidth - originalOffset.X : originalOffset.X, originalOffset.Y);
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
        new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }
    }
}
