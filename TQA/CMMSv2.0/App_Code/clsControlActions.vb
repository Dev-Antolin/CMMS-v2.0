Imports Microsoft.VisualBasic
Imports System.Web.HttpContext

Public Class clsControlActions

    Public Property Action() As String
        Get
            Return Current.Session("clickAction")
        End Get
        Set(ByVal value As String)
            Current.Session("clickAction") = value
        End Set
    End Property

End Class
