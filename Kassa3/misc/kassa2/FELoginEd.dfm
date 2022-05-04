object eLoginEd: TeLoginEd
  Left = 389
  Top = 182
  BorderIcons = []
  BorderStyle = bsDialog
  Caption = #1042#1093#1086#1076
  ClientHeight = 137
  ClientWidth = 313
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  OldCreateOrder = False
  Position = poScreenCenter
  OnHide = FormHide
  OnShow = FormShow
  PixelsPerInch = 96
  TextHeight = 13
  object Label1: TLabel
    Left = 8
    Top = 8
    Width = 73
    Height = 13
    Caption = #1055#1086#1083#1100#1079#1086#1074#1072#1090#1077#1083#1100
  end
  object Label2: TLabel
    Left = 8
    Top = 56
    Width = 38
    Height = 13
    Caption = #1055#1072#1088#1086#1083#1100
  end
  object rsbOK: TRxSpeedButton
    Left = 136
    Top = 104
    Width = 81
    Height = 25
    Caption = #1054#1050
    Style = bsWin31
    OnClick = rsbOKClick
  end
  object rsbCancel: TRxSpeedButton
    Left = 224
    Top = 104
    Width = 81
    Height = 25
    Caption = #1054#1090#1084#1077#1085#1072
    Style = bsWin31
    OnClick = rsbCancelClick
  end
  object rleUserName: TRxLookupEdit
    Left = 8
    Top = 24
    Width = 297
    Height = 21
    LookupDisplay = 'username'
    LookupField = 'username'
    LookupSource = dsusr
    DirectInput = False
    TabOrder = 0
    Text = 'rleUserName'
    OnKeyPress = rleUserNameKeyPress
  end
  object ePassword: TEdit
    Left = 8
    Top = 72
    Width = 297
    Height = 21
    PasswordChar = '*'
    TabOrder = 1
    OnKeyPress = ePasswordKeyPress
  end
  object dsusr: TDataSource
    DataSet = qusr
    Left = 240
    Top = 8
  end
  object qusr: TAdsQuery
    StoreActive = True
    Version = '6.2 (ACE 6.20)'
    ReadAllColumns = False
    Left = 272
    Top = 8
    ParamData = <>
  end
  object fs: TFormStorage
    Active = False
    IniFileName = 'Software\Kassa 2'
    IniSection = 'Logon'
    Options = []
    UseRegistry = True
    OnSavePlacement = fsSavePlacement
    OnRestorePlacement = fsRestorePlacement
    StoredValues = <>
    Left = 208
    Top = 8
  end
end
