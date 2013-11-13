Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports Microsoft.VisualBasic

Public Delegate Sub SearchDelegate(ByVal TxtToFind As String)
Public Delegate Sub JobTitleDelegate(ByVal Code As String, ByVal Description As String)
Public Delegate Sub DivisionDelegate(ByVal Code As String, ByVal Name As String)
Public Delegate Sub BranchDelegate(ByVal Code As String, ByVal Name As String)
Public Delegate Sub UsersIDDelegate(ByVal userID As String, ByVal Name As String)
Public Delegate Sub UsersIDDDelegate(ByVal userID As String, ByVal Name As String)
Public Delegate Sub UserDelegate(ByVal userID As String)
Public Delegate Sub RMDelegate(ByVal userID As String)
Public Delegate Sub DivDesigDelegate(ByVal Name As String)
Public Delegate Sub DivDesigManDelegate(ByVal Name As String)
Public Delegate Sub RegDesigLPTLDelegate(ByVal Name As String)
Public Delegate Sub LPTDesigDelegate(ByVal Name As String)
Public Delegate Sub RCTDesigDelegate(ByVal name As String)
Public Delegate Sub AttachFile(ByVal A1 As String, ByVal A2 As String, ByVal A3 As String, ByVal A4 As String, ByVal A5 As String)

