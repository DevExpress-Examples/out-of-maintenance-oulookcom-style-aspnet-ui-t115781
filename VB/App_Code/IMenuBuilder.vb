Imports Microsoft.VisualBasic
Imports DevExpress.Web.ASPxMenu
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls

Public Interface IMenuBuilder
	Sub BuildMenu(ByVal menu As ASPxMenu)
End Interface