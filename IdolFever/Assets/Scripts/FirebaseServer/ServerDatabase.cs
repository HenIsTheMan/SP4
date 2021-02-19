﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Firebase;
using Firebase.Auth;
using Firebase.Database;
using System.Linq;
using TMPro;

namespace IdolFever.Server
{

    public class ServerDatabase : MonoBehaviour
    {

        #region DATABASE_KEYS

        // a dozen of keys
        public const string DATABASE_USERS = "users";
        public const string DATABASE_USERNAME = "USERNAME";
        public const string DATABASE_GEM = "GEM";
        public const string DATABASE_ACHIEVEMENT = "ACHIEVEMENT";
        public const string DATABASE_ACHIEVEMENT_CLAIMED = "CLAIMED";
        public const string DATABASE_CHARACTER = "CHARACTER";
        public const string DATABASE_CHARACTER_NUMBER = "NUMBER";
        public const string DATABASE_ENERGY = "ENERGY";
        public const string DATABASE_LAST_LOGIN = "LAST_LOGIN";

        //public const string DATABASE_SONG_HIGHSCORE = "SONG_HIGHSCORE";
        #region SONG_DATABASE_KEYS
        public const string DBSH_MOUNTAIN_KING = "MOUNTAIN_KING_HIGHSCORE";
        public const string DBSH_ORIGINAL_SONG = "ORIGINAL_SONG_HIGHSCORE";
        public const string DBSH_WELLERMAN = "WELLERMAN_SONG_HIGHSCORE";
        #endregion

        #endregion

        #region FIREBASE

        //Firebase variables
        [Header("Firebase")]
        public DependencyStatus dependencyStatus;
        public FirebaseAuth auth;
        public FirebaseUser User;
        public DatabaseReference DBreference;

        #endregion

        void Awake()
        {
            Init();
        }

        private void Init()
        {
            auth = FirebaseAuth.DefaultInstance;
            User = FirebaseAuth.DefaultInstance.CurrentUser;
            DBreference = FirebaseDatabase.DefaultInstance.RootReference;

            //StartCoroutine(UpdateSongHighscore(DBSH_MOUNTAIN_KING, 100));
            //StartCoroutine(UpdateSongHighscore(DBSH_ORIGINAL_SONG, 50));
            //StartCoroutine(UpdateSongHighscore(DBSH_WELLERMAN, 10));

            StartCoroutine(GrabHighestScoreOfASong(DBSH_MOUNTAIN_KING, (scorer) =>
            {
                Debug.Log("Highest score: " + scorer);
            }));

            StartCoroutine(GrabAllScoresOfASong(DBSH_MOUNTAIN_KING, (scores) =>
            {
                foreach (KeyValuePair<string, int> keyValuePair in scores)
                {
                    Debug.Log("Scoring: " + keyValuePair);
                }
            }));

        }

        public IEnumerator UpdateGems(int gems)
        {
            // update the value of the gem
            // will create here if it doesn't exist
            var DBTask = DBreference.Child(DATABASE_USERS).Child(User.UserId).Child(DATABASE_GEM).SetValueAsync(gems);

            yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

            // error
            if (DBTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
            }
        }

        public IEnumerator GetGems(System.Action<int> callbackOnFinish)
        {
            var DBTask = DBreference.Child(DATABASE_USERS).Child(User.UserId).GetValueAsync();

            yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

            if (DBTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
            }
            else if (DBTask.Result.Value == null)
            {
                callbackOnFinish(0);
            }
            else
            {
                Debug.Log("Snapshot Gems");
                DataSnapshot snapshot = DBTask.Result;

                List<DataSnapshot> dataSnapshots = snapshot.Children.ToList();

                if (snapshot.HasChild(DATABASE_GEM))
                {
                    Debug.Log("Able to access data");
                    int value = int.Parse(snapshot.Child(DATABASE_GEM).Value.ToString());
                    callbackOnFinish(value);
                }
                else
                {
                    Debug.Log("Unable to access data");
                    callbackOnFinish(0);
                }
            }

        }

        public IEnumerator UpdateEnergy(int energy)
        {
            // update the value of the gem
            // will create here if it doesn't exist
            var DBTask = DBreference.Child(DATABASE_USERS).Child(User.UserId).Child(DATABASE_ENERGY).SetValueAsync(energy);

            yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

            // error
            if (DBTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
            }
        }

        public IEnumerator GetEnergy(System.Action<int> callbackOnFinish)
        {
            var DBTask = DBreference.Child(DATABASE_USERS).Child(User.UserId).GetValueAsync();

            yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

            if (DBTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
            }
            else if (DBTask.Result.Value == null)
            {
                callbackOnFinish(0);
            }
            else
            {
                Debug.Log("Snapshot Energy");
                DataSnapshot snapshot = DBTask.Result;

                if (snapshot.HasChild(DATABASE_ENERGY))
                {
                    Debug.Log("Able to access data");
                    int value = int.Parse(snapshot.Child(DATABASE_GEM).Value.ToString());
                    callbackOnFinish(value);
                }
                else
                {
                    Debug.Log("Unable to access data");
                    callbackOnFinish(0);
                }
            }

        }

        public IEnumerator UpdateUsername(string username)
        {
            // update the value of the username
            // will create here if it doesn't exist
            var DBTask = DBreference.Child(DATABASE_USERS).Child(User.UserId).Child(DATABASE_USERNAME).SetValueAsync(username);

            yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

            // error
            if (DBTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
            }
        }

        public IEnumerator UpdateAchievements(string achievementName, bool hasClaimed)
        {
            //update the achievement
            //will create here if it doesn't exist
            var DBTask = DBreference.Child(DATABASE_USERS).Child(User.UserId).Child(DATABASE_ACHIEVEMENT).Child(achievementName).Child(DATABASE_ACHIEVEMENT_CLAIMED).SetValueAsync(hasClaimed);

            yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

            if (DBTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
            }
        }

        public IEnumerator GrabAchievements(System.Action<List<string>> callbackOnFinish)
        {
            List<string> achievements = new List<string>();

            var DBTask = DBreference.Child(DATABASE_USERS).Child(User.UserId).Child(DATABASE_ACHIEVEMENT).GetValueAsync();

            yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

            if (DBTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
                callbackOnFinish(achievements);
            }
            else if (DBTask.Result.Value == null)
            {
                Debug.Log("Achievement no result");
                callbackOnFinish(achievements);
            }
            else
            {
                Debug.Log("Snapshot Achievement");
                DataSnapshot snapshot = DBTask.Result;

                // grab all the children of the achievement
                List<DataSnapshot> snapshots = snapshot.Children.ToList();

                // insert all the children into a list to return
                for (int i = 0; i < snapshot.ChildrenCount; ++i)
                {
                    //string value = snapshots[i].Value.ToString();
                    //string value = snapshots[i].Key;
                    achievements.Add(snapshots[i].Key);
                }


                callbackOnFinish(achievements);

            }

        }

        public IEnumerator HasAchievement(string achievementName, System.Action<bool> callbackOnFinish)
        {
            var DBTask = DBreference.Child(DATABASE_USERS).Child(User.UserId).Child(DATABASE_ACHIEVEMENT).GetValueAsync();

            yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

            if (DBTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
            }
            else if (DBTask.Result.Value == null)
            {
                Debug.Log("Achievement no result");
                callbackOnFinish(false);
            }
            else
            {
                Debug.Log("Snapshot Achievement");
                DataSnapshot snapshot = DBTask.Result;
                callbackOnFinish(snapshot.HasChild(achievementName));
            }

        }

        public IEnumerator HasAchievementBeenClaimed(string achievementName, System.Action<bool> callbackOnFinish)
        {
            var DBTask = DBreference.Child(DATABASE_USERS).Child(User.UserId).Child(DATABASE_ACHIEVEMENT).Child(achievementName).GetValueAsync();

            yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

            if (DBTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
            }
            else if (DBTask.Result.Value == null)
            {
                Debug.Log("Achievement Claim no result");
                callbackOnFinish(false);
            }
            else
            {
                Debug.Log("Snapshot Achievement");
                DataSnapshot snapshot = DBTask.Result;

                callbackOnFinish(bool.Parse(snapshot.Child(DATABASE_ACHIEVEMENT_CLAIMED).Value.ToString()));
            }
        }

        public IEnumerator UpdateCharacters(string characterName, int number)
        {
            // update the characters
            // will create here if it doesn't exist
            var DBTask = DBreference.Child(DATABASE_USERS).Child(User.UserId).Child(DATABASE_CHARACTER).Child(characterName).Child(DATABASE_CHARACTER_NUMBER).SetValueAsync(number);

            yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

            // error
            if (DBTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
            }

        }

        public IEnumerator NumberOfCharacters(string characterName, System.Action<int> callbackOnFinish)
        {

            var DBTask = DBreference.Child(DATABASE_USERS).Child(User.UserId).Child(DATABASE_CHARACTER).GetValueAsync();

            yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

            if (DBTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
            }
            else if (DBTask.Result.Value == null)
            {
                Debug.Log("Characters no result");
                callbackOnFinish(0); // no character exists anyway so the character will be zero
            }
            else
            {
                // check if character name exists
                DBTask = DBreference.Child(DATABASE_USERS).Child(User.UserId).Child(DATABASE_CHARACTER).Child(characterName).GetValueAsync();

                yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

                if (DBTask.Exception != null)
                {
                    Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
                }
                else if (DBTask.Result.Value == null)
                {
                    Debug.Log("Character Name no result");
                    callbackOnFinish(0); // none of the character anyway
                }
                else
                {
                    Debug.Log("Snapshot Number of Characters");
                    DataSnapshot snapshot = DBTask.Result;

                    callbackOnFinish(int.Parse(snapshot.Child(DATABASE_CHARACTER_NUMBER).Value.ToString()));
                }
            }
        }

        public IEnumerator GrabCharacters(System.Action<List<KeyValuePair<string, int>>> callbackOnFinish)
        {
            List<KeyValuePair<string, int>> characters = new List<KeyValuePair<string, int>>();


            var DBTask = DBreference.Child(DATABASE_USERS).Child(User.UserId).Child(DATABASE_CHARACTER).GetValueAsync();

            yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

            if (DBTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
                callbackOnFinish(characters);
            }
            else if (DBTask.Result.Value == null)
            {
                Debug.Log("Character no result");
                callbackOnFinish(characters);
            }
            else
            {
                Debug.Log("Snapshot Grab Characters");
                DataSnapshot snapshot = DBTask.Result;

                // grab all the children of the achievement
                List<DataSnapshot> snapshots = snapshot.Children.ToList();


                // insert all the children into a list to return
                for (int i = 0; i < snapshot.ChildrenCount; ++i)
                {
                    Debug.Log("Snapshot Key: " + snapshots[i].Key);

                    string name = snapshots[i].Key;

                    Debug.Log("Snapshot Child: " + snapshots[i].Child(DATABASE_CHARACTER_NUMBER).Value.ToString());

                    int value = int.Parse(snapshots[i].Child(DATABASE_CHARACTER_NUMBER).Value.ToString());

                    KeyValuePair<string, int> retrievedCharacter = new KeyValuePair<string, int>(name, value);

                    characters.Add(retrievedCharacter);

                }

                callbackOnFinish(characters);

            }

        }

        public IEnumerator UpdateSongHighscore(string songName, int highscore)
        {
            // update the high score
            // will create here if it doesn't exist
            var DBTask = DBreference.Child(DATABASE_USERS).Child(User.UserId).Child(songName).SetValueAsync(highscore);

            yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

            if (DBTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
            }
        }

        public IEnumerator GrabHighestScoreOfASong(string songName, System.Action<KeyValuePair<string, int>> callbackOnFinish)
        {
            // grab all the scores of a song
            var DBTask = DBreference.Child(DATABASE_USERS).OrderByChild(songName).GetValueAsync();

            yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

            if (DBTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
                callbackOnFinish(new KeyValuePair<string, int>("", -1));
            }
            else
            {
                DataSnapshot snapshot = DBTask.Result;

                Debug.Log("Highest score of a song");

                // highest score of a song will be the last one in the snapshot
                // the scores are sorted via ascending order when retrieving
                DataSnapshot topScorer = snapshot.Children.Last();

                Debug.Log("Username: " + topScorer.Child(DATABASE_USERNAME).Value.ToString() + "; Score: " + topScorer.Child(songName).Value.ToString());

                callbackOnFinish(new KeyValuePair<string, int>(topScorer.Child(DATABASE_USERNAME).Value.ToString(), int.Parse(topScorer.Child(songName).Value.ToString())));

            }

        }

        public IEnumerator GrabAllScoresOfASong(string songName, System.Action<List<KeyValuePair<string, int>>> callbackOnFinish)
        {

            // prepare the list
            List<KeyValuePair<string, int>> songScores = new List<KeyValuePair<string, int>>();

            // grab all the scores of a song
            var DBTask = DBreference.Child(DATABASE_USERS).OrderByChild(songName).GetValueAsync();

            yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

            if (DBTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
                callbackOnFinish(null); // nothing to send anyway
            }
            else
            {
                DataSnapshot snapshot = DBTask.Result;

                Debug.Log("Highest score of a song");

                // the scores are sorted via ascending order when retrieving


                // loop through every users UID
                foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse())
                {
                    // check if the song name exists first
                    // because if they haven't played the song the song high score may not exist
                    if (childSnapshot.HasChild(songName))
                    {
                        //Debug.Log("Username: " + childSnapshot.Child(DATABASE_USERNAME).Value.ToString() + "; Score: " + childSnapshot.Child(songName).Value.ToString());

                        songScores.Add(new KeyValuePair<string, int>(childSnapshot.Value.ToString(), int.Parse(snapshot.Child(songName).Value.ToString())));

                    }



                }

            }

        }

    }

}

