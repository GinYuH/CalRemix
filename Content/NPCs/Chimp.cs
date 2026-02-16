using CalamityMod;
using CalRemix.Core.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
//using CalamityMod.CalPlayer;

namespace CalRemix.Content.NPCs
{
    public class Chimp : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 1;
        }

        public static SoundStyle Monkey = new SoundStyle("CalRemix/Assets/Sounds/Chimp/Monkey", 7) { Pitch = 1.3f, PitchVariance = 0.3f, MaxInstances = 0 };
        public static SoundStyle MonkeyRare = new SoundStyle("CalRemix/Assets/Sounds/Chimp/MonkeyRare") { Pitch = 1.3f, PitchVariance = 0.3f, MaxInstances = 0 };

        public override void SetDefaults()
        {
            NPC.width = 52;
            NPC.height = 38;
            NPC.CloneDefaults(NPCID.Herpling);
            NPC.lavaImmune = false;
            AIType = NPCID.Herpling;
            NPC.HitSound = new SoundStyle("CalRemix/Assets/Sounds/Chimp/MonkeyHurt", 4) { Pitch = 1.3f, PitchVariance = 0.3f };
            NPC.DeathSound = new SoundStyle("CalRemix/Assets/Sounds/Chimp/MonkeyDeath", 3) { Pitch = 1.3f, PitchVariance = 0.3f };
            NPC.lifeMax = 160;
            NPC.defense = 5;
            NPC.damage = 30;
            NPC.Calamity().VulnerableToCold = true;
            NPC.Calamity().VulnerableToSickness = true;
            NPC.Calamity().VulnerableToHeat = true;
        }

        public override void AI()
        {
            NPC.spriteDirection = -NPC.direction;

            if (Main.rand.NextBool(50))
            {
                SoundEngine.PlaySound(Monkey, NPC.Center);
            }
            if (Main.rand.NextBool(600))
            {
                SoundEngine.PlaySound(MonkeyRare, NPC.Center);
            }
            if (NPC.velocity.Y < 0)
            {
                NPC.rotation = MathHelper.Min(NPC.rotation + 0.1f, MathHelper.PiOver4);
            }
            else if (NPC.velocity.Y > 0)
            {
                NPC.rotation = MathHelper.Max(NPC.rotation - 0.1f, -MathHelper.PiOver4);
            }
            else
            {
                NPC.rotation = Utils.AngleLerp(NPC.rotation, 0, 0.05f);
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundJungle,
                new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (CalRemixWorld.vernalTiles >= 50)
                return SpawnCondition.UndergroundJungle.Chance * 0.05f;
            return 0;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D tex = TextureAssets.Npc[Type].Value;
            float rotOff = 0; // NPC.spriteDirection == -1 ? MathHelper.Pi : 0;
            spriteBatch.Draw(tex, NPC.Center - screenPos, null, NPC.GetAlpha(drawColor), NPC.rotation + rotOff, tex.Size() / 2, NPC.scale, NPC.FlippedEffects(), 0);
            return false;
        }
    }
}