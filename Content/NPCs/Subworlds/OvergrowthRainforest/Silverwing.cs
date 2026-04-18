using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Core.Biomes;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using System;
using CalRemix.Core.Biomes.Subworlds;
using CalamityMod;
using Terraria.Audio;

namespace CalRemix.Content.NPCs.Subworlds.OvergrowthRainforest
{
    public class Silverwing : ModNPC
    {
        public ref float Timer => ref NPC.ai[0];

        public static SoundStyle BeeBuzz = new SoundStyle("CalRemix/Assets/Sounds/Beebuzz");

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 2;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 100;
            NPC.width = 60;
            NPC.height = 60;
            NPC.defense = 80;
            NPC.lifeMax = 3000;
            NPC.value = Item.buyPrice(silver: 20);
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = false;
            NPC.HitSound = SoundID.NPCHit4 with { Pitch = -0.4f };
            NPC.DeathSound = BeeBuzz;
            SpawnModBiomes = new int[] { ModContent.GetInstance<OvergrowthRainforestBiome>().Type, ModContent.GetInstance<CanopiesBiome>().Type };
        }

        public override void AI()
        {
            NPC.TargetClosest(true);
            NPC.spriteDirection = NPC.direction = NPC.velocity.X.DirectionalSign();

            Timer++;
            if ((Timer % 300 == 0) || Math.Abs(NPC.velocity.X) < 1 || Math.Abs(NPC.velocity.Y) < 1)
            {
                NPC.velocity = Vector2.UnitY.RotatedByRandom(MathHelper.TwoPi) * Main.rand.NextFloat(1, 3);
            }

            if ((Main.rand.NextBool(200) && NPC.life < NPC.lifeMax) || Main.rand.NextBool(600))
            {
                SoundEngine.PlaySound(BeeBuzz with { MaxInstances = 0 }, NPC.Center);
        }
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.SilverCoin, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.SilverCoin, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }

        public override void FindFrame(int frameHeight)
        {
            if (NPC.frameCounter++ > 2)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y += frameHeight;
            }
            if (NPC.frame.Y > frameHeight)
            {
                NPC.frame.Y = 0;
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
        new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemID.BottledHoney, 1, 1, 3);
            npcLoot.Add(ItemID.SilverOre, 2);
        }
    }
}
