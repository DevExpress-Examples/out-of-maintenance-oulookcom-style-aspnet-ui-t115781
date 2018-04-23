Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports DevExpress.Web.ASPxClasses

	Partial Public Class RootMaster
		Inherits System.Web.UI.MasterPage
		Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
			Dim menuBuilder As IMenuBuilder = TryCast(Me.MainContentPlaceHolder.Page, IMenuBuilder)
			If menuBuilder IsNot Nothing Then
				menuBuilder.BuildMenu(MainMenu)
			End If
		End Sub
	End Class