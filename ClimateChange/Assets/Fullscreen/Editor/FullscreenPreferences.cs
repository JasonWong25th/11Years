using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace FullscreenEditor {

    /// <summary>Define a source mode to get a fullscreen rect.</summary>
    public enum RectSourceMode {
        /// <summary>The bounds of the main display.</summary>
        MainDisplay,
        /// <summary>The bounds of the display where the mouse pointer is.</summary>
        AtMousePosition,
        /// <summary>A rect that spans across all the displays. (Windows only)</summary>
        VirtualSpace,
        /// <summary>A custom rect defined by <see cref="FullscreenPreferences.CustomRect"/>.</summary>
        Custom
    }

    /// <summary>Contains preferences for the Fullscreen Editor plugin.</summary>
    [InitializeOnLoad]
    public static class FullscreenPreferences {

        private const string DEVELOPER_EMAIL = "samuelschultze@gmail.com";
        private const string ASSET_STORE_PAGE = "https://assetstore.unity.com/packages/tools/utilities/fullscreen-editor-69534";
        private const string FORUM_THREAD = "https://forum.unity.com/threads/released-fullscreen-editor.661519/";

        /// <summary>Current version of the Fullscreen Editor plugin.</summary> 
        public static readonly Version pluginVersion = new Version(2, 1, 0);
        /// <summary>Release date of this version.</summary> 
        public static readonly DateTime pluginDate = new DateTime(2019, 07, 16);

        private static readonly GUIContent resetSettingsContent = new GUIContent("Use Defaults", "Reset all settings to default ones");
        private static readonly GUIContent versionContent = new GUIContent(string.Format("Version: {0} ({1:d})", pluginVersion, pluginDate));

        private static readonly GUIContent[] links = new GUIContent[] {
            new GUIContent("Store Page", ASSET_STORE_PAGE),
            new GUIContent("Forum Thread", FORUM_THREAD),
            new GUIContent("Email Contact", GetEmailURL()),
            new GUIContent("Changelog", GetFilePath("Changelog.pdf")),
            new GUIContent("Readme", GetFilePath("Readme.pdf")),
        };

        internal static Action onLoadDefaults = () => { };
        internal static readonly List<GUIContent> contents = new List<GUIContent>();

        private static readonly PrefItem<Vector2> scroll = new PrefItem<Vector2>("Scroll", Vector2.zero, string.Empty, string.Empty);

        /// <summary>Is the window toolbar currently visible?</summary>
        public static readonly PrefItem<bool> ToolbarVisible;

        /// <summary>Is Fullscreen on Play currently enabled?</summary>
        public static readonly PrefItem<bool> FullscreenOnPlayEnabled;

        /// <summary>Defines a source to get a fullscreen rect.</summary>
        public static readonly PrefItem<RectSourceMode> RectSource;

        /// <summary>Custom rect to be used when <see cref="RectSource"/> is set to <see cref="RectSourceMode.Custom"/>.</summary>
        public static readonly PrefItem<Rect> CustomRect;

        /// <summary>Disable notifications when opening fullscreen windows.</summary>
        public static readonly PrefItem<bool> DisableNotifications;

        static FullscreenPreferences() {
            var rectSourceTooltip = string.Empty;

            rectSourceTooltip += "Controls where Fullscreen Views opens.\n\n";
            rectSourceTooltip += "Primary Screen: Fullscreen opens on the main screen;\n\n";
            rectSourceTooltip += "At Mouse Position: Fullscreen opens on the screen where the mouse pointer is;\n\n";
            rectSourceTooltip += "Virtual Space: Fullscreen spans across all screens (Windows only);\n\n";
            rectSourceTooltip += "Custom Rect: Fullscreen opens on the given custom Rect.";

            ToolbarVisible = new PrefItem<bool>("Toolbar", false, "Toolbar Visible", "Show and hide the toolbar on the top of some windows, like the Game View and Scene View.");
            FullscreenOnPlayEnabled = new PrefItem<bool>("FullscreenOnPlay", false, "Fullscreen On Play", "Override the \"Maximize on Play\" option of the game view to \"Fullscreen on Play\"");
            RectSource = new PrefItem<RectSourceMode>("RectSource", RectSourceMode.AtMousePosition, "Rect Source", rectSourceTooltip);
            CustomRect = new PrefItem<Rect>("CustomRect", FullscreenRects.GetMainDisplayRect(), "Custom Rect", string.Empty);
            DisableNotifications = new PrefItem<bool>("DisableNotifications", false, "Disable Notifications", "Disable the notifications that shows up when opening a new fullscreen view.");

            if (FullscreenUtility.MenuItemHasShortcut(Shortcut.TOOLBAR_PATH))
                ToolbarVisible.Content.text += string.Format(" ({0})", FullscreenUtility.TextifyMenuItemShortcut(Shortcut.TOOLBAR_PATH));
            if (FullscreenUtility.MenuItemHasShortcut(Shortcut.FULLSCREEN_ON_PLAY_PATH))
                FullscreenOnPlayEnabled.Content.text += string.Format(" ({0})", FullscreenUtility.TextifyMenuItemShortcut(Shortcut.FULLSCREEN_ON_PLAY_PATH));
        }

        #if UNITY_2018_3_OR_NEWER
        [SettingsProvider]
        private static SettingsProvider RetrieveSettingsProvider() {
            var settingsProvider = new SettingsProvider("Preferences/Fullscreen Editor", SettingsScope.User, contents.Select(c => c.text));
            settingsProvider.guiHandler = new Action<string>((str) => OnPreferencesGUI(str));
            return settingsProvider;
        }

        #else
        [PreferenceItem("Fullscreen")]
        private static void OnPreferencesGUI() {
            OnPreferencesGUI(string.Empty);
        }
        #endif

        private static void OnPreferencesGUI(string search) {

            if (FullscreenUtility.IsLinux)
                EditorGUILayout.HelpBox("This plugin was not tested on Linux and its behaviour is unknown.", MessageType.Warning);

            EditorGUILayout.Separator();

            #if !UNITY_2018_3_OR_NEWER
            scroll.Value = EditorGUILayout.BeginScrollView(scroll);
            #endif

            EditorGUILayout.Separator();
            ToolbarVisible.DoGUI();
            FullscreenOnPlayEnabled.DoGUI();

            EditorGUILayout.Separator();
            RectSource.DoGUI();
            DisableNotifications.DoGUI();

            if (!IsRectModeSupported(RectSource))
                EditorGUILayout.HelpBox("The selected Rect Source mode is not supported on this platform", MessageType.Warning);

            switch (RectSource.Value) {
                case RectSourceMode.Custom:
                    EditorGUI.indentLevel++;
                    CustomRect.DoGUI();

                    var customRect = CustomRect.Value;

                    if (customRect.width < 300f)
                        customRect.width = 300f;
                    if (customRect.height < 300f)
                        customRect.height = 300f;

                    CustomRect.Value = customRect;

                    EditorGUI.indentLevel--;
                    break;
            }

            EditorGUILayout.Separator();
            Shortcut.DoShortcutsGUI();

            #if !UNITY_2018_3_OR_NEWER
            EditorGUILayout.EndScrollView();
            #endif

            GUILayout.FlexibleSpace();

            Func<GUIContent, bool> linkLabel = (label) => typeof(EditorGUILayout).InvokeMethod<bool>("LinkLabel", label, new GUILayoutOption[0]);

            using(new EditorGUILayout.HorizontalScope()) {
                GUILayout.FlexibleSpace();
                if (linkLabel(new GUIContent("Consider leaving a review if you're enjoying Fullscreen Editor!", ASSET_STORE_PAGE)))
                    Application.OpenURL(ASSET_STORE_PAGE);
                GUILayout.FlexibleSpace();
            }

            using(new EditorGUILayout.HorizontalScope()) {
                GUILayout.FlexibleSpace();
                for (var i = 0; i < links.Length; i++) {
                    if (linkLabel(links[i]))
                        Application.OpenURL(links[i].tooltip);
                    GUILayout.Space(5f);
                }
                GUILayout.FlexibleSpace();
            }

            EditorGUILayout.Separator();

            using(new EditorGUILayout.HorizontalScope()) {
                if (GUILayout.Button(resetSettingsContent, GUILayout.Width(120f)))
                    onLoadDefaults();
                GUILayout.FlexibleSpace();
                EditorGUILayout.LabelField(versionContent, GUILayout.Width(170f));
            }

            EditorGUILayout.Separator();
        }

        private static string GetFilePath(string file) {
            var stack = new StackFrame(0, true);
            var currentFile = stack.GetFileName();
            var currentPath = Path.GetDirectoryName(currentFile);

            return Path.Combine(currentPath, "../" + file);
        }

        private static string GetEmailURL(Exception e = null) {
            var full = new StringBuilder();
            var body = new StringBuilder();

            #if UNITY_2018_1_OR_NEWER
            Func<string, string> EscapeURL = url => UnityEngine.Networking.UnityWebRequest.EscapeURL(url).Replace("+", "%20");
            #else
            Func<string, string> EscapeURL = url => WWW.EscapeURL(url).Replace("+", "%20");
            #endif

            body.Append("\nDescribe your issue or make your request here");
            body.Append("\n\nAdditional Information:");
            body.AppendFormat("\nVersion: {0}", pluginVersion.ToString(3));
            body.AppendFormat("\nUnity {0}", InternalEditorUtility.GetFullUnityVersion());
            body.AppendFormat("\n{0}", SystemInfo.operatingSystem);

            if (e != null)
                body.AppendFormat("\n\nEXCEPTION\n", e);

            full.Append("mailto:");
            full.Append(DEVELOPER_EMAIL);
            full.Append("?subject=");
            full.Append(EscapeURL("Fullscreen Editor - Support"));
            full.Append("&body=");
            full.Append(EscapeURL(body.ToString()));

            return full.ToString();
        }

        internal static bool IsRectModeSupported(RectSourceMode mode) {
            switch (mode) {
                case RectSourceMode.VirtualSpace:
                    return FullscreenUtility.IsWindows;

                case RectSourceMode.Custom:
                case RectSourceMode.MainDisplay:
                case RectSourceMode.AtMousePosition:
                    return true;

                default:
                    return false;
            }
        }

    }

}