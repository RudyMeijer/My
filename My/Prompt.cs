using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


public static class Prompt
{
    // Methods
    public static string ShowDialog(string text, string caption)
    {
        Form prompt = new Form();
        prompt.Width = 200;
        prompt.Height = 150;
        prompt.Text = caption;
        Label label1 = new Label();
        label1.Left = 10;
        label1.Top = 20;
        label1.Text = text;
        label1.AutoSize = true;
        Label label = label1;
        TextBox box1 = new TextBox();
        box1.Left = 10;
        box1.Top = 50;
        box1.Width = prompt.ClientSize.Width - 60;
        TextBox textBox = box1;
        Button button1 = new Button();
        button1.Text = "Ok";
        button1.Left = textBox.Right + 5;
        button1.Width = 30;
        button1.Top = textBox.Top;
        Button Ok = button1;
        Ok.Click += delegate (object sender, EventArgs e)
        {
            prompt.Close();
        };
        if (label.Text.ToUpper().Contains("PASSWORD"))
        {
            textBox.PasswordChar = '*';
        }
        prompt.Controls.Add(Ok);
        prompt.Controls.Add(label);
        prompt.Controls.Add(textBox);
        prompt.AcceptButton = Ok;
        prompt.ShowDialog();
        return textBox.Text;
    }
}

