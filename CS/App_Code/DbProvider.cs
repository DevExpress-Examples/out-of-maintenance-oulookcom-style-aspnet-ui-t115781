using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;

public class Author {
    public String Id { get; set; }
    public String Display { get; set; }
    public String Footer { get; set; }
    public String Desciption { get; set; }
}

public class Book {
    public String Name { get; set; }
}

public class DocObj {
    public Guid Id { get; set; }
    public String Author { get; set; }
    public String Book { get; set; }
    public String Title { get; set; }
    public String Chapter { get; set; }
    public String Content { get; set; }
    public String Year { get; set; }
}

public class MemCache {
    public static void Remove(Guid id) {
        Cache cache = HttpRuntime.Cache;
        if (cache == null) {
            return;
        }
        cache.Remove(id.ToString("N"));
    }
    public static Object Cache(String key) {
        Cache cache = HttpRuntime.Cache;
        if (cache == null)
            return null;
        Object r = cache.Get(key);
        return r;
    }
    public static void Cache(String key, Object obj) {        
        Cache cache = HttpRuntime.Cache;
        if (cache == null)
            return;
        const int minutes = 0xFF;
        if (obj == null)
            cache.Remove(key);
        else
            cache.Insert(key, obj, null,
                System.Web.Caching.Cache.NoAbsoluteExpiration,
                TimeSpan.FromMinutes(minutes));
    }
}

public class WebDbProvider {
    public static Author[] Authors = new Author[] { 
    
        new Author() {
            Id = "Shakespeare",
            Display = "Shakespeare, William",
            Footer = "16th century",
            Desciption = "William Shakespeare was an English poet, playwright and actor, widely regarded as the greatest writer in the English language.", 
        },

        new Author() {
            Id = "Poe",
            Display = "Poe, Edgar Allan",
            Footer = "19th century",
            Desciption = "Edgar Allan Poe was an American author, poet, editor, and literary critic, considered part of the American Romantic Movement.", 
        },

        new Author() {
            Id = "Dickinson",
            Display = "Dickinson, Emily",
            Footer = "19th century",
            Desciption = "Emily Elizabeth Dickinson was an American poet. Born in Amherst, Massachusetts. She lived a mostly introverted and reclusive life.", 
        },

    };

    public static String GetAuthorDescription(String author) {
        foreach (Author a in Authors) {
            if (String.Equals(a.Id, author, StringComparison.InvariantCultureIgnoreCase)) {
                return a.Desciption;
            }
        }
        return String.Empty;
    }

    public static IList<Book> GetBooks(String author) {
            
#if TRACE
            Trace.WriteLine(String.Format("GetBooks({0})",
                author));

#endif
        String key = String.Format("GetBooks(\"{0}\")", author);

        Object obj = MemCache.Cache(key);

        if (obj is IList<Book>) {
            return (IList<Book>)obj;
        }

        obj = _DbProvider.GetBooks(author);

        MemCache.Cache(
            key, 
            obj);

        return (IList<Book>)obj;
    }

     public static IList<DocObj> GetDocuments(String author, String book, String sort, String mode) {

#if TRACE
            Trace.WriteLine(String.Format("GetDocuments({0}, {1})",
                author, book));

#endif
        
        String key = String.Format("GetDocuments(\"{0}\", \"{1}\")", author, book);

        Object obj = MemCache.Cache(key);

        if (!(obj is IList<DocObj>)) {

            obj = _DbProvider.GetDocuments(author, book, sort, mode);

            MemCache.Cache(
                key,
                obj);

        }

        if (!(String.Equals(sort, "Title", StringComparison.InvariantCultureIgnoreCase)
                       || String.Equals(sort, "Chapter", StringComparison.InvariantCultureIgnoreCase))) {

            sort = "Chapter";
        }

        if (!(String.Equals(mode, "Asc", StringComparison.InvariantCultureIgnoreCase)
                                || String.Equals(mode, "Desc", StringComparison.InvariantCultureIgnoreCase))) {

            mode = "ASC";
        }


        if (String.Equals(sort, "Title", StringComparison.InvariantCultureIgnoreCase)) {
            if (String.Equals(mode, "ASC", StringComparison.InvariantCultureIgnoreCase)) {
                return ((IList<DocObj>)obj).OrderBy(k => k.Title).ToList();
            } else {
                return ((IList<DocObj>)obj).OrderByDescending(k => k.Title).ToList();
            }
        } else {
            if (String.Equals(mode, "ASC", StringComparison.InvariantCultureIgnoreCase)) {
                return ((IList<DocObj>)obj).OrderBy(k => k.Chapter).ToList();
            } else {
                return ((IList<DocObj>)obj).OrderByDescending(k => k.Chapter).ToList();
            }
        }

    }

     public static DocObj GetDocument(Guid id) {

#if TRACE
         Trace.WriteLine(String.Format("GetDocuments({0})", id));
#endif

         String key = String.Format("GetDocuments(\"{0}\")", id);

         Object obj = MemCache.Cache(key);

         if (obj is DocObj) {

             return (DocObj)obj;

         }

         obj = _DbProvider.GetDocument(id);

         MemCache.Cache(
             key,
             obj);

         return (DocObj)obj;

     }

     public static void SaveDocument(DocObj doc) {

         String key = String.Format("GetDocuments(\"{0}\")", doc.Id);

         _DbProvider.SaveDocument(doc);
         
         MemCache.Cache(
             key,
             doc);

         MemCache.Cache(
            String.Format("GetDocuments(\"{0}\", \"{1}\")", doc.Author, doc.Book),
            null);

     }

     public static DocObj InsertDocument(DocObj doc) {

         DocObj inserted = _DbProvider.InsertDocument(doc);

         String key = String.Format("GetDocuments(\"{0}\")", inserted.Id);

         MemCache.Cache(
             key,
             inserted);

         MemCache.Cache(
            String.Format("GetDocuments(\"{0}\", \"{1}\")", doc.Author, doc.Book),
            null);

         return inserted;

     }
}

public class _DbProvider {
    public _DbProvider() {
    }

    #region GetBooks
    public static IList<Book> GetBooks(String author) {
        if (String.IsNullOrWhiteSpace(author)) {
            return new List<Book>();
        }

        author = author.Trim();

        using (AccessDataSource dataSource = new AccessDataSource("~/App_Data/Reader.mdb", String.Format(@"

            SELECT DISTINCT [Book] FROM Documents 

    WHERE [Author] = @Author

    ORDER BY [Book] ASC

", author))) {

            dataSource.SelectParameters.Add("Author", DbType.String, author);

            bool bHasOther = false;
            List<Book> List = new List<Book>();

            DataView view = (DataView)dataSource.Select(DataSourceSelectArguments.Empty);

            if (view != null) {

                for (int i = 0; i < view.Count; i++) {
                    String title = Convert.ToString(view.Table.Rows[i]["Book"]);
                    if (!String.IsNullOrWhiteSpace(title) && title != "Other") {
                        List.Add(new Book() { Name = title });
                    } else {
                        bHasOther = true;
                    }
                }

            }

            if (bHasOther) {
                if (List.Count > 0) {
                    
                    List.Add(new Book() { Name = "Other" });
                } else {
                    
                    List.Add(new Book() { Name = "All" });
                }
            } else {
                if (List.Count <= 0) {

                    List.Add(new Book() { Name = "All" });
                }
            }

            return List;

        }

    }
    #endregion

    #region GetDocuments
    public static IList<DocObj> GetDocuments(String author, String book, String sort, String mode) {
        if (String.IsNullOrWhiteSpace(book)
                                || String.Equals(book, "Other", StringComparison.InvariantCultureIgnoreCase)) {

            book = "Other";
        }

        if (!(String.Equals(sort, "Title", StringComparison.InvariantCultureIgnoreCase)
                                || String.Equals(sort, "Chapter", StringComparison.InvariantCultureIgnoreCase))) {

            sort = "Chapter";
        }
        
        if (!(String.Equals(mode, "Asc", StringComparison.InvariantCultureIgnoreCase)
                                || String.Equals(mode, "Desc", StringComparison.InvariantCultureIgnoreCase))) {

            mode = "ASC";
        }

        String ORDER = String.Format("ORDER BY [{0}] {1}", sort, mode);

        using (AccessDataSource dataSource = new AccessDataSource("~/App_Data/Reader.mdb", String.Format(

@"SELECT

 [Id], [Book], [Author], [Title], [Chapter], [_Year] FROM Documents

WHERE [Author] = @Author AND 
(
                ([Book] = @Book) OR
                (      
                    
                        (@Book = 'Other' And ([Book] is null or [Book] = 'Other' or [Book] = '') ) 
                ) OR
                (      
                    
                        (@Book = 'All') 
                )
)

{0}", ORDER))) {

            List<DocObj> docs = new List<DocObj>();

            dataSource.SelectParameters.Add("Author", DbType.String, author);
            dataSource.SelectParameters.Add("Book", DbType.String, book);

            DataView view = dataSource.Select(DataSourceSelectArguments.Empty) as DataView;

            if (view != null) {

                for (int i = 0; i < view.Count; i++) {

                    docs.Add(new DocObj() {
                        Id = (Guid)(view.Table.Rows[i]["Id"]),
                        Author = Convert.ToString(view.Table.Rows[i]["Author"]),
                        Book = Convert.ToString(view.Table.Rows[i]["Book"]),
                        Title = Convert.ToString(view.Table.Rows[i]["Title"]),
                        Chapter = Convert.ToString(view.Table.Rows[i]["Chapter"]),
                        Year = Convert.ToString(view.Table.Rows[i]["_Year"]),
                    });

                }

            }

            return docs;

        }

    }
    #endregion

    #region GetDocument
    public static DocObj GetDocument(Guid id) {

        using (AccessDataSource dataSource = new AccessDataSource("~/App_Data/Reader.mdb", "")) {

            dataSource.SelectCommand = String.Format(@"
            
                SELECT * 
                                
                        FROM [Documents]

                WHERE [Documents].[Id] = {{guid {0}}}", id.ToString("B"));

            dataSource.SelectParameters.Add("Id", DbType.Guid, id.ToString());

            DataView view = (DataView)dataSource.Select(DataSourceSelectArguments.Empty);

            if (view.Count > 0) {

                return new DocObj() {

                    Id = (Guid)(view.Table.Rows[0]["Id"]),
                    Author = Convert.ToString(view.Table.Rows[0]["Author"]),
                    Book = Convert.ToString(view.Table.Rows[0]["Book"]),
                    Content = Convert.ToString(view.Table.Rows[0]["Body"]),
                    Title = Convert.ToString(view.Table.Rows[0]["Title"]),
                    Chapter = Convert.ToString(view.Table.Rows[0]["Chapter"]),
                    Year = Convert.ToString(view.Table.Rows[0]["_Year"]),

                };

            }

        }

        return null;

    }
    #endregion

    #region SaveDocument
    public static void SaveDocument(DocObj doc) {

        using (AccessDataSource dataSource = new AccessDataSource("~/App_Data/Reader.mdb", "")) {

            dataSource.UpdateCommand = String.Format(@"

                   UPDATE 
                        [Documents]
                       
                   SET 
                        [Documents].[Author] = @Author,
                        [Documents].[Title] = @Title,
                        [Documents].[Chapter] = @Chapter,
                        [Documents].[Body] = @Body,
                        [Documents].[Book] = @Book,
                        [Documents].[_Year] = @_Year
                
                    WHERE [Documents].[Id] = {{guid {0}}} 

                ", doc.Id.ToString("B"));

            dataSource.UpdateParameters.Add("Author", DbType.String, doc.Author);
            dataSource.UpdateParameters.Add("Title", DbType.String, doc.Title);
            dataSource.UpdateParameters.Add("Chapter", DbType.String, doc.Chapter);
            dataSource.UpdateParameters.Add("Body", DbType.String, doc.Content);
            dataSource.UpdateParameters.Add("Book", DbType.String, doc.Book);
            dataSource.UpdateParameters.Add("_Year", DbType.String, doc.Year);

            dataSource.Update();

        }

    }
    #endregion

    #region InsertDocument
    public static DocObj InsertDocument(DocObj docObj) {
        using (AccessDataSource dataSource = new AccessDataSource("~/App_Data/Reader.mdb", "")) {

            Guid id = Guid.NewGuid();

            dataSource.InsertCommand = String.Format(@"

                            INSERT INTO [Documents] (  
                                [Id],
                                [Author],
                                [Title],
                                [Chapter],
                                [Body],
                                [Book],
                                [_Year])

                            VALUES
                                ({{guid {0}}},
                                 @Author,
                                 @Title,
                                 @Chapter,
                                 @Body,
                                 @Book,
                                 @_Year)


                    ", id.ToString("B"));

            dataSource.InsertParameters.Add("Author", DbType.String, docObj.Author);
            dataSource.InsertParameters.Add("Title", DbType.String, docObj.Title);
            dataSource.InsertParameters.Add("Chapter", DbType.String, docObj.Chapter);
            dataSource.InsertParameters.Add("Body", DbType.String, docObj.Content);
            dataSource.InsertParameters.Add("Book", DbType.String, docObj.Book);
            dataSource.InsertParameters.Add("_Year", DbType.String, docObj.Year);

            dataSource.Insert();

            return new DocObj() {

                Id = id,
                Author = docObj.Author,
                Book = docObj.Book,
                Title = docObj.Title,
                Chapter = docObj.Chapter,
                Content = docObj.Content,
                Year = docObj.Year,

            };

        }
    }
    #endregion

}