// --------------------------------------------------------------------------------------------------------------------
// <summary>
//   The page editor.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Web.UI.WebControls;
using App_Code;
using BlogEngine.Core;
using BlogEngine.Core.Web.Extensions;
using CodeCarvings.Piczard;
using Resources;
using Page = System.Web.UI.Page;

namespace Admin.Sponsoren
{
    public partial class EditSponsor : Page
    {
        private string ID;
        private bool IsEdit;
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

            ID = Request.QueryString["id"];
            IsEdit = !String.IsNullOrEmpty(ID) && ID.Length == 36;

            if (IsEdit)
            {
                crSponsor = new CRSponsor(new Guid(ID));
                txtName.Text = crSponsor.Name;
                txtUrl.Text = crSponsor.Url;
                cbActive.Checked = crSponsor.Active;

                BindSponsorTypes(SponsorType.Hoofdsponsor);
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

                crSponsor = new CRSponsor();
                BindSponsorTypes(SponsorType.Sponsor);

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
            if (!IsPostBack)
            {
                imgLogo.CropConstraint = new FixedCropConstraint(300, 200);
                imgLogo.PreviewFilter = new FixedResizeConstraint(300, 200, Color.Black);
            }
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
            Response.Write(crSponsor.Save());
        }
    }
}