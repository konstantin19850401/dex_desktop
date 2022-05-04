object EAct: TEAct
  Left = 301
  Top = 211
  BorderIcons = [biSystemMenu]
  BorderStyle = bsSingle
  Caption = #1054#1087#1077#1088#1072#1094#1080#1103
  ClientHeight = 264
  ClientWidth = 455
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  OldCreateOrder = False
  Position = poOwnerFormCenter
  OnCreate = FormCreate
  OnShow = FormShow
  PixelsPerInch = 96
  TextHeight = 13
  object Label1: TLabel
    Left = 8
    Top = 12
    Width = 80
    Height = 13
    Caption = #1044#1072#1090#1072' '#1086#1087#1077#1088#1072#1094#1080#1080':'
  end
  object Label2: TLabel
    Left = 224
    Top = 12
    Width = 73
    Height = 13
    Caption = #1058#1080#1087' '#1086#1087#1077#1088#1072#1094#1080#1080':'
  end
  object Label3: TLabel
    Left = 8
    Top = 36
    Width = 53
    Height = 13
    Caption = #1054#1087#1077#1088#1072#1094#1080#1103':'
  end
  object Label8: TLabel
    Left = 8
    Top = 180
    Width = 88
    Height = 13
    Caption = #1057#1091#1084#1084#1072' '#1086#1087#1077#1088#1072#1094#1080#1080':'
  end
  object Label9: TLabel
    Left = 272
    Top = 180
    Width = 45
    Height = 13
    Caption = #1050#1091#1088#1089' '#1059#1045':'
  end
  object Label10: TLabel
    Left = 8
    Top = 204
    Width = 59
    Height = 13
    Caption = #1054#1089#1085#1086#1074#1072#1085#1080#1077':'
  end
  object Label11: TLabel
    Left = 8
    Top = 228
    Width = 28
    Height = 13
    Caption = #1042#1074#1086#1076':'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clPurple
    Font.Height = -11
    Font.Name = 'MS Sans Serif'
    Font.Style = []
    ParentFont = False
  end
  object Label12: TLabel
    Left = 8
    Top = 242
    Width = 61
    Height = 13
    Caption = #1048#1079#1084#1077#1085#1077#1085#1080#1077':'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clPurple
    Font.Height = -11
    Font.Name = 'MS Sans Serif'
    Font.Style = []
    ParentFont = False
  end
  object lUser_cr: TLabel
    Left = 72
    Top = 228
    Width = 217
    Height = 13
    AutoSize = False
    Caption = 'lUser_cr'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clNavy
    Font.Height = -11
    Font.Name = 'MS Sans Serif'
    Font.Style = []
    ParentFont = False
    WordWrap = True
  end
  object lUser_ch: TLabel
    Left = 72
    Top = 242
    Width = 217
    Height = 13
    AutoSize = False
    Caption = 'lUser_ch'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clNavy
    Font.Height = -11
    Font.Name = 'MS Sans Serif'
    Font.Style = []
    ParentFont = False
    WordWrap = True
  end
  object deK_Date: TDateEdit
    Left = 96
    Top = 8
    Width = 121
    Height = 21
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -11
    Font.Name = 'MS Sans Serif'
    Font.Style = [fsBold]
    NumGlyphs = 2
    ParentFont = False
    TabOrder = 0
    OnKeyPress = deK_DateKeyPress
  end
  object cbOpType: TComboBox
    Left = 304
    Top = 8
    Width = 145
    Height = 21
    Style = csDropDownList
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -11
    Font.Name = 'MS Sans Serif'
    Font.Style = [fsBold]
    ItemHeight = 13
    ParentFont = False
    TabOrder = 1
    OnChange = cbOpTypeChange
    OnKeyPress = deK_DateKeyPress
    Items.Strings = (
      #1055#1088#1080#1093#1086#1076
      #1056#1072#1089#1093#1086#1076)
  end
  object gbsrc: TGroupBox
    Left = 8
    Top = 56
    Width = 217
    Height = 113
    Caption = #1048#1089#1090#1086#1095#1085#1080#1082
    TabOrder = 3
    object Label4: TLabel
      Left = 8
      Top = 16
      Width = 55
      Height = 13
      Caption = #1058#1080#1087' '#1074#1074#1086#1076#1072':'
    end
    object Label5: TLabel
      Left = 8
      Top = 64
      Width = 88
      Height = 13
      Caption = #1057#1095#1105#1090'/'#1082#1086#1085#1090#1088#1072#1075#1077#1085#1090':'
    end
    object rsbAddSrc: TRxSpeedButton
      Left = 188
      Top = 80
      Width = 21
      Height = 21
      Caption = '+'
      OnClick = rsbAddSrcClick
    end
    object cbIsrc: TComboBox
      Left = 8
      Top = 32
      Width = 201
      Height = 21
      Style = csDropDownList
      Font.Charset = DEFAULT_CHARSET
      Font.Color = clWindowText
      Font.Height = -11
      Font.Name = 'MS Sans Serif'
      Font.Style = [fsBold]
      ItemHeight = 13
      ParentFont = False
      TabOrder = 0
      OnChange = cbIsrcChange
      OnKeyPress = deK_DateKeyPress
    end
    object rleNSrc: TRxLookupEdit
      Left = 8
      Top = 80
      Width = 177
      Height = 21
      LookupDisplay = 'rec_name'
      LookupField = 'rec_name'
      LookupSource = dsqsrc
      PopupOnlyLocate = False
      Font.Charset = DEFAULT_CHARSET
      Font.Color = clWindowText
      Font.Height = -11
      Font.Name = 'MS Sans Serif'
      Font.Style = [fsBold]
      ParentFont = False
      TabOrder = 1
      Text = 'rleNSrc'
      OnKeyPress = deK_DateKeyPress
      OnKeyUp = rleNSrcKeyUp
    end
  end
  object gbdst: TGroupBox
    Left = 232
    Top = 56
    Width = 217
    Height = 113
    Caption = #1055#1088#1080#1105#1084#1085#1080#1082
    TabOrder = 4
    object Label6: TLabel
      Left = 8
      Top = 16
      Width = 55
      Height = 13
      Caption = #1058#1080#1087' '#1074#1074#1086#1076#1072':'
    end
    object Label7: TLabel
      Left = 8
      Top = 64
      Width = 88
      Height = 13
      Caption = #1057#1095#1105#1090'/'#1082#1086#1085#1090#1088#1072#1075#1077#1085#1090':'
    end
    object rsbAddDst: TRxSpeedButton
      Left = 188
      Top = 80
      Width = 21
      Height = 21
      Caption = '+'
      OnClick = rsbAddDstClick
    end
    object cbIdst: TComboBox
      Left = 8
      Top = 32
      Width = 201
      Height = 21
      Style = csDropDownList
      Font.Charset = DEFAULT_CHARSET
      Font.Color = clWindowText
      Font.Height = -11
      Font.Name = 'MS Sans Serif'
      Font.Style = [fsBold]
      ItemHeight = 13
      ParentFont = False
      TabOrder = 0
      OnChange = cbIdstChange
      OnKeyPress = deK_DateKeyPress
    end
    object rleNDst: TRxLookupEdit
      Left = 8
      Top = 80
      Width = 177
      Height = 21
      LookupDisplay = 'rec_name'
      LookupField = 'rec_name'
      LookupSource = dsqdst
      PopupOnlyLocate = False
      Font.Charset = DEFAULT_CHARSET
      Font.Color = clWindowText
      Font.Height = -11
      Font.Name = 'MS Sans Serif'
      Font.Style = [fsBold]
      ParentFont = False
      TabOrder = 1
      Text = 'rleNDst'
      OnKeyPress = deK_DateKeyPress
      OnKeyUp = rleNDstKeyUp
    end
  end
  object ceSum: TCurrencyEdit
    Left = 104
    Top = 176
    Width = 121
    Height = 21
    AutoSize = False
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -11
    Font.Name = 'MS Sans Serif'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 5
    OnKeyPress = deK_DateKeyPress
  end
  object ceCurs: TCurrencyEdit
    Left = 328
    Top = 176
    Width = 121
    Height = 21
    AutoSize = False
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -11
    Font.Name = 'MS Sans Serif'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 6
    OnKeyPress = deK_DateKeyPress
  end
  object ePrim: TEdit
    Left = 72
    Top = 200
    Width = 377
    Height = 21
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -11
    Font.Name = 'MS Sans Serif'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 7
    Text = 'ePrim'
    OnKeyPress = ePrimKeyPress
  end
  object bOk: TButton
    Left = 296
    Top = 232
    Width = 75
    Height = 25
    Caption = #1054#1050
    TabOrder = 8
    OnClick = bOkClick
  end
  object bCancel: TButton
    Left = 376
    Top = 232
    Width = 75
    Height = 25
    Cancel = True
    Caption = #1054#1090#1084#1077#1085#1072
    ModalResult = 2
    TabOrder = 9
  end
  object dbcbOp: TRxLookupEdit
    Left = 72
    Top = 32
    Width = 377
    Height = 21
    LookupDisplay = 'name'
    LookupField = 'name'
    LookupSource = dsqop
    DirectInput = False
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -11
    Font.Name = 'MS Sans Serif'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 2
    Text = 'dbcbOp'
    OnCloseUp = dbcbOpChange
    OnChange = dbcbOpChange
    OnKeyPress = deK_DateKeyPress
  end
  object qop: TAdsQuery
    StoreActive = True
    Version = '6.2 (ACE 6.20)'
    ReadAllColumns = False
    Left = 16
    Top = 48
    ParamData = <>
  end
  object dsqop: TDataSource
    DataSet = qop
    Left = 24
    Top = 56
  end
  object qsrc: TAdsQuery
    StoreActive = True
    Version = '6.2 (ACE 6.20)'
    ReadAllColumns = False
    Left = 64
    Top = 48
    ParamData = <>
  end
  object qdst: TAdsQuery
    StoreActive = True
    Version = '6.2 (ACE 6.20)'
    ReadAllColumns = False
    Left = 112
    Top = 48
    ParamData = <>
  end
  object dsqsrc: TDataSource
    DataSet = qsrc
    Left = 72
    Top = 56
  end
  object dsqdst: TDataSource
    DataSet = qdst
    Left = 120
    Top = 56
  end
  object fs: TFormStorage
    Active = False
    StoredProps.Strings = (
      'deK_Date.Text')
    StoredValues = <>
    Left = 168
    Top = 48
  end
end
