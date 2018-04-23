Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Collections.Specialized
Imports System.Linq
Imports System.Web
Imports System.Web.SessionState

Public NotInheritable Class CallbackParams
	Private Sub New()
	End Sub
	Public Shared Function [Get](ByVal request As HttpRequest) As NameValueCollection
		Dim __CALLBACKPARAM As String = Convert.ToString(request.Form("__CALLBACKPARAM"))

		If __CALLBACKPARAM IsNot Nothing AndAlso __CALLBACKPARAM.IndexOf("http://tempuri.org/?") > 0 Then

			If __CALLBACKPARAM.EndsWith(";") Then
				__CALLBACKPARAM = __CALLBACKPARAM.Remove(__CALLBACKPARAM.Length - 1)
			End If

			__CALLBACKPARAM = __CALLBACKPARAM.Substring(__CALLBACKPARAM.IndexOf("http://tempuri.org/?"))

			Dim parameters As NameValueCollection = System.Web.HttpUtility.ParseQueryString(New Uri(__CALLBACKPARAM).Query)

			Return parameters

		End If

		Return Nothing
	End Function
End Class

Public NotInheritable Class SqlParams
	Private Sub New()
	End Sub
	Public Shared Function [Get](ByVal parameters As NameValueCollection, ByVal session As HttpSessionState) As NameValueCollection

		If parameters IsNot Nothing Then

			Dim sort As String = parameters("Sort")
			If String.IsNullOrWhiteSpace(sort) Then
				sort = Convert.ToString(session("Sort"))
			End If

			If String.Equals(sort, "title", StringComparison.InvariantCultureIgnoreCase) Then
				sort = "Title"
			Else
				sort = "Chapter"
			End If

			parameters("Sort") = sort

			Dim mode As String = parameters("Mode")
			If String.IsNullOrWhiteSpace(mode) Then
				mode = Convert.ToString(session("Mode"))
			End If

			If String.Equals(mode, "desc", StringComparison.InvariantCultureIgnoreCase) Then
				mode = "DESC"
			Else
				mode = "ASC"
			End If

			parameters("Mode") = mode

		End If

		Return parameters
	End Function
End Class