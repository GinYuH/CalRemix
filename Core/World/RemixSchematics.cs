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
        internal const string IonAltarName = "Core/World/ionaltar.csch";
        internal const string OrigenWorkshopName = "Core/World/origenworkshop.csch";
        internal const string HallowShrineName = "Core/World/hallowshrine.csch";
        internal const string FrozenStrongholdName = "Core/World/FrozenStrongholdLocked.csch";
        internal const string CrimsonHeartName = "Core/World/CrimsonHeart.csch";
        internal const string PiggyStrawName = "Core/World/PiggyStraw.csch";
        internal const string PiggyStickName = "Core/World/PiggyStick.csch";
        internal const string PiggyBrickName = "Core/World/PiggyBrick.csch";
        internal const string TurnipName = "Core/World/turnip.csch";
        internal const string PlumestoneName = "Core/World/plumestoneVolcano.csch";
        internal const string SealedHouseSmallName = "Core/World/sealedHouseSmall.csch";
        internal const string SealedHouseLargeName = "Core/World/sealedHouseLarge.csch";
        internal const string SealedHouseLibraryName = "Core/World/sealedHouseLibrary.csch";
        internal const string SealedHouseChurchName = "Core/World/sealedHouseChurch.csch";
        internal const string SealedCitadelName = "Core/World/sealedCitadel.csch";
        internal const string SealedChamberName = "Core/World/sealedChamber.csch";
        internal const string MonorianShrineName = "Core/World/monorianShrine.csch";

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