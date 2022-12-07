using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;

    public partial class RootMaster : System.Web.UI.MasterPage {
        protected void Page_Load(object sender, EventArgs e) {
            IMenuBuilder menuBuilder = this.MainContentPlaceHolder.Page as IMenuBuilder;
            if (menuBuilder != null) {
                menuBuilder.BuildMenu(MainMenu);
            }
        }
    }