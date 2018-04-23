using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page, IMenuBuilder {
    public String Author { get; set; }
    
    void IMenuBuilder.BuildMenu(DevExpress.Web.ASPxMenu.ASPxMenu menu) {        
        if (!IsPostBack && !IsCallback) {        
            menu.Items.Add("New", "CMD_NEW");
            menu.Items.Add("Edit", "CMD_EDIT");
        }
    }

    void CreateFolders(String author) {
        
        NavBar.Groups.Clear();
        
        if (String.IsNullOrWhiteSpace(author)) {
            return;
        }

        NavBar.Groups.Add(author);

        NavBar.Groups[0].Items.Clear();
        NavBar.Groups[0].Text = author;

        IList<Book> books = WebDbProvider.GetBooks(author);

        foreach (Book b in books) {

             NavBar.Groups[0].Items.Add(b.Name);
        }

        if (NavBar.Groups[0].Items.Count > 0) {
            NavBar.Groups[0].Items[0].Selected = true;
        } 

    }

    void BindDataGrid(String author, String book, String sort, String mode) {
        this.GridControl.DataSourceID = "";
        this.GridControl.DataSource = WebDbProvider.GetDocuments(author, book, sort, mode);
        this.GridControl.DataBind();
    }

    void CheckMenuItemByName(String name, Boolean bChecked) {
        var menuitem = SortMenu.Items.FindByName(name);        
        if (menuitem != null) { 
            menuitem.Checked = bChecked; 
        }
    }

    void PrepareMenuItems() {
        if (String.Equals((String)Session["Sort"], "Title", StringComparison.InvariantCultureIgnoreCase)) {
            
            CheckMenuItemByName("CMD_SORT_BY_TITLE", true);
            CheckMenuItemByName("CMD_SORT_BY_CHAPTER", false);

            Session["Sort"] = "Title";

        } else {
            
            CheckMenuItemByName("CMD_SORT_BY_TITLE", false);
            CheckMenuItemByName("CMD_SORT_BY_CHAPTER", true);

            Session["Sort"] = "Chapter";

        }
        
        if (String.Equals((String)Session["Mode"], "DESC", StringComparison.InvariantCultureIgnoreCase)) {
            
            CheckMenuItemByName("CMD_SORT_ASC", false);
            CheckMenuItemByName("CMD_SORT_DESC", true);

            Session["Mode"] = "DESC";

        } else {
            
            CheckMenuItemByName("CMD_SORT_ASC", true);
            CheckMenuItemByName("CMD_SORT_DESC", false);

            Session["Mode"] = "ASC";

        }
    }

    void BuildNavBar() {
        CreateFolders((String)Session["Author"]);

        String book = Session["Book"] as String;

        int index = 0;

        if (!String.IsNullOrWhiteSpace(book)) {
            index = NavBar.Groups[0].Items.IndexOfText(book);
            if (index <= 0) {
                index = 0;
            }
        }

        if (NavBar.Groups.Count > 0 && NavBar.Groups[0].Items.Count > 0) {
            NavBar.SelectedItem = NavBar.Groups[0].Items[index];
            Session["Book"] = NavBar.Groups[0].Items[index].Text;
        }
    }

    Guid GetGuid(String id) {
        Guid g; 
        if (Guid.TryParse(id, out g)) {
            return g;
        }
        return Guid.Empty;
    }

    Boolean OnFirstLoad() {
        String author = Request.Params["Author"] as String;

        if (!String.IsNullOrWhiteSpace(author)) {

            Session["Author"] = author;
            Session["Book"] = Request.Params["Book"] as String;
            Session["Id"] = GetGuid(Request.Params["Id"]);
            Author = (String)Session["Author"];

            Response.Redirect("Default.aspx", true);

            return true;

        }

        if (String.IsNullOrWhiteSpace(Session["Author"] as String)) {

            Session["Author"] = "Shakespeare";
            Session["Book"] = null;
            Author = (String)Session["Author"];

        }

        BuildNavBar();

        return false;
    }

    protected void Page_Load(object sender, EventArgs e) {
        
        if (!IsPostBack && !IsCallback) {
            if (OnFirstLoad()) {
                return;
            }
        }
        
        if (GridControl.IsCallback || NavBarCallback.IsCallback) {
            NameValueCollection parameters = SqlParams.Get(CallbackParams.Get(Request), Session);
            if (parameters != null) {                
                Session["Author"] = parameters["Author"];
                Session["Book"] = parameters["Book"];
                Session["Sort"] = parameters["Sort"];
                Session["Mode"] = parameters["Mode"];
                Session["Id"] = GetGuid(parameters["Id"]);

                if (NavBarCallback.IsCallback) {
                    BuildNavBar();  
                }
            }
        }

        Author = (String)Session["Author"];

        PrepareMenuItems();

        BindDataGrid(
            (String)Session["Author"], 
            (String)Session["Book"],
            (String)Session["Sort"],
            (String)Session["Mode"]);

        if (Session["Id"] is Guid) {

            int FocusedRowIndex = GridControl.FindVisibleIndexByKeyValue(Session["Id"]);
            
            if (FocusedRowIndex < 0) {
                FocusedRowIndex = 0;
            }

            GridControl.FocusedRowIndex = FocusedRowIndex;

            Session["Id"] = null;

        }
           
    }

}