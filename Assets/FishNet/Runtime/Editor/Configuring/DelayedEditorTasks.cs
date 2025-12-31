#if UNITY_EDITOR
using System;
using FishNet.Configuring;
using FishNet.Configuring.EditorCloning;
using FishNet.Managing;
using UnityEditor;
using UnityEngine;

namespace FishNet.Editing
{
    /// <summary>
    /// Contributed by YarnCat! Thank you!
    /// </summary>
    [InitializeOnLoad]
    public class DelayedEditorTasks : EditorWindow
    {
        private static double _startTime = double.MinValue;

        static DelayedEditorTasks()
        {
            if (CloneChecker.IsMultiplayerClone(out _))
                return;

            const string startupCheckString = "FishNetDelayedEditorTasks";
            if (SessionState.GetBool(startupCheckString, false))
                return;

            _startTime = EditorApplication.timeSinceStartup;
            EditorApplication.update += CheckRunTasks;

            SessionState.SetBool(startupCheckString, true);
        }

        private static void CheckRunTasks()
        {
            if (EditorApplication.timeSinceStartup - _startTime < 1f)
                return;

            EditorApplication.update -= CheckRunTasks;

            // First time use, no other actions should be done.
            if (FishNetGettingStartedEditor.ShowGettingStarted())
                return;

            ReviewReminderEditor.CheckRemindToReview();
        }
    }
}
#endif