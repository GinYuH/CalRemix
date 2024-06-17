using CalamityMod.Buffs.DamageOverTime;
using CalRemix.Items.Materials;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.UI
{
    public partial class ScreenHelperMessageManager : ModSystem
    {
        public static void LoadBabil()
        {
            HelperMessage babil1 = new HelperMessage("Babil1", "Hey there, adventurer! Have you heard about the Essence of Babil? It's this amazing crafting material that drops from certain jungle creatures. Let me give you some tips on how to find it!",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => Main.hardMode && Main.LocalPlayer.ZoneJungle).SetHoverTextOverride("Umm, Essence of Babil?");

            screenHelperMessages.Add(babil1);

            HelperMessage babil2 = new HelperMessage("Babil2", "Oh you sweet summer child! The Essence of Babil is this incredible, mystical substance you can gather from jungle enemies. It's a key ingredient for crafting some seriously awesome gear. You should definitely try to collect it!",
                "FannyNuhuh", HelperMessage.AlwaysShow).SetHoverTextOverride("Huh, okay. So, where do I find it?").AddDelay(2).NeedsActivation().AddItemDisplay(ModContent.ItemType<EssenceofBabil>());

            babil1.AddEndEvent(() => babil2.ActivateMessage());

            screenHelperMessages.Add(babil2);

            HelperMessage babil3 = new HelperMessage("Babil3", "Seriously? I just told you, it drops from jungle creatures. You know, those critters lurking around in the jungle biome? Go hunt them down, and you might get your hands on some Essence of Babil!",
                "FannyNuhuh", HelperMessage.AlwaysShow).SetHoverTextOverride("Jungle creatures... got it!").NeedsActivation();

            babil2.AddEndEvent(() => babil3.ActivateMessage());

            screenHelperMessages.Add(babil3);

            HelperMessage babil4 = new HelperMessage("Babil4", "The Essence of Babil used for crafting powerful items. You can create some fantastic air-themed equipment with it. Seriously, just check the crafting menu, it's all there!",
                "FannyNuhuh", HelperMessage.AlwaysShow).SetHoverTextOverride("I'm not sure I understand...").AddDelay(5).NeedsActivation().AddItemDisplay(ModContent.ItemType<EssenceofBabil>());

            babil3.AddEndEvent(() => babil4.ActivateMessage());

            screenHelperMessages.Add(babil4);

            HelperMessage babil5 = new HelperMessage("Babil5", "Okay, let me spell it out for you one more time. Essence of Babil is a crafting material. You find it in the jungle. You use it to make cool stuff. Got it now? Good!",
                "FannyNuhuh", HelperMessage.AlwaysShow).SetHoverTextOverride("Uh, thanks, Fanny. I think I get it now.").NeedsActivation().AddItemDisplay(ModContent.ItemType<EssenceofBabil>());

            babil4.AddEndEvent(() => babil5.ActivateMessage());

            screenHelperMessages.Add(babil5);

            HelperMessage babil6 = new HelperMessage("Babil6", "Hey there, adventurer... Have you heard about the Essence of Babil? It's this... remarkable crafting material that drops from such unworthy jungle creatures. Let me grace you with some information, whether you appreciate it or not.",
                "EvilFannyIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.ZoneJungle).SetHoverTextOverride("Umm, Essence of Babil? What's that?").AddDelay(3).SpokenByEvilFanny();

            screenHelperMessages.Add(babil6);

            HelperMessage babil7 = new HelperMessage("Babil7", "Oh, how utterly clueless. The Essence of Babil is this incredibly mundane substance you can get from jungle enemies. You might even consider it somewhat important for crafting marginally useful gear. But, hey, who cares, right?",
                "EvilFannyIdle", HelperMessage.AlwaysShow).SetHoverTextOverride("Huh, okay. So, where do I find it?").NeedsActivation().SpokenByEvilFanny();

            babil6.AddEndEvent(() => babil7.ActivateMessage());

            screenHelperMessages.Add(babil7);

            HelperMessage babil8 = new HelperMessage("Babil8", "Seriously? I can't believe I have to repeat myself. It drops from those jungle creatures, assuming you can manage to defeat them, of course. Go ahead, give it a shot. Not like it matters.",
                "EvilFannyIdle", HelperMessage.AlwaysShow).SetHoverTextOverride("Jungle creatures... got it. But what can I make with it?").NeedsActivation().SpokenByEvilFanny().AddDelay(5);

            babil7.AddEndEvent(() => babil8.ActivateMessage());

            screenHelperMessages.Add(babil8);

            HelperMessage babil9 = new HelperMessage("Babil9", "You're really pushing your limits here, aren't you? It's used to craft... well, whatever. You can create some totally average air-themed equipment. But, honestly, who cares about that, right?",
                "EvilFannyIdle", HelperMessage.AlwaysShow).SetHoverTextOverride("I'm not sure I understand...").NeedsActivation().SpokenByEvilFanny();

            babil8.AddEndEvent(() => babil9.ActivateMessage());

            screenHelperMessages.Add(babil9);

            HelperMessage babil10 = new HelperMessage("Babil10", "Of course, you don't!!! Why would I expect any different? Essence of Babil is just a crafting material. You find it in the jungle. You use it to make \"cool\" stuff, if you're into that sort of thing. But, frankly, I couldn't care less.",
                "EvilFannyIdle", HelperMessage.AlwaysShow).SetHoverTextOverride("Uh, thanks, Evil Fanny. I think I get it now.").NeedsActivation().SpokenByEvilFanny().AddDelay(2);

            babil9.AddEndEvent(() => babil10.ActivateMessage());

            screenHelperMessages.Add(babil10);

            HelperMessage babil11 = new HelperMessage("Babil11", "You think you \"get it\"? You're beyond hopeless! There, now you're truly enlightened. Enjoy your essence... of oblivion!",
                "EvilFannyIdle", HelperMessage.AlwaysShow, duration: 20).NeedsActivation().SpokenByEvilFanny().AddStartEvent(EssenceOfOblivionEvilFanny);

            babil10.AddEndEvent(() => babil11.ActivateMessage());

            screenHelperMessages.Add(babil11);
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