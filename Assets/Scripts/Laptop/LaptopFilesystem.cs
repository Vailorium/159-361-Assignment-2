using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class File
{
    public string content;

    public File(string content)
    {
        this.content = content;
    }
}

[System.Serializable]
class Folder
{
    public Dictionary<string, Folder> subFolders;
    public Dictionary<string, File> files;

    public Folder(Dictionary<string, Folder> subFolders, Dictionary<string, File> files)
    {
        this.subFolders = subFolders;
        this.files = files;
    }

    /// <summary>
    /// Get list of all folder names and filenames in this folder
    /// </summary>
    /// <returns>List of all folder and filenames</returns>
    public List<string> getItems()
    {
        List<string> items = new List<string>();
        foreach (KeyValuePair<string, Folder> item in subFolders)
        {
            items.Add(item.Key);
        }
        foreach(KeyValuePair<string, File> item in files)
        {
            items.Add(item.Key);
        }
        return items;
    }
}
class FileSystem
{
    public List<string> currentPath;
    public Folder root;
    public FileSystem(Folder rootFolder)
    {
        currentPath = new List<string>() { "C:" };
        this.root = rootFolder;
    }

    /// <summary>
    /// Gets current folder
    /// </summary>
    /// <returns>Folder user is currently in</returns>
    public Folder getFromPath()
    {
        Folder currentFolder = root;
        foreach(string path in currentPath)
        {
            if(currentFolder.subFolders.ContainsKey(path))
            {
                currentFolder = currentFolder.subFolders[path];
            }
        }
        return currentFolder;
    }

    /// <summary>
    /// Get folder from provided path
    /// </summary>
    /// <param name="pathList">List of strings giving path relative to root</param>
    /// <returns>Folder from provided path</returns>
    /// <exception cref="KeyNotFoundException">If path doesn't exist</exception>
    public Folder getFromPath(List<string> pathList)
    {
        Folder currentFolder = root;
        foreach (string path in pathList)
        {
            if (currentFolder.subFolders.ContainsKey(path))
            {
                currentFolder = currentFolder.subFolders[path];
            } else
            {
                throw new KeyNotFoundException();
            }
        }
        return currentFolder;
    }

    /// <summary>
    /// Attempts to navigate to provided path
    /// </summary>
    /// <param name="path">Folder name</param>
    /// <returns>True is successful, false if unsuccessful</returns>
    public bool navigateTo(string path)
    {
        string[] pathItems = path.Split(new string[] { "/" }, System.StringSplitOptions.RemoveEmptyEntries);

        List<string> clonePath = new List<string>(currentPath);
        for (int i = 0; i < pathItems.Length; i++)
        {
            string item = pathItems[i];
            if (item == "..")
            {
                if(clonePath.Count == 0)
                {
                    return false;
                }
                clonePath.RemoveRange(clonePath.Count - 1, 1);
                continue;
            }

            clonePath.Add(item);
        }

        try
        {
            Folder res = this.getFromPath(clonePath);
        } catch(KeyNotFoundException)
        {
            return false;
        }
        currentPath = clonePath;
        return true;
    }

    /// <summary>
    /// Get path represented in a string (e.g. C:\files)
    /// </summary>
    /// <returns>Path string</returns>
    public string getPathString()
    {
        string loc = "";
        foreach (string directory in this.currentPath)
        {
            loc += directory + "\\";
        }
        return loc;
    }
}