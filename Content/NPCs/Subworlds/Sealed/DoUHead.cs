using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using CalamityMod;
using CalamityMod.BiomeManagers;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using System;
using CalRemix.UI;
using System.Linq;
using CalRemix.Core.World;
using CalRemix.Core.Biomes;
using System.Collections.Generic;
using CalamityMod.DataStructures;
using CalamityMod.Graphics.Metaballs;
using CalRemix.Content.Particles;

namespace CalRemix.Content.NPCs.Subworlds.Sealed
{
    public class DoUHead : ModNPC
    {
        public List<Vector2> controlPoints = new();
        public List<Vector2> lashPoints = new();

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 0;
            NPC.width = 28;
            NPC.height = 28;
            NPC.defense = 600;
            NPC.lifeMax = 22;
            NPC.knockBackResist = 0f;
            NPC.value = 0;
            NPC.noGravity = true;
            NPC.noTileCollide = false;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.rarity = 4;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<SealedFieldsBiome>().Type };
        }

        public override void AI()
        {
            InitializeSegments();

            //NPC.Calamity().newAI[0]++;
        }

        public void InitializeSegments()
        {
            if (controlPoints.Count <= 1 || Main.LocalPlayer.controlUseItem)
            {
                controlPoints.Clear();
                controlPoints.Add(new(-340, -500));
                controlPoints.Add(new(0, -600));
                controlPoints.Add(new(-70, 260));
                controlPoints.Add(new(160, -120));
                controlPoints.Add(new(620, 0));
            }
            if (lashPoints.Count <= 1 || Main.LocalPlayer.controlUseItem)
            {
                lashPoints.Clear();
                lashPoints.Add(new(-840, 0));
                lashPoints.Add(new(-500, 0));
                lashPoints.Add(new(-170, 0));
                lashPoints.Add(new(210, 0));
                lashPoints.Add(new(700, 0));
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (NPC.IsABestiaryIconDummy)
                return true;

            if (NPC.IsABestiaryIconDummy)
            {
                InitializeSegments();
            }

            int cycle = 30;
            int half = cycle / 2;
            float localTimer = NPC.Calamity().newAI[0] % cycle;


            List<Vector2> correctedPoints = new();
            for (int i = 0; i < controlPoints.Count; i++)
            {
                Vector2 v = controlPoints[i];
                Vector2 fPos = Vector2.Lerp(v, lashPoints[i], localTimer > half ? Utils.GetLerpValue(cycle, half, localTimer, true) : Utils.GetLerpValue(0, half, localTimer, true));
                correctedPoints.Add(NPC.Center + fPos - screenPos);
            }

            BezierCurve curve = new BezierCurve(correctedPoints.ToArray());

            List<Vector2> finalPoints = curve.GetPoints(30);

            for (int i = finalPoints.Count - 1; i >= 0; i--)
            {
                Texture2D texture = i == 0 ? TextureAssets.Npc[NPC.type].Value : i == finalPoints.Count - 1 ? ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Subworlds/Sealed/DoUTail").Value : ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Subworlds/Sealed/DoUBody").Value;
                Vector2 origin = new Vector2(texture.Width / 2, texture.Height);
                Vector2 v = finalPoints[i];

                float rot = MathHelper.ToRadians(150);

                if (i > 0)
                {
                    rot = v.DirectionTo(finalPoints[i - 1]).ToRotation();
                }
                else
                {
                    rot = v.DirectionTo(finalPoints[i + 1]).ToRotation() + MathHelper.Pi;
                }

                Main.spriteBatch.Draw(texture, v, null, Color.White, rot + NPC.rotation + MathHelper.PiOver2, origin, NPC.scale, SpriteEffects.None, 0f);
            }


            return false;
        }
    }
}
