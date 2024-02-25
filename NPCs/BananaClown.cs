using CalamityMod.Dusts;
using CalamityMod.Items.Placeables.Ores;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using CalamityMod;
using CalRemix.Items;
using CalamityMod.NPCs;
using CalamityMod.Items.SummonItems;
using CalamityMod.Items.Materials;
using Microsoft.Xna.Framework;
using CalamityMod.Projectiles.Summon;
using CalamityMod.World;
using Terraria.Audio;
using Terraria.DataStructures;
using CalamityMod.Buffs.DamageOverTime;
using CalRemix.Projectiles.Weapons;
using CalRemix.UI;
using System.Collections.Generic;
using System.Linq;
using CalamityMod.Items.Placeables.Furniture.Trophies;
using System.Xml.Serialization;
using CalRemix.Items.Materials;
using CalRemix.Items.Potions;
using CalamityMod.Items.Weapons.Summon;
using CalRemix.Biomes;
using CalamityMod.Projectiles.Boss;
using CalRemix.Items.Armor;
using CalRemix.Projectiles.Hostile;

namespace CalRemix.NPCs
{
    public class BananaClown : ModNPC
    {
        public float[] GreenAI = new float[4];
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Banana Clown");
            Main.npcFrameCount[NPC.type] = 12;

            if (Main.dedServ)
                return;
            FannyManager.LoadFannyMessage(new FannyMessage("BananaClown",
                "Wuh oh! A Banana Clown! Don't get too close to them or you'll go bananas cleaning up the terrain damage they cause!",
                "Nuhuh",
                (FannySceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == Type)));
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 40;
            NPC.width = 54;
            NPC.height = 126;
            NPC.defense = 5;
            NPC.lifeMax = 160;
            NPC.knockBackResist = Main.expertMode ? 0.775f : 0.75f;
            NPC.value = Item.buyPrice(silver: 20);
            NPC.noGravity = false;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath50;
            NPC.Calamity().VulnerableToHeat = true;
            NPC.Calamity().VulnerableToCold = true;
            NPC.Calamity().VulnerableToElectricity = false;
            AnimationType = NPCID.Clown;
        }

        public override void OnSpawn(IEntitySource source)
        {
            GreenAI[0] = 0;
            GreenAI[1] = 0;
        }

        public override void AI()
        {
            GreenAI[0]++;
            GreenAI[2]++;
            CalamityMod.NPCs.CalamityGlobalAI.BuffedFighterAI(NPC, Mod);
            NPC.spriteDirection = NPC.direction;
            if (NPC.HasPlayerTarget)
            {
                if (Collision.CanHitLine(NPC.Center, 1, 1, Main.player[NPC.target].Center, 1, 1) && GreenAI[0] % (130 + Main.rand.Next(-10, 21)) == 0 && NPC.Distance(Main.player[NPC.target].Center) < 160)
                {
                    Vector2 dist = Main.player[NPC.target].position - NPC.position;
                    dist.Normalize();
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, dist * 10, ModContent.ProjectileType<BananaBomb>(), 15, 0, Main.myPlayer);
                }
            }
            if (GreenAI[2] >= (120 + Main.rand.Next(0, 20)))
            {
                int choice = Main.rand.Next(0, 3);
                SoundStyle fuckyou = SoundID.Zombie121;
                switch (choice)
                {
                    case 0:
                        fuckyou = SoundID.Zombie121;
                        break;
                    case 1:
                        fuckyou = SoundID.Zombie122;
                        break;
                    case 2:
                        fuckyou = SoundID.Zombie123;
                        break;
                }
                SoundEngine.PlaySound(fuckyou, NPC.Center);
                GreenAI[2] = 0;
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
        new FlavorTextBestiaryInfoElement("After the fall of the Godseeker's empire, his clowns were left jobless. Filled with malice, they decided to stand up against the rest of the world and become one of the few enemies that are able to destroy tiles.")
            });
        }
        public override void FindFrame(int frameHeight)
        {
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (Main.dayTime || !CalRemixWorld.clowns)
                return 0f;

            return SpawnCondition.OverworldNightMonster.Chance * 0.05f;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<BananaClownHat>(), 3);
            npcLoot.Add(ModContent.ItemType<BananaClownSleeves>(), 3);
            npcLoot.Add(ModContent.ItemType<BananaClownPants>(), 3);
        }
    }
}
