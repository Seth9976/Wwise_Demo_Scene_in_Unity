using System.Collections;
using System.Collections.Generic;
using AK.Wwise.Unity.Logging;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Wwise.API.Editor.Tests;

public class MergedDataSourceTests
{
    [Test]
    public void MergedDataSourceTestsSimplePasses()
    {
        AkWwiseTreeProjectDatabaseMockup projectMockup = new AkWwiseTreeProjectDatabaseMockup();
        AkWwiseTreeWAAPIMockup waapiMockup = new AkWwiseTreeWAAPIMockup();
        AkWwiseTreeMergedDataSource dataSource = new AkWwiseTreeMergedDataSource(projectMockup, waapiMockup);
        dataSource.FetchData();
        dataSource.MergeDataSources();

        Assert.IsTrue(dataSource.ProjectRoot.children.Count > 0);

        foreach(var child in dataSource.ProjectRoot.children)
        {
            Assert.IsTrue(child.children.Count == 6);
            Assert.IsTrue((child.children[0] as AkWwiseTreeViewItem).IsUpToDate);
            Assert.IsTrue((child.children[1] as AkWwiseTreeViewItem).IsRenamedInWwise);
            Assert.IsTrue((child.children[2] as AkWwiseTreeViewItem).IsMovedInWwise);
            Assert.IsTrue((child.children[3] as AkWwiseTreeViewItem).bDifferentGuid);
            Assert.IsTrue((child.children[4] as AkWwiseTreeViewItem).IsDeletedInWwise);
            Assert.IsTrue((child.children[5] as AkWwiseTreeViewItem).bNewInWwise);            
        }
    }
    
    [Test]
    public void MergedDataSourceTestsPerformancePasses()
    {
        AkWwiseTreeProjectDatabasePerformanceMockup projectMockup = new AkWwiseTreeProjectDatabasePerformanceMockup();
        AkWwiseTreeWAAPIPerformanceMockup waapiMockup = new AkWwiseTreeWAAPIPerformanceMockup();
        AkWwiseTreeMergedDataSource dataSource = new AkWwiseTreeMergedDataSource(projectMockup, waapiMockup);
        dataSource.FetchData();
        var time = System.DateTime.Now;
        dataSource.MergeDataSources();

        Assert.IsTrue(dataSource.ProjectRoot.children.Count > 0);

        WwiseLogger.Log("This run took:" + (System.DateTime.Now - time).TotalSeconds + " seconds");
        Assert.IsTrue((System.DateTime.Now - time).Seconds < 3);
    }
}
