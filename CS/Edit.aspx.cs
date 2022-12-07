using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Edit : System.Web.UI.Page, IMenuBuilder {
    void Insert() {

        DocObj docObj = new DocObj();
        docObj.Author = (String)Request.Form["_H_AUTHOR"];
        docObj.Title = (String)Request.Form["_H_TITLE"];
        docObj.Chapter = (String)Request.Form["_H_CHAPTER"];
        docObj.Content = (String)Request.Form["_H_HTML"];
        docObj.Book = (String)Request.Form["_H_BOOK"];
        docObj.Year = (String)Request.Form["_H_YEAR"];

        DocObj docInserted = WebDbProvider.InsertDocument(docObj);

        Session["Author"] = docInserted.Author;
        Session["Book"] = docInserted.Book;
        Session["Id"] = docInserted.Id;
    }

    void Update(Guid id) {

        DocObj docObj = new DocObj();
        docObj.Id = id;
        docObj.Book = (String)Request.Form["_H_BOOK"];
        docObj.Author = (String)Request.Form["_H_AUTHOR"];
        docObj.Chapter = (String)Request.Form["_H_CHAPTER"];
        docObj.Title = (String)Request.Form["_H_TITLE"];
        docObj.Content = (String)Request.Form["_H_HTML"];
        docObj.Year = (String)Request.Form["_H_YEAR"];

        WebDbProvider.SaveDocument(docObj);

        Session["Author"] = docObj.Author;
        Session["Book"] = docObj.Book;
        Session["Id"] = id;


    }

    void Select(Guid id) {

        DocObj view = WebDbProvider.GetDocument(id);

        if (view != null) {

            ASPxHtmlEditor.Html = view.Content;
            ASPxComboBox_Author.Value = view.Author;
            ASPxTextBox_Chapter.Value = view.Chapter;
            ASPxTextBox_Title.Value = view.Title;
            ASPxTextBox_Book.Value = view.Book;
            ASPxTextBox_Year.Value = view.Year;

            Title = String.Format("{0}: {1}", ASPxTextBox_Book.Value, ASPxTextBox_Title.Value);

        }

    }

    protected void Page_Load(object sender, EventArgs e) {
        ASPxComboBox_Author.Items.Clear();
        foreach (Author i in WebDbProvider.Authors) {
            ASPxComboBox_Author.Items.Add(i.Id);
        }

        Guid id = Guid.Empty;

        if (!Guid.TryParse(Request.Params["Id"], out id)) {
            id = Guid.Empty;
        }

        Session["Id"] = id;

        if (IsPostBack) {

#if ALLOW_SAVE
            
            if (id == Guid.Empty && (String.IsNullOrWhiteSpace(Request.Params["Id"]) || Request.Params["Id"] == "-1")) {

                Insert();

            } else if (id != Guid.Empty) {

                Update(id);

            }

            Response.Redirect(String.Format("Default.aspx?Author={0}&Book={1}&Id={2}",
                            (String)Session["Author"],
                            (String)Session["Book"],
                            ((Guid)Session["Id"]).ToString("N")),
                            true);
#endif

            return;

        } else {

            if (id == Guid.Empty) {

                String author = Session["Author"] as String;

                int index = ASPxComboBox_Author.Items.IndexOfText(author);
                if (index >= 0) {

                    ASPxComboBox_Author.Value = author;

                } else {

                    ASPxComboBox_Author.Value = String.Empty;

                }

            } else {

                Select(id);

            }

        }

    }




    void IMenuBuilder.BuildMenu(DevExpress.Web.ASPxMenu menu) {
        if (!IsPostBack && !IsCallback) {

            menu.Items.Add("Save", "CMD_SAVE");
            menu.Items.Add("Cancel", "CMD_CANCEL");

        }
    }
}