using System.Collections.Generic;
using Wwise.API.Editor.Tests;

public class AkWwiseTreeProjectDatabaseMockup : AkWwiseTreeProjectDataSource
{
    
    public override void FetchData()
    {
        Data.Clear();
        InitializeMinimal();
    }
    protected virtual List<AkWwiseProjectData.WwiseTreeObject> GenerateObjectList(WwiseObjectType type)
    {
        uint typeModifier = (uint)type;
        List<AkWwiseProjectData.WwiseTreeObject> output = new List<AkWwiseProjectData.WwiseTreeObject>();
        output.Add(WwiseBrowserTestsUtils.GenerateObject("UpToDate", WwiseProjectDatabase.GenerateGuidFromInts(1, 2, 3, typeModifier), "UpToDate", type));
        output.Add(WwiseBrowserTestsUtils.GenerateObject("Renamed", WwiseProjectDatabase.GenerateGuidFromInts(2, 3, 4, typeModifier), "Renamed", type));
        output.Add(WwiseBrowserTestsUtils.GenerateObject("Moved", WwiseProjectDatabase.GenerateGuidFromInts(3, 4, 5, typeModifier), "Moved", type));
        output.Add(WwiseBrowserTestsUtils.GenerateObject("NeedsUpdate", WwiseProjectDatabase.GenerateGuidFromInts(4, 5, 6, typeModifier), "NeedsUpdate", type));
        output.Add(WwiseBrowserTestsUtils.GenerateObject("Deleted", WwiseProjectDatabase.GenerateGuidFromInts(5, 6, 7, typeModifier), "Deleted", type));
        
        return output;
    }
    
    protected override AkWwiseTreeViewItem BuildObjectTypeTree(WwiseObjectType objectType)
    {
        var rootElement = new AkWwiseTreeViewItem();
        rootElement = BuildTree(AkWwiseTreeDataSource.FolderNames[objectType], GenerateObjectList(objectType));
        wwiseObjectFolders[objectType] = rootElement;
        return rootElement;
    }
}
