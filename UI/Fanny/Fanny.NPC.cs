using CalamityMod.NPCs.AcidRain;
using CalamityMod.NPCs.HiveMind;
using CalamityMod.NPCs.Leviathan;
using CalamityMod.NPCs.NormalNPCs;
using CalamityMod.NPCs.Perforator;
using CalamityMod.NPCs.ProfanedGuardians;
using CalamityMod.NPCs.SupremeCalamitas;
using CalRemix.Items.Materials;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.UI
{
    public partial class FannyManager : ModSystem
    {
        public static void LoadNPCMessages()
        {
            fannyMessages.Add(new FannyMessage("WulfrumPylone",
                "Woah, you hear that? No? Well it sounded like something big... we should get it's attention. A dose of some of that tower over there's energy in a special chest might be just the motivation it needs to come to the surface!",
                "Idle",
                (FannySceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<WulfrumAmplifier>() && !CalRemixWorld.downedExcavator), onlyPlayOnce: false, cooldown: 2400).AddItemDisplay(ItemID.LivingWoodChest));

            fannyMessages.Add(new FannyMessage("Cysts",
                "That pimple thing looks useless, but it drops a very useful material. Please kill it!",
                "Awooga",
                (FannySceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<HiveTumor>() || n.type == ModContent.NPCType<PerforatorCyst>())));

            FannyMessage pumpking1 = new FannyMessage("Pumpking1", "Wh- Ahh! AAAAAAAAAAAAH!!",
                "Awooga", (FannySceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == NPCID.Pumpking), 3, needsToBeClickedOff: false).SetHoverTextOverride("What?");

            fannyMessages.Add(pumpking1);

            FannyMessage pumpking2 = new FannyMessage("Pumpking2", "I told Fanny as a joke that jack-o-lanterns get their lights by eating flames. Don't tell him, though. It's funnier this way.",
                "EvilIdle", FannyMessage.AlwaysShow).AddDelay(2).NeedsActivation().SetHoverTextOverride("Sure? I might tell Fanny later...");

            pumpking1.AddEndEvent(() => pumpking2.ActivateMessage());

            fannyMessages.Add(pumpking2);

            // Empress
            FannyMessage eol1 = new FannyMessage("EoL1", "So, there's a second boss for the Hallow... Then where's the second boss for the other biomes? Did they just like this one more than the others?",
                "EvilIdle", (FannySceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == NPCID.HallowBoss), 8, needsToBeClickedOff: false, displayOutsideInventory: true).SpokenByEvilFanny();

            fannyMessages.Add(eol1);

            FannyMessage eol2 = new FannyMessage("EoL2", "... then again, the other boss is a recolor. Maybe for the best.",
                "EvilIdle", FannyMessage.AlwaysShow, 6, needsToBeClickedOff: false, displayOutsideInventory: true).SpokenByEvilFanny().NeedsActivation();

            eol1.AddEndEvent(() => eol2.ActivateMessage());

            fannyMessages.Add(eol2);

            FannyMessage eol3 = new FannyMessage("EoL3", "Just like you!",
                "Nuhuh", FannyMessage.AlwaysShow, 5, needsToBeClickedOff: false, displayOutsideInventory: true).NeedsActivation();

            eol2.AddEndEvent(() => eol3.ActivateMessage());

            fannyMessages.Add(eol3);

            FannyMessage eol4 = new FannyMessage("EoL4", "...",
                "EvilIdle", FannyMessage.AlwaysShow, 5, needsToBeClickedOff: false, displayOutsideInventory: true).SpokenByEvilFanny().NeedsActivation();

            eol3.AddEndEvent(() => eol4.ActivateMessage());

            fannyMessages.Add(eol4); 
            
            FannyMessage eol5 = new FannyMessage("EoL5", "Bitch",
                "EvilIdle", (FannySceneMetrics scene) => ChildSafety.Disabled, 5, onlyPlayOnce: false, displayOutsideInventory: true).SpokenByEvilFanny().NeedsActivation();

            eol4.AddEndEvent(() => eol5.ActivateMessage());

            fannyMessages.Add(eol5);

            fannyMessages.Add(new FannyMessage("Deimos", "That \"Deimos\" over there. She has some delicious Mars Bars, you should go buy some!",
    "Idle", (FannySceneMetrics scene) => CrossModBoss(scene, "EverquartzAdventure", "StarbornPrincess")).SetHoverTextOverride("Thanks Fanny! I'll buy you plenty of Mars Bars!"));

            fannyMessages.Add(new FannyMessage("Anauwu", "I sense an ominous presence. I think the best course of action here would be to kill everything you see. If something is dead it can't hurt you!",
                "Idle", (FannySceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<LeviathanStart>())));

            fannyMessages.Add(new FannyMessage("Fairy", "That thing is hurting my eyes! Kill it, quick!",
                "Sob", (FannySceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == NPCID.EmpressButterfly)));

            fannyMessages.Add(new FannyMessage("Cultists", "Looks like some blue robe-wearing hooligans are worshiping a coin! Try not to interrupt them, they seem to be having a good time.",
                "Nuhuh", (FannySceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == NPCID.CultistDevote)));

            fannyMessages.Add(new FannyMessage("AncientDom", "Who is this guy???",
                "Sob", (FannySceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == NPCID.AncientCultistSquidhead)));

            fannyMessages.Add(new FannyMessage("PGuardians", "It seems like these mischievous scoundrels are up to no good, and plan to burn all the delicious meat! We gotta go put an end to their plan of calamity!",
                "Nuhuh", (FannySceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<ProfanedGuardianCommander>())));

            fannyMessages.Add(new FannyMessage("Bloodworm", "Crush it under your boot.",
                "Idle", (FannySceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<BloodwormNormal>() || n.type == ModContent.NPCType<BloodwormFleeing>())));

            fannyMessages.Add(new FannyMessage("Wolf", "Aw look a cute wolf! You can extract valuable Coyote Venom from their lifeless corpses in order to make some neat ice items.",
    "Idle", (FannySceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == NPCID.Wolf)).AddItemDisplay(ModContent.ItemType<CoyoteVenom>()));

            fannyMessages.Add(new FannyMessage("Dungeondie", "Oh, it appears my hack didn't work.",
    "Sob", (FannySceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == NPCID.DungeonGuardian) && Main.LocalPlayer.dead));

            fannyMessages.Add(new FannyMessage("Scaldie", "Are you hurt? That was calamitous!",
    "Sob", (FannySceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<SupremeCalamitas>()) && Main.LocalPlayer.dead));
        }
    }
}