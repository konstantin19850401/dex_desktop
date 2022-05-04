object ESvParam: TESvParam
  Left = 301
  Top = 128
  BorderIcons = [biSystemMenu]
  BorderStyle = bsDialog
  Caption = #1055#1072#1088#1072#1084#1077#1090#1088#1099' '#1089#1074#1086#1076#1082#1080
  ClientHeight = 424
  ClientWidth = 319
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
    Top = 0
    Width = 118
    Height = 13
    Caption = #1053#1072#1080#1084#1077#1085#1086#1074#1072#1085#1080#1077' '#1089#1074#1086#1076#1082#1080':'
  end
  object RxSpeedButton1: TRxSpeedButton
    Left = 128
    Top = 392
    Width = 89
    Height = 25
    Caption = #1054#1050
    Style = bsWin31
    OnClick = RxSpeedButton1Click
  end
  object RxSpeedButton2: TRxSpeedButton
    Left = 224
    Top = 392
    Width = 89
    Height = 25
    Caption = #1054#1090#1084#1077#1085#1072
    Style = bsWin31
    OnClick = RxSpeedButton2Click
  end
  object eTitle: TEdit
    Left = 8
    Top = 16
    Width = 305
    Height = 21
    TabOrder = 0
    Text = 'eTitle'
  end
  object pc: TPageControl
    Left = 8
    Top = 48
    Width = 305
    Height = 161
    ActivePage = ts1
    MultiLine = True
    Style = tsFlatButtons
    TabOrder = 1
    object ts1: TTabSheet
      Caption = #1054#1090#1095#1105#1090' '#1087#1086' '#1082#1086#1085#1090#1088#1072#1075#1077#1085#1090#1072#1084
      object Label4: TLabel
        Left = 0
        Top = 4
        Width = 38
        Height = 13
        Caption = #1043#1088#1091#1087#1087#1072':'
      end
      object clbcli: TRxCheckListBox
        Left = 0
        Top = 24
        Width = 297
        Height = 81
        ItemHeight = 13
        TabOrder = 0
        OnKeyUp = clbcliKeyUp
        InternalVersion = 202
      end
      object cbGrp: TComboBox
        Left = 48
        Top = 0
        Width = 249
        Height = 21
        Style = csDropDownList
        ItemHeight = 13
        TabOrder = 1
        OnChange = cbGrpChange
      end
      object cbRestrictAcc: TCheckBox
        Left = 0
        Top = 112
        Width = 281
        Height = 17
        Caption = #1054#1075#1088#1072#1085#1080#1095#1080#1090#1100' '#1089#1074#1086#1076#1082#1091' '#1087#1086' '#1091#1082#1072#1079#1072#1085#1085#1099#1084' '#1089#1095#1077#1090#1072#1084
        TabOrder = 2
      end
    end
    object ts2: TTabSheet
      Caption = #1054#1090#1095#1105#1090' '#1087#1086' '#1089#1095#1077#1090#1072#1084
      ImageIndex = 1
      object clbacc: TRxCheckListBox
        Left = 0
        Top = 0
        Width = 297
        Height = 121
        ItemHeight = 13
        TabOrder = 0
        OnKeyUp = clbaccKeyUp
        InternalVersion = 202
      end
    end
  end
  object GroupBox1: TGroupBox
    Left = 8
    Top = 209
    Width = 305
    Height = 48
    Caption = #1055#1077#1088#1080#1086#1076
    TabOrder = 2
    object Label2: TLabel
      Left = 8
      Top = 20
      Width = 7
      Height = 13
      Caption = #1057
    end
    object Label3: TLabel
      Left = 152
      Top = 20
      Width = 12
      Height = 13
      Caption = #1087#1086
    end
    object deStart: TDateEdit
      Left = 24
      Top = 16
      Width = 121
      Height = 21
      NumGlyphs = 2
      TabOrder = 0
    end
    object deend: TDateEdit
      Left = 176
      Top = 16
      Width = 121
      Height = 21
      NumGlyphs = 2
      TabOrder = 1
    end
  end
  object GroupBox2: TGroupBox
    Left = 8
    Top = 257
    Width = 305
    Height = 128
    Caption = #1054#1087#1077#1088#1072#1094#1080#1080
    TabOrder = 3
    object clbOP: TRxCheckListBox
      Left = 8
      Top = 16
      Width = 289
      Height = 105
      ItemHeight = 13
      TabOrder = 0
      OnKeyUp = clbOPKeyUp
      InternalVersion = 202
    end
  end
end
