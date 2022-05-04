object eUserEd: TeUserEd
  Left = 378
  Top = 167
  BorderIcons = [biSystemMenu]
  BorderStyle = bsDialog
  Caption = #1044#1072#1085#1085#1099#1077' '#1087#1086#1083#1100#1079#1086#1074#1072#1090#1077#1083#1103
  ClientHeight = 381
  ClientWidth = 361
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  OldCreateOrder = False
  Position = poScreenCenter
  OnShow = FormShow
  PixelsPerInch = 96
  TextHeight = 13
  object Label1: TLabel
    Left = 8
    Top = 12
    Width = 105
    Height = 13
    Alignment = taRightJustify
    AutoSize = False
    Caption = #1048#1084#1103' '#1087#1086#1083#1100#1079#1086#1074#1072#1090#1077#1083#1103
  end
  object Label2: TLabel
    Left = 8
    Top = 36
    Width = 105
    Height = 13
    Alignment = taRightJustify
    AutoSize = False
    Caption = #1055#1072#1088#1086#1083#1100
  end
  object rsbOK: TRxSpeedButton
    Left = 168
    Top = 344
    Width = 89
    Height = 25
    Caption = #1054#1050
    Style = bsWin31
    OnClick = rsbOKClick
  end
  object rsbCancel: TRxSpeedButton
    Left = 264
    Top = 344
    Width = 89
    Height = 25
    Caption = #1054#1090#1084#1077#1085#1072
    ModalResult = 2
    Style = bsWin31
  end
  object cbActive: TCheckBox
    Left = 120
    Top = 56
    Width = 145
    Height = 17
    Caption = #1055#1086#1083#1100#1079#1086#1074#1072#1090#1077#1083#1100' '#1072#1082#1090#1080#1074#1077#1085
    TabOrder = 2
    OnKeyPress = cbActiveKeyPress
  end
  object eUsername: TEdit
    Left = 120
    Top = 8
    Width = 233
    Height = 21
    TabOrder = 0
    Text = 'eUsername'
    OnKeyPress = eUsernameKeyPress
  end
  object ePassword: TEdit
    Left = 120
    Top = 32
    Width = 233
    Height = 21
    PasswordChar = '*'
    TabOrder = 1
    Text = 'ePassword'
    OnKeyPress = eUsernameKeyPress
  end
  object GroupBox1: TGroupBox
    Left = 8
    Top = 80
    Width = 345
    Height = 249
    Caption = #1056#1072#1079#1088#1077#1096#1077#1085#1080#1103
    TabOrder = 3
    object pc: TPageControl
      Left = 8
      Top = 16
      Width = 329
      Height = 225
      ActivePage = ts1
      HotTrack = True
      Style = tsButtons
      TabOrder = 0
      object ts1: TTabSheet
        Caption = #1054#1073#1097#1080#1077
        object Bevel1: TBevel
          Left = 0
          Top = 96
          Width = 321
          Height = 9
          Shape = bsTopLine
        end
        object Label4: TLabel
          Left = 8
          Top = 91
          Width = 173
          Height = 13
          Caption = #1054#1087#1077#1088#1072#1094#1080#1080' '#1089#1086' '#1089#1087#1088#1072#1074#1086#1095#1085#1080#1082#1072#1084#1080
          Font.Charset = DEFAULT_CHARSET
          Font.Color = clWindowText
          Font.Height = -11
          Font.Name = 'MS Sans Serif'
          Font.Style = [fsBold]
          ParentFont = False
        end
        object cbUSD: TCheckBox
          Left = 8
          Top = 0
          Width = 225
          Height = 17
          Caption = #1042#1074#1086#1076'/'#1080#1079#1084#1077#1085#1077#1085#1080#1077' '#1082#1091#1088#1089#1072' '#1074#1072#1083#1102#1090#1099
          TabOrder = 0
        end
        object cbSvodki: TCheckBox
          Left = 8
          Top = 16
          Width = 257
          Height = 17
          Caption = #1055#1088#1086#1089#1084#1086#1090#1088' '#1089#1074#1086#1076#1086#1082' '#1087#1086' '#1089#1095#1077#1090#1072#1084
          TabOrder = 1
        end
        object cbBalance: TCheckBox
          Left = 8
          Top = 32
          Width = 161
          Height = 17
          Caption = #1055#1088#1086#1089#1084#1086#1090#1088' '#1086#1073#1097#1077#1075#1086' '#1073#1072#1083#1072#1085#1089#1072
          TabOrder = 2
        end
        object cbDicAccounts: TCheckBox
          Left = 8
          Top = 128
          Width = 97
          Height = 17
          Caption = #1057#1095#1077#1090#1072
          TabOrder = 6
        end
        object cbDicClients: TCheckBox
          Left = 8
          Top = 144
          Width = 97
          Height = 17
          Caption = #1050#1086#1085#1090#1088#1072#1075#1077#1085#1090#1099
          TabOrder = 7
          OnClick = cbDicClientsClick
        end
        object cbDicClientsAdd: TCheckBox
          Left = 16
          Top = 160
          Width = 201
          Height = 17
          Caption = #1058#1086#1083#1100#1082#1086' '#1076#1086#1073#1072#1074#1083#1077#1085#1080#1077' '#1087#1088#1080' '#1086#1087#1077#1088#1072#1094#1080#1080
          Font.Charset = DEFAULT_CHARSET
          Font.Color = clTeal
          Font.Height = -11
          Font.Name = 'MS Sans Serif'
          Font.Style = []
          ParentFont = False
          TabOrder = 8
        end
        object cbDicOps: TCheckBox
          Left = 8
          Top = 176
          Width = 97
          Height = 17
          Caption = #1054#1087#1077#1088#1072#1094#1080#1080
          TabOrder = 9
        end
        object cbDicUsers: TCheckBox
          Left = 8
          Top = 112
          Width = 97
          Height = 17
          Caption = #1055#1086#1083#1100#1079#1086#1074#1072#1090#1077#1083#1080
          TabOrder = 5
        end
        object cbDelRecords: TCheckBox
          Left = 8
          Top = 48
          Width = 145
          Height = 17
          Caption = #1059#1076#1072#1083#1077#1085#1080#1077' '#1079#1072#1087#1080#1089#1077#1081
          TabOrder = 3
        end
        object cbImpExp: TCheckBox
          Left = 8
          Top = 64
          Width = 209
          Height = 17
          Caption = #1069#1082#1089#1087#1086#1088#1090' '#1080' '#1080#1084#1087#1086#1088#1090' '#1079#1072#1087#1080#1089#1077#1081
          TabOrder = 4
        end
      end
      object ts2: TTabSheet
        Caption = #1057#1095#1077#1090#1072
        ImageIndex = 1
        object dbg1: TDBGridEh
          Left = 0
          Top = 0
          Width = 321
          Height = 186
          Align = alClient
          AllowedOperations = [alopUpdateEh]
          AutoFitColWidths = True
          DataSource = ds1
          Flat = True
          FooterColor = clWindow
          FooterFont.Charset = DEFAULT_CHARSET
          FooterFont.Color = clWindowText
          FooterFont.Height = -11
          FooterFont.Name = 'MS Sans Serif'
          FooterFont.Style = []
          Options = [dgTitles, dgColLines, dgRowLines, dgTabs, dgCancelOnExit]
          TabOrder = 0
          TitleFont.Charset = DEFAULT_CHARSET
          TitleFont.Color = clWindowText
          TitleFont.Height = -11
          TitleFont.Name = 'MS Sans Serif'
          TitleFont.Style = []
          Columns = <
            item
              Color = clInfoBk
              EditButtons = <>
              FieldName = 'name'
              Footers = <>
              ReadOnly = True
              Title.Caption = #1057#1095#1105#1090
              Width = 209
            end
            item
              EditButtons = <>
              FieldName = 'op_in'
              Footers = <>
              Title.Caption = #1042#1074#1086#1076
            end
            item
              EditButtons = <>
              FieldName = 'op_out'
              Footers = <>
              Title.Caption = #1042#1099#1074#1086#1076
            end>
        end
      end
      object ts3: TTabSheet
        Caption = #1054#1087#1077#1088#1072#1094#1080#1080
        ImageIndex = 2
        object dbg2: TDBGridEh
          Left = 0
          Top = 0
          Width = 321
          Height = 162
          Align = alClient
          AllowedOperations = [alopUpdateEh]
          AutoFitColWidths = True
          DataSource = ds2
          Flat = True
          FooterColor = clWindow
          FooterFont.Charset = DEFAULT_CHARSET
          FooterFont.Color = clWindowText
          FooterFont.Height = -11
          FooterFont.Name = 'MS Sans Serif'
          FooterFont.Style = []
          Options = [dgEditing, dgTitles, dgColLines, dgRowLines, dgTabs, dgCancelOnExit]
          TabOrder = 0
          TitleFont.Charset = DEFAULT_CHARSET
          TitleFont.Color = clWindowText
          TitleFont.Height = -11
          TitleFont.Name = 'MS Sans Serif'
          TitleFont.Style = []
          Columns = <
            item
              Color = clInfoBk
              EditButtons = <>
              FieldName = 'name'
              Footers = <>
              ReadOnly = True
              Title.Caption = #1054#1087#1077#1088#1072#1094#1080#1103
              Width = 229
            end
            item
              EditButtons = <>
              FieldName = 'en'
              Footers = <>
              Title.Caption = #1055#1086#1079#1074#1086#1083#1077#1085#1072
              Width = 62
            end>
        end
      end
    end
  end
  object md1: TdxMemData
    Active = True
    Indexes = <>
    SortOptions = []
    Left = 12
    Top = 35
    object md1id: TIntegerField
      FieldName = 'id'
    end
    object md1name: TStringField
      FieldName = 'name'
      Size = 240
    end
    object md1op_in: TBooleanField
      FieldName = 'op_in'
    end
    object md1op_out: TBooleanField
      FieldName = 'op_out'
    end
  end
  object md2: TdxMemData
    Indexes = <>
    SortOptions = []
    Left = 44
    Top = 35
    object md2id: TIntegerField
      FieldName = 'id'
    end
    object md2name: TStringField
      FieldName = 'name'
      Size = 240
    end
    object md2en: TBooleanField
      FieldName = 'en'
    end
  end
  object ds1: TDataSource
    DataSet = md1
    Left = 12
    Top = 67
  end
  object ds2: TDataSource
    DataSet = md2
    Left = 44
    Top = 67
  end
end
