using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Web.UI.WebControls;
using App_Code;
using BlogEngine.Core;
using BlogEngine.Core.Web.Extensions;
using CodeCarvings.Piczard;
using CodeCarvings.Piczard.Web;
using Resources;
using Page = System.Web.UI.Page;

namespace Admin.Sponsoren
{
    public partial class EditSponsor : Page
    {
        private string SponsorID;
        private string Delete;
        private Guid GuidID;
        private bool IsEdit;
        private bool IsDelete;
        private CRSponsor crSponsor;

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event to initialize the sponsor.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            //TODO create sponsor right
            WebUtils.CheckRightsForAdminPagesPages(false);
            MaintainScrollPositionOnPostBack = true;

            SponsorID = Request.QueryString["id"];
            Delete = Request.QueryString["delete"];
            if (!String.IsNullOrEmpty(SponsorID))
                IsEdit = Guid.TryParse(SponsorID, out GuidID);
            if (!String.IsNullOrEmpty(Delete))
                IsDelete = Guid.TryParse(Delete, out GuidID);
            
            if (!IsPostBack)
            {
                imgLogo.CropConstraint = new FixedCropConstraint(GfxUnit.Pixel,300, 194.117F);
                imgLogo.PreviewFilter = new FixedResizeConstraint(GfxUnit.Pixel, 300, 194.117F, Color.Black);
            }

            if (IsEdit)
            {
                PageTitle.Text = labels.editSponsor;
                crSponsor = new CRSponsor(GuidID, false);
                txtName.Text = crSponsor.Name;
                txtUrl.Text = crSponsor.Url;

                BindSponsorTypes(crSponsor.SponsorType);
                //ddlSponsorType.SelectedValue = ((int) crSponsor.SponsorType).ToString();
                    
                if (!string.IsNullOrEmpty(crSponsor.LogoURL))
                    imgLogo.LoadImageFromFileSystem(crSponsor.LogoPhysicalPath);
                cbActive.Checked = crSponsor.Active;

                cbShowInWidget.Checked = crSponsor.WidgetSwitch;
                cbShowInPlayerSwitch.Checked = crSponsor.PlayerSwitch;
                cbShowInPlayerSolid.Checked = crSponsor.PlayerSolid;
                cbShowInMobileSwitch.Checked = crSponsor.MobileSwitch;
                cbShowInMobileSolid.Checked = crSponsor.MobileSolid;

                ddlMobileFrequency.SelectedValue = ((int)crSponsor.MFrequency).ToString();

                txtDescription.Text = crSponsor.Description;
                dtEndDate.SetDate(crSponsor.EndDate);


            }
            else if(IsDelete)
            {
                crSponsor = new CRSponsor(GuidID, true);
                Response.Write(crSponsor.Delete().ToString());
                Response.Redirect("Sponsoren.aspx");
            }
            else
            {
                PageTitle.Text = labels.addNewSponsor;
                crSponsor = new CRSponsor();
                BindSponsorTypes(SponsorType.VriendenVanCr);
            }


            if(IsEdit)
            {
            }
            else if (!String.IsNullOrEmpty(Request.QueryString["delete"]) &&
                     Request.QueryString["delete"].Length == 36)
            {
                var id = new Guid(Request.QueryString["delete"]);
            }
            else
            {
                //TODO create sponsor right
                if (!Security.IsAuthorizedTo(Rights.CreateNewPages))
                {
                    Response.Redirect(Utils.RelativeWebRoot);
                    return;
                }

               //TODO create sponsor right
                //cbActive.Checked = Security.IsAuthorizedTo(Rights.PublishOwnPages);
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
            base.OnLoad(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selected"></param>
        private void BindSponsorTypes(SponsorType selected)
        {
            foreach (SponsorType sp in Enum.GetValues(typeof (SponsorType)))
            {
                ddlSponsorType.Items.Add(new ListItem(sp.ToString(), ((int) sp).ToString()));
            }

            ddlSponsorType.SelectedValue = ((int) selected).ToString();
        }


        /// <summary>
        /// Handles the Click event of the btnSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public void BtnSaveClick(object sender, EventArgs e)
        {
            crSponsor.Name = txtName.Text;
            crSponsor.Url = txtUrl.Text;

            if (!string.IsNullOrEmpty(ddlSponsorType.SelectedValue))
                crSponsor.SponsorType = (SponsorType)Convert.ToInt32(ddlSponsorType.SelectedValue);

            crSponsor.WidgetSwitch = cbShowInWidget.Checked;
            crSponsor.PlayerSwitch = cbShowInPlayerSwitch.Checked;
            crSponsor.PlayerSolid = cbShowInPlayerSolid.Checked;
            crSponsor.MobileSwitch = cbShowInMobileSwitch.Checked;
            crSponsor.MobileSolid = cbShowInMobileSolid.Checked;
            if(!string.IsNullOrEmpty(ddlMobileFrequency.SelectedValue))
                crSponsor.MFrequency = (MobileFrequency)Convert.ToInt32(ddlMobileFrequency.SelectedValue);
            if(imgLogo.HasImage)
            {
                crSponsor.LogoURL = string.Format("Upload/sponsoren/{0}.png", crSponsor.ID);
                if(imgLogo.ImageEdited)
                    imgLogo.SaveProcessedImageToFileSystem(crSponsor.LogoPhysicalPath);
            }
            crSponsor.EndDate = dtEndDate.Date;
            crSponsor.Description = txtDescription.Text;
            crSponsor.Active = cbActive.Checked;
            
            crSponsor.Save();

            Response.Redirect("Sponsoren.aspx");
        }
    }
}