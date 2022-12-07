Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.OleDb
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls

Partial Public Class Edit
	Inherits System.Web.UI.Page
	Implements IMenuBuilder
	Private Sub Insert()

		Dim docObj As New DocObj()
		docObj.Author = CType(Request.Form("_H_AUTHOR"), String)
		docObj.Title = CType(Request.Form("_H_TITLE"), String)
		docObj.Chapter = CType(Request.Form("_H_CHAPTER"), String)
		docObj.Content = CType(Request.Form("_H_HTML"), String)
		docObj.Book = CType(Request.Form("_H_BOOK"), String)
		docObj.Year = CType(Request.Form("_H_YEAR"), String)

		Dim docInserted As DocObj = WebDbProvider.InsertDocument(docObj)

		Session("Author") = docInserted.Author
		Session("Book") = docInserted.Book
		Session("Id") = docInserted.Id
	End Sub

	Private Sub Update(ByVal id As Guid)

		Dim docObj As New DocObj()
		docObj.Id = id
		docObj.Book = CType(Request.Form("_H_BOOK"), String)
		docObj.Author = CType(Request.Form("_H_AUTHOR"), String)
		docObj.Chapter = CType(Request.Form("_H_CHAPTER"), String)
		docObj.Title = CType(Request.Form("_H_TITLE"), String)
		docObj.Content = CType(Request.Form("_H_HTML"), String)
		docObj.Year = CType(Request.Form("_H_YEAR"), String)

		WebDbProvider.SaveDocument(docObj)

		Session("Author") = docObj.Author
		Session("Book") = docObj.Book
		Session("Id") = id


	End Sub

	Private Sub [Select](ByVal id As Guid)

		Dim view As DocObj = WebDbProvider.GetDocument(id)

		If view IsNot Nothing Then

			ASPxHtmlEditor.Html = view.Content
			ASPxComboBox_Author.Value = view.Author
			ASPxTextBox_Chapter.Value = view.Chapter
			ASPxTextBox_Title.Value = view.Title
			ASPxTextBox_Book.Value = view.Book
			ASPxTextBox_Year.Value = view.Year

			Title = String.Format("{0}: {1}", ASPxTextBox_Book.Value, ASPxTextBox_Title.Value)

		End If

	End Sub

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
		ASPxComboBox_Author.Items.Clear()
		For Each i As Author In WebDbProvider.Authors
			ASPxComboBox_Author.Items.Add(i.Id)
		Next i

		Dim id As Guid = Guid.Empty

		If (Not Guid.TryParse(Request.Params("Id"), id)) Then
			id = Guid.Empty
		End If

		Session("Id") = id

		If IsPostBack Then

#If ALLOW_SAVE Then

			If id = Guid.Empty AndAlso (String.IsNullOrWhiteSpace(Request.Params("Id")) OrElse Request.Params("Id") = "-1") Then

				Insert()

			ElseIf id <> Guid.Empty Then

				Update(id)

			End If

			Response.Redirect(String.Format("Default.aspx?Author={0}&Book={1}&Id={2}", CType(Session("Author"), String), CType(Session("Book"), String), (CType(Session("Id"), Guid)).ToString("N")), True)
#End If

			Return

		Else

			If id = Guid.Empty Then

				Dim author As String = TryCast(Session("Author"), String)

				Dim index As Integer = ASPxComboBox_Author.Items.IndexOfText(author)
				If index >= 0 Then

					ASPxComboBox_Author.Value = author

				Else

					ASPxComboBox_Author.Value = String.Empty

				End If

			Else

				[Select](id)

			End If

		End If

	End Sub




	Private Sub BuildMenu(ByVal menu As DevExpress.Web.ASPxMenu) Implements IMenuBuilder.BuildMenu
		If (Not IsPostBack) AndAlso (Not IsCallback) Then

			menu.Items.Add("Save", "CMD_SAVE")
			menu.Items.Add("Cancel", "CMD_CANCEL")

		End If
	End Sub
End Class