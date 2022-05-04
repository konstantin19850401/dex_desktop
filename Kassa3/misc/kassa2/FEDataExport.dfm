object EDataExport: TEDataExport
  Left = 311
  Top = 238
  BorderIcons = [biSystemMenu]
  BorderStyle = bsSingle
  Caption = #1069#1082#1089#1087#1086#1088#1090' '#1076#1072#1085#1085#1099#1093
  ClientHeight = 226
  ClientWidth = 479
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
  object GroupBox1: TGroupBox
    Left = 8
    Top = 8
    Width = 465
    Height = 129
    Caption = #1055#1072#1088#1072#1084#1077#1090#1088#1099
    TabOrder = 0
    object rbDate: TRadioButton
      Left = 8
      Top = 24
      Width = 113
      Height = 17
      Caption = #1048#1085#1090#1077#1088#1074#1072#1083
      Checked = True
      TabOrder = 0
      TabStop = True
      OnClick = rbDateClick
    end
    object rbFull: TRadioButton
      Left = 8
      Top = 104
      Width = 113
      Height = 17
      Caption = #1042#1089#1103' '#1073#1072#1079#1072
      TabOrder = 1
      OnClick = rbDateClick
    end
    object pDate: TPanel
      Left = 8
      Top = 48
      Width = 449
      Height = 37
      TabOrder = 2
      object Label1: TLabel
        Left = 8
        Top = 12
        Width = 81
        Height = 13
        Caption = #1053#1072#1095#1072#1083#1100#1085#1072#1103' '#1076#1072#1090#1072
      end
      object Label2: TLabel
        Left = 240
        Top = 12
        Width = 74
        Height = 13
        Caption = #1050#1086#1085#1077#1095#1085#1072#1103' '#1076#1072#1090#1072
      end
      object deStart: TDateEdit
        Left = 96
        Top = 8
        Width = 121
        Height = 21
        NumGlyphs = 2
        TabOrder = 0
      end
      object deEnd: TDateEdit
        Left = 320
        Top = 8
        Width = 121
        Height = 21
        NumGlyphs = 2
        TabOrder = 1
      end
    end
  end
  object gbStatus: TGroupBox
    Left = 8
    Top = 144
    Width = 465
    Height = 41
    Caption = #1057#1090#1072#1090#1091#1089
    TabOrder = 1
    object pbStatus: TProgressBar
      Left = 8
      Top = 16
      Width = 449
      Height = 16
      TabOrder = 0
    end
  end
  object Button1: TButton
    Left = 376
    Top = 192
    Width = 99
    Height = 25
    Caption = #1053#1072#1095#1072#1090#1100' '#1101#1082#1089#1087#1086#1088#1090
    TabOrder = 2
    OnClick = Button1Click
  end
  object fs: TFormStorage
    Active = False
    Options = []
    StoredProps.Strings = (
      'deEnd.Text'
      'deStart.Text'
      'rbDate.Checked'
      'rbFull.Checked')
    StoredValues = <>
    Left = 336
    Top = 192
  end
  object sd: TSaveDialog
    DefaultExt = 'xml'
    Filter = #1060#1072#1081#1083#1099' XML|*.xml'
    FilterIndex = 0
    Options = [ofOverwritePrompt, ofHideReadOnly, ofPathMustExist, ofEnableSizing]
    Left = 8
    Top = 192
  end
end
