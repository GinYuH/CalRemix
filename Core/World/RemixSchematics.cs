using CalamityMod;
using CalamityMod.Schematics;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Terraria.ModLoader;

namespace CalRemix.Core.World
{
    public class RemixSchematics : ModSystem
    {
        internal const string IonAltarName = "Core/Schematics/ionaltar.csch";
        internal const string OrigenWorkshopName = "Core/Schematics/origenworkshop.csch";
        internal const string HallowShrineName = "Core/Schematics/hallowshrine.csch";
        internal const string FrozenStrongholdName = "Core/Schematics/FrozenStrongholdLocked.csch";
        internal const string CrimsonHeartName = "Core/Schematics/CrimsonHeart.csch";
        internal const string PiggyStrawName = "Core/Schematics/PiggyStraw.csch";
        internal const string PiggyStickName = "Core/Schematics/PiggyStick.csch";
        internal const string PiggyBrickName = "Core/Schematics/PiggyBrick.csch";
        internal const string TurnipName = "Core/Schematics/turnip.csch";
        internal const string PlumestoneName = "Core/Schematics/plumestoneVolcano.csch";
        internal const string SealedHouseSmallName = "Core/Schematics/sealedHouseSmall.csch";
        internal const string SealedHouseLargeName = "Core/Schematics/sealedHouseLarge.csch";
        internal const string SealedHouseLibraryName = "Core/Schematics/sealedHouseLibrary.csch";
        internal const string SealedHouseChurchName = "Core/Schematics/sealedHouseChurch.csch";
        internal const string SealedCitadelName = "Core/Schematics/sealedCitadel.csch";
        internal const string SealedChamberName = "Core/Schematics/sealedChamber.csch";
        internal const string MonorianShrineName = "Core/Schematics/monorianShrine.csch";
        internal const string BrightShrineName = "Core/Schematics/brightShrine.csch";
        internal const string GrayTempleName = "Core/Schematics/grayTemple.csch";

        internal static Dictionary<string, SchematicMetaTile[,]> TileMaps =>
            typeof(SchematicManager).GetField("TileMaps", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null) as Dictionary<string, SchematicMetaTile[,]>;

        internal static readonly MethodInfo ImportSchematicMethod = typeof(CalamitySchematicIO).GetMethod("ImportSchematic", BindingFlags.NonPublic | BindingFlags.Static);


        public override void PostSetupContent()
        {
            TileMaps.Add("Ion Altar", LoadSchematic(IonAltarName).ShaveOffEdge());
            TileMaps.Add("Origen Workshop", LoadSchematic(OrigenWorkshopName).ShaveOffEdge());
            TileMaps.Add("Hallow Shrine", LoadSchematic(HallowShrineName).ShaveOffEdge());
            TileMaps.Add("Frozen Stronghold", LoadSchematic(FrozenStrongholdName).ShaveOffEdge());
            TileMaps.Add("Crimson Heart", LoadSchematic(CrimsonHeartName).ShaveOffEdge());
            TileMaps.Add("Piggy Straw", LoadSchematic(PiggyStrawName));
            TileMaps.Add("Piggy Stick", LoadSchematic(PiggyStickName));
            TileMaps.Add("Piggy Brick", LoadSchematic(PiggyBrickName));
            TileMaps.Add("Plumestone", LoadSchematic(PlumestoneName));
            TileMaps.Add("Turnip", LoadSchematic(TurnipName));
            TileMaps.Add("Sealed House Small", LoadSchematic(SealedHouseSmallName));
            TileMaps.Add("Sealed House Large", LoadSchematic(SealedHouseLargeName));
            TileMaps.Add("Sealed House Library", LoadSchematic(SealedHouseLibraryName));
            TileMaps.Add("Sealed House Church", LoadSchematic(SealedHouseChurchName));
            TileMaps.Add("Sealed Citadel", LoadSchematic(SealedCitadelName));
            TileMaps.Add("Sealed Chamber", LoadSchematic(SealedChamberName));
            TileMaps.Add("Monorian Shrine", LoadSchematic(MonorianShrineName));
            TileMaps.Add("Bright Shrine", LoadSchematic(BrightShrineName));
            TileMaps.Add("Gray Temple", LoadSchematic(GrayTempleName));
        }


        public static SchematicMetaTile[,] LoadSchematic(string filename)
        {
            SchematicMetaTile[,] ret = null;
            using (Stream st = CalRemix.instance.GetFileStream(filename, true))
                ret = (SchematicMetaTile[,])ImportSchematicMethod.Invoke(null, [st]);

            return ret;
        }

    }
}