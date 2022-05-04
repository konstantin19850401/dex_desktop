object EAcc: TEAcc
  Left = 240
  Top = 161
  Width = 746
  Height = 436
  Caption = #1057#1095#1105#1090
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  FormStyle = fsMDIChild
  OldCreateOrder = False
  Position = poDefault
  Visible = True
  OnClose = FormClose
  OnShow = FormShow
  PixelsPerInch = 96
  TextHeight = 13
  object Panel1: TPanel
    Left = 0
    Top = 0
    Width = 730
    Height = 49
    Align = alTop
    BevelOuter = bvNone
    TabOrder = 0
    object rsbUsl: TRxSpeedButton
      Left = 8
      Top = 12
      Width = 73
      Height = 25
      DropDownMenu = pmUsl
      Caption = #1059#1089#1083#1086#1074#1080#1103
      Style = bsWin31
    end
    object rsbPrint: TRxSpeedButton
      Left = 88
      Top = 12
      Width = 73
      Height = 25
      Action = aPrintAcc
      Style = bsWin31
    end
    object Label1: TLabel
      Left = 168
      Top = 8
      Width = 84
      Height = 13
      Caption = #1053#1072#1095#1072#1083#1100#1085#1072#1103' '#1076#1072#1090#1072':'
    end
    object Label2: TLabel
      Left = 264
      Top = 8
      Width = 77
      Height = 13
      Caption = #1050#1086#1085#1077#1095#1085#1072#1103' '#1076#1072#1090#1072':'
    end
    object Label3: TLabel
      Left = 408
      Top = 8
      Width = 59
      Height = 13
      Caption = #1048#1085#1076#1080#1082#1072#1094#1080#1103':'
    end
    object Label4: TLabel
      Left = 419
      Top = 26
      Width = 12
      Height = 16
      Caption = #1055
      Font.Charset = DEFAULT_CHARSET
      Font.Color = clNavy
      Font.Height = -13
      Font.Name = 'MS Sans Serif'
      Font.Style = [fsBold]
      ParentFont = False
    end
    object Label5: TLabel
      Left = 431
      Top = 26
      Width = 11
      Height = 16
      Caption = #1056
      Font.Charset = DEFAULT_CHARSET
      Font.Color = clMaroon
      Font.Height = -13
      Font.Name = 'MS Sans Serif'
      Font.Style = [fsBold]
      ParentFont = False
    end
    object Label6: TLabel
      Left = 409
      Top = 26
      Width = 11
      Height = 16
      Caption = #1047
      Font.Charset = DEFAULT_CHARSET
      Font.Color = clTeal
      Font.Height = -13
      Font.Name = 'MS Sans Serif'
      Font.Style = [fsBold]
      ParentFont = False
    end
    object rsbRefreshView: TRxSpeedButton
      Left = 360
      Top = 12
      Width = 25
      Height = 25
      Caption = '>>'
      Style = bsWin31
      OnClick = rsbRefreshViewClick
    end
    object Label7: TLabel
      Left = 442
      Top = 26
      Width = 11
      Height = 16
      Caption = #1042
      Font.Charset = DEFAULT_CHARSET
      Font.Color = clPurple
      Font.Height = -13
      Font.Name = 'MS Sans Serif'
      Font.Style = [fsBold]
      ParentFont = False
    end
    object Label8: TLabel
      Left = 453
      Top = 26
      Width = 11
      Height = 16
      Caption = #1040
      Font.Charset = DEFAULT_CHARSET
      Font.Color = clGreen
      Font.Height = -13
      Font.Name = 'MS Sans Serif'
      Font.Style = [fsBold]
      ParentFont = False
    end
    object Panel3: TPanel
      Left = 624
      Top = 0
      Width = 106
      Height = 49
      Align = alRight
      BevelOuter = bvNone
      TabOrder = 0
      object rsbFit: TRxSpeedButton
        Left = 8
        Top = 8
        Width = 25
        Height = 25
        AllowAllUp = True
        GroupIndex = 5
        Caption = 'F'
        GrayedInactive = False
        Style = bsWin31
        OnClick = rsbFitClick
      end
      object RxSpeedButton1: TRxSpeedButton
        Left = 40
        Top = 8
        Width = 25
        Height = 25
        Action = aBookmark
        Glyph.Data = {
          76010000424D7601000000000000760000002800000020000000100000000100
          04000000000000010000120B0000120B00001000000000000000000000000000
          800000800000008080008000000080008000808000007F7F7F00BFBFBF000000
          FF0000FF000000FFFF00FF000000FF00FF00FFFF0000FFFFFF00555555555555
          55555555FFFFFFFF5555555000000005555555577777777FF555550999999900
          55555575555555775F55509999999901055557F55555557F75F5001111111101
          105577FFFFFFFF7FF75F00000000000011057777777777775F755070FFFFFF0F
          01105777F555557F75F75500FFFFFF0FF0105577F555FF7F57575550FF700008
          8F0055575FF7777555775555000888888F005555777FFFFFFF77555550000000
          0F055555577777777F7F555550FFFFFF0F05555557F5FFF57F7F555550F000FF
          0005555557F777557775555550FFFFFF0555555557F555FF7F55555550FF7000
          05555555575FF777755555555500055555555555557775555555}
        NumGlyphs = 2
        Style = bsWin31
      end
      object rsbExpCsv: TRxSpeedButton
        Left = 72
        Top = 8
        Width = 25
        Height = 25
        Caption = 'E'
        Style = bsWin31
        OnClick = rsbExpCsvClick
      end
    end
    object deStart: TDateEdit
      Left = 168
      Top = 24
      Width = 89
      Height = 21
      NumGlyphs = 2
      TabOrder = 1
      OnAcceptDate = deStartAcceptDate
    end
    object deEnd: TDateEdit
      Left = 264
      Top = 24
      Width = 89
      Height = 21
      NumGlyphs = 2
      TabOrder = 2
    end
  end
  object Panel2: TPanel
    Left = 0
    Top = 49
    Width = 730
    Height = 330
    Align = alClient
    BevelOuter = bvNone
    Caption = #1053#1077#1090' '#1076#1072#1085#1085#1099#1093
    TabOrder = 1
    object dge1: TDBGridEh
      Left = 0
      Top = 0
      Width = 730
      Height = 330
      Align = alClient
      AllowedOperations = []
      AutoFitColWidths = True
      ColumnDefValues.Title.TitleButton = True
      DataSource = ds1
      Flat = True
      Font.Charset = DEFAULT_CHARSET
      Font.Color = clWindowText
      Font.Height = -11
      Font.Name = 'MS Sans Serif'
      Font.Style = []
      FooterColor = clSilver
      FooterFont.Charset = DEFAULT_CHARSET
      FooterFont.Color = clBlack
      FooterFont.Height = -11
      FooterFont.Name = 'MS Sans Serif'
      FooterFont.Style = []
      FooterRowCount = 2
      MinAutoFitWidth = 32
      Options = [dgTitles, dgColumnResize, dgColLines, dgRowLines, dgTabs, dgRowSelect, dgAlwaysShowSelection, dgCancelOnExit]
      OptionsEh = [dghFixed3D, dghFrozen3D, dghFooter3D, dghHighlightFocus, dghClearSelection, dghFitRowHeightToText, dghAutoSortMarking, dghTraceColSizing]
      ParentFont = False
      PopupMenu = pmSub
      RowHeight = 2
      RowLines = 1
      TabOrder = 0
      TitleFont.Charset = DEFAULT_CHARSET
      TitleFont.Color = clWindowText
      TitleFont.Height = -11
      TitleFont.Name = 'MS Sans Serif'
      TitleFont.Style = []
      UseMultiTitle = True
      VertScrollBar.Tracking = True
      VTitleMargin = 4
      OnDblClick = dge1DblClick
      OnGetCellParams = dge1GetCellParams
      OnKeyUp = dge1KeyUp
      OnSortMarkingChanged = dge1SortMarkingChanged
      Columns = <
        item
          EditButtons = <>
          FieldName = 'Date'
          Footers = <>
          Title.Caption = #1044#1072#1090#1072
        end
        item
          EditButtons = <>
          FieldName = 'Name'
          Footers = <>
          Title.Caption = #1050#1083#1080#1077#1085#1090'/'#1089#1095#1077#1090
          Width = 88
        end
        item
          EditButtons = <>
          FieldName = 'Opname'
          Footers = <>
          Title.Caption = #1054#1087#1077#1088#1072#1094#1080#1103
          Width = 86
        end
        item
          EditButtons = <>
          FieldName = 'OptypeS'
          Footers = <>
          Title.Caption = #1058#1080#1087' '#1086#1087#1077#1088#1072#1094#1080#1080
          Width = 24
        end
        item
          EditButtons = <>
          FieldName = 'Prim'
          Footers = <>
          Title.Caption = #1054#1089#1085#1086#1074#1072#1085#1080#1077
          Width = 112
        end
        item
          EditButtons = <>
          FieldName = 'Np'
          Font.Charset = DEFAULT_CHARSET
          Font.Color = clBlack
          Font.Height = -11
          Font.Name = 'MS Sans Serif'
          Font.Style = []
          Footer.Font.Charset = DEFAULT_CHARSET
          Footer.Font.Color = clWhite
          Footer.Font.Height = -11
          Footer.Font.Name = 'MS Sans Serif'
          Footer.Font.Style = [fsBold]
          Footer.ValueType = fvtSum
          Footers = <
            item
              ValueType = fvtStaticText
            end
            item
              ValueType = fvtStaticText
            end>
          Title.Caption = #1053#1072#1083#1080#1095#1085#1099#1077'|'#1055#1088#1080#1093#1086#1076
        end
        item
          EditButtons = <>
          FieldName = 'Nr'
          Font.Charset = DEFAULT_CHARSET
          Font.Color = clBlack
          Font.Height = -11
          Font.Name = 'MS Sans Serif'
          Font.Style = []
          Footer.Font.Charset = DEFAULT_CHARSET
          Footer.Font.Color = clWhite
          Footer.Font.Height = -11
          Footer.Font.Name = 'MS Sans Serif'
          Footer.Font.Style = [fsBold]
          Footer.ValueType = fvtSum
          Footers = <
            item
              ValueType = fvtStaticText
            end
            item
              ValueType = fvtStaticText
            end>
          Title.Caption = #1053#1072#1083#1080#1095#1085#1099#1077'|'#1056#1072#1089#1093#1086#1076
        end
        item
          EditButtons = <>
          FieldName = 'Bp'
          Font.Charset = DEFAULT_CHARSET
          Font.Color = clBlack
          Font.Height = -11
          Font.Name = 'MS Sans Serif'
          Font.Style = []
          Footer.Font.Charset = DEFAULT_CHARSET
          Footer.Font.Color = clWhite
          Footer.Font.Height = -11
          Footer.Font.Name = 'MS Sans Serif'
          Footer.Font.Style = [fsBold]
          Footer.ValueType = fvtSum
          Footers = <
            item
              ValueType = fvtStaticText
            end
            item
              ValueType = fvtStaticText
            end>
          Title.Caption = #1041#1077#1079#1085#1072#1083#1080#1095#1085#1099#1077'|'#1055#1088#1080#1093#1086#1076
        end
        item
          EditButtons = <>
          FieldName = 'Br'
          Font.Charset = DEFAULT_CHARSET
          Font.Color = clBlack
          Font.Height = -11
          Font.Name = 'MS Sans Serif'
          Font.Style = []
          Footer.Font.Charset = DEFAULT_CHARSET
          Footer.Font.Color = clWhite
          Footer.Font.Height = -11
          Footer.Font.Name = 'MS Sans Serif'
          Footer.Font.Style = [fsBold]
          Footer.ValueType = fvtSum
          Footers = <
            item
              ValueType = fvtStaticText
            end
            item
              ValueType = fvtStaticText
            end>
          Title.Caption = #1041#1077#1079#1085#1072#1083#1080#1095#1085#1099#1077'|'#1056#1072#1089#1093#1086#1076
        end
        item
          EditButtons = <>
          FieldName = 'Up'
          Font.Charset = DEFAULT_CHARSET
          Font.Color = clBlack
          Font.Height = -11
          Font.Name = 'MS Sans Serif'
          Font.Style = []
          Footer.Font.Charset = DEFAULT_CHARSET
          Footer.Font.Color = clWhite
          Footer.Font.Height = -11
          Footer.Font.Name = 'MS Sans Serif'
          Footer.Font.Style = [fsBold]
          Footer.ValueType = fvtSum
          Footers = <
            item
              ValueType = fvtStaticText
            end
            item
              ValueType = fvtStaticText
            end>
          Title.Caption = #1042#1072#1083#1102#1090#1072'|'#1055#1088#1080#1093#1086#1076
        end
        item
          EditButtons = <>
          FieldName = 'Ur'
          Footers = <
            item
              ValueType = fvtStaticText
            end
            item
              ValueType = fvtStaticText
            end>
          Title.Caption = #1042#1072#1083#1102#1090#1072'|'#1056#1072#1089#1093#1086#1076
        end
        item
          EditButtons = <>
          FieldName = 'usrcr'
          Footers = <>
          Title.Caption = #1055#1086#1083#1100#1079#1086#1074#1072#1090#1077#1083#1100'|'#1042#1074#1086#1076
        end
        item
          EditButtons = <>
          FieldName = 'usrch'
          Footers = <>
          Title.Caption = #1055#1086#1083#1100#1079#1086#1074#1072#1090#1077#1083#1100'|'#1048#1079#1084#1077#1085#1077#1085#1080#1077
        end>
    end
  end
  object sb: TStatusBar
    Left = 0
    Top = 379
    Width = 730
    Height = 19
    Panels = <>
  end
  object pmUsl: TPopupMenu
    AutoHotkeys = maManual
    Left = 8
    Top = 41
    object N1: TMenuItem
      Tag = 1
      Caption = #1054#1090#1086#1073#1088#1072#1078#1072#1090#1100' '#1089' '#1085#1072#1095#1072#1083#1100#1085#1086#1081' '#1087#1086' '#1082#1086#1085#1077#1095#1085#1091#1102' '#1076#1072#1090#1091
      Checked = True
      GroupIndex = 1
      RadioItem = True
      OnClick = N3Click
    end
    object N2: TMenuItem
      Tag = 2
      Caption = #1054#1090#1086#1073#1088#1072#1079#1080#1090#1100' '#1074#1089#1077' '#1079#1072#1087#1080#1089#1080' '#1089#1095#1105#1090#1072
      GroupIndex = 1
      RadioItem = True
      OnClick = N3Click
    end
    object N3: TMenuItem
      Tag = 3
      Caption = #1054#1090#1086#1073#1088#1072#1079#1080#1090#1100' '#1086#1087#1088#1077#1076#1077#1083#1077#1085#1085#1099#1081' '#1076#1077#1085#1100
      GroupIndex = 1
      RadioItem = True
    end
    object N4: TMenuItem
      Caption = '-'
      GroupIndex = 1
    end
    object N5: TMenuItem
      Caption = #1055#1086#1082#1072#1079#1099#1074#1072#1090#1100' '#1087#1088#1080#1093#1086#1076
      Checked = True
      GroupIndex = 1
      OnClick = N5Click
    end
    object N6: TMenuItem
      Caption = #1055#1086#1082#1072#1079#1099#1074#1072#1090#1100' '#1088#1072#1089#1093#1086#1076
      Checked = True
      GroupIndex = 1
      OnClick = N6Click
    end
    object N8: TMenuItem
      Caption = #1055#1086#1082#1072#1079#1099#1074#1072#1090#1100' '#1086#1087#1077#1088#1072#1094#1080#1080' '#1074#1085#1091#1090#1088#1080' '#1089#1095#1105#1090#1072
      GroupIndex = 1
      OnClick = N8Click
    end
    object N7: TMenuItem
      Caption = #1055#1086#1082#1072#1079#1099#1074#1072#1090#1100' '#1079#1072#1082#1083#1072#1076#1082#1080
      GroupIndex = 1
      OnClick = N7Click
    end
    object N9: TMenuItem
      Caption = #1055#1086#1082#1072#1079#1099#1074#1072#1090#1100' '#1087#1086#1083#1100#1079#1086#1074#1072#1090#1077#1083#1077#1081
      GroupIndex = 1
      OnClick = N9Click
    end
  end
  object md: TRxMemoryData
    FieldDefs = <
      item
        Name = 'ID'
        DataType = ftInteger
      end
      item
        Name = 'bmk_id'
        DataType = ftInteger
      end
      item
        Name = 'Date'
        DataType = ftDateTime
      end
      item
        Name = 'Name'
        DataType = ftString
        Size = 240
      end
      item
        Name = 'Optype'
        DataType = ftBoolean
      end
      item
        Name = 'OptypeS'
        DataType = ftString
        Size = 20
      end
      item
        Name = 'Opname'
        DataType = ftString
        Size = 240
      end
      item
        Name = 'Prim'
        DataType = ftString
        Size = 240
      end
      item
        Name = 'Np'
        DataType = ftCurrency
      end
      item
        Name = 'Nr'
        DataType = ftCurrency
      end
      item
        Name = 'Bp'
        DataType = ftCurrency
      end
      item
        Name = 'Br'
        DataType = ftCurrency
      end
      item
        Name = 'Up'
        DataType = ftCurrency
      end
      item
        Name = 'Ur'
        DataType = ftCurrency
      end
      item
        Name = 'usrcr'
        DataType = ftString
        Size = 32
      end
      item
        Name = 'usrch'
        DataType = ftString
        Size = 32
      end
      item
        Name = 'r_cr'
        DataType = ftString
        Size = 20
      end>
    Left = 8
    Top = 72
    object mdJournal_Id: TIntegerField
      FieldName = 'ID'
    end
    object mdbmk_id: TIntegerField
      FieldName = 'bmk_id'
    end
    object mdDate: TDateTimeField
      FieldName = 'Date'
    end
    object mdName: TStringField
      FieldName = 'Name'
      Size = 240
    end
    object mdOptype: TBooleanField
      FieldName = 'Optype'
    end
    object mdOptypeS: TStringField
      FieldName = 'OptypeS'
    end
    object mdOpname: TStringField
      FieldName = 'Opname'
      Size = 240
    end
    object mdPrim: TStringField
      FieldName = 'Prim'
      Size = 240
    end
    object mdNp: TCurrencyField
      FieldName = 'Np'
    end
    object mdNr: TCurrencyField
      FieldName = 'Nr'
    end
    object mdBp: TCurrencyField
      FieldName = 'Bp'
    end
    object mdBr: TCurrencyField
      FieldName = 'Br'
    end
    object mdUp: TCurrencyField
      FieldName = 'Up'
    end
    object mdUr: TCurrencyField
      FieldName = 'Ur'
    end
    object mdusrcr: TStringField
      FieldName = 'usrcr'
      Size = 32
    end
    object mdusrch: TStringField
      FieldName = 'usrch'
      Size = 32
    end
    object mdr_cr: TStringField
      FieldKind = fkCalculated
      FieldName = 'r_cr'
      Calculated = True
    end
  end
  object ds1: TDataSource
    DataSet = md
    Left = 48
    Top = 72
  end
  object pdb: TPrintDBGridEh
    DBGridEh = dge1
    Options = [pghFitGridToPageWidth, pghColored, pghRowAutoStretch, pghFitingByColWidths, pghOptimalColWidths]
    PageFooter.Font.Charset = DEFAULT_CHARSET
    PageFooter.Font.Color = clWindowText
    PageFooter.Font.Height = -11
    PageFooter.Font.Name = 'MS Sans Serif'
    PageFooter.Font.Style = []
    PageHeader.Font.Charset = DEFAULT_CHARSET
    PageHeader.Font.Color = clWindowText
    PageHeader.Font.Height = -11
    PageHeader.Font.Name = 'MS Sans Serif'
    PageHeader.Font.Style = []
    PrintFontName = 'Arial'
    Units = MM
    Left = 8
    Top = 104
  end
  object dtpm: TPopupMenu
    AutoHotkeys = maManual
    Left = 48
    Top = 40
  end
  object al2: TActionList
    Left = 8
    Top = 137
    object aPrintAcc: TAction
      Caption = 'F8 - '#1055#1077#1095#1072#1090#1100
      ShortCut = 119
      OnExecute = aPrintAccExecute
    end
    object aOrder: TAction
      Caption = #1054#1088#1076#1077#1088
      ShortCut = 116
      OnExecute = aOrderExecute
    end
    object aBookmark: TAction
      Caption = 'aBookmark'
      Hint = #1055#1086#1089#1090#1072#1074#1080#1090#1100' '#1079#1072#1082#1083#1072#1076#1082#1091' '#1085#1072' '#1079#1072#1087#1080#1089#1100
      ShortCut = 16461
    end
    object aIntOrder: TAction
      Caption = #1042#1085#1091#1090#1088#1077#1085#1085#1080#1081' '#1086#1088#1076#1077#1088
      ShortCut = 8308
      OnExecute = aIntOrderExecute
    end
  end
  object pmSub: TPopupMenu
    Left = 88
    Top = 41
    object F51: TMenuItem
      Action = aOrder
    end
    object N10: TMenuItem
      Action = aIntOrder
    end
  end
end
