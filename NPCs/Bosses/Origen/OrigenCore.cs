using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using CalamityMod.World;
using CalamityMod.Events;
using CalamityMod.Items.Materials;
using System;
using CalRemix.World;
using CalRemix.Items.Placeables.Trophies;
using CalRemix.Items.Accessories;
using CalRemix.Items.Lore;
using CalRemix.Items.Weapons;

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
            NPC.value = Item.buyPrice(0, 0, 2, 0);
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
            // Generic setup
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
            // All of this is just Flocko code
            // I don't even know if any values are changed
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
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Snow,
        new FlavorTextBestiaryInfoElement("This element was once the first wielder of the Zero Point. After his untimely demise to a party of two John Wicks, Master Chief, and Rick, he was severed from the omniverse to bring upon superior clones of itself.")
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

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.LesserHealingPotion;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemID.Nazar, 1, 8, 10);
            npcLoot.Add(ItemID.FrozenKey, 3);
            npcLoot.Add(ModContent.ItemType<OrigenTrophy>(), 10);
            npcLoot.Add(ModContent.ItemType<SoulofOrigen>());
            npcLoot.Add(ModContent.ItemType<IOU>());
            npcLoot.Add(ModContent.ItemType<PaletteUncleanser>());
            npcLoot.AddConditionalPerPlayer(() => !RemixDowned.downedOrigen, ModContent.ItemType<KnowledgeOrigen>(), desc: DropHelper.FirstKillText);
        }
        public override void OnKill()
        {
            RemixDowned.downedOrigen = true;
            CalRemixWorld.UpdateWorldBool();
        }
    }
}
