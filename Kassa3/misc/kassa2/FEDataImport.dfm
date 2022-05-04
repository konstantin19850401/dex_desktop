object EDataImport: TEDataImport
  Left = 281
  Top = 197
  Width = 607
  Height = 382
  Caption = #1048#1084#1087#1086#1088#1090' '#1076#1072#1085#1085#1099#1093
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  OldCreateOrder = False
  PixelsPerInch = 96
  TextHeight = 13
  object lbLog: TListBox
    Left = 8
    Top = 8
    Width = 585
    Height = 305
    ItemHeight = 13
    TabOrder = 0
  end
  object bSelectFile: TButton
    Left = 8
    Top = 320
    Width = 75
    Height = 25
    Caption = #1048#1084#1087#1086#1088#1090
    TabOrder = 1
    OnClick = bSelectFileClick
  end
  object od: TOpenDialog
    DefaultExt = 'xml'
    Filter = #1060#1072#1081#1083#1099' XML|*.xml'
    FilterIndex = 0
    Left = 568
    Top = 320
  end
end
