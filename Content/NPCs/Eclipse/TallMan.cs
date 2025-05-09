using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using Terraria.ModLoader.Utilities;
using Microsoft.Xna.Framework;
using CalamityMod.Items.Materials;
using Terraria.DataStructures;
using Terraria.Audio;
using CalRemix.Content.Items.Placeables.Banners;
using CalRemix.Content.Items.Armor;
using CalRemix.Content.Items.Weapons;

namespace CalRemix.Content.NPCs.Eclipse
{
    public class TallMan : ModNPC
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Tall Man");
            Main.npcFrameCount[NPC.type] = 6;
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 30;
            NPC.height = 32;
            NPC.lifeMax = 6000;
            NPC.damage = 0;
            NPC.defense = 12;
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(platinum: 1, gold: 5);
            NPC.HitSound = SoundID.NPCHit36;
            NPC.DeathSound = SoundID.NPCDeath39;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<TallManBanner>();
        }

        public override void OnSpawn(IEntitySource source)
        {
            NPC.ai[0] = Main.screenWidth * 0.666f;
            NPC.localAI[0] = Main.rand.Next(0, 3);
        }

        public override void AI()
        {
            NPC.ai[1]++;
            if (NPC.ai[1] % 600 == 0)
            {
                int tpDist = 400;
                for (int i = 0; i < 22; i++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.UndergroundHallowedEnemies);
                }
                SoundEngine.PlaySound(SoundID.Item8, NPC.Center);
                NPC.position = NPC.position + new Vector2(Main.rand.Next(-tpDist, tpDist), Main.rand.Next(-tpDist, tpDist));
                for (int i = 0; i < 22; i++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.UndergroundHallowedEnemies);
                }
            }
            Rectangle screen = new Rectangle((int)Main.screenPosition.X, (int)Main.screenPosition.Y, (int)(Main.screenWidth / Main.GameZoomTarget), (int)(Main.screenHeight / Main.GameZoomTarget));
            if (NPC.getRect().Intersects(screen) || NPC.ai[0] < 0)
            {
                NPC.ai[0] -= Main.screenWidth * 0.666f / 600;
                if (NPC.ai[0] < -60)
                {
                    if (NPC.ai[2] < 2f)
                    {
                        NPC.ai[2] += 0.025f;
                    }
                    else
                    {
                        Main.LocalPlayer.KillMe(PlayerDeathReason.ByNPC(NPC.whoAmI), 999999, 1);
                        NPC.active = false;
                    }
                }
            }
            else
            {
                if (NPC.ai[0] < Main.screenWidth * 0.666f)
                {
                    NPC.ai[0] += 20;
                }
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Eclipse,
        new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter += 1.0;
            if (NPC.frameCounter > 6.0)
            {
                NPC.frameCounter = 0.0;
                NPC.frame.Y += frameHeight;
            }
            if (NPC.frame.Y > frameHeight * 5)
            {
                NPC.frame.Y = 0;
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!DownedBossSystem.downedDoG || NPC.AnyNPCs(Type))
                return 0f;

            return SpawnCondition.SolarEclipse.Chance * 0.05f;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<DarksunFragment>(), 1, 7, 7);
            npcLoot.Add(ModContent.ItemType<BlankStare>(), 10);
            npcLoot.Add(ModContent.ItemType<SignoftheOperator>(), 10);
        }
    }
}