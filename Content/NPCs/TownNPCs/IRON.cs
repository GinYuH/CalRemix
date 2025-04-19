using System;
using System.Collections.Generic;
using CalamityMod.BiomeManagers;
using CalamityMod.Graphics.Primitives;
using CalamityMod.Projectiles.Boss;
using CalamityMod.Projectiles.Ranged;
using CalRemix.Core.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Events;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace CalRemix.Content.NPCs.TownNPCs
{
    [AutoloadHead]
    public class IRON : ModNPC
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Archmagian");
            Main.npcFrameCount[NPC.type] = 25;
            NPCID.Sets.ExtraFramesCount[NPC.type] = 9;
            NPCID.Sets.AttackFrameCount[NPC.type] = 4;
            NPCID.Sets.DangerDetectRange[NPC.type] = 700;
            NPCID.Sets.AttackType[NPC.type] = 0;
            NPCID.Sets.AttackTime[NPC.type] = 90;
            NPCID.Sets.AttackAverageChance[NPC.type] = 30;
            NPCID.Sets.ShimmerTownTransform[Type] = false;
            NPC.Happiness
                .SetBiomeAffection<SulphurousSeaBiome>(AffectionLevel.Like)
                .SetBiomeAffection<JungleBiome>(AffectionLevel.Dislike)
                .SetNPCAffection(NPCID.Cyborg, AffectionLevel.Like)
                .SetNPCAffection(NPCID.Pirate, AffectionLevel.Dislike);
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Velocity = 1f // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, drawModifiers);
        }

        public override void SetDefaults()
        {
            NPC.townNPC = true;
            NPC.friendly = true;
            NPC.lavaImmune = true;
            NPC.width = 18;
            NPC.height = 40;
            NPC.aiStyle = NPCAIStyleID.Passive;
            NPC.damage = 10;
            NPC.defense = 15;
            NPC.lifeMax = 20000;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.8f;
            AnimationType = NPCID.Guide;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<SulphurousSeaBiome>().Type };
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }

        public override bool CanTownNPCSpawn(int numTownNPCs) => RemixDowned.downedIonogen;

        public override List<string> SetNPCNameList() => new List<string>() { "Surge" };

        public override string GetChat()
        {
            WeightedRandom<string> dialogue = new WeightedRandom<string>();

            dialogue.Add("Need a spark to brighten your day? Just say the word!");
            dialogue.Add("Quick on my feet and quicker with my wit. Care for a shockingly good time?");
            dialogue.Add("Power is all about control. Fortunately for you, I’ve got plenty of both.");

            if (!Main.dayTime)
            {
                dialogue.Add("Even in the dark, my energy never dims. Let's light up the night!");
                dialogue.Add("Nightfall can't slow me down. I thrive when the world goes quiet.");
            }

            if (BirthdayParty.PartyIsUp)
                dialogue.Add("Time to amp up the fun! Let’s make this celebration electrifying!");

            if (Main.bloodMoon)
            {
                dialogue.Add("A Blood Moon? Perfect time for a little extra voltage. Stay alert!");
                dialogue.Add("This crimson curse can’t dampen my spirits. Let’s turn up the power!");
            }

            if (Main.IsItStorming)
            {
                dialogue.Add("Ah, a lightning storm! Feels like home. Let’s channel this energy!");
                dialogue.Add("The storm's energy is invigorating. Keep up if you can!");
            }

            return dialogue;
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = "Shop";
        }

        public override void OnChatButtonClicked(bool firstButton, ref string shopName)
        {
            if (firstButton)
            {
                shopName = "Surge";
            }
        }

        public override void AddShops()
        {
           /* string wiz = NPCShopDatabase.GetShopNameFromVanillaIndex(7); // wizard index
            NPCShopDatabase.TryGetNPCShop(wiz, out AbstractNPCShop shope);
            NPCShop shopee = shope as NPCShop;

            NPCShop npcShop = new NPCShop(Type, "Surge");
            foreach (NPCShop.Entry entry in shopee.Entries)
            {
                if (entry.Item.type != ItemID.Harp)
                    npcShop.Add(entry);
            }
            npcShop.Register();*/
        }


        public override bool CanGoToStatue(bool toKingStatue) => toKingStatue;

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 20;
            knockback = 9f;
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 10;
            randExtraCooldown = 50;
        }

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = ModContent.ProjectileType<LuxorsGiftRanged>();
            attackDelay = 1;
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 2f;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            spriteBatch.Draw(ModContent.Request<Texture2D>(Texture).Value, NPC.Center - screenPos + new Vector2(0, NPC.gfxOffY) - new Vector2(0f, 6f), NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, new Vector2(ModContent.Request<Texture2D>(Texture).Value.Width / 2, ModContent.Request<Texture2D>(Texture).Value.Height / 2 / Main.npcFrameCount[NPC.type]), NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            Vector2 startPos = NPC.Center + new Vector2(-12 * NPC.direction, -20);
            List<Vector2> points = AresTeslaOrb.DetermineElectricArcPoints(startPos, startPos + new Vector2(0, -20), 250290787);
            PrimitiveRenderer.RenderTrail(points, new(BackgroundWidthFunction, BackgroundColorFunction, smoothen: false), 90);
            PrimitiveRenderer.RenderTrail(points, new(WidthFunction, ColorFunction, smoothen: false), 90);
            return false;
        }
        internal float WidthFunction(float completionRatio)
        {
            return completionRatio;
        }

        internal float BackgroundWidthFunction(float completionRatio) => WidthFunction(completionRatio) * 4f;

        public Color BackgroundColorFunction(float completionRatio) => Color.CornflowerBlue * 0.4f;

        internal Color ColorFunction(float completionRatio)
        {
            Color baseColor1 = Color.Yellow;
            Color baseColor2 = Color.Yellow;
            float fadeToWhite = MathHelper.Lerp(0f, 0.65f, (float)Math.Sin(MathHelper.TwoPi * completionRatio + Main.GlobalTimeWrappedHourly * 4f) * 0.5f + 0.5f);
            Color baseColor = Color.Lerp(baseColor1, Color.White, fadeToWhite);
            Color color = Color.Lerp(baseColor, baseColor2, ((float)Math.Sin(MathHelper.Pi * completionRatio + Main.GlobalTimeWrappedHourly * 4f) * 0.5f + 0.5f) * 0.8f) * 0.65f;
            color.A = 84;
            return color;
        }
    }
}
