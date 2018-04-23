Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Collections.Specialized
Imports System.Data
Imports System.Linq
Imports System.Text
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls

Partial Public Class _Default
	Inherits System.Web.UI.Page
	Implements IMenuBuilder
	Private privateAuthor As String
	Public Property Author() As String
		Get
			Return privateAuthor
		End Get
		Set(ByVal value As String)
			privateAuthor = value
		End Set
	End Property

	Private Sub BuildMenu(ByVal menu As DevExpress.Web.ASPxMenu.ASPxMenu) Implements IMenuBuilder.BuildMenu
		If (Not IsPostBack) AndAlso (Not IsCallback) Then
			menu.Items.Add("New", "CMD_NEW")
			menu.Items.Add("Edit", "CMD_EDIT")
		End If
	End Sub

	Private Sub CreateFolders(ByVal author As String)

		NavBar.Groups.Clear()

		If String.IsNullOrWhiteSpace(author) Then
			Return
		End If

		NavBar.Groups.Add(author)

		NavBar.Groups(0).Items.Clear()
		NavBar.Groups(0).Text = author

		Dim books As IList(Of Book) = WebDbProvider.GetBooks(author)

		For Each b As Book In books

			 NavBar.Groups(0).Items.Add(b.Name)
		Next b

		If NavBar.Groups(0).Items.Count > 0 Then
			NavBar.Groups(0).Items(0).Selected = True
		End If

	End Sub

	Private Sub BindDataGrid(ByVal author As String, ByVal book As String, ByVal sort As String, ByVal mode As String)
		Me.GridControl.DataSourceID = ""
		Me.GridControl.DataSource = WebDbProvider.GetDocuments(author, book, sort, mode)
		Me.GridControl.DataBind()
	End Sub

	Private Sub CheckMenuItemByName(ByVal name As String, ByVal bChecked As Boolean)
		Dim menuitem = SortMenu.Items.FindByName(name)
		If menuitem IsNot Nothing Then
			menuitem.Checked = bChecked
		End If
	End Sub

	Private Sub PrepareMenuItems()
		If String.Equals(CType(Session("Sort"), String), "Title", StringComparison.InvariantCultureIgnoreCase) Then

			CheckMenuItemByName("CMD_SORT_BY_TITLE", True)
			CheckMenuItemByName("CMD_SORT_BY_CHAPTER", False)

			Session("Sort") = "Title"

		Else

			CheckMenuItemByName("CMD_SORT_BY_TITLE", False)
			CheckMenuItemByName("CMD_SORT_BY_CHAPTER", True)

			Session("Sort") = "Chapter"

		End If

		If String.Equals(CType(Session("Mode"), String), "DESC", StringComparison.InvariantCultureIgnoreCase) Then

			CheckMenuItemByName("CMD_SORT_ASC", False)
			CheckMenuItemByName("CMD_SORT_DESC", True)

			Session("Mode") = "DESC"

		Else

			CheckMenuItemByName("CMD_SORT_ASC", True)
			CheckMenuItemByName("CMD_SORT_DESC", False)

			Session("Mode") = "ASC"

		End If
	End Sub

	Private Sub BuildNavBar()
		CreateFolders(CType(Session("Author"), String))

		Dim book As String = TryCast(Session("Book"), String)

		Dim index As Integer = 0

		If (Not String.IsNullOrWhiteSpace(book)) Then
			index = NavBar.Groups(0).Items.IndexOfText(book)
			If index <= 0 Then
				index = 0
			End If
		End If

		If NavBar.Groups.Count > 0 AndAlso NavBar.Groups(0).Items.Count > 0 Then
			NavBar.SelectedItem = NavBar.Groups(0).Items(index)
			Session("Book") = NavBar.Groups(0).Items(index).Text
		End If
	End Sub

	Private Function GetGuid(ByVal id As String) As Guid
		Dim g As Guid
		If Guid.TryParse(id, g) Then
			Return g
		End If
		Return Guid.Empty
	End Function

	Private Function OnFirstLoad() As Boolean
		Dim author As String = TryCast(Request.Params("Author"), String)

		If (Not String.IsNullOrWhiteSpace(author)) Then

			Session("Author") = author
			Session("Book") = TryCast(Request.Params("Book"), String)
			Session("Id") = GetGuid(Request.Params("Id"))
			Author = CType(Session("Author"), String)

			Response.Redirect("Default.aspx", True)

			Return True

		End If

		If String.IsNullOrWhiteSpace(TryCast(Session("Author"), String)) Then

			Session("Author") = "Shakespeare"
			Session("Book") = Nothing
			Author = CType(Session("Author"), String)

		End If

		BuildNavBar()

		Return False
	End Function

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)

		If (Not IsPostBack) AndAlso (Not IsCallback) Then
			If OnFirstLoad() Then
				Return
			End If
		End If

		If GridControl.IsCallback OrElse NavBarCallback.IsCallback Then
			Dim parameters As NameValueCollection = SqlParams.Get(CallbackParams.Get(Request), Session)
			If parameters IsNot Nothing Then
				Session("Author") = parameters("Author")
				Session("Book") = parameters("Book")
				Session("Sort") = parameters("Sort")
				Session("Mode") = parameters("Mode")
				Session("Id") = GetGuid(parameters("Id"))

				If NavBarCallback.IsCallback Then
					BuildNavBar()
				End If
			End If
		End If

		Author = CType(Session("Author"), String)

		PrepareMenuItems()

		BindDataGrid(CType(Session("Author"), String), CType(Session("Book"), String), CType(Session("Sort"), String), CType(Session("Mode"), String))

		If TypeOf Session("Id") Is Guid Then

			Dim FocusedRowIndex As Integer = GridControl.FindVisibleIndexByKeyValue(Session("Id"))

			If FocusedRowIndex < 0 Then
				FocusedRowIndex = 0
			End If

			GridControl.FocusedRowIndex = FocusedRowIndex

			Session("Id") = Nothing

		End If

	End Sub

End Class