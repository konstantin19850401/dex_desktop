object ECurs: TECurs
  Left = 356
  Top = 169
  BorderIcons = [biSystemMenu]
  BorderStyle = bsSingle
  Caption = 'Текущий курс УЕ'
  ClientHeight = 89
  ClientWidth = 170
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
    Top = 8
    Width = 99
    Height = 13
    Caption = 'В одной УЕ рублей:'
  end
  object ceCurs: TCurrencyEdit
    Left = 8
    Top = 24
    Width = 153
    Height = 21
    AutoSize = False
    TabOrder = 0
  end
  object bOk: TButton
    Left = 8
    Top = 56
    Width = 75
    Height = 25
    Caption = 'ОК'
    Default = True
    ModalResult = 1
    TabOrder = 1
  end
  object bCancel: TButton
    Left = 88
    Top = 56
    Width = 75
    Height = 25
    Cancel = True
    Caption = 'Отмена'
    ModalResult = 2
    TabOrder = 2
  end
end
