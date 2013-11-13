Public Interface IRegion    
    Function getRegions(ByVal constr As String, ByVal zone As String) As List(Of String)
End Interface

Public Interface IArea
    Inherits IRegion
    Function getArea(ByVal zone As String, ByVal constr As String, ByVal Region As String) As List(Of String)
End Interface

Public Interface IBranch
    Inherits IArea
    Function getBranches(ByVal Zone As String, ByVal Region As String, ByVal Area As String, ByVal Connection As String) As List(Of String)
    Function getBranchEmployee(ByVal Zone As String, ByVal Branch As String, ByVal Connection As String) As List(Of String)
    Sub getAllBrachEmployeeReport(ByVal Bc_Name As String, ByVal Connection As String)
    Sub getBranchEmployeeRerpot(ByVal Bc_Name As String, ByVal Bc_Emp As String, ByVal Connection As String)
End Interface

Friend Interface IRegionRpt
    Function getRegionalManager(ByVal Connection As String, ByVal Zone As String, ByVal Region As String) As String
    Sub GenerateRegionRpt(ByVal Region As String, ByVal Manager As String, ByVal Connection As String, ByVal Zone As String)
End Interface

Friend Interface IAreaRpt
    Function getAreaManager(ByVal Zone As String, ByVal Area As String, ByVal MsConstr As String) As String
    Sub GenerateAreaRpt(ByVal Region As String, ByVal Area As String, ByVal Zone As String, ByVal Connection As String, Optional ByVal AreaMan As String = Nothing)
End Interface

Friend Interface IDivision
    Function getDivisions(ByVal Zone As String, ByVal Connection As String) As List(Of String)
    Function DivisionManager(ByVal Division As String, ByVal Zone As String, ByVal Connection As String) As String
    Function AllDivManRpt(ByVal Connection As String) As CrystalDecisions.CrystalReports.Engine.ReportDocument
    Function ByDivManRpt(ByVal Connection As String, ByVal Division As String, ByVal DivisionManager As String) As CrystalDecisions.CrystalReports.Engine.ReportDocument
    Sub CheckAttachment(ByVal AssetNo As String, ByVal AAssetNo As String, ByVal Connection As String)
End Interface