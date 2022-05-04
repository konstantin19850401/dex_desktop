object EAgentDic: TEAgentDic
  Left = 308
  Top = 202
  BorderIcons = [biSystemMenu]
  BorderStyle = bsDialog
  Caption = #1055#1088#1077#1076#1089#1090#1072#1074#1080#1090#1077#1083#1100
  ClientHeight = 304
  ClientWidth = 327
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  OldCreateOrder = False
  Position = poOwnerFormCenter
  OnShow = FormShow
  PixelsPerInch = 96
  TextHeight = 13
  object Label1: TLabel
    Left = 8
    Top = 8
    Width = 112
    Height = 13
    Caption = #1057#1087#1080#1089#1086#1082' '#1082#1086#1085#1090#1088#1072#1075#1077#1085#1090#1086#1074':'
  end
  object Label2: TLabel
    Left = 8
    Top = 176
    Width = 36
    Height = 13
    Caption = #1060'.'#1048'.'#1054'.'
  end
  object Label3: TLabel
    Left = 8
    Top = 224
    Width = 107
    Height = 13
    Caption = #1055#1072#1089#1087#1086#1088#1090#1085#1099#1077' '#1076#1072#1085#1085#1099#1077':'
  end
  object rsbOK: TRxSpeedButton
    Left = 136
    Top = 272
    Width = 89
    Height = 25
    Caption = #1054#1050
    Style = bsWin31
    OnClick = rsbOKClick
  end
  object rsbCancel: TRxSpeedButton
    Left = 232
    Top = 272
    Width = 89
    Height = 25
    Caption = #1054#1090#1084#1077#1085#1072
    Style = bsWin31
    OnClick = rsbCancelClick
  end
  object Label4: TLabel
    Left = 8
    Top = 88
    Width = 313
    Height = 13
    Alignment = taCenter
    AutoSize = False
    Caption = #1053#1077#1090' '#1079#1072#1087#1080#1089#1077#1081
  end
  object dbgA: TRxDBGrid
    Left = 8
    Top = 24
    Width = 313
    Height = 145
    DataSource = dsqa
    Options = [dgTitles, dgColLines, dgRowLines, dgRowSelect, dgAlwaysShowSelection, dgCancelOnExit]
    TabOrder = 0
    TitleFont.Charset = DEFAULT_CHARSET
    TitleFont.Color = clWindowText
    TitleFont.Height = -11
    TitleFont.Name = 'MS Sans Serif'
    TitleFont.Style = []
    OnCellClick = dbgACellClick
    OnDblClick = dbgADblClick
    Columns = <
      item
        Expanded = False
        FieldName = 'c_name'
        Title.Caption = #1060'.'#1048'.'#1054'.'
        Width = 291
        Visible = True
      end>
  end
  object eAgent: TEdit
    Left = 8
    Top = 192
    Width = 313
    Height = 21
    TabOrder = 1
  end
  object ePassport: TEdit
    Left = 8
    Top = 240
    Width = 313
    Height = 21
    TabOrder = 2
  end
  object qa: TAdsQuery
    StoreActive = True
    Version = '6.2 (ACE 6.20)'
    ReadAllColumns = False
    Left = 288
    Top = 8
    ParamData = <>
  end
  object dsqa: TDataSource
    DataSet = qa
    Left = 248
    Top = 8
  end
end
