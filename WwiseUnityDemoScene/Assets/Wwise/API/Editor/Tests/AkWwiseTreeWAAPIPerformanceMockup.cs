using System.Collections.Generic;

namespace Wwise.API.Editor.Tests
{
	public class AkWwiseTreeWAAPIPerformanceMockup : AkWwiseTreeWAAPIMockup
	{
		protected override List<AkWwiseProjectData.WwiseTreeObject> GenerateObjectList(WwiseObjectType type)
		{
			List<AkWwiseProjectData.WwiseTreeObject> output = new List<AkWwiseProjectData.WwiseTreeObject>();
			for (uint i = 0; i < 1000; i++)
			{
				uint typeModifier = (uint)type;
				output.Add(WwiseBrowserTestsUtils.GenerateObject("UpToDate" + i.ToString(), WwiseProjectDatabase.GenerateGuidFromInts(1, 2 + i, 3, typeModifier), "UpToDate" + i.ToString(), type));
				output.Add(WwiseBrowserTestsUtils.GenerateObject("Renamed2" + i.ToString(), WwiseProjectDatabase.GenerateGuidFromInts(2, 3 + i, 4, typeModifier), "Renamed2" + i.ToString(), type));
				output.Add(WwiseBrowserTestsUtils.GenerateObject("Folder\\Moved" + i.ToString(), WwiseProjectDatabase.GenerateGuidFromInts(3, 4 + i, 5, typeModifier), "Moved" + i.ToString(), type));
				output.Add(WwiseBrowserTestsUtils.GenerateObject("NeedsUpdate" + i.ToString(), WwiseProjectDatabase.GenerateGuidFromInts(6, 7 + i, 8, typeModifier), "NeedsUpdate" + i.ToString(), type));
				output.Add(WwiseBrowserTestsUtils.GenerateObject("New" + i.ToString(), WwiseProjectDatabase.GenerateGuidFromInts(7, 8 + i, 9, typeModifier), "New" + i.ToString(), type));
			}
			return output;
		}
	}
}