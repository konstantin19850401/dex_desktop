object Form1: TForm1
  Left = 312
  Top = 121
  Width = 410
  Height = 117
  Caption = 'Form1'
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  OldCreateOrder = False
  PixelsPerInch = 96
  TextHeight = 13
  object Label1: TLabel
    Left = 8
    Top = 12
    Width = 58
    Height = 13
    Caption = #1057#1090#1072#1088#1072#1103' '#1041#1044':'
  end
  object Label2: TLabel
    Left = 8
    Top = 36
    Width = 54
    Height = 13
    Caption = #1053#1086#1074#1072#1103' '#1041#1044':'
  end
  object deSRC: TDirectoryEdit
    Left = 80
    Top = 8
    Width = 313
    Height = 21
    NumGlyphs = 1
    TabOrder = 0
    Text = 'c:\bases\kassa'
  end
  object deDST: TDirectoryEdit
    Left = 80
    Top = 32
    Width = 313
    Height = 21
    NumGlyphs = 1
    TabOrder = 1
    Text = 'c:\bases\kassa2'
  end
  object Button1: TButton
    Left = 80
    Top = 56
    Width = 313
    Height = 25
    Caption = #1050#1086#1085#1074#1077#1088#1090#1080#1088#1086#1074#1072#1090#1100' '#1073#1072#1079#1091
    TabOrder = 2
    OnClick = Button1Click
  end
  object aq: TAdsQuery
    StoreActive = True
    Version = '6.2 (ACE 6.20)'
    ReadAllColumns = False
    Left = 8
    Top = 56
    ParamData = <>
  end
  object bq: TQuery
    Left = 40
    Top = 56
  end
end
