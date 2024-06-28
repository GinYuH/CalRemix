using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix
{
    public static class CalRemixHelper
    {
        public static bool HasStack(this Player player, int itemType, int stackNum)
        {
            for (int i = 0; i < 58; i++)
            {
                Item item = player.inventory[i];
                if (item.type == itemType) { if (item.stack >= stackNum) return true; }
            }
            return false;
        }
        public static void ConsumeStack(this Player player, int itemType, int stackNum)
        {
            for(int i = 0; i < 58; i++)
            {
                ref Item item = ref player.inventory[i];
                if (player.HasStack(itemType, stackNum)) item.stack -= stackNum;
            }
        }

        public static bool HasItems(this Player player, List<int> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (!player.HasItem(items[i]))
                {
                    return false;
                }
            }
            return true;
        }
        public static bool HasCrossModItem(Player player, string ModName, string ItemName)
        {
            if (ModLoader.HasMod(ModName))
            {
                if (player.HasItem(ModLoader.GetMod(ModName).Find<ModItem>(ItemName).Type))
                    return true;
            }
            return false;
        }

        public static CalRemixGlobalNPC Remix(this NPC npc)
        {
            return npc.GetGlobalNPC<CalRemixGlobalNPC>();
        }

    }

    public static class BetterSoundID
    {
        public static SoundStyle ItemSwing => SoundID.Item1;
        public static SoundStyle ItemEat => SoundID.Item2;
        public static SoundStyle ItemDrink => SoundID.Item3;
        public static SoundStyle ItemLifeCrystal => SoundID.Item4;
        public static SoundStyle ItemBow => SoundID.Item5;
        public static SoundStyle ItemTeleportMirror => SoundID.Item6;
        public static SoundStyle ItemCast => SoundID.Item8;
        public static SoundStyle ItemMagicStar => SoundID.Item9;
        public static SoundStyle ItemWeakLaunch => SoundID.Item10;
        public static SoundStyle ItemBasicGun => SoundID.Item11;
        public static SoundStyle ItemSpaceLaser => SoundID.Item12;
        public static SoundStyle ItemMagicStream => SoundID.Item13;
        public static SoundStyle ItemExplosion => SoundID.Item14;
        public static SoundStyle ItemPhaseblade => SoundID.Item15;
        public static SoundStyle ItemFart => SoundID.Item16;
        public static SoundStyle ItemSpit => SoundID.Item17;
        public static SoundStyle ItemSpit2 => SoundID.Item18;
        public static SoundStyle ItemSpit3 => SoundID.Item19;
        public static SoundStyle ItemFireball => SoundID.Item20;
        public static SoundStyle ItemWaterBolt => SoundID.Item21;
        public static SoundStyle ItemDrill => SoundID.Item22;
        public static SoundStyle ItemDrill2 => SoundID.Item23;
        public static SoundStyle ItemRocketHover => SoundID.Item24;
        public static SoundStyle ItemMagicMount => SoundID.Item25;
        public static SoundStyle ItemHarp => SoundID.Item26;
        public static SoundStyle ItemIceBreak => SoundID.Item27;
        public static SoundStyle ItemRainbowRodIce => SoundID.Item28;
        public static SoundStyle ItemManaCrystal => SoundID.Item29;
        public static SoundStyle ItemMagicIceBlock => SoundID.Item30;
        public static SoundStyle ItemClockworkGun => SoundID.Item31;
        public static SoundStyle ItemWingFlap => SoundID.Item32;
        public static SoundStyle ItemThisStupidFuckingLaser => SoundID.Item33;
        public static SoundStyle ItemFlamethrower => SoundID.Item34;
        public static SoundStyle ItemBell => SoundID.Item35;
        public static SoundStyle ItemShotgun => SoundID.Item36;
        public static SoundStyle ItemReforge => SoundID.Item37;
        public static SoundStyle ItemExplosiveShotgun => SoundID.Item38;
        public static SoundStyle ItemFlingRazorpine => SoundID.Item39;
        public static SoundStyle ItemSniperGun => SoundID.Item40;
        public static SoundStyle ItemMachineGun => SoundID.Item41;
        public static SoundStyle ItemMissileFireSqueak => SoundID.Item42;
        public static SoundStyle ItemMagicStaff => SoundID.Item43;
        public static SoundStyle ItemSummonWeapon => SoundID.Item44;
        public static SoundStyle ItemFireballImpact => SoundID.Item45;
        public static SoundStyle ItemSentrySummon => SoundID.Item46;
        public static SoundStyle ItemAxeGuitar => SoundID.Item47;
        public static SoundStyle ItemSnowHit => SoundID.Item48;
        public static SoundStyle ItemSnowHit2 => SoundID.Item49;
        public static SoundStyle ItemIceHit => SoundID.Item50;
        public static SoundStyle ItemSnowballHit => SoundID.Item51;
        public static SoundStyle ItemMinecartHit => SoundID.Item52;
        public static SoundStyle ItemMinecartCling => SoundID.Item53;
        public static SoundStyle ItemBubblePop => SoundID.Item54;
        public static SoundStyle ItemMinecartSlowdown => SoundID.Item55;
        public static SoundStyle ItemMinecartBounce => SoundID.Item56;
        public static SoundStyle ItemMeowBounce => SoundID.Item57;
        public static SoundStyle ItemMeowBounce2 => SoundID.Item58;
        public static SoundStyle ItemPigOink => SoundID.Item59;
        public static SoundStyle ItemTerraBeam => SoundID.Item60;
        public static SoundStyle ItemGrenadeChuck => SoundID.Item61;
        public static SoundStyle ItemGrenadeExplosion => SoundID.Item62;
        public static SoundStyle ItemBlowpipe => SoundID.Item63;
        public static SoundStyle ItemBlowgunGrandDesign => SoundID.Item64;
        public static SoundStyle ItemBlowReload => SoundID.Item65;
        public static SoundStyle ItemNimbusRain => SoundID.Item66;
        public static SoundStyle ItemRainbowGun => SoundID.Item67;
        public static SoundStyle ItemRainbowGun2 => SoundID.Item68;
        public static SoundStyle ItemStaffofEarth => SoundID.Item69;
        public static SoundStyle ItemBoulderImpact => SoundID.Item70;
        public static SoundStyle ItemDeathSickle => SoundID.Item71;
        public static SoundStyle ItemShadowbeamStaff => SoundID.Item72;
        public static SoundStyle ItemInfernoFork => SoundID.Item73;
        public static SoundStyle ItemInfernoExplosion => SoundID.Item74;
        public static SoundStyle ItemPulseBowLaser => SoundID.Item75;
        public static SoundStyle ItemHornetSummon => SoundID.Item76;
        public static SoundStyle ItemImpSummon => SoundID.Item77;
        public static SoundStyle ItemSentrySummonStrong => SoundID.Item78;
        public static SoundStyle ItemBunnyMountSummon => SoundID.Item79;
        public static SoundStyle ItemTruffleMountSummon => SoundID.Item80;
        public static SoundStyle ItemSlimeMountSummon => SoundID.Item81;
        public static SoundStyle ItemOpticStaffSummon => SoundID.Item82;
        public static SoundStyle ItemSpiderStaffSummon => SoundID.Item83;
        public static SoundStyle ItemRazorbladeTyphoon => SoundID.Item84;
        public static SoundStyle ItemBubbleGun => SoundID.Item85;
        public static SoundStyle ItemBubbleGun2 => SoundID.Item86;
        public static SoundStyle ItemBubbleGun3 => SoundID.Item87;
        public static SoundStyle ItemMeteorStaffLunarFlare => SoundID.Item88;
        public static SoundStyle ItemMeteorImpact => SoundID.Item89;
        public static SoundStyle ItemBrainScrambler => SoundID.Item90;
        public static SoundStyle ItemLaserMachinegun => SoundID.Item91;
        public static SoundStyle ItemElectrospherePetSummon => SoundID.Item92;
        public static SoundStyle ItemElectricFizzleExplosion => SoundID.Item93;
        public static SoundStyle ItemElectricMineSettle => SoundID.Item94;
        public static SoundStyle ItemXenopopper => SoundID.Item95;
        public static SoundStyle ItemXenopopperPop => SoundID.Item96;
        public static SoundStyle ItemBeesKnees => SoundID.Item97;
        public static SoundStyle ItemDartPistol => SoundID.Item98;
        public static SoundStyle ItemDartRifle => SoundID.Item99;
        public static SoundStyle ItemClingerStaff => SoundID.Item100;
        public static SoundStyle ItemCrystalVileShard => SoundID.Item101;
        public static SoundStyle ItemAerialBane => SoundID.Item102;
        public static SoundStyle ItemShadowflameHexDoll => SoundID.Item103;
        public static SoundStyle ItemShadowflameHexDoll2 => SoundID.Item104;
        public static SoundStyle ItemStarWrath => SoundID.Item105;
        public static SoundStyle ItemToxicFlaskThrow => SoundID.Item106;
        public static SoundStyle ItemToxicFlaskImpact => SoundID.Item107;
        public static SoundStyle ItemNailGun => SoundID.Item108;
        public static SoundStyle ItemCrystalSerpent => SoundID.Item109;
        public static SoundStyle ItemCrystalSerpentImpact => SoundID.Item110;
        public static SoundStyle ItemToxikarp => SoundID.Item111;
        public static SoundStyle ItemToxikarp2 => SoundID.Item112;
        public static SoundStyle ItemDeadlySphereVroom => SoundID.Item113;
        public static SoundStyle ItemPortalGun => SoundID.Item114;
        public static SoundStyle ItemPortalGun2 => SoundID.Item115;
        public static SoundStyle ItemSolarEruption => SoundID.Item116;
        public static SoundStyle ItemNebulaArcanum => SoundID.Item117;
        public static SoundStyle ItemCrystalChargeImpact => SoundID.Item118;
        public static SoundStyle ItemPhantasmDragon => SoundID.Item119;
        public static SoundStyle ItemIceMistCultist => SoundID.Item120;
        public static SoundStyle ItemCultistUnused => SoundID.Item121;
        public static SoundStyle ItemLightningOrbCultist => SoundID.Item122;
        public static SoundStyle ItemPhantasmalBolt => SoundID.Item123;
        public static SoundStyle ItemPhantasmalBolt2 => SoundID.Item124;

    }
}
