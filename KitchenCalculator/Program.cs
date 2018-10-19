namespace KitchenCalculator
{
    using System;
    using System.Windows.Forms;

    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            appKitchenCalculatorDialog mainForm = new appKitchenCalculatorDialog();
            mainForm.Initialize();
            Application.Run(mainForm);
        }
    }
}

