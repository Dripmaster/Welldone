using UnityEngine;

namespace Com.CrossLab.WellDone
{
    public class WellDone
    {
        public const float WellDone_MIN_SPAWN_TIME = 5.0f;
        public const float WellDone_MAX_SPAWN_TIME = 10.0f;

        public const float PLAYER_RESPAWN_TIME = 4.0f;

        public const int PLAYER_MAX_LIVES = 3;

        public const string PLAYER_LIVES = "PlayerLives";
        public const string PLAYER_READY = "IsPlayerReady";
        public const string PLAYER_LOADED_LEVEL = "PlayerLoadedLevel";

        public static Color GetColor(int colorChoice)
        {
            switch (colorChoice)
            {
                case 0: return Color.red;
                case 1: return Color.green;
                case 2: return Color.blue;
                case 3: return Color.yellow;
                case 4: return Color.cyan;
                case 5: return Color.grey;
                case 6: return Color.magenta;
                case 7: return Color.white;
            }

            return Color.black;
        }
        public static string GetName(PlacementType typeChoice)
        {
            switch (typeChoice)
            {
                case PlacementType.DOMA:
                    return "��";
                case PlacementType.PAN:
                    return "����";
                case PlacementType.BOWL:
                    return "����";
                case PlacementType.DISH:
                    return "";
                case PlacementType.POT:
                    return "����";
                case PlacementType.ICEPOT:
                    return "��";
                case PlacementType.TOMATO:
                    return "�丶��";
                case PlacementType.MEAT:
                    return "���";
                case PlacementType.GARLIC:
                    return "����";
                case PlacementType.CABBAGE:
                    return "�����";
                case PlacementType.FISH:
                    return "����";
                case PlacementType.PEANUT:
                    return "����";
            }

            return "";
        }
    }
    public enum PlacementType
    {
        NONE = 0,
        DOMA = 1,
        PAN,
        BOWL,

        DISH,
        POT,
        ICEPOT,

        TOMATO,
        MEAT,
        GARLIC,
        CABBAGE,
        FISH,
        PEANUT
    };
}