Imports Microsoft.VisualBasic
Imports MySql.Data.MySqlClient

Public Class Report
    Public Sub New()
        MyBase.New()
    End Sub

    Private mDateFrom As Date
    Private mDateTo As Date
    Private mCurrency As String
    Private mCurrDesc As String
    Private mClassif As Integer
    Private mParticulars As String
    Private mDesc As String


    Public Property DateFrom() As Date
        Get
            Return mDateFrom
        End Get
        Set(ByVal value As Date)
            mDateFrom = value
        End Set
    End Property

    Public Property DateTo() As Date
        Get
            Return mDateTo
        End Get
        Set(ByVal value As Date)
            mDateTo = value
        End Set
    End Property

    Public Property Currency() As String
        Get
            Return mCurrency
        End Get
        Set(ByVal value As String)
            mCurrency = value
        End Set
    End Property

    Public Property CurrDesc() As String
        Get
            Return mCurrDesc
        End Get
        Set(ByVal value As String)
            mCurrDesc = value
        End Set
    End Property

    Public Property Classif() As Integer
        Get
            Return mClassif
        End Get
        Set(ByVal value As Integer)
            mClassif = value
        End Set
    End Property

    Public Property Particulars() As String
        Get
            Return mParticulars
        End Get
        Set(ByVal value As String)
            mParticulars = value
        End Set
    End Property

    Public Property Desc() As String
        Get
            Return mDesc
        End Get
        Set(ByVal value As String)
            mDesc = value
        End Set
    End Property

End Class
