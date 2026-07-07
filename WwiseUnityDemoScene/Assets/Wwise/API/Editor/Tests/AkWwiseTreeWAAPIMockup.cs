using System.Collections.Generic;
using Wwise.API.Editor.Tests;

public class AkWwiseTreeWAAPIMockup : AkWwiseTreeWAAPIDataSource
{
    
    public override void FetchData()
    {
        Data.Clear();
        CreateProjectRootItem();
        TreeUtility.TreeToList(ProjectRoot, ref Data);
    }

    protected virtual List<AkWwiseProjectData.WwiseTreeObject> GenerateObjectList(WwiseObjectType type)
    {
        uint typeModifier = (uint)type;
        List<AkWwiseProjectData.WwiseTreeObject> output = new List<AkWwiseProjectData.WwiseTreeObject>();
        output.Add(WwiseBrowserTestsUtils.GenerateObject("UpToDate", WwiseProjectDatabase.GenerateGuidFromInts(1, 2, 3, typeModifier), "UpToDate", type));
        output.Add(WwiseBrowserTestsUtils.GenerateObject("Renamed2", WwiseProjectDatabase.GenerateGuidFromInts(2, 3, 4, typeModifier), "Renamed2", type));
        output.Add(WwiseBrowserTestsUtils.GenerateObject("Folder\\Moved", WwiseProjectDatabase.GenerateGuidFromInts(3, 4, 5, typeModifier), "Moved", type));
        output.Add(WwiseBrowserTestsUtils.GenerateObject("NeedsUpdate", WwiseProjectDatabase.GenerateGuidFromInts(6, 7, 8, typeModifier), "NeedsUpdate", type));
        output.Add(WwiseBrowserTestsUtils.GenerateObject("New", WwiseProjectDatabase.GenerateGuidFromInts(7, 8, 9, typeModifier), "New", type));
        
        return output;
    }

    public override AkWwiseTreeViewItem CreateProjectRootItem()
    {
        ProjectRoot = new AkWwiseTreeViewItem();
        ProjectRoot.AddWwiseItemChild(BuildObjectTypeTree(WwiseObjectType.Event));
        ProjectRoot.AddWwiseItemChild(BuildObjectTypeTree(WwiseObjectType.Switch));
        ProjectRoot.AddWwiseItemChild(BuildObjectTypeTree(WwiseObjectType.State));
        ProjectRoot.AddWwiseItemChild(BuildObjectTypeTree(WwiseObjectType.Soundbank));
        ProjectRoot.AddWwiseItemChild(BuildObjectTypeTree(WwiseObjectType.AuxBus));
        ProjectRoot.AddWwiseItemChild(BuildObjectTypeTree(WwiseObjectType.GameParameter));
        ProjectRoot.AddWwiseItemChild(BuildObjectTypeTree(WwiseObjectType.Trigger));
        ProjectRoot.AddWwiseItemChild(BuildObjectTypeTree(WwiseObjectType.AcousticTexture));
        return ProjectRoot;
    }
    
    AkWwiseTreeViewItem BuildTree(AkWwiseTreeViewItem treeViewItem, List<AkWwiseProjectData.WwiseTreeObject> childrenInfo, int depth)
    {
        if (childrenInfo == null)
        {
            return treeViewItem;
        }
        foreach (var children in childrenInfo)
        {
            var childItem = new AkWwiseTreeViewItem(children.Name, depth, GenerateUniqueID(), children.Guid, children.Type);
            childItem.path = children.Path;
            childItem = BuildTree(childItem, children.Children, depth++);
            childItem.waapiName = children.Name;
            childItem.waapiPath = children.Path;
            if (childItem != null)
            {
                treeViewItem.AddWwiseItemChild(childItem);	
            }
        }

        return treeViewItem;
    }

    public AkWwiseTreeViewItem BuildTree(string name,
        List<AkWwiseProjectData.WwiseTreeObject> wwiseTreeObjects)
    {
        var rootFolder = new AkWwiseTreeViewItem(name, 1, GenerateUniqueID(), new System.Guid(), WwiseObjectType.PhysicalFolder);
        rootFolder.waapiName = name;
        rootFolder.waapiPath = name;
        return BuildTree(rootFolder, wwiseTreeObjects, 1);
    }

    
    protected AkWwiseTreeViewItem BuildObjectTypeTree(WwiseObjectType objectType)
    {
        var rootElement = new AkWwiseTreeViewItem();
        rootElement = BuildTree(AkWwiseTreeDataSource.FolderNames[objectType], GenerateObjectList(objectType));
        wwiseObjectFolders[objectType] = rootElement;
        return rootElement;
    }
}
