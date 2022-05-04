object ECln: TECln
  Left = 319
  Top = 222
  BorderIcons = [biSystemMenu]
  BorderStyle = bsDialog
  Caption = #1050#1086#1085#1090#1088#1072#1075#1077#1085#1090
  ClientHeight = 137
  ClientWidth = 376
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
    Top = 12
    Width = 36
    Height = 13
    Caption = #1060'.'#1048'.'#1054'.'
  end
  object Label2: TLabel
    Left = 8
    Top = 56
    Width = 173
    Height = 13
    Caption = #1055#1072#1089#1087#1086#1088#1090#1085#1099#1077' '#1076#1072#1085#1085#1099#1077' '#1082#1086#1085#1090#1088#1072#1075#1077#1085#1090#1072':'
  end
  object rsbOK: TRxSpeedButton
    Left = 184
    Top = 104
    Width = 89
    Height = 25
    Caption = #1054#1050
    Style = bsWin31
    OnClick = rsbOKClick
  end
  object rsbCancel: TRxSpeedButton
    Left = 280
    Top = 104
    Width = 89
    Height = 25
    Caption = #1054#1090#1084#1077#1085#1072
    Style = bsWin31
    OnClick = rsbCancelClick
  end
  object Label3: TLabel
    Left = 8
    Top = 36
    Width = 38
    Height = 13
    Caption = #1043#1088#1091#1087#1087#1072':'
  end
  object eName: TEdit
    Left = 56
    Top = 8
    Width = 313
    Height = 21
    TabOrder = 0
    Text = 'eName'
    OnKeyPress = eNameKeyPress
  end
  object ePassport: TEdit
    Left = 8
    Top = 72
    Width = 361
    Height = 21
    TabOrder = 2
    Text = 'ePassport'
    OnKeyPress = ePassportKeyPress
  end
  object rleGrp: TRxLookupEdit
    Left = 56
    Top = 32
    Width = 313
    Height = 21
    LookupDisplay = 'C_Group'
    LookupField = 'C_Group'
    LookupSource = dsqgrp
    PopupOnlyLocate = False
    TabOrder = 1
    OnKeyPress = eNameKeyPress
  end
  object qGrp: TAdsQuery
    StoreActive = True
    Version = '6.2 (ACE 6.20)'
    ReadAllColumns = False
    Left = 8
    Top = 96
    ParamData = <>
  end
  object dsqgrp: TDataSource
    DataSet = qGrp
    Left = 40
    Top = 96
  end
end
