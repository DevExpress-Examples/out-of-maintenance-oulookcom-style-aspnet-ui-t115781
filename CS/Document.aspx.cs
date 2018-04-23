using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Document : System.Web.UI.Page {
    public String Body {
        get; 
        set; 
    }

    public String Author {
        get;
        set;
    }

    public String DocumentTitle { 
        get; 
        set; 
    }
    
    public String Chapter { 
        get; 
        set; 
    }

    protected void Page_Load(object sender, EventArgs e) {
        
        Body = String.Empty;
        DocumentTitle = String.Empty;
        Author = String.Empty;
        Chapter = String.Empty;

        Guid id;
            
        if (Guid.TryParse(Request.Params["Id"], out id)) {

            DocObj document = WebDbProvider.GetDocument(id);

            if (document != null) {

                Author = document.Author;
                Body = document.Content;
                DocumentTitle = document.Title;
                Chapter = document.Chapter;

            }

        }

        if (String.IsNullOrWhiteSpace(Title)) {
            if (!String.IsNullOrWhiteSpace(Body)) {
                Title = "(No Title)";
            }
        }

    }
}