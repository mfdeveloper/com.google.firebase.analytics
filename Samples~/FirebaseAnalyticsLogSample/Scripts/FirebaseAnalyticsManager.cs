using System;
using System.Threading.Tasks;
using UnityEngine;
using Firebase;
using Firebase.Extensions;
using Firebase.Analytics;

namespace Firebase.Analytics.Samples
{
    /// <summary>
    /// A manager class that initialize <see cref="FirebaseApp"/> and
    /// track Analytics events
    /// </summary>
    /// <remarks>
    /// TODO: <b>[Refactor]</b> Move this manager to a "subpackage" as a monorepo that could be reused
    /// by others Unity projects.
    /// </remarks>
    public class FirebaseAnalyticsManager : MonoBehaviour
    {
        protected FirebaseApp app;

        async void Start()
        {
            await InitializeFirebase();
        }

        private async Task InitializeFirebase(Action<Task> onInit = null) 
        {
            await FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(previousTask => 
                {
                    var dependencyStatus = previousTask.Result;
                    if (dependencyStatus == DependencyStatus.Available) {
                        // Create and hold a reference to your FirebaseApp,
                        app = FirebaseApp.DefaultInstance;
                    
                        onInit?.Invoke(previousTask);
                    } else {
                        Debug.LogError(
                        $"Could not resolve all Firebase dependencies: \"{dependencyStatus}\"\n" +
                        "Firebase Unity SDK is not safe to use here");
                    }
                }
            );
        }

        public virtual void TrackEvent(string name) 
        {
            FirebaseAnalytics.LogEvent(
                name: name, 
                parameters: new Parameter[]
                { 
                    new(parameterName: "parameter1", parameterValue: "test"),
                    new(parameterName: "parameter2", parameterValue: "anotherTest")
                }
            );
        }
    }
}
