Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Web
Imports System.Web.Caching
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Linq

Public Class Author
	Private privateId As String
	Public Property Id() As String
		Get
			Return privateId
		End Get
		Set(ByVal value As String)
			privateId = value
		End Set
	End Property
	Private privateDisplay As String
	Public Property Display() As String
		Get
			Return privateDisplay
		End Get
		Set(ByVal value As String)
			privateDisplay = value
		End Set
	End Property
	Private privateFooter As String
	Public Property Footer() As String
		Get
			Return privateFooter
		End Get
		Set(ByVal value As String)
			privateFooter = value
		End Set
	End Property
	Private privateDesciption As String
	Public Property Desciption() As String
		Get
			Return privateDesciption
		End Get
		Set(ByVal value As String)
			privateDesciption = value
		End Set
	End Property
End Class

Public Class Book
	Private privateName As String
	Public Property Name() As String
		Get
			Return privateName
		End Get
		Set(ByVal value As String)
			privateName = value
		End Set
	End Property
End Class

Public Class DocObj
	Private privateId As Guid
	Public Property Id() As Guid
		Get
			Return privateId
		End Get
		Set(ByVal value As Guid)
			privateId = value
		End Set
	End Property
	Private privateAuthor As String
	Public Property Author() As String
		Get
			Return privateAuthor
		End Get
		Set(ByVal value As String)
			privateAuthor = value
		End Set
	End Property
	Private privateBook As String
	Public Property Book() As String
		Get
			Return privateBook
		End Get
		Set(ByVal value As String)
			privateBook = value
		End Set
	End Property
	Private privateTitle As String
	Public Property Title() As String
		Get
			Return privateTitle
		End Get
		Set(ByVal value As String)
			privateTitle = value
		End Set
	End Property
	Private privateChapter As String
	Public Property Chapter() As String
		Get
			Return privateChapter
		End Get
		Set(ByVal value As String)
			privateChapter = value
		End Set
	End Property
	Private privateContent As String
	Public Property Content() As String
		Get
			Return privateContent
		End Get
		Set(ByVal value As String)
			privateContent = value
		End Set
	End Property
	Private privateYear As String
	Public Property Year() As String
		Get
			Return privateYear
		End Get
		Set(ByVal value As String)
			privateYear = value
		End Set
	End Property
End Class

Public Class MemCache
	Public Shared Sub Remove(ByVal id As Guid)
		Dim cache As Cache = HttpRuntime.Cache
		If cache Is Nothing Then
			Return
		End If
		cache.Remove(id.ToString("N"))
	End Sub
	Public Shared Function Cache(ByVal key As String) As Object
'INSTANT VB NOTE: The local variable cache was renamed since Visual Basic will not allow local variables with the same name as their enclosing function or property:
		Dim cache_Renamed As Cache = HttpRuntime.Cache
		If cache_Renamed Is Nothing Then
			Return Nothing
		End If
		Dim r As Object = cache_Renamed.Get(key)
		Return r
	End Function
	Public Shared Sub Cache(ByVal key As String, ByVal obj As Object)
		Dim cache As Cache = HttpRuntime.Cache
		If cache Is Nothing Then
			Return
		End If
		Const minutes As Integer = &HFF
		If obj Is Nothing Then
			cache.Remove(key)
		Else
			cache.Insert(key, obj, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(minutes))
		End If
	End Sub
End Class

Public Class WebDbProvider
	Public Shared Authors() As Author = { New Author() With {.Id = "Shakespeare", .Display = "Shakespeare, William", .Footer = "16th century", .Desciption = "William Shakespeare was an English poet, playwright and actor, widely regarded as the greatest writer in the English language."}, New Author() With {.Id = "Poe", .Display = "Poe, Edgar Allan", .Footer = "19th century", .Desciption = "Edgar Allan Poe was an American author, poet, editor, and literary critic, considered part of the American Romantic Movement."}, New Author() With {.Id = "Dickinson", .Display = "Dickinson, Emily", .Footer = "19th century", .Desciption = "Emily Elizabeth Dickinson was an American poet. Born in Amherst, Massachusetts. She lived a mostly introverted and reclusive life."} }

	Public Shared Function GetAuthorDescription(ByVal author As String) As String
		For Each a As Author In Authors
			If String.Equals(a.Id, author, StringComparison.InvariantCultureIgnoreCase) Then
				Return a.Desciption
			End If
		Next a
		Return String.Empty
	End Function

	Public Shared Function GetBooks(ByVal author As String) As IList(Of Book)

#If TRACE Then
			Trace.WriteLine(String.Format("GetBooks({0})", author))

#End If
		Dim key As String = String.Format("GetBooks(""{0}"")", author)

		Dim obj As Object = MemCache.Cache(key)

		If TypeOf obj Is IList(Of Book) Then
			Return CType(obj, IList(Of Book))
		End If

		obj = _DbProvider.GetBooks(author)

		MemCache.Cache(key, obj)

		Return CType(obj, IList(Of Book))
	End Function

	 Public Shared Function GetDocuments(ByVal author As String, ByVal book As String, ByVal sort As String, ByVal mode As String) As IList(Of DocObj)

#If TRACE Then
			Trace.WriteLine(String.Format("GetDocuments({0}, {1})", author, book))

#End If

		Dim key As String = String.Format("GetDocuments(""{0}"", ""{1}"")", author, book)

		Dim obj As Object = MemCache.Cache(key)

		If Not(TypeOf obj Is IList(Of DocObj)) Then

			obj = _DbProvider.GetDocuments(author, book, sort, mode)

			MemCache.Cache(key, obj)

		End If

		If Not(String.Equals(sort, "Title", StringComparison.InvariantCultureIgnoreCase) OrElse String.Equals(sort, "Chapter", StringComparison.InvariantCultureIgnoreCase)) Then

			sort = "Chapter"
		End If

		If Not(String.Equals(mode, "Asc", StringComparison.InvariantCultureIgnoreCase) OrElse String.Equals(mode, "Desc", StringComparison.InvariantCultureIgnoreCase)) Then

			mode = "ASC"
		End If


		If String.Equals(sort, "Title", StringComparison.InvariantCultureIgnoreCase) Then
			If String.Equals(mode, "ASC", StringComparison.InvariantCultureIgnoreCase) Then
				Return (CType(obj, IList(Of DocObj))).OrderBy(Function(k) k.Title).ToList()
			Else
				Return (CType(obj, IList(Of DocObj))).OrderByDescending(Function(k) k.Title).ToList()
			End If
		Else
			If String.Equals(mode, "ASC", StringComparison.InvariantCultureIgnoreCase) Then
				Return (CType(obj, IList(Of DocObj))).OrderBy(Function(k) k.Chapter).ToList()
			Else
				Return (CType(obj, IList(Of DocObj))).OrderByDescending(Function(k) k.Chapter).ToList()
			End If
		End If

	 End Function

	 Public Shared Function GetDocument(ByVal id As Guid) As DocObj

#If TRACE Then
		 Trace.WriteLine(String.Format("GetDocuments({0})", id))
#End If

		 Dim key As String = String.Format("GetDocuments(""{0}"")", id)

		 Dim obj As Object = MemCache.Cache(key)

		 If TypeOf obj Is DocObj Then

			 Return CType(obj, DocObj)

		 End If

		 obj = _DbProvider.GetDocument(id)

		 MemCache.Cache(key, obj)

		 Return CType(obj, DocObj)

	 End Function

	 Public Shared Sub SaveDocument(ByVal doc As DocObj)

		 Dim key As String = String.Format("GetDocuments(""{0}"")", doc.Id)

		 _DbProvider.SaveDocument(doc)

		 MemCache.Cache(key, doc)

		 MemCache.Cache(String.Format("GetDocuments(""{0}"", ""{1}"")", doc.Author, doc.Book), Nothing)

	 End Sub

	 Public Shared Function InsertDocument(ByVal doc As DocObj) As DocObj

		 Dim inserted As DocObj = _DbProvider.InsertDocument(doc)

		 Dim key As String = String.Format("GetDocuments(""{0}"")", inserted.Id)

		 MemCache.Cache(key, inserted)

		 MemCache.Cache(String.Format("GetDocuments(""{0}"", ""{1}"")", doc.Author, doc.Book), Nothing)

		 Return inserted

	 End Function
End Class

Public Class _DbProvider
	Public Sub New()
	End Sub

	#Region "GetBooks"
	Public Shared Function GetBooks(ByVal author As String) As IList(Of Book)
		If String.IsNullOrWhiteSpace(author) Then
			Return New List(Of Book)()
		End If

		author = author.Trim()

		Using dataSource As New AccessDataSource("~/App_Data/Reader.mdb", String.Format("" & ControlChars.CrLf & ControlChars.CrLf & "            SELECT DISTINCT [Book] FROM Documents " & ControlChars.CrLf & ControlChars.CrLf & "    WHERE [Author] = @Author" & ControlChars.CrLf & ControlChars.CrLf & "    ORDER BY [Book] ASC" & ControlChars.CrLf & ControlChars.CrLf & "", author))

			dataSource.SelectParameters.Add("Author", DbType.String, author)

			Dim bHasOther As Boolean = False
			Dim List As New List(Of Book)()

			Dim view As DataView = CType(dataSource.Select(DataSourceSelectArguments.Empty), DataView)

			If view IsNot Nothing Then

				For i As Integer = 0 To view.Count - 1
					Dim title As String = Convert.ToString(view.Table.Rows(i)("Book"))
					If (Not String.IsNullOrWhiteSpace(title)) AndAlso title <> "Other" Then
						List.Add(New Book() With {.Name = title})
					Else
						bHasOther = True
					End If
				Next i

			End If

			If bHasOther Then
				If List.Count > 0 Then

					List.Add(New Book() With {.Name = "Other"})
				Else

					List.Add(New Book() With {.Name = "All"})
				End If
			Else
				If List.Count <= 0 Then

					List.Add(New Book() With {.Name = "All"})
				End If
			End If

			Return List

		End Using

	End Function
	#End Region

	#Region "GetDocuments"
	Public Shared Function GetDocuments(ByVal author As String, ByVal book As String, ByVal sort As String, ByVal mode As String) As IList(Of DocObj)
		If String.IsNullOrWhiteSpace(book) OrElse String.Equals(book, "Other", StringComparison.InvariantCultureIgnoreCase) Then

			book = "Other"
		End If

		If Not(String.Equals(sort, "Title", StringComparison.InvariantCultureIgnoreCase) OrElse String.Equals(sort, "Chapter", StringComparison.InvariantCultureIgnoreCase)) Then

			sort = "Chapter"
		End If

		If Not(String.Equals(mode, "Asc", StringComparison.InvariantCultureIgnoreCase) OrElse String.Equals(mode, "Desc", StringComparison.InvariantCultureIgnoreCase)) Then

			mode = "ASC"
		End If

		Dim ORDER As String = String.Format("ORDER BY [{0}] {1}", sort, mode)

		Using dataSource As New AccessDataSource("~/App_Data/Reader.mdb", String.Format("SELECT" & ControlChars.CrLf & ControlChars.CrLf & " [Id], [Book], [Author], [Title], [Chapter], [_Year] FROM Documents" & ControlChars.CrLf & ControlChars.CrLf & "WHERE [Author] = @Author AND " & ControlChars.CrLf & "(" & ControlChars.CrLf & "                ([Book] = @Book) OR" & ControlChars.CrLf & "                (      " & ControlChars.CrLf & "                    " & ControlChars.CrLf & "                        (@Book = 'Other' And ([Book] is null or [Book] = 'Other' or [Book] = '') ) " & ControlChars.CrLf & "                ) OR" & ControlChars.CrLf & "                (      " & ControlChars.CrLf & "                    " & ControlChars.CrLf & "                        (@Book = 'All') " & ControlChars.CrLf & "                )" & ControlChars.CrLf & ")" & ControlChars.CrLf & ControlChars.CrLf & "{0}", ORDER))

			Dim docs As New List(Of DocObj)()

			dataSource.SelectParameters.Add("Author", DbType.String, author)
			dataSource.SelectParameters.Add("Book", DbType.String, book)

			Dim view As DataView = TryCast(dataSource.Select(DataSourceSelectArguments.Empty), DataView)

			If view IsNot Nothing Then

				For i As Integer = 0 To view.Count - 1

					docs.Add(New DocObj() With {.Id = CType(view.Table.Rows(i)("Id"), Guid), .Author = Convert.ToString(view.Table.Rows(i)("Author")), .Book = Convert.ToString(view.Table.Rows(i)("Book")), .Title = Convert.ToString(view.Table.Rows(i)("Title")), .Chapter = Convert.ToString(view.Table.Rows(i)("Chapter")), .Year = Convert.ToString(view.Table.Rows(i)("_Year"))})

				Next i

			End If

			Return docs

		End Using

	End Function
	#End Region

	#Region "GetDocument"
	Public Shared Function GetDocument(ByVal id As Guid) As DocObj

		Using dataSource As New AccessDataSource("~/App_Data/Reader.mdb", "")

			dataSource.SelectCommand = String.Format("" & ControlChars.CrLf & "            " & ControlChars.CrLf & "                SELECT * " & ControlChars.CrLf & "                                " & ControlChars.CrLf & "                        FROM [Documents]" & ControlChars.CrLf & ControlChars.CrLf & "                WHERE [Documents].[Id] = {{guid {0}}}", id.ToString("B"))

			dataSource.SelectParameters.Add("Id", DbType.Guid, id.ToString())

			Dim view As DataView = CType(dataSource.Select(DataSourceSelectArguments.Empty), DataView)

			If view.Count > 0 Then

				Return New DocObj() With {.Id = CType(view.Table.Rows(0)("Id"), Guid), .Author = Convert.ToString(view.Table.Rows(0)("Author")), .Book = Convert.ToString(view.Table.Rows(0)("Book")), .Content = Convert.ToString(view.Table.Rows(0)("Body")), .Title = Convert.ToString(view.Table.Rows(0)("Title")), .Chapter = Convert.ToString(view.Table.Rows(0)("Chapter")), .Year = Convert.ToString(view.Table.Rows(0)("_Year"))}

			End If

		End Using

		Return Nothing

	End Function
	#End Region

	#Region "SaveDocument"
	Public Shared Sub SaveDocument(ByVal doc As DocObj)

		Using dataSource As New AccessDataSource("~/App_Data/Reader.mdb", "")

			dataSource.UpdateCommand = String.Format("" & ControlChars.CrLf & ControlChars.CrLf & "                   UPDATE " & ControlChars.CrLf & "                        [Documents]" & ControlChars.CrLf & "                       " & ControlChars.CrLf & "                   SET " & ControlChars.CrLf & "                        [Documents].[Author] = @Author," & ControlChars.CrLf & "                        [Documents].[Title] = @Title," & ControlChars.CrLf & "                        [Documents].[Chapter] = @Chapter," & ControlChars.CrLf & "                        [Documents].[Body] = @Body," & ControlChars.CrLf & "                        [Documents].[Book] = @Book," & ControlChars.CrLf & "                        [Documents].[_Year] = @_Year" & ControlChars.CrLf & "                " & ControlChars.CrLf & "                    WHERE [Documents].[Id] = {{guid {0}}} " & ControlChars.CrLf & ControlChars.CrLf & "                ", doc.Id.ToString("B"))

			dataSource.UpdateParameters.Add("Author", DbType.String, doc.Author)
			dataSource.UpdateParameters.Add("Title", DbType.String, doc.Title)
			dataSource.UpdateParameters.Add("Chapter", DbType.String, doc.Chapter)
			dataSource.UpdateParameters.Add("Body", DbType.String, doc.Content)
			dataSource.UpdateParameters.Add("Book", DbType.String, doc.Book)
			dataSource.UpdateParameters.Add("_Year", DbType.String, doc.Year)

			dataSource.Update()

		End Using

	End Sub
	#End Region

	#Region "InsertDocument"
	Public Shared Function InsertDocument(ByVal docObj As DocObj) As DocObj
		Using dataSource As New AccessDataSource("~/App_Data/Reader.mdb", "")

			Dim id As Guid = Guid.NewGuid()

			dataSource.InsertCommand = String.Format("" & ControlChars.CrLf & ControlChars.CrLf & "                            INSERT INTO [Documents] (  " & ControlChars.CrLf & "                                [Id]," & ControlChars.CrLf & "                                [Author]," & ControlChars.CrLf & "                                [Title]," & ControlChars.CrLf & "                                [Chapter]," & ControlChars.CrLf & "                                [Body]," & ControlChars.CrLf & "                                [Book]," & ControlChars.CrLf & "                                [_Year])" & ControlChars.CrLf & ControlChars.CrLf & "                            VALUES" & ControlChars.CrLf & "                                ({{guid {0}}}," & ControlChars.CrLf & "                                 @Author," & ControlChars.CrLf & "                                 @Title," & ControlChars.CrLf & "                                 @Chapter," & ControlChars.CrLf & "                                 @Body," & ControlChars.CrLf & "                                 @Book," & ControlChars.CrLf & "                                 @_Year)" & ControlChars.CrLf & ControlChars.CrLf & ControlChars.CrLf & "                    ", id.ToString("B"))

			dataSource.InsertParameters.Add("Author", DbType.String, docObj.Author)
			dataSource.InsertParameters.Add("Title", DbType.String, docObj.Title)
			dataSource.InsertParameters.Add("Chapter", DbType.String, docObj.Chapter)
			dataSource.InsertParameters.Add("Body", DbType.String, docObj.Content)
			dataSource.InsertParameters.Add("Book", DbType.String, docObj.Book)
			dataSource.InsertParameters.Add("_Year", DbType.String, docObj.Year)

			dataSource.Insert()

			Return New DocObj() With {.Id = id, .Author = docObj.Author, .Book = docObj.Book, .Title = docObj.Title, .Chapter = docObj.Chapter, .Content = docObj.Content, .Year = docObj.Year}

		End Using
	End Function
	#End Region

End Class