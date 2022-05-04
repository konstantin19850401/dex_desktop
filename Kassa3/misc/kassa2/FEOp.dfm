object EOp: TEOp
  Left = 294
  Top = 255
  BorderIcons = [biSystemMenu]
  BorderStyle = bsSingle
  Caption = 'Операция'
  ClientHeight = 239
  ClientWidth = 341
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
    Width = 181
    Height = 13
    Caption = 'Наименование кассовой операции:'
  end
  object rsbOK: TRxSpeedButton
    Left = 152
    Top = 208
    Width = 89
    Height = 25
    Caption = 'ОК'
    Style = bsWin31
    OnClick = rsbOKClick
  end
  object rsbCancel: TRxSpeedButton
    Left = 248
    Top = 208
    Width = 89
    Height = 25
    Caption = 'Отмена'
    Style = bsWin31
    OnClick = rsbCancelClick
  end
  object Label5: TLabel
    Left = 8
    Top = 80
    Width = 321
    Height = 57
    Alignment = taCenter
    AutoSize = False
    Caption = 
      'Внимание!'#13#10'Данная кассовая операция уже применялась в текущей ба' +
      'зе. Вы можете изменять только название операции.'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clPurple
    Font.Height = -11
    Font.Name = 'MS Sans Serif'
    Font.Style = [fsBold]
    ParentFont = False
    WordWrap = True
  end
  object ename: TEdit
    Left = 8
    Top = 24
    Width = 329
    Height = 21
    TabOrder = 0
    Text = 'ename'
  end
  object gbBehavior: TGroupBox
    Left = 8
    Top = 48
    Width = 329
    Height = 153
    Caption = 'Поведение'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -11
    Font.Name = 'MS Sans Serif'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 1
    object Label2: TLabel
      Left = 8
      Top = 20
      Width = 73
      Height = 13
      Caption = 'Тип операции:'
      Font.Charset = DEFAULT_CHARSET
      Font.Color = clWindowText
      Font.Height = -11
      Font.Name = 'MS Sans Serif'
      Font.Style = []
      ParentFont = False
    end
    object Bevel1: TBevel
      Left = 8
      Top = 48
      Width = 313
      Height = 9
      Shape = bsTopLine
    end
    object Bevel2: TBevel
      Left = 160
      Top = 48
      Width = 9
      Height = 97
      Shape = bsLeftLine
    end
    object Label3: TLabel
      Left = 8
      Top = 56
      Width = 61
      Height = 13
      Caption = 'Источник:'
      Font.Charset = DEFAULT_CHARSET
      Font.Color = clWindowText
      Font.Height = -11
      Font.Name = 'MS Sans Serif'
      Font.Style = [fsBold]
      ParentFont = False
    end
    object Label4: TLabel
      Left = 168
      Top = 56
      Width = 65
      Height = 13
      Caption = 'Приёмник:'
      Font.Charset = DEFAULT_CHARSET
      Font.Color = clWindowText
      Font.Height = -11
      Font.Name = 'MS Sans Serif'
      Font.Style = [fsBold]
      ParentFont = False
    end
    object cbOpType: TComboBox
      Left = 96
      Top = 16
      Width = 225
      Height = 21
      Style = csDropDownList
      Font.Charset = DEFAULT_CHARSET
      Font.Color = clWindowText
      Font.Height = -11
      Font.Name = 'MS Sans Serif'
      Font.Style = []
      ItemHeight = 13
      ParentFont = False
      TabOrder = 0
      OnChange = cbOpTypeChange
      Items.Strings = (
        'Приход'
        'Расход')
    end
    object cbns: TCheckBox
      Left = 8
      Top = 80
      Width = 97
      Height = 17
      Caption = 'Наличные'
      Font.Charset = DEFAULT_CHARSET
      Font.Color = clWindowText
      Font.Height = -11
      Font.Name = 'MS Sans Serif'
      Font.Style = []
      ParentFont = False
      TabOrder = 1
    end
    object cbbs: TCheckBox
      Left = 8
      Top = 96
      Width = 97
      Height = 17
      Caption = 'Безналичные'
      Font.Charset = DEFAULT_CHARSET
      Font.Color = clWindowText
      Font.Height = -11
      Font.Name = 'MS Sans Serif'
      Font.Style = []
      ParentFont = False
      TabOrder = 2
    end
    object cbus: TCheckBox
      Left = 8
      Top = 112
      Width = 97
      Height = 17
      Caption = 'Валюта'
      Font.Charset = DEFAULT_CHARSET
      Font.Color = clWindowText
      Font.Height = -11
      Font.Name = 'MS Sans Serif'
      Font.Style = []
      ParentFont = False
      TabOrder = 3
    end
    object cbcs: TCheckBox
      Left = 8
      Top = 128
      Width = 97
      Height = 17
      Caption = 'Контрагент'
      Font.Charset = DEFAULT_CHARSET
      Font.Color = clWindowText
      Font.Height = -11
      Font.Name = 'MS Sans Serif'
      Font.Style = []
      ParentFont = False
      TabOrder = 4
    end
    object cbnd: TCheckBox
      Left = 168
      Top = 80
      Width = 97
      Height = 17
      Caption = 'Наличные'
      Font.Charset = DEFAULT_CHARSET
      Font.Color = clWindowText
      Font.Height = -11
      Font.Name = 'MS Sans Serif'
      Font.Style = []
      ParentFont = False
      TabOrder = 5
    end
    object cbbd: TCheckBox
      Left = 168
      Top = 96
      Width = 97
      Height = 17
      Caption = 'Безналичные'
      Font.Charset = DEFAULT_CHARSET
      Font.Color = clWindowText
      Font.Height = -11
      Font.Name = 'MS Sans Serif'
      Font.Style = []
      ParentFont = False
      TabOrder = 6
    end
    object cbud: TCheckBox
      Left = 168
      Top = 112
      Width = 97
      Height = 17
      Caption = 'Валюта'
      Font.Charset = DEFAULT_CHARSET
      Font.Color = clWindowText
      Font.Height = -11
      Font.Name = 'MS Sans Serif'
      Font.Style = []
      ParentFont = False
      TabOrder = 7
    end
    object cbcd: TCheckBox
      Left = 168
      Top = 128
      Width = 97
      Height = 17
      Caption = 'Контрагент'
      Font.Charset = DEFAULT_CHARSET
      Font.Color = clWindowText
      Font.Height = -11
      Font.Name = 'MS Sans Serif'
      Font.Style = []
      ParentFont = False
      TabOrder = 8
    end
  end
end
