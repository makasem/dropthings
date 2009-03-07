﻿#region Header

// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

#endregion Header

/// <summary>
/// Summary description for WidgetService
/// </summary>
namespace Dropthings.Web.Framework
{
    using System;
    using System.Collections;
    using System.Web;
    using System.Web.Script.Services;
    using System.Web.Services;
    using System.Web.Services.Protocols;
    using System.Workflow.Runtime;

    using Dropthings.Business;
    using Dropthings.Business.Container;
    using Dropthings.Business.Workflows;
    using Dropthings.Business.Workflows.EntryPointWorkflows;
    using Dropthings.Business.Workflows.TabWorkflows;
    using Dropthings.Business.Workflows.WidgetWorkflows;
    using Dropthings.DataAccess;
    using Dropthings.Web.Framework;

    public class PageService : WebServiceBase
    {
        #region Constructors

        public PageService()
        {
            //Uncomment the following line if using designed components
            //InitializeComponent();
        }

        #endregion Constructors

        #region Methods

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public void ChangeCurrentPage(int pageId)
        {
            //new Dropthings.Business.DashboardFacade(Profile.UserName).ChangeCurrentTab(pageId);

            RunWorkflow.Run<ChangeTabWorkflow, ChangeTabWorkflowRequest, ChangeTabWorkflowResponse>(
                new ChangeTabWorkflowRequest { PageID = pageId, UserName = Profile.UserName }
            );
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public void ChangePageLayout(int newLayout)
        {
            //IUserVisitWorkflowResponse setup = new Dropthings.Business.DashboardFacade(Profile.UserName).LoadUserSetup(string.Empty);

            ////if new layout is 3 cols then do nothing
            ////if new layout is 2 cols we need to move the 3rd into the second
            //if (newLayout == 2 | newLayout == 3 | newLayout == 5)
            //{
            //    foreach (WidgetInstance instance in setup.WidgetInstances)
            //    {
            //        if (setup.CurrentPage.ID == instance.PageId)
            //        {
            //            if (instance.ColumnNo == 2)
            //            {
            //                //move widget to the middle panel
            //                new DashboardFacade(Profile.UserName).MoveWidgetInstance(instance.Id, 1, 0);
            //            }
            //        }
            //    }

            //}
            //else if (newLayout == 4)
            //{ //move the widgets from both middle panels to the left panel.

            //    foreach (WidgetInstance instance in setup.WidgetInstances)
            //    {
            //        if (setup.CurrentPage.ID == instance.PageId)
            //        {
            //            if (instance.ColumnNo == 1 | instance.ColumnNo == 2)
            //            {
            //                new DashboardFacade(Profile.UserName).MoveWidgetInstance(instance.Id, 0, 0);
            //            }
            //        }
            //    }

            //}

            //new DashboardFacade(Profile.UserName).ModifyPageLayout(setup.CurrentPage.ID, newLayout);
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public string DeletePage(int PageID)
        {
            var response = RunWorkflow.Run<DeletePageWorkflow, DeleteTabWorkflowRequest, DeleteTabWorkflowResponse>(
                new DeleteTabWorkflowRequest { PageID = PageID, UserName = Profile.UserName }
            );

            Context.Cache.Remove(Profile.UserName);
            return response.NewCurrentPage.TabName;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public string NewPage(string newLayout)
        {
            //var newPage = new DashboardFacade(Profile.UserName).AddNewPage(newLayout);
            //return newPage.TabName();

            var response = RunWorkflow.Run<AddNewTabWorkflow, AddNewTabWorkflowRequest, AddNewTabWorkflowResponse>(
                new AddNewTabWorkflowRequest { LayoutType = newLayout, UserName = Profile.UserName }
            );

            return response.NewPage.TabName;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public void RenamePage(string newName)
        {
            RunWorkflow.Run<ChangePageNameWorkflow, ChangeTabNameWorkflowRequest, ChangeTabNameWorkflowResponse>(
                new ChangeTabNameWorkflowRequest { IsAnonymous = Profile.IsAnonymous, PageName = newName, UserName = Profile.UserName }
            );
        }

        #endregion Methods
    }
}