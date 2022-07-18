using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using UnityEngine.SceneManagement;

public class LoginRegister : MonoBehaviour
{
    [SerializeField]
    private Text email, password, error;

    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser User;

    void Awake()
    {
        email = GameObject.FindGameObjectWithTag("user").GetComponent<Text>();
        password = GameObject.FindGameObjectWithTag("pass").GetComponent<Text>();
        error = GameObject.FindGameObjectWithTag("fail").GetComponent<Text>();
        //Check that all of the necessary dependencies for Firebase are present on the system
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                //If they are avalible Initialize Firebase
                InitializeFirebase();
            }
            else
            {
                error.text = "Could not resolve all Firebase dependencies: " + dependencyStatus;
            }
        });
    }

    private void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        //Set the authentication instance object
        auth = FirebaseAuth.DefaultInstance;
    }

    public void LoginButton()
    {
        //Call the login coroutine passing the email and password
        AudioManager.emailLogat = email.text;
        StartCoroutine(Login(email.text, password.text));
    }

    public void RegisterButton()
    {
        //Call the register coroutine passing the email, password, and username
        AudioManager.emailLogat = email.text;
        StartCoroutine(Register(email.text, password.text));
    }

    private IEnumerator Login(string _email, string _password)
    {
        //Call the Firebase auth signin function passing the email and password
        var LoginTask = auth.SignInWithEmailAndPasswordAsync(_email, _password);
        //Wait until the task completes
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        if (LoginTask.Exception != null)
        {
            //If there are errors handle them
            Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "Login Failed!";
            switch (errorCode)
            {
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
            error.text = message;
        }
        else
        {
            //User is now logged in
            //Now get the result
            User = LoginTask.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})", User.DisplayName, User.Email);
            AudioManager.LoggedIn = true;
            SceneManager.LoadScene(0);
        }
    }

    private IEnumerator Register(string _email, string _password)
    {
        if (_email == "")
        {
            //If the username field is blank show a warning
            Debug.Log("Missing Email");
        }
        else if (password.text == "")
        {
            //If the password does not match show a warning
            Debug.Log("Missing Password");
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
                }
                error.text = message;
            }
            else
            {
                //User has now been created
                //Now get the result
                User = RegisterTask.Result;

                if (User != null)
                {
                    //Create a user profile and set the username
                    UserProfile profile = new UserProfile { DisplayName = _email };

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
                        Debug.Log("Username Set Failed!");
                    }
                    else
                    {
                        //Username is now set
                        //Now return to login screen
                        AudioManager.LoggedIn = true;
                        SceneManager.LoadScene(0);
                    }
                }
            }
        }
    }



    //public void Login()
    //{

    //    email = GameObject.FindGameObjectWithTag("user").GetComponent<Text>();
    //    password = GameObject.FindGameObjectWithTag("pass").GetComponent<Text>();

    //    if (email.text.Equals("") && password.text.Equals(""))
    //    {
    //        Debug.Log("Enter an email and password!");
    //        return;
    //    }
    //    FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync(email.text, password.text).ContinueWith((task => {
    //        if (task.IsCanceled)
    //        {
    //            Debug.Log("Canceled");
    //            return;
    //        }
    //        if (task.IsFaulted)
    //        {
    //            Debug.Log("Faulted");
    //            return;
    //        }
    //        if (task.IsCompleted)
    //        {
    //            Debug.Log("Conected!");
    //            return;
    //        }
    //    }));
    //    AudioManager.LoggedIn = true;
    //    SceneManager.LoadScene(0);
    //}
    //public void Register()
    //{

    //    email = GameObject.FindGameObjectWithTag("user").GetComponent<Text>();
    //    password = GameObject.FindGameObjectWithTag("pass").GetComponent<Text>();
    //    if (email.text.Equals("") && password.text.Equals(""))
    //    {
    //        Debug.Log("Enter an email and password!");
    //        return;
    //    }
    //    FirebaseAuth.DefaultInstance.CreateUserWithEmailAndPasswordAsync(email.text, password.text).ContinueWith((task => {
    //        if (task.IsCanceled)
    //        {
    //            Debug.Log("Canceled");
    //            return;
    //        }
    //        if (task.IsFaulted)
    //        {
    //            Debug.Log("Faulted");
    //            return;
    //        }
    //        if (task.IsCompleted)
    //        {
    //            Debug.Log("Conected!");
    //            return;
    //        }
    //    }));
    //    AudioManager.LoggedIn = true;
    //    SceneManager.LoadScene(0);
    //}

}
