using CalamityMod;
using CalamityMod.Items.SummonItems;
using CalamityMod.TileEntities;
using CalRemix.Content.Items.Weapons;
using CalRemix.Content.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;

namespace CalRemix.Content.NPCs.TownNPCs
{
    public class WALLSTREET : ModNPC
    {
        public override bool CanChat() => true;
        public override bool NeedSaving() => true;
        private static bool OnScreen(Vector2 center)
        {
            int w = NPC.sWidth + NPC.safeRangeX * 2;
            int h = NPC.sHeight + NPC.safeRangeY * 2;
            Rectangle npcScreenRect = new((int)center.X - w / 2, (int)center.Y - h / 2, w, h);
            foreach (Player player in Main.player)
                if (player.active && player.getRect().Intersects(npcScreenRect)) return true;
            return false;
        }
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.ArmsDealer];
            NPCID.Sets.ExtraFramesCount[Type] = NPCID.Sets.ExtraFramesCount[NPCID.ArmsDealer];
            NPCID.Sets.AttackFrameCount[Type] = NPCID.Sets.AttackFrameCount[NPCID.ArmsDealer];
            NPCID.Sets.DangerDetectRange[Type] = 700;
            NPCID.Sets.AttackType[Type] = 1;
            NPCID.Sets.AttackTime[Type] = 90;
            NPCID.Sets.AttackAverageChance[Type] = 10;
            NPCID.Sets.HatOffsetY[Type] = 4;
            NPCID.Sets.ActsLikeTownNPC[Type] = true;
            NPCID.Sets.ShimmerTownTransform[NPC.type] = false;
            NPCID.Sets.ShimmerTownTransform[Type] = false;
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new() { Velocity = 1f, Direction = 1 };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
        }
        public override void SetDefaults()
        {
            NPC.friendly = true;
            NPC.width = 18;
            NPC.height = 40;
            NPC.damage = 22;
            NPC.defense = 15;
            NPC.lifeMax = 600;
            NPC.knockBackResist = 0.5f;
            NPC.dontTakeDamage = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.aiStyle = NPCAIStyleID.Passive;
            AnimationType = NPCID.ArmsDealer;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime,
                new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }
        public override List<string> SetNPCNameList() => new List<string> { "Klepticarl" };
        
        public override string GetChat()
        {
            WeightedRandom<string> chat = new();
            if(Main.dayTime)
            {
                if(Main.eclipse)
                {
                    chat.Add("God damn it, it's all going to the Underworld! Sell! Sell!");
                    chat.Add("You've gotta sell, now! I haven't seen anything this bad since the second of February!");
                } else
                {
                    chat.Add("Buy low, sell high, get rich! That's the name of the game, my friend!");
                    chat.Add("Got a great feeling about today's market! Now's your chance to buy, buy, buy!");
                    chat.Add("You want to be rich? I know you want to be rich! Today's the day you're going to get rich if you buy now!");
                    chat.Add("Feeling lucky? Buy RMX! Either you'll make the biggest bucks, or you'll lose all your money... but doesn't it feel good to win big?");
                    chat.Add("Buy CAL! Buy CAL! Update's only a week away, I swear on all of my investments!");
                    if (Main.slimeRain) chat.Add("This slime's making GZM ooze faster than I've ever seen! That's pure liquid assets right there!", 3);
                    if (NPC.downedMoonlord) chat.Add("It's all going to the moon! How do I know? Because we brought the goddamn moon to us!");
                }
            } else
            {
                chat.Add("Market's closed, friend. But tonight's the perfect night to figure out what you're buying next.");
                chat.Add("Even though the market's asleep, I'm still awake to talk money. You ever thought about investing in crypto?");
            }
            return chat;
        }
        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = Language.GetTextValue("Stocks");
            Main.LocalPlayer.currentShoppingSettings.HappinessReport = "";
        }
        public override void OnChatButtonClicked(bool firstButton, ref string shop)
        {
            Main.npcChatText = "this hasn't been implemented yet";
        }

        public override bool CanGoToStatue(bool toKingStatue) => false; // stays on the gilded isle
        public override bool CanTownNPCSpawn(int numTownNPCs) => false;
        public override void AI()
        {
            NPC.homeless = true;
            NPC.dontTakeDamage = true;
        }
        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 20;
            knockback = 4f;
        }
        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 20;
            randExtraCooldown = 10;
        }
        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = ModContent.ProjectileType<AgentPoon>();
            attackDelay = 1;
        }
        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 44f;
            randomOffset = 0.05f;
        }
        public override void DrawTownAttackGun(ref Texture2D item, ref Rectangle itemFrame, ref float scale, ref int horizontalHoldoutOffset)
        {
            if (!NPC.downedMoonlord)
            {
                Main.GetItemDrawFrame(ItemID.Harpoon, out item, out itemFrame);
                horizontalHoldoutOffset = (int)Main.DrawPlayerItemPos(1, ItemID.Harpoon).X - 64;
            }
            else
            {
                Main.GetItemDrawFrame(ModContent.ItemType<Triploon>(), out item, out itemFrame);
                horizontalHoldoutOffset = (int)Main.DrawPlayerItemPos(1, ModContent.ItemType<Triploon>()).X - 64;
            }
        }
    }
}
