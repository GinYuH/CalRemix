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
using CalRemix.Content.Items.Weapons;
using CalRemix.Content.NPCs.Bosses.Origen;
using CalamityMod.Sounds;
using CalamityMod.NPCs.DevourerofGods;
using Terraria.DataStructures;
using CalRemix.Content.Items.Placeables;
using System.IO;
using CalRemix.Content.Projectiles.Hostile;

namespace CalRemix.Content.NPCs.Subworlds.Sealed
{
    public class DoUHead : ModNPC
    {
        public List<Vector2> controlPoints = new();

        public Player Target => Main.player[NPC.target];

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 200;
            NPC.width = 100;
            NPC.height = 200;
            NPC.defense = 600;
            NPC.lifeMax = 10000;
            NPC.value = Item.buyPrice(gold: 2);
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.HitSound = CommonCalamitySounds.OtherwordlyHitSound;
            NPC.DeathSound = DevourerofGodsHead.DeathAnimationSound;
            NPC.rarity = 1;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<BadlandsBiome>().Type };
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            NPC.Calamity().newAI[0] = reader.ReadSingle();
            NPC.Calamity().newAI[1] = reader.ReadSingle();
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(NPC.Calamity().newAI[0]);
            writer.Write(NPC.Calamity().newAI[1]);
        }

        public override void AI()
        {
            InitializeSegments();

            NPC.Calamity().newAI[0]++;

            NPC.TargetClosest();

            if (NPC.ai[0] == 1)
            {
                NPC.velocity.X = NPC.DirectionTo(Target.Center).X * 5;

                if (NPC.collideX)
                {
                    NPC.velocity.Y = -10;
                }
                if (NPC.Distance(Target.Center) < 600 && Target.Center.Y > NPC.Center.Y && Target.Center.Y > NPC.Bottom.Y + 20)
                {
                    NPC.Calamity().newAI[1] = 1;
                    if (NPC.Calamity().newAI[0] % 30 == 15)
                    {
                        NPC.netUpdate = true;
                        Target.Hurt(PlayerDeathReason.ByNPC(NPC.whoAmI), NPC.damage, NPC.direction);
                    }
                }
                else
                {
                    NPC.Calamity().newAI[1] = 0;
                }
            }
            else
            {
                NPC.Calamity().newAI[1] = 0;
            }

            if (NPC.justHit)
            {
                NPC.ai[0] = 1;
                NPC.netUpdate = true;
            }

        }

        public void InitializeSegments()
        {
            if (controlPoints.Count <= 1)
            {
                controlPoints.Clear();
                controlPoints.Add(new(-340, -500));
                controlPoints.Add(new(0, -600));
                controlPoints.Add(new(-70, 260));
                controlPoints.Add(new(160, -120));
                controlPoints.Add(new(620, 0));
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }

        public void DrawDoT(SpriteBatch spriteBatch, Vector2 screenPos)
        {
            if (NPC.IsABestiaryIconDummy)
                return;

            if (NPC.IsABestiaryIconDummy)
            {
                InitializeSegments();
            }

            int cycle = 30;
            int half = cycle / 2;
            float localTimer = NPC.Calamity().newAI[0] % cycle;

            bool walking = (NPC.Calamity().newAI[1] == 0 && Math.Abs(NPC.velocity.X) > 0);

            float mult = walking ? 0.25f : NPC.Calamity().newAI[1] == 1 ? 1f : 0;

            List<Vector2> correctedPoints = new();
            for (int i = 0; i < controlPoints.Count; i++)
            {
                Vector2 v = controlPoints[i];
                if (NPC.direction == 1)
                {
                    v.X *= -1;
                }
                bool flip = v.X <= 0;
                Vector2 fPos = Vector2.Lerp(v, v.Length() * -Vector2.UnitX * flip.ToDirectionInt(), mult * (localTimer > half ? Utils.GetLerpValue(cycle, half, localTimer, true) : Utils.GetLerpValue(0, half, localTimer, true)));
                correctedPoints.Add(NPC.Bottom + fPos - screenPos);
            }

            BezierCurve curve = new BezierCurve(correctedPoints.ToArray());

            List<Vector2> finalPoints = curve.GetPoints(40);

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

                if (!NPC.IsABestiaryIconDummy && Main.rand.NextBool(120) && Main.hasFocus)
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), v + Main.rand.NextVector2Circular(100, 100) + screenPos + NPC.velocity, Main.rand.NextVector2Circular(4, 4), ModContent.ProjectileType<DoUSmoke>(), 0, 0);


                spriteBatch.Draw(texture, v, null, Color.White, rot + NPC.rotation + MathHelper.PiOver2, origin, NPC.scale, SpriteEffects.None, 0f);
                if (i == (finalPoints.Count / 2))
                {
                    Texture2D bloom = ModContent.Request<Texture2D>("CalamityMod/Particles/Light").Value;
                    spriteBatch.Draw(bloom, v, null, Color.Magenta, 0, bloom.Size() / 2, NPC.scale + MathF.Sin(Main.GlobalTimeWrappedHourly * 22) * 0.2f, SpriteEffects.None, 0f);
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            DrawDoT(spriteBatch, screenPos);
            return false;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.AddIf(() => !NPC.downedMoonlord, ModContent.ItemType<CosmiliteSlag>());
            npcLoot.AddIf(() => NPC.downedMoonlord, ModContent.ItemType<CosmiliteSlag>(), 1, 54, 86);
        }
    }
}
