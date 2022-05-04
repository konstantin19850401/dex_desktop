object EAEd: TEAEd
  Left = 292
  Top = 203
  BorderStyle = bsDialog
  Caption = #1044#1072#1085#1085#1099#1077' '#1089#1095#1105#1090#1072
  ClientHeight = 358
  ClientWidth = 536
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  OldCreateOrder = False
  OnShow = FormShow
  PixelsPerInch = 96
  TextHeight = 13
  object Label1: TLabel
    Left = 8
    Top = 8
    Width = 110
    Height = 13
    Caption = #1053#1072#1080#1084#1077#1085#1086#1074#1072#1085#1080#1077' '#1089#1095#1105#1090#1072':'
  end
  object Label2: TLabel
    Left = 168
    Top = 8
    Width = 121
    Height = 13
    Caption = #1053#1072#1080#1084#1077#1085#1086#1074#1072#1085#1080#1077' '#1092#1080#1088#1084#1099': '
  end
  object Label3: TLabel
    Left = 8
    Top = 48
    Width = 95
    Height = 13
    Caption = #1043#1083#1072#1074#1085#1099#1081' '#1073#1091#1075#1072#1083#1090#1077#1088':'
  end
  object Label4: TLabel
    Left = 176
    Top = 48
    Width = 40
    Height = 13
    Caption = #1050#1072#1089#1089#1080#1088':'
  end
  object Label11: TLabel
    Left = 392
    Top = 8
    Width = 134
    Height = 13
    Caption = #1044#1086#1083#1078#1085#1086#1089#1090#1100' '#1088#1091#1082#1086#1074#1086#1076#1080#1090#1077#1083#1103':'
  end
  object Label12: TLabel
    Left = 360
    Top = 48
    Width = 74
    Height = 13
    Caption = #1056#1091#1082#1086#1074#1086#1076#1080#1090#1077#1083#1100':'
  end
  object rsbOK: TRxSpeedButton
    Left = 344
    Top = 328
    Width = 89
    Height = 25
    Caption = #1054#1050
    Style = bsWin31
    OnClick = rsbOKClick
  end
  object rsbCancel: TRxSpeedButton
    Left = 440
    Top = 328
    Width = 89
    Height = 25
    Caption = #1054#1090#1084#1077#1085#1072
    Style = bsWin31
    OnClick = rsbCancelClick
  end
  object Label21: TLabel
    Left = 8
    Top = 92
    Width = 227
    Height = 13
    Caption = #1055#1088#1077#1076#1089#1090#1072#1074#1080#1090#1077#1083#1100' '#1092#1080#1088#1084#1099', '#1087#1086#1083#1091#1095#1072#1102#1097#1080#1081' '#1076#1077#1085#1100#1075#1080':'
  end
  object Label22: TLabel
    Left = 248
    Top = 92
    Width = 39
    Height = 13
    Caption = #1060'.'#1048'.'#1054'.:'
  end
  object Label23: TLabel
    Left = 8
    Top = 116
    Width = 107
    Height = 13
    Caption = #1055#1072#1089#1087#1086#1088#1090#1085#1099#1077' '#1076#1072#1085#1085#1099#1077':'
  end
  object RxSpeedButton1: TRxSpeedButton
    Left = 504
    Top = 112
    Width = 25
    Height = 21
    Caption = '>>'
    OnClick = RxSpeedButton1Click
  end
  object eAcc: TEdit
    Left = 8
    Top = 24
    Width = 153
    Height = 21
    TabOrder = 0
  end
  object eOwnCompanyName: TEdit
    Left = 168
    Top = 24
    Width = 217
    Height = 21
    TabOrder = 1
  end
  object eGlavBuh: TEdit
    Left = 8
    Top = 64
    Width = 169
    Height = 21
    TabOrder = 3
  end
  object eKassir: TEdit
    Left = 184
    Top = 64
    Width = 169
    Height = 21
    TabOrder = 4
  end
  object eDirTitle: TEdit
    Left = 392
    Top = 24
    Width = 137
    Height = 21
    TabOrder = 2
  end
  object eDirName: TEdit
    Left = 360
    Top = 64
    Width = 169
    Height = 21
    TabOrder = 5
  end
  object PageControl1: TPageControl
    Left = 8
    Top = 152
    Width = 521
    Height = 169
    ActivePage = ts41
    TabOrder = 6
    object ts41: TTabSheet
      Caption = #1055#1088#1080#1093#1086#1076#1085#1099#1081' '#1082#1072#1089#1089#1086#1074#1099#1081' '#1086#1088#1076#1077#1088
      object Label5: TLabel
        Left = 0
        Top = 12
        Width = 87
        Height = 13
        Caption = #1060#1086#1088#1084#1072' '#1087#1086' '#1054#1050#1059#1044
      end
      object Label6: TLabel
        Left = 324
        Top = 12
        Width = 46
        Height = 13
        Caption = #1087#1086' '#1054#1050#1055#1054
      end
      object Label7: TLabel
        Left = 56
        Top = 36
        Width = 32
        Height = 13
        Caption = #1044#1077#1073#1077#1090
      end
      object Label8: TLabel
        Left = 198
        Top = 36
        Width = 173
        Height = 13
        Caption = #1050#1086#1076' '#1089#1090#1088#1091#1082#1090#1091#1088#1085#1086#1075#1086' '#1087#1086#1076#1088#1072#1079#1076#1077#1083#1077#1085#1080#1103':'
      end
      object Label9: TLabel
        Left = 192
        Top = 60
        Width = 180
        Height = 13
        Caption = #1050#1086#1088#1088#1077#1089#1087#1086#1085#1076#1080#1088#1091#1102#1097#1080#1081' '#1089#1095#1105#1090', '#1089#1091#1073#1089#1095#1105#1090':'
      end
      object Label10: TLabel
        Left = 0
        Top = 84
        Width = 136
        Height = 13
        Caption = #1050#1086#1076' '#1072#1085#1072#1083#1080#1090#1080#1095#1077#1089#1082#1086#1075#1086' '#1091#1095#1105#1090#1072':'
      end
      object Label13: TLabel
        Left = 248
        Top = 84
        Width = 134
        Height = 13
        Caption = #1050#1086#1076' '#1094#1077#1083#1077#1074#1086#1075#1086' '#1085#1072#1079#1085#1072#1095#1077#1085#1080#1103':'
      end
      object rsbShowPrihod: TRxSpeedButton
        Left = 8
        Top = 112
        Width = 489
        Height = 25
        Caption = #1055#1088#1086#1089#1084#1086#1090#1088' '#1073#1083#1072#1085#1082#1072' '#1087#1088#1080#1093#1086#1076#1085#1086#1075#1086' '#1082#1072#1089#1089#1086#1074#1086#1075#1086' '#1086#1088#1076#1077#1088#1072
        Style = bsWin31
        OnClick = rsbShowPrihodClick
      end
      object epOKUD: TEdit
        Left = 96
        Top = 8
        Width = 121
        Height = 21
        TabOrder = 0
      end
      object epOKPO: TEdit
        Left = 376
        Top = 8
        Width = 121
        Height = 21
        TabOrder = 1
      end
      object epDEBET: TEdit
        Left = 96
        Top = 32
        Width = 81
        Height = 21
        TabOrder = 2
      end
      object epSPODR: TEdit
        Left = 376
        Top = 32
        Width = 121
        Height = 21
        TabOrder = 3
      end
      object epKSS: TEdit
        Left = 376
        Top = 56
        Width = 121
        Height = 21
        TabOrder = 4
      end
      object epKANAL: TEdit
        Left = 144
        Top = 80
        Width = 97
        Height = 21
        TabOrder = 5
      end
      object epKCEL: TEdit
        Left = 392
        Top = 80
        Width = 105
        Height = 21
        TabOrder = 6
      end
    end
    object ts42: TTabSheet
      Caption = #1056#1072#1089#1093#1086#1076#1085#1099#1081' '#1082#1072#1089#1089#1086#1074#1099#1081' '#1086#1088#1076#1077#1088
      ImageIndex = 1
      object Label14: TLabel
        Left = 0
        Top = 12
        Width = 87
        Height = 13
        Caption = #1060#1086#1088#1084#1072' '#1087#1086' '#1054#1050#1059#1044
      end
      object Label15: TLabel
        Left = 324
        Top = 12
        Width = 46
        Height = 13
        Caption = #1087#1086' '#1054#1050#1055#1054
      end
      object Label16: TLabel
        Left = 52
        Top = 36
        Width = 36
        Height = 13
        Caption = #1050#1088#1077#1076#1080#1090
      end
      object Label17: TLabel
        Left = 198
        Top = 36
        Width = 173
        Height = 13
        Caption = #1050#1086#1076' '#1089#1090#1088#1091#1082#1090#1091#1088#1085#1086#1075#1086' '#1087#1086#1076#1088#1072#1079#1076#1077#1083#1077#1085#1080#1103':'
      end
      object Label18: TLabel
        Left = 192
        Top = 60
        Width = 180
        Height = 13
        Caption = #1050#1086#1088#1088#1077#1089#1087#1086#1085#1076#1080#1088#1091#1102#1097#1080#1081' '#1089#1095#1105#1090', '#1089#1091#1073#1089#1095#1105#1090':'
      end
      object Label19: TLabel
        Left = 0
        Top = 84
        Width = 136
        Height = 13
        Caption = #1050#1086#1076' '#1072#1085#1072#1083#1080#1090#1080#1095#1077#1089#1082#1086#1075#1086' '#1091#1095#1105#1090#1072':'
      end
      object Label20: TLabel
        Left = 248
        Top = 84
        Width = 134
        Height = 13
        Caption = #1050#1086#1076' '#1094#1077#1083#1077#1074#1086#1075#1086' '#1085#1072#1079#1085#1072#1095#1077#1085#1080#1103':'
      end
      object rsbShowRashod: TRxSpeedButton
        Left = 8
        Top = 112
        Width = 489
        Height = 25
        Caption = #1055#1088#1086#1089#1084#1086#1090#1088' '#1073#1083#1072#1085#1082#1072' '#1088#1072#1089#1093#1086#1076#1085#1086#1075#1086' '#1082#1072#1089#1089#1086#1074#1086#1075#1086' '#1086#1088#1076#1077#1088#1072
        Style = bsWin31
        OnClick = rsbShowRashodClick
      end
      object erOKUD: TEdit
        Left = 96
        Top = 8
        Width = 121
        Height = 21
        TabOrder = 0
      end
      object erOKPO: TEdit
        Left = 376
        Top = 8
        Width = 121
        Height = 21
        TabOrder = 1
      end
      object erKREDIT: TEdit
        Left = 96
        Top = 32
        Width = 81
        Height = 21
        TabOrder = 2
      end
      object erSPODR: TEdit
        Left = 376
        Top = 32
        Width = 121
        Height = 21
        TabOrder = 3
      end
      object erKSS: TEdit
        Left = 376
        Top = 56
        Width = 121
        Height = 21
        TabOrder = 4
      end
      object erKANAL: TEdit
        Left = 144
        Top = 80
        Width = 97
        Height = 21
        TabOrder = 5
      end
      object erKCEL: TEdit
        Left = 392
        Top = 80
        Width = 105
        Height = 21
        TabOrder = 6
      end
    end
  end
  object eAgent: TEdit
    Left = 296
    Top = 88
    Width = 233
    Height = 21
    TabOrder = 7
    Text = 'eAgent'
  end
  object ePassport: TEdit
    Left = 128
    Top = 112
    Width = 369
    Height = 21
    TabOrder = 8
    Text = 'ePassport'
  end
end
