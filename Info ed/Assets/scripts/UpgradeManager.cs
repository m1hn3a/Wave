using UnityEngine;

public static class UpgradeManager
{
    public static int dashLevel = 0;
    public static int speedLevel = 0;
    public static int pierceLevel = 0;
    public static FireMode fireMode = FireMode.SingleReload;

    // NECESAR pentru upgrade-uri
    public static int tokens = 0;

    // Nu mai există salvări → funcția rămâne goală
    public static void SaveAll()
    {
        // Intenționat gol
    }
}
