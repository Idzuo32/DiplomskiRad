using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System.IO;

[InitializeOnLoad]
public class SelectFoldersByLetters
{
    static float _lastKeyPressTime;
    static string _lastPressedKey = "";
    static int _currentIndex = -1;
    const float KeyPressTimeout = 1.0f; // Reset after 1 second
    private static bool _isRenaming = false;

    static SelectFoldersByLetters()
    {
        SceneView.duringSceneGui += OnSceneGUI;
        EditorApplication.projectWindowItemOnGUI += ProjectWindowItemCallback;

        // Subscribe to events that might indicate rename operations
        EditorApplication.hierarchyChanged += OnHierarchyChanged;
        Selection.selectionChanged += OnSelectionChanged;
    }

    static void OnHierarchyChanged()
    {
        // Reset rename state when hierarchy changes
        _isRenaming = false;
    }

    static void OnSelectionChanged()
    {
        // Check if we're in a rename operation
        if (EditorWindow.focusedWindow != null)
        {
            var currentEvent = Event.current;
            if (currentEvent != null &&
                (currentEvent.commandName == "ObjectSelectorUpdated" ||
                 currentEvent.commandName == "Rename" ||
                 EditorGUIUtility.editingTextField))
            {
                _isRenaming = true;
            }
            else
            {
                _isRenaming = false;
            }
        }
    }

    static void ProjectWindowItemCallback(string guid, Rect selectionRect)
    {
        if (Event.current != null && Event.current.type == EventType.Layout)
        {
            EditorWindow projectWindow = EditorWindow.focusedWindow;
            if (projectWindow != null && projectWindow.GetType().Name == "ProjectBrowser")
            {
                projectWindow.wantsMouseMove = true;
            }
        }

        // Check if any text field is being edited
        if (!EditorGUIUtility.editingTextField)
        {
            HandleKeyboardInput();
        }
    }

    static void OnSceneGUI(SceneView sceneView)
    {
        if (!EditorGUIUtility.editingTextField && !_isRenaming)
        {
            HandleKeyboardInput();
        }
    }

    static string GetCurrentProjectWindowPath()
    {
        // Get the Project window
        var projectWindow = EditorWindow.focusedWindow;
        if (projectWindow == null || projectWindow.GetType().Name != "ProjectBrowser")
            return "Assets";

        // Try to get the current folder view path from the selected object
        var selected = Selection.activeObject;
        if (selected != null)
        {
            string path = AssetDatabase.GetAssetPath(selected);
            if (!string.IsNullOrEmpty(path))
            {
                if (Directory.Exists(path))
                {
                    return path;
                }

                // Get the parent folder of the selected item
                return Path.GetDirectoryName(path)?.Replace('\\', '/') ?? "Assets";
            }
        }

        return "Assets";
    }

    // ... rest of your existing code ...

    static void HandleKeyboardInput()
    {
        if (!EditorWindow.focusedWindow ||
            EditorWindow.focusedWindow.GetType().Name != "ProjectBrowser" ||
            EditorGUIUtility.editingTextField ||
            _isRenaming)
            return;

        Event currentEvent = Event.current;
        if (currentEvent == null || currentEvent.type != EventType.KeyDown)
            return;

        // Check for rename shortcut (F2 or Return)
        if (currentEvent.keyCode == KeyCode.F2 || currentEvent.keyCode == KeyCode.Return)
        {
            _isRenaming = true;
            return;
        }

        // Rest of your existing HandleKeyboardInput code...
        if (currentEvent.keyCode == KeyCode.None || !char.IsLetter((char)currentEvent.keyCode))
            return;

        string currentKey = ((char)currentEvent.keyCode).ToString().ToLower();
        float currentTime = Time.realtimeSinceStartup;

        // Get all items in the current folder
        string currentFolder = GetCurrentProjectWindowPath();
        var directoryInfo = new DirectoryInfo(currentFolder);

        // Get all files and directories in the current folder
        var allItems = directoryInfo.GetFileSystemInfos()
            .Where(item => !item.Name.EndsWith(".meta"))
            .Select(item => new
            {
                Name = item.Name,
                FullPath = item.FullName.Replace('\\', '/'),
                IsDirectory = (item.Attributes & FileAttributes.Directory) == FileAttributes.Directory
            })
            .Where(item => item.Name.ToLower().StartsWith(currentKey))
            .OrderBy(item => item.Name)
            .ToList();

        if (allItems.Count == 0)
            return;

        // Reset index if it's a different key or too much time has passed
        if (currentKey != _lastPressedKey || (currentTime - _lastKeyPressTime) > KeyPressTimeout)
        {
            _currentIndex = 0;
        }
        else
        {
            // Move to next item
            _currentIndex = (_currentIndex + 1) % allItems.Count;
        }

        // Get the selected item
        var selectedItem = allItems[_currentIndex];

        // Convert the full system path to a Unity asset path
        string unityPath = selectedItem.FullPath.Substring(Application.dataPath.Length - "Assets".Length)
            .Replace('\\', '/');

        // Select the item without opening it
        Object selectedObject = AssetDatabase.LoadAssetAtPath<Object>(unityPath);
        if (selectedObject != null)
        {
            Selection.activeObject = selectedObject;
            // Just highlight the item without opening it
            EditorGUIUtility.PingObject(selectedObject);
        }

        // Update tracking variables
        _lastPressedKey = currentKey;
        _lastKeyPressTime = currentTime;

        currentEvent.Use();
        EditorWindow.focusedWindow.Repaint();
    }
}