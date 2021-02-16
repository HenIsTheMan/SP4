﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdolFever.UI
{
    public static class StaticDataStorage
    {
        public enum GAME_MODE
        {
            MODE_MENU,
            MODE_ONLINE,
            MODE_STORY
        }

        public static string LastSceneName
        {
            get { return lastSceneName; }
            set { lastSceneName = value; }
        }

        public static GAME_MODE GameMode
        {
            get { return gameMode; }
            set { gameMode = value; }
        }

        private static string lastSceneName;    // last scene name
        private static GAME_MODE gameMode;      // current game mode

        public static bool R_Girl = false;
    }

}