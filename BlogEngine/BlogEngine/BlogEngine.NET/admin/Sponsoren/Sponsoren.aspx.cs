// --------------------------------------------------------------------------------------------------------------------
// <summary>
//   The admin pages pages.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BlogEngine.Core.Providers;

namespace Admin.Sponsoren
{
    using System;
    using Resources;

    using Page = System.Web.UI.Page;
    using App_Code;
    using BlogEngine.Core.Web.Extensions;

    /// <summary>
    /// The admin pages pages.
    /// </summary>
    public partial class SponsorenPage : Page
    {
        #region Methods
        static protected ExtensionSettings Settings;
        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event to initialize the page.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            //TODO check admin rights for sponsor instead of pages
            WebUtils.CheckRightsForAdminPagesPages(false);
            MaintainScrollPositionOnPostBack = true;

            Page.Title = labels.sponsoren;



            base.OnInit(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load"/> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            Settings = ExtensionManager.GetSettings("Sponsor");
          
            base.OnLoad(e);

            if (!IsPostBack)
            {
                BindGrid();
            }
        }

        private void BindGrid()
        {
            grid.DataSource = Settings.GetDataTable();
            grid.DataBind();
        }



        /*
        <ul class="rowTools">
                    <li>
                        <a class="toolsAction" href="#"><span class="">Tools</span></a>
                        <ul class="rowToolsMenu">
                            {#if $T.p.CanUserEdit}<li><a class="editAction" href="EditPage.aspx?id={$T.p.Id}">Wijzigen</a></li>{#/if}
                            {#if $T.p.HasChildren == false && $T.p.CanUserDelete}<li><a href="#" class="deleteAction" onclick="return DeletePage(this);">Verwijderen</a></li>{#/if}
                       </ul>
                    </li>
        </ul>*/

        #endregion

        protected void GridRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                e.Row.Cells[0].Text = e.Row.Cells[0].Text == "True"
                                          ? "<span class=\"published-true\"></span>"
                                          : (e.Row.Cells[0].Text == "False"
                                                 ? "<span class=\"published-false\"></span>"
                                                 : "");
                //Response.Write(DateTime.Now.ToString());

                const string format = "dd-MM-yyyy HH:mm:ss"; 
                if (!string.IsNullOrEmpty(e.Row.Cells[2].Text))
                    e.Row.Cells[2].Text = DateTime.ParseExact((e.Row.Cells[2].Text), format, null).ToString("dd-MM-yyyy");

            }
        }
    }
}