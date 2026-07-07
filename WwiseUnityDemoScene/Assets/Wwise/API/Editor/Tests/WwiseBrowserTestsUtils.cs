namespace Wwise.API.Editor.Tests
{
	public class WwiseBrowserTestsUtils
	{
		public static AkWwiseProjectData.WwiseTreeObject GenerateObject(string path, System.Guid guid, string name, WwiseObjectType type)
		{
			AkWwiseProjectData.WwiseTreeObject Object = new AkWwiseProjectData.WwiseTreeObject();
			Object.Path = AkWwiseTreeDataSource.FolderNames[type] + "\\" + path;
			Object.Guid = guid;
			Object.Name = name;
			Object.Type = type;
			return Object;
		}
	}
}