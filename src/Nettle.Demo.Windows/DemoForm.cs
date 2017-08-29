namespace Nettle.Demo.Windows
{
    using Nettle.Compiler;
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public partial class DemoForm : Form
    {
        private INettleCompiler _compiler;

        public DemoForm()
        {
            InitializeComponent();

            _compiler = NettleEngine.GetCompiler();

            var partialContent = @"Partial Content: {{$}}";

            _compiler.RegisterTemplate
            (
                "PartialSample",
                partialContent
            );
        }

        private void renderButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            try
            {
                var template = _compiler.Compile
                (
                    templateTextBox.Text
                );

                var model = new
                {
                    Message = "Hello World",
                    Names = new string[]
                    {
                        "Craig",
                        "John",
                        "Simon"
                    }
                };

                var output = template(model);

                outputTextBox.ForeColor = Color.White;
                outputTextBox.Text = output;
            }
            catch (Exception ex)
            {
                outputTextBox.ForeColor = Color.Red;
                outputTextBox.Text = ex.Message;
            }
        }
    }
}
