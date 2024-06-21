using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.Audio;
using CalamityMod.World;
using CalamityMod.Particles;
using CalRemix.Projectiles.Hostile;
using CalRemix.Items.Placeables;
using CalamityMod.Events;
using CalRemix.Biomes;
using CalamityMod.BiomeManagers;
using CalamityMod.Items.Materials;
using System;
using CalamityMod.Projectiles.Enemy;
using Newtonsoft.Json.Serialization;
using CalRemix.UI;
using System.Linq;
using CalamityMod.NPCs.Cryogen;

namespace CalRemix.NPCs.Bosses.Origen
{
    [AutoloadBossHead]
    public class OrigenCore : ModNPC
    {
        public ref float Phase => ref NPC.ai[0];

        public ref Player Target => ref Main.player[NPC.target];

        public Rectangle teleportPos = new Rectangle();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Origen");
        }

        public override void SetDefaults()
        {
            NPC.Calamity().canBreakPlayerDefense = true;
            NPC.npcSlots = 24f;
            NPC.damage = 0;
            NPC.width = 74;
            NPC.height = 74;
            NPC.defense = 15;
            NPC.DR_NERD(0.3f);
            NPC.LifeMaxNERB(200, 400, 30000);
            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(0, 40, 0, 0);
            NPC.boss = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.DeathSound = CalamityMod.NPCs.Cryogen.Cryogen.DeathSound;
            NPC.Calamity().VulnerableToHeat = true;
            NPC.Calamity().VulnerableToCold = false;
            NPC.Calamity().VulnerableToSickness = false;
            if (!Main.dedServ)
            {
                Music = MusicLoader.GetMusicSlot("CalamityModMusic/Sounds/Music/Cryogen");
            }
        }

        public override void AI()
        {
            NPC.TargetClosest();
            bool rev = CalamityWorld.revenge || BossRushEvent.BossRushActive;
            bool death = CalamityWorld.death || BossRushEvent.BossRushActive;
            bool master = Main.masterMode || BossRushEvent.BossRushActive;
            bool expert = Main.expertMode || BossRushEvent.BossRushActive;
            if (Target == null || Target.dead)
            {
                NPC.velocity.Y += 1;
                NPC.Calamity().newAI[3]++;
                if (NPC.Calamity().newAI[3] > 240)
                {
                    NPC.active = false;
                }
                return;
            }
            NPC.Calamity().newAI[3] = 0;
            float speedMult = death ? 16 : rev ? 14 : expert ? 12 : 10f;
            if (master)
            {
                speedMult += 2;
            }
            Vector2 vector39 = new Vector2(NPC.Center.X + (float)(NPC.direction * 20), NPC.Center.Y + 6f);
            float dirX = Target.position.X + (float)Target.width * 0.5f - vector39.X;
            float dirY = Target.Center.Y - vector39.Y;
            float finalSpeed = (float)Math.Sqrt(dirX * dirX + dirY * dirY);
            float speedMod = speedMult / finalSpeed;
            dirX *= speedMod;
            dirY *= speedMod;
            NPC.ai[0] -= 1f;
            if (finalSpeed < 200f || NPC.ai[0] > 0f)
            {
                if (finalSpeed < 200f)
                {
                    NPC.ai[0] = 20f;
                }
                if (NPC.velocity.X < 0f)
                {
                    NPC.direction = -1;
                }
                else
                {
                    NPC.direction = 1;
                }
                NPC.rotation += (float)NPC.direction * 0.3f;
                return;
            }
            NPC.velocity.X = (NPC.velocity.X * 50f + dirX) / 51f;
            NPC.velocity.Y = (NPC.velocity.Y * 50f + dirY) / 51f;
            if (finalSpeed < 350f)
            {
                NPC.velocity.X = (NPC.velocity.X * 10f + dirX) / 11f;
                NPC.velocity.Y = (NPC.velocity.Y * 10f + dirY) / 11f;
            }
            if (finalSpeed < 300f)
            {
                NPC.velocity.X = (NPC.velocity.X * 7f + dirX) / 8f;
                NPC.velocity.Y = (NPC.velocity.Y * 7f + dirY) / 8f;
            }
            NPC.rotation = NPC.velocity.X * 0.15f;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
        new FlavorTextBestiaryInfoElement("A prismatic living ice crystal. Though typically glimpsed only through the harsh sleet of blizzards, on the rare days where it is seen during a sunny day, its body gleams a deadly, beautiful blue.")
            });
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.soundDelay == 0)
            {
                NPC.soundDelay = 3;
                SoundEngine.PlaySound(CalamityMod.NPCs.Cryogen.Cryogen.HitSound, NPC.Center);
            }
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.IceGolem, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.IceGolem, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<EssenceofEleum>(), 1, 4, 8);
        }
        public override void OnKill()
        {
            RemixDowned.downedOrigen = true;
            CalRemixWorld.UpdateWorldBool();
        }

        public override bool SpecialOnKill()
        {
            // work you stupid stupid
            RemixDowned.downedOrigen = true;
            CalRemixWorld.UpdateWorldBool();
            return false;
        }
    }
}
