using CalamityMod.Buffs.DamageOverTime;
using CalRemix.Content.Items.Materials;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.UI
{
    public partial class ScreenHelperManager : ModSystem
    {
        public static void LoadBabil()
        {
            //Fanny's lines
            var firstMessage = HelperMessage.New("Babil1", "Hey there, adventurer! Have you heard about the Essence of Babil? It's this amazing crafting material that drops from certain jungle creatures. Let me give you some tips on how to find it!",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => Main.hardMode && Main.LocalPlayer.ZoneJungle).SetHoverTextOverride("Umm, Essence of Babil?").InitiateConversation();

            HelperMessage.New("Babil2", "Oh you sweet summer child! The Essence of Babil is this incredible, mystical substance you can gather from jungle enemies. It's a key ingredient for crafting some seriously awesome gear. You should definitely try to collect it!",
                "FannyNuhuh").SetHoverTextOverride("Huh, okay. So, where do I find it?").ChainAfter(delay: 2).AddItemDisplay(ModContent.ItemType<EssenceofBabil>());

            HelperMessage.New("Babil3", "Seriously? I just told you, it drops from jungle creatures. You know, those critters lurking around in the jungle biome? Go hunt them down, and you might get your hands on some Essence of Babil!",
                "FannyNuhuh").SetHoverTextOverride("Jungle creatures... got it!").ChainAfter();

            HelperMessage.New("Babil4", "The Essence of Babil used for crafting powerful items. You can create some fantastic air-themed equipment with it. Seriously, just check the crafting menu, it's all there!",
                "FannyNuhuh").SetHoverTextOverride("I'm not sure I understand...").ChainAfter(delay: 5).AddItemDisplay(ModContent.ItemType<EssenceofBabil>());

            HelperMessage.New("Babil5", "Okay, let me spell it out for you one more time. Essence of Babil is a crafting material. You find it in the jungle. You use it to make cool stuff. Got it now? Good!",
                "FannyNuhuh").SetHoverTextOverride("Uh, thanks, Fanny. I think I get it now.").ChainAfter().AddItemDisplay(ModContent.ItemType<EssenceofBabil>());

            //Evil fanny's lines
            HelperMessage.New("Babil6", "Hey there, adventurer... Have you heard about the Essence of Babil? It's this... remarkable crafting material that drops from such unworthy jungle creatures. Let me grace you with some information, whether you appreciate it or not.",
                "EvilFannyPoint").SetHoverTextOverride("Umm, Essence of Babil? What's that?").SpokenByEvilFanny().ChainAfter(firstMessage, 3f, true);
            HelperMessage.New("Babil7", "Oh, how utterly clueless. The Essence of Babil is this incredibly mundane substance you can get from jungle enemies. You might even consider it somewhat important for crafting marginally useful gear. But, hey, who cares, right?",
                "EvilFannyIdle").SetHoverTextOverride("Huh, okay. So, where do I find it?").ChainAfter().SpokenByEvilFanny();
            HelperMessage.New("Babil8", "Seriously? I can't believe I have to repeat myself. It drops from those jungle creatures, assuming you can manage to defeat them, of course. Go ahead, give it a shot. Not like it matters.",
                "EvilFannyIdle").SetHoverTextOverride("Jungle creatures... got it. But what can I make with it?").ChainAfter(delay: 5).SpokenByEvilFanny();
            HelperMessage.New("Babil9", "You're really pushing your limits here, aren't you? It's used to craft... well, whatever. You can create some totally average air-themed equipment. But, honestly, who cares about that, right?",
                "EvilFannyIdle").SetHoverTextOverride("I'm not sure I understand...").ChainAfter().SpokenByEvilFanny();
            HelperMessage.New("Babil10", "Of course, you don't!!! Why would I expect any different? Essence of Babil is just a crafting material. You find it in the jungle. You use it to make \"cool\" stuff, if you're into that sort of thing. But, frankly, I couldn't care less.",
                "EvilFannyPissed").SetHoverTextOverride("Uh, thanks, Evil Fanny. I think I get it now.").ChainAfter(delay: 2).SpokenByEvilFanny();
            HelperMessage.New("Babil11", "You think you \"get it\"? You're beyond hopeless! There, now you're truly enlightened. Enjoy your essence... of oblivion!",
                "EvilFannyWrathful", HelperMessage.AlwaysShow, duration: 20).ChainAfter().SpokenByEvilFanny().AddStartEvent(EssenceOfOblivionEvilFanny).EndConversation();

        }

        private static void EssenceOfOblivionEvilFanny()
        {
            Main.LocalPlayer.AddBuff(ModContent.BuffType<VulnerabilityHex>(), 216000);
            Main.LocalPlayer.AddBuff(ModContent.BuffType<AstralInfectionDebuff>(), 216000);
            Main.LocalPlayer.AddBuff(ModContent.BuffType<BurningBlood>(), 216000);
            Main.LocalPlayer.AddBuff(ModContent.BuffType<GodSlayerInferno>(), 216000);
            Main.LocalPlayer.AddBuff(ModContent.BuffType<HolyFlames>(), 216000);
            Main.LocalPlayer.AddBuff(ModContent.BuffType<MiracleBlight>(), 216000);
            Main.LocalPlayer.statLife = Math.Max(Main.LocalPlayer.statLife, Main.LocalPlayer.statLifeMax2 / 4);

            for (int i = 0; i < 5; i++)
                SoundEngine.PlaySound(SoundID.Thunder with { MaxInstances = 0 });
        }
    }
}