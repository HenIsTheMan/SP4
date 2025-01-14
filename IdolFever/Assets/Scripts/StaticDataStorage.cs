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

        public static bool CardBack = false;
        public static bool isFlipped = false;
        public static bool R_Girl = false;
        public static bool R_Boy = false;
        public static bool SR_Girl = false;
        public static bool SR_Boy = false;
        public static bool SSR_Girl = false;
        public static bool SSR_Boy = false;

        public static int R_GirlDrawCount = 0;
        public static int R_BoyDrawCount = 0;
        public static int SR_GirlDrawCount = 0;
        public static int SR_BoyDrawCount = 0;
        public static int SSR_GirlDrawCount = 0;
        public static int SSR_BoyDrawCount = 0;

        public static int roundPlayed = 0;
        public static int roundMulti = 0;
        public static int dailyDone = 0;

        public static string nowTime = "50";
        public static string nextRound = "50";
        public static string nextMulti = "300";
        public static string nextAll = "10";

        public static int gems = 0;

        
        

        //tasks
        
    }

}