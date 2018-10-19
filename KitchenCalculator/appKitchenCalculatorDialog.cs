namespace KitchenCalculator
{
    using Microsoft.Win32;
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.Windows.Forms;

    public class appKitchenCalculatorDialog : Form
    {
        private ToolStripMenuItem AbbreviateUnitsToolStripMenuItem;
        private ToolStripMenuItem AboutToolStripMenuItem;
        private Button AddButton;
        private ToolStripMenuItem AustraliaToolStripMenuItem;
        private Button BackspaceButton;
        private Button CalorieButton;
        private ToolStripMenuItem CanadaToolStripMenuItem;
        private Button ClearButton;
        private Button ClearEntryButton;
        private IContainer components;
        private ToolStripMenuItem CopyAllToolStripMenuItem;
        private ToolStripMenuItem CopyToolStripMenuItem;
        private Button CupButton;
        private Button DegreeCButton;
        private Button DegreeFButton;
        private Label DisplayLabel;
        private Button DivideButton;
        private ToolStripMenuItem EditToolStripMenuItem;
        private Button EightButton;
        private Button EqualsButton;
        private Button FiveButton;
        private Button FluidOunceButton;
        private Button FourButton;
        private Button GallonButton;
        private Button GramButton;
        private readonly ToolStripMenuItem HelpToolStripMenuItem;
        private readonly ToolStripMenuItem HelpTopicsToolStripMenuItem;
        private bool ibAbbreviateUnits;
        private bool ibError;
        private bool ibKeepOnTop = true;
        private decimal idcMemoryValue;
        private decimal idcSessionValue;
        private appUnit ieCurrentEntryUnit;
        private appOperator ieCurrentOperator;
        private appDisplayMode ieDisplayMode;
        private appUnit ieMemoryUnit;
        private appOperator ieSessionOperator;
        private appUnit ieSessionUnit;
        private appUnitMode ieSessionUnitMode;
        private appUnitSystem ieUnitSystem;
        private string isCurrentEntry = "";
        private readonly string isUserKey;
        private ToolStripMenuItem KeepOnTopToolStripMenuItem;
        private Button KilogramButton;
        private Button KilojouleButton;
        private Button LiterButton;
        private Button MemoryAddButton;
        private Button MemoryClearButton;
        private Label MemoryLabel;
        private Button MemoryRecallButton;
        private Button MemorySaveButton;
        private Button MilligramButton;
        private Button MilliliterButton;
        private Button MultiplyButton;
        private ToolStripMenuItem NewZealandToolStripMenuItem;
        private Button NineButton;
        private Button OneButton;
        private ToolStripMenuItem OptionsToolStripMenuItem;
        private Button OunceButton;
        private ToolStripMenuItem PasteToolStripMenuItem;
        private Button PeriodButton;
        private Button PintButton;
        private Button PlusMinusButton;
        private Button PoundButton;
        private Button QuartButton;
        private RadioButton radioButton1;
        private RadioButton radioButton2;
        private RadioButton radioButton3;
        private ComboBox rfcComboBox5;
        private Button SevenButton;
        private Button SixButton;
        private Button SubtractButton;
        private Button TablespoonButton;
        private Button TeaspoonButton;
        private Button ThreeButton;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripSeparator toolStripMenuItem2;
        private Button TwoButton;
        private ToolStripMenuItem UKToolStripMenuItem;
        private ToolStripMenuItem USToolStripMenuItem;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ToolStripStatusLabel toolStripStatusLabel2;
        private Button ZeroButton;

        public appKitchenCalculatorDialog()
        {
            InitializeComponent();
            isUserKey = @"HKEY_CURRENT_USER\Software\lolliesoft\Pub Grub Calculator\1.0";
        }

        private void AbbreviateUnitsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ibAbbreviateUnits = !ibAbbreviateUnits;
                AbbreviateUnitsToolStripMenuItem.Checked = ibAbbreviateUnits;
                RefreshDisplay();
                SaveUserOptions();
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                new appAboutDialog().Show();
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void Add()
        {
            try
            {
                if (((!ibError && (ieCurrentOperator == appOperator.None)) && (idcCurrentEntryValue != 0M)) && (((ieSessionOperator != appOperator.Add) && (ieSessionOperator != appOperator.Subtract)) || ((ieSessionUnitMode == appUnitMode.None) || (ieCurrentEntryUnit != appUnit.None))))
                {
                    if (ieSessionUnitMode == appUnitMode.None)
                    {
                        SetSessionUnitMode(GetUnitMode(ieCurrentEntryUnit));
                    }
                    ProcessSessionOperation();
                    ieCurrentOperator = appOperator.Add;
                    ieSessionOperator = ieCurrentOperator;
                    EnableControls();
                    RefreshDisplay();
                }
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            try
            {
                Add();
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void AddDecimal()
        {
            try
            {
                if (!ibError && ((ieDisplayMode != appDisplayMode.DataEntry) || (ieCurrentEntryUnit == appUnit.None)))
                {
                    if (ieDisplayMode == appDisplayMode.Total)
                    {
                        SetSessionUnitMode(appUnitMode.None);
                        idcSessionValue = 0M;
                        ieSessionUnit = appUnit.None;
                        ieSessionOperator = appOperator.None;
                        EnableVolumeControls(true);
                        EnableWeightControls(true);
                        EnableTemperatureControls(true, true);
                        EnableEnergyControls(true);
                    }
                    if (ieDisplayMode != appDisplayMode.DataEntry)
                    {
                        isCurrentEntry = "";
                        ieCurrentEntryUnit = appUnit.None;
                        ieCurrentOperator = appOperator.None;
                        ieDisplayMode = appDisplayMode.DataEntry;
                    }
                    if (isCurrentEntry.IndexOf(NumberFormatInfo.CurrentInfo.NumberDecimalSeparator) < 0)
                    {
                        isCurrentEntry = isCurrentEntry + NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
                        RefreshDisplay();
                    }
                }
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private decimal AddEnergies(decimal adcFirstValue, appUnit aeFirstUnit, decimal adcSecondValue, appUnit aeSecondUnit, out appUnit aeResultUnit)
        {
            decimal adcKiloJoules = 0M;
            try
            {
                decimal num2 = ConvertToKiloJoules(adcFirstValue, aeFirstUnit);
                decimal num3 = ConvertToKiloJoules(adcSecondValue, aeSecondUnit);
                adcKiloJoules = num2 + num3;
                adcKiloJoules = ConvertFromKiloJoules(adcKiloJoules, aeFirstUnit);
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
            aeResultUnit = aeFirstUnit;
            return adcKiloJoules;
        }

        private decimal AddTemperatures(decimal adcFirstValue, appUnit aeFirstUnit, decimal adcSecondValue, appUnit aeSecondUnit, out appUnit aeResultUnit)
        {
            decimal num = 0M;
            try
            {
                num = adcFirstValue + adcSecondValue;
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
            aeResultUnit = aeFirstUnit;
            return num;
        }

        private decimal AddVolumes(decimal adcFirstValue, appUnit aeFirstUnit, decimal adcSecondValue, appUnit aeSecondUnit, out appUnit aeResultUnit)
        {
            decimal adcLiters = 0M;
            try
            {
                decimal num2 = ConvertToLiters(adcFirstValue, aeFirstUnit);
                decimal num3 = ConvertToLiters(adcSecondValue, aeSecondUnit);
                adcLiters = num2 + num3;
                adcLiters = ConvertFromLiters(adcLiters, aeFirstUnit);
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
            aeResultUnit = aeFirstUnit;
            return adcLiters;
        }

        private decimal AddWeights(decimal adcFirstValue, appUnit aeFirstUnit, decimal adcSecondValue, appUnit aeSecondUnit, out appUnit aeResultUnit)
        {
            decimal adcGrams = 0M;
            try
            {
                decimal num2 = ConvertToGrams(adcFirstValue, aeFirstUnit);
                decimal num3 = ConvertToGrams(adcSecondValue, aeSecondUnit);
                adcGrams = num2 + num3;
                adcGrams = ConvertFromGrams(adcGrams, aeFirstUnit);
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
            aeResultUnit = aeFirstUnit;
            return adcGrams;
        }

        private void appKitchenCalculatorDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void appKitchenCalculatorDialog_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                Keys modifiers = e.Modifiers;
                if (modifiers != Keys.None)
                {
                    if (modifiers != Keys.Shift)
                    {
                        if (modifiers != Keys.Control)
                        {
                            return;
                        }
                        goto Label_02C3;
                    }
                }
                else
                {
                    switch (e.KeyCode)
                    {
                        case Keys.Back:
                            Backspace();
                            e.Handled = true;
                            return;

                        case Keys.Enter:
                            Total();
                            return;

                        case Keys.Escape:
                            Clear();
                            return;

                        case Keys.Delete:
                            ClearEntry();
                            e.Handled = true;
                            return;

                        case Keys.D0:
                        case Keys.NumPad0:
                            DigitButton(0);
                            e.Handled = true;
                            return;

                        case Keys.D1:
                        case Keys.NumPad1:
                            DigitButton(1);
                            e.Handled = true;
                            return;

                        case Keys.D2:
                        case Keys.NumPad2:
                            DigitButton(2);
                            e.Handled = true;
                            return;

                        case Keys.D3:
                        case Keys.NumPad3:
                            DigitButton(3);
                            e.Handled = true;
                            return;

                        case Keys.D4:
                        case Keys.NumPad4:
                            DigitButton(4);
                            e.Handled = true;
                            return;

                        case Keys.D5:
                        case Keys.NumPad5:
                            DigitButton(5);
                            e.Handled = true;
                            return;

                        case Keys.D6:
                        case Keys.NumPad6:
                            DigitButton(6);
                            e.Handled = true;
                            return;

                        case Keys.D7:
                        case Keys.NumPad7:
                            DigitButton(7);
                            e.Handled = true;
                            return;

                        case Keys.D8:
                        case Keys.NumPad8:
                            DigitButton(8);
                            e.Handled = true;
                            return;

                        case Keys.D9:
                        case Keys.NumPad9:
                            DigitButton(9);
                            e.Handled = true;
                            return;

                        case Keys.Multiply:
                            Multiply();
                            return;

                        case Keys.Add:
                            Add();
                            e.Handled = true;
                            return;

                        case Keys.Subtract:
                            Subtract();
                            e.Handled = true;
                            return;

                        case Keys.Decimal:
                        case Keys.Oemcomma:
                        case Keys.OemPeriod:
                            AddDecimal();
                            return;

                        case Keys.Divide:
                            Divide();
                            return;

                        case Keys.F9:
                            PlusMinus();
                            return;
                    }
                    if (e.KeyValue == 0xbd)
                    {
                        Subtract();
                    }
                    else if (e.KeyValue == 0xbf)
                    {
                        Divide();
                    }
                    else if (e.KeyValue == 0xbb)
                    {
                        Total();
                    }
                    return;
                }
                switch (e.KeyValue)
                {
                    case 0x38:
                        Multiply();
                        break;

                    case 0xbb:
                        Add();
                        break;
                }
                return;
            Label_02C3:
                switch (e.KeyCode)
                {
                    case Keys.P:
                        MemoryAdd();
                        return;

                    case Keys.Q:
                        return;

                    case Keys.R:
                        MemoryRecall();
                        return;

                    case Keys.S:
                        MemorySave();
                        return;

                    case Keys.L:
                        MemoryClear();
                        return;
                }
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void Backspace()
        {
            try
            {
                if ((!ibError && (ieDisplayMode == appDisplayMode.DataEntry)) && ((ieCurrentEntryUnit == appUnit.None) && (isCurrentEntry != "")))
                {
                    isCurrentEntry = isCurrentEntry.Substring(0, isCurrentEntry.Length - 1);
                    RefreshDisplay();
                }
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void BackspaceButton_Click(object sender, EventArgs e)
        {
            try
            {
                Backspace();
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void Button_Enter(object sender, EventArgs e)
        {
            try
            {
                DisplayLabel.Focus();
                base.ActiveControl = null;
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void CalorieButton_Click(object sender, EventArgs e)
        {
            try
            {
                UnitButton(appUnit.Calorie);
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void Clear()
        {
            try
            {
                ibError = false;
                isCurrentEntry = "";
                ieCurrentEntryUnit = appUnit.None;
                ieCurrentOperator = appOperator.None;
                SetSessionUnitMode(appUnitMode.None);
                idcSessionValue = 0M;
                ieSessionUnit = appUnit.None;
                ieSessionOperator = appOperator.None;
                ieDisplayMode = appDisplayMode.DataEntry;
                EnableControls();
                RefreshDisplay();
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            try
            {
                Clear();
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void ClearEntry()
        {
            try
            {
                ibError = false;
                if (ieDisplayMode == appDisplayMode.Total)
                {
                    isCurrentEntry = "";
                    ieCurrentEntryUnit = appUnit.None;
                    ieCurrentOperator = appOperator.None;
                    SetSessionUnitMode(appUnitMode.None);
                    idcSessionValue = 0M;
                    ieSessionUnit = appUnit.None;
                    ieSessionOperator = appOperator.None;
                    EnableVolumeControls(true);
                    EnableWeightControls(true);
                    EnableTemperatureControls(true, true);
                    EnableEnergyControls(true);
                }
                else
                {
                    isCurrentEntry = "";
                    ieCurrentEntryUnit = appUnit.None;
                    ieCurrentOperator = appOperator.None;
                    ieDisplayMode = appDisplayMode.DataEntry;
                    EnableControls();
                }
                RefreshDisplay();
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void ClearEntryButton_Click(object sender, EventArgs e)
        {
            try
            {
                ClearEntry();
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private decimal ConvertFromGrams(decimal adcGrams, appUnit aeUnit)
        {
            decimal num = 0M;
            try
            {
                switch (aeUnit)
                {
                    case appUnit.Ounce:
                        return (adcGrams / 28.3495231M);

                    case appUnit.Pound:
                        return (adcGrams / 453.59237M);

                    case appUnit.Milligram:
                        return (adcGrams * 1000M);

                    case appUnit.Gram:
                        return adcGrams;

                    case appUnit.Kilogram:
                        return (adcGrams / 1000M);
                }
                return num;
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
            return num;
        }

        private decimal ConvertFromKelvin(decimal adcKelvin, appUnit aeUnit)
        {
            decimal num = 0M;
            try
            {
                switch (aeUnit)
                {
                    case appUnit.DegreeF:
                        return (((adcKelvin - 273.15M) / 0.5555555555M) + 32M);

                    case appUnit.DegreeC:
                        return (adcKelvin - 273.15M);
                }
                return num;
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
            return num;
        }

        private decimal ConvertFromKiloJoules(decimal adcKiloJoules, appUnit aeUnit)
        {
            decimal num = 0M;
            try
            {
                switch (aeUnit)
                {
                    case appUnit.Calorie:
                        return (adcKiloJoules / 4.184M);

                    case appUnit.Kilojoule:
                        return adcKiloJoules;
                }
                return num;
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
            return num;
        }

        private decimal ConvertFromLiters(decimal adcLiters, appUnit aeUnit)
        {
            decimal num = 0M;
            try
            {
                switch (ieUnitSystem)
                {
                    case appUnitSystem.US:
                        switch (aeUnit)
                        {
                            case appUnit.Teaspoon:
                                return (adcLiters / 0.00492892161M);

                            case appUnit.Tablespoon:
                                return (adcLiters / 0.0147867648M);

                            case appUnit.FluidOunce:
                                return (adcLiters / 0.0295735297M);

                            case appUnit.Cup:
                                return (adcLiters / 0.236588237M);

                            case appUnit.Pint:
                                return (adcLiters / 0.473176475M);

                            case appUnit.Quart:
                                return (adcLiters / 0.94635295M);

                            case appUnit.Gallon:
                                return (adcLiters / 3.7854118M);

                            case appUnit.Milliliter:
                                return (adcLiters * 1000M);

                            case appUnit.Liter:
                                return adcLiters;
                        }
                        return num;

                    case appUnitSystem.UK:
                        switch (aeUnit)
                        {
                            case appUnit.Teaspoon:
                                return (adcLiters / 0.00355163M);

                            case appUnit.Tablespoon:
                                return (adcLiters / 0.0142065M);

                            case appUnit.FluidOunce:
                                return (adcLiters / 0.028413M);

                            case appUnit.Cup:
                                return (adcLiters / 0.2841306M);

                            case appUnit.Pint:
                                return (adcLiters / 0.568261M);

                            case appUnit.Quart:
                                return (adcLiters / 1.136522M);

                            case appUnit.Gallon:
                                return (adcLiters / 4.54609M);

                            case appUnit.Milliliter:
                                return (adcLiters * 1000M);

                            case appUnit.Liter:
                                return adcLiters;
                        }
                        return num;

                    case appUnitSystem.Australia:
                        switch (aeUnit)
                        {
                            case appUnit.Teaspoon:
                                return (adcLiters / 0.005M);

                            case appUnit.Tablespoon:
                                return (adcLiters / 0.02M);

                            case appUnit.FluidOunce:
                                return (adcLiters / 0.0295735297M);

                            case appUnit.Cup:
                                return (adcLiters / 0.25M);

                            case appUnit.Pint:
                                return (adcLiters / 0.473176475M);

                            case appUnit.Quart:
                                return (adcLiters / 0.94635295M);

                            case appUnit.Gallon:
                                return (adcLiters / 3.7854118M);

                            case appUnit.Milliliter:
                                return (adcLiters * 1000M);

                            case appUnit.Liter:
                                return adcLiters;
                        }
                        return num;

                    case appUnitSystem.NewZealand:
                    case appUnitSystem.Canada:
                        switch (aeUnit)
                        {
                            case appUnit.Teaspoon:
                                return (adcLiters / 0.005M);

                            case appUnit.Tablespoon:
                                return (adcLiters / 0.015M);

                            case appUnit.FluidOunce:
                                return (adcLiters / 0.0295735297M);

                            case appUnit.Cup:
                                return (adcLiters / 0.25M);

                            case appUnit.Pint:
                                return (adcLiters / 0.473176475M);

                            case appUnit.Quart:
                                return (adcLiters / 0.94635295M);

                            case appUnit.Gallon:
                                return (adcLiters / 3.7854118M);

                            case appUnit.Milliliter:
                                return (adcLiters * 1000M);
                        }
                        return num;

                    default:
                        return num;
                }
                num = adcLiters;
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
            return num;
        }

        private decimal ConvertToGrams(decimal adcValue, appUnit aeUnit)
        {
            decimal num = 0M;
            try
            {
                switch (aeUnit)
                {
                    case appUnit.Ounce:
                        return (adcValue * 28.3495231M);

                    case appUnit.Pound:
                        return (adcValue * 453.59237M);

                    case appUnit.Milligram:
                        return (adcValue / 1000M);

                    case appUnit.Gram:
                        return adcValue;

                    case appUnit.Kilogram:
                        return (adcValue * 1000M);
                }
                return num;
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
            return num;
        }

        private decimal ConvertToKelvin(decimal adcValue, appUnit aeUnit)
        {
            decimal num = 0M;
            try
            {
                switch (aeUnit)
                {
                    case appUnit.DegreeF:
                        return (((adcValue - 32M) * 0.5555555555M) + 273.15M);

                    case appUnit.DegreeC:
                        return (adcValue + 273.15M);
                }
                return num;
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
            return num;
        }

        private decimal ConvertToKiloJoules(decimal adcValue, appUnit aeUnit)
        {
            decimal num = 0M;
            try
            {
                switch (aeUnit)
                {
                    case appUnit.Calorie:
                        return (adcValue * 4.184M);

                    case appUnit.Kilojoule:
                        return adcValue;
                }
                return num;
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
            return num;
        }

        private decimal ConvertToLiters(decimal adcValue, appUnit aeUnit)
        {
            decimal num = 0M;
            try
            {
                switch (ieUnitSystem)
                {
                    case appUnitSystem.US:
                        switch (aeUnit)
                        {
                            case appUnit.Teaspoon:
                                return (adcValue * 0.00492892161M);

                            case appUnit.Tablespoon:
                                return (adcValue * 0.0147867648M);

                            case appUnit.FluidOunce:
                                return (adcValue * 0.0295735297M);

                            case appUnit.Cup:
                                return (adcValue * 0.236588237M);

                            case appUnit.Pint:
                                return (adcValue * 0.473176475M);

                            case appUnit.Quart:
                                return (adcValue * 0.94635295M);

                            case appUnit.Gallon:
                                return (adcValue * 3.7854118M);

                            case appUnit.Milliliter:
                                return (adcValue / 1000M);

                            case appUnit.Liter:
                                return adcValue;
                        }
                        return num;

                    case appUnitSystem.UK:
                        switch (aeUnit)
                        {
                            case appUnit.Teaspoon:
                                return (adcValue * 0.00355163M);

                            case appUnit.Tablespoon:
                                return (adcValue * 0.0142065M);

                            case appUnit.FluidOunce:
                                return (adcValue * 0.028413M);

                            case appUnit.Cup:
                                return (adcValue * 0.2841306M);

                            case appUnit.Pint:
                                return (adcValue * 0.568261M);

                            case appUnit.Quart:
                                return (adcValue * 1.136522M);

                            case appUnit.Gallon:
                                return (adcValue * 4.54609M);

                            case appUnit.Milliliter:
                                return (adcValue / 1000M);

                            case appUnit.Liter:
                                return adcValue;
                        }
                        return num;

                    case appUnitSystem.Australia:
                        switch (aeUnit)
                        {
                            case appUnit.Teaspoon:
                                return (adcValue * 0.005M);

                            case appUnit.Tablespoon:
                                return (adcValue * 0.02M);

                            case appUnit.FluidOunce:
                                return (adcValue * 0.0295735297M);

                            case appUnit.Cup:
                                return (adcValue * 0.25M);

                            case appUnit.Pint:
                                return (adcValue * 0.473176475M);

                            case appUnit.Quart:
                                return (adcValue * 0.94635295M);

                            case appUnit.Gallon:
                                return (adcValue * 3.7854118M);

                            case appUnit.Milliliter:
                                return (adcValue / 1000M);

                            case appUnit.Liter:
                                return adcValue;
                        }
                        return num;

                    case appUnitSystem.NewZealand:
                    case appUnitSystem.Canada:
                        switch (aeUnit)
                        {
                            case appUnit.Teaspoon:
                                return (adcValue * 0.005M);

                            case appUnit.Tablespoon:
                                return (adcValue * 0.015M);

                            case appUnit.FluidOunce:
                                return (adcValue * 0.0295735297M);

                            case appUnit.Cup:
                                return (adcValue * 0.25M);

                            case appUnit.Pint:
                                return (adcValue * 0.473176475M);

                            case appUnit.Quart:
                                return (adcValue * 0.94635295M);

                            case appUnit.Gallon:
                                return (adcValue * 3.7854118M);

                            case appUnit.Milliliter:
                                return (adcValue / 1000M);
                        }
                        return num;

                    default:
                        return num;
                }
                num = adcValue;
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
            return num;
        }

        private void ConvertUnit(appUnit aeUnit)
        {
            decimal num2 = 0M;
            try
            {
                switch (GetUnitMode(aeUnit))
                {
                    case appUnitMode.Volume:
                        {
                            decimal adcLiters = ConvertToLiters(idcCurrentEntryValue, ieCurrentEntryUnit);
                            num2 = ConvertFromLiters(adcLiters, aeUnit);
                            break;
                        }
                    case appUnitMode.Weight:
                        {
                            decimal adcGrams = ConvertToGrams(idcCurrentEntryValue, ieCurrentEntryUnit);
                            num2 = ConvertFromGrams(adcGrams, aeUnit);
                            break;
                        }
                    case appUnitMode.Temperature:
                        {
                            decimal adcKelvin = ConvertToKelvin(idcCurrentEntryValue, ieCurrentEntryUnit);
                            num2 = ConvertFromKelvin(adcKelvin, aeUnit);
                            break;
                        }
                    case appUnitMode.Energy:
                        {
                            decimal adcKiloJoules = ConvertToKiloJoules(idcCurrentEntryValue, ieCurrentEntryUnit);
                            num2 = ConvertFromKiloJoules(adcKiloJoules, aeUnit);
                            break;
                        }
                }
                idcSessionValue = num2;
                ieSessionUnit = aeUnit;
                ieDisplayMode = appDisplayMode.SessionData;
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void Copy()
        {
            DataObject data = new DataObject();
            string isCurrentEntry = "";
            try
            {
                if ((ieDisplayMode == appDisplayMode.DataEntry) || (ieDisplayMode == appDisplayMode.Total))
                {
                    if ((this.isCurrentEntry == "") || (this.isCurrentEntry == "."))
                    {
                        isCurrentEntry = "0";
                    }
                    else if (this.isCurrentEntry.StartsWith("."))
                    {
                        isCurrentEntry = "0" + this.isCurrentEntry;
                    }
                    else
                    {
                        isCurrentEntry = this.isCurrentEntry;
                    }
                }
                else
                {
                    isCurrentEntry = rfcDataService.FormatDecimal(idcSessionValue, 5);
                }
                data.SetData(DataFormats.Text, isCurrentEntry);
                Clipboard.SetDataObject(data);
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void CopyAll()
        {
            DataObject data = new DataObject();
            string isCurrentEntry = "";
            try
            {
                if ((ieDisplayMode == appDisplayMode.DataEntry) || (ieDisplayMode == appDisplayMode.Total))
                {
                    if ((this.isCurrentEntry == "") || (this.isCurrentEntry == "."))
                    {
                        isCurrentEntry = "0";
                    }
                    else if (this.isCurrentEntry.StartsWith("."))
                    {
                        isCurrentEntry = "0" + this.isCurrentEntry;
                    }
                    else
                    {
                        isCurrentEntry = this.isCurrentEntry;
                    }
                    if (ieCurrentEntryUnit != appUnit.None)
                    {
                        isCurrentEntry = isCurrentEntry + " " + GetUnitText(ieCurrentEntryUnit, idcCurrentEntryValue);
                    }
                }
                else
                {
                    isCurrentEntry = rfcDataService.FormatDecimal(idcSessionValue, 5);
                    if (ieSessionUnit != appUnit.None)
                    {
                        isCurrentEntry = isCurrentEntry + " " + GetUnitText(ieSessionUnit, idcSessionValue);
                    }
                }
                data.SetData(DataFormats.Text, isCurrentEntry);
                Clipboard.SetDataObject(data);
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void CopyAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                CopyAll();
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Copy();
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void CupButton_Click(object sender, EventArgs e)
        {
            try
            {
                UnitButton(appUnit.Cup);
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void DegreeCButton_Click(object sender, EventArgs e)
        {
            try
            {
                UnitButton(appUnit.DegreeC);
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void DegreeFButton_Click(object sender, EventArgs e)
        {
            try
            {
                UnitButton(appUnit.DegreeF);
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void DigitButton(int aiDigit)
        {
            try
            {
                if (!ibError && ((ieDisplayMode != appDisplayMode.DataEntry) || (ieCurrentEntryUnit == appUnit.None)))
                {
                    if (ieDisplayMode == appDisplayMode.Total)
                    {
                        SetSessionUnitMode(appUnitMode.None);
                        idcSessionValue = 0M;
                        ieSessionUnit = appUnit.None;
                        ieSessionOperator = appOperator.None;
                        EnableVolumeControls(true);
                        EnableWeightControls(true);
                        EnableTemperatureControls(true, true);
                        EnableEnergyControls(true);
                    }
                    if (ieDisplayMode != appDisplayMode.DataEntry)
                    {
                        isCurrentEntry = "";
                        ieCurrentEntryUnit = appUnit.None;
                        ieCurrentOperator = appOperator.None;
                        ieDisplayMode = appDisplayMode.DataEntry;
                    }
                    if (((aiDigit != 0) || (isCurrentEntry != "")) && (GetCurrentEntryLength() < 15))
                    {
                        isCurrentEntry = isCurrentEntry + aiDigit.ToString();
                        RefreshDisplay();
                    }
                }
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void Divide()
        {
            try
            {
                if (((!ibError && (ieCurrentOperator == appOperator.None)) && (idcCurrentEntryValue != 0M)) && (((ieSessionOperator != appOperator.Add) && (ieSessionOperator != appOperator.Subtract)) || ((ieSessionUnitMode == appUnitMode.None) || (ieCurrentEntryUnit != appUnit.None))))
                {
                    if (ieSessionUnitMode == appUnitMode.None)
                    {
                        SetSessionUnitMode(GetUnitMode(ieCurrentEntryUnit));
                    }
                    ProcessSessionOperation();
                    if (!ibError)
                    {
                        ieCurrentOperator = appOperator.Divide;
                        ieSessionOperator = ieCurrentOperator;
                        EnableControls();
                        RefreshDisplay();
                    }
                }
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void DivideButton_Click(object sender, EventArgs e)
        {
            try
            {
                Divide();
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private decimal DivideEnergies(decimal adcFirstValue, appUnit aeFirstUnit, decimal adcSecondValue, out appUnit aeResultUnit)
        {
            decimal num = 0M;
            appUnit none = appUnit.None;
            try
            {
                num = adcFirstValue / adcSecondValue;
                none = aeFirstUnit;
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
            aeResultUnit = none;
            return num;
        }

        private decimal DivideVolumes(decimal adcFirstValue, appUnit aeFirstUnit, decimal adcSecondValue, out appUnit aeResultUnit)
        {
            decimal num = 0M;
            appUnit none = appUnit.None;
            try
            {
                num = adcFirstValue / adcSecondValue;
                none = aeFirstUnit;
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
            aeResultUnit = none;
            return num;
        }

        private decimal DivideWeights(decimal adcFirstValue, appUnit aeFirstUnit, decimal adcSecondValue, out appUnit aeResultUnit)
        {
            decimal num = 0M;
            appUnit none = appUnit.None;
            try
            {
                num = adcFirstValue / adcSecondValue;
                none = aeFirstUnit;
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
            aeResultUnit = none;
            return num;
        }

        private void EightButton_Click(object sender, EventArgs e)
        {
            try
            {
                DigitButton(8);
            }
            catch (Exception exception)
            {
                HandleTerminalError(exception);
            }
        }

        private void EnableControls()
        {
            try
            {
                switch (ieSessionOperator)
                {
                    case appOperator.None:
                        EnableVolumeControls(true);
                        EnableWeightControls(true);
                        EnableTemperatureControls(true, true);
                        EnableEnergyControls(true);
                        goto Label_01F7;

                    case appOperator.Add:
                    case appOperator.Subtract:
                        switch (ieSessionUnitMode)
                        {
                            case appUnitMode.Volume:
                                goto Label_0090;

                            case appUnitMode.Weight:
                                goto Label_00B2;

                            case appUnitMode.Temperature:
                                goto Label_00D4;

                            case appUnitMode.Energy:
                                goto Label_010D;
                        }
                        goto Label_01F7;

                    case appOperator.Multiply:
                        switch (ieSessionUnitMode)
                        {
                            case appUnitMode.None:
                                goto Label_0155;

                            case appUnitMode.Volume:
                            case appUnitMode.Weight:
                            case appUnitMode.Temperature:
                            case appUnitMode.Energy:
                                goto Label_0177;
                        }
                        goto Label_01F7;

                    case appOperator.Divide:
                        switch (ieSessionUnitMode)
                        {
                            case appUnitMode.None:
                                goto Label_01BB;

                            case appUnitMode.Volume:
                            case appUnitMode.Weight:
                            case appUnitMode.Temperature:
                            case appUnitMode.Energy:
                                goto Label_01DA;
                        }
                        goto Label_01F7;

                    default:
                        goto Label_01F7;
                }
                EnableVolumeControls(false);
                EnableWeightControls(false);
                EnableTemperatureControls(false, false);
                EnableEnergyControls(false);
                goto Label_01F7;
            Label_0090:
                EnableVolumeControls(true);
                EnableWeightControls(false);
                EnableTemperatureControls(false, false);
                EnableEnergyControls(false);
                goto Label_01F7;
            Label_00B2:
                EnableVolumeControls(false);
                EnableWeightControls(true);
                EnableTemperatureControls(false, false);
                EnableEnergyControls(false);
                goto Label_01F7;
            Label_00D4:
                EnableVolumeControls(false);
                EnableWeightControls(false);
                EnableEnergyControls(false);
                if (ieSessionUnit == appUnit.DegreeF)
                {
                    EnableTemperatureControls(true, false);
                }
                else
                {
                    EnableTemperatureControls(false, true);
                }
                goto Label_01F7;
            Label_010D:
                EnableVolumeControls(false);
                EnableWeightControls(false);
                EnableTemperatureControls(false, false);
                EnableEnergyControls(true);
                goto Label_01F7;
            Label_0155:
                EnableVolumeControls(true);
                EnableWeightControls(true);
                EnableTemperatureControls(false, false);
                EnableEnergyControls(true);
                goto Label_01F7;
            Label_0177:
                EnableVolumeControls(false);
                EnableWeightControls(false);
                EnableTemperatureControls(false, false);
                EnableEnergyControls(false);
                goto Label_01F7;
            Label_01BB:
                EnableVolumeControls(false);
                EnableWeightControls(false);
                EnableTemperatureControls(false, false);
                EnableEnergyControls(false);
                goto Label_01F7;
            Label_01DA:
                EnableVolumeControls(false);
                EnableWeightControls(false);
                EnableTemperatureControls(false, false);
                EnableEnergyControls(false);
            Label_01F7:
                if (idcMemoryValue != 0M)
                {
                    if (ieDisplayMode == appDisplayMode.Total)
                    {
                        MemoryRecallButton.Enabled = true;
                        MemoryAddButton.Enabled = true;
                    }
                    else if (ieSessionOperator == appOperator.None)
                    {
                        if ((GetUnitMode(ieCurrentEntryUnit) != GetUnitMode(ieMemoryUnit)) && (ieCurrentEntryUnit != appUnit.None))
                        {
                            MemoryRecallButton.Enabled = false;
                            MemoryAddButton.Enabled = false;
                        }
                        else
                        {
                            MemoryRecallButton.Enabled = true;
                            MemoryAddButton.Enabled = true;
                        }
                    }
                    else if ((ieSessionOperator == appOperator.Add) || (ieSessionOperator == appOperator.Subtract))
                    {
                        if (ieSessionUnitMode != GetUnitMode(ieMemoryUnit))
                        {
                            MemoryRecallButton.Enabled = false;
                            MemoryAddButton.Enabled = false;
                        }
                        else
                        {
                            MemoryRecallButton.Enabled = true;
                            MemoryAddButton.Enabled = true;
                        }
                    }
                    else if ((ieSessionOperator == appOperator.Multiply) || (ieSessionOperator == appOperator.Divide))
                    {
                        if ((ieSessionUnitMode == appUnitMode.None) && (GetUnitMode(ieMemoryUnit) == appUnitMode.None))
                        {
                            MemoryRecallButton.Enabled = false;
                            MemoryAddButton.Enabled = false;
                        }
                        else if ((ieSessionUnitMode != appUnitMode.None) && (GetUnitMode(ieMemoryUnit) != appUnitMode.None))
                        {
                            MemoryRecallButton.Enabled = false;
                            MemoryAddButton.Enabled = false;
                        }
                        else
                        {
                            MemoryRecallButton.Enabled = true;
                            MemoryAddButton.Enabled = true;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void EnableEnergyControls(bool abEnable)
        {
            try
            {
                CalorieButton.Enabled = abEnable;
                KilojouleButton.Enabled = abEnable;
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void EnableTemperatureControls(bool abFahrenheitEnable, bool abCentigradeEnable)
        {
            try
            {
                DegreeFButton.Enabled = abFahrenheitEnable;
                DegreeCButton.Enabled = abCentigradeEnable;
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void EnableVolumeControls(bool abEnable)
        {
            try
            {
                TeaspoonButton.Enabled = abEnable;
                TablespoonButton.Enabled = abEnable;
                FluidOunceButton.Enabled = abEnable;
                CupButton.Enabled = abEnable;
                PintButton.Enabled = abEnable;
                QuartButton.Enabled = abEnable;
                GallonButton.Enabled = abEnable;
                MilliliterButton.Enabled = abEnable;
                LiterButton.Enabled = abEnable;
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void EnableWeightControls(bool abEnable)
        {
            try
            {
                OunceButton.Enabled = abEnable;
                PoundButton.Enabled = abEnable;
                MilligramButton.Enabled = abEnable;
                GramButton.Enabled = abEnable;
                KilogramButton.Enabled = abEnable;
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void EqualsButton_Click(object sender, EventArgs e)
        {
            try
            {
                Total();
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void FiveButton_Click(object sender, EventArgs e)
        {
            try
            {
                DigitButton(5);
            }
            catch (Exception exception)
            {
                HandleTerminalError(exception);
            }
        }

        private void FluidOunceButton_Click(object sender, EventArgs e)
        {
            try
            {
                UnitButton(appUnit.FluidOunce);
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void FourButton_Click(object sender, EventArgs e)
        {
            try
            {
                DigitButton(4);
            }
            catch (Exception exception)
            {
                HandleTerminalError(exception);
            }
        }

        private void FractionButton(string asFraction)
        {
            try
            {
                if (!ibError && ((ieDisplayMode != appDisplayMode.DataEntry) || (ieCurrentEntryUnit == appUnit.None)))
                {
                    if (ieDisplayMode == appDisplayMode.Total)
                    {
                        SetSessionUnitMode(appUnitMode.None);
                        idcSessionValue = 0M;
                        ieSessionUnit = appUnit.None;
                        ieSessionOperator = appOperator.None;
                        EnableVolumeControls(true);
                        EnableWeightControls(true);
                        EnableTemperatureControls(true, true);
                        EnableEnergyControls(true);
                    }
                    if (ieDisplayMode != appDisplayMode.DataEntry)
                    {
                        isCurrentEntry = "";
                        ieCurrentEntryUnit = appUnit.None;
                        ieCurrentOperator = appOperator.None;
                        ieDisplayMode = appDisplayMode.DataEntry;
                    }
                    if (GetCurrentEntryLength() < 15)
                    {
                        isCurrentEntry = isCurrentEntry + asFraction;
                        RefreshDisplay();
                    }
                }
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void GallonButton_Click(object sender, EventArgs e)
        {
            try
            {
                UnitButton(appUnit.Gallon);
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private decimal GetCurrentEntryDecimalValue()
        {
            decimal num = 0M;
            try
            {
                if (isCurrentEntry == ".")
                {
                    return 0M;
                }
                if (isCurrentEntry == "-")
                {
                    return 0M;
                }
                num = rfcDataService.NotNullDecimal(isCurrentEntry);
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
            return num;
        }

        private int GetCurrentEntryLength()
        {
            int num = 0;
            try
            {
                for (int i = 0; i != isCurrentEntry.Length; i++)
                {
                    if (isCurrentEntry.Substring(i, 1) != ".")
                    {
                        num++;
                    }
                }
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
            return num;
        }

        private appUnit GetUnit(string asText)
        {
            appUnit none = appUnit.None;
            bool flag = false;
            try
            {
                switch (asText.ToLower())
                {
                    case "ea":
                        none = appUnit.None;
                        flag = true;
                        break;

                    case "milligram":
                    case "milligrams":
                    case "mg":
                    case "mg.":
                        none = appUnit.Milligram;
                        flag = true;
                        break;

                    case "gram":
                    case "grams":
                    case "g":
                    case "g.":
                    case "gr":
                    case "gr.":
                        none = appUnit.Gram;
                        flag = true;
                        break;

                    case "kilogram":
                    case "kilograms":
                    case "kg":
                    case "kg.":
                    case "kilo":
                    case "kilos":
                        none = appUnit.Kilogram;
                        flag = true;
                        break;

                    case "liter":
                    case "litre":
                    case "l":
                    case "l.":
                        none = appUnit.Liter;
                        flag = true;
                        break;

                    case "liters":
                    case "litres":
                        none = appUnit.Liter;
                        flag = true;
                        break;

                    case "milliliter":
                    case "millilitre":
                    case "ml":
                    case "ml.":
                    case "milliliters":
                    case "millilitres":
                    case "mls":
                    case "mls.":
                        none = appUnit.Milliliter;
                        flag = true;
                        break;

                    case "cup":
                    case "c":
                    case "c.":
                        none = appUnit.Cup;
                        flag = true;
                        break;

                    case "cups":
                        none = appUnit.Cup;
                        flag = true;
                        break;

                    case "ounce":
                    case "ounces":
                    case "oz":
                    case "oz.":
                        none = appUnit.Ounce;
                        flag = true;
                        break;

                    case "tablespoon":
                    case "tablespoons":
                    case "tbs":
                    case "tbs.":
                    case "tbsp":
                    case "tbsp.":
                    case "tbspn":
                    case "tbspn.":
                    case "tb":
                    case "tb.":
                    case "tbl":
                    case "tbl.":
                    case "tbsps":
                    case "tbsps.":
                    case "tbspns":
                    case "tbspns.":
                        none = appUnit.Tablespoon;
                        flag = true;
                        break;

                    case "teaspoon":
                    case "teaspoons":
                    case "tsp":
                    case "tsp.":
                    case "tspn":
                    case "tspn.":
                    case "tsps":
                    case "tsps.":
                    case "tspns":
                    case "tspns.":
                        none = appUnit.Teaspoon;
                        flag = true;
                        break;

                    case "pound":
                    case "lb":
                    case "lb.":
                        none = appUnit.Pound;
                        flag = true;
                        break;

                    case "pounds":
                    case "lbs":
                    case "lbs.":
                        none = appUnit.Pound;
                        flag = true;
                        break;

                    case "quart":
                    case "qt":
                    case "qt.":
                        none = appUnit.Quart;
                        flag = true;
                        break;

                    case "quarts":
                    case "qts":
                    case "qts.":
                        none = appUnit.Quart;
                        flag = true;
                        break;

                    case "pint":
                    case "pt":
                    case "pt.":
                        none = appUnit.Pint;
                        flag = true;
                        break;

                    case "pints":
                    case "pts":
                    case "pts.":
                        none = appUnit.Pint;
                        flag = true;
                        break;

                    case "fl":
                    case "fl.":
                    case "fluid ounce":
                    case "fluid ounces":
                        none = appUnit.FluidOunce;
                        flag = true;
                        break;

                    case "gallon":
                        none = appUnit.Gallon;
                        flag = true;
                        break;

                    case "gallons":
                        none = appUnit.Gallon;
                        flag = true;
                        break;
                }
                if (flag)
                {
                    return none;
                }
                if (((asText == "t") || (asText == "t.")) || ((asText == "ts") || (asText == "ts.")))
                {
                    none = appUnit.Teaspoon;
                    flag = true;
                    return none;
                }
                if ((!(asText == "T") && !(asText == "T.")) && (!(asText == "Ts") && !(asText == "Ts.")))
                {
                    return none;
                }
                none = appUnit.Tablespoon;
                flag = true;
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
            return none;
        }

        private appUnitMode GetUnitMode(appUnit aeUnit)
        {
            appUnitMode none = appUnitMode.None;
            try
            {
                switch (aeUnit)
                {
                    case appUnit.Teaspoon:
                    case appUnit.Tablespoon:
                    case appUnit.FluidOunce:
                    case appUnit.Cup:
                    case appUnit.Pint:
                    case appUnit.Quart:
                    case appUnit.Gallon:
                    case appUnit.Milliliter:
                    case appUnit.Liter:
                        return appUnitMode.Volume;

                    case appUnit.Ounce:
                    case appUnit.Pound:
                    case appUnit.Milligram:
                    case appUnit.Gram:
                    case appUnit.Kilogram:
                        return appUnitMode.Weight;

                    case appUnit.DegreeF:
                    case appUnit.DegreeC:
                        return appUnitMode.Temperature;

                    case appUnit.Calorie:
                    case appUnit.Kilojoule:
                        return appUnitMode.Energy;
                }
                return none;
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
            return none;
        }

        private string GetUnitText(appUnit aeUnit, decimal adcValue)
        {
            string str = "";
            try
            {
                if (!ibAbbreviateUnits)
                {
                    goto Label_05E6;
                }
                switch (aeUnit)
                {
                    case appUnit.Teaspoon:
                        if (!(adcValue == 0M))
                        {
                            break;
                        }
                        return "tsp";

                    case appUnit.Tablespoon:
                        if (!(adcValue == 0M))
                        {
                            goto Label_00DA;
                        }
                        return "Tbs";

                    case appUnit.FluidOunce:
                        if (!(adcValue == 0M))
                        {
                            goto Label_0133;
                        }
                        return "fl oz";

                    case appUnit.Cup:
                        if (!(adcValue == 0M))
                        {
                            goto Label_018C;
                        }
                        return "cups";

                    case appUnit.Pint:
                        if (!(adcValue == 0M))
                        {
                            goto Label_01E5;
                        }
                        return "pt";

                    case appUnit.Quart:
                        if (!(adcValue == 0M))
                        {
                            goto Label_0230;
                        }
                        return "qt";

                    case appUnit.Gallon:
                        if (!(adcValue == 0M))
                        {
                            goto Label_027B;
                        }
                        return "gal";

                    case appUnit.Milliliter:
                        if (!(adcValue == 0M))
                        {
                            goto Label_02C6;
                        }
                        return "ml";

                    case appUnit.Liter:
                        if (!(adcValue == 0M))
                        {
                            goto Label_0311;
                        }
                        return "l";

                    case appUnit.Ounce:
                        if (!(adcValue == 0M))
                        {
                            goto Label_035C;
                        }
                        return "oz";

                    case appUnit.Pound:
                        if (!(adcValue == 0M))
                        {
                            goto Label_03A7;
                        }
                        return "lbs";

                    case appUnit.Milligram:
                        if (!(adcValue == 0M))
                        {
                            goto Label_03F2;
                        }
                        return "mg";

                    case appUnit.Gram:
                        if (!(adcValue == 0M))
                        {
                            goto Label_043D;
                        }
                        return "g";

                    case appUnit.Kilogram:
                        if (!(adcValue == 0M))
                        {
                            goto Label_0488;
                        }
                        return "kg";

                    case appUnit.DegreeF:
                        if (!(adcValue == 0M))
                        {
                            goto Label_04D3;
                        }
                        return "\x00b0F";

                    case appUnit.DegreeC:
                        if (!(adcValue == 0M))
                        {
                            goto Label_051E;
                        }
                        return "\x00b0C";

                    case appUnit.Calorie:
                        if (!(adcValue == 0M))
                        {
                            goto Label_0569;
                        }
                        return "cal";

                    case appUnit.Kilojoule:
                        if (!(adcValue == 0M))
                        {
                            goto Label_05B4;
                        }
                        return "kJ";

                    default:
                        return str;
                }
                if (((adcValue <= 1M) && (adcValue >= -1M)) && (adcValue >= -1M))
                {
                }
                return "tsp";
            Label_00DA:
                if (((adcValue <= 1M) && (adcValue >= -1M)) && (adcValue >= -1M))
                {
                }
                return "Tbs";
            Label_0133:
                if (((adcValue <= 1M) && (adcValue >= -1M)) && (adcValue >= -1M))
                {
                }
                return "fl oz";
            Label_018C:
                if (((adcValue > 1M) || (adcValue < -1M)) || (adcValue < -1M))
                {
                    return "cups";
                }
                return "cup";
            Label_01E5:
                if ((adcValue <= 1M) && (adcValue >= -1M))
                {
                }
                return "pt";
            Label_0230:
                if ((adcValue <= 1M) && (adcValue >= -1M))
                {
                }
                return "qt";
            Label_027B:
                if ((adcValue <= 1M) && (adcValue >= -1M))
                {
                }
                return "gal";
            Label_02C6:
                if ((adcValue <= 1M) && (adcValue >= -1M))
                {
                }
                return "ml";
            Label_0311:
                if ((adcValue <= 1M) && (adcValue >= -1M))
                {
                }
                return "l";
            Label_035C:
                if ((adcValue <= 1M) && (adcValue >= -1M))
                {
                }
                return "oz";
            Label_03A7:
                if ((adcValue > 1M) || (adcValue < -1M))
                {
                    return "lbs";
                }
                return "lb";
            Label_03F2:
                if ((adcValue <= 1M) && (adcValue >= -1M))
                {
                }
                return "mg";
            Label_043D:
                if ((adcValue <= 1M) && (adcValue >= -1M))
                {
                }
                return "g";
            Label_0488:
                if ((adcValue <= 1M) && (adcValue >= -1M))
                {
                }
                return "kg";
            Label_04D3:
                if ((adcValue <= 1M) && (adcValue >= -1M))
                {
                }
                return "\x00b0F";
            Label_051E:
                if ((adcValue <= 1M) && (adcValue >= -1M))
                {
                }
                return "\x00b0C";
            Label_0569:
                if ((adcValue <= 1M) && (adcValue >= -1M))
                {
                }
                return "cal";
            Label_05B4:
                if ((adcValue <= 1M) && (adcValue >= -1M))
                {
                }
                return "kJ";
            Label_05E6:
                switch (aeUnit)
                {
                    case appUnit.Teaspoon:
                        if (!(adcValue == 0M))
                        {
                            break;
                        }
                        return "teaspoons";

                    case appUnit.Tablespoon:
                        if (!(adcValue == 0M))
                        {
                            goto Label_06AF;
                        }
                        return "tablespoons";

                    case appUnit.FluidOunce:
                        if (!(adcValue == 0M))
                        {
                            goto Label_0708;
                        }
                        return "fluid ounces";

                    case appUnit.Cup:
                        if (!(adcValue == 0M))
                        {
                            goto Label_0761;
                        }
                        return "cups";

                    case appUnit.Pint:
                        if (!(adcValue == 0M))
                        {
                            goto Label_07BA;
                        }
                        return "pints";

                    case appUnit.Quart:
                        if (!(adcValue == 0M))
                        {
                            goto Label_0805;
                        }
                        return "quarts";

                    case appUnit.Gallon:
                        if (!(adcValue == 0M))
                        {
                            goto Label_0850;
                        }
                        return "gallons";

                    case appUnit.Milliliter:
                        if (!(adcValue == 0M))
                        {
                            goto Label_089B;
                        }
                        return "milliliters";

                    case appUnit.Liter:
                        if (!(adcValue == 0M))
                        {
                            goto Label_08E6;
                        }
                        return "liters";

                    case appUnit.Ounce:
                        if (!(adcValue == 0M))
                        {
                            goto Label_0931;
                        }
                        return "ounces";

                    case appUnit.Pound:
                        if (!(adcValue == 0M))
                        {
                            goto Label_097C;
                        }
                        return "pounds";

                    case appUnit.Milligram:
                        if (!(adcValue == 0M))
                        {
                            goto Label_09C7;
                        }
                        return "milligrams";

                    case appUnit.Gram:
                        if (!(adcValue == 0M))
                        {
                            goto Label_0A12;
                        }
                        return "grams";

                    case appUnit.Kilogram:
                        if (!(adcValue == 0M))
                        {
                            goto Label_0A5D;
                        }
                        return "kilograms";

                    case appUnit.DegreeF:
                        if (!(adcValue == 0M))
                        {
                            goto Label_0AA8;
                        }
                        return "\x00b0F";

                    case appUnit.DegreeC:
                        if (!(adcValue == 0M))
                        {
                            goto Label_0AF3;
                        }
                        return "\x00b0C";

                    case appUnit.Calorie:
                        if (!(adcValue == 0M))
                        {
                            goto Label_0B3B;
                        }
                        return "calories";

                    case appUnit.Kilojoule:
                        if (!(adcValue == 0M))
                        {
                            goto Label_0B7D;
                        }
                        return "kJ";

                    default:
                        return str;
                }
                if (((adcValue > 1M) || (adcValue < -1M)) || (adcValue < -1M))
                {
                    return "teaspoons";
                }
                return "teaspoon";
            Label_06AF:
                if (((adcValue > 1M) || (adcValue < -1M)) || (adcValue < -1M))
                {
                    return "tablespoons";
                }
                return "tablespoon";
            Label_0708:
                if (((adcValue > 1M) || (adcValue < -1M)) || (adcValue < -1M))
                {
                    return "fluid ounces";
                }
                return "fluid ounce";
            Label_0761:
                if (((adcValue > 1M) || (adcValue < -1M)) || (adcValue < -1M))
                {
                    return "cups";
                }
                return "cup";
            Label_07BA:
                if ((adcValue > 1M) || (adcValue < -1M))
                {
                    return "pints";
                }
                return "pint";
            Label_0805:
                if ((adcValue > 1M) || (adcValue < -1M))
                {
                    return "quarts";
                }
                return "quart";
            Label_0850:
                if ((adcValue > 1M) || (adcValue < -1M))
                {
                    return "gallons";
                }
                return "gallon";
            Label_089B:
                if ((adcValue > 1M) || (adcValue < -1M))
                {
                    return "milliliters";
                }
                return "milliliter";
            Label_08E6:
                if ((adcValue > 1M) || (adcValue < -1M))
                {
                    return "liters";
                }
                return "liter";
            Label_0931:
                if ((adcValue > 1M) || (adcValue < -1M))
                {
                    return "ounces";
                }
                return "ounce";
            Label_097C:
                if ((adcValue > 1M) || (adcValue < -1M))
                {
                    return "pounds";
                }
                return "pound";
            Label_09C7:
                if ((adcValue > 1M) || (adcValue < -1M))
                {
                    return "milligrams";
                }
                return "milligram";
            Label_0A12:
                if ((adcValue > 1M) || (adcValue < -1M))
                {
                    return "grams";
                }
                return "gram";
            Label_0A5D:
                if ((adcValue > 1M) || (adcValue < -1M))
                {
                    return "kilograms";
                }
                return "kilogram";
            Label_0AA8:
                if ((adcValue <= 1M) && (adcValue >= -1M))
                {
                }
                return "\x00b0F";
            Label_0AF3:
                if ((adcValue <= 1M) && (adcValue >= -1M))
                {
                }
                return "\x00b0C";
            Label_0B3B:
                if ((adcValue > 1M) || (adcValue < -1M))
                {
                    return "calories";
                }
                return "calorie";
            Label_0B7D:
                if ((adcValue > 1M) || (adcValue < -1M))
                {
                    return "kJ";
                }
                str = "kJ";
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
            return str;
        }

        private void GramButton_Click(object sender, EventArgs e)
        {
            try
            {
                UnitButton(appUnit.Gram);
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void HalfButton_Click(object sender, EventArgs e)
        {
            try
            {
                FractionButton("\x00bd");
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void HandleSystemError(Exception aoException)
        {
            MessageBox.Show(aoException.Message);
        }

        private void HandleTerminalError(Exception aoException)
        {
            MessageBox.Show(aoException.Message);
        }

        private void HelpTopicsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // try
            // {
            //     Help.ShowHelp(this, "KitchenCalculatorHelp.chm", HelpNavigator.Topic, "Getting Started.htm");
            // }
            // catch (Exception exception)
            // {
            //     this.HandleSystemError(exception);
            // }
        }

        public void Initialize()
        {
            try
            {
                LoadUserOptions();
                base.TopMost = ibKeepOnTop;
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(appKitchenCalculatorDialog));
            this.MemoryClearButton = new System.Windows.Forms.Button();
            this.MemoryRecallButton = new System.Windows.Forms.Button();
            this.MemorySaveButton = new System.Windows.Forms.Button();
            this.MemoryAddButton = new System.Windows.Forms.Button();
            this.ZeroButton = new System.Windows.Forms.Button();
            this.OneButton = new System.Windows.Forms.Button();
            this.PlusMinusButton = new System.Windows.Forms.Button();
            this.TwoButton = new System.Windows.Forms.Button();
            this.PeriodButton = new System.Windows.Forms.Button();
            this.ThreeButton = new System.Windows.Forms.Button();
            this.SixButton = new System.Windows.Forms.Button();
            this.FiveButton = new System.Windows.Forms.Button();
            this.FourButton = new System.Windows.Forms.Button();
            this.NineButton = new System.Windows.Forms.Button();
            this.EightButton = new System.Windows.Forms.Button();
            this.SevenButton = new System.Windows.Forms.Button();
            this.BackspaceButton = new System.Windows.Forms.Button();
            this.AddButton = new System.Windows.Forms.Button();
            this.SubtractButton = new System.Windows.Forms.Button();
            this.DivideButton = new System.Windows.Forms.Button();
            this.MultiplyButton = new System.Windows.Forms.Button();
            this.EqualsButton = new System.Windows.Forms.Button();
            this.ClearButton = new System.Windows.Forms.Button();
            this.ClearEntryButton = new System.Windows.Forms.Button();
            this.EditToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CopyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CopyAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.AboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.USToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CanadaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.UKToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AustraliaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.NewZealandToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.AbbreviateUnitsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.KeepOnTopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DisplayLabel = new System.Windows.Forms.Label();
            this.MemoryLabel = new System.Windows.Forms.Label();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.rfcComboBox5 = new System.Windows.Forms.ComboBox();
            this.FluidOunceButton = new System.Windows.Forms.Button();
            this.TablespoonButton = new System.Windows.Forms.Button();
            this.CupButton = new System.Windows.Forms.Button();
            this.PintButton = new System.Windows.Forms.Button();
            this.QuartButton = new System.Windows.Forms.Button();
            this.OunceButton = new System.Windows.Forms.Button();
            this.PoundButton = new System.Windows.Forms.Button();
            this.TeaspoonButton = new System.Windows.Forms.Button();
            this.LiterButton = new System.Windows.Forms.Button();
            this.MilliliterButton = new System.Windows.Forms.Button();
            this.MilligramButton = new System.Windows.Forms.Button();
            this.GallonButton = new System.Windows.Forms.Button();
            this.GramButton = new System.Windows.Forms.Button();
            this.KilogramButton = new System.Windows.Forms.Button();
            this.DegreeFButton = new System.Windows.Forms.Button();
            this.DegreeCButton = new System.Windows.Forms.Button();
            this.CalorieButton = new System.Windows.Forms.Button();
            this.KilojouleButton = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // MemoryClearButton
            // 
            this.MemoryClearButton.ForeColor = System.Drawing.Color.Red;
            this.MemoryClearButton.Location = new System.Drawing.Point(12, 71);
            this.MemoryClearButton.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.MemoryClearButton.Name = "MemoryClearButton";
            this.MemoryClearButton.Size = new System.Drawing.Size(36, 28);
            this.MemoryClearButton.TabIndex = 0;
            this.MemoryClearButton.TabStop = false;
            this.MemoryClearButton.Text = "MC";
            this.MemoryClearButton.UseVisualStyleBackColor = true;
            this.MemoryClearButton.Click += new System.EventHandler(this.MemoryClearButton_Click);
            this.MemoryClearButton.Enter += new System.EventHandler(this.Button_Enter);
            // 
            // MemoryRecallButton
            // 
            this.MemoryRecallButton.ForeColor = System.Drawing.Color.Red;
            this.MemoryRecallButton.Location = new System.Drawing.Point(12, 102);
            this.MemoryRecallButton.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.MemoryRecallButton.Name = "MemoryRecallButton";
            this.MemoryRecallButton.Size = new System.Drawing.Size(36, 28);
            this.MemoryRecallButton.TabIndex = 0;
            this.MemoryRecallButton.TabStop = false;
            this.MemoryRecallButton.Text = "MR";
            this.MemoryRecallButton.UseVisualStyleBackColor = true;
            this.MemoryRecallButton.Click += new System.EventHandler(this.MemoryRecallButton_Click);
            this.MemoryRecallButton.Enter += new System.EventHandler(this.Button_Enter);
            // 
            // MemorySaveButton
            // 
            this.MemorySaveButton.ForeColor = System.Drawing.Color.Red;
            this.MemorySaveButton.Location = new System.Drawing.Point(12, 133);
            this.MemorySaveButton.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.MemorySaveButton.Name = "MemorySaveButton";
            this.MemorySaveButton.Size = new System.Drawing.Size(36, 28);
            this.MemorySaveButton.TabIndex = 0;
            this.MemorySaveButton.TabStop = false;
            this.MemorySaveButton.Text = "MS";
            this.MemorySaveButton.UseVisualStyleBackColor = true;
            this.MemorySaveButton.Click += new System.EventHandler(this.MemorySaveButton_Click);
            this.MemorySaveButton.Enter += new System.EventHandler(this.Button_Enter);
            // 
            // MemoryAddButton
            // 
            this.MemoryAddButton.ForeColor = System.Drawing.Color.Red;
            this.MemoryAddButton.Location = new System.Drawing.Point(12, 164);
            this.MemoryAddButton.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.MemoryAddButton.Name = "MemoryAddButton";
            this.MemoryAddButton.Size = new System.Drawing.Size(36, 28);
            this.MemoryAddButton.TabIndex = 0;
            this.MemoryAddButton.TabStop = false;
            this.MemoryAddButton.Text = "M+";
            this.MemoryAddButton.UseVisualStyleBackColor = true;
            this.MemoryAddButton.Click += new System.EventHandler(this.MemoryAddButton_Click);
            this.MemoryAddButton.Enter += new System.EventHandler(this.Button_Enter);
            // 
            // ZeroButton
            // 
            this.ZeroButton.ForeColor = System.Drawing.Color.Blue;
            this.ZeroButton.Location = new System.Drawing.Point(57, 164);
            this.ZeroButton.Margin = new System.Windows.Forms.Padding(9, 3, 0, 0);
            this.ZeroButton.Name = "ZeroButton";
            this.ZeroButton.Size = new System.Drawing.Size(36, 28);
            this.ZeroButton.TabIndex = 0;
            this.ZeroButton.TabStop = false;
            this.ZeroButton.Text = "0";
            this.ZeroButton.UseVisualStyleBackColor = true;
            this.ZeroButton.Click += new System.EventHandler(this.ZeroButton_Click);
            this.ZeroButton.Enter += new System.EventHandler(this.Button_Enter);
            // 
            // OneButton
            // 
            this.OneButton.ForeColor = System.Drawing.Color.Blue;
            this.OneButton.Location = new System.Drawing.Point(57, 133);
            this.OneButton.Margin = new System.Windows.Forms.Padding(9, 3, 0, 0);
            this.OneButton.Name = "OneButton";
            this.OneButton.Size = new System.Drawing.Size(36, 28);
            this.OneButton.TabIndex = 0;
            this.OneButton.TabStop = false;
            this.OneButton.Text = "1";
            this.OneButton.UseVisualStyleBackColor = true;
            this.OneButton.Click += new System.EventHandler(this.OneButton_Click);
            this.OneButton.Enter += new System.EventHandler(this.Button_Enter);
            // 
            // PlusMinusButton
            // 
            this.PlusMinusButton.ForeColor = System.Drawing.Color.Blue;
            this.PlusMinusButton.Location = new System.Drawing.Point(96, 164);
            this.PlusMinusButton.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.PlusMinusButton.Name = "PlusMinusButton";
            this.PlusMinusButton.Size = new System.Drawing.Size(36, 28);
            this.PlusMinusButton.TabIndex = 0;
            this.PlusMinusButton.TabStop = false;
            this.PlusMinusButton.Text = "+/-";
            this.PlusMinusButton.UseVisualStyleBackColor = true;
            this.PlusMinusButton.Click += new System.EventHandler(this.PlusMinusButton_Click);
            this.PlusMinusButton.Enter += new System.EventHandler(this.Button_Enter);
            // 
            // TwoButton
            // 
            this.TwoButton.ForeColor = System.Drawing.Color.Blue;
            this.TwoButton.Location = new System.Drawing.Point(96, 133);
            this.TwoButton.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.TwoButton.Name = "TwoButton";
            this.TwoButton.Size = new System.Drawing.Size(36, 28);
            this.TwoButton.TabIndex = 0;
            this.TwoButton.TabStop = false;
            this.TwoButton.Text = "2";
            this.TwoButton.UseVisualStyleBackColor = true;
            this.TwoButton.Click += new System.EventHandler(this.TwoButton_Click);
            this.TwoButton.Enter += new System.EventHandler(this.Button_Enter);
            // 
            // PeriodButton
            // 
            this.PeriodButton.ForeColor = System.Drawing.Color.Blue;
            this.PeriodButton.Location = new System.Drawing.Point(135, 164);
            this.PeriodButton.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.PeriodButton.Name = "PeriodButton";
            this.PeriodButton.Size = new System.Drawing.Size(36, 28);
            this.PeriodButton.TabIndex = 0;
            this.PeriodButton.TabStop = false;
            this.PeriodButton.Text = ".";
            this.PeriodButton.UseVisualStyleBackColor = true;
            this.PeriodButton.Click += new System.EventHandler(this.PeriodButton_Click);
            this.PeriodButton.Enter += new System.EventHandler(this.Button_Enter);
            // 
            // ThreeButton
            // 
            this.ThreeButton.ForeColor = System.Drawing.Color.Blue;
            this.ThreeButton.Location = new System.Drawing.Point(135, 133);
            this.ThreeButton.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.ThreeButton.Name = "ThreeButton";
            this.ThreeButton.Size = new System.Drawing.Size(36, 28);
            this.ThreeButton.TabIndex = 0;
            this.ThreeButton.TabStop = false;
            this.ThreeButton.Text = "3";
            this.ThreeButton.UseVisualStyleBackColor = true;
            this.ThreeButton.Click += new System.EventHandler(this.ThreeButton_Click);
            this.ThreeButton.Enter += new System.EventHandler(this.Button_Enter);
            // 
            // SixButton
            // 
            this.SixButton.ForeColor = System.Drawing.Color.Blue;
            this.SixButton.Location = new System.Drawing.Point(135, 102);
            this.SixButton.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.SixButton.Name = "SixButton";
            this.SixButton.Size = new System.Drawing.Size(36, 28);
            this.SixButton.TabIndex = 0;
            this.SixButton.TabStop = false;
            this.SixButton.Text = "6";
            this.SixButton.UseVisualStyleBackColor = true;
            this.SixButton.Click += new System.EventHandler(this.SixButton_Click);
            this.SixButton.Enter += new System.EventHandler(this.Button_Enter);
            // 
            // FiveButton
            // 
            this.FiveButton.ForeColor = System.Drawing.Color.Blue;
            this.FiveButton.Location = new System.Drawing.Point(96, 102);
            this.FiveButton.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.FiveButton.Name = "FiveButton";
            this.FiveButton.Size = new System.Drawing.Size(36, 28);
            this.FiveButton.TabIndex = 0;
            this.FiveButton.TabStop = false;
            this.FiveButton.Text = "5";
            this.FiveButton.UseVisualStyleBackColor = true;
            this.FiveButton.Click += new System.EventHandler(this.FiveButton_Click);
            this.FiveButton.Enter += new System.EventHandler(this.Button_Enter);
            // 
            // FourButton
            // 
            this.FourButton.ForeColor = System.Drawing.Color.Blue;
            this.FourButton.Location = new System.Drawing.Point(57, 102);
            this.FourButton.Margin = new System.Windows.Forms.Padding(9, 3, 0, 0);
            this.FourButton.Name = "FourButton";
            this.FourButton.Size = new System.Drawing.Size(36, 28);
            this.FourButton.TabIndex = 0;
            this.FourButton.TabStop = false;
            this.FourButton.Text = "4";
            this.FourButton.UseVisualStyleBackColor = true;
            this.FourButton.Click += new System.EventHandler(this.FourButton_Click);
            this.FourButton.Enter += new System.EventHandler(this.Button_Enter);
            // 
            // NineButton
            // 
            this.NineButton.ForeColor = System.Drawing.Color.Blue;
            this.NineButton.Location = new System.Drawing.Point(135, 71);
            this.NineButton.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.NineButton.Name = "NineButton";
            this.NineButton.Size = new System.Drawing.Size(36, 28);
            this.NineButton.TabIndex = 0;
            this.NineButton.TabStop = false;
            this.NineButton.Text = "9";
            this.NineButton.UseVisualStyleBackColor = true;
            this.NineButton.Click += new System.EventHandler(this.NineButton_Click);
            this.NineButton.Enter += new System.EventHandler(this.Button_Enter);
            // 
            // EightButton
            // 
            this.EightButton.ForeColor = System.Drawing.Color.Blue;
            this.EightButton.Location = new System.Drawing.Point(96, 71);
            this.EightButton.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.EightButton.Name = "EightButton";
            this.EightButton.Size = new System.Drawing.Size(36, 28);
            this.EightButton.TabIndex = 0;
            this.EightButton.TabStop = false;
            this.EightButton.Text = "8";
            this.EightButton.UseVisualStyleBackColor = true;
            this.EightButton.Click += new System.EventHandler(this.EightButton_Click);
            this.EightButton.Enter += new System.EventHandler(this.Button_Enter);
            // 
            // SevenButton
            // 
            this.SevenButton.ForeColor = System.Drawing.Color.Blue;
            this.SevenButton.Location = new System.Drawing.Point(57, 71);
            this.SevenButton.Margin = new System.Windows.Forms.Padding(9, 3, 0, 0);
            this.SevenButton.Name = "SevenButton";
            this.SevenButton.Size = new System.Drawing.Size(36, 28);
            this.SevenButton.TabIndex = 0;
            this.SevenButton.TabStop = false;
            this.SevenButton.Text = "7";
            this.SevenButton.UseVisualStyleBackColor = true;
            this.SevenButton.Click += new System.EventHandler(this.SevenButton_Click);
            this.SevenButton.Enter += new System.EventHandler(this.Button_Enter);
            // 
            // BackspaceButton
            // 
            this.BackspaceButton.ForeColor = System.Drawing.Color.Red;
            this.BackspaceButton.Location = new System.Drawing.Point(57, 40);
            this.BackspaceButton.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.BackspaceButton.Name = "BackspaceButton";
            this.BackspaceButton.Size = new System.Drawing.Size(75, 28);
            this.BackspaceButton.TabIndex = 0;
            this.BackspaceButton.TabStop = false;
            this.BackspaceButton.Text = "Backspace";
            this.BackspaceButton.UseVisualStyleBackColor = true;
            this.BackspaceButton.Click += new System.EventHandler(this.BackspaceButton_Click);
            this.BackspaceButton.Enter += new System.EventHandler(this.Button_Enter);
            // 
            // AddButton
            // 
            this.AddButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AddButton.ForeColor = System.Drawing.Color.Red;
            this.AddButton.Location = new System.Drawing.Point(174, 102);
            this.AddButton.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(36, 59);
            this.AddButton.TabIndex = 0;
            this.AddButton.TabStop = false;
            this.AddButton.Text = "+";
            this.AddButton.UseVisualStyleBackColor = true;
            this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
            this.AddButton.Enter += new System.EventHandler(this.Button_Enter);
            // 
            // SubtractButton
            // 
            this.SubtractButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SubtractButton.ForeColor = System.Drawing.Color.Red;
            this.SubtractButton.Location = new System.Drawing.Point(213, 102);
            this.SubtractButton.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.SubtractButton.Name = "SubtractButton";
            this.SubtractButton.Size = new System.Drawing.Size(36, 59);
            this.SubtractButton.TabIndex = 0;
            this.SubtractButton.TabStop = false;
            this.SubtractButton.Text = "-";
            this.SubtractButton.UseVisualStyleBackColor = true;
            this.SubtractButton.Click += new System.EventHandler(this.SubtractButton_Click);
            this.SubtractButton.Enter += new System.EventHandler(this.Button_Enter);
            // 
            // DivideButton
            // 
            this.DivideButton.ForeColor = System.Drawing.Color.Red;
            this.DivideButton.Location = new System.Drawing.Point(213, 71);
            this.DivideButton.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.DivideButton.Name = "DivideButton";
            this.DivideButton.Size = new System.Drawing.Size(36, 28);
            this.DivideButton.TabIndex = 0;
            this.DivideButton.TabStop = false;
            this.DivideButton.Text = "/";
            this.DivideButton.UseVisualStyleBackColor = true;
            this.DivideButton.Click += new System.EventHandler(this.DivideButton_Click);
            this.DivideButton.Enter += new System.EventHandler(this.Button_Enter);
            // 
            // MultiplyButton
            // 
            this.MultiplyButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MultiplyButton.ForeColor = System.Drawing.Color.Red;
            this.MultiplyButton.Location = new System.Drawing.Point(174, 71);
            this.MultiplyButton.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.MultiplyButton.Name = "MultiplyButton";
            this.MultiplyButton.Size = new System.Drawing.Size(36, 28);
            this.MultiplyButton.TabIndex = 0;
            this.MultiplyButton.TabStop = false;
            this.MultiplyButton.Text = "*";
            this.MultiplyButton.UseVisualStyleBackColor = true;
            this.MultiplyButton.Click += new System.EventHandler(this.MultiplyButton_Click);
            this.MultiplyButton.Enter += new System.EventHandler(this.Button_Enter);
            // 
            // EqualsButton
            // 
            this.EqualsButton.ForeColor = System.Drawing.Color.Red;
            this.EqualsButton.Location = new System.Drawing.Point(174, 164);
            this.EqualsButton.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.EqualsButton.Name = "EqualsButton";
            this.EqualsButton.Size = new System.Drawing.Size(75, 28);
            this.EqualsButton.TabIndex = 0;
            this.EqualsButton.TabStop = false;
            this.EqualsButton.Text = "=";
            this.EqualsButton.UseVisualStyleBackColor = true;
            this.EqualsButton.Click += new System.EventHandler(this.EqualsButton_Click);
            this.EqualsButton.Enter += new System.EventHandler(this.Button_Enter);
            // 
            // ClearButton
            // 
            this.ClearButton.ForeColor = System.Drawing.Color.Red;
            this.ClearButton.Location = new System.Drawing.Point(193, 40);
            this.ClearButton.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.ClearButton.Name = "ClearButton";
            this.ClearButton.Size = new System.Drawing.Size(56, 28);
            this.ClearButton.TabIndex = 0;
            this.ClearButton.TabStop = false;
            this.ClearButton.Text = "C";
            this.ClearButton.UseVisualStyleBackColor = true;
            this.ClearButton.Click += new System.EventHandler(this.ClearButton_Click);
            this.ClearButton.Enter += new System.EventHandler(this.Button_Enter);
            // 
            // ClearEntryButton
            // 
            this.ClearEntryButton.ForeColor = System.Drawing.Color.Red;
            this.ClearEntryButton.Location = new System.Drawing.Point(135, 40);
            this.ClearEntryButton.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.ClearEntryButton.Name = "ClearEntryButton";
            this.ClearEntryButton.Size = new System.Drawing.Size(55, 28);
            this.ClearEntryButton.TabIndex = 0;
            this.ClearEntryButton.TabStop = false;
            this.ClearEntryButton.Text = "CE";
            this.ClearEntryButton.UseVisualStyleBackColor = true;
            this.ClearEntryButton.Click += new System.EventHandler(this.ClearEntryButton_Click);
            this.ClearEntryButton.Enter += new System.EventHandler(this.Button_Enter);
            // 
            // EditToolStripMenuItem
            // 
            this.EditToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CopyToolStripMenuItem,
            this.CopyAllToolStripMenuItem,
            this.PasteToolStripMenuItem});
            this.EditToolStripMenuItem.Name = "EditToolStripMenuItem";
            this.EditToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.EditToolStripMenuItem.Text = "&Edit";
            // 
            // CopyToolStripMenuItem
            // 
            this.CopyToolStripMenuItem.Name = "CopyToolStripMenuItem";
            this.CopyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.CopyToolStripMenuItem.Size = new System.Drawing.Size(255, 22);
            this.CopyToolStripMenuItem.Text = "&Copy Value";
            this.CopyToolStripMenuItem.Click += new System.EventHandler(this.CopyToolStripMenuItem_Click);
            // 
            // CopyAllToolStripMenuItem
            // 
            this.CopyAllToolStripMenuItem.Name = "CopyAllToolStripMenuItem";
            this.CopyAllToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.C)));
            this.CopyAllToolStripMenuItem.Size = new System.Drawing.Size(255, 22);
            this.CopyAllToolStripMenuItem.Text = "Copy Value and Unit";
            this.CopyAllToolStripMenuItem.Click += new System.EventHandler(this.CopyAllToolStripMenuItem_Click);
            // 
            // PasteToolStripMenuItem
            // 
            this.PasteToolStripMenuItem.Name = "PasteToolStripMenuItem";
            this.PasteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.PasteToolStripMenuItem.Size = new System.Drawing.Size(255, 22);
            this.PasteToolStripMenuItem.Text = "&Paste";
            this.PasteToolStripMenuItem.Click += new System.EventHandler(this.PasteToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(204, 6);
            // 
            // AboutToolStripMenuItem
            // 
            this.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem";
            this.AboutToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.AboutToolStripMenuItem.Text = "&About Pub Grub Calculator";
            this.AboutToolStripMenuItem.Click += new System.EventHandler(this.AboutToolStripMenuItem_Click);
            // 
            // OptionsToolStripMenuItem
            // 
            this.OptionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.USToolStripMenuItem,
            this.CanadaToolStripMenuItem,
            this.UKToolStripMenuItem,
            this.AustraliaToolStripMenuItem,
            this.NewZealandToolStripMenuItem,
            this.toolStripMenuItem2,
            this.AbbreviateUnitsToolStripMenuItem,
            this.KeepOnTopToolStripMenuItem});
            this.OptionsToolStripMenuItem.Name = "OptionsToolStripMenuItem";
            this.OptionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.OptionsToolStripMenuItem.Text = "&Options";
            // 
            // USToolStripMenuItem
            // 
            this.USToolStripMenuItem.Checked = true;
            this.USToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.USToolStripMenuItem.Name = "USToolStripMenuItem";
            this.USToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.USToolStripMenuItem.Text = "US";
            this.USToolStripMenuItem.Click += new System.EventHandler(this.UnitSystemToolStripMenuItem_Click);
            // 
            // CanadaToolStripMenuItem
            // 
            this.CanadaToolStripMenuItem.Name = "CanadaToolStripMenuItem";
            this.CanadaToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.CanadaToolStripMenuItem.Text = "Canada";
            this.CanadaToolStripMenuItem.Click += new System.EventHandler(this.UnitSystemToolStripMenuItem_Click);
            // 
            // UKToolStripMenuItem
            // 
            this.UKToolStripMenuItem.Name = "UKToolStripMenuItem";
            this.UKToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.UKToolStripMenuItem.Text = "UK";
            this.UKToolStripMenuItem.Click += new System.EventHandler(this.UnitSystemToolStripMenuItem_Click);
            // 
            // AustraliaToolStripMenuItem
            // 
            this.AustraliaToolStripMenuItem.Name = "AustraliaToolStripMenuItem";
            this.AustraliaToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.AustraliaToolStripMenuItem.Text = "Australia";
            this.AustraliaToolStripMenuItem.Click += new System.EventHandler(this.UnitSystemToolStripMenuItem_Click);
            // 
            // NewZealandToolStripMenuItem
            // 
            this.NewZealandToolStripMenuItem.Name = "NewZealandToolStripMenuItem";
            this.NewZealandToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.NewZealandToolStripMenuItem.Text = "New Zealand";
            this.NewZealandToolStripMenuItem.Click += new System.EventHandler(this.UnitSystemToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(158, 6);
            // 
            // AbbreviateUnitsToolStripMenuItem
            // 
            this.AbbreviateUnitsToolStripMenuItem.Name = "AbbreviateUnitsToolStripMenuItem";
            this.AbbreviateUnitsToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.AbbreviateUnitsToolStripMenuItem.Text = "Abbreviate Units";
            this.AbbreviateUnitsToolStripMenuItem.Click += new System.EventHandler(this.AbbreviateUnitsToolStripMenuItem_Click);
            // 
            // KeepOnTopToolStripMenuItem
            // 
            this.KeepOnTopToolStripMenuItem.Checked = true;
            this.KeepOnTopToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.KeepOnTopToolStripMenuItem.Name = "KeepOnTopToolStripMenuItem";
            this.KeepOnTopToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.KeepOnTopToolStripMenuItem.Text = "Keep On Top";
            this.KeepOnTopToolStripMenuItem.Click += new System.EventHandler(this.KeepOnTopToolStripMenuItem_Click);
            // 
            // DisplayLabel
            // 
            this.DisplayLabel.BackColor = System.Drawing.Color.White;
            this.DisplayLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.DisplayLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DisplayLabel.Location = new System.Drawing.Point(12, 9);
            this.DisplayLabel.Name = "DisplayLabel";
            this.DisplayLabel.Size = new System.Drawing.Size(237, 21);
            this.DisplayLabel.TabIndex = 0;
            this.DisplayLabel.Text = "0";
            this.DisplayLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // MemoryLabel
            // 
            this.MemoryLabel.BackColor = System.Drawing.SystemColors.Control;
            this.MemoryLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.MemoryLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MemoryLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.MemoryLabel.Location = new System.Drawing.Point(12, 40);
            this.MemoryLabel.Name = "MemoryLabel";
            this.MemoryLabel.Size = new System.Drawing.Size(36, 28);
            this.MemoryLabel.TabIndex = 0;
            this.MemoryLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(12, 58);
            this.radioButton1.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(60, 17);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Volume";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(86, 58);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(59, 17);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Weight";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(164, 58);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(85, 17);
            this.radioButton3.TabIndex = 2;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "Temperature";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // rfcComboBox5
            // 
            this.rfcComboBox5.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.rfcComboBox5.FormattingEnabled = true;
            this.rfcComboBox5.Items.AddRange(new object[] {
            "fluid ounce"});
            this.rfcComboBox5.Location = new System.Drawing.Point(174, 31);
            this.rfcComboBox5.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.rfcComboBox5.Name = "rfcComboBox5";
            this.rfcComboBox5.Size = new System.Drawing.Size(75, 21);
            this.rfcComboBox5.TabIndex = 0;
            this.rfcComboBox5.TabStop = false;
            // 
            // FluidOunceButton
            // 
            this.FluidOunceButton.ForeColor = System.Drawing.Color.Green;
            this.FluidOunceButton.Location = new System.Drawing.Point(258, 71);
            this.FluidOunceButton.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.FluidOunceButton.Name = "FluidOunceButton";
            this.FluidOunceButton.Size = new System.Drawing.Size(75, 28);
            this.FluidOunceButton.TabIndex = 3;
            this.FluidOunceButton.TabStop = false;
            this.FluidOunceButton.Text = "fluid ounce";
            this.FluidOunceButton.UseVisualStyleBackColor = true;
            this.FluidOunceButton.Click += new System.EventHandler(this.FluidOunceButton_Click);
            this.FluidOunceButton.Enter += new System.EventHandler(this.Button_Enter);
            // 
            // TablespoonButton
            // 
            this.TablespoonButton.ForeColor = System.Drawing.Color.Green;
            this.TablespoonButton.Location = new System.Drawing.Point(258, 40);
            this.TablespoonButton.Margin = new System.Windows.Forms.Padding(9, 3, 0, 0);
            this.TablespoonButton.Name = "TablespoonButton";
            this.TablespoonButton.Size = new System.Drawing.Size(75, 28);
            this.TablespoonButton.TabIndex = 4;
            this.TablespoonButton.TabStop = false;
            this.TablespoonButton.Text = "tablespoon";
            this.TablespoonButton.UseVisualStyleBackColor = true;
            this.TablespoonButton.Click += new System.EventHandler(this.TablespoonButton_Click);
            this.TablespoonButton.Enter += new System.EventHandler(this.Button_Enter);
            // 
            // CupButton
            // 
            this.CupButton.ForeColor = System.Drawing.Color.Green;
            this.CupButton.Location = new System.Drawing.Point(258, 102);
            this.CupButton.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.CupButton.Name = "CupButton";
            this.CupButton.Size = new System.Drawing.Size(75, 28);
            this.CupButton.TabIndex = 5;
            this.CupButton.TabStop = false;
            this.CupButton.Text = "cup";
            this.CupButton.UseVisualStyleBackColor = true;
            this.CupButton.Click += new System.EventHandler(this.CupButton_Click);
            this.CupButton.Enter += new System.EventHandler(this.Button_Enter);
            // 
            // PintButton
            // 
            this.PintButton.ForeColor = System.Drawing.Color.Green;
            this.PintButton.Location = new System.Drawing.Point(258, 133);
            this.PintButton.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.PintButton.Name = "PintButton";
            this.PintButton.Size = new System.Drawing.Size(75, 28);
            this.PintButton.TabIndex = 6;
            this.PintButton.TabStop = false;
            this.PintButton.Text = "pint";
            this.PintButton.UseVisualStyleBackColor = true;
            this.PintButton.Click += new System.EventHandler(this.PintButton_Click);
            this.PintButton.Enter += new System.EventHandler(this.Button_Enter);
            // 
            // QuartButton
            // 
            this.QuartButton.ForeColor = System.Drawing.Color.Green;
            this.QuartButton.Location = new System.Drawing.Point(258, 164);
            this.QuartButton.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.QuartButton.Name = "QuartButton";
            this.QuartButton.Size = new System.Drawing.Size(75, 28);
            this.QuartButton.TabIndex = 7;
            this.QuartButton.TabStop = false;
            this.QuartButton.Text = "quart";
            this.QuartButton.UseVisualStyleBackColor = true;
            this.QuartButton.Click += new System.EventHandler(this.QuartButton_Click);
            this.QuartButton.Enter += new System.EventHandler(this.Button_Enter);
            // 
            // OunceButton
            // 
            this.OunceButton.ForeColor = System.Drawing.Color.Red;
            this.OunceButton.Location = new System.Drawing.Point(336, 102);
            this.OunceButton.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.OunceButton.Name = "OunceButton";
            this.OunceButton.Size = new System.Drawing.Size(75, 28);
            this.OunceButton.TabIndex = 8;
            this.OunceButton.TabStop = false;
            this.OunceButton.Text = "ounce";
            this.OunceButton.UseVisualStyleBackColor = true;
            this.OunceButton.Click += new System.EventHandler(this.OunceButton_Click);
            this.OunceButton.Enter += new System.EventHandler(this.Button_Enter);
            // 
            // PoundButton
            // 
            this.PoundButton.ForeColor = System.Drawing.Color.Red;
            this.PoundButton.Location = new System.Drawing.Point(336, 133);
            this.PoundButton.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.PoundButton.Name = "PoundButton";
            this.PoundButton.Size = new System.Drawing.Size(75, 28);
            this.PoundButton.TabIndex = 9;
            this.PoundButton.TabStop = false;
            this.PoundButton.Text = "pound";
            this.PoundButton.UseVisualStyleBackColor = true;
            this.PoundButton.Click += new System.EventHandler(this.PoundButton_Click);
            this.PoundButton.Enter += new System.EventHandler(this.Button_Enter);
            // 
            // TeaspoonButton
            // 
            this.TeaspoonButton.ForeColor = System.Drawing.Color.Green;
            this.TeaspoonButton.Location = new System.Drawing.Point(258, 9);
            this.TeaspoonButton.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.TeaspoonButton.Name = "TeaspoonButton";
            this.TeaspoonButton.Size = new System.Drawing.Size(75, 28);
            this.TeaspoonButton.TabIndex = 11;
            this.TeaspoonButton.TabStop = false;
            this.TeaspoonButton.Text = "teaspoon";
            this.TeaspoonButton.UseVisualStyleBackColor = true;
            this.TeaspoonButton.Click += new System.EventHandler(this.TeaspoonButton_Click);
            this.TeaspoonButton.Enter += new System.EventHandler(this.Button_Enter);
            // 
            // LiterButton
            // 
            this.LiterButton.ForeColor = System.Drawing.Color.Green;
            this.LiterButton.Location = new System.Drawing.Point(336, 71);
            this.LiterButton.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.LiterButton.Name = "LiterButton";
            this.LiterButton.Size = new System.Drawing.Size(75, 28);
            this.LiterButton.TabIndex = 13;
            this.LiterButton.TabStop = false;
            this.LiterButton.Text = "liter";
            this.LiterButton.UseVisualStyleBackColor = true;
            this.LiterButton.Click += new System.EventHandler(this.LiterButton_Click);
            this.LiterButton.Enter += new System.EventHandler(this.Button_Enter);
            // 
            // MilliliterButton
            // 
            this.MilliliterButton.ForeColor = System.Drawing.Color.Green;
            this.MilliliterButton.Location = new System.Drawing.Point(336, 40);
            this.MilliliterButton.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.MilliliterButton.Name = "MilliliterButton";
            this.MilliliterButton.Size = new System.Drawing.Size(75, 28);
            this.MilliliterButton.TabIndex = 14;
            this.MilliliterButton.TabStop = false;
            this.MilliliterButton.Text = "milliliter";
            this.MilliliterButton.UseVisualStyleBackColor = true;
            this.MilliliterButton.Click += new System.EventHandler(this.MilliliterButton_Click);
            this.MilliliterButton.Enter += new System.EventHandler(this.Button_Enter);
            // 
            // MilligramButton
            // 
            this.MilligramButton.ForeColor = System.Drawing.Color.Red;
            this.MilligramButton.Location = new System.Drawing.Point(336, 164);
            this.MilligramButton.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.MilligramButton.Name = "MilligramButton";
            this.MilligramButton.Size = new System.Drawing.Size(75, 28);
            this.MilligramButton.TabIndex = 15;
            this.MilligramButton.TabStop = false;
            this.MilligramButton.Text = "milligram";
            this.MilligramButton.UseVisualStyleBackColor = true;
            this.MilligramButton.Click += new System.EventHandler(this.MilligramButton_Click);
            this.MilligramButton.Enter += new System.EventHandler(this.Button_Enter);
            // 
            // GallonButton
            // 
            this.GallonButton.ForeColor = System.Drawing.Color.Green;
            this.GallonButton.Location = new System.Drawing.Point(336, 9);
            this.GallonButton.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.GallonButton.Name = "GallonButton";
            this.GallonButton.Size = new System.Drawing.Size(75, 28);
            this.GallonButton.TabIndex = 16;
            this.GallonButton.TabStop = false;
            this.GallonButton.Text = "gallon";
            this.GallonButton.UseVisualStyleBackColor = true;
            this.GallonButton.Click += new System.EventHandler(this.GallonButton_Click);
            this.GallonButton.Enter += new System.EventHandler(this.Button_Enter);
            // 
            // GramButton
            // 
            this.GramButton.ForeColor = System.Drawing.Color.Red;
            this.GramButton.Location = new System.Drawing.Point(414, 133);
            this.GramButton.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.GramButton.Name = "GramButton";
            this.GramButton.Size = new System.Drawing.Size(75, 28);
            this.GramButton.TabIndex = 18;
            this.GramButton.TabStop = false;
            this.GramButton.Text = "gram";
            this.GramButton.UseVisualStyleBackColor = true;
            this.GramButton.Click += new System.EventHandler(this.GramButton_Click);
            this.GramButton.Enter += new System.EventHandler(this.Button_Enter);
            // 
            // KilogramButton
            // 
            this.KilogramButton.ForeColor = System.Drawing.Color.Red;
            this.KilogramButton.Location = new System.Drawing.Point(414, 164);
            this.KilogramButton.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.KilogramButton.Name = "KilogramButton";
            this.KilogramButton.Size = new System.Drawing.Size(75, 28);
            this.KilogramButton.TabIndex = 17;
            this.KilogramButton.TabStop = false;
            this.KilogramButton.Text = "kilogram";
            this.KilogramButton.UseVisualStyleBackColor = true;
            this.KilogramButton.Click += new System.EventHandler(this.KilogramButton_Click);
            this.KilogramButton.Enter += new System.EventHandler(this.Button_Enter);
            // 
            // DegreeFButton
            // 
            this.DegreeFButton.ForeColor = System.Drawing.Color.Blue;
            this.DegreeFButton.Location = new System.Drawing.Point(414, 71);
            this.DegreeFButton.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.DegreeFButton.Name = "DegreeFButton";
            this.DegreeFButton.Size = new System.Drawing.Size(75, 28);
            this.DegreeFButton.TabIndex = 19;
            this.DegreeFButton.TabStop = false;
            this.DegreeFButton.Text = "degree F";
            this.DegreeFButton.UseVisualStyleBackColor = true;
            this.DegreeFButton.Click += new System.EventHandler(this.DegreeFButton_Click);
            this.DegreeFButton.Enter += new System.EventHandler(this.Button_Enter);
            // 
            // DegreeCButton
            // 
            this.DegreeCButton.ForeColor = System.Drawing.Color.Blue;
            this.DegreeCButton.Location = new System.Drawing.Point(414, 102);
            this.DegreeCButton.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.DegreeCButton.Name = "DegreeCButton";
            this.DegreeCButton.Size = new System.Drawing.Size(75, 28);
            this.DegreeCButton.TabIndex = 20;
            this.DegreeCButton.TabStop = false;
            this.DegreeCButton.Text = "degree C";
            this.DegreeCButton.UseVisualStyleBackColor = true;
            this.DegreeCButton.Click += new System.EventHandler(this.DegreeCButton_Click);
            this.DegreeCButton.Enter += new System.EventHandler(this.Button_Enter);
            // 
            // CalorieButton
            // 
            this.CalorieButton.ForeColor = System.Drawing.Color.Purple;
            this.CalorieButton.Location = new System.Drawing.Point(414, 9);
            this.CalorieButton.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.CalorieButton.Name = "CalorieButton";
            this.CalorieButton.Size = new System.Drawing.Size(75, 28);
            this.CalorieButton.TabIndex = 21;
            this.CalorieButton.TabStop = false;
            this.CalorieButton.Text = "Calorie";
            this.CalorieButton.UseVisualStyleBackColor = true;
            this.CalorieButton.Click += new System.EventHandler(this.CalorieButton_Click);
            this.CalorieButton.Enter += new System.EventHandler(this.Button_Enter);
            // 
            // KilojouleButton
            // 
            this.KilojouleButton.ForeColor = System.Drawing.Color.Purple;
            this.KilojouleButton.Location = new System.Drawing.Point(414, 40);
            this.KilojouleButton.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.KilojouleButton.Name = "KilojouleButton";
            this.KilojouleButton.Size = new System.Drawing.Size(75, 28);
            this.KilojouleButton.TabIndex = 22;
            this.KilojouleButton.TabStop = false;
            this.KilojouleButton.Text = "kilojoule";
            this.KilojouleButton.UseVisualStyleBackColor = true;
            this.KilojouleButton.Click += new System.EventHandler(this.KilojouleButton_Click);
            this.KilojouleButton.Enter += new System.EventHandler(this.Button_Enter);
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 201);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(498, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 23;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.ForeColor = System.Drawing.Color.White;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(436, 17);
            this.toolStripStatusLabel1.Spring = true;
            this.toolStripStatusLabel1.Text = "© 2018 Lolliesoft Magazine";
            this.toolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripStatusLabel2.Image")));
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(16, 17);
            this.toolStripStatusLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolStripStatusLabel2.Click += new System.EventHandler(this.toolStripStatusLabel2_Click);
            // 
            // appKitchenCalculatorDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(498, 223);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.KilojouleButton);
            this.Controls.Add(this.CalorieButton);
            this.Controls.Add(this.DegreeCButton);
            this.Controls.Add(this.DegreeFButton);
            this.Controls.Add(this.GramButton);
            this.Controls.Add(this.KilogramButton);
            this.Controls.Add(this.GallonButton);
            this.Controls.Add(this.MilligramButton);
            this.Controls.Add(this.MilliliterButton);
            this.Controls.Add(this.LiterButton);
            this.Controls.Add(this.TeaspoonButton);
            this.Controls.Add(this.PoundButton);
            this.Controls.Add(this.OunceButton);
            this.Controls.Add(this.QuartButton);
            this.Controls.Add(this.PintButton);
            this.Controls.Add(this.CupButton);
            this.Controls.Add(this.TablespoonButton);
            this.Controls.Add(this.FluidOunceButton);
            this.Controls.Add(this.MemoryLabel);
            this.Controls.Add(this.DisplayLabel);
            this.Controls.Add(this.ClearEntryButton);
            this.Controls.Add(this.ClearButton);
            this.Controls.Add(this.EqualsButton);
            this.Controls.Add(this.DivideButton);
            this.Controls.Add(this.MultiplyButton);
            this.Controls.Add(this.SubtractButton);
            this.Controls.Add(this.AddButton);
            this.Controls.Add(this.BackspaceButton);
            this.Controls.Add(this.NineButton);
            this.Controls.Add(this.EightButton);
            this.Controls.Add(this.SevenButton);
            this.Controls.Add(this.SixButton);
            this.Controls.Add(this.FiveButton);
            this.Controls.Add(this.FourButton);
            this.Controls.Add(this.ThreeButton);
            this.Controls.Add(this.PeriodButton);
            this.Controls.Add(this.TwoButton);
            this.Controls.Add(this.PlusMinusButton);
            this.Controls.Add(this.OneButton);
            this.Controls.Add(this.ZeroButton);
            this.Controls.Add(this.MemoryAddButton);
            this.Controls.Add(this.MemorySaveButton);
            this.Controls.Add(this.MemoryRecallButton);
            this.Controls.Add(this.MemoryClearButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "appKitchenCalculatorDialog";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bartender Express Calculator";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.appKitchenCalculatorDialog_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.appKitchenCalculatorDialog_KeyDown);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void KeepOnTopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ibKeepOnTop = !ibKeepOnTop;
                KeepOnTopToolStripMenuItem.Checked = ibKeepOnTop;
                base.TopMost = ibKeepOnTop;
                SaveUserOptions();
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void KilogramButton_Click(object sender, EventArgs e)
        {
            try
            {
                UnitButton(appUnit.Kilogram);
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void KilojouleButton_Click(object sender, EventArgs e)
        {
            try
            {
                UnitButton(appUnit.Kilojoule);
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void LiterButton_Click(object sender, EventArgs e)
        {
            try
            {
                UnitButton(appUnit.Liter);
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void LoadUserOptions()
        {
            try
            {
                string str3;
                string str4 = rfcDataService.NotNullString(Registry.GetValue(isUserKey, "Unit System", "US")).Trim().ToLower();
                if (str4 == null)
                {
                    goto Label_018E;
                }
                if (!(str4 == "uk"))
                {
                    if (str4 == "australia")
                    {
                        goto Label_00B9;
                    }
                    if (str4 == "newzealand")
                    {
                        goto Label_0101;
                    }
                    if (str4 == "canada")
                    {
                        goto Label_0149;
                    }
                    goto Label_018E;
                }
                ieUnitSystem = appUnitSystem.UK;
                UKToolStripMenuItem.Checked = true;
                AustraliaToolStripMenuItem.Checked = false;
                NewZealandToolStripMenuItem.Checked = false;
                CanadaToolStripMenuItem.Checked = false;
                USToolStripMenuItem.Checked = false;
                goto Label_01D1;
            Label_00B9:
                ieUnitSystem = appUnitSystem.Australia;
                UKToolStripMenuItem.Checked = false;
                AustraliaToolStripMenuItem.Checked = true;
                NewZealandToolStripMenuItem.Checked = false;
                CanadaToolStripMenuItem.Checked = false;
                USToolStripMenuItem.Checked = false;
                goto Label_01D1;
            Label_0101:
                ieUnitSystem = appUnitSystem.NewZealand;
                UKToolStripMenuItem.Checked = false;
                AustraliaToolStripMenuItem.Checked = false;
                NewZealandToolStripMenuItem.Checked = true;
                CanadaToolStripMenuItem.Checked = false;
                USToolStripMenuItem.Checked = false;
                goto Label_01D1;
            Label_0149:
                ieUnitSystem = appUnitSystem.Canada;
                UKToolStripMenuItem.Checked = false;
                AustraliaToolStripMenuItem.Checked = false;
                NewZealandToolStripMenuItem.Checked = false;
                CanadaToolStripMenuItem.Checked = true;
                USToolStripMenuItem.Checked = false;
                goto Label_01D1;
            Label_018E:
                ieUnitSystem = appUnitSystem.US;
                UKToolStripMenuItem.Checked = false;
                AustraliaToolStripMenuItem.Checked = false;
                NewZealandToolStripMenuItem.Checked = false;
                CanadaToolStripMenuItem.Checked = false;
                USToolStripMenuItem.Checked = true;
            Label_01D1:
                str3 = rfcDataService.NotNullString(Registry.GetValue(isUserKey, "Keep On Top", "No"));
                ibKeepOnTop = rfcDataService.NotNullBoolean(str3);
                KeepOnTopToolStripMenuItem.Checked = ibKeepOnTop;
                string aoValue = rfcDataService.NotNullString(Registry.GetValue(isUserKey, "Abbreviate Units", "No"));
                ibAbbreviateUnits = rfcDataService.NotNullBoolean(aoValue);
                AbbreviateUnitsToolStripMenuItem.Checked = ibAbbreviateUnits;
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void MathError(string asErrorMessage)
        {
            try
            {
                DisplayLabel.Text = asErrorMessage;
                ibError = true;
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void MemoryAdd()
        {
            decimal num2 = 0M;
            appUnit none = appUnit.None;
            try
            {
                if ((idcMemoryValue == 0M) && (ieMemoryUnit == appUnit.None))
                {
                    MemorySave();
                }
                else
                {
                    decimal idcCurrentEntryValue;
                    appUnit ieCurrentEntryUnit;
                    if (ieDisplayMode == appDisplayMode.DataEntry)
                    {
                        idcCurrentEntryValue = this.idcCurrentEntryValue;
                        ieCurrentEntryUnit = this.ieCurrentEntryUnit;
                    }
                    else
                    {
                        idcCurrentEntryValue = idcSessionValue;
                        ieCurrentEntryUnit = ieSessionUnit;
                    }
                    switch (GetUnitMode(ieMemoryUnit))
                    {
                        case appUnitMode.None:
                            num2 = idcCurrentEntryValue + idcMemoryValue;
                            none = ieMemoryUnit;
                            break;

                        case appUnitMode.Volume:
                            num2 = AddVolumes(idcCurrentEntryValue, ieCurrentEntryUnit, idcMemoryValue, ieMemoryUnit, out none);
                            break;

                        case appUnitMode.Weight:
                            num2 = AddWeights(idcCurrentEntryValue, ieCurrentEntryUnit, idcMemoryValue, ieMemoryUnit, out none);
                            break;

                        case appUnitMode.Temperature:
                            num2 = AddTemperatures(idcCurrentEntryValue, ieCurrentEntryUnit, idcMemoryValue, ieMemoryUnit, out none);
                            break;

                        case appUnitMode.Energy:
                            num2 = AddEnergies(idcCurrentEntryValue, ieCurrentEntryUnit, idcMemoryValue, ieMemoryUnit, out none);
                            break;
                    }
                    idcMemoryValue = num2;
                    ieMemoryUnit = none;
                }
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void MemoryAddButton_Click(object sender, EventArgs e)
        {
            try
            {
                MemoryAdd();
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void MemoryClear()
        {
            try
            {
                idcMemoryValue = 0M;
                ieMemoryUnit = appUnit.None;
                MemoryLabel.Text = "";
                MemoryRecallButton.Enabled = true;
                MemoryAddButton.Enabled = true;
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void MemoryClearButton_Click(object sender, EventArgs e)
        {
            try
            {
                MemoryClear();
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void MemoryRecall()
        {
            try
            {
                if (ieDisplayMode == appDisplayMode.Total)
                {
                    idcSessionValue = 0M;
                    ieSessionUnit = appUnit.None;
                    ieSessionOperator = appOperator.None;
                }
                if (ieDisplayMode != appDisplayMode.DataEntry)
                {
                    ieCurrentOperator = appOperator.None;
                    ieDisplayMode = appDisplayMode.DataEntry;
                }
                isCurrentEntry = rfcDataService.FormatDecimal(idcMemoryValue, 5);
                ieCurrentEntryUnit = ieMemoryUnit;
                ieDisplayMode = appDisplayMode.DataEntry;
                EnableControls();
                RefreshDisplay();
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void MemoryRecallButton_Click(object sender, EventArgs e)
        {
            try
            {
                MemoryRecall();
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void MemorySave()
        {
            try
            {
                if (((ieDisplayMode == appDisplayMode.DataEntry) || (ieDisplayMode == appDisplayMode.Total)) && (idcCurrentEntryValue != 0M))
                {
                    idcMemoryValue = idcCurrentEntryValue;
                    ieMemoryUnit = ieCurrentEntryUnit;
                    MemoryLabel.Text = "M";
                }
                else if (idcSessionValue != 0M)
                {
                    idcMemoryValue = idcSessionValue;
                    ieMemoryUnit = ieSessionUnit;
                    MemoryLabel.Text = "M";
                }
                EnableControls();
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void MemorySaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                MemorySave();
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void MilligramButton_Click(object sender, EventArgs e)
        {
            try
            {
                UnitButton(appUnit.Milligram);
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void MilliliterButton_Click(object sender, EventArgs e)
        {
            try
            {
                UnitButton(appUnit.Milliliter);
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void Multiply()
        {
            try
            {
                if (((!ibError && (ieCurrentOperator == appOperator.None)) && ((idcCurrentEntryValue != 0M) || (ieDisplayMode == appDisplayMode.Total))) && (((ieSessionOperator != appOperator.Add) && (ieSessionOperator != appOperator.Subtract)) || ((ieSessionUnitMode == appUnitMode.None) || (ieCurrentEntryUnit != appUnit.None))))
                {
                    if (ieSessionUnitMode == appUnitMode.None)
                    {
                        SetSessionUnitMode(GetUnitMode(ieCurrentEntryUnit));
                    }
                    ProcessSessionOperation();
                    ieCurrentOperator = appOperator.Multiply;
                    ieSessionOperator = ieCurrentOperator;
                    EnableControls();
                    RefreshDisplay();
                }
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void MultiplyButton_Click(object sender, EventArgs e)
        {
            try
            {
                Multiply();
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private decimal MultiplyEnergies(decimal adcFirstValue, appUnit aeFirstUnit, decimal adcSecondValue, appUnit aeSecondUnit, out appUnit aeResultUnit)
        {
            decimal num = 0M;
            appUnit none = appUnit.None;
            try
            {
                if (aeFirstUnit == appUnit.None)
                {
                    num = adcFirstValue * adcSecondValue;
                    none = aeSecondUnit;
                }
                else
                {
                    num = adcFirstValue * adcSecondValue;
                    none = aeFirstUnit;
                }
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
            aeResultUnit = none;
            return num;
        }

        private decimal MultiplyVolumes(decimal adcFirstValue, appUnit aeFirstUnit, decimal adcSecondValue, appUnit aeSecondUnit, out appUnit aeResultUnit)
        {
            decimal num = 0M;
            appUnit none = appUnit.None;
            try
            {
                if (aeFirstUnit == appUnit.None)
                {
                    num = adcFirstValue * adcSecondValue;
                    none = aeSecondUnit;
                }
                else
                {
                    num = adcFirstValue * adcSecondValue;
                    none = aeFirstUnit;
                }
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
            aeResultUnit = none;
            return num;
        }

        private decimal MultiplyWeights(decimal adcFirstValue, appUnit aeFirstUnit, decimal adcSecondValue, appUnit aeSecondUnit, out appUnit aeResultUnit)
        {
            decimal num = 0M;
            appUnit none = appUnit.None;
            try
            {
                if (aeFirstUnit == appUnit.None)
                {
                    num = adcFirstValue * adcSecondValue;
                    none = aeSecondUnit;
                }
                else
                {
                    num = adcFirstValue * adcSecondValue;
                    none = aeFirstUnit;
                }
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
            aeResultUnit = none;
            return num;
        }

        private void NineButton_Click(object sender, EventArgs e)
        {
            try
            {
                DigitButton(9);
            }
            catch (Exception exception)
            {
                HandleTerminalError(exception);
            }
        }

        private void OneButton_Click(object sender, EventArgs e)
        {
            try
            {
                DigitButton(1);
            }
            catch (Exception exception)
            {
                HandleTerminalError(exception);
            }
        }

        private void OneQuarterButton_Click(object sender, EventArgs e)
        {
            try
            {
                FractionButton("\x00bc");
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void OunceButton_Click(object sender, EventArgs e)
        {
            try
            {
                UnitButton(appUnit.Ounce);
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void Paste()
        {
            bool flag = false;
            try
            {
                string str;
                if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text))
                {
                    str = Clipboard.GetDataObject().GetData(DataFormats.Text).ToString();
                }
                else
                {
                    return;
                }
                if (str.Trim() != "")
                {
                    rfcDataService.ParseNumbersAndText(str, out decimal num, out string str2);
                    appUnit aeUnit = GetUnit(str2);
                    if (ieDisplayMode == appDisplayMode.Total)
                    {
                        flag = true;
                    }
                    else if (ieSessionOperator == appOperator.None)
                    {
                        if ((GetUnitMode(ieCurrentEntryUnit) != GetUnitMode(aeUnit)) && (ieCurrentEntryUnit != appUnit.None))
                        {
                            flag = false;
                        }
                        else
                        {
                            flag = true;
                        }
                    }
                    else if ((ieSessionOperator == appOperator.Add) || (ieSessionOperator == appOperator.Subtract))
                    {
                        if (ieSessionUnitMode != GetUnitMode(aeUnit))
                        {
                            flag = false;
                        }
                        else
                        {
                            flag = true;
                        }
                    }
                    else if ((ieSessionOperator == appOperator.Multiply) || (ieSessionOperator == appOperator.Divide))
                    {
                        if ((ieSessionUnitMode == appUnitMode.None) && (GetUnitMode(aeUnit) == appUnitMode.None))
                        {
                            flag = false;
                        }
                        else if ((ieSessionUnitMode != appUnitMode.None) && (GetUnitMode(aeUnit) != appUnitMode.None))
                        {
                            flag = false;
                        }
                        else
                        {
                            flag = true;
                        }
                    }
                    if (!flag)
                    {
                        Clear();
                    }
                    if (ieDisplayMode == appDisplayMode.Total)
                    {
                        idcSessionValue = 0M;
                        ieSessionUnit = appUnit.None;
                        ieSessionOperator = appOperator.None;
                    }
                    if (ieDisplayMode != appDisplayMode.DataEntry)
                    {
                        ieCurrentOperator = appOperator.None;
                        ieDisplayMode = appDisplayMode.DataEntry;
                    }
                    isCurrentEntry = rfcDataService.FormatDecimal(num, 5);
                    ieCurrentEntryUnit = aeUnit;
                    ieDisplayMode = appDisplayMode.DataEntry;
                    EnableControls();
                    RefreshDisplay();
                }
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Paste();
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void PeriodButton_Click(object sender, EventArgs e)
        {
            try
            {
                AddDecimal();
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void PintButton_Click(object sender, EventArgs e)
        {
            try
            {
                UnitButton(appUnit.Pint);
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void PlusMinus()
        {
            try
            {
                if ((ieDisplayMode == appDisplayMode.DataEntry) || (ieDisplayMode == appDisplayMode.Total))
                {
                    if (idcCurrentEntryValue == 0M)
                    {
                        return;
                    }
                    if (isCurrentEntry.StartsWith("-"))
                    {
                        isCurrentEntry = isCurrentEntry.Substring(1);
                    }
                    else
                    {
                        isCurrentEntry = "-" + isCurrentEntry;
                    }
                }
                else
                {
                    idcSessionValue *= -1M;
                }
                RefreshDisplay();
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void PlusMinusButton_Click(object sender, EventArgs e)
        {
            try
            {
                PlusMinus();
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void PoundButton_Click(object sender, EventArgs e)
        {
            try
            {
                UnitButton(appUnit.Pound);
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void ProcessSessionOperation()
        {
            decimal idcCurrentEntryValue = 0M;
            appUnit none = appUnit.None;
            try
            {
                switch (ieSessionOperator)
                {
                    case appOperator.None:
                        idcCurrentEntryValue = this.idcCurrentEntryValue;
                        none = ieCurrentEntryUnit;
                        goto Label_035A;

                    case appOperator.Add:
                        switch (ieSessionUnitMode)
                        {
                            case appUnitMode.Volume:
                                goto Label_0083;

                            case appUnitMode.Weight:
                                goto Label_00A9;

                            case appUnitMode.Temperature:
                                goto Label_00CF;

                            case appUnitMode.Energy:
                                goto Label_00F5;
                        }
                        goto Label_035A;

                    case appOperator.Subtract:
                        switch (ieSessionUnitMode)
                        {
                            case appUnitMode.None:
                                goto Label_0143;

                            case appUnitMode.Volume:
                                goto Label_015C;

                            case appUnitMode.Weight:
                                goto Label_0182;

                            case appUnitMode.Temperature:
                                goto Label_01A8;

                            case appUnitMode.Energy:
                                goto Label_01CE;
                        }
                        goto Label_035A;

                    case appOperator.Multiply:
                        switch (ieSessionUnitMode)
                        {
                            case appUnitMode.None:
                                goto Label_021C;

                            case appUnitMode.Volume:
                                goto Label_0235;

                            case appUnitMode.Weight:
                                goto Label_025B;

                            case appUnitMode.Energy:
                                goto Label_0281;
                        }
                        goto Label_035A;

                    case appOperator.Divide:
                        if (!(idcSessionValue == 0M))
                        {
                            goto Label_02CA;
                        }
                        MathError("Cannot divide by zero.");
                        return;

                    default:
                        goto Label_035A;
                }
                idcCurrentEntryValue = idcSessionValue + this.idcCurrentEntryValue;
                none = appUnit.None;
                goto Label_035A;
            Label_0083:
                idcCurrentEntryValue = AddVolumes(idcSessionValue, ieSessionUnit, this.idcCurrentEntryValue, ieCurrentEntryUnit, out none);
                goto Label_035A;
            Label_00A9:
                idcCurrentEntryValue = AddWeights(idcSessionValue, ieSessionUnit, this.idcCurrentEntryValue, ieCurrentEntryUnit, out none);
                goto Label_035A;
            Label_00CF:
                idcCurrentEntryValue = AddTemperatures(idcSessionValue, ieSessionUnit, this.idcCurrentEntryValue, ieCurrentEntryUnit, out none);
                goto Label_035A;
            Label_00F5:
                idcCurrentEntryValue = AddEnergies(idcSessionValue, ieSessionUnit, this.idcCurrentEntryValue, ieCurrentEntryUnit, out none);
                goto Label_035A;
            Label_0143:
                idcCurrentEntryValue = idcSessionValue - this.idcCurrentEntryValue;
                none = appUnit.None;
                goto Label_035A;
            Label_015C:
                idcCurrentEntryValue = SubtractVolumes(idcSessionValue, ieSessionUnit, this.idcCurrentEntryValue, ieCurrentEntryUnit, out none);
                goto Label_035A;
            Label_0182:
                idcCurrentEntryValue = SubtractWeights(idcSessionValue, ieSessionUnit, this.idcCurrentEntryValue, ieCurrentEntryUnit, out none);
                goto Label_035A;
            Label_01A8:
                idcCurrentEntryValue = SubtractTemperatures(idcSessionValue, ieSessionUnit, this.idcCurrentEntryValue, ieCurrentEntryUnit, out none);
                goto Label_035A;
            Label_01CE:
                idcCurrentEntryValue = SubtractEnergies(idcSessionValue, ieSessionUnit, this.idcCurrentEntryValue, ieCurrentEntryUnit, out none);
                goto Label_035A;
            Label_021C:
                idcCurrentEntryValue = idcSessionValue * this.idcCurrentEntryValue;
                none = appUnit.None;
                goto Label_035A;
            Label_0235:
                idcCurrentEntryValue = MultiplyVolumes(idcSessionValue, ieSessionUnit, this.idcCurrentEntryValue, ieCurrentEntryUnit, out none);
                goto Label_035A;
            Label_025B:
                idcCurrentEntryValue = MultiplyWeights(idcSessionValue, ieSessionUnit, this.idcCurrentEntryValue, ieCurrentEntryUnit, out none);
                goto Label_035A;
            Label_0281:
                idcCurrentEntryValue = MultiplyEnergies(idcSessionValue, ieSessionUnit, this.idcCurrentEntryValue, ieCurrentEntryUnit, out none);
                goto Label_035A;
            Label_02CA:
                switch (ieSessionUnitMode)
                {
                    case appUnitMode.None:
                        idcCurrentEntryValue = idcSessionValue / this.idcCurrentEntryValue;
                        none = appUnit.None;
                        break;

                    case appUnitMode.Volume:
                        idcCurrentEntryValue = DivideVolumes(idcSessionValue, ieSessionUnit, this.idcCurrentEntryValue, out none);
                        break;

                    case appUnitMode.Weight:
                        idcCurrentEntryValue = DivideWeights(idcSessionValue, ieSessionUnit, this.idcCurrentEntryValue, out none);
                        break;

                    case appUnitMode.Energy:
                        idcCurrentEntryValue = DivideEnergies(idcSessionValue, ieSessionUnit, this.idcCurrentEntryValue, out none);
                        break;
                }
            Label_035A:
                idcSessionValue = idcCurrentEntryValue;
                ieSessionUnit = none;
                isCurrentEntry = "";
                ieCurrentEntryUnit = appUnit.None;
                ieDisplayMode = appDisplayMode.SessionData;
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void QuartButton_Click(object sender, EventArgs e)
        {
            try
            {
                UnitButton(appUnit.Quart);
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void RefreshDisplay()
        {
            try
            {
                string isCurrentEntry;
                if ((ieDisplayMode == appDisplayMode.DataEntry) || (ieDisplayMode == appDisplayMode.Total))
                {
                    if ((this.isCurrentEntry == "") && ((ieCurrentEntryUnit == appUnit.DegreeC) || (ieCurrentEntryUnit == appUnit.DegreeF)))
                    {
                        isCurrentEntry = "0";
                    }
                    else if (((this.isCurrentEntry == "") || (this.isCurrentEntry == ".")) || (this.isCurrentEntry == "-"))
                    {
                        isCurrentEntry = "0";
                    }
                    else if (this.isCurrentEntry.StartsWith("."))
                    {
                        isCurrentEntry = "0" + this.isCurrentEntry;
                    }
                    else
                    {
                        isCurrentEntry = this.isCurrentEntry;
                    }
                    if (ieCurrentEntryUnit != appUnit.None)
                    {
                        isCurrentEntry = isCurrentEntry + " " + GetUnitText(ieCurrentEntryUnit, idcCurrentEntryValue);
                    }
                }
                else
                {
                    isCurrentEntry = rfcDataService.FormatDecimal(idcSessionValue, 5);
                    if (ieSessionUnit != appUnit.None)
                    {
                        isCurrentEntry = isCurrentEntry + " " + GetUnitText(ieSessionUnit, idcSessionValue);
                    }
                }
                DisplayLabel.Text = isCurrentEntry;
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void SaveUserOptions()
        {
            try
            {
                Registry.SetValue(isUserKey, "Unit System", ieUnitSystem.ToString());
                Registry.SetValue(isUserKey, "Keep On Top", rfcDataService.ToYesNo(ibKeepOnTop));
                Registry.SetValue(isUserKey, "Abbreviate Units", rfcDataService.ToYesNo(ibAbbreviateUnits));
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void SetSessionUnitMode(appUnitMode aeUnitMode)
        {
            try
            {
                ieSessionUnitMode = aeUnitMode;
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void SevenButton_Click(object sender, EventArgs e)
        {
            try
            {
                DigitButton(7);
            }
            catch (Exception exception)
            {
                HandleTerminalError(exception);
            }
        }

        private void SixButton_Click(object sender, EventArgs e)
        {
            try
            {
                DigitButton(6);
            }
            catch (Exception exception)
            {
                HandleTerminalError(exception);
            }
        }

        private void Subtract()
        {
            try
            {
                if (!ibError && (ieCurrentOperator == appOperator.None))
                {
                    if (idcCurrentEntryValue == 0M)
                    {
                        if (isCurrentEntry == "")
                        {
                            isCurrentEntry = "-";
                        }
                    }
                    else if (((ieSessionOperator != appOperator.Add) && (ieSessionOperator != appOperator.Subtract)) || ((ieSessionUnitMode == appUnitMode.None) || (ieCurrentEntryUnit != appUnit.None)))
                    {
                        if (ieSessionUnitMode == appUnitMode.None)
                        {
                            SetSessionUnitMode(GetUnitMode(ieCurrentEntryUnit));
                        }
                        ProcessSessionOperation();
                        ieCurrentOperator = appOperator.Subtract;
                        ieSessionOperator = ieCurrentOperator;
                        EnableControls();
                        RefreshDisplay();
                    }
                }
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void SubtractButton_Click(object sender, EventArgs e)
        {
            try
            {
                Subtract();
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private decimal SubtractEnergies(decimal adcFirstValue, appUnit aeFirstUnit, decimal adcSecondValue, appUnit aeSecondUnit, out appUnit aeResultUnit)
        {
            decimal adcKiloJoules = 0M;
            try
            {
                decimal num2 = ConvertToKiloJoules(adcFirstValue, aeFirstUnit);
                decimal num3 = ConvertToKiloJoules(adcSecondValue, aeSecondUnit);
                adcKiloJoules = num2 - num3;
                adcKiloJoules = ConvertFromKiloJoules(adcKiloJoules, aeFirstUnit);
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
            aeResultUnit = aeFirstUnit;
            return adcKiloJoules;
        }

        private decimal SubtractTemperatures(decimal adcFirstValue, appUnit aeFirstUnit, decimal adcSecondValue, appUnit aeSecondUnit, out appUnit aeResultUnit)
        {
            decimal num = 0M;
            try
            {
                num = adcFirstValue - adcSecondValue;
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
            aeResultUnit = aeFirstUnit;
            return num;
        }

        private decimal SubtractVolumes(decimal adcFirstValue, appUnit aeFirstUnit, decimal adcSecondValue, appUnit aeSecondUnit, out appUnit aeResultUnit)
        {
            decimal adcLiters = 0M;
            try
            {
                decimal num2 = ConvertToLiters(adcFirstValue, aeFirstUnit);
                decimal num3 = ConvertToLiters(adcSecondValue, aeSecondUnit);
                adcLiters = num2 - num3;
                adcLiters = ConvertFromLiters(adcLiters, aeFirstUnit);
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
            aeResultUnit = aeFirstUnit;
            return adcLiters;
        }

        private decimal SubtractWeights(decimal adcFirstValue, appUnit aeFirstUnit, decimal adcSecondValue, appUnit aeSecondUnit, out appUnit aeResultUnit)
        {
            decimal adcGrams = 0M;
            try
            {
                decimal num2 = ConvertToGrams(adcFirstValue, aeFirstUnit);
                decimal num3 = ConvertToGrams(adcSecondValue, aeSecondUnit);
                adcGrams = num2 - num3;
                adcGrams = ConvertFromGrams(adcGrams, aeFirstUnit);
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
            aeResultUnit = aeFirstUnit;
            return adcGrams;
        }

        private void TablespoonButton_Click(object sender, EventArgs e)
        {
            try
            {
                UnitButton(appUnit.Tablespoon);
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void TeaspoonButton_Click(object sender, EventArgs e)
        {
            try
            {
                UnitButton(appUnit.Teaspoon);
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void ThreeButton_Click(object sender, EventArgs e)
        {
            try
            {
                DigitButton(3);
            }
            catch (Exception exception)
            {
                HandleTerminalError(exception);
            }
        }

        private void ThreeQuarterButton_Click(object sender, EventArgs e)
        {
            try
            {
                FractionButton("\x00be");
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void Total()
        {
            try
            {
                if (((!ibError && (ieCurrentOperator == appOperator.None)) && (idcCurrentEntryValue != 0M)) && (((ieSessionOperator != appOperator.Add) && (ieSessionOperator != appOperator.Subtract)) || ((ieSessionUnitMode == appUnitMode.None) || (ieCurrentEntryUnit != appUnit.None))))
                {
                    if (ieSessionUnitMode == appUnitMode.None)
                    {
                        SetSessionUnitMode(GetUnitMode(ieCurrentEntryUnit));
                    }
                    ProcessSessionOperation();
                    ieCurrentOperator = appOperator.None;
                    ieDisplayMode = appDisplayMode.Total;
                    isCurrentEntry = rfcDataService.FormatDecimal(idcSessionValue, 5);
                    ieCurrentEntryUnit = ieSessionUnit;
                    idcSessionValue = 0M;
                    ieSessionUnit = appUnit.None;
                    SetSessionUnitMode(GetUnitMode(ieCurrentEntryUnit));
                    ieSessionOperator = ieCurrentOperator;
                    EnableControls();
                    RefreshDisplay();
                }
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void TwoButton_Click(object sender, EventArgs e)
        {
            try
            {
                DigitButton(2);
            }
            catch (Exception exception)
            {
                HandleTerminalError(exception);
            }
        }

        private void UnitButton(appUnit aeUnit)
        {
            try
            {
                if (!ibError && (((isCurrentEntry != "") || (aeUnit == appUnit.DegreeC)) || (aeUnit == appUnit.DegreeF)))
                {
                    if (ieCurrentEntryUnit != appUnit.None)
                    {
                        appUnitMode unitMode = GetUnitMode(ieCurrentEntryUnit);
                        appUnitMode mode2 = GetUnitMode(aeUnit);
                        if (unitMode != mode2)
                        {
                            return;
                        }
                        ConvertUnit(aeUnit);
                    }
                    else
                    {
                        ieCurrentEntryUnit = aeUnit;
                    }
                    ieCurrentOperator = appOperator.None;
                    EnableControls();
                    RefreshDisplay();
                }
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void UnitSystemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            decimal adcLiters = 0M;
            try
            {
                decimal idcCurrentEntryValue;
                appUnit ieCurrentEntryUnit;
                ToolStripMenuItem item = (ToolStripMenuItem)sender;
                USToolStripMenuItem.Checked = false;
                CanadaToolStripMenuItem.Checked = false;
                UKToolStripMenuItem.Checked = false;
                AustraliaToolStripMenuItem.Checked = false;
                NewZealandToolStripMenuItem.Checked = false;
                if ((ieDisplayMode == appDisplayMode.DataEntry) || (ieDisplayMode == appDisplayMode.Total))
                {
                    idcCurrentEntryValue = this.idcCurrentEntryValue;
                    ieCurrentEntryUnit = this.ieCurrentEntryUnit;
                }
                else
                {
                    idcCurrentEntryValue = idcSessionValue;
                    ieCurrentEntryUnit = ieSessionUnit;
                }
                if (GetUnitMode(ieCurrentEntryUnit) == appUnitMode.Volume)
                {
                    adcLiters = ConvertToLiters(idcCurrentEntryValue, ieCurrentEntryUnit);
                }
                string str = item.Text.ToLower();
                if (str != null)
                {
                    if (!(str == "us"))
                    {
                        if (str == "canada")
                        {
                            goto Label_00FC;
                        }
                        if (str == "new zealand")
                        {
                            goto Label_0111;
                        }
                        if (str == "uk")
                        {
                            goto Label_0126;
                        }
                        if (str == "australia")
                        {
                            goto Label_013B;
                        }
                    }
                    else
                    {
                        ieUnitSystem = appUnitSystem.US;
                        USToolStripMenuItem.Checked = true;
                    }
                }
                goto Label_014E;
            Label_00FC:
                ieUnitSystem = appUnitSystem.Canada;
                CanadaToolStripMenuItem.Checked = true;
                goto Label_014E;
            Label_0111:
                ieUnitSystem = appUnitSystem.NewZealand;
                NewZealandToolStripMenuItem.Checked = true;
                goto Label_014E;
            Label_0126:
                ieUnitSystem = appUnitSystem.UK;
                UKToolStripMenuItem.Checked = true;
                goto Label_014E;
            Label_013B:
                ieUnitSystem = appUnitSystem.Australia;
                AustraliaToolStripMenuItem.Checked = true;
            Label_014E:
                if (GetUnitMode(ieCurrentEntryUnit) == appUnitMode.Volume)
                {
                    Clear();
                    idcCurrentEntryValue = ConvertFromLiters(adcLiters, ieCurrentEntryUnit);
                    if (ieDisplayMode == appDisplayMode.Total)
                    {
                        idcSessionValue = 0M;
                        ieSessionUnit = appUnit.None;
                        ieSessionOperator = appOperator.None;
                    }
                    if (ieDisplayMode != appDisplayMode.DataEntry)
                    {
                        ieCurrentOperator = appOperator.None;
                        ieDisplayMode = appDisplayMode.DataEntry;
                    }
                    isCurrentEntry = rfcDataService.FormatDecimal(idcCurrentEntryValue, 5);
                    this.ieCurrentEntryUnit = ieCurrentEntryUnit;
                    ieDisplayMode = appDisplayMode.DataEntry;
                    EnableControls();
                    RefreshDisplay();
                }
                SaveUserOptions();
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private void ZeroButton_Click(object sender, EventArgs e)
        {
            try
            {
                DigitButton(0);
            }
            catch (Exception exception)
            {
                HandleSystemError(exception);
            }
        }

        private decimal idcCurrentEntryValue => GetCurrentEntryDecimalValue();

        private enum appDisplayMode
        {
            DataEntry,
            SessionData,
            Total
        }

        private enum appOperator
        {
            None,
            Add,
            Subtract,
            Multiply,
            Divide
        }

        private enum appUnit
        {
            None,
            Teaspoon,
            Tablespoon,
            FluidOunce,
            Cup,
            Pint,
            Quart,
            Gallon,
            Milliliter,
            Liter,
            Ounce,
            Pound,
            Milligram,
            Gram,
            Kilogram,
            DegreeF,
            DegreeC,
            Calorie,
            Kilojoule
        }

        private enum appUnitMode
        {
            None,
            Volume,
            Weight,
            Temperature,
            Energy
        }

        private enum appUnitSystem
        {
            US,
            UK,
            Australia,
            NewZealand,
            Canada
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            appAboutDialog About = new appAboutDialog();
            About.ShowDialog();
        }

        private void toolStripStatusLabel2_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.lolliesoft.com");
        }
    }
}

