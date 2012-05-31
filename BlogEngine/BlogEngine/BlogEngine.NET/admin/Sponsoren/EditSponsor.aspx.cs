// --------------------------------------------------------------------------------------------------------------------
// <summary>
//   The page editor.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using App_Code;
using BlogEngine.Core;
using CodeCarvings.Piczard;
using Resources;
using Page = System.Web.UI.Page;

namespace Admin.Sponsoren
{
    public partial class EditSponsor : Page
    {
        protected string PageUrl
        {
            get
            {
                if (!String.IsNullOrEmpty(Request.QueryString["id"]) && Request.QueryString["id"].Length == 36)
                {
                    var id = new Guid(Request.QueryString["id"]);
                    BlogEngine.Core.Page pg = BlogEngine.Core.Page.GetPage(id);
                    return pg.RelativeLink;
                }
                return string.Empty;
            }
        }

        #region Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event to initialize the page.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            WebUtils.CheckRightsForAdminPagesPages(false);
            MaintainScrollPositionOnPostBack = true;

            if (!String.IsNullOrEmpty(Request.QueryString["id"]) && Request.QueryString["id"].Length == 36)
            {
                var id = new Guid(Request.QueryString["id"]);
                BindPage(id);
                BindSponsorTypes(SponsorType.Hoofdsponsor);
            }
            else if (!String.IsNullOrEmpty(Request.QueryString["delete"]) &&
                     Request.QueryString["delete"].Length == 36)
            {
                var id = new Guid(Request.QueryString["delete"]);
                DeletePage(id);
            }
            else
            {
                if (!Security.IsAuthorizedTo(Rights.CreateNewPages))
                {
                    Response.Redirect(Utils.RelativeWebRoot);
                    return;
                }

                BindSponsorTypes(SponsorType.Sponsor);
                cbPublished.Checked = Security.IsAuthorizedTo(Rights.PublishOwnPages);
            }
            Page.Title = labels.sponsoren;

            base.OnInit(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load"/> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.imgLogo.CropConstraint = new FixedCropConstraint(300, 200);
                this.imgLogo.PreviewFilter = new FixedResizeConstraint(300, 200, Color.Black);


                //this.ImageUpload2.PostProcessingFilter = new ScaledResizeConstraint(300, 300);
            }
            base.OnLoad(e);
        }

        /// <summary>
        /// The bind page.
        /// </summary>
        /// <param name="pageId">
        /// The page id.
        /// </param>
        private void BindPage(Guid pageId)
        {
            BlogEngine.Core.Page page = BlogEngine.Core.Page.GetPage(pageId);

            if (page == null || !page.CanUserEdit)
            {
                Response.Redirect(Request.Path);
                return;
            }

            txtName.Text = page.Title;

            txtDescription.Text = page.Description;
            cbShowInPlayerSwitch.Checked = page.IsFrontPage;
            cbShowInPlayerSolid.Checked = page.ShowInList;
            cbPublished.Checked = page.IsPublished;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selected"></param>
        private void BindSponsorTypes(SponsorType selected)
        {
            foreach (SponsorType sp in Enum.GetValues(typeof(SponsorType)))
            {
                ddlSponsorType.Items.Add(new ListItem(sp.ToString(), ((int)sp).ToString()));
            }

            ddlSponsorType.SelectedValue = ((int)selected).ToString();
        }

        /// <summary>
        /// Builds the child page list.
        /// </summary>
        /// <param name="page">The page to make a child list for.</param>
        /// <returns>The page list.</returns>
        private HtmlGenericControl BuildChildPageList(BlogEngine.Core.Page page)
        {
            var ul = new HtmlGenericControl("ul");
            foreach (BlogEngine.Core.Page childPage in BlogEngine.Core.Page.Pages.FindAll(p => p.Parent == page.Id))
            {
                var cLi = new HtmlGenericControl("li");
                cLi.Attributes.CssStyle.Add("font-weight", "normal");
                var cA = new HtmlAnchor {HRef = string.Format("?id={0}", childPage.Id), InnerHtml = childPage.Title};

                var childText =
                    new LiteralControl(string.Format(" ({0}) ", childPage.DateCreated.ToString("yyyy-dd-MM HH:mm")));

                const string DeleteText = "Are you sure you want to delete the page?";
                var delete = new HtmlAnchor {InnerText = labels.delete};
                delete.Attributes["onclick"] = string.Format("if (confirm('{0}')){{location.href='?delete={1}'}}",
                                                             DeleteText, childPage.Id);
                delete.HRef = "javascript:void(0);";
                delete.Style.Add(HtmlTextWriterStyle.FontWeight, "normal");

                cLi.Controls.Add(cA);
                cLi.Controls.Add(childText);
                cLi.Controls.Add(delete);

                if (childPage.HasChildPages)
                {
                    cLi.Attributes.CssStyle.Remove("font-weight");
                    cLi.Attributes.CssStyle.Add("font-weight", "bold");
                    cLi.Controls.Add(BuildChildPageList(childPage));
                }

                ul.Controls.Add(cLi);
            }

            return ul;
        }

        /// <summary>
        /// The delete page.
        /// </summary>
        /// <param name="pageId">
        /// The page id.
        /// </param>
        private void DeletePage(Guid pageId)
        {
            BlogEngine.Core.Page page = BlogEngine.Core.Page.GetPage(pageId);
            if (page == null)
            {
                return;
            }
            if (!page.CanUserDelete)
            {
                Response.Redirect(Utils.RelativeWebRoot);
                return;
            }

            page.Delete();
            page.Save();
            Response.Redirect("pages.aspx");
        }


        /// <summary>
        /// Formats the size.
        /// </summary>
        /// <param name="size">The size to format.</param>
        /// <param name="formatString">The format string.</param>
        /// <returns>The formatted string.</returns>
        private static string SizeFormat(float size, string formatString)
        {
            if (size < 1024)
            {
                return string.Format("{0} bytes", size.ToString(formatString));
            }

            if (size < Math.Pow(1024, 2))
            {
                return string.Format("{0} kb", (size/1024).ToString(formatString));
            }

            if (size < Math.Pow(1024, 3))
            {
                return string.Format("{0} mb", (size/Math.Pow(1024, 2)).ToString(formatString));
            }

            if (size < Math.Pow(1024, 4))
            {
                return string.Format("{0} gb", (size/Math.Pow(1024, 3)).ToString(formatString));
            }

            return size.ToString(formatString);
        }


        /// <summary>
        /// Handles the Click event of the btnSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void BtnSaveClick(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                throw new InvalidOperationException("One or more validators are invalid.");
            }

            BlogEngine.Core.Page page = Request.QueryString["id"] != null
                                            ? BlogEngine.Core.Page.GetPage(new Guid(Request.QueryString["id"]))
                                            : new BlogEngine.Core.Page();


            page.Title = txtName.Text;

            page.Description = txtDescription.Text;

            if (cbShowInPlayerSwitch.Checked)
            {
                foreach (
                    BlogEngine.Core.Page otherPage in
                        BlogEngine.Core.Page.Pages.Where(otherPage => otherPage.IsFrontPage))
                {
                    otherPage.IsFrontPage = false;
                    otherPage.Save();
                }
            }

            page.IsFrontPage = cbShowInPlayerSwitch.Checked;
            page.ShowInList = cbShowInPlayerSolid.Checked;
            page.IsPublished = cbPublished.Checked;

            page.Parent = ddlSponsorType.SelectedIndex != 0 ? new Guid(ddlSponsorType.SelectedValue) : Guid.Empty;

            page.Save();

            Response.Redirect(page.RelativeLink);
        }

        #endregion
    }
}