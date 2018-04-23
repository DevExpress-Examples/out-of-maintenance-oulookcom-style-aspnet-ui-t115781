Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls

Partial Public Class Document
	Inherits System.Web.UI.Page
	Private privateBody As String
	Public Property Body() As String
		Get
			Return privateBody
		End Get
		Set(ByVal value As String)
			privateBody = value
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

	Private privateDocumentTitle As String
	Public Property DocumentTitle() As String
		Get
			Return privateDocumentTitle
		End Get
		Set(ByVal value As String)
			privateDocumentTitle = value
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

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)

		Body = String.Empty
		DocumentTitle = String.Empty
		Author = String.Empty
		Chapter = String.Empty

		Dim id As Guid

		If Guid.TryParse(Request.Params("Id"), id) Then

			Dim document As DocObj = WebDbProvider.GetDocument(id)

			If document IsNot Nothing Then

				Author = document.Author
				Body = document.Content
				DocumentTitle = document.Title
				Chapter = document.Chapter

			End If

		End If

		If String.IsNullOrWhiteSpace(Title) Then
			If (Not String.IsNullOrWhiteSpace(Body)) Then
				Title = "(No Title)"
			End If
		End If

	End Sub
End Class