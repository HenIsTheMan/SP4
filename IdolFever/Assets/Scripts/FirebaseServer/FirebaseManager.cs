﻿using System.Collections;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using System.Linq;
using TMPro;
using UnityEngine.SceneManagement;

namespace IdolFever.Server
{
    public class FirebaseManager : MonoBehaviour
    {

        #region Fields

        //Firebase variables
        [Header("Firebase")]
        public DependencyStatus dependencyStatus;
        public FirebaseAuth auth;
        public FirebaseUser User;
        public DatabaseReference DBreference;
        //public static bool firebaseReady;
        //public TextMeshProUGUI helpDebug;

        //Login variables
        [Header("Login")]
        public TMP_InputField emailLoginField;
        public TMP_InputField passwordLoginField;
        public TMP_Text warningLoginText;
        public TMP_Text confirmLoginText;

        //Register variables
        [Header("Register")]
        public TMP_InputField usernameRegisterField;
        public TMP_InputField emailRegisterField;
        public TMP_InputField passwordRegisterField;
        public TMP_InputField passwordRegisterVerifyField;
        public TMP_Text warningRegisterText;

        private const string PLAYER_LAST_EMAIL = "last_email";
        private const string PLAYER_LAST_PASSWORD = "last_password";

        [SerializeField] private AsyncSceneTransitionOut asyncSceneTransitionOut = null;

        #endregion

        #region Unity Messages

        void Start()
        {
            Init();
        }

        //private void OnDestroy()
        //{
        //if (auth != null)
        //{
        //    auth.StateChanged -= AuthStateChanged;
        //}
        //}

        #endregion

        private void Init()
        {
            //Check that all of the necessary dependencies for Firebase are present on the system
            Debug.Log("Inside Init");

            //firebaseReady = true;

            //firebaseReady = false;
            auth = null;
            User = null;
            DBreference = null;

            // load from playerprefs the password and the username if any
            emailLoginField.text = PlayerPrefs.GetString(PLAYER_LAST_EMAIL, "");
            passwordLoginField.text = PlayerPrefs.GetString(PLAYER_LAST_PASSWORD, "");

            // CheckAndFixDependenciesAsync() appears to conflict with android builds
            // so we are removing it here
            // this allows android to access firebase correctly
            // however we need to integrate our own check for internet here

            //FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            //{
            //    dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                //If they are avalible Initialize Firebase
                InitializeFirebase();

                //auth.StateChanged += AuthStateChanged;
                //AuthStateChanged(this, null);
                //if (auth.CurrentUser != null)
                //    Debug.Log("User: " + auth.CurrentUser.DisplayName + " is logged in");
                //if (auth.CurrentUser != null)
                //    Debug.Log("User2: " + auth.CurrentUser.DisplayName + " is logged in");
                //Debug.Log("Log out");
                //warningLoginText.text = "Auth signed out";

            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
            //});

        }

        // check if auth changed, useful for debugging
        //void AuthStateChanged(object sender, System.EventArgs eventArgs)
        //{
        //    if (auth.CurrentUser != User)
        //    {
        //        bool signedIn = User != auth.CurrentUser && auth.CurrentUser != null;
        //        if (!signedIn && User != null)
        //        {
        //            Debug.Log("Signed out " + User.DisplayName + " :" + User.UserId);
        //            //helpDebug.text = "Signed out " + User.DisplayName + " :" + User.UserId;
        //        }
        //        User = auth.CurrentUser;
        //        if (signedIn)
        //        {
        //            Debug.Log("Signed in " + User.DisplayName + " :" + User.UserId);
        //            //helpDebug.text = "Signed in " + User.DisplayName + " :" + User.UserId;
        //        }
        //    }
        //}

        private void InitializeFirebase()
        {
            Debug.Log("Setting up Firebase Auth");
            //Set the authentication instance object
            auth = FirebaseAuth.DefaultInstance;

            FirebaseDatabase.DefaultInstance.SetPersistenceEnabled(true);
            DBreference = FirebaseDatabase.DefaultInstance.RootReference;
            DBreference.KeepSynced(true);

            //if (auth.CurrentUser != null)
            //{
            //    helpDebug.text = "Hi Ro";
            //}

            auth.SignOut();

            //if (auth.CurrentUser != null)
            //{
            //    helpDebug.text = "Hi So";
            //}

            //firebaseReady = true;

        }

        //Function for the login button
        public void LoginButton()
        {
            warningLoginText.text = confirmLoginText.text = "";

            //if (firebaseReady)
            //{
            //Call the login coroutine passing the email and password
            StartCoroutine(Login(emailLoginField.text, passwordLoginField.text));
            //Login(emailLoginField.text, passwordLoginField.text);
            //}
            //else
            //{
            Debug.Log("Login: Firebase not ready!");
            //}
        }

        //Function for the register button
        public void RegisterButton()
        {
            //if (firebaseReady)
            //{
            //Call the register coroutine passing the email, password, and username
            StartCoroutine(Register(emailRegisterField.text, passwordRegisterField.text, usernameRegisterField.text));
            //}
            //else
            //{
            //Debug.Log("Register: Firebase not ready!");
            //}
        }

        private IEnumerator Login(string _email, string _password)
        {

            //warningLoginText.text = "Inside Login";

            auth.SignOut();

            //warningLoginText.text = "Auth Signed out";

            //Call the Firebase auth signin function passing the email and password
            var LoginTask = auth.SignInWithEmailAndPasswordAsync(_email, _password);

            //Wait until the task completes
            yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

            //warningLoginText.text = "Login Task Is Done";

            if (LoginTask.Exception != null)
            {
                //If there are errors handle them
                Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
                FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                string message = "Login Failed: ";
                switch (errorCode)
                {
                    default:
                        message = "Are you connected to the internet?";
                        break;
                    case AuthError.MissingEmail:
                        message = "Missing Email";
                        break;
                    case AuthError.MissingPassword:
                        message = "Missing Password";
                        break;
                    case AuthError.WrongPassword:
                        message = "Wrong Password";
                        break;
                    case AuthError.InvalidEmail:
                        message = "Invalid Email";
                        break;
                    case AuthError.UserNotFound:
                        message = "Account does not exist";
                        break;
                }
                confirmLoginText.text = "";
                warningLoginText.text = message;
            }
            else
            {
                //User is now logged in
                //Now get the result
                User = LoginTask.Result;
                Debug.LogFormat("User signed in successfully: {0} ({1})", User.DisplayName, User.Email);
                warningLoginText.text = "";
                confirmLoginText.text = "Logged In";

                // save to player prefs the password and username
                PlayerPrefs.SetString(PLAYER_LAST_EMAIL, _email);
                PlayerPrefs.SetString(PLAYER_LAST_PASSWORD, _password);
                PlayerPrefs.Save(); // just to save it

                asyncSceneTransitionOut.ChangeScene();
            }
        }

        private IEnumerator Register(string _email, string _password, string _username)
        {
            if (_username == "")
            {
                //If the username field is blank show a warning
                warningRegisterText.text = "Missing Username";
            }
            else if (passwordRegisterField.text != passwordRegisterVerifyField.text)
            {
                //If the password does not match show a warning
                warningRegisterText.text = "Password Does Not Match!";
            }
            else
            {
                //Call the Firebase auth signin function passing the email and password
                var RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
                //Wait until the task completes
                yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

                if (RegisterTask.Exception != null)
                {
                    //If there are errors handle them
                    Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
                    FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                    AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                    string message = "Register Failed!";
                    switch (errorCode)
                    {
                        default:
                            message = "Are you connected to the internet?";
                            break;
                        case AuthError.MissingEmail:
                            message = "Missing Email";
                            break;
                        case AuthError.MissingPassword:
                            message = "Missing Password";
                            break;
                        case AuthError.WeakPassword:
                            message = "Weak Password";
                            break;
                        case AuthError.EmailAlreadyInUse:
                            message = "Email Already In Use";
                            break;
                        case AuthError.InvalidEmail:
                            message = "Email Invalid";
                            break;
                    }
                    warningRegisterText.text = message;
                }
                else
                {
                    //User has now been created
                    //Now get the result
                    User = RegisterTask.Result;

                    if (User != null)
                    {
                        //Create a user profile and set the username
                        UserProfile profile = new UserProfile { DisplayName = _username };

                        //Call the Firebase auth update user profile function passing the profile with the username
                        var ProfileTask = User.UpdateUserProfileAsync(profile);
                        //Wait until the task completes
                        yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                        if (ProfileTask.Exception != null)
                        {
                            //If there are errors handle them
                            Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
                            FirebaseException firebaseEx = ProfileTask.Exception.GetBaseException() as FirebaseException;
                            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                            warningRegisterText.text = "Username Set Failed!";
                        }
                        else
                        {
                            //Username is now set
                            // sync it with the database

                            var DBTask = DBreference.Child("users").Child(User.UserId).Child("USERNAME").SetValueAsync(_username);

                            yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

                            // error
                            if (DBTask.Exception != null)
                            {
                                Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
                            }


                            //Now return to login screen
                            UIManager.instance.LoginScreen();
                            warningRegisterText.text = "";
                        }
                    }
                }
            }
        }

    }

}