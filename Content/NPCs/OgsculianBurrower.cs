using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using CalamityMod.Buffs.DamageOverTime;
using CalRemix.Content.Projectiles.Hostile;
using CalRemix.UI;
using System.Linq;

namespace CalRemix.Content.NPCs
{
    public class OgsculianBurrower : ModNPC
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Ogsculian Burrower");
            Main.npcFrameCount[NPC.type] = 5;
            NPCID.Sets.SpecificDebuffImmunity[Type][ModContent.BuffType<BrimstoneFlames>()] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][ModContent.BuffType<VulnerabilityHex>()] = true;

            if (Main.dedServ)
                return;
            HelperMessage.New("Ogscule",
                "That Ogsculian Burrower over there. A dangerous foe. The best course of action here is to jump over them to dodge their laser of doom.",
                "FannyAwooga",
                (ScreenHelperSceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == Type));
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 25;
            NPC.width = 68;
            NPC.height = 46;
            NPC.defense = 20;
            NPC.lifeMax = 70;
            NPC.knockBackResist = 0f;
            NPC.Calamity().DR = 0.05f;
            NPC.value = 100;
            NPC.noGravity = false;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.Calamity().VulnerableToHeat = true;
            NPC.Calamity().VulnerableToSickness = false;
            NPC.Calamity().VulnerableToCold = true;
            NPC.Calamity().VulnerableToWater = true;
            NPC.Calamity().VulnerableToElectricity = true;
            NPC.dontTakeDamage = true;
        }

        public override void AI()
        {
            NPC.TargetClosest();
            if (NPC.HasPlayerTarget)
            {
                Player targ = Main.player[NPC.target];
                if (targ.Distance(NPC.Center) < 980 && CalamityUtils.CountProjectiles(ModContent.ProjectileType<OgsculeBeamHostile>()) < 1)
                {
                    NPC.ai[0] = 22;
                    NPC.dontTakeDamage = false;
                    float rotationSpeed = 0.01f;
                    float dir = NPC.position.X > targ.position.X ? rotationSpeed : -rotationSpeed;
                    Vector2 eyePos = NPC.Center - Vector2.UnitY * 22;
                    if (NPC.frame.Y == 344)
                    {
                        SoundEngine.PlaySound(SoundID.Zombie104, NPC.Center);
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), eyePos, Vector2.Zero, ModContent.ProjectileType<OgsculeBeamHostile>(), Main.expertMode ? 15 : 13, 0, Main.myPlayer, dir);
                            Main.projectile[p].ModProjectile<OgsculeBeamHostile>().NPCOwner = NPC.whoAmI;
                        }
                    }
                }
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Desert,
                new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }
        public override void FindFrame(int frameHeight)
        {
            if (NPC.ai[0] != 22)
            {
                NPC.frame.Y = 0;
                NPC.frameCounter = 0.0;
                return;
            }
            NPC.frameCounter += 1;
            if (NPC.frameCounter > 8 && NPC.frame.Y < frameHeight * 4)
            {
                NPC.frame.Y = NPC.frame.Y + frameHeight;
                NPC.frameCounter = 0.0;
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.PillarZone() ||
                spawnInfo.Player.InAstral() ||
                spawnInfo.Player.ZoneCorrupt ||
                spawnInfo.Player.ZoneCrimson ||
                spawnInfo.Player.ZoneOldOneArmy ||
                spawnInfo.Player.ZoneSkyHeight ||
                spawnInfo.PlayerSafe ||
                !spawnInfo.Player.ZoneDesert ||
                !spawnInfo.Player.ZoneOverworldHeight ||
                Main.eclipse ||
                Main.snowMoon ||
                Main.pumpkinMoon ||
                Main.invasionType != InvasionID.None)
                return 0f;

            // Keep this as a separate if check, because it's a loop and we don't want to be checking it constantly.
            if (NPC.AnyNPCs(NPC.type))
                return 0f;

            return 0.05f;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<StormlionMandible>(), 1, 2, 3);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<VulnerabilityHex>(), 120);
        }
    }
}
