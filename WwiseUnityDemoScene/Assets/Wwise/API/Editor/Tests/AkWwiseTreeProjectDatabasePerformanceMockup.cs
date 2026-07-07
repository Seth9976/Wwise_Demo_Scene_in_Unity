using System.Collections.Generic;

namespace Wwise.API.Editor.Tests
{
	public class AkWwiseTreeProjectDatabasePerformanceMockup : AkWwiseTreeProjectDatabaseMockup
	{
		protected override List<AkWwiseProjectData.WwiseTreeObject> GenerateObjectList(WwiseObjectType type)
		{
			List<AkWwiseProjectData.WwiseTreeObject> output = new List<AkWwiseProjectData.WwiseTreeObject>();
			for (uint i = 0; i < 1000; i++)
			{
				uint typeModifier = (uint)type;
				output.Add(WwiseBrowserTestsUtils.GenerateObject("UpToDate" + i.ToString(), WwiseProjectDatabase.GenerateGuidFromInts(1, 2 + i, 3, typeModifier), "UpToDate" + i.ToString(), type));
				output.Add(WwiseBrowserTestsUtils.GenerateObject("Renamed" + i.ToString(), WwiseProjectDatabase.GenerateGuidFromInts(2, 3 + i, 4, typeModifier), "Renamed" + i.ToString(), type));
				output.Add(WwiseBrowserTestsUtils.GenerateObject("Moved" + i.ToString(), WwiseProjectDatabase.GenerateGuidFromInts(3, 4 + i, 5, typeModifier), "Moved", type));
				output.Add(WwiseBrowserTestsUtils.GenerateObject("NeedsUpdate" + i.ToString(), WwiseProjectDatabase.GenerateGuidFromInts(4, 5 + i, 6, typeModifier), "NeedsUpdate" + i.ToString(), type));
				output.Add(WwiseBrowserTestsUtils.GenerateObject("Deleted" + i.ToString(), WwiseProjectDatabase.GenerateGuidFromInts(5, 6 + i, 7, typeModifier), "Deleted" + i.ToString(), type));
			}
			return output;
		}
	}
}